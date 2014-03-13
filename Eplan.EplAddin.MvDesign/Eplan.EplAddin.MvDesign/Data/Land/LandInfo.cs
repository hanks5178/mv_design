using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Eplan.EplAddin.MvDesign.Data.Land
{
    [Serializable]
    public class LandInfo
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

        public string ProjectName { get; set; }
        public string Customer { get; set; }
        public string Equipment { get; set; }
        public string CadNo { get; set; }

		#endregion

		#region Constructors

		public LandInfo() { }

        public LandInfo(string projectNo)
        {
			this.MarginRight = 30;
            this.ProjectNo = projectNo;
            this.DrawingNo = projectNo.Substring(2, 2) + projectNo.Substring(10);
		}

		#endregion
	}
}
