using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Eplan.EplApi.Base;
using Eplan.EplApi.DataModel;

namespace Eplan.EplAddin.MvDesign.UI
{
    public partial class MainForm : Form
	{
		#region Properties & Constructors

		NLog.Logger _logger;

		public Data.MvProject MvProject;

        private MainForm()
        {
            InitializeComponent();
            //this.tsb_Debug.Visible = true;
        }

		#endregion

		#region Singleton Pattern

		private static MainForm singleton;
        public static MainForm GetSingleton()
        {
            if (singleton == null || singleton.IsDisposed)
            {
                singleton = new MainForm();
            }
            else
            {
                singleton.BringToFront();
            }

            return singleton;
        }

        #endregion

		#region Event Handler

		private void MainForm_Load(object sender, EventArgs e)
        {
			_logger = MyLogManager.MyLogManager.Instance.GetCurrentClassLogger();
            if (this.MvProject.IsShip)
            {
                this.Text = "선박 배전반: " + this.MvProject.FileName;
            }
            else
            {
				this.Text = "육상 배전반: " + this.MvProject.FileName;
            }
        }

        private void tabPage1_Enter(object sender, EventArgs e)
        {
            if (this.MvProject.IsShip)
            {
                if (tabPage1.Controls.Count == 0)
                {
                    UI.Ship.ProjectInfo info = new Ship.ProjectInfo(this.MvProject);
                    this.tabPage1.Controls.Add(info);
                }

                this.Size = new Size(617, 525);
            }
            else
            {
                if (tabPage1.Controls.Count == 0)
                {
                    UI.Land.ProjectInfo info = new Land.ProjectInfo(this.MvProject);
                    this.tabPage1.Controls.Add(info);
                }

                this.Size = new Size(610, 578);
            }
        }


        private void tabPage2_Enter(object sender, EventArgs e)
        {
            if (this.MvProject.IsShip)
            {
                if (tabPage2.Controls.Count == 0)
                {
                    Ship.Spec spec = new Ship.Spec();
                    spec.Dock = DockStyle.Fill;
                    this.tabPage2.Controls.Add(spec);
                }

                this.Size = new Size(911, 706);
            }
            else
            {
                if (tabPage2.Controls.Count == 0)
                {
                    Land.Spec spec = new Land.Spec();
                    spec.Dock = DockStyle.Fill;
                    this.tabPage2.Controls.Add(spec);
                }

                this.Size = new Size(675, 669);
            }

            if (this.tabPage2.Controls.Count > 0)
            {
                Util.FormSerializer.Deserialise(tabPage2.Controls[0], this.MvProject.FormXmlFile);
            }
        }

        private void tabPage3_Enter(object sender, EventArgs e)
        {
            if (tabPage3.Controls.Count == 0)
            {
                this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
                UC.GroupInfo control = new UC.GroupInfo(this.MvProject);
                control.Dock = DockStyle.Fill;
                this.tabPage3.Controls.Add(control);
            }

            new Util.FormSizeSetter(this);
        }


        private void tabPage4_Enter(object sender, EventArgs e)
        {
            if (tabPage4.Controls.Count == 0)
            {
                UC.FeederInfo control = new UC.FeederInfo(this.MvProject);
                control.Dock = DockStyle.Fill;
                this.tabPage4.Controls.Add(control);
            }

            new Util.FormSizeSetter(this);
        }


        private void tabPage5_Enter(object sender, EventArgs e)
        {
            if (tabPage5.Controls.Count == 0)
            {
                UC.Layout control = new UC.Layout(this.MvProject);
                control.Dock = DockStyle.Fill;
                this.tabPage5.Controls.Add(control);
            }

            new Util.FormSizeSetter(this);
        }


        private void tabPage6_Enter(object sender, EventArgs e)
        {
            if (tabPage6.Controls.Count == 0)
            {
                UC.PartArrange control = new UC.PartArrange(this.MvProject);
                control.Dock = DockStyle.Fill;
                this.tabPage6.Controls.Add(control);
            }

			new Util.FormSizeSetter(this);
			if (this.MvProject.Project == null)
			{
				this.MvProject.Project = Util.eplan.FindOpenedProject(this.MvProject.FileName);
				if (this.MvProject.Project != null)
				{
					MessageBox.Show("프로젝트를 찾았습니다.");
				}
				else
				{
					this.MvProject.Project = Util.eplan.OpenProject(this.MvProject.FileName);
					if (this.MvProject.Project == null)
					{
						MessageBox.Show("부품 배치를 위해서는 EPLAN Project를 생성해야 합니다");
					}
				}
			}
        }


        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (Data.MvProject.Open(ref this.MvProject))
            {
                this.tabPage1.Focus();
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Data.MvProject.Save(this.MvProject);
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Data.MvProject.SaveAs(this.MvProject);
        }


        private void toolStripButton4_Click(object sender, EventArgs e)
        {


			try
			{
				this.Cursor = Cursors.WaitCursor;
				if (this.MvProject.IsShip)
				{
					if (this.MvProject.FeederTable.Rows.Count < 16 || this.MvProject.FeederTable.Rows.Count > 22)
					{
						MessageBox.Show("Panel 수량이 16~22개가 아닙니다.");
						return;
					}
				}

				foreach (DataRow row in this.MvProject.FeederTable.Rows)
				{
					if (row["FeederNo"].ToString().Length < 1)
					{
						MessageBox.Show("Feeder No를 모두 입력하십시오...");
						return;
					}
				}

				if (this.tabPage2.Controls.Count > 0)
				{
					Util.FormSerializer.Serialise(tabPage2.Controls[0], this.MvProject.FormXmlFile);
				}
			}
			catch(Exception ex)
			{
				MessageBox.Show("ERROR: " + ex.Message + "\n" + ex.StackTrace);
				return;
			}

			FormProgressBar form = new FormProgressBar(); 
			try
			{
				form.MvProject = this.MvProject;
				form.ShowDialog();

				if (form.DialogResult == DialogResult.OK)
				{
					MessageBox.Show("프로젝트를 생성했습니다: " + this.MvProject.ProjectLinkFilePath);
				}
				else
				{
					MessageBox.Show("프로젝트를 생성하지 못했습니다. 잘못된 파일/폴더가 있을 수 있습니다.\n파일/폴더를 지우고 다시 하십시오... \n" + this.MvProject.ProjectLinkFilePath);
				}
			}
			finally
			{
				form.Close();
				form.Dispose();
				this.Cursor = Cursors.Default;
			}
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            AboutBox1 ab = new AboutBox1();
            ab.ShowDialog();
        }

        private void tsb_Debug_Click(object sender, EventArgs e)
        {
			Eplan.EplApi.HEServices.SelectionSet selectionSet= new EplApi.HEServices.SelectionSet();

			foreach (StorableObject so in selectionSet.Selection)
			{
				;
			}
		}

		#endregion

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.Dispose();
		}

		private void openProjectFolderToolStripMenuItem_Click(object sender, EventArgs e)
		{
			PathInfo p = new PathInfo();
			Process.Start(p.Projects);
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.Close();
			this.Dispose();
		}

		private void toolStripButton6_Click(object sender, EventArgs e)
		{

		}
	}
}
