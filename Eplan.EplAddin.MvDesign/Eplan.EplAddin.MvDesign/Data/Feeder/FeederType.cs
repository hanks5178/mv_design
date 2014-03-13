using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Eplan.EplAddin.MvDesign.Data.Feeder
{
    public class FeederType
    {
        public string Name { get; set; }
        public string Extension { get; set; }
        public string Location { get; set; }
        public string FullPath { get; set; }

        public FeederType(string path)
        {
            this.Name = Path.GetFileNameWithoutExtension(path);
            this.Extension = Path.GetExtension(path).Replace(".", string.Empty);
            this.Location = Path.GetDirectoryName(path);
            this.FullPath = path;
        }
    }
}
