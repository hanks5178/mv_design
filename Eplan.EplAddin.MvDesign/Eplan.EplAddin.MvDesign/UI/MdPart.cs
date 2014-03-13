using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Eplan.EplApi.Base;
using Eplan.EplApi.DataModel;
using Eplan.EplApi.MasterData;

namespace Eplan.EplAddin.MvDesign.UI
{
	public partial class MdPart : Form
	{
		public MdPart()
		{
			InitializeComponent();
		}


		#region Singleton Pattern

		private static MdPart _uniqueInstance = null;

		public static MdPart GetUniqueInstance()
		{
			if (_uniqueInstance == null || _uniqueInstance.IsDisposed)
			{
				_uniqueInstance = new MdPart();
			}
			else
			{
				_uniqueInstance.BringToFront();
			}
			return _uniqueInstance;
		}

		#endregion

		private void MdPart_Load(object sender, EventArgs e)
		{
			Data.Mdb mdb = Data.MdbFactory.GetUniqueInstance;
			mdb.ShowCatergoty(comboBox1);
		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			Data.Mdb mdb = Data.MdbFactory.GetUniqueInstance;
			mdb.ShowSubCatergory(comboBox1.Text, comboBox2);

			IsValidPartNr();
		}


		private bool IsValidPartNr()
		{
			bool isValid = true;

			if (comboBox1.Text.Length > 0 && comboBox2.Text.Length > 0)
			{
				groupBox1.ForeColor = SystemColors.ControlText;
			}
			else
			{
				isValid = false;
				groupBox1.ForeColor = Color.Red;
			}

			if (comboBox4.Text.Length > 0)
			{
				groupBox3.ForeColor = SystemColors.ControlText;
			}
			else
			{
				groupBox3.ForeColor = Color.Red;
				isValid = false;
			}

			return isValid;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (!this.IsValidPartNr())
			{
				MessageBox.Show("부품 종류와 부품 번호를 입력해야 합니다.");
				return;
			}

			string partNr = comboBox4.Text;
			string category = comboBox1.Text;
			string subCategory = comboBox2.Text;
			string maker = comboBox3.Text;
			string description1 = comboBox5.Text;
			string note = textBox1.Text;
			string groupSymbolMacro = textBox2.Text;

			MDPartsManagement pm = new MDPartsManagement();
			MDPartsDatabase database = pm.OpenDatabase();
			MDPart mdPart = database.GetPart(partNr);
			if (mdPart != null)
			{
				MessageBox.Show(partNr + " 부품이 있습니다.");
				return;
			}

			DialogResult dr = MessageBox.Show(partNr + " 부품을 생성합니다.", "부품 생성 확인", MessageBoxButtons.OKCancel);
			if (DialogResult.OK != dr) return;


			Data.Mdb mdb = Data.MdbFactory.GetUniqueInstance;
			string[] attributes = mdb.GetAttribute(comboBox1.Text, comboBox2.Text);


			mdPart = database.AddPart(partNr);
			if (mdPart == null)
			{
				MessageBox.Show(mdPart.PartNr + " 부품이 생성이 실패했습니다.");
				return;
			}

			mdPart.Variant = "0";
			mdPart.Properties.ARTICLE_PRODUCTGROUP = attributes[0];
			mdPart.Properties.ARTICLE_PRODUCTSUBGROUP = attributes[1];
			mdPart.Properties.ARTICLE_PRODUCTTOPGROUP = attributes[2];
			mdPart.Properties.ARTICLE_MANUFACTURER = maker.Trim().Length == 0 ? " " : maker;
			mdPart.Properties.ARTICLE_TYPENR = partNr;
			mdPart.Properties.ARTICLE_NOTE = note;
			mdPart.Properties.ARTICLE_GROUPSYMBOLMACRO = groupSymbolMacro;

			MultiLangString mls = new MultiLangString();
			mls.AddString(ISOCode.Language.L___, description1);
			mdPart.Properties.ARTICLE_DESCR1 = mls;

			mdb.AddAttribite(mdPart, category, subCategory);

			MessageBox.Show(mdPart.PartNr + " 부품이 생성되었습니다.");
		}

		private void button2_Click(object sender, EventArgs e)
		{
			this.Close();
			this.Dispose();
		}

		private void MdPart_Load_1(object sender, EventArgs e)
		{
			Data.Mdb mdb = Data.MdbFactory.GetUniqueInstance;
			mdb.ShowCatergoty(comboBox1);
		}

		private void button3_Click(object sender, EventArgs e)
		{
			OpenFileDialog of = new OpenFileDialog();
			of.Filter = "Macro Files (*.ema)|*.ema|All Files (*.*)|*.*";

			ProjectManager pm = new ProjectManager();
			of.InitialDirectory = pm.Paths.Macros;
			if (of.ShowDialog() == DialogResult.OK)
			{
				textBox2.Text = of.FileName.Replace(PathMap.SubstitutePath("$(MD_MACROS)"), "$(MD_MACROS)");
			}
		}
	}
}
