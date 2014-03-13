using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Eplan.EplAddin.MvDesign.Data.Ship
{
    [Serializable]
    public class ShipInfo
    {
        #region Properties

        public string ProjectNo { get; set; }
        public string DrawingNo { get; set; }
        public string Rating { get; set; }
        public string ApprovedBy { get; set; }
        public string CheckedBy { get; set; }
        public string DesignedBy { get; set; }
        public string EplanTemplate { get; set; }
        public string FrameName { get; set; }

        public double MarginRight { get; set; }
        public double MarginBottom { get; set; }

        public string PanelName { get; set; }
        public string Yard { get; set; }
        public string ShipNo { get; set; }
        public string Code { get; set; }

        public string Class { get; set; }

        #endregion

        #region Constructors

        public ShipInfo()
        {
        }

        public ShipInfo(string projectNo)
        {
			this.MarginRight = 30;
            this.ProjectNo = projectNo;
            this.PanelName = "6.6KV MAIN SWITCHBOARD";
        }

        #endregion

		#region Yard List
		[XmlIgnore]
        public Dictionary<string, string> YardList
        {
            get
            {
                Dictionary<string, string> yardList = new Dictionary<string, string>();
                yardList.Add("HI", "HYUNDAI HEAVY INDUSTRIES CO., LTD");
                yardList.Add("SH", "HYUNDAI SAMHO HEAVY INDUSTRIES CO., LTD");
                yardList.Add("DW", "DAEWOO SHIPBUILDING & MARINE ENGINEERING CO., LTD");
                yardList.Add("SM", "SUNGDONG SHIPBUILDING & MARINE ENGINEERING CO., LTD");
                yardList.Add("HJ", "HANJIN HEAVY INDUSTRIES & CONSTRUCTION CO., LTD");
                yardList.Add("SPP", "SPP SHIPBUILDING CO., LTD");
                yardList.Add("STX", "STX OFFSHORE & SHIPBUILDINBG CO., LTD");

                return yardList;
            }
		}

		#endregion
	}
}
