using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using NLog;

using Eplan.EplApi.DataModel;
using Eplan.EplApi.HEServices;

namespace Eplan.EplAddin.MvDesign
{
	class EmptySymbol
	{
		public string PageName { get; set; }
		public string DeviceTag { get; set; }

		public EmptySymbol(string pageName, string dt)
		{
			this.PageName = pageName;
			this.DeviceTag = dt;
		}
	}

	class SymbolCheck
	{
		Logger _logger;
		public Project Project { get; set; }

		public SymbolCheck()
		{
			_logger = MyLogManager.MyLogManager.Instance.GetCurrentClassLogger();
		}

		public void Execute()
		{
			using (new LockingStep())
			{
				this.Project = null;
				SelectionSet ss = new SelectionSet();

				foreach(Page p in ss.OpenedPages)
				{
					this.Project = p.Project;
					if (this.Project != null) break;
				}

				if (this.Project == null)
				{
					MessageBox.Show("선택된 프로젝트가 없습니다.");
					return;
				}
				else
				{
					DialogResult dr = MessageBox.Show("\"" + this.Project.ProjectName + "\"의 부품이 입력되지 않은 블랙박스를 표시합니다", "확인", MessageBoxButtons.OKCancel);
					if (dr != DialogResult.OK) return;
				}

				List<EmptySymbol> list = new List<EmptySymbol>();
				foreach (Page page in this.Project.Pages)
				{
					if (page.PageType != DocumentTypeManager.DocumentType.Circuit) continue;

					foreach (BoxedDevice bd in page.BoxedDevices)
					{
						if (!bd.IsMainFunction) continue;
						if (bd.ArticleReferences.Length > 0) continue;

						EmptySymbol es = new EmptySymbol(page.Name, bd.Name);
					}
				}
			}
		}


		public void Clear()
		{


		}
	}
}
