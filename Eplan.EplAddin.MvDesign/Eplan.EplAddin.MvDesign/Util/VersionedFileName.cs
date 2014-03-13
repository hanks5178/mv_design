using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Eplan.EplAddin.MvDesign.Util
{
    public class VersionedFileName1
    {
		public string FilePath { get; set; }

        public VersionedFileName1(string filePath)
		{
			string fileName;
			int version = 0;

			this.Parse(filePath, out fileName, out version);

			string directory = Path.GetDirectoryName(filePath);
			string[] fileNames = Directory.GetFiles(directory);

			string fileName1;
			int version1;
			foreach (string path in fileNames)
			{
				this.Parse(path, out fileName1, out version1);
				if (fileName == fileName1)
				{
					if (version < version1)
					{
						version = version1;
					}
				}
			}

			FilePath = directory + "\\" + fileName + "." + string.Format("{0}", version + 1);
		}

		private void Parse(string filePath, out string fileName, out int version)
		{
			string extension = Path.GetExtension(filePath).Replace(".", "");
			if (int.TryParse(extension, out version))
			{
				fileName = Path.GetFileNameWithoutExtension(filePath);
			}
			else
			{
				fileName = Path.GetFileName(filePath);
			}
		}
    }
}
