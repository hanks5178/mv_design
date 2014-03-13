using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Eplan.EplApi.Base;
using Eplan.EplApi.DataModel;
using Eplan.EplApi.DataModel.MasterData;
using Eplan.EplApi.HEServices;

namespace Eplan.EplAddin.MvDesign.UI
{
	public partial class PreView : Form
	{
		public string SelectedPath { get; set; }
		public Project Project { get; set; }

		private string MacroPath { get; set; }
		private DrawingService _drawingService { get; set; }
		private WindowMacro _windowMacro { get; set; }
		private double _zoomFactor = 1.0;
		private PointD _origin = new PointD(0, 0);
		private Point _lastMouseLocation { get; set; }

		private PreView(string path)
		{
			this.MacroPath = path;
			InitializeComponent();

			this.AcceptButton = button1;
			this.CancelButton = button2;
		}

		#region Singleton Pattern

		private static PreView singleton;
		public static PreView GetSingleton(string path)
		{
			if (singleton == null || singleton.IsDisposed)
			{
				singleton = new PreView(path);
			}
			else
			{
				singleton.BringToFront();
			}

			return singleton;
		}

		#endregion



		private void PreView_Load(object sender, EventArgs e)
		{
			
			if (!File.Exists(this.MacroPath))
			{
				MessageBox.Show("ERROR: File not found: " + this.MacroPath);
				return;
			}

			_windowMacro = new WindowMacro();
			_windowMacro.Open(this.MacroPath, this.Project);
			_drawingService = new DrawingService();
			_drawingService.DrawConnections = true;
			_drawingService.MacroPreview = true;

			_drawingService.CreateDisplayList(_windowMacro);

			panel1.MouseWheel += new MouseEventHandler(panel1_MouseWheel);

		}

		private void panel1_Paint(object sender, PaintEventArgs e)
		{
			Rectangle r = new Rectangle();
			r.X = (int)this._origin.X;
			r.Y = (int)this._origin.Y;
			r.Width = (int)(this.panel1.Size.Width * this._zoomFactor);
			r.Height = (int)(this.panel1.Size.Height * this._zoomFactor);
			_drawingService.DrawDisplayList(e, r);
		}

		void panel1_MouseWheel(object sender, MouseEventArgs e)
		{
			this._zoomFactor += (e.Delta / 1200.0) * 2.0;
			if (_zoomFactor < 1.0) this._zoomFactor = 1.0;

			panel1.Invalidate();
		}

		private void panel1_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Middle)
			{
				this._lastMouseLocation = e.Location;
			}
		}

		private void panel1_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Middle)
			{
				this._origin = new PointD(this._origin.X + e.Location.X - this._lastMouseLocation.X, this._origin.Y + e.Location.Y - this._lastMouseLocation.Y);
				this.panel1.Invalidate();
			}
		}

		private void panel1_MouseClick(object sender, MouseEventArgs e)
		{
			panel1.Focus();
		}

		private void panel1_SizeChanged(object sender, EventArgs e)
		{
			panel1.Invalidate();
		}

		private void PreView_FormClosed(object sender, FormClosedEventArgs e)
		{
			if (this._windowMacro != null)
			{
				this._windowMacro.Dispose();
			}

			if (this._drawingService != null)
			{
				this._drawingService.Dispose();
			}

			GC.Collect();
			this.Dispose();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
		}

		private void button2_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		}
	}
}
