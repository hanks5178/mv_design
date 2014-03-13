using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Eplan.EplApi.DataModel;

namespace Eplan.EplAddin.MvDesign.UI.Land
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
            BindControls();
        }

        private void BindControls()
        {
            Util.CSharp.ReadTemplates(comboBox_EplanTemplates);
			Util.CSharp.ReadPlotFrames(comboBox_FrameName);

            Util.CSharp.BindRatings(this.MvProject.ModelName, comboBox_Rating);
            Util.CSharp.BindComboBox(this.MvProject, this.comboBox_ModelName, "ModelName");

            Util.CSharp.BindTextBox(this.MvProject, this.textBox_FileName, "FileName");

            Util.CSharp.BindTextBox(this.MvProject.LandInfo, this.textBox_ApprovedBy, "ApprovedBY");
            Util.CSharp.BindTextBox(this.MvProject.LandInfo, this.textBox_CadNo, "CadNo");
            Util.CSharp.BindTextBox(this.MvProject.LandInfo, this.textBox_CheckedBy, "CheckedBY");
            Util.CSharp.BindTextBox(this.MvProject.LandInfo, this.textBox_Customer, "Customer");
            Util.CSharp.BindTextBox(this.MvProject.LandInfo, this.textBox_DesignedBy, "DesignedBy");
            Util.CSharp.BindTextBox(this.MvProject.LandInfo, this.textBox_DrawingNo, "DrawingNo");
            Util.CSharp.BindTextBox(this.MvProject.LandInfo, this.textBox_Equipment, "Equipment");
            Util.CSharp.BindTextBox(this.MvProject.LandInfo, this.textBox_ProjectName, "ProjectName");
            Util.CSharp.BindTextBox(this.MvProject.LandInfo, this.textBox_ProjectNo, "ProjectNo");

            Util.CSharp.BindComboBox(this.MvProject.LandInfo, this.comboBox_EplanTemplates, "EplanTemplate");
            Util.CSharp.BindComboBox(this.MvProject.LandInfo, this.comboBox_FrameName, "FrameName");
            Util.CSharp.BindComboBox(this.MvProject.LandInfo, this.comboBox_Rating, "Rating");

            Util.CSharp.BindNumericUpDown(this.MvProject.LandInfo, this.numericUpDown_MarginRight, "MarginRight");
            Util.CSharp.BindNumericUpDown(this.MvProject.LandInfo, this.numericUpDown_MarginBottom, "MarginBottom");

			return;
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
