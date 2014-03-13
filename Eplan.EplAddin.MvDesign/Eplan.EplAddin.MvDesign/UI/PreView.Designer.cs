namespace Eplan.EplAddin.MvDesign.UI
{
    partial class PreView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
             this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
             this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
             this.button2 = new System.Windows.Forms.Button();
             this.button1 = new System.Windows.Forms.Button();
             this.panel1 = new System.Windows.Forms.Panel();
             this.tableLayoutPanel1.SuspendLayout();
             this.tableLayoutPanel2.SuspendLayout();
             this.SuspendLayout();
             // 
             // tableLayoutPanel1
             // 
             this.tableLayoutPanel1.ColumnCount = 1;
             this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
             this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
             this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
             this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
             this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
             this.tableLayoutPanel1.Name = "tableLayoutPanel1";
             this.tableLayoutPanel1.RowCount = 2;
             this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
             this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56F));
             this.tableLayoutPanel1.Size = new System.Drawing.Size(491, 589);
             this.tableLayoutPanel1.TabIndex = 0;
             // 
             // tableLayoutPanel2
             // 
             this.tableLayoutPanel2.ColumnCount = 2;
             this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
             this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
             this.tableLayoutPanel2.Controls.Add(this.button2, 1, 0);
             this.tableLayoutPanel2.Controls.Add(this.button1, 0, 0);
             this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
             this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 536);
             this.tableLayoutPanel2.Name = "tableLayoutPanel2";
             this.tableLayoutPanel2.RowCount = 1;
             this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
             this.tableLayoutPanel2.Size = new System.Drawing.Size(485, 50);
             this.tableLayoutPanel2.TabIndex = 0;
             // 
             // button2
             // 
             this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
             this.button2.Location = new System.Drawing.Point(292, 13);
             this.button2.Margin = new System.Windows.Forms.Padding(50, 3, 50, 3);
             this.button2.Name = "button2";
             this.button2.Size = new System.Drawing.Size(143, 23);
             this.button2.TabIndex = 1;
             this.button2.Text = "닫기";
             this.button2.UseVisualStyleBackColor = true;
             this.button2.Click += new System.EventHandler(this.button2_Click);
             // 
             // button1
             // 
             this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
             this.button1.Location = new System.Drawing.Point(50, 13);
             this.button1.Margin = new System.Windows.Forms.Padding(50, 3, 50, 3);
             this.button1.Name = "button1";
             this.button1.Size = new System.Drawing.Size(142, 23);
             this.button1.TabIndex = 0;
             this.button1.Text = "적용";
             this.button1.UseVisualStyleBackColor = true;
             this.button1.Click += new System.EventHandler(this.button1_Click);
             // 
             // panel1
             // 
             this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
             this.panel1.Location = new System.Drawing.Point(3, 3);
             this.panel1.Name = "panel1";
             this.panel1.Size = new System.Drawing.Size(485, 527);
             this.panel1.TabIndex = 1;
             this.panel1.SizeChanged += new System.EventHandler(this.panel1_SizeChanged);
             this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
             this.panel1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseClick);
             this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
             this.panel1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseUp);
             // 
             // PreView
             // 
             this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
             this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
             this.ClientSize = new System.Drawing.Size(491, 589);
             this.Controls.Add(this.tableLayoutPanel1);
             this.Name = "PreView";
             this.Text = "PreView";
             this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PreView_FormClosed);
             this.Load += new System.EventHandler(this.PreView_Load);
             this.tableLayoutPanel1.ResumeLayout(false);
             this.tableLayoutPanel2.ResumeLayout(false);
             this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel1;
    }
}