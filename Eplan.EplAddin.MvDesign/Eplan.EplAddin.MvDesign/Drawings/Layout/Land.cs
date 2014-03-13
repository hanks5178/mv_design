using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using Eplan.EplApi.Base;
using Eplan.EplApi.ApplicationFramework;
using Eplan.EplApi.DataModel;
using Eplan.EplApi.DataModel.Graphics;
using Eplan.EplApi.DataModel.MasterData;
using Eplan.EplApi.HEServices;

namespace Eplan.EplAddin.MvDesign.Drawings.Layout
{
    public class Land
    {
        public Data.MvProject MvProject { get; set; }

        #region Properties
        private PointD PageSize;
        public PointD PageMargin
        {
            get
            {
                return new PointD(420 - PageSize.X, 297 - PageSize.Y);
            }
            set
            {
                PageSize.X = 420 - value.X;
                PageSize.Y = 297 - value.Y;
            }
        }

        private PointD _origin = new PointD(900, 6800);
        private string _macroFolder { get; set; }

		private string pageDescription;

        private string LayoutBackground_ema
        {
            get
            {
                return this._macroFolder + @"\Decorator\layout_background.ema";
            }
        }

        private string LeftDimension_ema
        {
            get
            {
                return this._macroFolder + @"\Decorator\left_dimension.ema";
            }
        }
        private string RightDimension_ema
        {
            get
            {
                return this._macroFolder + @"\Decorator\right_dimension.ema";
            }
        }

        private string LeftHalf_ema
        {
            get
            {
                return this._macroFolder + @"\Decorator\left_half.ema";
            }
        }

        private string RightHalf_ema
        {
            get
            {
                return this._macroFolder + @"\Decorator\right_half.ema";
            }
        }

        private string BusDuctSize_ema
        {
            get
            {
                return this._macroFolder + @"\Decorator\bus_duct_";
            }

        }

        double[] _dimOffset = new double[] { 3650 };

        #endregion

        public Land(Data.MvProject mvProject)
        {
            this.MvProject = mvProject;

            this.PageMargin = new PointD(this.MvProject.LandInfo.MarginRight, this.MvProject.LandInfo.MarginBottom);

            foreach (DataRow row in this.MvProject.GroupTable.Rows)
            {
                Data.MvGroup group = new Data.MvGroup(this.MvProject, row);
                DataRow[] feederRows = this.MvProject.FeederTable.Select("GroupID = '" + group.GroupID + "'", "PanelX ASC");
                this._macroFolder = Data.Location.Front(false, this.MvProject.ModelName, group.Rating);

                List<Data.MvFeeder> feederList = new List<Data.MvFeeder>();
                foreach (DataRow feederRow in feederRows)
                {
                    Data.MvFeeder feeder = new Data.MvFeeder(feederRow);
					string macroPath = this._macroFolder + feeder.FeederType + ".ema";
                    feeder.Width = (int) Util.eplan.GetWidth(this.MvProject.Project, macroPath);
                    feederList.Add(feeder);
                }

                if (!group.LeftToRight)
                    feederList.Reverse();

                this.SetSplit(group.IncomingType, feederList);

				this.pageDescription = "PANEL DIMENSIONS(FRONT)";
                this.Create("FRONT", group.GroupNo, group.Rating, feederList, group.BusDuctExists);
				this.pageDescription = "PANEL DIMENSIONS(BOTTOM)";
				this.Create("BOTTOM", group.GroupNo, group.Rating, feederList, group.BusDuctExists);
            }
        }


        private void Create(string type, string groupNumber, string rating, List<Data.MvFeeder> feederList, bool busDuctExists)
        {
            SetConsts(type, rating);

            Page page = AppendLayoutPage(groupNumber, this.LayoutBackground_ema, type);

            double distance = _origin.X;
			Util.eplan.AddGraphicMacro(page, this.LeftDimension_ema, 0, distance, _origin.Y);

            double pageScale = page.Properties.PAGE_SCALE.ToDouble();

            double totalWidth = GetTotalWidth(feederList);

            int nVariant = 0; ;
            for (int i = 0; i < feederList.Count; i++)
            {
                string macroPath = this._macroFolder + feederList[i].FeederType + ".ema";
                nVariant = GetVariants(type, feederList, nVariant, i);

                if (i < feederList.Count - 1 && feederList[i].IsLeft.HasValue && feederList[i].IsLeft.Value)
                {
                    if (distance + feederList[i].Width + feederList[i + 1].Width > this.PageSize.X * pageScale - 580)
                    {
						page.PageType = DocumentTypeManager.DocumentType.PanelLayout;
                        InsertNewPage(type, groupNumber, ref page, ref distance, totalWidth, nVariant);
                    }
                }

                if (distance + feederList[i].Width > this.PageSize.X * pageScale - 580)
                {
					page.PageType = DocumentTypeManager.DocumentType.PanelLayout;
					InsertNewPage(type, groupNumber, ref page, ref distance, totalWidth, nVariant);
                }

                AddLandPanel(page, macroPath, nVariant, distance, _origin.Y, feederList[i]);
                InsertBusDuct(feederList, busDuctExists, page, distance, macroPath, nVariant, i);

                distance += feederList[i].Width;
                if (i == feederList.Count - 1)
                {
                    AddTotalDimension(page, totalWidth, _origin.X, distance);
                }
            }

			page.PageType = DocumentTypeManager.DocumentType.PanelLayout;
        }

        private void InsertNewPage(string type, string groupNumber, ref Page page, ref double distance, double totalWidth, int nVariant)
        {
            AddTotalDimension(page, totalWidth, _origin.X, distance);

			Util.eplan.AddGraphicMacro(page, this.RightHalf_ema, nVariant, distance, _origin.Y);
            page = AppendLayoutPage(groupNumber, this.LayoutBackground_ema, type);
            distance = _origin.X;
			Util.eplan.AddGraphicMacro(page, this.LeftHalf_ema, nVariant, distance, _origin.Y);
        }



        private Page AppendLayoutPage(string groupNumber, string macroName, string name)
        {
            Page page = Util.eplan.AddLandGraphicPage(this.MvProject.Project, groupNumber, "OV", "L", "LAYOUT", this.pageDescription);
			if (page != null)
			{
				Util.eplan.AddGraphicMacro(page, macroName, 0, 0, 0);
			}

            return page;
        }


        private void AddTotalDimension(Page page, double width, double startX, double endX)
        {
            DimensionItem item = Util.eplan.AddDimension(page, null, width, startX, _dimOffset[0], endX, _dimOffset[0], 20);
            if (item != null)
            {
                item.DimensionCalculated = false;
                item.DimensionText = string.Format("{0:F0}", width);
                item.DimensionLineTermination = DimensionItem.Enums.DimensionLineTermination.Without;
            }
        }


        private void AddLandPanel(Page page, string macroPath, int nVariant, double x, double y, Data.MvFeeder feeder)
        {
			StorableObject[] sos = Util.eplan.AddGraphicMacro(page, macroPath, nVariant, x, _origin.Y);
			foreach (StorableObject so in sos)
			{
				BoxedDevice bd = so as BoxedDevice;
				if (bd == null) continue;

				bd.Name = bd.Name.Substring(0, bd.Name.IndexOf("=") + 1) + feeder.FeederNo + bd.Name.Substring(bd.Name.IndexOf("+"));
				break;
			}
        }


        private void InsertBusDuct(List<Data.MvFeeder> feederList, bool busDuctExists, Page page, double distance, string macroPath, int nVariant, int i)
        {
            if (busDuctExists)
            {
                if (feederList[i].FeederType.ToUpper().StartsWith("INCOMG A") || feederList[i].FeederType.ToUpper().StartsWith("INCOMG B"))
                {
                    macroPath = this.BusDuctSize_ema + feederList[i].Width.ToString("F0") + ".ema";
					Util.eplan.AddGraphicMacro(page, macroPath, nVariant, distance, _origin.Y);
                }
            }
            return;
        }

        private static int GetVariants(string type, List<Data.MvFeeder> feederList, int nVariant, int i)
        {

            if (type == "FRONT")
            {
                if (!feederList[i].IsLeft.HasValue)
                    nVariant = 1;
                else if (feederList[i].IsLeft.Value)
                    nVariant = 0;
                else
                    nVariant = 2;

                if (feederList[i].FeederType == "DUMMY") nVariant = 0;
            }
            else
            {
                nVariant = 0;
            }
            return nVariant;
        }

        private void SetConsts(string type, string rating)
        {
            if (type == "FRONT")
            {
                this._macroFolder = Data.Location.Front(this.MvProject.IsShip, this.MvProject.ModelName, rating);
                this._dimOffset[0] = 3650;
            }
            else
            {
                this._macroFolder = Data.Location.Bottom(this.MvProject.IsShip, this.MvProject.ModelName, rating);
                this._dimOffset[0] = 3650 + 384;
            }
        }

        private static double GetTotalWidth(List<Data.MvFeeder> feederList)
        {
            double totalWidth = 0;
            for (int i = 0; i < feederList.Count; i++)
            {
                totalWidth += feederList[i].Width;
            }
            return totalWidth;
        }

   
        #region Utility Functions

        private void SetSplit(string incomingType, List<Data.MvFeeder> feederList)
        {
            bool isLeft = true;
            if (incomingType.StartsWith("1"))
            {
                for (int i = 0; i < feederList.Count; i++)
                {
                    if (feederList[i].FeederType != "DUMMY")
                    {
                        feederList[i].IsLeft = isLeft;
                        isLeft = !isLeft;
                    }
                }

            }
            else
            {
                int index = -1;
                for (int i = 0; i < feederList.Count; i++)
                {
                    if (feederList[i].FeederType.ToUpper().StartsWith("BUS PT B"))
                    {
                        index = i;
                        break;
                    }
                }

                isLeft = false;
                for (int i = index - 1; i >= 0; i--)
                {
                    if (feederList[i].FeederType != "DUMMY")
                    {
                        feederList[i].IsLeft = isLeft;
                        isLeft = !isLeft;
                    }

                    if (i == 0)
                    {
                        if (!feederList[i].IsLeft.Value) feederList[i].IsLeft = null;
                    }
                }

                isLeft = true;
                for (int i = index + 1; i < feederList.Count; i++)
                {
                    if (feederList[i].FeederType != "DUMMY")
                    {
                        feederList[i].IsLeft = isLeft;
                        isLeft = !isLeft;
                    }

                    if (i == feederList.Count - 1)
                    {
                        if (feederList[i].IsLeft.Value) feederList[i].IsLeft = null;
                    }
                }
            }

            if (feederList[0].IsLeft.HasValue &&
                !feederList[0].IsLeft.Value)
                feederList[0].IsLeft = null;

            if (feederList[feederList.Count - 1].IsLeft.HasValue &&
                feederList[feederList.Count - 1].IsLeft.Value)
                feederList[feederList.Count - 1].IsLeft = null;

        }

        #endregion
    }
}
