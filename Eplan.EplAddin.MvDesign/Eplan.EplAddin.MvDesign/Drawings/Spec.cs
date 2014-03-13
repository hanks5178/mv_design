using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using Eplan.EplApi.Base;
using Eplan.EplApi.DataModel;
using Eplan.EplApi.DataModel.Graphics;

namespace Eplan.EplAddin.MvDesign.Drawings
{
    class Spec
    {
        private string MacroPath { get; set; }
        private Data.MvProject MvProject { get; set; }

        public Spec(Data.MvProject mvProject, UserControl uc)
        {
            this.MvProject = mvProject;
            this.MacroPath = Data.Location.Macro(this.MvProject.IsShip);
            if (mvProject.IsShip)
            {
                this.CreateShipSpec(mvProject.Project, uc);
            }
            else
            {
                this.CreateLandSpec(mvProject.Project, uc);
            }
        }


        private Rectangle DrawCheckBox(Page page, double x, double y, bool isChecked, bool fill = true)
        {
            PointD point1 = new PointD(x, y);
            PointD point2 = new PointD(x + 4, y + 4);

            Rectangle r =  Util.eplan.DrawRectangle(page, point1, point2);
            if (r == null) return null;

            if (!isChecked) return r;

            if (this.MvProject.IsShip)
            {
                r.IsSurfaceFilled = true;
            }
            else
            {
                point1 = new PointD(x, y);
				point2 = new PointD(x + 4, y + 4);
                Util.eplan.DrawLine(page, point1, point2);

                point1 = new PointD(x, y + 4);
                point2 = new PointD(x + 4, y);
                Util.eplan.DrawLine(page, point1, point2);
            }

            return r;
        }


        private Text WriteControl(Page page, Control control)
        {
            string tag = control.Tag as string;
            if (tag == null) return null;

            if (tag.StartsWith("N")) tag = tag.Substring(1);

            string[] tokens = tag.Split(',', '/');
            if (tokens.Length != 2) return null;

            int x = 0;
            int y = 0;

            if (!int.TryParse(tokens[0], out x) || !int.TryParse(tokens[1], out y))
            {
                MessageBox.Show("ERROR: " + control.Name + ", Tag = " + tag);
                return null;
            }

            y += 2;

            Text text = null;
            CheckBox checkBox = control as CheckBox;
            if (checkBox != null)
            {
                Rectangle r = this.DrawCheckBox(page, x, y - 2, checkBox.Checked);
                text = Util.eplan.WriteText(page, checkBox.Text, x + 6, y);
            }
            else
            {
                text = Util.eplan.WriteText(page, control.Text, x, y);
            }

            if (text == null) return null;

			text.Justification = TextBase.JustificationType.MiddleLeft;
			text.RowSpacing = 80;

            if (this.MvProject.IsShip)
            {
			}
            else
            {
                text.Height = 3.5;
			}

            return text;
        }


        private void CollectColtrols(Control control, List<Control> controls)
        {
            GroupBox groupBox = control as GroupBox;
            if (groupBox == null) return;

            foreach (Control c in groupBox.Controls)
            {
                GroupBox g = c as GroupBox;
                if (g != null)
                {
                    this.CollectColtrols(c, controls);
                }
                else
                {
                    controls.Add(c);
                }
            }
        }

        private void WritePage(Page page, Control tabControl, string macroPath)
        {
            if (page == null || tabControl == null) return;

            if (page.AllGraphicalPlacements.Length == 0)
            {
				if (this.MvProject.IsShip)
				{
					Util.eplan.InsertWindowMacro(page, macroPath, 0, 0);
				}
				else
				{
					Util.eplan.InsertWindowMacro(page, macroPath, 92, 284);
				}
            }

            List<Control> controlList = new List<Control>();
            foreach (Control control in tabControl.Controls)
            {
                this.CollectColtrols(control, controlList);
            }

            foreach (Control control in controlList)
            {
                Text text = this.WriteControl(page, control);
            }
        }

        private Page ClearPage(Page page)
        {
            if (page == null) return null;
            foreach (Placement p in page.AllGraphicalPlacements)
            {
                p.Remove();
            }

            return page;
        }


        private void CreateShipSpec(Project project, UserControl uc)
        {
            Page page = Util.eplan.GetPageWithProperty(project, "SPECIFICATION");
            if (page == null) return;

            TabControl tabControl = uc.Controls[0] as TabControl;
            if (tabControl == null) return;

            this.ClearPage(page);
            foreach (TabPage tabPage in tabControl.TabPages)
            {
                this.WritePage(page, tabPage, this.MacroPath + @"\SPECIFICATION.EMA");
            }
        }



        private void CreateLandSpec(Project project, UserControl uc)
        {
            TabControl tabControl = uc.Controls[0] as TabControl;
            if (tabControl == null) return;

			Page page = Util.eplan.GetPageWithProperty(project, "SPECIFICATION_1");
            this.ClearPage(page);
			this.WritePage(page, tabControl.TabPages[0], this.MacroPath + @"\SPECIFICATION_1.EMA");

			page = Util.eplan.GetPageWithProperty(project, "SPECIFICATION_2");
            this.ClearPage(page);
			this.WritePage(page, tabControl.TabPages[1], this.MacroPath + @"\SPECIFICATION_2.EMA");

			page = Util.eplan.GetPageWithProperty(project, "SPECIFICATION_3");
            this.ClearPage(page);
			this.WritePage(page, tabControl.TabPages[2], this.MacroPath + @"\SPECIFICATION_3.EMA");

			page = Util.eplan.GetPageWithProperty(project, "SPECIFICATION_4");
            this.ClearPage(page);
			this.WritePage(page, tabControl.TabPages[3], this.MacroPath + @"\SPECIFICATION_4.EMA");

			page = Util.eplan.GetPageWithProperty(project, "SPECIFICATION_5");
            this.ClearPage(page);
			this.WritePage(page, tabControl.TabPages[4], this.MacroPath + @"\SPECIFICATION_5.EMA");

			page = Util.eplan.GetPageWithProperty(project, "SPECIFICATION_6");
            this.ClearPage(page);
			this.WritePage(page, tabControl.TabPages[5], this.MacroPath + @"\SPECIFICATION_6.EMA");
        }
    }
}
