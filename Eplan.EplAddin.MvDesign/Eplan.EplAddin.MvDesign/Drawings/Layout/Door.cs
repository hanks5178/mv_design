using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;

using Eplan.EplApi.DataModel;
using Eplan.EplApi.DataModel.MasterData;
using Eplan.EplApi.HEServices;

namespace Eplan.EplAddin.MvDesign.Drawings.Layout
{
    public class Door
    {
        public Data.MvProject MvProject { get; set; }

        public Door(Data.MvProject mvProject)
        {
            this.MvProject = mvProject;
            this.Create();
        }

        public void Create()
        {
            string macroFolder = Data.Location.Door(true, this.MvProject.ModelName, "");

            Data.MvGroup group = new Data.MvGroup(this.MvProject, this.MvProject.GroupTable.Rows[0]);
            DataRow[] rows = MvProject.FeederTable.Select("GroupID = '" + group.GroupID + "'", "PanelX ASC");

            foreach (DataRow row in rows)
            {
                Data.MvFeeder feeder = new Data.MvFeeder(row);
                string macroPath = macroFolder + feeder.FeederType + ".emp";
                Page page = Util.eplan.AddShipPanellayoutPage(this.MvProject.Project, macroPath, "VOL.2", feeder.FeederNo, "LAYOUT", "OUTLINE VIEW(" + feeder.PanelName1 + ")");

				if (page == null)
				{
					MessageBox.Show("ERROR: Inserting PageMacro: " + macroPath);
					continue;
				}
				else
				{
					page.Properties.PAGE_SCALE = 12.0;

					foreach (Placement p in page.Functions)
					{
						Function f = p as Function;
						if (f == null) continue;

						int index = f.Name.IndexOf('-');
						if (index >= 0)
						{
							f.Name = f.VisibleName = "=VOL.5+" + feeder.FeederNo + f.Name.Substring(f.Name.IndexOf('-'));
						}
						else
						{
							f.Name = f.VisibleName = "=VOL.5+" + feeder.FeederNo;
						}
						//f.VisibleName = "+" + feeder.FeederNo;
						Util.eplan.RemoveArticleReferences(f);
					}

					new NameService().EvaluateAndSetAllNames(page);
				}

            }
        }
    }
}
