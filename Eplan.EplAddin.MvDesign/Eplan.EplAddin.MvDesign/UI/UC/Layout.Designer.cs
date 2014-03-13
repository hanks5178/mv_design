namespace Eplan.EplAddin.MvDesign.UI.UC
{
    partial class Layout
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Layout));
			this.toolStrip4 = new System.Windows.Forms.ToolStrip();
			this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
			this.tscb_GroupNumber = new System.Windows.Forms.ToolStripComboBox();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.tstb_FeederQty = new System.Windows.Forms.ToolStripTextBox();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
			this.tsb_InsertMode = new System.Windows.Forms.ToolStripButton();
			this.tsb_ReplaceMode = new System.Windows.Forms.ToolStripButton();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.panel_Arranged = new System.Windows.Forms.Panel();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.panel_UnArranged = new System.Windows.Forms.Panel();
			this.toolStrip4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStrip4
			// 
			this.toolStrip4.AutoSize = false;
			this.toolStrip4.ImageScalingSize = new System.Drawing.Size(24, 24);
			this.toolStrip4.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.tscb_GroupNumber,
            this.toolStripSeparator4,
            this.tstb_FeederQty,
            this.toolStripSeparator1,
            this.toolStripButton2,
            this.tsb_InsertMode,
            this.tsb_ReplaceMode});
			this.toolStrip4.Location = new System.Drawing.Point(0, 0);
			this.toolStrip4.Name = "toolStrip4";
			this.toolStrip4.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.toolStrip4.Size = new System.Drawing.Size(784, 30);
			this.toolStrip4.TabIndex = 3;
			this.toolStrip4.Text = "toolStrip4";
			// 
			// toolStripLabel1
			// 
			this.toolStripLabel1.Name = "toolStripLabel1";
			this.toolStripLabel1.Size = new System.Drawing.Size(66, 27);
			this.toolStripLabel1.Text = "   그룹 No:";
			// 
			// tscb_GroupNumber
			// 
			this.tscb_GroupNumber.AutoSize = false;
			this.tscb_GroupNumber.Name = "tscb_GroupNumber";
			this.tscb_GroupNumber.Size = new System.Drawing.Size(200, 23);
			this.tscb_GroupNumber.TextChanged += new System.EventHandler(this.tscb_GroupNumber_TextChanged);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.AutoSize = false;
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(12, 30);
			// 
			// tstb_FeederQty
			// 
			this.tstb_FeederQty.AutoSize = false;
			this.tstb_FeederQty.Name = "tstb_FeederQty";
			this.tstb_FeederQty.Size = new System.Drawing.Size(60, 23);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.AutoSize = false;
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(12, 30);
			// 
			// toolStripButton2
			// 
			this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
			this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton2.Name = "toolStripButton2";
			this.toolStripButton2.Size = new System.Drawing.Size(28, 27);
			this.toolStripButton2.Text = "Refresh";
			this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
			// 
			// tsb_InsertMode
			// 
			this.tsb_InsertMode.AutoSize = false;
			this.tsb_InsertMode.CheckOnClick = true;
			this.tsb_InsertMode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsb_InsertMode.Image = ((System.Drawing.Image)(resources.GetObject("tsb_InsertMode.Image")));
			this.tsb_InsertMode.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsb_InsertMode.Name = "tsb_InsertMode";
			this.tsb_InsertMode.Size = new System.Drawing.Size(26, 26);
			this.tsb_InsertMode.Text = "판넬을 순서대로 밀기(Click To Change)";
			this.tsb_InsertMode.CheckStateChanged += new System.EventHandler(this.tsb_InsertMode_CheckStateChanged);
			// 
			// tsb_ReplaceMode
			// 
			this.tsb_ReplaceMode.AutoSize = false;
			this.tsb_ReplaceMode.CheckOnClick = true;
			this.tsb_ReplaceMode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsb_ReplaceMode.Image = ((System.Drawing.Image)(resources.GetObject("tsb_ReplaceMode.Image")));
			this.tsb_ReplaceMode.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsb_ReplaceMode.Name = "tsb_ReplaceMode";
			this.tsb_ReplaceMode.Size = new System.Drawing.Size(26, 26);
			this.tsb_ReplaceMode.Text = "판넬 자리 바꾸기(Click To Change)";
			this.tsb_ReplaceMode.CheckStateChanged += new System.EventHandler(this.tsb_ReplaceMode_CheckStateChanged);
			this.tsb_ReplaceMode.Click += new System.EventHandler(this.tsb_ReplaceMode_Click);
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainer1.Location = new System.Drawing.Point(0, 30);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
			this.splitContainer1.Size = new System.Drawing.Size(784, 580);
			this.splitContainer1.SplitterDistance = 275;
			this.splitContainer1.TabIndex = 5;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.panel_Arranged);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox1.Location = new System.Drawing.Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(784, 275);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "배치된 Panel";
			// 
			// panel_Arranged
			// 
			this.panel_Arranged.AutoScroll = true;
			this.panel_Arranged.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel_Arranged.Location = new System.Drawing.Point(3, 17);
			this.panel_Arranged.Name = "panel_Arranged";
			this.panel_Arranged.Size = new System.Drawing.Size(778, 255);
			this.panel_Arranged.TabIndex = 0;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.panel_UnArranged);
			this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox2.Location = new System.Drawing.Point(0, 0);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(784, 301);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "배치되지 않은 Panel";
			// 
			// panel_UnArranged
			// 
			this.panel_UnArranged.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel_UnArranged.Location = new System.Drawing.Point(3, 17);
			this.panel_UnArranged.Name = "panel_UnArranged";
			this.panel_UnArranged.Size = new System.Drawing.Size(778, 281);
			this.panel_UnArranged.TabIndex = 0;
			// 
			// Layout
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.toolStrip4);
			this.Name = "Layout";
			this.Size = new System.Drawing.Size(784, 610);
			this.Load += new System.EventHandler(this.Layout_Load);
			this.VisibleChanged += new System.EventHandler(this.Layout_VisibleChanged);
			this.toolStrip4.ResumeLayout(false);
			this.toolStrip4.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip4;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox tscb_GroupNumber;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripTextBox tstb_FeederQty;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsb_InsertMode;
		private System.Windows.Forms.ToolStripButton tsb_ReplaceMode;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel_Arranged;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel panel_UnArranged;
		private System.Windows.Forms.ToolStripButton toolStripButton2;
    }
}
