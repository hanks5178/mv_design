using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;

using Eplan.EplApi.Base;
using Eplan.EplApi.DataModel;
using Eplan.EplApi.DataModel.MasterData;

namespace Eplan.EplAddin.MvDesign.Util
{
	public class Finder
	{
		public bool IsShip { get; set; }
		public Project Project { get; set; }
		public Data.MvFeeder Feeder { get; set; }


		public Finder(Data.MvProject mvProject, DataGridViewRow row)
		{
			this.Project = mvProject.Project;
			this.IsShip = mvProject.IsShip;

			DataRowView rowView = row.DataBoundItem as DataRowView;
			this.Feeder = new Data.MvFeeder(rowView.Row);
		}

		#region Find Singleline/Threeline

		public LocationBox Singleline
		{
			get
			{
				if (this.Project == null) return null;

				string pageName = null;
				string deviceTag = null;
				if (this.IsShip)
				{
					pageName = "=VOL.4";
					deviceTag = "=VOL.5+" + this.Feeder.FeederNo;
				}
				else
				{
					pageName = "==" + this.Feeder.GroupNo + "=OV/";
					deviceTag = "=" + this.Feeder.FeederNo + "+G";
				}

				return this.FindSingleline(pageName, deviceTag);
			}
		}

		private LocationBox FindSingleline(string pageName, string deviceTag)
		{
			foreach (Page page in this.Project.Pages)
			{
				if (page.PageType != DocumentTypeManager.DocumentType.CircuitSingleLine) continue;

				if (page.IdentifyingName.ToUpper().StartsWith(pageName))
				{
					foreach (Placement p in page.AllFirstLevelPlacements)
					{
						LocationBox locationBox = p as LocationBox;
						if (locationBox == null) continue;

						if (locationBox.Name.Equals(deviceTag, StringComparison.InvariantCultureIgnoreCase))
							return locationBox;
					}
				}
			}

			return null;
		}


		public Page Threeline
		{
			get
			{
				if (this.Project == null) return null;
				string pageName = null;
				if (this.IsShip)
				{
					pageName = "=VOL.5+" + this.Feeder.FeederNo;
				}
				else
				{
					pageName = "==" + this.Feeder.GroupNo + "=" + this.Feeder.FeederNo + "+R";
				}

				return this.FindThreeline(pageName);
			}
		}




		private Page FindThreeline(string path)
		{
			foreach (Page page in this.Project.Pages)
			{
				if (page.PageType != DocumentTypeManager.DocumentType.Circuit) continue;

				if (page.Name.ToUpper().StartsWith(path))
				{
					return page;
				}
			}

			return null;
		}

		#endregion

		#region SetAsCurrentFeeder Methods

		public LocationBox ReplaceSingleline(string macroPath)
		{
			if (!File.Exists(macroPath))
			{
				MessageBox.Show("ERROR: File not found: " + macroPath);
				return null;
			}

			LocationBox locationBox = Util.eplan.DeleteSingleline(this.Singleline, false);
			Page page = locationBox.Page;
			PointD location = locationBox.Location;
			locationBox.Remove();

			LocationBox lb = Util.eplan.AddSinglelineMacro(this.IsShip, page, macroPath, this.Feeder, location.X, location.Y - 240);

			return lb;
		}

		public Page ReplaceThreeline(string macroPath)
		{
			Page page = Util.eplan.DeleteThreeline(this.Threeline);

			macroPath = macroPath.ToUpper().Replace(@"\SINGLE\", @"\THREE\");
			Util.eplan.AddThreelineMacro(this.IsShip, page, macroPath, this.Feeder, 0, 0);

			return page;
		}

		#endregion

		#region Copy and Paste Methods

		public LocationBox ReplaceSingleline(LocationBox sourceSingleline)
		{
			LocationBox locationBox = Util.eplan.DeleteSingleline(this.Singleline, true);
			locationBox = Util.eplan.PasteSingleline(sourceSingleline, locationBox);

			return locationBox;
		}

		public Page ReplaceThreeline(Util.Finder finder)
		{
			Page page = Util.eplan.DeleteThreeline(this.Threeline);

			string fromName = null;
			string toName = null; 
			if (this.IsShip)
			{
				fromName = "=VOL.5+" + finder.Feeder.FeederNo;
				toName = "=VOL.5+" + this.Feeder.FeederNo;
			}
			else
			{
				fromName = "=" + finder.Feeder.FeederNo + "+R";
				toName = "=" + this.Feeder.FeederNo + "+R";
			}

			page = Util.eplan.PasteThreeline(fromName, finder.Threeline, toName, this.Threeline);

			return page;
		}

		#endregion
	}
}
