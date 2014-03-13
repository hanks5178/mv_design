using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Eplan.EplAddin.MvDesign.UI
{
    public partial class ModelSelection : Form
	{
		#region Properties

		public bool IsShip { get; set; }
        public string ModelName { get; set; }
        public string Number { get; set; }

        private string name_template = "20123980-SHS191";
        private int Index;

		#endregion

		#region Constructor

		private ModelSelection()
        {
            InitializeComponent();

            this.CancelButton = button_Cancel;
            this.AcceptButton = button_Accept;
            BindControls();
        }

		#endregion

		#region Singleton Pattern

		private static ModelSelection singleton;
        public static ModelSelection GetSingleton()
        {
            if (singleton == null || singleton.IsDisposed)
            {
                singleton = new ModelSelection();
            }
            else
            {
                singleton.BringToFront();
            }

            return singleton;
        }

        #endregion


        private void BindControls()
        {
            comboBox1.Items.Clear();

            string path = Data.Location.Model(true);
            string[] names = Directory.GetDirectories(path);
            foreach (string name in names)	
            {
                string model = Path.GetFileName(name);
                this.comboBox1.Items.Add(model);
            }

            this.comboBox1.Items.Add("---------");

            path = Data.Location.Model(false);
            names = Directory.GetDirectories(path);
            foreach (string name in names)
            {
                string model = Path.GetFileName(name);
                this.comboBox1.Items.Add(model);
            }
        }


        private void ModelSelection_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < comboBox1.Items.Count; i++)
            {
                if (comboBox1.Items[i].ToString().Contains("----"))
                {
                    this.Index = i;
                    break;
                }
            }
            comboBox1.SelectedIndex = 0;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex < this.Index)
            {
                groupBox2.Text = "Ship No";
            }
            else if (comboBox1.SelectedIndex == this.Index)
            {
                groupBox2.Text = "";
            }
            else
            {
                groupBox2.Text = "프로젝트 번호(" + name_template + ")";
            }
        }

        private void button_Accept_Click(object sender, EventArgs e)
        {
            if (this.comboBox1.SelectedIndex == this.Index || comboBox1.SelectedIndex < 0)
            {
                MessageBox.Show("모델을 선택하십시오...");
                return;
            }
            else if (comboBox1.SelectedIndex < this.Index)
            {
                this.IsShip = true;
                if (this.textBox1.Text.Length < 1)
                {
                    MessageBox.Show("호선명을 입력하십시오...");
                    return;
                }
            }
            else
            {
                this.IsShip = false;
                if (this.textBox1.Text.Length != name_template.Length)
                {
                    MessageBox.Show("프로젝트 번호는 모두 " + name_template.Length.ToString() + " 자리이어야 합니다.");
                    return;
                }
            }

            this.ModelName = comboBox1.Text;
            this.Number = textBox1.Text;
            this.DialogResult = DialogResult.OK;
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Dispose();
        }
    }
}
