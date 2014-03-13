using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

using Eplan.EplApi.Base;
using Eplan.EplApi.DataModel;

namespace Eplan.EplAddin.MvDesign.Data
{
    [Serializable]
    public class MvProject
    {
        #region Permanent Properties

        public bool IsShip { get; set; }
        public string ModelName { get; set; }
        public string FileName { get; set; }
        public string LinkFileName { get { return this.FileName + ".elk"; } }

		public bool IsNewProject { get; set; }
        
        public Ship.ShipInfo ShipInfo { get; set; }
        public Land.LandInfo LandInfo { get; set; }
        public bool IsDoubleLayer { get; set; }
        public DataSet1 DataSet { get; set; }
        public bool IsPanelInsertMode { get; set; }

        #endregion

        #region Temporary Properties;

        [XmlIgnore]
        public Data.MvGroup CurrentGroup { get; set; }
        [XmlIgnore]
        public Project Project;
        [XmlIgnore]
        public DataTable GroupTable { get { return this.DataSet.Tables["Group"]; } }
        [XmlIgnore]
        public DataTable FeederTable { get { return this.DataSet.Tables["Feeder"]; } }
        [XmlIgnore]
        public DataView FeederView
        {
            get
            {
                DataView dataView = this.FeederTable.DefaultView;
                dataView.Sort = "GroupID ASC, PanelX ASC, PanelY ASC";
                return dataView;
            }
        }

        [XmlIgnore]
        public string Directory
        {
            get
            {
                ProjectManager pm = new ProjectManager();
                return pm.Paths.Projects + "\\";
            }
        }

        [XmlIgnore]
        public string ProjectTemplatePath
        {
            get
            {
                ProjectManager pm = new ProjectManager();
                if (this.IsShip)
                {
                    return pm.Paths.Templates + "\\" + this.ShipInfo.EplanTemplate;
                }
                else
                {
                    return pm.Paths.Templates + "\\" + this.LandInfo.EplanTemplate;
                }
            }
        }
                
        [XmlIgnore]
        public string FormXmlFile
        {
            get
            {
                return this.Directory + "\\" + this.FileName + "_Form.XML";
            }
        }

        [XmlIgnore]
        public string ProjectLinkFilePath
        {
            get
            {
                ProjectManager pm = new ProjectManager();
                return pm.Paths.Projects + "\\" + this.FileName + ".elk";
            }
        }

        [XmlIgnore]
        public string XmlFilePath
        {
            get
            {
                ProjectManager pm = new ProjectManager();
                return pm.Paths.Projects + "\\" + this.FileName + ".xml";
            }
        }

        #endregion

        #region Constructors
        public MvProject() { }

        public MvProject(bool isShip, string modelName, string projectNo)
        {
            this.IsShip = isShip;
            this.ModelName = modelName;
            this.FileName = projectNo;
            this.DataSet = new DataSet1();

            if (this.IsShip)
            {
                this.ShipInfo = new Ship.ShipInfo(projectNo);
                this.ShipInfo.ShipNo = projectNo;
                this.ShipInfo.ProjectNo = string.Empty;
            }
            else
            {
                this.LandInfo = new Land.LandInfo(projectNo);
                if (modelName.Contains("2단적"))
                {
                    this.IsDoubleLayer = true;
                }
            }
        }

        #endregion

        #region Static Methods

        private static bool ReadXml(string filePath, ref MvProject project)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                try
                {
                    XmlSerializer xs = new XmlSerializer(typeof(MvProject), "MvProject");
                    project = xs.Deserialize(fs) as MvProject;
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
                    return false;
                }
            }
        }


        public static bool Open(string filePath, ref MvProject project)
        {
            if (File.Exists(filePath))
            {
                if (!ReadXml(filePath, ref project)) return false;
                return true;
            }
            else
            {
                MessageBox.Show("File not found: " + filePath);
                return false;
            }
        }

        public static bool Open(ref MvProject project)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Title = "프로젝트 열기";
            openFileDialog.Filter = "Xml Files (*.xml)|*.xml*|All Files (*.*)|*.*";
            openFileDialog.DefaultExt = "xml";
            openFileDialog.FilterIndex = 0;
            openFileDialog.RestoreDirectory = true;
			openFileDialog.InitialDirectory = PathMap.SubstitutePath("$EPLAN_DATA");
			PathInfo p = new PathInfo();
			openFileDialog.InitialDirectory = p.Projects;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(openFileDialog.FileName))
                {
                    Environment.CurrentDirectory = Path.GetDirectoryName(openFileDialog.FileName);
                    return Open(openFileDialog.FileName, ref project);
                }
                else
                {
                    MessageBox.Show("프로젝트 파일의 형식이 다릅니다.");
                    return false;
                }
            }
            else
            {
                MessageBox.Show("취소되었습니다.");
                return false;
            }
        }

        private static bool WriteToXml(MvProject project, string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                try
                {
                    XmlSerializer xs = new XmlSerializer(typeof(MvProject), "MvProject");
                    xs.Serialize(fs, project);
                    fs.Close();

                    MessageBox.Show("Saved To: " + filePath);
                    return true;
                }
                catch (Exception e)
                {
                    MessageBox.Show("mv_design: " + e.Message + "\n" + e.StackTrace);
                    return false;
                }
            }

        }

        public static bool Save(MvProject project)
        {
            if (project.XmlFilePath != null)
            {
                //Util.VersionedFileName fileName = new Util.VersionedFileName(project.XmlFilePath);
				return WriteToXml(project, project.XmlFilePath);
            }
            else
            {
                return SaveAs(project);
            }
        }

        public static bool SaveAs(MvProject project)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Xml Files (*.xml)|*.xml*|All Files (*.*)|*.*";
            saveFileDialog.InitialDirectory = Path.GetDirectoryName(project.XmlFilePath);
            saveFileDialog.DefaultExt = ".xml";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
				//Util.VersionedFileName fileName = new Util.VersionedFileName(saveFileDialog.FileName);
				//project.FileName = Path.GetFileName(saveFileDialog.FileName);
				return WriteToXml(project, project.XmlFilePath);
            }
            else
            {
                return false;
            }
        }


        #endregion

        #region Methods

        public void AddGroup()
        {
            DataRow dataRow = this.GroupTable.NewRow();

            if (!this.IsShip)
            {
                if (this.LandInfo.Rating == null || this.LandInfo.Rating.Length < 1)
                {
                    MessageBox.Show("Rating이 값이 먼저 지정되지 않으면 그룹 생성을 할 수 없습니다.");
                    return;
                }
                else
                {
                    dataRow["Rating"] = this.LandInfo.Rating;
                }
            }

            this.GroupTable.Rows.Add(dataRow);
        }

        public void SetCurrentGroup(string groupNumber)
        {
            DataRow[] rows = this.GroupTable.Select("GroupNo = '" + groupNumber + "'");
            if (rows.Length < 1) return;

            this.CurrentGroup = new MvGroup(this, rows[0]);
        }

        public bool CreateProject()
        {
			this.IsNewProject = false;
            this.Project = Util.eplan.FindOpenedProject(this.FileName);
			if (this.Project != null) return true;

			this.Project = Util.eplan.OpenProject(this.FileName);
			if (this.Project != null) return true;

			this.IsNewProject = true;
			this.Project = Util.eplan.CreateProject(this.ProjectTemplatePath, this.ProjectLinkFilePath);
			if (this.Project != null) return true;

			return false;
        }

        public string GetGroupRating(object groupID)
        {
            DataRow[] dataRows = this.GroupTable.Select("GroupID = '" + groupID.ToString() + "'");
            if (dataRows.Length < 1)
            {
                return null;
            }

            return dataRows[0]["Rating"].ToString();
        }

        #endregion
    }
}
