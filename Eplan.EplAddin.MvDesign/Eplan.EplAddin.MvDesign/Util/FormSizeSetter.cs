using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Eplan.EplAddin.MvDesign.Util
{
    public class FormSizeSetter
    {
        public FormSizeSetter(Form form)
        {
			if (form == null) return;

            form.Size = new Size(1280, 640);
        }

        public FormSizeSetter(Control control)
        {
            if (control.Tag == null) return;
            Control form = control;
            while (form != null)
            {
                form = form.Parent;
                if (form is Form)
                {
                    break;
                }
            }

            if (form == null) return;

            string[] tokens = control.Tag.ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length != 2) return;

			int x = form.Size.Width;
            int y = Convert.ToInt32(tokens[1]);
            form.Size = new Size(x, y);

            return;
        }
    }
}
