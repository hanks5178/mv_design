using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Eplan.EplAddin.MvDesign.UI.UC
{
    public partial class Layout : UserControl
    {
        public Data.MvProject MvProject { get; set; }
        public List<Data.MvButton> FeederList { get; set; }

        public Layout(Data.MvProject mvProject)
        {
            InitializeComponent();

            this.MvProject = mvProject;
        }

        private void SetToolbars()
        {
            tsb_InsertMode.Checked = this.MvProject.IsPanelInsertMode;
            tsb_ReplaceMode.Checked = !this.MvProject.IsPanelInsertMode;

            tscb_GroupNumber.Items.Clear();
            foreach (DataRow row in this.MvProject.GroupTable.Rows)
            {
                Data.MvGroup group = new Data.MvGroup(this.MvProject, row);
                tscb_GroupNumber.Items.Add(group.GroupNo);
            }

            if (tscb_GroupNumber.Items.Count < 1) return;

            if (this.MvProject.CurrentGroup == null)
            {
                tscb_GroupNumber.SelectedIndex = 0;
                this.MvProject.SetCurrentGroup(tscb_GroupNumber.Text);
            }
            else
            {
                tscb_GroupNumber.Text = this.MvProject.CurrentGroup.GroupNo;
            }
        }

        private GroupBox GetGroupBox(int index)
        {
            GroupBox groupBox = new GroupBox();

            int denominator = this.MvProject.IsDoubleLayer ? 2 : 1;
            int x = index / denominator;
            int y = index % denominator;
            groupBox.Location = new Point(x * 110 + 10, y * 100 + 10);
            groupBox.Size = new Size(100, 200 / denominator);
            groupBox.Text = this.MvProject.IsDoubleLayer ? string.Format("Panel({0}, {1})", x + 1, y + 1) : string.Format("Panel({0})", x + 1);
           
            groupBox.Tag = new Data.PanelTag(index);
           
            return groupBox;
        }


        private void ShowPanelArrange()
        {
            if (this.MvProject.CurrentGroup == null) return;
            if (this.MvProject.CurrentGroup.FeederRows.Length < 1) return;
            this.tstb_FeederQty.Text = "QTY: " + this.MvProject.CurrentGroup.FeederRows.Length.ToString();

            this.panel_Arranged.Controls.Clear();
            this.panel_UnArranged.Controls.Clear();

            for (int i=0; i<this.MvProject.CurrentGroup.FeederRows.Length; i++)
            {
                GroupBox groupBox = this.GetGroupBox(i);
                panel_Arranged.Controls.Add(groupBox);
            }

            this.FeederList = new List<Data.MvButton>();
            for (int i = 0; i < this.MvProject.CurrentGroup.FeederRows.Length; i++)
            {
                Data.MvButton feeder = new Data.MvButton(this.MvProject, this.MvProject.CurrentGroup.FeederRows[i]);
                feeder.Info.PanelX = i;

                feeder.PanelArranged = panel_Arranged;
                feeder.PanelUnArranged = panel_UnArranged;

                this.FeederList.Add(feeder);
            }

            foreach (Data.MvButton button in this.FeederList)
            {
                button.Locate();
            }
        }

		       private void AddDummies()
        {
            foreach (Data.MvButton feeder in this.FeederList)
            {
                if (feeder.IsSelected)
                {
                    DataRow[] feederRows = this.MvProject.FeederTable.Select("GroupID = '" + feeder.Info.GroupID.ToString() + "'");
                    foreach (DataRow feederRow in feederRows)
                    {
                        int panelX = Convert.ToInt32(feederRow["PanelX"]);
                        if (panelX >= feeder.Info.PanelX)
                        {
                            feederRow["PanelX"] = ++panelX;
                        }
                    }

                    DataRow row = this.MvProject.FeederTable.NewRow();
                    row["GroupID"] = feeder.Info.GroupID;
                    row["GroupNo"] = feeder.Info.GroupNo;

                    row["PanelX"] = feeder.Info.PanelX;
                    row["PanelY"] = feeder.Info.PanelY;
                    this.MvProject.FeederTable.Rows.Add(row);
                }
            }

            ShowPanelArrange();
        }


        private void Layout_Load(object sender, EventArgs e)
        {
            this.SetToolbars();
            this.ShowPanelArrange();
        }

        private void tscb_GroupNumber_TextChanged(object sender, EventArgs e)
        {
            this.MvProject.SetCurrentGroup(tscb_GroupNumber.Text);
            this.ShowPanelArrange();
        }

        private void tsb_InsertMode_Click(object sender, EventArgs e)
        {
			//this.MvProject.IsPanelInsertMode = false;
			//this.tsb_InsertMode.Visible = false;
			//this.tsb_ReplaceMode.Visible = true;
        }

        private void tsb_ReplaceMode_Click(object sender, EventArgs e)
        {
			//this.MvProject.IsPanelInsertMode = true;
			//this.tsb_InsertMode.Visible = true;
			//this.tsb_ReplaceMode.Visible = false;
        }

		private void tsb_InsertMode_CheckStateChanged(object sender, EventArgs e)
		{
			tsb_ReplaceMode.Checked = !tsb_InsertMode.Checked;
			this.MvProject.IsPanelInsertMode = tsb_InsertMode.Checked;
		}

		private void tsb_ReplaceMode_CheckStateChanged(object sender, EventArgs e)
		{
			tsb_InsertMode.Checked = !tsb_ReplaceMode.Checked;
			this.MvProject.IsPanelInsertMode = !tsb_ReplaceMode.Checked;
		}


		private void toolStripButton2_Click(object sender, EventArgs e)
		{
			this.MvProject.SetCurrentGroup(tscb_GroupNumber.Text);
			this.ShowPanelArrange();
		}

		private void Layout_VisibleChanged(object sender, EventArgs e)
		{
			if (this.Visible)
			{
				this.MvProject.SetCurrentGroup(tscb_GroupNumber.Text);
				this.ShowPanelArrange();
			}
		}





    }
}
