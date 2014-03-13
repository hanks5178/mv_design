using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

using Eplan.EplApi.Base;
using Eplan.EplApi.DataModel;
using Eplan.EplApi.DataModel.Graphics;

namespace Eplan.EplAddin.MvDesign.Drawings.Layout
{
    public class Ship
    {
        public Data.MvProject MvProject { get; set; }
        public Project Project { get; set; }

        #region Properties

        private enum Split { Start, End, None }
        private PointD _origin = new PointD(1200, 5500);
        private double[] _dimOffset = new double[] { 1345, 4550, 4650 };
        private double _totalWidth = 0;
        private string _macroFolder = string.Empty;

        private string LayoutPage_emp
        {
            get
            {
                return this._macroFolder + @"\Decorator\layout_page.emp";
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

        private string FluorescentLight_ema
        {
            get
            {
                return this._macroFolder + @"\Decorator\fluorescent_light.ema";
            }
        }

        private string LayoutDefault_ema
        {
            get
            {
                return this._macroFolder + @"\Decorator\layout_default.ema";
            }
        }

        public PointD PageSize;
        public PointD PageMargin
        {
            get
            {
                return new PointD(420.0 - PageSize.X, 297.0 - PageSize.Y);
            }
            set
            {
                PageSize.X = 420.0 - value.X;
                PageSize.Y = 297.0 - value.Y;
            }
        }

        private bool _panelName2Exists = false;

        #endregion

        public Ship(Data.MvProject mvProject)
        {
            this.MvProject = mvProject;
            this.Project = MvProject.Project;

            this.PageMargin = new PointD(this.MvProject.ShipInfo.MarginRight, this.MvProject.ShipInfo.MarginBottom);
            _macroFolder = Data.Location.Front(true, this.MvProject.ModelName, "");

            foreach(DataRow row in this.MvProject.GroupTable.Rows)
            {
                Data.MvGroup group = new Data.MvGroup(this.MvProject, row);
                DataRow[] feederRows = MvProject.FeederTable.Select("GroupID = '" + group.GroupID + "'", "PanelX ASC");

                List<Data.MvFeeder> feederList = new List<Data.MvFeeder>();
                foreach(DataRow feederRow in feederRows)
                {
                    Data.MvFeeder feeder = new Data.MvFeeder(feederRow);
                    if (feeder.PanelName2.Trim().Length > 0)
                    {
                        _panelName2Exists = true;
                    }

                    feeder.Width =(int) Util.eplan.GetWidth(Project, this._macroFolder + feeder.FeederType + ".ema");
                    _totalWidth += feeder.Width;
                    feederList.Add(feeder);
                }

                if (!group.LeftToRight)
                {
                    feederList.Reverse();
                }

                this.Create(feederList);
            }
        }

        private Page AppendLayoutPage()
        {
            Page page = Util.eplan.AddShipGraphicPage(this.Project, "VOL.2", null, "LAYOUT", "OUTLINE VIEW(PANEL DIMENSION)");
            if (page != null)
            {
				page.Properties.PAGE_SCALE = 22.0;
				string macroPath = this._macroFolder + @"\Decorator\layout_background.ema";
				Util.eplan.AddGraphicMacro(page, macroPath, 0, 0, 0);
            }

            return page;
        }

        private void Create(List<Data.MvFeeder> feederList)
        {
            int[] feederSplit = GetFeederSplit(feederList.Count);
            if (feederSplit == null) return;

            Page page = AppendLayoutPage();
            if (page == null) return;


			Util.eplan.AddGraphicMacro(page, this.LeftDimension_ema, 0, _origin.X, _origin.Y);

            double locationX = _origin.X;
            double lastDimLocation = _origin.X;
            int index = 0;

            bool isFirstPage = true;
            bool sendToLeftHalf = false;

            for (int i = 0; i < feederSplit.Length; i++)
            {
                double splitWidth = 0;
                for (int j = index; j < index + feederSplit[i]; j++)
                {
                    splitWidth += feederList[j].Width;
                }

                for (int j = 0; j < feederSplit[i]; j++)
                {
                    double panelWidth = feederList[index].Width;
                    string macroPath = this._macroFolder + feederList[index].FeederType + ".ema";

                    double pageScale = page.Properties.PAGE_SCALE.ToDouble();

                    if (locationX + panelWidth > this.PageSize.X * pageScale)
                    {
						Util.eplan.AddGraphicMacro(page, this.RightHalf_ema, 0, locationX, _origin.Y);

                        Split split;
                        if (j == 0) split = Split.End;
                        else if (j == 1) split = Split.Start;
                        else split = Split.None;

                        AddPageEndingDimension(page, split, splitWidth, locationX, lastDimLocation, isFirstPage);
                        isFirstPage = false;

                        if (index % 2 != 0)
                        {
							Util.eplan.AddGraphicMacro(page, this.FluorescentLight_ema, 0, locationX, _origin.Y - 1720);
                        }


                        page = AppendLayoutPage();
						if (page == null) return;

                        locationX = _origin.X;
                        lastDimLocation = _origin.X;
						Util.eplan.AddGraphicMacro(page, this.LeftHalf_ema, 0, locationX, _origin.Y);
                        if (j == 0)
                        {
                            split = Split.Start;
                        }
                        else if (j == feederSplit[i] - 1)
                        {
                            split = Split.End;
                            sendToLeftHalf = true;
                        }
                        else
                        {
                            sendToLeftHalf = true;
                            split = Split.None;
                        }

                        AddPageStartingDimension(page, split, feederList[index].Width, splitWidth, locationX);
                    }

                    if (index % 2 != 0)
                    {
						Util.eplan.AddGraphicMacro(page, this.FluorescentLight_ema, 0, locationX, _origin.Y - 1720);
                    }

                    if (j == 0)
                    {
						Util.eplan.AddGraphicMacro(page, macroPath, 0, locationX, _origin.Y);
                    }
                    else if (j == feederSplit[i] - 1)
                    {
						Util.eplan.AddGraphicMacro(page, macroPath, 2, locationX, _origin.Y);
                        AddSplitEndingDimension(page, sendToLeftHalf, splitWidth, locationX + panelWidth, lastDimLocation);
                        sendToLeftHalf = false;

                        if (index < feederList.Count - 1)
                        {
							Util.eplan.AddGraphicMacro(page, this._macroFolder + @"\Decorator\triangle.ema", 0, locationX + panelWidth, _origin.Y);
                        }

                        lastDimLocation = locationX + panelWidth;
                    }
                    else
                    {
						Util.eplan.AddGraphicMacro(page, macroPath, 1, locationX, _origin.Y);
                    }

                    if (_panelName2Exists)
                    {
                        AddPanelName2(feederList, page, locationX, index, panelWidth);
                    }
                    else
                    {
                        Text text = Util.eplan.AddText(page, null, feederList[index].FeederNo, locationX + panelWidth / 2, _origin.Y - 168);
                        text.Height = 40;
                        text = Util.eplan.AddText(page, null, feederList[index].PanelName1, locationX + panelWidth / 2, _origin.Y - 168 - 320);
                        text.Height = 40;
                    }

                    if (index == feederList.Count - 1)
                    {
                        AddTotalDimension(page, locationX + panelWidth, true, false);
                    }

                    index++;
                    locationX += panelWidth;
                }
            }
        }

        private void AddPanelName2(List<Data.MvFeeder> feederList, Page page, double locationX, int index, double panelWidth)
        {
            Text text = Util.eplan.AddText(page, null, feederList[index].FeederNo, locationX + panelWidth / 2, _origin.Y - 168);
            text.Height = 40;
			//text = Util.eplan.AddText(page, null, feederList[index].PanelName2, locationX + panelWidth / 2, _origin.Y - 168 - 320);
			//text.Height = 40;
			//text = Util.eplan.AddText(page, null, feederList[index].PanelName1, locationX + panelWidth / 2, _origin.Y - 168 - 320 - 320);
			//text.Height = 40;

            Line line = Util.eplan.DrawLine(page, new PointD(locationX, _origin.Y - 630), new PointD(locationX, _origin.Y - 630 - 320));
            line.Width = 0.02;
            line = Util.eplan.DrawLine(page, new PointD(locationX + panelWidth, _origin.Y - 630), new PointD(locationX + panelWidth, _origin.Y - 630 - 320));
            line.Width = 0.02;
            line = Util.eplan.DrawLine(page, new PointD(locationX, _origin.Y - 630 - 13), new PointD(locationX + panelWidth, _origin.Y - 630 - 13));
            line.Width = 0.02;
            line = Util.eplan.DrawLine(page, new PointD(locationX, _origin.Y - 630 - 320), new PointD(locationX + panelWidth, _origin.Y - 630 - 320));
            line.Width = 0.02;
        }



        private void AddSplitEndingDimension(Page page, bool sendToLeftHalf, double splitWidth, double locationX, double lastDimLocation)
        {
            DimensionItem item;

            if (sendToLeftHalf)
            {
                item = Util.eplan.AddDimension(
                    page,
                    null,
                    splitWidth - 1000,
                    lastDimLocation - 420,
                    _origin.Y - _dimOffset[0],
                    locationX - 500,
                    _origin.Y - _dimOffset[0],
                    20);
                item.ExtensionLineFirst = false;
                item.DimensionCalculated = false;
                item.DimensionLineTermination = DimensionItem.Enums.DimensionLineTermination.Without;
                item.DimensionText = string.Format("{0:F0}", splitWidth - 1000);

                item = Util.eplan.AddDimension(
                    page,
                    null,
                    splitWidth,
                    lastDimLocation - 210,
                    _origin.Y - _dimOffset[1],
                    locationX,
                    _origin.Y - _dimOffset[1],
                    20);

                item.ExtensionLineFirst = false;
                item.DimensionCalculated = false;
                item.DimensionLineTermination = DimensionItem.Enums.DimensionLineTermination.Without;
                item.DimensionText = string.Format("{0:F0}", splitWidth);
            }
            else
            {
                Util.eplan.AddDimension(
                    page,
                    null,
                    splitWidth - 1000,
                    lastDimLocation + 500,
                    _origin.Y - _dimOffset[0],
                    lastDimLocation + splitWidth - 500,
                    _origin.Y - _dimOffset[0],
                    20);
                Util.eplan.AddDimension(
                    page,
                    null,
                    splitWidth,
                    lastDimLocation,
                    _origin.Y - _dimOffset[1],
                    lastDimLocation + splitWidth,
                    _origin.Y - _dimOffset[1],
                    20);
            }
        }

        private void AddPageEndingDimension(Page page, Split split, double splitWidth, double locationX, double lastDimensionX, bool isFirstPage)
        {
            DimensionItem item;

            if (split == Split.Start)
            {
                item = Util.eplan.AddDimension(
                    page,
                    null,
                    splitWidth - 1000,
                    lastDimensionX + 500,
                    _origin.Y - _dimOffset[0],
                    locationX + 420,
                    _origin.Y - _dimOffset[0],
                    20);
                item.ExtensionLineSecond = false;
                item.DimensionCalculated = false;
                item.DimensionLineTermination = DimensionItem.Enums.DimensionLineTermination.Without;
                item.DimensionText = string.Format("{0:F0}", splitWidth - 1000);

                item = Util.eplan.AddDimension(
                    page,
                    null,
                    splitWidth,
                    lastDimensionX,
                    _origin.Y - _dimOffset[1],
                    locationX + 210,
                    _origin.Y - _dimOffset[1],
                    20);
                item.ExtensionLineSecond = false;
                item.DimensionCalculated = false;
                item.DimensionLineTermination = DimensionItem.Enums.DimensionLineTermination.Without;
                item.DimensionText = splitWidth.ToString("F0");

            }
            else if (split == Split.End)
            {
                item = Util.eplan.AddDimension(
                    page,
                    null,
                    420,
                    locationX,
                    _origin.Y - _dimOffset[0],
                    locationX + 420,
                    _origin.Y - _dimOffset[0],
                    20);
                item.ExtensionLineSecond = false;
                item.DimensionCalculated = false;
                item.DimensionLineTermination = DimensionItem.Enums.DimensionLineTermination.Without;
                item.DimensionText = "";

                item = Util.eplan.AddDimension(
                    page,
                    null,
                    210,
                    lastDimensionX,
                    _origin.Y - _dimOffset[1],
                    locationX + 210,
                    _origin.Y - _dimOffset[1],
                    20);

                item.ExtensionLineSecond = false;
                item.DimensionCalculated = false;
                item.DimensionLineTermination = DimensionItem.Enums.DimensionLineTermination.Without;
                item.DimensionText = "";

            }
            else
            {
                item = Util.eplan.AddDimension(
                    page,
                    null,
                    splitWidth - 1000,
                    lastDimensionX + 500,
                    _origin.Y - _dimOffset[0],
                    locationX + 420,
                    _origin.Y - _dimOffset[0],
                    20);
                item.ExtensionLineSecond = false;
                item.DimensionCalculated = false;
                item.DimensionLineTermination = DimensionItem.Enums.DimensionLineTermination.Without;
                item.DimensionText = string.Format("{0:F0}", splitWidth - 1000);
                item = Util.eplan.AddDimension(
                     page,
                     null,
                     splitWidth,
                     lastDimensionX,
                     _origin.Y - _dimOffset[1],
                     locationX + 210,
                     _origin.Y - _dimOffset[1],
                     20);

                item.ExtensionLineSecond = false;
                item.DimensionCalculated = false;
                item.DimensionLineTermination = DimensionItem.Enums.DimensionLineTermination.Without;
                item.DimensionText = splitWidth.ToString("F0");
            }

            AddTotalDimension(page, locationX, !isFirstPage, true);
        }

        private DimensionItem AddTotalDimension(Page page, double locationX, bool indent_left, bool indent_right)
        {
            double x1 = _origin.X;
            double x2 = locationX;
            if (indent_left) x1 -= 240;
            if (indent_right) x2 += 240;

            DimensionItem item = Util.eplan.AddDimension(
                 page,
                 null,
                 _totalWidth,
                 x1,
                 _origin.Y - _dimOffset[2],
                 x2,
                 _origin.Y - _dimOffset[2],
                 20);

            item.ExtensionLineFirst = !indent_left;
            item.ExtensionLineSecond = !indent_right;
            item.DimensionCalculated = false;
            item.DimensionLineTermination = DimensionItem.Enums.DimensionLineTermination.Without;
            item.DimensionText = _totalWidth.ToString("F0");
            return item;
        }

        private void AddPageStartingDimension(Page page, Split split, double panelWidth, double splitWidth, double locationX)
        {
            DimensionItem item;
            if (split == Split.Start)
            {
                item = Util.eplan.AddDimension(
                    page,
                    null,
                    splitWidth - 1000,
                    locationX - 420,
                    _origin.Y - _dimOffset[0],
                    locationX,
                    _origin.Y - _dimOffset[0],
                    20);
				if (item == null) return;

                item.ExtensionLineFirst = false;
                item.DimensionCalculated = false;
                item.DimensionLineTermination = DimensionItem.Enums.DimensionLineTermination.Without;
                item.DimensionText = "";

                item  =Util.eplan.AddDimension(
                    page,
                    null,
                    splitWidth,
                    locationX - 210,
                    _origin.Y - _dimOffset[1],
                    locationX,
                    _origin.Y - _dimOffset[1],
                    20);

                item.ExtensionLineFirst = false;
                item.DimensionCalculated = false;
                item.DimensionLineTermination = DimensionItem.Enums.DimensionLineTermination.Without;
                item.DimensionText = "";
            }
            else if (split == Split.End)
            {
                //API.Common.AddDimension(
                //    page,
                //    null,
                //    splitWidth - 1000,
                //    locationX - 420,
                //    _origin.Y - _dimOffset1,
                //    locationX + panelWidth - 500,
                //    _origin.Y - _dimOffset1,
                //    20);
                //API.Common.AddDimension(
                //    page,
                //    null,
                //    splitWidth,
                //    locationX - 210,
                //    _origin.Y - _dimOffset2,
                //    locationX + panelWidth,
                //    _origin.Y - _dimOffset2,
                //    20);
            }
            else
            {
                //API.Common.AddDimension(
                //    page,
                //    null,
                //    splitWidth - 1000,
                //    dimensionX1 + 500,
                //    _origin.Y - _dimOffset1,
                //    locationX + 420,
                //    _origin.Y - _dimOffset1,
                //    20);
                //API.Common.AddDimension(
                //    page,
                //    null,
                //    splitWidth,
                //    dimensionX1,
                //    _origin.Y - _dimOffset2,
                //    locationX,
                //    _origin.Y - _dimOffset2,
                //    20);
                //API.Common.AddDimension(
                //    page,
                //    null,
                //    _TotalWidth,
                //    _origin.X,
                //    _origin.Y - _dimOffset3,
                //    locationX,
                //    _origin.Y - _dimOffset3,
                //    20);
            }
        }


        private int[] GetFeederSplit(int count)
        {
            switch (count)
            {
                case 16:
                    return new int[] { 4, 4, 4, 4 };
                case 17:
                    return new int[] { 4, 4, 4, 5 };
                case 18:
                    return new int[] { 3, 4, 4, 4, 3 };
                case 19:
                    return new int[] { 4, 4, 4, 4, 3 };
                case 20:
                    return new int[] { 4, 4, 4, 4, 4 };
                case 21:
                    return new int[] { 4, 4, 4, 4, 5 };
                case 22:
                    return new int[] { 5, 4, 4, 4, 5 };
                default:
                    {
                        MessageBox.Show("Panel 수량이 16~22개가 아닙니다.");
                        return null;
                    }
            }
        }
    }

}
