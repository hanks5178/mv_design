using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Eplan.EplApi.Base;
using Eplan.EplApi.DataModel;

namespace Eplan.EplAddin.MvDesign.UI
{
	public partial class FormProgressBar : Form
	{
		public Data.MvProject MvProject { get; set; }
		private BackgroundWorker _backgroudWorker;
		public FormProgressBar()
		{
			InitializeComponent();
		}

		private void FormProgressBar_Shown(object sender, EventArgs e)
		{
			this.Cursor = Cursors.WaitCursor;

			_backgroudWorker = new BackgroundWorker();
			_backgroudWorker.WorkerReportsProgress = true;
			_backgroudWorker.DoWork += new DoWorkEventHandler(_backgroudWorker_DoWork);
			_backgroudWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_backgroudWorker_RunWorkerCompleted);

			_backgroudWorker.RunWorkerAsync();
		}

		void _backgroudWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			this.Close();
			this.Dispose();
		}

		private void SetShipProperties(Project project)
		{
			project.Properties.PROJ_DRAWINGNUMBER = this.MvProject.FileName;
			MultiLangString mls = new MultiLangString();
			mls.AddString(ISOCode.Language.L___, this.MvProject.ShipInfo.Yard + "-" + this.MvProject.ShipInfo.ShipNo);
			project.Properties.PROJ_TYPE = mls;
		}

		private void SetLandProperties(Project project)
		{
			project.Properties.PROJ_DRAWINGNUMBER = this.MvProject.LandInfo.DrawingNo;
		}


		void _backgroudWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			try
			{
				this.label1.Text = "Project 생성 중: " + this.MvProject.FileName;
				if (!MvProject.CreateProject())
				{
					return;
				}

				this.label1.Text = "Cover Sheet 생성 중: " + this.MvProject.FileName;
				Drawings.Cover cover = new Drawings.Cover(this.MvProject);

				this.label1.Text = "Spec Sheet 생성 중: " + this.MvProject.FileName;
				if (this.MvProject.IsShip)
				{
					this.SetShipProperties(this.MvProject.Project);
					Ship.Spec spec = new Ship.Spec();
					Util.FormSerializer.Deserialise(spec, this.MvProject.FormXmlFile);
					new Drawings.Spec(this.MvProject, spec);
				}
				else
				{
					this.SetLandProperties(this.MvProject.Project);
					Land.Spec spec = new Land.Spec();
					Util.FormSerializer.Deserialise(spec, this.MvProject.FormXmlFile);
					new Drawings.Spec(this.MvProject, spec);
				}


				this.label1.Text = "Layout Drawing 생성 중: " + this.MvProject.FileName;
				if (this.MvProject.IsShip)
				{
					Drawings.Layout.Ship layout = new Drawings.Layout.Ship(this.MvProject);
					Drawings.Layout.Door door = new Drawings.Layout.Door(this.MvProject);
				}
				else
				{
					Drawings.Layout.Land layout = new Drawings.Layout.Land(this.MvProject);
				}

				this.label1.Text = "회로 도면 생성 중: " + this.MvProject.FileName;
				new Drawings.DrawingFactory(this.MvProject);

				foreach (Page p in this.MvProject.Project.Pages)
				{
					if (p.Name.StartsWith("==GROUP"))
					{
						p.Remove();
					}
				}

				this.DialogResult = DialogResult.OK;
			}
			catch (Exception ex)
			{
				MessageBox.Show("ERROR: " + ex.Message + "\n" + ex.StackTrace);
				this.DialogResult = DialogResult.No;
			}
			finally
			{
				this.Cursor = Cursors.Default;
			}
		}
	}
}
