using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Eplan.EplAddin.MvDesign.Data
{
    [Serializable]
    public class MvFeeder
    {
        public Data.MvGroup MvGroup { get; set; }

        public int FeederID { get; set; }
        public int GroupID { get; set; }

        public string GroupNo    { get; set; }
        public string FeederNo    { get; set; }
		public string FeederType { get; set; }
		public string PanelName1 { get; set; }
		public string PanelName2 { get; set; }
        public string PanelName3 { get; set; }
        public string ConsumerKW { get; set; }
        public string ConsumerA  { get; set; }

		public string Column01		 { get; set; }
		public string Column02		{ get; set; }
		public string Column03		{ get; set; }
		public string Column04		{ get; set; }
		public string Column05		{ get; set; }
		public string Column06		{ get; set; }
		public string Column07		{ get; set; }
		public string Column08		{ get; set; }
		public string Column09		{ get; set; }
		public string Column10		{ get; set; }
		public string Column11		{ get; set; }

        public int PanelX { get; set; }
        public int PanelY { get; set; }
        public bool IsSelected { get; set; }
        public bool IsNew { get; set; }
        public bool IsDone { get; set; }

		public int Width { get; set; }
		public double SinglelineX { get; set; }

        public bool? IsLeft { get; set; }

        [XmlIgnore]
        public Panel Control { get; set; }


        public MvFeeder(DataRow row)
        {
			this.FeederID = Convert.ToInt32(row["FeederID"]);
			this.GroupID = Convert.ToInt32(row["GroupID"]);
			this.GroupNo = Convert.ToString(row["GroupNo"]);
			this.FeederNo = Convert.ToString(row["FeederNo"]);
			this.FeederType = Convert.ToString(row["FeederType"]);
			this.PanelName1 = Convert.ToString(row["PanelName1"]);
			this.PanelName2 = Convert.ToString(row["PanelName2"]);
			this.PanelName3 = Convert.ToString(row["PanelName3"]);
			this.ConsumerKW = Convert.ToString(row["ConsumerKW"]);
			this.ConsumerA = Convert.ToString(row["ConsumerA"]);

			this.Column01 = Convert.ToString(row["Column01"]);
			this.Column02 = Convert.ToString(row["Column02"]);
			this.Column03 = Convert.ToString(row["Column03"]);
			this.Column04 = Convert.ToString(row["Column04"]);
			this.Column05 = Convert.ToString(row["Column05"]);
			this.Column06 = Convert.ToString(row["Column06"]);
			this.Column07 = Convert.ToString(row["Column07"]);
			this.Column08 = Convert.ToString(row["Column08"]);
			this.Column09 = Convert.ToString(row["Column09"]);
			this.Column10 = Convert.ToString(row["Column10"]);
			this.Column11 = Convert.ToString(row["Column11"]);

			this.PanelX = Convert.ToInt32(row["PanelX"]);
			this.PanelY = Convert.ToInt32(row["PanelY"]);

			this.IsSelected = Convert.ToBoolean(row["IsSelected"]);
			this.IsNew = Convert.ToBoolean(row["IsNew"]);
			this.IsDone = Convert.ToBoolean(row["IsDone"]);

			this.SinglelineX = Convert.ToDouble(row["SinglelineX"]);

        }


        public MvFeeder(int groupID, string groupNo, string name)
        {
            this.GroupID = groupID;
            this.GroupNo = groupNo;

            this.FeederType  = name;
            this.PanelName1 = name;

            this.PanelX = -1;
            this.PanelY = -1;
        }

        public MvFeeder(MvGroup group, string name)
            :this(group.GroupID, group.GroupNo, name)
        {
            this.MvGroup = group;
        }

		public DataRow DataRow()
		{
			DataRow row = this.MvGroup.MvProject.FeederTable.NewRow();

			//row["FeederID"] = this.FeederID;
			row["GroupID"] = this.GroupID;
			row["GroupNo"] = this.GroupNo;

			row["FeederNo"] = this.FeederNo;
			row["FeederType"] = this.FeederType;
			row["PanelName1"] = this.PanelName1;
			row["PanelName2"] = this.PanelName2;
			row["PanelName3"] = this.PanelName3;
			row["ConsumerKW"] = this.ConsumerKW;
			row["ConsumerA"] = this.ConsumerA;

			row["Column01"] = this.Column01;
			row["Column02"] = this.Column02;
			row["Column03"] = this.Column03;
			row["Column04"] = this.Column04;
			row["Column05"] = this.Column05;
			row["Column06"] = this.Column06;
			row["Column07"] = this.Column07;
			row["Column08"] = this.Column08;
			row["Column09"] = this.Column09;
			row["Column10"] = this.Column10;
			row["Column11"] = this.Column11;

			row["PanelX"] = this.PanelX;
			row["PanelY"] = this.PanelY;

			row["IsSelected"] = this.IsSelected;
			row["IsNew"] = this.IsNew;
			row["IsDone"] = this.IsDone;
			row["SinglelineX"] = this.SinglelineX;

			return row;
		}


    }
}
