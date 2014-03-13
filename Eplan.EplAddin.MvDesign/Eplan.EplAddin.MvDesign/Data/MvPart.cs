using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Eplan.EplAddin.MvDesign.Data
{
	public class MvPart
	{
		public string PartNr { get; set; }
		public string Manufacturer { get; set; }
		public string GroupSymbolMacro { get; set; }

		public MvPart(DataGridViewRow row)
		{
			this.PartNr = row.Cells["partnr"].Value.ToString();

			if (row.Cells["manufacturer"].Value != null)
				this.Manufacturer = row.Cells["manufacturer"].Value.ToString();
			else
				this.Manufacturer = string.Empty;

			if (row.Cells["groupsymbolmacro"].Value != null)
				this.GroupSymbolMacro = row.Cells["groupsymbolmacro"].Value.ToString();
			else
				this.GroupSymbolMacro = string.Empty;

		}
	}
}
