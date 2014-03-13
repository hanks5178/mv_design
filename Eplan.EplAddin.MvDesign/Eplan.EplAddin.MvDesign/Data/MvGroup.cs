using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Windows.Forms;
namespace Eplan.EplAddin.MvDesign.Data
{
    [Serializable]
    public class MvGroup
    {
        public Data.MvProject MvProject { get; set; }

        public int GroupID { get; set; }
        public string GroupNo { get; set; }
        public string Name { get; set; }
        public int FeederQty { get; set; }
        public string Rating { get; set; }
        public string IncomingType { get; set; }
        public string IncomingPt { get; set; }
        public bool BusDuctExists { get; set; }
        public bool LeftToRight { get; set; }
        public bool IncomingPtExists { get; set; }
        public bool BusPtExists { get; set; }
        public string KA { get; set; }
        public bool Visible { get; set; }

        public DataRow[] FeederRows { get { return this.MvProject.FeederTable.Select("GroupID = '" + GroupID.ToString() + "'", "PanelX ASC"); } }

        public MvGroup() { }

        private MvGroup(MvProject mvProject)
        {
            this.MvProject = mvProject;
        }

        private MvGroup(DataRow row)
        {
            GroupID = Convert.ToInt32(row["GroupID"]);
            GroupNo = row["GroupNo"].ToString();
            Name = row["GroupName"].ToString();
            FeederQty = Convert.ToInt32(row["FeederQty"]);
            Rating = row["Rating"].ToString();
            IncomingType = row["IncomingType"].ToString();
            IncomingPt = row["IncomingPt"].ToString();
            BusDuctExists = Convert.ToBoolean(row["BusDuctExists"]);
            LeftToRight = Convert.ToBoolean(row["LeftToRight"]);
            IncomingPtExists = Convert.ToBoolean(row["IncomingPtExists"]);
            BusPtExists = Convert.ToBoolean(row["BusPtExists"]);
            KA = row["KA"].ToString();
            Visible = Convert.ToBoolean(row["Visible"]);
        }

        public MvGroup(Data.MvProject mvProject, DataRow row)
            :this(row)
        {
            this.MvProject = mvProject;
        }
        
        public int GetPanelX(int groupId)
        {
            DataRow[] rows = this.MvProject.FeederTable.Select("GroupID = '" + groupId.ToString() + "'");
            return rows.Length;
        }



        private void CreateShipFeeders()
        {
            DataRow[] groupRows = this.MvProject.GroupTable.Select("GroupID = '" + this.GroupID.ToString() + "'");
            object groupNumber = groupRows[0]["GroupNo"];

            DataRow[] feederRows = this.MvProject.FeederTable.Select("GroupID = '" + this.GroupID.ToString() + "'");
            if (feederRows.Length >= this.FeederQty) return;

            for (int i = feederRows.Length; i < this.FeederQty; i++)
            {
                DataRow row = new Data.MvFeeder(this, "GENERATOR").DataRow();
				row["PanelX"] = i;
                this.MvProject.FeederTable.Rows.Add(row);
            }
        }

		private string GetDefaultFeeder(string feederType)
		{
			string location = Data.Location.Front(this.MvProject.IsShip, this.MvProject.ModelName, this.MvProject.LandInfo.Rating);

			string[] files = Directory.GetFiles(location, "*.EMA");
			List<string> list = new List<string>();
			foreach (string file in files)
			{
				list.Add(Path.GetFileNameWithoutExtension(file).ToUpper());
			}

			string defaultFeederType = list.FindLast(item => item.StartsWith(feederType.ToUpper()));
			if (defaultFeederType == null)
			{
				MessageBox.Show("ERROR: Could not find feeder: " + feederType);
			}

			return defaultFeederType;
		}

        private void CreateLandFeeder(int groupID, string groupNo, string feederType)
        {
            DataRow row = this.MvProject.FeederTable.NewRow();

            row["GroupID"] = groupID;
            row["GroupNo"] = groupNo;
            row["PanelName1"] = row["FeederType"] = this.GetDefaultFeeder(feederType.ToUpper());

            row["PanelX"] = GetPanelX(groupID);

            this.MvProject.FeederTable.Rows.Add(row);
        }

        private void CreateLandFeeders()
        {
            DataRow[] feederRows = this.MvProject.FeederTable.Select("GroupID = '" + this.GroupID + "'");
            foreach (DataRow feederRow in feederRows)
            {
                this.MvProject.FeederTable.Rows.Remove(feederRow);
            }

			//string location = Data.Location.Front(this.MvProject.IsShip, this.MvProject.ModelName, this.MvProject.LandInfo.Rating);
			//Util.CompositeFactory reader = new Util.CompositeFactory(location, "EMA");

			//Util.Composite feeder = reader.List.FindLast(item => item.Name.StartsWith("F"));
			string defaultFeederType = this.GetDefaultFeeder("Feeder");

            if (this.IncomingType.StartsWith("1"))
            {
                CreateLandFeeder(this.GroupID, this.GroupNo, "Incomg PT A");
                CreateLandFeeder(this.GroupID, this.GroupNo, "Incomg A");
                CreateLandFeeder(this.GroupID, this.GroupNo, "Bus PT");
                for (int i = 0; i < this.FeederQty; i++)
                {
                    CreateLandFeeder(this.GroupID, this.GroupNo, defaultFeederType);
                }
            }
            else if (IncomingType.StartsWith("2"))
            {
                int half = FeederQty / 2;
                for (int i = 0; i < half; i++)
                {
                    CreateLandFeeder(this.GroupID, this.GroupNo, defaultFeederType);
                }

                CreateLandFeeder(this.GroupID, this.GroupNo, "Incomg PT A");
                CreateLandFeeder(this.GroupID, this.GroupNo, "Incomg A");
                CreateLandFeeder(this.GroupID, this.GroupNo, "Bus PT A");
                CreateLandFeeder(this.GroupID, this.GroupNo, "Bus Tie A");
                CreateLandFeeder(this.GroupID, this.GroupNo, "Bus PT B");
                CreateLandFeeder(this.GroupID, this.GroupNo, "Incomg B");
                CreateLandFeeder(this.GroupID, this.GroupNo, "Incomg PT B");
				 
                for (int i = half; i < this.FeederQty; i++)
                {
                    CreateLandFeeder(this.GroupID, this.GroupNo, defaultFeederType);
                }
            }
            else
            {
                int oneThird = this.FeederQty / 3;
                for (int i = 0; i < oneThird; i++)
                {
                    CreateLandFeeder(this.GroupID, this.GroupNo, defaultFeederType);
                }

                CreateLandFeeder(this.GroupID, this.GroupNo, "Incomg PT A");
                CreateLandFeeder(this.GroupID, this.GroupNo, "Incomg A");
                CreateLandFeeder(this.GroupID, this.GroupNo, "Bus PT");
                CreateLandFeeder(this.GroupID, this.GroupNo, "Bus Tie A");
                CreateLandFeeder(this.GroupID, this.GroupNo, "Bus PT");
                CreateLandFeeder(this.GroupID, this.GroupNo, "Incomg B");
                CreateLandFeeder(this.GroupID, this.GroupNo, "Incomg PT B");

                int twoThird = this.FeederQty * 2 / 3;
                for (int i = oneThird; i < twoThird; i++)
                {
                    CreateLandFeeder(this.GroupID, this.GroupNo, defaultFeederType);
                }

                CreateLandFeeder(this.GroupID, this.GroupNo, "Bus Tie B");
                CreateLandFeeder(this.GroupID, this.GroupNo, "Bus PT");
                CreateLandFeeder(this.GroupID, this.GroupNo, "Incomg B");
                CreateLandFeeder(this.GroupID, this.GroupNo, "Incomg PT B");

                for (int i = twoThird; i < this.FeederQty; i++)
                {
                    CreateLandFeeder(this.GroupID, this.GroupNo, defaultFeederType);
                }
            }
        }

        public void CreateFeeders()
        {
            if (this.MvProject.IsShip)
            {
                CreateShipFeeders();
            }
            else
            {
                CreateLandFeeders();
            }
        }

    }
}
