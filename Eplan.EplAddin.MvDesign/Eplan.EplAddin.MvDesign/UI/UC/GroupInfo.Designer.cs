namespace Eplan.EplAddin.MvDesign.UI.UC
{
    partial class GroupInfo
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GroupInfo));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.groupIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.visibleDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.groupNoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.groupNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.feederQtyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ratingDataGridViewComboBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.incomingTypeDataGridViewComboBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.leftToRightDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.incomingPtExistsDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.busPtExistsDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.busDuctExistsDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.incomingPtDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.kADataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.groupBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.dataSet1 = new Eplan.EplAddin.MvDesign.Data.DataSet1();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
			this.tstb = new System.Windows.Forms.ToolStripTextBox();
			this.tsb_AddGroup = new System.Windows.Forms.ToolStripButton();
			this.tsb_DeleteGroup = new System.Windows.Forms.ToolStripButton();
			this.tsb_CreateFeeder = new System.Windows.Forms.ToolStripButton();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.그룹추가ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.선택된그룹삭제ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.feeder생성ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.셀복사ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.연속데이터채우기ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.groupBindingSource)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dataSet1)).BeginInit();
			this.toolStrip1.SuspendLayout();
			this.contextMenuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.dataGridView1);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox1.Location = new System.Drawing.Point(0, 30);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(742, 546);
			this.groupBox1.TabIndex = 5;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "그룹 정보";
			// 
			// dataGridView1
			// 
			this.dataGridView1.AllowUserToAddRows = false;
			this.dataGridView1.AllowUserToDeleteRows = false;
			this.dataGridView1.AllowUserToOrderColumns = true;
			this.dataGridView1.AutoGenerateColumns = false;
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.groupIDDataGridViewTextBoxColumn,
            this.visibleDataGridViewCheckBoxColumn,
            this.groupNoDataGridViewTextBoxColumn,
            this.groupNameDataGridViewTextBoxColumn,
            this.feederQtyDataGridViewTextBoxColumn,
            this.ratingDataGridViewComboBoxColumn,
            this.incomingTypeDataGridViewComboBoxColumn,
            this.leftToRightDataGridViewCheckBoxColumn,
            this.incomingPtExistsDataGridViewCheckBoxColumn,
            this.busPtExistsDataGridViewCheckBoxColumn,
            this.busDuctExistsDataGridViewCheckBoxColumn,
            this.incomingPtDataGridViewTextBoxColumn,
            this.kADataGridViewTextBoxColumn});
			this.dataGridView1.DataSource = this.groupBindingSource;
			this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dataGridView1.Location = new System.Drawing.Point(3, 17);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.RowTemplate.Height = 23;
			this.dataGridView1.Size = new System.Drawing.Size(736, 526);
			this.dataGridView1.TabIndex = 0;
			this.dataGridView1.CurrentCellDirtyStateChanged += new System.EventHandler(this.dataGridView1_CurrentCellDirtyStateChanged);
			this.dataGridView1.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dataGridView1_EditingControlShowing);
			this.dataGridView1.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridView1_RowPostPaint);
			// 
			// groupIDDataGridViewTextBoxColumn
			// 
			this.groupIDDataGridViewTextBoxColumn.DataPropertyName = "GroupID";
			this.groupIDDataGridViewTextBoxColumn.HeaderText = "GroupID";
			this.groupIDDataGridViewTextBoxColumn.Name = "groupIDDataGridViewTextBoxColumn";
			// 
			// visibleDataGridViewCheckBoxColumn
			// 
			this.visibleDataGridViewCheckBoxColumn.DataPropertyName = "Visible";
			this.visibleDataGridViewCheckBoxColumn.HeaderText = "Visible";
			this.visibleDataGridViewCheckBoxColumn.Name = "visibleDataGridViewCheckBoxColumn";
			// 
			// groupNoDataGridViewTextBoxColumn
			// 
			this.groupNoDataGridViewTextBoxColumn.DataPropertyName = "GroupNo";
			this.groupNoDataGridViewTextBoxColumn.HeaderText = "GroupNo";
			this.groupNoDataGridViewTextBoxColumn.Name = "groupNoDataGridViewTextBoxColumn";
			// 
			// groupNameDataGridViewTextBoxColumn
			// 
			this.groupNameDataGridViewTextBoxColumn.DataPropertyName = "GroupName";
			this.groupNameDataGridViewTextBoxColumn.HeaderText = "GroupName";
			this.groupNameDataGridViewTextBoxColumn.Name = "groupNameDataGridViewTextBoxColumn";
			// 
			// feederQtyDataGridViewTextBoxColumn
			// 
			this.feederQtyDataGridViewTextBoxColumn.DataPropertyName = "FeederQty";
			this.feederQtyDataGridViewTextBoxColumn.HeaderText = "FeederQty";
			this.feederQtyDataGridViewTextBoxColumn.Name = "feederQtyDataGridViewTextBoxColumn";
			// 
			// ratingDataGridViewComboBoxColumn
			// 
			this.ratingDataGridViewComboBoxColumn.DataPropertyName = "Rating";
			this.ratingDataGridViewComboBoxColumn.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
			this.ratingDataGridViewComboBoxColumn.HeaderText = "Rating";
			this.ratingDataGridViewComboBoxColumn.Name = "ratingDataGridViewComboBoxColumn";
			this.ratingDataGridViewComboBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.ratingDataGridViewComboBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			// 
			// incomingTypeDataGridViewComboBoxColumn
			// 
			this.incomingTypeDataGridViewComboBoxColumn.DataPropertyName = "IncomingType";
			this.incomingTypeDataGridViewComboBoxColumn.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
			this.incomingTypeDataGridViewComboBoxColumn.HeaderText = "IncomingType";
			this.incomingTypeDataGridViewComboBoxColumn.Items.AddRange(new object[] {
            "1 INCOMING",
            "2 INCOMING 1 TIE",
            "3 INCOMING 2 TIE"});
			this.incomingTypeDataGridViewComboBoxColumn.Name = "incomingTypeDataGridViewComboBoxColumn";
			this.incomingTypeDataGridViewComboBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.incomingTypeDataGridViewComboBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			// 
			// leftToRightDataGridViewCheckBoxColumn
			// 
			this.leftToRightDataGridViewCheckBoxColumn.DataPropertyName = "LeftToRight";
			this.leftToRightDataGridViewCheckBoxColumn.HeaderText = "LeftToRight";
			this.leftToRightDataGridViewCheckBoxColumn.Name = "leftToRightDataGridViewCheckBoxColumn";
			// 
			// incomingPtExistsDataGridViewCheckBoxColumn
			// 
			this.incomingPtExistsDataGridViewCheckBoxColumn.DataPropertyName = "IncomingPtExists";
			this.incomingPtExistsDataGridViewCheckBoxColumn.HeaderText = "IncomingPtExists";
			this.incomingPtExistsDataGridViewCheckBoxColumn.Name = "incomingPtExistsDataGridViewCheckBoxColumn";
			// 
			// busPtExistsDataGridViewCheckBoxColumn
			// 
			this.busPtExistsDataGridViewCheckBoxColumn.DataPropertyName = "BusPtExists";
			this.busPtExistsDataGridViewCheckBoxColumn.HeaderText = "BusPtExists";
			this.busPtExistsDataGridViewCheckBoxColumn.Name = "busPtExistsDataGridViewCheckBoxColumn";
			// 
			// busDuctExistsDataGridViewCheckBoxColumn
			// 
			this.busDuctExistsDataGridViewCheckBoxColumn.DataPropertyName = "BusDuctExists";
			this.busDuctExistsDataGridViewCheckBoxColumn.HeaderText = "BusDuctExists";
			this.busDuctExistsDataGridViewCheckBoxColumn.Name = "busDuctExistsDataGridViewCheckBoxColumn";
			// 
			// incomingPtDataGridViewTextBoxColumn
			// 
			this.incomingPtDataGridViewTextBoxColumn.DataPropertyName = "IncomingPt";
			this.incomingPtDataGridViewTextBoxColumn.HeaderText = "IncomingPt";
			this.incomingPtDataGridViewTextBoxColumn.Name = "incomingPtDataGridViewTextBoxColumn";
			// 
			// kADataGridViewTextBoxColumn
			// 
			this.kADataGridViewTextBoxColumn.DataPropertyName = "KA";
			this.kADataGridViewTextBoxColumn.HeaderText = "KA";
			this.kADataGridViewTextBoxColumn.Name = "kADataGridViewTextBoxColumn";
			// 
			// groupBindingSource
			// 
			this.groupBindingSource.DataMember = "Group";
			this.groupBindingSource.DataSource = this.dataSet1;
			// 
			// dataSet1
			// 
			this.dataSet1.DataSetName = "DataSet1";
			this.dataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// toolStrip1
			// 
			this.toolStrip1.AutoSize = false;
			this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.tstb,
            this.tsb_AddGroup,
            this.tsb_DeleteGroup,
            this.tsb_CreateFeeder});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.toolStrip1.Size = new System.Drawing.Size(742, 30);
			this.toolStrip1.TabIndex = 4;
			this.toolStrip1.Text = "toolStrip2";
			// 
			// toolStripLabel1
			// 
			this.toolStripLabel1.Name = "toolStripLabel1";
			this.toolStripLabel1.Size = new System.Drawing.Size(62, 27);
			this.toolStripLabel1.Text = "그룹 수량:";
			// 
			// tstb
			// 
			this.tstb.AutoSize = false;
			this.tstb.BackColor = System.Drawing.SystemColors.ButtonFace;
			this.tstb.Name = "tstb";
			this.tstb.Size = new System.Drawing.Size(40, 22);
			this.tstb.Text = "0";
			this.tstb.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.tstb.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tstb_KeyPress);
			// 
			// tsb_AddGroup
			// 
			this.tsb_AddGroup.AutoSize = false;
			this.tsb_AddGroup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsb_AddGroup.Image = ((System.Drawing.Image)(resources.GetObject("tsb_AddGroup.Image")));
			this.tsb_AddGroup.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsb_AddGroup.Name = "tsb_AddGroup";
			this.tsb_AddGroup.Size = new System.Drawing.Size(26, 26);
			this.tsb_AddGroup.Text = "toolStripButton1";
			this.tsb_AddGroup.ToolTipText = "그룹 추가";
			this.tsb_AddGroup.Click += new System.EventHandler(this.tsb_AddGroup_Click);
			// 
			// tsb_DeleteGroup
			// 
			this.tsb_DeleteGroup.AutoSize = false;
			this.tsb_DeleteGroup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsb_DeleteGroup.Image = ((System.Drawing.Image)(resources.GetObject("tsb_DeleteGroup.Image")));
			this.tsb_DeleteGroup.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsb_DeleteGroup.Name = "tsb_DeleteGroup";
			this.tsb_DeleteGroup.Size = new System.Drawing.Size(26, 26);
			this.tsb_DeleteGroup.Text = "toolStripButton17";
			this.tsb_DeleteGroup.ToolTipText = "그룹 삭제";
			this.tsb_DeleteGroup.Click += new System.EventHandler(this.tsb_DeleteGroup_Click);
			// 
			// tsb_CreateFeeder
			// 
			this.tsb_CreateFeeder.AutoSize = false;
			this.tsb_CreateFeeder.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsb_CreateFeeder.Image = ((System.Drawing.Image)(resources.GetObject("tsb_CreateFeeder.Image")));
			this.tsb_CreateFeeder.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsb_CreateFeeder.Name = "tsb_CreateFeeder";
			this.tsb_CreateFeeder.Size = new System.Drawing.Size(26, 26);
			this.tsb_CreateFeeder.Text = "toolStripButton18";
			this.tsb_CreateFeeder.ToolTipText = "Feeder 생성";
			this.tsb_CreateFeeder.Click += new System.EventHandler(this.tsb_CreateFeeder_Click);
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.그룹추가ToolStripMenuItem,
            this.선택된그룹삭제ToolStripMenuItem,
            this.feeder생성ToolStripMenuItem,
            this.toolStripSeparator1,
            this.셀복사ToolStripMenuItem,
            this.연속데이터채우기ToolStripMenuItem});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(179, 120);
			// 
			// 그룹추가ToolStripMenuItem
			// 
			this.그룹추가ToolStripMenuItem.Name = "그룹추가ToolStripMenuItem";
			this.그룹추가ToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
			this.그룹추가ToolStripMenuItem.Text = "그룹 추가";
			this.그룹추가ToolStripMenuItem.Click += new System.EventHandler(this.그룹추가ToolStripMenuItem_Click);
			// 
			// 선택된그룹삭제ToolStripMenuItem
			// 
			this.선택된그룹삭제ToolStripMenuItem.Name = "선택된그룹삭제ToolStripMenuItem";
			this.선택된그룹삭제ToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
			this.선택된그룹삭제ToolStripMenuItem.Text = "선택된 그룹 삭제";
			this.선택된그룹삭제ToolStripMenuItem.Click += new System.EventHandler(this.선택된그룹삭제ToolStripMenuItem_Click);
			// 
			// feeder생성ToolStripMenuItem
			// 
			this.feeder생성ToolStripMenuItem.Name = "feeder생성ToolStripMenuItem";
			this.feeder생성ToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
			this.feeder생성ToolStripMenuItem.Text = "Feeder 생성";
			this.feeder생성ToolStripMenuItem.Click += new System.EventHandler(this.feeder생성ToolStripMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(175, 6);
			// 
			// 셀복사ToolStripMenuItem
			// 
			this.셀복사ToolStripMenuItem.Name = "셀복사ToolStripMenuItem";
			this.셀복사ToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
			this.셀복사ToolStripMenuItem.Text = "셀 복사";
			this.셀복사ToolStripMenuItem.Click += new System.EventHandler(this.셀복사ToolStripMenuItem_Click);
			// 
			// 연속데이터채우기ToolStripMenuItem
			// 
			this.연속데이터채우기ToolStripMenuItem.Name = "연속데이터채우기ToolStripMenuItem";
			this.연속데이터채우기ToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
			this.연속데이터채우기ToolStripMenuItem.Text = "연속 데이터 채우기";
			this.연속데이터채우기ToolStripMenuItem.Click += new System.EventHandler(this.연속데이터채우기ToolStripMenuItem_Click);
			// 
			// GroupInfo
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.toolStrip1);
			this.Name = "GroupInfo";
			this.Size = new System.Drawing.Size(742, 576);
			this.Load += new System.EventHandler(this.GroupInfo_Load);
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.groupBindingSource)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dataSet1)).EndInit();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.contextMenuStrip1.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource groupBindingSource;
        private Data.DataSet1 dataSet1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox tstb;
        private System.Windows.Forms.ToolStripButton tsb_AddGroup;
        private System.Windows.Forms.ToolStripButton tsb_DeleteGroup;
        private System.Windows.Forms.ToolStripButton tsb_CreateFeeder;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 그룹추가ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 선택된그룹삭제ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem feeder생성ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem 셀복사ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 연속데이터채우기ToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn groupIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn visibleDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn groupNoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn groupNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn feederQtyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn ratingDataGridViewComboBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn incomingTypeDataGridViewComboBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn leftToRightDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn incomingPtExistsDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn busPtExistsDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn busDuctExistsDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn incomingPtDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn kADataGridViewTextBoxColumn;
    }
}
