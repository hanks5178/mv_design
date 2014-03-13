using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Eplan.EplApi.DataModel;
using Eplan.EplApi.DataModel.Graphics;
using Eplan.EplApi.DataModel.MasterData;

namespace Eplan.EplAddin.MvDesign.Drawings
{
    class Cover
    {
        public Data.MvProject MvProject;
        public string MacroFolder { get; set; }

        public Cover(Data.MvProject mvProject)
        {
            this.MvProject = mvProject;

            foreach (Placement p in this.MvProject.Project.Pages[0].AllGraphicalPlacements)
            {
                p.Remove();
            }

            this.MacroFolder = Data.Location.Macro(this.MvProject.IsShip);
            if (this.MvProject.IsShip)
            {
                this.CreateShip(this.MvProject.Project.Pages[0]);
            }
            else
            {
                this.CreateLand(this.MvProject.Project.Pages[0]);
            }
        }

        public void CreateLand(Page page)
        {
			try
			{
				Util.eplan.InsertWindowMacro(page, this.MacroFolder + @"\COVER_TITLE.EMA", 0, 0);

				Text text = Util.eplan.WriteText(this.MvProject.Project.Pages[0], this.MvProject.LandInfo.ProjectNo, 128, 230);
				if (text != null) text.Height = 10;

				text = Util.eplan.WriteText(this.MvProject.Project.Pages[0], this.MvProject.LandInfo.ProjectName, 128, 204);
				if (text != null) text.Height = 10;

				text = Util.eplan.WriteText(this.MvProject.Project.Pages[0], this.MvProject.LandInfo.Customer, 128, 178);
				if (text != null) text.Height = 10;

				text = Util.eplan.WriteText(this.MvProject.Project.Pages[0], this.MvProject.LandInfo.Equipment, 128, 152);
				if (text != null) text.Height = 10;

				text = Util.eplan.WriteText(this.MvProject.Project.Pages[0], this.MvProject.LandInfo.DesignedBy, 314, 40);
				text = Util.eplan.WriteText(this.MvProject.Project.Pages[0], this.MvProject.LandInfo.CheckedBy, 354, 40);
				text = Util.eplan.WriteText(this.MvProject.Project.Pages[0], this.MvProject.LandInfo.ApprovedBy, 394, 40);

				page.Project.Properties.PROJ_INSTALLATIONNAME = this.MvProject.LandInfo.ProjectName;
				page.Project.Properties.PROJ_CREATORID = this.MvProject.LandInfo.DesignedBy;
				page.Project.Properties.PROJ_REVISION_CHECKEDBY = this.MvProject.LandInfo.CheckedBy;
				page.Project.Properties.PROJ_REVISION_APPROVEDBY = this.MvProject.LandInfo.ApprovedBy;
			}
			catch (Exception ex)
			{
				MessageBox.Show("ERROR: " + ex.Message + "\n" + ex.StackTrace);
			}
        }


        public void CreateShip(Page page)
        {
			try
			{
				Util.eplan.InsertWindowMacro(page, this.MacroFolder + @"\COVER_TITLE.EMA", 0, 0);

				Text text = Util.eplan.WriteText(this.MvProject.Project.Pages[0], this.MvProject.ShipInfo.ShipNo, 190, 196);
				if (text != null) text.Height = 10;

				text = Util.eplan.WriteText(this.MvProject.Project.Pages[0], this.MvProject.ShipInfo.PanelName, 190, 164);
				if (text != null) text.Height = 10;

				text = Util.eplan.WriteText(this.MvProject.Project.Pages[0], this.MvProject.ShipInfo.DesignedBy, 270, 34);
				if (text != null) text.Height = 4;
				text = Util.eplan.WriteText(this.MvProject.Project.Pages[1], this.MvProject.ShipInfo.DesignedBy, 136, 248);
				if (text != null) text.Justification = TextBase.JustificationType.MiddleCenter;

				text = Util.eplan.WriteText(this.MvProject.Project.Pages[0], this.MvProject.ShipInfo.CheckedBy, 270, 50);
				if (text != null) text.Height = 4;
				text = Util.eplan.WriteText(this.MvProject.Project.Pages[1], this.MvProject.ShipInfo.CheckedBy, 160, 248);
				if (text != null) text.Justification = TextBase.JustificationType.MiddleCenter;

				text = Util.eplan.WriteText(this.MvProject.Project.Pages[0], this.MvProject.ShipInfo.ApprovedBy, 270, 66);
				if (text != null) text.Height = 4;
				text = Util.eplan.WriteText(this.MvProject.Project.Pages[1], this.MvProject.ShipInfo.ApprovedBy, 184, 248);
				if (text != null) text.Justification = TextBase.JustificationType.MiddleCenter;

				string date = DateTime.Now.ToString("yyyy-MM-dd");
				text = Util.eplan.WriteText(this.MvProject.Project.Pages[1], date, 34, 248);
				if (text != null) text.Justification = TextBase.JustificationType.MiddleCenter;

				if (this.MvProject.ShipInfo.Yard != null)
				{
					string yard = this.MvProject.ShipInfo.YardList[this.MvProject.ShipInfo.Yard];
					text = Util.eplan.WriteText(this.MvProject.Project.Pages[0], yard, 218, 80);
					text.Height = 5.5;
				}

				text = Util.eplan.WriteText(this.MvProject.Project.Pages[0], this.MvProject.ShipInfo.Class, 370, 67);
				if (text != null)
				{
					text.Height = 4;
					text.Justification = TextBase.JustificationType.MiddleLeft;
				}

				text = Util.eplan.WriteText(this.MvProject.Project.Pages[0], this.MvProject.ShipInfo.DrawingNo, 370, 50);
				if (text != null) text.Height = 4;
				text = Util.eplan.WriteText(this.MvProject.Project.Pages[0], this.MvProject.ShipInfo.ProjectNo, 370, 36);
				if (text != null)
				{
					text.Height = 4;
					text.Justification = TextBase.JustificationType.MiddleLeft;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("ERROR: " + ex.Message + "\n" + ex.StackTrace);
			}
        }
    }
}
