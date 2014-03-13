using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Eplan.EplAddin.MvDesign.UI.Ship
{
    public partial class ProjectInfo : UserControl
    {
        public Data.MvProject MvProject;

        public ProjectInfo(Data.MvProject mvProject)
        {
            InitializeComponent();
            this.MvProject = mvProject;
        }

        private void ProjectInfo_Load(object sender, EventArgs e)
        {
            this.BindControls();
        }

        private void BindControls()
        {
            Util.CSharp.ReadTemplates(this.comboBox_EplanTemplates);
			Util.CSharp.ReadPlotFrames(comboBox_FrameName);

            comboBox_Yard.DisplayMember = "Key";
            comboBox_Yard.ValueMember = "Value";
            comboBox_Yard.DataSource = new BindingSource(this.MvProject.ShipInfo.YardList, null);

			if (this.MvProject.ShipInfo.Class != null)
			{
				this.MvProject.ShipInfo.Class = this.MvProject.ShipInfo.Class.Replace("\n", "\r\n");
			}

			if (this.MvProject.ShipInfo.ProjectNo != null)
			{
				this.MvProject.ShipInfo.ProjectNo = this.MvProject.ShipInfo.ProjectNo.Replace("\n", "\r\n");
			}

			Util.CSharp.BindTextBox(this.MvProject, this.textBox_FileName, "FileName");

            Util.CSharp.BindTextBox(this.MvProject.ShipInfo, this.textBox_ApprovedBy, "ApprovedBy");
            Util.CSharp.BindTextBox(this.MvProject.ShipInfo, this.textBox_CheckedBy, "CheckedBy");
            Util.CSharp.BindTextBox(this.MvProject.ShipInfo, this.textBox_ProjectNo, "ProjectNo");
            Util.CSharp.BindTextBox(this.MvProject.ShipInfo, this.textBox_DesignedBy, "DesignedBy");
            Util.CSharp.BindTextBox(this.MvProject.ShipInfo, this.textBox_DrawingNo, "DrawingNo");
            Util.CSharp.BindTextBox(this.MvProject.ShipInfo, this.textBox_Code, "Code");
            Util.CSharp.BindTextBox(this.MvProject.ShipInfo, this.textBox_PanelName, "PanelName");
            Util.CSharp.BindTextBox(this.MvProject.ShipInfo, this.textBox_ProjectNo, "ProjectNo");
            Util.CSharp.BindTextBox(this.MvProject.ShipInfo, this.textBox_Class, "Class");
            Util.CSharp.BindTextBox(this.MvProject.ShipInfo, this.textBox_ShipNo, "ShipNo");

            Util.CSharp.BindComboBox(this.MvProject.ShipInfo, this.comboBox_EplanTemplates, "EplanTemplate");
            Util.CSharp.BindComboBox(this.MvProject.ShipInfo, this.comboBox_FrameName, "FrameName");
            Util.CSharp.BindComboBox(this.MvProject.ShipInfo, this.comboBox_Rating, "Rating");
            Util.CSharp.BindComboBox(this.MvProject.ShipInfo, this.comboBox_Yard, "Yard");

            numericUpDown_MarginBottom.DataBindings.Add(new Binding("Value", this.MvProject.ShipInfo, "MarginBottom", true, DataSourceUpdateMode.OnPropertyChanged));
            numericUpDown_MarginRight.DataBindings.Add(new Binding("Value", this.MvProject.ShipInfo, "MarginRight", true, DataSourceUpdateMode.OnPropertyChanged));
        
        }

        private void SetDrawingNo()
        {
            string s = this.MvProject.FileName =
                comboBox_Yard.Text + "-" +
                textBox_ShipNo.Text + "-" +
                textBox_Code.Text;

            textBox_FileName.Text = textBox_DrawingNo.Text = s;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.SetDrawingNo();
        }

		private void button_Apply_Click(object sender, EventArgs e)
		{
			Eplan.EplApi.DataModel.Project p = Util.eplan.GetEplanProject(this.MvProject.FileName);
			if (p != null)
			{
				Util.eplan.ApplyPlotFrame(p, comboBox_FrameName.Text);
			}
		}
    }
}
