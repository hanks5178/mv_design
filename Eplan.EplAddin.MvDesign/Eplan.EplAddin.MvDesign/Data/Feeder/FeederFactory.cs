using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Eplan.EplAddin.MvDesign.Data.Feeder
{
    class FeederFactory
    {
        public List<FeederType> FeederTypes { get; set; }

        public FeederFactory(Data.MvProject mvProject)
        {
            string folder;
            if (mvProject.IsShip)
            {
                folder = Data.Location.Front(mvProject.IsShip, mvProject.ModelName, null);
            }
            else
            {
                folder = Data.Location.Front(mvProject.IsShip, mvProject.ModelName, mvProject.LandInfo.Rating);
            }

            this.ReadFiles(folder, "*.ema");
        }

		private void ReadFiles(string path, string filter)
		{
			if (!Directory.Exists(path))
			{
				MessageBox.Show("Directory not found: " + path);
				return;
			}

			string[] fileNames = Directory.GetFiles(path, filter);
			this.FeederTypes = new List<FeederType>();
			foreach (string fileName in fileNames)
			{
				this.FeederTypes.Add(new FeederType(fileName.ToUpper()));
			}

			return;
		}

        public FeederFactory(Data.MvProject mvProject, string groupRating)
        {
            string folder = Data.Location.Front(mvProject.IsShip, mvProject.ModelName, groupRating);

			this.ReadFiles(folder, "*.ema");
        }

		private void ReadFolders(string path)
		{
			if (!Directory.Exists(path))
			{
				MessageBox.Show("Directory not found: " + path);
				return;
			}

			string[] fodlerNames = Directory.GetDirectories(path);
			this.FeederTypes = new List<FeederType>();
			foreach (string folderName in fodlerNames)
			{
				this.FeederTypes.Add(new FeederType(folderName.ToUpper()));
			}

			return;
		}
    }
}
