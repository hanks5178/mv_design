using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using NLog;

namespace Eplan.EplAddin.MvDesign.UI.UC
{
    public partial class FeederInfo : UserControl
    {
		Logger _logger;

        public Data.MvProject MvProject { get; set; }
		List<string> _feederNameList;

        public FeederInfo(Data.MvProject mvProject)
        {
            InitializeComponent();

            this.MvProject = mvProject;

			_feederNameList = new List<string>();
			Data.Feeder.FeederFactory factory = new Data.Feeder.FeederFactory(this.MvProject);
			foreach (Data.Feeder.FeederType feederType in factory.FeederTypes)
			{
				if (!_feederNameList.Contains(feederType.Name))
				{
					_feederNameList.Add(feederType.Name);
				}
			}
        }


        private void ShowFeederInfo()
        {
			_logger.Debug("private void ShowFeederInfo()");

			this.dataGridView2.DataSource = null;
            this.dataGridView2.DataSource = MvProject.FeederView;

			feederTypeDataGridViewComboBoxColumn.DataSource = this._feederNameList;

			foreach (DataGridViewColumn column in dataGridView2.Columns)
			{
				column.Width = 150;
			}  

            if (this.MvProject.IsShip)
            {
				// Named Field
                this.dataGridView2.Columns[0].HeaderText = "F_ID";
				this.dataGridView2.Columns[0].Visible = false;

				this.dataGridView2.Columns[1].HeaderText = "G_ID";
				this.dataGridView2.Columns[1].Visible = false;

				this.dataGridView2.Columns[2].HeaderText = "그룹 이름";
				this.dataGridView2.Columns[2].Visible = false;

				this.dataGridView2.Columns[3].HeaderText = "FEEDER NO";
				this.dataGridView2.Columns[4].HeaderText = "FEEDER TYPE";

				this.dataGridView2.Columns[3].Frozen = true;
				this.dataGridView2.Columns[4].Frozen = true;
				this.dataGridView2.Columns[5].Frozen = true;

				// 
				this.dataGridView2.Columns[5].HeaderText = "PANEL NAME 1";  
				this.dataGridView2.Columns[6].HeaderText = "PANEL NAME 2";	
				this.dataGridView2.Columns[7].HeaderText = "PANEL NAME 3";	
				this.dataGridView2.Columns[8].HeaderText = "CONSUMER(KW)";	
				this.dataGridView2.Columns[9].HeaderText = "CONSUMER(A)";
				this.dataGridView2.Columns[10].HeaderText = "CABLE SQ(YARD)";
				this.dataGridView2.Columns[11].HeaderText = "CABLE INLET";
				this.dataGridView2.Columns[12].HeaderText = "TERMINAL LUG";
				this.dataGridView2.Columns[12].Visible = false;

				this.dataGridView2.Columns[13].HeaderText = "BREAKER TYPE";
				this.dataGridView2.Columns[14].HeaderText = "ACC'Y";
                this.dataGridView2.Columns[15].HeaderText = "EM'GY STOP";
				this.dataGridView2.Columns[16].HeaderText = "INST TRIP NOB";
				this.dataGridView2.Columns[17].HeaderText = "REMARK";

				this.dataGridView2.Columns[13].DefaultCellStyle.BackColor = Color.Beige;
				this.dataGridView2.Columns[14].DefaultCellStyle.BackColor = Color.Beige;
				this.dataGridView2.Columns[15].DefaultCellStyle.BackColor = Color.Beige;
				this.dataGridView2.Columns[16].DefaultCellStyle.BackColor = Color.Beige;
				this.dataGridView2.Columns[17].DefaultCellStyle.BackColor = Color.Beige;

				this.dataGridView2.Columns[18].Visible = false;
				this.dataGridView2.Columns[19].Visible = false;
				this.dataGridView2.Columns[20].Visible = false;
            }
            else
            {
                this.dataGridView2.Columns[0].HeaderText = "F_ID";
				this.dataGridView2.Columns[0].Visible = false;

				this.dataGridView2.Columns[1].HeaderText = "G_ID";
				this.dataGridView2.Columns[1].Visible = false;

				this.dataGridView2.Columns[2].Frozen = true;
				this.dataGridView2.Columns[3].Frozen = true;
				this.dataGridView2.Columns[4].Frozen = true;
				this.dataGridView2.Columns[5].Frozen = true;

				this.dataGridView2.Columns[2].HeaderText = "그룹 번호";
                this.dataGridView2.Columns[3].HeaderText = "FEEDER NO";
				this.dataGridView2.Columns[4].HeaderText = "FEEDER TYPE";
				this.dataGridView2.Columns[5].HeaderText = "PANEL NAME 1";
				this.dataGridView2.Columns[6].HeaderText = "PANEL NAME 2";
				this.dataGridView2.Columns[7].HeaderText = "PANEL NAME 3";
				this.dataGridView2.Columns[8].HeaderText = "CONSUMER(KW)";
				this.dataGridView2.Columns[9].HeaderText = "CONSUMER(A)";
				this.dataGridView2.Columns[10].HeaderText = "CABLE SQ(HHI)";
				this.dataGridView2.Columns[11].HeaderText = "CABLE SQ(CONSUMER)";
				this.dataGridView2.Columns[12].HeaderText = "TYPICAL DWG NO";
				this.dataGridView2.Columns[13].HeaderText = "CABLE CLAMP";
                this.dataGridView2.Columns[14].HeaderText = "NAME PLATE TYPE";

                this.dataGridView2.Columns[15].HeaderText = "UDEFINED";
				this.dataGridView2.Columns[16].HeaderText = "UDEFINED";
				this.dataGridView2.Columns[17].HeaderText = "UDEFINED";
				this.dataGridView2.Columns[18].HeaderText = "UDEFINED";
				this.dataGridView2.Columns[19].HeaderText = "UDEFINED";
				this.dataGridView2.Columns[20].HeaderText = "UDEFINED";

				this.dataGridView2.Columns[14].Visible = false;
				this.dataGridView2.Columns[15].Visible = false;
				this.dataGridView2.Columns[16].Visible = false;
				this.dataGridView2.Columns[17].Visible = false;
				this.dataGridView2.Columns[18].Visible = false;
				this.dataGridView2.Columns[19].Visible = false;
				this.dataGridView2.Columns[20].Visible = false;
			}

			this.dataGridView2.Columns[21].HeaderText = "PanelX";
			this.dataGridView2.Columns[22].HeaderText = "PanelY";
			this.dataGridView2.Columns[23].HeaderText = "IsSelected";
			this.dataGridView2.Columns[24].HeaderText = "IsNew";
			this.dataGridView2.Columns[25].HeaderText = "IsDone";

			this.dataGridView2.Columns[21].Visible = false;
			this.dataGridView2.Columns[22].Visible = false;
			this.dataGridView2.Columns[23].Visible = false;
			this.dataGridView2.Columns[24].Visible = false;
			this.dataGridView2.Columns[25].Visible = false;
        }



        private void ShowGroupInfo()
        {
			_logger.Debug("private void ShowGroupInfo()");

            this.dataGridView1.DataSource = null;
            this.dataGridView1.DataSource = this.MvProject.GroupTable;

            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.Visible = false;
            }

            dataGridView1.Columns[1].Visible = false;
            dataGridView1.Columns[2].Visible = true;
            dataGridView1.Columns[3].Visible = false;
            dataGridView1.Columns[4].Visible = true;

            dataGridView1.Columns[1].HeaderText = "";
            dataGridView1.Columns[2].HeaderText = "그룹 번호";
            dataGridView1.Columns[3].HeaderText = "그룹 이름";
            dataGridView1.Columns[4].HeaderText = "수량";

            dataGridView1.Columns[1].Width = 24;
            dataGridView1.Columns[2].Width = 100;
            dataGridView1.Columns[3].Width = 100;
            dataGridView1.Columns[4].Width = 55;
        }


        private void FeederInfo_Load(object sender, EventArgs e)
        {
			_logger = MyLogManager.MyLogManager.Instance.GetCurrentClassLogger();

            ShowGroupInfo();
            ShowFeederInfo();
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Util.CSharp.ShowRowHeaderNumber(dataGridView2, e);
        }

        private void dataGridView2_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Util.CSharp.ShowRowHeaderNumber(dataGridView2, e);
        }

        private void 셀복사ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Util.CSharp.CopyCell(dataGridView2);
        }

        private void 연속데이터채우기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Util.CSharp.FillContinuousData(dataGridView2);
        }

        private void dataGridView2_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
			ComboBox comboBox = e.Control as ComboBox;
			if (comboBox == null) return;

			comboBox.DropDownClosed += new EventHandler(comboBox_DropDownClosed);
			//comboBox.DropDownStyle = ComboBoxStyle.DropDown;

			//string groupRating = this.MvProject.GetGroupRating(this.dataGridView2.CurrentRow.Cells[1].Value);
			//Data.Feeder.FeederFactory factory = new Data.Feeder.FeederFactory(this.MvProject, groupRating);

			//comboBox.Items.Clear();
			//foreach (Data.Feeder.FeederType feederType in factory.FeederTypes)
			//{
			//    if (!comboBox.Items.Contains(feederType.Name))
			//    {
			//        comboBox.Items.Add(feederType.Name);
			//    }
			//}
        }

		void comboBox_DropDownClosed(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox == null) return;

			dataGridView2.EndEdit();
            dataGridView2.CurrentRow.Cells[5].Value = comboBox.Text;
        }

        private void dataGridView2_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            
        }

        private void dataGridView2_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            DataGridView dataGridView = sender as DataGridView;
            if (dataGridView.CurrentCell.IsInEditMode)
            {
                if (dataGridView.CurrentCell.GetType() == typeof(DataGridViewComboBoxCell))
                {
                    if (!((DataGridViewComboBoxColumn)dataGridView.Columns[e.ColumnIndex]).Items.Contains(e.FormattedValue))
                    {
                        ((DataGridViewComboBoxColumn)dataGridView.Columns[e.ColumnIndex]).Items.Add(e.FormattedValue);
                    }
                }
            }
        }

		//private void dummy추가ToolStripMenuItem_Click(object sender, EventArgs e)
		//{
		//    if (this.MvProject.IsShip)
		//    {
		//        MessageBox.Show("선박배전반에는 적용되지 않는 기능입니다.");
		//        return;
		//    }

		//    string groupID = dataGridView2.CurrentRow.Cells["GroupID"].Value.ToString();
		//    string groupNo = dataGridView2.CurrentRow.Cells["GroupNo"].Value.ToString();
		//    int panelX = Convert.ToInt32(dataGridView2.CurrentRow.Cells[19].Value.ToString());

		//    DataRow[] rows = this.MvProject.FeederTable.Select("GroupID = '" + groupID + "'");
		//    foreach (DataRow row in rows)
		//    {
		//        int n = Convert.ToInt32(row["PanelX"]);
		//        if (n >= panelX) row["PanelX"] = n + 1;
		//    }

		//    DataRow new_row = this.MvProject.FeederTable.NewRow();
		//    new_row["GroupID"] = groupID;
		//    new_row["GroupNo"] = groupNo;
		//    new_row["FeederType"] = "DUMMY";
		//    new_row["PanelName1"] = "DUMMY";
		//    new_row["PanelX"] = panelX;

		//    this.MvProject.FeederTable.Rows.Add(new_row);
		//}

		//private void dummy삭제ToolStripMenuItem_Click(object sender, EventArgs e)
		//{
		//    //if (this.MvProject.IsShip)
		//    //{
		//    //    MessageBox.Show("선박배전반에는 적용되지 않는 기능입니다.");
		//    //    return;
		//    //} 
            

		//}

		private void dataGridView2_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete)
			{
				foreach (DataGridViewCell cell in dataGridView2.SelectedCells)
				{
					cell.Value = string.Empty;
				}
			}

			if (e.KeyCode == Keys.C)
			{
				Util.CSharp.CopyToClipBoard(dataGridView2);
			}
			else if (e.KeyCode == Keys.V)
			{
				Util.CSharp.PasteFromClipboard(dataGridView2);
			}
		}

		private void feeder추가ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string groupID = dataGridView2.CurrentRow.Cells[1].Value.ToString();
			string groupNo = dataGridView2.CurrentRow.Cells[2].Value.ToString();
			int panelX = Convert.ToInt32(dataGridView2.CurrentRow.Cells[21].Value.ToString());

			DataRow[] rows = this.MvProject.FeederTable.Select("GroupID = '" + groupID + "'");
			foreach (DataRow row in rows)
			{
				int n = Convert.ToInt32(row["PanelX"]);
				if (n >= panelX) row["PanelX"] = n + 1;
			}

			DataRow new_row = this.MvProject.FeederTable.NewRow();
			new_row["GroupID"] = groupID;
			new_row["GroupNo"] = groupNo;
			new_row["PanelX"] = panelX;

			this.MvProject.FeederTable.Rows.Add(new_row);
		}

		private void feeder삭제ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string feederID = dataGridView2.CurrentRow.Cells[0].Value.ToString();
			string groupID = dataGridView2.CurrentRow.Cells[1].Value.ToString();

			DataRow[] rows = this.MvProject.FeederTable.Select("GroupID = '" + groupID + "' AND FeederID = '" + feederID + "'");
			if (rows.Length > 0) this.MvProject.FeederTable.Rows.Remove(rows[0]);
		}

		private void toolStripMenuItem1_Click(object sender, EventArgs e)
		{
			if (dataGridView2.SelectedCells.Count < 1)
			{
				MessageBox.Show("셀을 선택하십시오");
				return;
			}

			Util.CSharp.CopyToClipBoard(dataGridView2);
			//string text = string.Empty;
			//for (int i = dataGridView2.SelectedCells.Count - 1; i >= 0; i--)
			//{
			//    text += dataGridView2.SelectedCells[i].Value.ToString();
			//    if (i > 0) text += "\r\n";
			//}

			//Clipboard.SetText(text);
			//MessageBox.Show(dataGridView2.SelectedCells.Count.ToString() + "개의 값이 복사되었습니다.\n" + text);
		}

		private void toolStripMenuItem2_Click(object sender, EventArgs e)
		{
			Util.CSharp.PasteFromClipboard(dataGridView2);
			//string[] texts = Clipboard.GetText().Split(new string[] { "\r\n" }, StringSplitOptions.None);

			//int index = 0;
			//for (int i = dataGridView2.SelectedCells.Count - 1; i >= 0; i--)
			//{
			//    dataGridView2.SelectedCells[i].Value = texts[index];
			//    index++;
			//    if (index == texts.Length) index = 0;
			//}
		}


    }
}
