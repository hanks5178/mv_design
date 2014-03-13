using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using Eplan.EplApi.Base;
using Eplan.EplApi.DataModel;
using Eplan.EplApi.DataModel.MasterData;
using Eplan.EplApi.HEServices;
using Eplan.EplApi.MasterData;
using Eplan.EplApi.DataModel.Graphics;

namespace Eplan.EplAddin.MvDesign.UI.UC
{
	public partial class PartArrange : UserControl
	{
		public Data.MvProject MvProject { get; set; }

		private WindowMacro _singleLineMacro { get; set; }
		private string _singleLineFolder = string.Empty;
		private double _zoomFactor = 1.0;
		private DrawingService _feederView = null;
		private LocationBox _singleline;
		private Page _threeline;
		private PointD _origin = new PointD(0, 0);
		private Point _lastMouseLocation;
		private List<Data.Symbol> SelectedSymbols { get; set; }

		public PartArrange(Data.MvProject mvProject)
		{
			InitializeComponent();

			this.MvProject = mvProject;
			this.panel_Graphics.MouseWheel += new MouseEventHandler(panel_Graphics_MouseWheel);
			this.panel_Symbol.Tag = 0;
		}

		private string GetSingleLineFolder(string rating, string feederType)
		{
			string path = Data.Location.Single(this.MvProject.IsShip, this.MvProject.ModelName, rating);

			List<string> list = Util.CSharp.GetDirectoryList(path);

			foreach (string folder in list)
			{
				if (feederType.StartsWith(folder))
					return path + "\\" + folder + "\\";
			}

			return null;
		}

		public void BindFeederTypes()
		{
			if (dataGridView1.CurrentRow == null) return;

			if (this.MvProject.IsShip)
			{
				string feederType = dataGridView1.CurrentRow.Cells["FeederType"].Value.ToString();
				this._singleLineFolder = this.GetSingleLineFolder("", feederType);

				Util.CompositeBinder binder = new Util.CompositeBinder(this._singleLineFolder, "EMA", dataGridView3);

				return;
			}
			else
			{
				string groupID = dataGridView1.CurrentRow.Cells[1].Value.ToString();
				DataRow[] rows = this.MvProject.GroupTable.Select("GroupID = '" + groupID + "'");
				Data.MvGroup group = new Data.MvGroup(this.MvProject, rows[0]);

				string feederType = dataGridView1.CurrentRow.Cells["FeederType"].Value.ToString();
				this._singleLineFolder = this.GetSingleLineFolder(group.Rating, feederType);

				Util.CompositeBinder binder = new Util.CompositeBinder(this._singleLineFolder, "EMA", dataGridView3);
			}

			return;
		}

		private void BindControls()
		{
			dataGridView1.DataSource = null;
			dataGridView1.DataSource = this.MvProject.FeederTable;
			foreach (DataGridViewColumn column in dataGridView1.Columns)
			{
				column.Visible = false;
			}

			dataGridView1.Columns[2].Visible = !this.MvProject.IsShip;

			dataGridView1.Columns[4].Visible = true;
			dataGridView1.Columns[4].Width = 150;
			dataGridView1.Columns[5].Visible = true;

			dataGridView1.Columns[2].DisplayIndex = 0;
			dataGridView1.Columns[4].DisplayIndex = 1;
			dataGridView1.Columns[5].DisplayIndex = 2;

			Util.CompositeBinder binder = new Util.CompositeBinder(this._singleLineFolder, "EMA", dataGridView3);

			Data.Mdb mdb = Data.MdbFactory.GetUniqueInstance;
			mdb.ShowCatergoty(comboBox1);
			mdb.ShowSubCatergory(comboBox1.Text, comboBox2);
		}


		private void ShowSearchResults()
		{
			Data.Mdb mdb = Data.MdbFactory.GetUniqueInstance;
			string category = comboBox1.Text;
			string subCategory = comboBox2.Text;
			string condition = textBox1.Text;

			mdb.ShowSearchResult(comboBox1.Text, comboBox2.Text, textBox1.Text, dataGridView2);

			this.panel_Symbol.Invalidate();
		}

		private bool SelectionOK()
		{
			if (dataGridView1.CurrentRow == null)
			{
				MessageBox.Show("Target feeder를 선택하십시오");
				return false;
			}

			if (dataGridView3.CurrentRow == null)
			{
				MessageBox.Show("Source feeder를 선택하십시오");
				return false;
			}

			return true;
		}



		private Point GetCurrentLocation(int x, int y)
		{
			Point point = new Point();
			if (this._singleline == null) return point;


			point.X = (int)((x - this._origin.X) / _zoomFactor * 120.0 / 117.0);
			point.Y = (int)((y - this._origin.Y) / _zoomFactor * 240.0 / 234.0);

			//point.X = (int)((x - this._origin.X) / _zoomFactor);
			//point.Y = (int)((y - this._origin.Y) / _zoomFactor);

			try
			{
				point.X += (int)this._singleline.Location.X;
				point.Y -= (int)this._singleline.Location.Y + 5;
			}
			catch
			{
				// Do Nothing;
			}

			point.Y = -point.Y;
			return point;
		}

		private System.Drawing.Rectangle GetDrawingRegion(WindowMacro wm)
		{
			System.Drawing.Rectangle r = new System.Drawing.Rectangle();
			r.X = (int)_origin.X;
			r.Y = (int)_origin.Y;
			try
			{
				r.Width = (int)(wm.Size.X * _zoomFactor);
				r.Height = (int)(wm.Size.Y * _zoomFactor);
			}
			catch
			{
				
			}

			return r;
		}




		private void ShowSingleline()
		{
			Util.Finder finder = new Util.Finder(this.MvProject, dataGridView1.CurrentRow);

			this._singleline = finder.Singleline;
			this._threeline = finder.Threeline;

			this.RefreshSingleline(this._singleline);
		}

		private void SetAsCurrentFeeder()
		{
			if (!this.SelectionOK()) return;

			Util.IFile file = dataGridView3.CurrentRow.DataBoundItem as Util.IFile;
			if (file == null) return;

			Util.Finder finder = new Util.Finder(this.MvProject, dataGridView1.SelectedRows[0]);

			this._singleline = finder.ReplaceSingleline(file.FullPath);
			this._threeline = finder.ReplaceThreeline(file.FullPath);

			this.RefreshSingleline(this._singleline);
		}

		private void ShowFeederPreView()
		{
			if (dataGridView3.CurrentRow == null) return;

			Util.IFolder folder = dataGridView3.CurrentRow.DataBoundItem as Util.IFolder;
			if (folder != null)
			{
				new Util.CompositeBinder(folder.Location, "EMA", dataGridView3);
				return;
			}

			Util.IFile comp = dataGridView3.CurrentRow.DataBoundItem as Util.IFile;
			if (comp == null)
			{
				MessageBox.Show("Feeder를 선택하십시오.");
				return;
			}

			PreView preview = PreView.GetSingleton(comp.FullPath);
			preview.Project = this.MvProject.Project;
			DialogResult dr = preview.ShowDialog();
			if (DialogResult.OK != dr) return;

			preview.Close();
			preview.Dispose();

			this.SetAsCurrentFeeder();

			this.ShowSingleline();
		}

		#region Event Handler

		private void PartArrange_Load(object sender, EventArgs e)
		{
			BindControls();
		}

		private void PartArrange_VisibleChanged(object sender, EventArgs e)
		{
			if (!this.Visible) return;

			BindControls();
		}


		private void comboBox1_TextChanged(object sender, EventArgs e)
		{
			Data.Mdb mdb = Data.MdbFactory.GetUniqueInstance;
			mdb.ShowSubCatergory(comboBox1.Text, comboBox2);
		}

		private void comboBox2_TextChanged(object sender, EventArgs e)
		{
			this.ShowSearchResults();
		}

		private void dataGridView1_SelectionChanged(object sender, EventArgs e)
		{
			this.BindFeederTypes();
		}

		private void dataGridView3_DoubleClick(object sender, EventArgs e)
		{
			this.ShowFeederPreView();
		}

		private void 미리보기ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.ShowFeederPreView();
		}


		private void 현재Feeder로설정ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.SetAsCurrentFeeder();
		}



		void panel_Graphics_MouseWheel(object sender, MouseEventArgs e)
		{
			_zoomFactor += (e.Delta / 1200.0) * 2.0;
			if (_zoomFactor < 1.0) _zoomFactor = 1.0;

			panel_Graphics.Invalidate();
		}

		private void panel_Graphics_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Middle)
			{
				_origin = new PointD(_origin.X + e.Location.X - _lastMouseLocation.X, _origin.Y + e.Location.Y - _lastMouseLocation.Y);
				panel_Graphics.Invalidate();

				this._lastMouseLocation.X = e.X;
				this._lastMouseLocation.Y = e.Y;
			}
		}
		#endregion

		private void panel_Graphics_Click(object sender, EventArgs e)
		{
			this.panel_Graphics.Focus();
		}

		private bool IsSameType(string name1, string name2)
		{
			name1 = name1.Substring(name1.LastIndexOf("-"));
			name2 = name2.Substring(name1.LastIndexOf("-"));

			return name1.ToUpper() == name2.ToUpper();
		}

		private void panel_Graphics_DragDrop(object sender, DragEventArgs e)
		{
			if (!e.Data.GetDataPresent(typeof(Data.MvPart))) return;

			Data.MvPart part = e.Data.GetData(typeof(Data.MvPart)) as Data.MvPart;
			if (part == null) return;

			Point point = panel_Graphics.PointToClient(new Point(e.X, e.Y));
			point = GetCurrentLocation(point.X, point.Y);

			string type = comboBox2.Text;
			int variant = (int)panel_Symbol.Tag;


			using (UndoStep undoStep = new UndoManager().CreateUndoStep())
			{
				string name3 = string.Empty;
				Function function1 = null;
				Function function3 = null;
				Function functiond = null;

				function1 = Util.eplan.GetBoxedDevice(this._singleline.Page, point);
				if (function1 == null)
				{
					function1 = Util.eplan.AddSinglelineSymbol(_singleline, type, part.PartNr, point, variant);
					function1.IsMainFunction = false;

					name3 = function1.Name.ToUpper().Replace("+G-", "+R-");
					function3 = Util.eplan.AddThreelineSymbol(_threeline, name3, part.PartNr, variant);
					if (function3 == null)
					{
						MessageBox.Show("ERROR: Threeline에 심볼을 추가하지 못했습니다:" + name3);
						return;
					}

					//if (this.MvProject.IsShip)
					//{
					//    functiond = Util.eplan.AddDoorArrangeSymbol(this.MvProject.Project, function1.Name, part.PartNr, 0);
					//}
				}

				name3 = function1.Name.ToUpper().Replace("+G-", "+R-");
				function3 = Util.eplan.RemoveARFromThreeline(_threeline, name3);
				if (function3 == null)
				{
					MessageBox.Show("ERROR: Threeline에서 심볼을 찾지 못했습니다:" + name3);
				}
				else
				{
					function3 = Util.eplan.AddArticleReference(function3, part.PartNr, 0);
				}

				if (this.MvProject.IsShip)
				{
					functiond = Util.eplan.RemoveARFromPanel(this.MvProject.Project, function1.Name);
					if (functiond == null)
					{
						//MessageBox.Show("ERROR: Door Arrange에서 심볼을 찾지 못했습니다: " + function1.Name);
					}

					functiond = Util.eplan.AddArticleReference(functiond, part.PartNr, 0);
				}

				undoStep.CloseOpenUndo();
			}

			this.RefreshSingleline(this._singleline);
		}

		private void RefreshSingleline(LocationBox singleline)
		{
			if (singleline == null) return;
			try
			{
				this._singleLineMacro = Util.eplan.CreateWindowMacro(singleline);
				this._feederView = new DrawingService();
				this._feederView.DrawConnections = true;

				this._feederView.CreateDisplayList(this._singleLineMacro);

				panel_Graphics.Invalidate();
			}
			catch (Exception ex)
			{
				MessageBox.Show("ERROR: Macro를 만들지 못했습니다..." + ex.Message + "\n" + ex.StackTrace);
			}
			return;
		}


		private void panel_Graphics_DragEnter(object sender, DragEventArgs e)
		{
			e.Effect = DragDropEffects.Copy;
		}

		private void panel_Graphics_MouseMove(object sender, MouseEventArgs e)
		{
			Point point = GetCurrentLocation(e.X, e.Y);
			toolStripStatusLabel1.Text = "Mouse = (" + e.X.ToString() + ", " + e.Y.ToString() + ")," +
						"Drawing = (" + point.X.ToString() + ", " + point.Y.ToString() + "), " +
						"Origin = (" + _origin.X.ToString() + ", " + _origin.Y.ToString() + "), " +
						"Zoom = " + _zoomFactor.ToString("F2");
		}


		private void panel_Graphics_Paint(object sender, PaintEventArgs e)
		{

			if (toolStripButton2.Checked)
			{
				this._zoomFactor = 1.0;
				toolStripButton2.Checked = false;
				this._origin = new PointD(0, 0);
			}

			panel_Graphics.Dock = DockStyle.Fill;

			System.Drawing.Rectangle r = this.GetDrawingRegion(this._singleLineMacro);
			try
			{
				this._feederView.DrawDisplayList(e, r);
			}
			catch { }
		}



		private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
		{
			this.ShowSingleline();
		}



		private void dataGridView2_MouseDown(object sender, MouseEventArgs e)
		{
			if (dataGridView2.SelectedRows.Count < 1) return;

			Data.MvPart part = new Data.MvPart(dataGridView2.SelectedRows[0]);
			DragDropEffects effect = DoDragDrop(part, DragDropEffects.Copy);
		}

		private void panel_Symbol_Click(object sender, EventArgs e)
		{
			int variant = 0;
			if (panel_Symbol.Tag != null)
			{
				variant = (int)panel_Symbol.Tag;
				variant++;
			}

			panel_Symbol.Tag = variant;
			panel_Symbol.Invalidate();
		}

		private void panel_Symbol_Paint(object sender, PaintEventArgs e)
		{
			if (this.MvProject.Project == null) return;
			if (dataGridView2.CurrentRow == null || dataGridView2.CurrentRow.Cells["PartNr"].Value == null) return;

			string partNr = dataGridView2.CurrentRow.Cells["PartNr"].Value.ToString();
			MDPartsDatabase mdb = Util.eplan.GetPartMaster();
			MDPart part = mdb.GetPart(partNr);
			if (part == null) return;

			string filePath = PathMap.SubstitutePath(part.Properties.ARTICLE_GROUPSYMBOLMACRO.ToString());
			if (!File.Exists(filePath)) return;

			int variant = 0;
			if (panel_Symbol.Tag != null)
			{
				variant = (int)panel_Symbol.Tag;
			}

			try
			{
				WindowMacro wm = new WindowMacro();
				wm.Open(filePath, this.MvProject.Project);
				wm.RepresentationType = WindowMacro.Enums.RepresentationType.SingleLine;

				if (wm.ExistVariant(WindowMacro.Enums.RepresentationType.SingleLine, variant))
					wm.Variant = variant;
				else
					panel_Symbol.Tag = wm.Variant = 0;

				DrawingService ds = new DrawingService();
				ds.MacroPreview = false;
				ds.CenterView = true;
				ds.CreateDisplayList(wm);
				ds.DrawDisplayList(e);
			}
			catch
			{
				//MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
			}
		}

		private void dataGridView2_MouseClick(object sender, MouseEventArgs e)
		{
			panel_Symbol.Invalidate();
		}

		private void dataGridView2_DragOver(object sender, DragEventArgs e)
		{
			e.Effect = DragDropEffects.Copy;
		}

		private void dataGridView2_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
		{
			if (e.EscapePressed)
			{
				e.Action = DragAction.Cancel;
			}
		}



		private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
		{
			Util.CSharp.ShowRowHeaderNumber(dataGridView1, e);
		}

		private void dataGridView2_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
		{
			Util.CSharp.ShowRowHeaderNumber(dataGridView2, e);
		}

		private void dataGridView2_VisibleChanged(object sender, EventArgs e)
		{
			if (this.Visible)
			{
				this.MvProject.Project = Util.eplan.GetEplanProject(this.MvProject.FileName);

			}
		}


		private void dataGridView2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{

			DataGridViewCell cell = this.dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex];

			DataRowView row = dataGridView2.Rows[e.RowIndex].DataBoundItem as DataRowView;
			if (row == null) return;

			string text = row["description1"].ToString();

			int index = text.IndexOf("@");
			if (index >= 0)
			{
				text = text.Substring(text.IndexOf("@") + 1);
			}

			cell.ToolTipText = text + " / " +
				row["partnr"].ToString() + " / " +
				row["manufacturer"].ToString();
		}




		Util.Finder _selectedFinder;

		private void feeder복사ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (dataGridView1.SelectedRows.Count < 1)
			{
				MessageBox.Show("선택된 Feeder 가 없습니다.");
				return;
			}

			_selectedFinder = new Util.Finder(this.MvProject, dataGridView1.SelectedRows[0]);
		}


		private void 선택된Feeder에붙여넣기ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (_selectedFinder.Singleline == null)
			{
				MessageBox.Show("복사된 Singleline이 없습니다.");
			}
			else
			{
				DialogResult dr = MessageBox.Show(_selectedFinder.Singleline.Name + "을 붙여넣기 합니다.", "MV DESIGN", MessageBoxButtons.OKCancel);
				if (dr != DialogResult.OK) return;
			}

			foreach (DataGridViewRow row in dataGridView1.SelectedRows)
			{
				Util.Finder finder = new Util.Finder(this.MvProject, row);
				using (UndoStep undoStep = new UndoManager().CreateUndoStep())
				{
					finder.ReplaceSingleline(_selectedFinder.Singleline);
					finder.ReplaceThreeline(_selectedFinder);

					undoStep.CloseOpenUndo();
				}
			}

			this.ShowSingleline();
		}


		#region ToolStrip Event Handlers

		private void toolStripButton1_Click(object sender, EventArgs e)
		{
			Util.eplan.UndoLast();
			this.RefreshSingleline(this._singleline);
		}

		private void toolStripButton2_Click(object sender, EventArgs e)
		{
			this.RefreshSingleline(this._singleline);
		}

		private void toolStripButton4_Click(object sender, EventArgs e)
		{
			new Edit().OpenPageWithName(this.MvProject.ProjectLinkFilePath, this._singleline.Page.Name);
		}

		private void toolStripButton5_Click(object sender, EventArgs e)
		{
			this._zoomFactor = this._zoomFactor / 2 < 1.0 ? 1.0 : this._zoomFactor / 2;

			this._origin = new PointD(0, 0);
			panel_Graphics.Invalidate();
		}

		private void toolStripButton6_Click(object sender, EventArgs e)
		{
			this._zoomFactor *= 2.0;
			panel_Graphics.Invalidate();
		}

		private void toolStripButton7_Click(object sender, EventArgs e)
		{
			Util.eplan.RedoLast();
			this.RefreshSingleline(this._singleline);
		}

		private void toolStripButton3_CheckStateChanged(object sender, EventArgs e)
		{
			if (toolStripButton3.Checked)
			{
				toolStripButton8.Checked = false;
				toolStripButton9.Checked = false;
				toolStripButton10.Checked = false;
				toolStripButton11.Checked = false;

				toolStripLabel1.Text = "삭제할 부품을 선택하십시오";
			}
			else
			{
				toolStripLabel1.Text = string.Empty;
			}

			this.SelectedSymbols = new List<Data.Symbol>();
		}

		private void toolStripButton8_CheckStateChanged(object sender, EventArgs e)
		{
			if (toolStripButton8.Checked)
			{
				toolStripButton3.Checked = false;
				toolStripButton9.Checked = false;
				toolStripButton10.Checked = false;
				toolStripButton11.Checked = false;

				toolStripLabel1.Text = "시계방향으로 연결할 부품 두개를 선택하십시오";
			}
			else
			{
				toolStripLabel1.Text = string.Empty;
			}
			this.SelectedSymbols = new List<Data.Symbol>();
		}

		private void toolStripButton9_CheckStateChanged(object sender, EventArgs e)
		{
			if (toolStripButton9.Checked)
			{
				toolStripButton3.Checked = false;
				toolStripButton8.Checked = false;
				toolStripButton10.Checked = false;
				toolStripButton11.Checked = false;

				toolStripLabel1.Text = "반시계방향으로 연결할 부품 두개를 선택하십시오";
			}
			else
			{
				toolStripLabel1.Text = string.Empty;
			}
			this.SelectedSymbols = new List<Data.Symbol>();
		}

		private void toolStripButton10_CheckStateChanged(object sender, EventArgs e)
		{
			if (toolStripButton10.Checked)
			{
				toolStripButton3.Checked = false;
				toolStripButton8.Checked = false;
				toolStripButton9.Checked = false;
				toolStripButton11.Checked = false;

				toolStripLabel1.Text = "수평으로 연결할 부품 두개를 선택하십시오";
			}
			else
			{
				toolStripLabel1.Text = string.Empty;
			}
			this.SelectedSymbols = new List<Data.Symbol>();
		}

		private void toolStripButton11_CheckStateChanged(object sender, EventArgs e)
		{
			if (toolStripButton11.Checked)
			{
				toolStripButton3.Checked = false;
				toolStripButton8.Checked = false;
				toolStripButton9.Checked = false;
				toolStripButton10.Checked = false;

				toolStripLabel1.Text = "수직으로 연결할 부품 두개를 선택하십시오";
			}
			else
			{
				toolStripLabel1.Text = string.Empty;
			}
			this.SelectedSymbols = new List<Data.Symbol>();
		}

		private void ResetToolStrip()
		{
			toolStripButton3.Checked = false;
			toolStripButton8.Checked = false;
			toolStripButton9.Checked = false;
			toolStripButton10.Checked = false;
			toolStripButton11.Checked = false;

			this.SelectedSymbols = new List<Data.Symbol>();
			toolStripLabel1.Text = "선택된 작업 없음";
		}


		#endregion

		private void AddToSelections(MouseEventArgs e)
		{
			if (this._singleline == null) return;
			if (this.SelectedSymbols == null)
			{
				this.SelectedSymbols = new List<Data.Symbol>();
			}

			Point point = GetCurrentLocation(e.X, e.Y);

			BoxedDevice bd = Util.eplan.GetBoxedDevice(this._singleline.Page, point);

			if (bd != null)
			{
				this.SelectedSymbols.Add(new Data.Symbol(bd));
			}

			toolStripLabel1.Text = this.SelectedSymbols.Count.ToString() + "개가 선택됨";
		}

		private void panel_Graphics_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Middle)
			{
				_lastMouseLocation = e.Location;
			}
			else if (e.Button == MouseButtons.Left)
			{
				this.AddToSelections(e);

				if (toolStripButton3.Checked)
				{
					DeletePart(e);
				}
				else if (toolStripButton8.Checked)
				{
					ConnectClockwise(e);
				}
				else if (toolStripButton9.Checked)
				{
					ConnectCounterClockwise(e);
				}
				else if (toolStripButton10.Checked)
				{
					AlignHorizontal(e);
				}
				else if (toolStripButton11.Checked)
				{
					AlignVertical(e);
				}

				return;
			}
		}

		private void DeletePart(MouseEventArgs e)
		{
			if (this.SelectedSymbols.Count < 1) return;

			string name = this.SelectedSymbols[0].Name;
			using (UndoStep undoStep = new UndoManager().CreateUndoStep())
			{
				Util.eplan.DeleteSinglelineSymbol(this.SelectedSymbols[0].Function);
				name = name.Replace("+G-", "+R-");
				Util.eplan.DeleteThreelineSymbol(_threeline, name);

				undoStep.CloseOpenUndo();
			}

			this.RefreshSingleline(this._singleline);
			this.ResetToolStrip();

			return;
		}

		private void ConnectClockwise(MouseEventArgs e)
		{
			if (this.SelectedSymbols.Count != 2) return;

			this.SelectedSymbols.Sort();

			int nVariant = 0;
			PointD point = new PointD();
			if (this.SelectedSymbols[0].Y < this.SelectedSymbols[1].Y)
			{
				point.X = this.SelectedSymbols[0].X;
				point.Y = this.SelectedSymbols[1].Y;

				nVariant = 0;
				Util.eplan.InsertSpecialSymbolVariant(this._singleline.Page, "CO", 0, point);
			}
			else
			{
				point.X = this.SelectedSymbols[1].X;
				point.Y = this.SelectedSymbols[0].Y;

				nVariant = 3;
			}

			using (UndoStep undoStep = new UndoManager().CreateUndoStep())
			{
				Util.eplan.InsertSpecialSymbolVariant(this._singleline.Page, "CO", nVariant, point);
				undoStep.CloseOpenUndo();
			}

			this.RefreshSingleline(this._singleline);
			this.ResetToolStrip();

			return;
		}

		private void ConnectCounterClockwise(MouseEventArgs e)
		{
			if (this.SelectedSymbols.Count != 2) return;

			this.SelectedSymbols.Sort();

			int nVariant = 0;
			PointD point = new PointD();

			if (this.SelectedSymbols[0].Y < this.SelectedSymbols[1].Y)
			{
				point.X = this.SelectedSymbols[1].X;
				point.Y = this.SelectedSymbols[0].Y;

				nVariant = 2;
			}
			else
			{
				point.X = this.SelectedSymbols[0].X;
				point.Y = this.SelectedSymbols[1].Y;

				nVariant = 1;
			}

			using (UndoStep undoStep = new UndoManager().CreateUndoStep())
			{
				Util.eplan.InsertSpecialSymbolVariant(this._singleline.Page, "CO", nVariant, point);
				undoStep.CloseOpenUndo();
			}

			this.RefreshSingleline(this._singleline);
			this.ResetToolStrip();

			return;
		}



		private void AlignHorizontal(MouseEventArgs e)
		{
			if (this.SelectedSymbols.Count != 2) return;

			PointD delta = new PointD();
			delta.Y = this.SelectedSymbols[0].Y - this.SelectedSymbols[1].Y;
			using (UndoStep undoStep = new UndoManager().CreateUndoStep())
			{
				this.SelectedSymbols[1].MovePlacements(delta);
				undoStep.CloseOpenUndo();
			}

			this.SelectedSymbols = new List<Data.Symbol>();
			this.ResetToolStrip();
			this.RefreshSingleline(this._singleline);

			return;
		}

		private void AlignVertical(MouseEventArgs e)
		{
			if (this.SelectedSymbols.Count != 2) return;

			PointD delta = new PointD();
			delta.X = this.SelectedSymbols[0].X - this.SelectedSymbols[1].X;
			using (UndoStep undoStep = new UndoManager().CreateUndoStep())
			{
				this.SelectedSymbols[1].MovePlacements(delta);
				undoStep.CloseOpenUndo();
			}

			this.SelectedSymbols = new List<Data.Symbol>();
			this.ResetToolStrip();
			this.RefreshSingleline(this._singleline);

			return;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			this.ShowSearchResults();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			textBox1.Text = string.Empty;
			this.ShowSearchResults();
		}


		private void button3_Click(object sender, EventArgs e)
		{
			UI.MdPart mdPart = UI.MdPart.GetUniqueInstance();
			mdPart.ShowDialog();

			this.ShowSearchResults();
		}

		private void textBox1_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				this.ShowSearchResults();
			}
		}
	}
}
