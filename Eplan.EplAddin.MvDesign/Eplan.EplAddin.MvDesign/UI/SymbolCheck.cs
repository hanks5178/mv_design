using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using NLog;

using Eplan.EplApi.DataModel;
using Eplan.EplApi.HEServices;

namespace Eplan.EplAddin.MvDesign.UI
{
	public partial class SymbolCheck : Form
	{
		private Logger _logger;
		public Project Project { get; set; }

		public SymbolCheck()
		{
			InitializeComponent();
			_logger = MyLogManager.MyLogManager.Instance.GetCurrentClassLogger();
		}

		public void CheckAgain()
		{
			using (new LockingStep())
			{
				this.Project = null;
				SelectionSet ss = new SelectionSet();

				foreach (StorableObject s in ss.Selection)
				{
					this.Project = s as Project;
					if (this.Project != null) break;

					Page page = s as Page;
					if (page != null)
					{
						this.Project = page.Project;
						break;
					}
				}

				if (this.Project == null)
				{
					MessageBox.Show("선택된 프로젝트가 없습니다.");
					return;
				}

				DialogResult dr = MessageBox.Show("\"" + this.Project.ProjectName + "\"의 부품이 입력되지 않은 블랙박스를 검색합니다", "확인", MessageBoxButtons.OKCancel);
				if (dr != DialogResult.OK) return;

				List<EmptySymbol> list = new List<EmptySymbol>();
				foreach (Page page in this.Project.Pages)
				{
					if (page.PageType != DocumentTypeManager.DocumentType.Circuit //&&
						//page.PageType != DocumentTypeManager.DocumentType.CircuitSingleLine
						) continue;

					foreach (BoxedDevice bd in page.BoxedDevices)
					{
						_logger.Debug("Blackbox found: n Articles: " + bd.ArticleReferences.Length.ToString("D3")
							+ " IsMainFunction: " + bd.IsMainFunction.ToString()
							+ " Name " + bd.Name
							+ " @Page: " + page.Name

							);

						if (!bd.IsMainFunction) continue;
						if (bd.ArticleReferences.Length > 0) continue;

						EmptySymbol es = new EmptySymbol(page.Name, bd.Name);
						list.Add(es);
					}
				}

				this.dataGridView1.DataSource = null;
				this.dataGridView1.DataSource = list;

				Util.CSharp.AutoSizeDataGridViewColumn(dataGridView1);
			}
		}

		private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (dataGridView1.SelectedRows.Count < 1) return;

			EmptySymbol es = dataGridView1.SelectedRows[0].DataBoundItem as EmptySymbol;
			if (es == null) return;

			foreach (Page page in this.Project.Pages)
			{
				if (page.Name == es.PageName)
				{
					new Edit().OpenPageWithNameAndDeviceName(this.Project.ProjectLinkFilePath, es.PageName, es.DeviceTag);

				}
			}
		}

		private void SymbolCheck_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.Dispose();
		}

		private void SymbolCheck_Load(object sender, EventArgs e)
		{
			CheckAgain();
		}

		private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
		{
			Util.CSharp.ShowRowHeaderNumber(sender, e);
		}


		private void button1_Click(object sender, EventArgs e)
		{
			CheckAgain();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			this.Close();
		}


	}
}
