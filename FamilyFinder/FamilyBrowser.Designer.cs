namespace ZGF.Revit
{
    partial class FamilyBrowser
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FamilyBrowser));
            this.comboBoxQuickFind = new System.Windows.Forms.ComboBox();
            this.labelListCount = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.gridFamilies = new System.Windows.Forms.DataGridView();
            this.colFamily = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFileType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colButtonEditFamily = new System.Windows.Forms.DataGridViewImageColumn();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.pictureBoxPreview = new System.Windows.Forms.PictureBox();
            this.dataGridCategories = new System.Windows.Forms.DataGridView();
            this.dgChecked = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgCategoryName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuCategories = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem_All = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_None = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_Invert = new System.Windows.Forms.ToolStripMenuItem();
            this.labelSearchInstructions = new System.Windows.Forms.Label();
            this.radioButtonLibrary = new System.Windows.Forms.RadioButton();
            this.radioButtonRevitModel = new System.Windows.Forms.RadioButton();
            this.labelTypeDescrip = new System.Windows.Forms.Label();
            this.buttonSelectInverse = new System.Windows.Forms.Button();
            this.buttonSelectNone = new System.Windows.Forms.Button();
            this.buttonSelectAll = new System.Windows.Forms.Button();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.buttonClearSearch = new System.Windows.Forms.Button();
            this.pictureBoxZgfLogo = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridFamilies)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridCategories)).BeginInit();
            this.contextMenuCategories.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxZgfLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBoxQuickFind
            // 
            this.comboBoxQuickFind.BackColor = System.Drawing.Color.Cornsilk;
            this.comboBoxQuickFind.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxQuickFind.Location = new System.Drawing.Point(9, 18);
            this.comboBoxQuickFind.Name = "comboBoxQuickFind";
            this.comboBoxQuickFind.Size = new System.Drawing.Size(220, 24);
            this.comboBoxQuickFind.TabIndex = 1;
            this.comboBoxQuickFind.TextUpdate += new System.EventHandler(this.comboBoxQuickFind_TextUpdate);
            this.comboBoxQuickFind.KeyUp += new System.Windows.Forms.KeyEventHandler(this.comboBoxQuickFind_KeyUp);
            this.comboBoxQuickFind.MouseEnter += new System.EventHandler(this.comboBoxQuickFind_MouseEnter);
            this.comboBoxQuickFind.MouseUp += new System.Windows.Forms.MouseEventHandler(this.comboBoxQuickFind_MouseUp);
            // 
            // labelListCount
            // 
            this.labelListCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelListCount.AutoSize = true;
            this.labelListCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelListCount.Location = new System.Drawing.Point(236, 503);
            this.labelListCount.Name = "labelListCount";
            this.labelListCount.Size = new System.Drawing.Size(58, 15);
            this.labelListCount.TabIndex = 2;
            this.labelListCount.Text = "50 of 100";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCancel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonCancel.Location = new System.Drawing.Point(632, 484);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(92, 34);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.TabStop = false;
            this.buttonCancel.Text = "Close";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonOK.Location = new System.Drawing.Point(731, 484);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(92, 34);
            this.buttonOK.TabIndex = 3;
            this.buttonOK.Text = "Place";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // gridFamilies
            // 
            this.gridFamilies.AllowUserToAddRows = false;
            this.gridFamilies.AllowUserToDeleteRows = false;
            this.gridFamilies.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridFamilies.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.gridFamilies.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            this.gridFamilies.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridFamilies.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridFamilies.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.gridFamilies.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridFamilies.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colFamily,
            this.colType,
            this.colCategory,
            this.colFileType,
            this.colButtonEditFamily});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridFamilies.DefaultCellStyle = dataGridViewCellStyle3;
            this.gridFamilies.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridFamilies.Location = new System.Drawing.Point(0, 0);
            this.gridFamilies.MultiSelect = false;
            this.gridFamilies.Name = "gridFamilies";
            this.gridFamilies.ReadOnly = true;
            this.gridFamilies.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridFamilies.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.gridFamilies.RowHeadersVisible = false;
            this.gridFamilies.RowHeadersWidth = 51;
            this.gridFamilies.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridFamilies.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.gridFamilies.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.gridFamilies.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridFamilies.Size = new System.Drawing.Size(599, 424);
            this.gridFamilies.TabIndex = 2;
            this.gridFamilies.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridFamilies_CellContentClick);
            this.gridFamilies.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridFamilies_CellDoubleClick);
            this.gridFamilies.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.gridFamilies_CellPainting);
            this.gridFamilies.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridFamilies_RowEnter);
            this.gridFamilies.RowLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridFamilies_RowLeave);
            this.gridFamilies.Sorted += new System.EventHandler(this.gridFamilies_Sorted);
            this.gridFamilies.Enter += new System.EventHandler(this.gridFamilies_Enter);
            this.gridFamilies.KeyUp += new System.Windows.Forms.KeyEventHandler(this.gridFamilies_KeyUp);
            // 
            // colFamily
            // 
            this.colFamily.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colFamily.DataPropertyName = "FamilyName";
            this.colFamily.FillWeight = 40F;
            this.colFamily.HeaderText = "Family";
            this.colFamily.MaxInputLength = 256;
            this.colFamily.MinimumWidth = 6;
            this.colFamily.Name = "colFamily";
            this.colFamily.ReadOnly = true;
            // 
            // colType
            // 
            this.colType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colType.DataPropertyName = "TypeName";
            this.colType.FillWeight = 40F;
            this.colType.HeaderText = "Type";
            this.colType.MaxInputLength = 256;
            this.colType.MinimumWidth = 100;
            this.colType.Name = "colType";
            this.colType.ReadOnly = true;
            // 
            // colCategory
            // 
            this.colCategory.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colCategory.DataPropertyName = "CategoryName";
            this.colCategory.FillWeight = 20F;
            this.colCategory.HeaderText = "Category";
            this.colCategory.MaxInputLength = 512;
            this.colCategory.MinimumWidth = 100;
            this.colCategory.Name = "colCategory";
            this.colCategory.ReadOnly = true;
            // 
            // colFileType
            // 
            this.colFileType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colFileType.FillWeight = 20F;
            this.colFileType.HeaderText = "File Type";
            this.colFileType.MaxInputLength = 256;
            this.colFileType.MinimumWidth = 100;
            this.colFileType.Name = "colFileType";
            this.colFileType.ReadOnly = true;
            // 
            // colButtonEditFamily
            // 
            this.colButtonEditFamily.HeaderText = "";
            this.colButtonEditFamily.Image = global::ZGF.Revit.Properties.Resources.EditFamilyBlank;
            this.colButtonEditFamily.MinimumWidth = 6;
            this.colButtonEditFamily.Name = "colButtonEditFamily";
            this.colButtonEditFamily.ReadOnly = true;
            this.colButtonEditFamily.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colButtonEditFamily.ToolTipText = "Edit...";
            this.colButtonEditFamily.Width = 20;
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainerMain.IsSplitterFixed = true;
            this.splitContainerMain.Location = new System.Drawing.Point(9, 49);
            this.splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.splitContainer2);
            this.splitContainerMain.Panel1MinSize = 100;
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.gridFamilies);
            this.splitContainerMain.Size = new System.Drawing.Size(823, 424);
            this.splitContainerMain.SplitterDistance = 220;
            this.splitContainerMain.TabIndex = 2;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.pictureBoxPreview);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.dataGridCategories);
            this.splitContainer2.Size = new System.Drawing.Size(220, 424);
            this.splitContainer2.SplitterDistance = 140;
            this.splitContainer2.TabIndex = 0;
            this.splitContainer2.TabStop = false;
            // 
            // pictureBoxPreview
            // 
            this.pictureBoxPreview.BackColor = System.Drawing.Color.White;
            this.pictureBoxPreview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBoxPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxPreview.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxPreview.Name = "pictureBoxPreview";
            this.pictureBoxPreview.Padding = new System.Windows.Forms.Padding(26, 0, 26, 0);
            this.pictureBoxPreview.Size = new System.Drawing.Size(220, 140);
            this.pictureBoxPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBoxPreview.TabIndex = 4;
            this.pictureBoxPreview.TabStop = false;
            // 
            // dataGridCategories
            // 
            this.dataGridCategories.AllowUserToAddRows = false;
            this.dataGridCategories.AllowUserToDeleteRows = false;
            this.dataGridCategories.AllowUserToResizeColumns = false;
            this.dataGridCategories.AllowUserToResizeRows = false;
            this.dataGridCategories.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            this.dataGridCategories.BackgroundColor = System.Drawing.Color.White;
            this.dataGridCategories.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridCategories.ColumnHeadersVisible = false;
            this.dataGridCategories.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgChecked,
            this.dgCategoryName});
            this.dataGridCategories.ContextMenuStrip = this.contextMenuCategories;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridCategories.DefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridCategories.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridCategories.GridColor = System.Drawing.Color.White;
            this.dataGridCategories.Location = new System.Drawing.Point(0, 0);
            this.dataGridCategories.Name = "dataGridCategories";
            this.dataGridCategories.ReadOnly = true;
            this.dataGridCategories.RowHeadersVisible = false;
            this.dataGridCategories.RowTemplate.Height = 24;
            this.dataGridCategories.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridCategories.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridCategories.Size = new System.Drawing.Size(220, 280);
            this.dataGridCategories.TabIndex = 3;
            this.dataGridCategories.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridCategories_CellClick);
            this.dataGridCategories.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridCategories_CellEndEdit);
            this.dataGridCategories.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridCategories_CellValueChanged);
            this.dataGridCategories.CurrentCellChanged += new System.EventHandler(this.dataGridCategories_CurrentCellChanged);
            this.dataGridCategories.CurrentCellDirtyStateChanged += new System.EventHandler(this.dataGridCategories_CurrentCellDirtyStateChanged);
            this.dataGridCategories.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dataGridCategories_MouseUp);
            // 
            // dgChecked
            // 
            this.dgChecked.DataPropertyName = "IsChecked";
            this.dgChecked.FalseValue = "false";
            this.dgChecked.HeaderText = "Selected";
            this.dgChecked.IndeterminateValue = "false";
            this.dgChecked.MinimumWidth = 25;
            this.dgChecked.Name = "dgChecked";
            this.dgChecked.ReadOnly = true;
            this.dgChecked.TrueValue = "true";
            this.dgChecked.Width = 25;
            // 
            // dgCategoryName
            // 
            this.dgCategoryName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgCategoryName.DataPropertyName = "Name";
            this.dgCategoryName.HeaderText = "Category";
            this.dgCategoryName.Name = "dgCategoryName";
            this.dgCategoryName.ReadOnly = true;
            this.dgCategoryName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // contextMenuCategories
            // 
            this.contextMenuCategories.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_All,
            this.toolStripMenuItem_None,
            this.toolStripMenuItem_Invert});
            this.contextMenuCategories.Name = "contextMenCategories";
            this.contextMenuCategories.ShowImageMargin = false;
            this.contextMenuCategories.Size = new System.Drawing.Size(130, 70);
            // 
            // toolStripMenuItem_All
            // 
            this.toolStripMenuItem_All.Name = "toolStripMenuItem_All";
            this.toolStripMenuItem_All.Size = new System.Drawing.Size(129, 22);
            this.toolStripMenuItem_All.Text = "Select ALL";
            this.toolStripMenuItem_All.Click += new System.EventHandler(this.toolStripMenuItem_All_Click);
            // 
            // toolStripMenuItem_None
            // 
            this.toolStripMenuItem_None.Name = "toolStripMenuItem_None";
            this.toolStripMenuItem_None.Size = new System.Drawing.Size(129, 22);
            this.toolStripMenuItem_None.Text = "Select NONE";
            this.toolStripMenuItem_None.Click += new System.EventHandler(this.toolStripMenuItem_None_Click);
            // 
            // toolStripMenuItem_Invert
            // 
            this.toolStripMenuItem_Invert.Name = "toolStripMenuItem_Invert";
            this.toolStripMenuItem_Invert.Size = new System.Drawing.Size(129, 22);
            this.toolStripMenuItem_Invert.Text = "Invert selection";
            this.toolStripMenuItem_Invert.Click += new System.EventHandler(this.toolStripMenuItem_Invert_Click);
            // 
            // labelSearchInstructions
            // 
            this.labelSearchInstructions.AutoSize = true;
            this.labelSearchInstructions.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSearchInstructions.Location = new System.Drawing.Point(264, 23);
            this.labelSearchInstructions.Name = "labelSearchInstructions";
            this.labelSearchInstructions.Size = new System.Drawing.Size(47, 15);
            this.labelSearchInstructions.TabIndex = 18;
            this.labelSearchInstructions.Text = "label1";
            // 
            // radioButtonLibrary
            // 
            this.radioButtonLibrary.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButtonLibrary.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButtonLibrary.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonLibrary.Location = new System.Drawing.Point(731, 9);
            this.radioButtonLibrary.Name = "radioButtonLibrary";
            this.radioButtonLibrary.Size = new System.Drawing.Size(92, 34);
            this.radioButtonLibrary.TabIndex = 25;
            this.radioButtonLibrary.Text = "Library";
            this.radioButtonLibrary.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButtonLibrary.UseVisualStyleBackColor = true;
            this.radioButtonLibrary.CheckedChanged += new System.EventHandler(this.radioButtonRevitContentLibrarySwitch_CheckedChanged);
            this.radioButtonLibrary.MouseEnter += new System.EventHandler(this.buttonSelectCategories_MouseEnter);
            // 
            // radioButtonRevitModel
            // 
            this.radioButtonRevitModel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButtonRevitModel.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButtonRevitModel.Checked = true;
            this.radioButtonRevitModel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonRevitModel.Location = new System.Drawing.Point(632, 9);
            this.radioButtonRevitModel.Name = "radioButtonRevitModel";
            this.radioButtonRevitModel.Size = new System.Drawing.Size(92, 34);
            this.radioButtonRevitModel.TabIndex = 24;
            this.radioButtonRevitModel.TabStop = true;
            this.radioButtonRevitModel.Text = "Model";
            this.radioButtonRevitModel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButtonRevitModel.UseVisualStyleBackColor = true;
            this.radioButtonRevitModel.CheckedChanged += new System.EventHandler(this.radioButtonRevitContentLibrarySwitch_CheckedChanged);
            this.radioButtonRevitModel.MouseEnter += new System.EventHandler(this.buttonSelectCategories_MouseEnter);
            // 
            // labelTypeDescrip
            // 
            this.labelTypeDescrip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelTypeDescrip.AutoSize = true;
            this.labelTypeDescrip.Location = new System.Drawing.Point(236, 483);
            this.labelTypeDescrip.Name = "labelTypeDescrip";
            this.labelTypeDescrip.Size = new System.Drawing.Size(87, 13);
            this.labelTypeDescrip.TabIndex = 26;
            this.labelTypeDescrip.Text = "Type Description";
            // 
            // buttonSelectInverse
            // 
            this.buttonSelectInverse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSelectInverse.BackgroundImage = global::ZGF.Revit.Properties.Resources.Categories_Select_Inverse;
            this.buttonSelectInverse.Location = new System.Drawing.Point(192, 478);
            this.buttonSelectInverse.Name = "buttonSelectInverse";
            this.buttonSelectInverse.Size = new System.Drawing.Size(24, 24);
            this.buttonSelectInverse.TabIndex = 27;
            this.buttonSelectInverse.UseVisualStyleBackColor = true;
            this.buttonSelectInverse.Click += new System.EventHandler(this.buttonSelectCategories_Click);
            this.buttonSelectInverse.MouseEnter += new System.EventHandler(this.buttonSelectCategories_MouseEnter);
            // 
            // buttonSelectNone
            // 
            this.buttonSelectNone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSelectNone.BackgroundImage = global::ZGF.Revit.Properties.Resources.Categories_Select_None;
            this.buttonSelectNone.Location = new System.Drawing.Point(163, 478);
            this.buttonSelectNone.Name = "buttonSelectNone";
            this.buttonSelectNone.Size = new System.Drawing.Size(24, 24);
            this.buttonSelectNone.TabIndex = 27;
            this.buttonSelectNone.UseVisualStyleBackColor = true;
            this.buttonSelectNone.Click += new System.EventHandler(this.buttonSelectCategories_Click);
            this.buttonSelectNone.MouseEnter += new System.EventHandler(this.buttonSelectCategories_MouseEnter);
            // 
            // buttonSelectAll
            // 
            this.buttonSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSelectAll.BackgroundImage = global::ZGF.Revit.Properties.Resources.Categories_Select_All;
            this.buttonSelectAll.Location = new System.Drawing.Point(134, 478);
            this.buttonSelectAll.Name = "buttonSelectAll";
            this.buttonSelectAll.Size = new System.Drawing.Size(24, 24);
            this.buttonSelectAll.TabIndex = 27;
            this.buttonSelectAll.UseVisualStyleBackColor = true;
            this.buttonSelectAll.Click += new System.EventHandler(this.buttonSelectCategories_Click);
            this.buttonSelectAll.MouseEnter += new System.EventHandler(this.buttonSelectCategories_MouseEnter);
            // 
            // dataGridViewImageColumn1
            // 
            this.dataGridViewImageColumn1.HeaderText = "";
            this.dataGridViewImageColumn1.Image = global::ZGF.Revit.Properties.Resources.EditFamily;
            this.dataGridViewImageColumn1.MinimumWidth = 6;
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            this.dataGridViewImageColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewImageColumn1.ToolTipText = "Edit...";
            this.dataGridViewImageColumn1.Width = 20;
            // 
            // buttonClearSearch
            // 
            this.buttonClearSearch.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonClearSearch.BackgroundImage")));
            this.buttonClearSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonClearSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonClearSearch.ForeColor = System.Drawing.SystemColors.Control;
            this.buttonClearSearch.Location = new System.Drawing.Point(238, 21);
            this.buttonClearSearch.Name = "buttonClearSearch";
            this.buttonClearSearch.Size = new System.Drawing.Size(19, 18);
            this.buttonClearSearch.TabIndex = 6;
            this.buttonClearSearch.TabStop = false;
            this.buttonClearSearch.Text = "...";
            this.buttonClearSearch.UseVisualStyleBackColor = true;
            this.buttonClearSearch.Click += new System.EventHandler(this.buttonClearSearch_Click);
            // 
            // pictureBoxZgfLogo
            // 
            this.pictureBoxZgfLogo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBoxZgfLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBoxZgfLogo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxZgfLogo.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxZgfLogo.Image")));
            this.pictureBoxZgfLogo.Location = new System.Drawing.Point(9, 478);
            this.pictureBoxZgfLogo.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBoxZgfLogo.Name = "pictureBoxZgfLogo";
            this.pictureBoxZgfLogo.Size = new System.Drawing.Size(70, 46);
            this.pictureBoxZgfLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBoxZgfLogo.TabIndex = 15;
            this.pictureBoxZgfLogo.TabStop = false;
            this.pictureBoxZgfLogo.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBoxZgfLogo_MouseUp);
            // 
            // FamilyBrowser
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(836, 537);
            this.Controls.Add(this.buttonSelectAll);
            this.Controls.Add(this.buttonSelectNone);
            this.Controls.Add(this.buttonSelectInverse);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.radioButtonLibrary);
            this.Controls.Add(this.radioButtonRevitModel);
            this.Controls.Add(this.labelSearchInstructions);
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.buttonClearSearch);
            this.Controls.Add(this.pictureBoxZgfLogo);
            this.Controls.Add(this.comboBoxQuickFind);
            this.Controls.Add(this.labelListCount);
            this.Controls.Add(this.labelTypeDescrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(800, 462);
            this.Name = "FamilyBrowser";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Family Finder";
            ((System.ComponentModel.ISupportInitialize)(this.gridFamilies)).EndInit();
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridCategories)).EndInit();
            this.contextMenuCategories.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxZgfLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxQuickFind;
        private System.Windows.Forms.DataGridView gridFamilies;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label labelListCount;
        private System.Windows.Forms.PictureBox pictureBoxPreview;
        private System.Windows.Forms.PictureBox pictureBoxZgfLogo;
        private System.Windows.Forms.Button buttonClearSearch;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Label labelSearchInstructions;
        private System.Windows.Forms.RadioButton radioButtonLibrary;
        private System.Windows.Forms.RadioButton radioButtonRevitModel;
        private System.Windows.Forms.Label labelTypeDescrip;
        private System.Windows.Forms.ContextMenuStrip contextMenuCategories;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_All;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_None;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_Invert;
        private System.Windows.Forms.DataGridView dataGridCategories;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFamily;
        private System.Windows.Forms.DataGridViewTextBoxColumn colType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCategory;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFileType;
        private System.Windows.Forms.DataGridViewImageColumn colButtonEditFamily;
        private System.Windows.Forms.Button buttonSelectInverse;
        private System.Windows.Forms.Button buttonSelectNone;
        private System.Windows.Forms.Button buttonSelectAll;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgChecked;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgCategoryName;
    }
}