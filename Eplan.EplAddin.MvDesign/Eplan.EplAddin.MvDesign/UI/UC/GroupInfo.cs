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
    public partial class GroupInfo : UserControl
    {
        public Data.MvProject MvProject { get; set; }

        public GroupInfo(Data.MvProject mvProject)
        {
            InitializeComponent();
            this.MvProject = mvProject;
        }


        #region Methods

        private void BindControls()
        {
            this.dataGridView1.DataSource = null;
            this.dataGridView1.DataSource = this.MvProject.GroupTable;

            if (this.MvProject.IsShip)
            {
                this.dataGridView1.Columns[0].Visible = false;
                this.dataGridView1.Columns[1].Visible = false;
				//this.dataGridView1.Columns[2].Visible = true;
				this.dataGridView1.Columns[1].Visible = false;
				this.dataGridView1.Columns[3].HeaderText = "그룹 이름"; 
				//this.dataGridView1.Columns[4].Visible = true;
				this.dataGridView1.Columns[5].Visible = false;
				this.dataGridView1.Columns[6].Visible = false;
				this.dataGridView1.Columns[7].Visible = false; 
				this.dataGridView1.Columns[8].Visible = false;
				this.dataGridView1.Columns[9].Visible = false;
                this.dataGridView1.Columns[10].Visible = false;
                this.dataGridView1.Columns[11].Visible = false;
            }
            else
            {
                this.dataGridView1.Columns[0].Visible = false;
				this.dataGridView1.Columns[1].Visible = false;
				this.dataGridView1.Columns[3].Visible = false;
				this.dataGridView1.Columns[11].Visible = false;
				this.dataGridView1.Columns[12].Visible = false;
				Util.CSharp.BindRatings(this.MvProject.ModelName, ratingDataGridViewComboBoxColumn);
            }
        }


        private void AddGroup()
        {
            dataGridView1.EndEdit();
			int count = 0;
			if (!int.TryParse(tstb.Text, out count))
			{
				MessageBox.Show("그룹 수량이 정수가 아닙니다.");
				tstb.Focus();
				return;
			}

            count++;
            tstb.Text = string.Format("{0}", count);
            for (int i = dataGridView1.Rows.Count; i < count; i++)
            {
                this.MvProject.AddGroup();
            }
        }

        private void DeleteGroup()
        {
			if (dataGridView1.SelectedCells.Count > 0)
			{
				foreach (DataGridViewCell cell in this.dataGridView1.SelectedCells)
				{
					dataGridView1.Rows[cell.RowIndex].Selected = true;
				}
			}

            if (dataGridView1.SelectedRows.Count < 1)
            {
                MessageBox.Show("삭제할 그룹을 선택하고 실행하십시오...");
                return;
            }

            DialogResult dr = MessageBox.Show(dataGridView1.SelectedRows.Count.ToString() + "개의 그룹을 삭제합니다...", "Warning", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    dataGridView1.Rows.Remove(row);
                }
            }

            tstb.Text = dataGridView1.Rows.Count.ToString();
        }


        private void AddFeeders()
        {
            dataGridView1.EndEdit();
            foreach (DataRow row in MvProject.GroupTable.Rows)
            {
                Data.MvGroup group = new Data.MvGroup(this.MvProject, row);

				if (group.GroupNo == null || group.GroupNo.Length < 1)
				{
					if (this.MvProject.IsShip)
					{
						row["GroupNo"] = "Group";
					}
					else
					{
						MessageBox.Show("Group No를 입력하십시오");
						return;
					}
				}
                group.CreateFeeders();
            }
        }

		private bool IsGroupFilled()
		{
			foreach (DataGridViewRow row in dataGridView1.Rows)
			{
				if (row.Cells["GroupNo"].Value == null || row.Cells[1].Value.ToString().Length < 1)
				{
					MessageBox.Show("그룹 번호를 입력하십시오.");
					return false;
				}
			}

			return true;
		}

        #endregion

        #region Event Handler
        private void GroupInfo_Load(object sender, EventArgs e)
        {
            BindControls();

            if (this.MvProject.IsShip)
            {
                toolStripLabel1.Visible = false;
                tstb.Visible = false;
                tsb_AddGroup.Visible = false;
                tsb_DeleteGroup.Visible = false;
                if (this.MvProject.GroupTable.Rows.Count < 1 && dataGridView1.Rows.Count < 1)
                {
                    this.AddGroup();
                }
            }
            else
            {
                toolStripLabel1.Visible = true;
                tstb.Visible = true;
                tsb_AddGroup.Visible = true;
                tsb_DeleteGroup.Visible = true;
                tstb.Text = this.MvProject.GroupTable.Rows.Count.ToString();
            }
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Util.CSharp.ShowRowHeaderNumber(dataGridView1, e);
        }

        private void tstb_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void tsb_AddGroup_Click(object sender, EventArgs e)
        {
            this.AddGroup();
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            ComboBox comboBox = e.Control as ComboBox;
            if (comboBox != null)
            {
                comboBox.DropDownStyle = ComboBoxStyle.DropDown;
                comboBox.AutoCompleteMode = AutoCompleteMode.Append;
                comboBox.KeyUp += new KeyEventHandler(comboBox_KeyUp);
            }
        }

        void comboBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                dataGridView1.EndEdit();
            }
        }

        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridView1.IsCurrentCellDirty)
            {
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void 그룹추가ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.AddGroup();
        }

        private void 선택된그룹삭제ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.DeleteGroup();
        }

        private void feeder생성ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.AddFeeders();
        }

        private void 셀복사ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Util.CSharp.CopyCell(dataGridView1);
        }

        private void 연속데이터채우기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Util.CSharp.FillContinuousData(dataGridView1);
        }

        private void tsb_DeleteGroup_Click(object sender, EventArgs e)
        {
            this.DeleteGroup();
        }

        private void tsb_CreateFeeder_Click(object sender, EventArgs e)
        {
			this.dataGridView1.EndEdit();

			foreach (DataRow row in this.MvProject.GroupTable.Rows)
			{
				if (row["IncomingType"] == null || row["IncomingType"].ToString().Length < 1)
				{
					MessageBox.Show("Incoming Type을 선택하십시오...");
					return;
				}
			}
			

            this.AddFeeders();
			dataGridView1.Invalidate();
        }

        #endregion

    }
}
