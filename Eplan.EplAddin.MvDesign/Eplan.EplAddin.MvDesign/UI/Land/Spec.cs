using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Eplan.EplAddin.MvDesign.UI.Land
{
    public partial class Spec : UserControl
    {
        public Spec()
        {
            InitializeComponent();
        }

        private void tabPage1_Enter(object sender, EventArgs e)
        {
            new Util.FormSizeSetter(tabPage1);
        }

        private void tabPage2_Enter(object sender, EventArgs e)
        {
            new Util.FormSizeSetter(tabPage2);
        }

        private void tabPage3_Enter(object sender, EventArgs e)
        {
            new Util.FormSizeSetter(tabPage3);
        }

        private void tabPage4_Enter(object sender, EventArgs e)
        {
            new Util.FormSizeSetter(tabPage4);
        }

        private void tabPage5_Enter(object sender, EventArgs e)
        {
            new Util.FormSizeSetter(tabPage5);
        }

        private void tabPage6_Enter(object sender, EventArgs e)
        {
            new Util.FormSizeSetter(tabPage6);
        }



        private void checkBox28_Click(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;

            checkBox28.Checked = false;
            checkBox29.Checked = false;
            checkBox30.Checked = false;
            checkBox31.Checked = false;
            checkBox.Checked = true;

            textBox44.Text = checkBox.Text.Substring(0, checkBox.Text.LastIndexOf(","));
            string[] tokens = checkBox.Text.Split(',');
            textBox46.Text = tokens[0];
            textBox47.Text = tokens[1];
            textBox48.Text = tokens[2];
        }

        private void checkBox23_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label37_Click(object sender, EventArgs e)
        {

        }

        private void checkBox37_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox50_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox53_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox56_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox51_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox28_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox29_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox36_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox39_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox20_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void comboBox12_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox11_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox10_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox9_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox8_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
