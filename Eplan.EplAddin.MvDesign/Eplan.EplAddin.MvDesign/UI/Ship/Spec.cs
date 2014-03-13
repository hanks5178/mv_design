using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Eplan.EplAddin.MvDesign.UI.Ship
{
    public partial class Spec : UserControl
    {
        public Spec()
        {
            InitializeComponent();
            new Util.FormSizeSetter(tabPage1);
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

        private void checkBox37_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox52_CheckedChanged(object sender, EventArgs e)
        {

        }

    }
}
