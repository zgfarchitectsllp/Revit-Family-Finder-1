namespace ZGF.Revit
{
    partial class ViewFinder
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
            this.buttonClose = new System.Windows.Forms.Button();
            this.gridViews = new System.Windows.Forms.DataGridView();
            this.colViewName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colViewType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colViewRefSheet = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colViewSystemType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colViewScale = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colViewDiscipline = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colViewPhase = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colViewTitleOnSheet = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonActivate = new System.Windows.Forms.Button();
            this.buttonPlaceOnSheet = new System.Windows.Forms.Button();
            this.groupBoxColumns = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBoxRememberLastSearch = new System.Windows.Forms.CheckBox();
            this.chkColViewTypeName = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkColTitleOnSheet = new System.Windows.Forms.CheckBox();
            this.chkColReferencingSheet = new System.Windows.Forms.CheckBox();
            this.chkColScale = new System.Windows.Forms.CheckBox();
            this.chkColPhaseName = new System.Windows.Forms.CheckBox();
            this.chkColDiscipline = new System.Windows.Forms.CheckBox();
            this.labelViewCount = new System.Windows.Forms.Label();
            this.elementHostThePreview = new System.Windows.Forms.Integration.ElementHost();
            this.chkPreview = new System.Windows.Forms.CheckBox();
            this.labelPreviewDisabled = new System.Windows.Forms.Label();
            this.pictureBoxZgfLogo = new System.Windows.Forms.PictureBox();
            this.buttonClearSearch = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.labelPreviewIsSchedule = new System.Windows.Forms.Label();
            this.comboBoxSearchTerms = new System.Windows.Forms.ComboBox();
            this.contextMenuViewFinder = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmItemOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmItemGoToSheet = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmItemClose = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmItemDuplicateViewFlyout = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmItemDuplicate = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmItemDuplicateWithDetailing = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmItemDuplicateAsDependent = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmItemDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmItemRename = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmItemAddToSheet = new System.Windows.Forms.ToolStripMenuItem();
            this.textBoxRenameView = new System.Windows.Forms.TextBox();
            this.chkCloseAfterActivateView = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridViews)).BeginInit();
            this.groupBoxColumns.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxZgfLogo)).BeginInit();
            this.contextMenuViewFinder.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClose.Location = new System.Drawing.Point(899, 499);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(76, 32);
            this.buttonClose.TabIndex = 0;
            this.buttonClose.TabStop = false;
            this.buttonClose.Text = "&Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // gridViews
            // 
            this.gridViews.AllowUserToAddRows = false;
            this.gridViews.AllowUserToDeleteRows = false;
            this.gridViews.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridViews.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            this.gridViews.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridViews.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridViews.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridViews.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colViewName,
            this.colViewType,
            this.colViewRefSheet,
            this.colViewSystemType,
            this.colViewScale,
            this.colViewDiscipline,
            this.colViewPhase,
            this.colViewTitleOnSheet});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridViews.DefaultCellStyle = dataGridViewCellStyle2;
            this.gridViews.Location = new System.Drawing.Point(171, 8);
            this.gridViews.MultiSelect = false;
            this.gridViews.Name = "gridViews";
            this.gridViews.ReadOnly = true;
            this.gridViews.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridViews.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.gridViews.RowHeadersVisible = false;
            this.gridViews.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.gridViews.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridViews.Size = new System.Drawing.Size(804, 483);
            this.gridViews.StandardTab = true;
            this.gridViews.TabIndex = 2;
            this.gridViews.CellContextMenuStripNeeded += new System.Windows.Forms.DataGridViewCellContextMenuStripNeededEventHandler(this.gridViews_CellContextMenuStripNeeded);
            this.gridViews.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridViews_CellDoubleClick);
            this.gridViews.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridViews_CellEnter);
            this.gridViews.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.gridViews_CellMouseUp);
            this.gridViews.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.gridViews_ColumnHeaderMouseClick);
            this.gridViews.Sorted += new System.EventHandler(this.gridViews_Sorted);
            this.gridViews.Click += new System.EventHandler(this.gridViews_Click);
            this.gridViews.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gridViews_KeyDown);
            this.gridViews.KeyUp += new System.Windows.Forms.KeyEventHandler(this.gridViews_KeyUp);
            this.gridViews.MouseHover += new System.EventHandler(this.gridViews_MouseHover);
            // 
            // colViewName
            // 
            this.colViewName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colViewName.DataPropertyName = "Name";
            this.colViewName.HeaderText = "Name";
            this.colViewName.MinimumWidth = 100;
            this.colViewName.Name = "colViewName";
            this.colViewName.ReadOnly = true;
            // 
            // colViewType
            // 
            this.colViewType.DataPropertyName = "ViewTypeName";
            this.colViewType.HeaderText = "View Type";
            this.colViewType.MinimumWidth = 100;
            this.colViewType.Name = "colViewType";
            this.colViewType.ReadOnly = true;
            // 
            // colViewRefSheet
            // 
            this.colViewRefSheet.DataPropertyName = "ReferenceSheet";
            this.colViewRefSheet.HeaderText = "Ref Sheet";
            this.colViewRefSheet.MinimumWidth = 10;
            this.colViewRefSheet.Name = "colViewRefSheet";
            this.colViewRefSheet.ReadOnly = true;
            this.colViewRefSheet.Width = 80;
            // 
            // colViewSystemType
            // 
            this.colViewSystemType.DataPropertyName = "ViewTypeSystemName";
            this.colViewSystemType.HeaderText = "System Type";
            this.colViewSystemType.MinimumWidth = 100;
            this.colViewSystemType.Name = "colViewSystemType";
            this.colViewSystemType.ReadOnly = true;
            // 
            // colViewScale
            // 
            this.colViewScale.DataPropertyName = "Scale";
            this.colViewScale.HeaderText = "Scale";
            this.colViewScale.MinimumWidth = 10;
            this.colViewScale.Name = "colViewScale";
            this.colViewScale.ReadOnly = true;
            this.colViewScale.Width = 59;
            // 
            // colViewDiscipline
            // 
            this.colViewDiscipline.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colViewDiscipline.DataPropertyName = "Discipline";
            this.colViewDiscipline.HeaderText = "Discipline";
            this.colViewDiscipline.MinimumWidth = 10;
            this.colViewDiscipline.Name = "colViewDiscipline";
            this.colViewDiscipline.ReadOnly = true;
            this.colViewDiscipline.Width = 77;
            // 
            // colViewPhase
            // 
            this.colViewPhase.DataPropertyName = "Phase";
            this.colViewPhase.HeaderText = "Phase";
            this.colViewPhase.MinimumWidth = 10;
            this.colViewPhase.Name = "colViewPhase";
            this.colViewPhase.ReadOnly = true;
            this.colViewPhase.Width = 62;
            // 
            // colViewTitleOnSheet
            // 
            this.colViewTitleOnSheet.DataPropertyName = "NameOnSheet";
            this.colViewTitleOnSheet.HeaderText = "Title on Sht";
            this.colViewTitleOnSheet.MinimumWidth = 10;
            this.colViewTitleOnSheet.Name = "colViewTitleOnSheet";
            this.colViewTitleOnSheet.ReadOnly = true;
            this.colViewTitleOnSheet.Width = 86;
            // 
            // buttonActivate
            // 
            this.buttonActivate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonActivate.Location = new System.Drawing.Point(817, 499);
            this.buttonActivate.Name = "buttonActivate";
            this.buttonActivate.Size = new System.Drawing.Size(76, 32);
            this.buttonActivate.TabIndex = 2;
            this.buttonActivate.TabStop = false;
            this.buttonActivate.Text = "&Activate";
            this.buttonActivate.UseVisualStyleBackColor = true;
            this.buttonActivate.Click += new System.EventHandler(this.buttonActivate_Click);
            // 
            // buttonPlaceOnSheet
            // 
            this.buttonPlaceOnSheet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPlaceOnSheet.Location = new System.Drawing.Point(707, 499);
            this.buttonPlaceOnSheet.Name = "buttonPlaceOnSheet";
            this.buttonPlaceOnSheet.Size = new System.Drawing.Size(103, 32);
            this.buttonPlaceOnSheet.TabIndex = 3;
            this.buttonPlaceOnSheet.Text = "&Place on Sheet";
            this.buttonPlaceOnSheet.UseVisualStyleBackColor = true;
            this.buttonPlaceOnSheet.Click += new System.EventHandler(this.buttonPlaceOnSheet_Click);
            // 
            // groupBoxColumns
            // 
            this.groupBoxColumns.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBoxColumns.Controls.Add(this.label2);
            this.groupBoxColumns.Controls.Add(this.checkBoxRememberLastSearch);
            this.groupBoxColumns.Controls.Add(this.chkColViewTypeName);
            this.groupBoxColumns.Controls.Add(this.label1);
            this.groupBoxColumns.Controls.Add(this.chkColTitleOnSheet);
            this.groupBoxColumns.Controls.Add(this.chkColReferencingSheet);
            this.groupBoxColumns.Controls.Add(this.chkColScale);
            this.groupBoxColumns.Controls.Add(this.chkColPhaseName);
            this.groupBoxColumns.Controls.Add(this.chkColDiscipline);
            this.groupBoxColumns.Location = new System.Drawing.Point(8, 229);
            this.groupBoxColumns.Name = "groupBoxColumns";
            this.groupBoxColumns.Size = new System.Drawing.Size(157, 263);
            this.groupBoxColumns.TabIndex = 10;
            this.groupBoxColumns.TabStop = false;
            this.groupBoxColumns.Text = "Settings";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 46);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Columns to display:";
            // 
            // checkBoxRememberLastSearch
            // 
            this.checkBoxRememberLastSearch.AutoSize = true;
            this.checkBoxRememberLastSearch.Checked = true;
            this.checkBoxRememberLastSearch.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxRememberLastSearch.Location = new System.Drawing.Point(11, 20);
            this.checkBoxRememberLastSearch.Name = "checkBoxRememberLastSearch";
            this.checkBoxRememberLastSearch.Size = new System.Drawing.Size(131, 17);
            this.checkBoxRememberLastSearch.TabIndex = 11;
            this.checkBoxRememberLastSearch.TabStop = false;
            this.checkBoxRememberLastSearch.Text = "&Remember last search";
            this.checkBoxRememberLastSearch.UseVisualStyleBackColor = true;
            // 
            // chkColViewTypeName
            // 
            this.chkColViewTypeName.AutoSize = true;
            this.chkColViewTypeName.Location = new System.Drawing.Point(11, 67);
            this.chkColViewTypeName.Name = "chkColViewTypeName";
            this.chkColViewTypeName.Size = new System.Drawing.Size(76, 17);
            this.chkColViewTypeName.TabIndex = 11;
            this.chkColViewTypeName.TabStop = false;
            this.chkColViewTypeName.Text = "&View Type";
            this.chkColViewTypeName.UseVisualStyleBackColor = true;
            this.chkColViewTypeName.CheckedChanged += new System.EventHandler(this.chkColViewTypeName_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 177);
            this.label1.MaximumSize = new System.Drawing.Size(143, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 52);
            this.label1.TabIndex = 11;
            this.label1.Text = "Warning: Searching for Referencing Sheets may take a while in large projects.";
            // 
            // chkColTitleOnSheet
            // 
            this.chkColTitleOnSheet.AutoSize = true;
            this.chkColTitleOnSheet.Location = new System.Drawing.Point(11, 151);
            this.chkColTitleOnSheet.Name = "chkColTitleOnSheet";
            this.chkColTitleOnSheet.Size = new System.Drawing.Size(92, 17);
            this.chkColTitleOnSheet.TabIndex = 2;
            this.chkColTitleOnSheet.TabStop = false;
            this.chkColTitleOnSheet.Text = "&Title on Sheet";
            this.chkColTitleOnSheet.UseVisualStyleBackColor = true;
            this.chkColTitleOnSheet.CheckedChanged += new System.EventHandler(this.chkColTitleOnSheet_CheckedChanged);
            // 
            // chkColReferencingSheet
            // 
            this.chkColReferencingSheet.AutoSize = true;
            this.chkColReferencingSheet.Location = new System.Drawing.Point(11, 232);
            this.chkColReferencingSheet.Name = "chkColReferencingSheet";
            this.chkColReferencingSheet.Size = new System.Drawing.Size(115, 17);
            this.chkColReferencingSheet.TabIndex = 3;
            this.chkColReferencingSheet.TabStop = false;
            this.chkColReferencingSheet.Text = "R&eferencing Sheet";
            this.chkColReferencingSheet.UseVisualStyleBackColor = true;
            this.chkColReferencingSheet.CheckedChanged += new System.EventHandler(this.chkColReferencingSheet_CheckedChanged);
            // 
            // chkColScale
            // 
            this.chkColScale.AutoSize = true;
            this.chkColScale.Location = new System.Drawing.Point(11, 88);
            this.chkColScale.Name = "chkColScale";
            this.chkColScale.Size = new System.Drawing.Size(53, 17);
            this.chkColScale.TabIndex = 1;
            this.chkColScale.TabStop = false;
            this.chkColScale.Text = "&Scale";
            this.chkColScale.UseVisualStyleBackColor = true;
            this.chkColScale.CheckedChanged += new System.EventHandler(this.chkColScale_CheckedChanged);
            // 
            // chkColPhaseName
            // 
            this.chkColPhaseName.AutoSize = true;
            this.chkColPhaseName.Location = new System.Drawing.Point(11, 130);
            this.chkColPhaseName.Name = "chkColPhaseName";
            this.chkColPhaseName.Size = new System.Drawing.Size(56, 17);
            this.chkColPhaseName.TabIndex = 2;
            this.chkColPhaseName.TabStop = false;
            this.chkColPhaseName.Text = "P&hase";
            this.chkColPhaseName.UseVisualStyleBackColor = true;
            this.chkColPhaseName.CheckedChanged += new System.EventHandler(this.chkColPhaseName_CheckedChanged);
            // 
            // chkColDiscipline
            // 
            this.chkColDiscipline.AutoSize = true;
            this.chkColDiscipline.Location = new System.Drawing.Point(11, 109);
            this.chkColDiscipline.Name = "chkColDiscipline";
            this.chkColDiscipline.Size = new System.Drawing.Size(71, 17);
            this.chkColDiscipline.TabIndex = 0;
            this.chkColDiscipline.TabStop = false;
            this.chkColDiscipline.Text = "&Discipline";
            this.chkColDiscipline.UseVisualStyleBackColor = true;
            this.chkColDiscipline.CheckedChanged += new System.EventHandler(this.chkColDiscipline_CheckedChanged);
            // 
            // labelViewCount
            // 
            this.labelViewCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelViewCount.AutoSize = true;
            this.labelViewCount.Location = new System.Drawing.Point(175, 499);
            this.labelViewCount.Name = "labelViewCount";
            this.labelViewCount.Size = new System.Drawing.Size(34, 13);
            this.labelViewCount.TabIndex = 4;
            this.labelViewCount.Text = "views";
            // 
            // elementHostThePreview
            // 
            this.elementHostThePreview.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.elementHostThePreview.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.elementHostThePreview.Location = new System.Drawing.Point(8, 42);
            this.elementHostThePreview.Margin = new System.Windows.Forms.Padding(2);
            this.elementHostThePreview.Name = "elementHostThePreview";
            this.elementHostThePreview.Size = new System.Drawing.Size(157, 158);
            this.elementHostThePreview.TabIndex = 11;
            this.elementHostThePreview.TabStop = false;
            this.elementHostThePreview.Text = "elementHost1";
            this.elementHostThePreview.Child = null;
            // 
            // chkPreview
            // 
            this.chkPreview.AutoSize = true;
            this.chkPreview.Location = new System.Drawing.Point(11, 206);
            this.chkPreview.Margin = new System.Windows.Forms.Padding(2);
            this.chkPreview.Name = "chkPreview";
            this.chkPreview.Size = new System.Drawing.Size(64, 17);
            this.chkPreview.TabIndex = 12;
            this.chkPreview.TabStop = false;
            this.chkPreview.Text = "&Preview";
            this.chkPreview.UseVisualStyleBackColor = true;
            this.chkPreview.CheckedChanged += new System.EventHandler(this.chkPreview_CheckedChanged);
            // 
            // labelPreviewDisabled
            // 
            this.labelPreviewDisabled.AutoSize = true;
            this.labelPreviewDisabled.Location = new System.Drawing.Point(14, 46);
            this.labelPreviewDisabled.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelPreviewDisabled.Name = "labelPreviewDisabled";
            this.labelPreviewDisabled.Size = new System.Drawing.Size(68, 13);
            this.labelPreviewDisabled.TabIndex = 13;
            this.labelPreviewDisabled.Text = "Preview OFF";
            // 
            // pictureBoxZgfLogo
            // 
            this.pictureBoxZgfLogo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBoxZgfLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBoxZgfLogo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxZgfLogo.Image = global::ZGF.Revit.Properties.Resources.ZGF_Logo_ViewBrowser;
            this.pictureBoxZgfLogo.Location = new System.Drawing.Point(8, 497);
            this.pictureBoxZgfLogo.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBoxZgfLogo.Name = "pictureBoxZgfLogo";
            this.pictureBoxZgfLogo.Size = new System.Drawing.Size(157, 34);
            this.pictureBoxZgfLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBoxZgfLogo.TabIndex = 14;
            this.pictureBoxZgfLogo.TabStop = false;
            this.pictureBoxZgfLogo.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBoxZgfLogo_MouseUp);
            // 
            // buttonClearSearch
            // 
            this.buttonClearSearch.BackgroundImage = global::ZGF.Revit.Properties.Resources.Clear_Search;
            this.buttonClearSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonClearSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonClearSearch.ForeColor = System.Drawing.SystemColors.Control;
            this.buttonClearSearch.Location = new System.Drawing.Point(151, 19);
            this.buttonClearSearch.Name = "buttonClearSearch";
            this.buttonClearSearch.Size = new System.Drawing.Size(15, 18);
            this.buttonClearSearch.TabIndex = 7;
            this.buttonClearSearch.TabStop = false;
            this.buttonClearSearch.Text = "...";
            this.buttonClearSearch.UseVisualStyleBackColor = true;
            this.buttonClearSearch.Click += new System.EventHandler(this.buttonClearSearch_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 2);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "Search:";
            // 
            // labelPreviewIsSchedule
            // 
            this.labelPreviewIsSchedule.AutoSize = true;
            this.labelPreviewIsSchedule.Location = new System.Drawing.Point(14, 46);
            this.labelPreviewIsSchedule.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelPreviewIsSchedule.Name = "labelPreviewIsSchedule";
            this.labelPreviewIsSchedule.Size = new System.Drawing.Size(108, 13);
            this.labelPreviewIsSchedule.TabIndex = 16;
            this.labelPreviewIsSchedule.Text = "Preview not available";
            // 
            // comboBoxSearchTerms
            // 
            this.comboBoxSearchTerms.BackColor = System.Drawing.Color.Cornsilk;
            this.comboBoxSearchTerms.FormattingEnabled = true;
            this.comboBoxSearchTerms.Location = new System.Drawing.Point(8, 17);
            this.comboBoxSearchTerms.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxSearchTerms.Name = "comboBoxSearchTerms";
            this.comboBoxSearchTerms.Size = new System.Drawing.Size(141, 21);
            this.comboBoxSearchTerms.TabIndex = 1;
            this.comboBoxSearchTerms.TextChanged += new System.EventHandler(this.comboBoxSearchTerms_TextChanged);
            this.comboBoxSearchTerms.MouseHover += new System.EventHandler(this.comboBoxSearchTerms_MouseHover);
            // 
            // contextMenuViewFinder
            // 
            this.contextMenuViewFinder.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuViewFinder.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmItemOpen,
            this.tsmItemGoToSheet,
            this.tsmItemClose,
            this.toolStripSeparator1,
            this.tsmItemDuplicateViewFlyout,
            this.tsmItemDelete,
            this.tsmItemRename,
            this.toolStripSeparator2,
            this.tsmItemAddToSheet});
            this.contextMenuViewFinder.Name = "contextMenuViewFinder";
            this.contextMenuViewFinder.Size = new System.Drawing.Size(153, 170);
            // 
            // tsmItemOpen
            // 
            this.tsmItemOpen.Name = "tsmItemOpen";
            this.tsmItemOpen.Size = new System.Drawing.Size(152, 22);
            this.tsmItemOpen.Text = "Activate";
            this.tsmItemOpen.Click += new System.EventHandler(this.tsmItemOpen_Click);
            // 
            // tsmItemGoToSheet
            // 
            this.tsmItemGoToSheet.Name = "tsmItemGoToSheet";
            this.tsmItemGoToSheet.Size = new System.Drawing.Size(152, 22);
            this.tsmItemGoToSheet.Text = "Go to sheet";
            this.tsmItemGoToSheet.Click += new System.EventHandler(this.tsmItemGoToSheet_Click);
            // 
            // tsmItemClose
            // 
            this.tsmItemClose.Name = "tsmItemClose";
            this.tsmItemClose.Size = new System.Drawing.Size(152, 22);
            this.tsmItemClose.Text = "Close";
            this.tsmItemClose.Click += new System.EventHandler(this.tsmItemClose_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // tsmItemDuplicateViewFlyout
            // 
            this.tsmItemDuplicateViewFlyout.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmItemDuplicate,
            this.tsmItemDuplicateWithDetailing,
            this.tsmItemDuplicateAsDependent});
            this.tsmItemDuplicateViewFlyout.Name = "tsmItemDuplicateViewFlyout";
            this.tsmItemDuplicateViewFlyout.Size = new System.Drawing.Size(152, 22);
            this.tsmItemDuplicateViewFlyout.Text = "Duplicate View";
            // 
            // tsmItemDuplicate
            // 
            this.tsmItemDuplicate.Name = "tsmItemDuplicate";
            this.tsmItemDuplicate.Size = new System.Drawing.Size(200, 22);
            this.tsmItemDuplicate.Text = "Duplicate";
            this.tsmItemDuplicate.Click += new System.EventHandler(this.tsmItemDuplicate_Click);
            // 
            // tsmItemDuplicateWithDetailing
            // 
            this.tsmItemDuplicateWithDetailing.Name = "tsmItemDuplicateWithDetailing";
            this.tsmItemDuplicateWithDetailing.Size = new System.Drawing.Size(200, 22);
            this.tsmItemDuplicateWithDetailing.Text = "Duplicate with Detailing";
            this.tsmItemDuplicateWithDetailing.Click += new System.EventHandler(this.tsmItemDuplicateWithDetailing_Click);
            // 
            // tsmItemDuplicateAsDependent
            // 
            this.tsmItemDuplicateAsDependent.Name = "tsmItemDuplicateAsDependent";
            this.tsmItemDuplicateAsDependent.Size = new System.Drawing.Size(200, 22);
            this.tsmItemDuplicateAsDependent.Text = "Duplicate as Dependent";
            this.tsmItemDuplicateAsDependent.Click += new System.EventHandler(this.tsmItemDuplicateAsDependent_Click);
            // 
            // tsmItemDelete
            // 
            this.tsmItemDelete.Name = "tsmItemDelete";
            this.tsmItemDelete.Size = new System.Drawing.Size(152, 22);
            this.tsmItemDelete.Text = "Delete";
            this.tsmItemDelete.Click += new System.EventHandler(this.tsmItemDelete_Click);
            // 
            // tsmItemRename
            // 
            this.tsmItemRename.Name = "tsmItemRename";
            this.tsmItemRename.Size = new System.Drawing.Size(152, 22);
            this.tsmItemRename.Text = "Rename";
            this.tsmItemRename.Click += new System.EventHandler(this.tsmItemRename_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(149, 6);
            // 
            // tsmItemAddToSheet
            // 
            this.tsmItemAddToSheet.Name = "tsmItemAddToSheet";
            this.tsmItemAddToSheet.Size = new System.Drawing.Size(152, 22);
            this.tsmItemAddToSheet.Text = "Add to Sheet";
            this.tsmItemAddToSheet.Click += new System.EventHandler(this.buttonPlaceOnSheet_Click);
            // 
            // textBoxRenameView
            // 
            this.textBoxRenameView.Location = new System.Drawing.Point(253, 496);
            this.textBoxRenameView.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxRenameView.Name = "textBoxRenameView";
            this.textBoxRenameView.Size = new System.Drawing.Size(98, 20);
            this.textBoxRenameView.TabIndex = 17;
            this.textBoxRenameView.Visible = false;
            this.textBoxRenameView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxRenameView_KeyDown);
            this.textBoxRenameView.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBoxRenameView_KeyUp);
            this.textBoxRenameView.Leave += new System.EventHandler(this.textBoxRenameView_Leave);
            // 
            // chkCloseAfterActivateView
            // 
            this.chkCloseAfterActivateView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkCloseAfterActivateView.AutoSize = true;
            this.chkCloseAfterActivateView.Checked = true;
            this.chkCloseAfterActivateView.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCloseAfterActivateView.Location = new System.Drawing.Point(177, 517);
            this.chkCloseAfterActivateView.Name = "chkCloseAfterActivateView";
            this.chkCloseAfterActivateView.Size = new System.Drawing.Size(159, 17);
            this.chkCloseAfterActivateView.TabIndex = 18;
            this.chkCloseAfterActivateView.Text = "Close after activating a view";
            this.chkCloseAfterActivateView.UseVisualStyleBackColor = true;
            this.chkCloseAfterActivateView.Visible = false;
            this.chkCloseAfterActivateView.CheckedChanged += new System.EventHandler(this.chkCloseAfterActivateView_CheckedChanged);
            // 
            // ViewFinder
            // 
            this.AcceptButton = this.buttonActivate;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.buttonClose;
            this.ClientSize = new System.Drawing.Size(984, 545);
            this.Controls.Add(this.chkCloseAfterActivateView);
            this.Controls.Add(this.textBoxRenameView);
            this.Controls.Add(this.comboBoxSearchTerms);
            this.Controls.Add(this.labelPreviewIsSchedule);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.pictureBoxZgfLogo);
            this.Controls.Add(this.labelPreviewDisabled);
            this.Controls.Add(this.chkPreview);
            this.Controls.Add(this.elementHostThePreview);
            this.Controls.Add(this.groupBoxColumns);
            this.Controls.Add(this.labelViewCount);
            this.Controls.Add(this.buttonClearSearch);
            this.Controls.Add(this.gridViews);
            this.Controls.Add(this.buttonPlaceOnSheet);
            this.Controls.Add(this.buttonActivate);
            this.Controls.Add(this.buttonClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(648, 582);
            this.Name = "ViewFinder";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "View Finder";
            ((System.ComponentModel.ISupportInitialize)(this.gridViews)).EndInit();
            this.groupBoxColumns.ResumeLayout(false);
            this.groupBoxColumns.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxZgfLogo)).EndInit();
            this.contextMenuViewFinder.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.DataGridView gridViews;
        private System.Windows.Forms.Button buttonActivate;
        private System.Windows.Forms.Button buttonPlaceOnSheet;
        private System.Windows.Forms.Button buttonClearSearch;
        private System.Windows.Forms.GroupBox groupBoxColumns;
        private System.Windows.Forms.CheckBox chkColTitleOnSheet;
        private System.Windows.Forms.CheckBox chkColReferencingSheet;
        private System.Windows.Forms.CheckBox chkColScale;
        private System.Windows.Forms.CheckBox chkColPhaseName;
        private System.Windows.Forms.CheckBox chkColDiscipline;
        private System.Windows.Forms.CheckBox checkBoxRememberLastSearch;
        private System.Windows.Forms.Label labelViewCount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkColViewTypeName;
        private System.Windows.Forms.Integration.ElementHost elementHostThePreview;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkPreview;
        private System.Windows.Forms.Label labelPreviewDisabled;
        private System.Windows.Forms.PictureBox pictureBoxZgfLogo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelPreviewIsSchedule;
        private System.Windows.Forms.ComboBox comboBoxSearchTerms;
        private System.Windows.Forms.ContextMenuStrip contextMenuViewFinder;
        private System.Windows.Forms.ToolStripMenuItem tsmItemOpen;
        private System.Windows.Forms.ToolStripMenuItem tsmItemClose;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tsmItemDuplicateViewFlyout;
        private System.Windows.Forms.ToolStripMenuItem tsmItemDelete;
        private System.Windows.Forms.ToolStripMenuItem tsmItemRename;
        private System.Windows.Forms.ToolStripMenuItem tsmItemDuplicate;
        private System.Windows.Forms.ToolStripMenuItem tsmItemDuplicateWithDetailing;
        private System.Windows.Forms.ToolStripMenuItem tsmItemDuplicateAsDependent;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem tsmItemAddToSheet;
        private System.Windows.Forms.TextBox textBoxRenameView;
        private System.Windows.Forms.CheckBox chkCloseAfterActivateView;
        private System.Windows.Forms.ToolStripMenuItem tsmItemGoToSheet;
        private System.Windows.Forms.DataGridViewTextBoxColumn colViewName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colViewType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colViewRefSheet;
        private System.Windows.Forms.DataGridViewTextBoxColumn colViewSystemType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colViewScale;
        private System.Windows.Forms.DataGridViewTextBoxColumn colViewDiscipline;
        private System.Windows.Forms.DataGridViewTextBoxColumn colViewPhase;
        private System.Windows.Forms.DataGridViewTextBoxColumn colViewTitleOnSheet;
    }
}