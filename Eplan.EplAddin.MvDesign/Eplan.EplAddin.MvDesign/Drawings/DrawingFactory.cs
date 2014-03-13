using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;

using Eplan.EplApi.Base;
using Eplan.EplApi.ApplicationFramework;
using Eplan.EplApi.DataModel;
using Eplan.EplApi.DataModel.Graphics;
using Eplan.EplApi.DataModel.MasterData;
using Eplan.EplApi.HEServices;

namespace Eplan.EplAddin.MvDesign.Drawings
{
    public class DrawingFactory
    {
        public Data.MvProject MvProject { get; set; }
        public string SingleMacroFolder { get; set; }
		public List<string> FolderList { get; set; }

		private NLog.Logger _logger = AddInModule.GetUniqueLogger();

		private string GetSinglelineMacroPath(string feederType)
		{
			foreach (string name in this.FolderList)
			{
				if (feederType.StartsWith(name))
				{
					return this.SingleMacroFolder + name + "\\" + name + ".ema";
				}
			}

			return null;
		}

		public double SetSinglelineX(string groupNo, string feederNo, double x)
		{
			x = 0;
			try
			{
				DataRow[] row = this.MvProject.FeederTable.Select("GroupNo = '" + groupNo + "' AND FeederNo = '" + feederNo + "'");
				if (row.Length > 0)
				{
					row[0]["SinglelineX"] = x;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("ERROR: Feeder No가 없습니다\n" + ex.Message + "\n" + ex.StackTrace);
			}
			return x;
		}

        public DrawingFactory(Data.MvProject mvProject)
        {
            this.MvProject = mvProject;

            foreach (DataRow groupRow in this.MvProject.GroupTable.Rows)
            {
                Data.MvGroup group = new Data.MvGroup(this.MvProject, groupRow);

				this.SingleMacroFolder = Data.Location.Single(this.MvProject.IsShip, this.MvProject.ModelName, group.Rating);
				this.FolderList = Util.CSharp.GetDirectoryList(this.SingleMacroFolder);

                DataRow[] feederRows = this.MvProject.FeederTable.Select("GroupID = '" + group.GroupID.ToString() + "'", "PanelX ASC");

                List<Data.MvFeeder> feederList = new List<Data.MvFeeder>();
                foreach (DataRow feederRow in feederRows)
                {
                    Data.MvFeeder feeder = new Data.MvFeeder(feederRow);
                    feederList.Add(feeder);
                }

                if (this.MvProject.IsShip)
                {
                    this.CreateShipSingleLine(feederList);
                }
                else
                {
                    this.CreateLandSingleLine(group.GroupNo, group.Rating, feederList);
                }
            }
        }

        private StorableObject[] AddSingleLineMacro(Page page, string macroPath, double x, double y)
        {
            StorableObject[] sos = Util.eplan.AddSinglelineMacro(page, macroPath, 0, x, y);

            foreach (StorableObject so in sos)
            {
                Function function = so as Function;
                if (function == null) continue;

                function.IsMainFunction = false;
            }

            return sos;
        }

		private Group CreateGroup(StorableObject[] sos)
		{
			List<Placement> list = new List<Placement>();
			foreach (StorableObject s in sos)
			{
				Placement p = s as Placement;
				if (p != null) list.Add(p);

			}

			Group group = new Group();
			group.Create(list.ToArray());

			return group;
		}


        private void CreateShipSingleLine(List<Data.MvFeeder> feederList)
        {
            int index = 0;
            Page page = null;
            string macroPath = string.Empty;
            foreach (Data.MvFeeder feeder in feederList)
            {
                if (index == 0)
                {
                    page = Util.eplan.AddShipSinglelinePage(this.MvProject.Project, "VOL.4", "", "SINGLE", "SINGLELINE DIAGRAM");
                    macroPath = Data.Location.Macro(true) + @"\load1_frame.ema";
					// Insert Load Frame
                    StorableObject[] sos = Util.eplan.AddSinglelineMacro(page, macroPath, 0, 0, 0);
					this.CreateGroup(sos);
                }

                double x = 45 + index * 120;
				double y = 44;

				this.SetSinglelineX(feeder.GroupNo, feeder.FeederNo, x);
				macroPath = this.GetSinglelineMacroPath(feeder.FeederType);

                Util.eplan.AddSinglelineMacro(true, page, macroPath, feeder, x, y);
                index = IncreaseIndex(index);

                Page threelinePage = Util.eplan.AddShipThreelinePage(this.MvProject.Project, "VOL.5", feeder.FeederNo, "THREE", "THREELINE DIAGRAM");

				macroPath = macroPath.ToUpper().Replace("\\SINGLE\\", "\\THREE\\");
				Util.eplan.AddThreelineMacro(true, threelinePage, macroPath, feeder, 0, 0);
            }
        }

        private int IncreaseIndex(int index)
        {
            if (index == 2)
            {
                index = 0;
            }
            else
            {
                index++;
            }
            return index;
        }

        private void CreateLandSingleLine(string groupNo, string rating, List<Data.MvFeeder> feederList)
        {
            int index = 0;
            Page page = null;
            string macroPath = string.Empty;
            foreach (Data.MvFeeder feeder in feederList)
            {
                if (index == 0)
                {
                    page = Util.eplan.AddLandSinglelinePage(this.MvProject.Project,groupNo, "OV", "G", "SINGLE LINE", null);
                    macroPath = Data.Location.Macro(false) + @"\load1_frame.ema";
                    StorableObject[] sos = Util.eplan.AddSinglelineMacro(page, macroPath, 0, 0, 0);
					this.CreateGroup(sos);
                }

                double x = 45 + index * 120;
                double y = 44;

				this.SetSinglelineX(feeder.GroupNo, feeder.FeederNo, x);
				macroPath = this.GetSinglelineMacroPath(feeder.FeederType);
                Util.eplan.AddSinglelineMacro(false, page, macroPath, feeder, x, y);
                index = this.IncreaseIndex(index);

                Page threeline_page = Util.eplan.AddLandThreelinePage(this.MvProject.Project, groupNo, feeder.FeederNo, "R", "THREELINE", feeder.PanelName1);

				_logger.Info("MacroPath = " + macroPath);
				macroPath = macroPath.ToUpper().Replace("\\SINGLE\\", "\\THREE\\");
				Util.eplan.AddThreelineMacro(false, threeline_page, macroPath, feeder, 0, 0);
            }
        }
    }
}
