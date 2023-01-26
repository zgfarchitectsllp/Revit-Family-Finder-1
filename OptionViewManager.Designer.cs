namespace ZGF.Revit
{
    partial class OptionViewManager
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.gridViews = new System.Windows.Forms.DataGridView();
            this.textBoxSearchTerms = new System.Windows.Forms.TextBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonClearSearch = new System.Windows.Forms.Button();
            this.groupBoxRowDisplay = new System.Windows.Forms.GroupBox();
            this.labelViewCount = new System.Windows.Forms.Label();
            this.checkedListBoxOptionSets = new System.Windows.Forms.CheckedListBox();
            this.treeViewPreview = new System.Windows.Forms.TreeView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.colCheckedRow = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colViewName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colViewType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colViewSystemType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colViewTitleOnSheet = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colViewScale = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colViewPhase = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gridViews)).BeginInit();
            this.groupBoxRowDisplay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(932, 925);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(114, 38);
            this.buttonCancel.TabIndex = 0;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // gridViews
            // 
            this.gridViews.AllowUserToAddRows = false;
            this.gridViews.AllowUserToDeleteRows = false;
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
            this.colCheckedRow,
            this.colViewName,
            this.colViewType,
            this.colViewSystemType,
            this.colViewTitleOnSheet,
            this.colViewScale,
            this.colViewPhase});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridViews.DefaultCellStyle = dataGridViewCellStyle3;
            this.gridViews.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridViews.Location = new System.Drawing.Point(0, 0);
            this.gridViews.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gridViews.MultiSelect = false;
            this.gridViews.Name = "gridViews";
            this.gridViews.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridViews.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.gridViews.RowHeadersVisible = false;
            this.gridViews.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.gridViews.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridViews.Size = new System.Drawing.Size(1161, 459);
            this.gridViews.TabIndex = 1;
            this.gridViews.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridViews_CellContentClick);
            this.gridViews.MouseHover += new System.EventHandler(this.gridViews_MouseHover);
            // 
            // textBoxSearchTerms
            // 
            this.textBoxSearchTerms.BackColor = System.Drawing.Color.Yellow;
            this.textBoxSearchTerms.Location = new System.Drawing.Point(12, 29);
            this.textBoxSearchTerms.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxSearchTerms.Name = "textBoxSearchTerms";
            this.textBoxSearchTerms.Size = new System.Drawing.Size(230, 26);
            this.textBoxSearchTerms.TabIndex = 2;
            this.textBoxSearchTerms.TextChanged += new System.EventHandler(this.textBoxSearchTerms_TextChanged);
            this.textBoxSearchTerms.MouseHover += new System.EventHandler(this.textBoxSearchTerms_MouseHover);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.Location = new System.Drawing.Point(1054, 925);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(114, 38);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonClearSearch
            // 
            this.buttonClearSearch.BackgroundImage = global::ZGF.Revit.Properties.Resources.Clear_Search;
            this.buttonClearSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonClearSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonClearSearch.ForeColor = System.Drawing.SystemColors.Control;
            this.buttonClearSearch.Location = new System.Drawing.Point(253, 29);
            this.buttonClearSearch.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonClearSearch.Name = "buttonClearSearch";
            this.buttonClearSearch.Size = new System.Drawing.Size(22, 28);
            this.buttonClearSearch.TabIndex = 7;
            this.buttonClearSearch.TabStop = false;
            this.buttonClearSearch.Text = "...";
            this.buttonClearSearch.UseVisualStyleBackColor = true;
            this.buttonClearSearch.Click += new System.EventHandler(this.buttonClearSearch_Click);
            // 
            // groupBoxRowDisplay
            // 
            this.groupBoxRowDisplay.Controls.Add(this.textBoxSearchTerms);
            this.groupBoxRowDisplay.Controls.Add(this.buttonClearSearch);
            this.groupBoxRowDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxRowDisplay.Location = new System.Drawing.Point(0, 0);
            this.groupBoxRowDisplay.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxRowDisplay.Name = "groupBoxRowDisplay";
            this.groupBoxRowDisplay.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxRowDisplay.Size = new System.Drawing.Size(1161, 73);
            this.groupBoxRowDisplay.TabIndex = 10;
            this.groupBoxRowDisplay.TabStop = false;
            this.groupBoxRowDisplay.Text = "View Quick Find";
            // 
            // labelViewCount
            // 
            this.labelViewCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelViewCount.AutoSize = true;
            this.labelViewCount.Location = new System.Drawing.Point(15, 928);
            this.labelViewCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelViewCount.Name = "labelViewCount";
            this.labelViewCount.Size = new System.Drawing.Size(47, 20);
            this.labelViewCount.TabIndex = 4;
            this.labelViewCount.Text = "views";
            // 
            // checkedListBoxOptionSets
            // 
            this.checkedListBoxOptionSets.CheckOnClick = true;
            this.checkedListBoxOptionSets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBoxOptionSets.FormattingEnabled = true;
            this.checkedListBoxOptionSets.IntegralHeight = false;
            this.checkedListBoxOptionSets.Location = new System.Drawing.Point(0, 0);
            this.checkedListBoxOptionSets.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkedListBoxOptionSets.Name = "checkedListBoxOptionSets";
            this.checkedListBoxOptionSets.Size = new System.Drawing.Size(198, 224);
            this.checkedListBoxOptionSets.TabIndex = 12;
            this.checkedListBoxOptionSets.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBoxOptionSets_ItemCheck);
            // 
            // treeViewPreview
            // 
            this.treeViewPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewPreview.Location = new System.Drawing.Point(0, 0);
            this.treeViewPreview.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.treeViewPreview.Name = "treeViewPreview";
            this.treeViewPreview.ShowPlusMinus = false;
            this.treeViewPreview.Size = new System.Drawing.Size(957, 365);
            this.treeViewPreview.TabIndex = 13;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(8, 8);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer4);
            this.splitContainer1.Panel2MinSize = 50;
            this.splitContainer1.Size = new System.Drawing.Size(1161, 908);
            this.splitContainer1.SplitterDistance = 365;
            this.splitContainer1.SplitterWidth = 6;
            this.splitContainer1.TabIndex = 15;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer3);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.treeViewPreview);
            this.splitContainer2.Size = new System.Drawing.Size(1161, 365);
            this.splitContainer2.SplitterDistance = 198;
            this.splitContainer2.SplitterWidth = 6;
            this.splitContainer2.TabIndex = 0;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.textBox1);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.checkedListBoxOptionSets);
            this.splitContainer3.Size = new System.Drawing.Size(198, 365);
            this.splitContainer3.SplitterDistance = 135;
            this.splitContainer3.SplitterWidth = 6;
            this.splitContainer3.TabIndex = 0;
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(198, 135);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "1) Select Option Sets below to create views.\r\n\r\n2) Select views to be duplicated." +
    " Each view will be duplicated for each checked option set and its options.";
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.IsSplitterFixed = true;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.groupBoxRowDisplay);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.gridViews);
            this.splitContainer4.Size = new System.Drawing.Size(1161, 537);
            this.splitContainer4.SplitterDistance = 73;
            this.splitContainer4.SplitterWidth = 5;
            this.splitContainer4.TabIndex = 11;
            // 
            // colCheckedRow
            // 
            this.colCheckedRow.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colCheckedRow.DataPropertyName = "Selected";
            dataGridViewCellStyle2.NullValue = false;
            this.colCheckedRow.DefaultCellStyle = dataGridViewCellStyle2;
            this.colCheckedRow.HeaderText = "";
            this.colCheckedRow.MinimumWidth = 30;
            this.colCheckedRow.Name = "colCheckedRow";
            this.colCheckedRow.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colCheckedRow.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colCheckedRow.Width = 30;
            // 
            // colViewName
            // 
            this.colViewName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colViewName.DataPropertyName = "Name";
            this.colViewName.FillWeight = 200F;
            this.colViewName.HeaderText = "Name";
            this.colViewName.MinimumWidth = 100;
            this.colViewName.Name = "colViewName";
            this.colViewName.Width = 603;
            // 
            // colViewType
            // 
            this.colViewType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colViewType.DataPropertyName = "ViewTypeName";
            this.colViewType.HeaderText = "View Type";
            this.colViewType.MinimumWidth = 100;
            this.colViewType.Name = "colViewType";
            // 
            // colViewSystemType
            // 
            this.colViewSystemType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colViewSystemType.DataPropertyName = "ViewTypeSystemName";
            this.colViewSystemType.HeaderText = "System Type";
            this.colViewSystemType.MinimumWidth = 100;
            this.colViewSystemType.Name = "colViewSystemType";
            // 
            // colViewTitleOnSheet
            // 
            this.colViewTitleOnSheet.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colViewTitleOnSheet.DataPropertyName = "NameOnSheet";
            this.colViewTitleOnSheet.HeaderText = "Title on Sht";
            this.colViewTitleOnSheet.MinimumWidth = 10;
            this.colViewTitleOnSheet.Name = "colViewTitleOnSheet";
            this.colViewTitleOnSheet.Width = 86;
            // 
            // colViewScale
            // 
            this.colViewScale.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colViewScale.DataPropertyName = "Scale";
            this.colViewScale.HeaderText = "Scale";
            this.colViewScale.MinimumWidth = 10;
            this.colViewScale.Name = "colViewScale";
            this.colViewScale.Width = 59;
            // 
            // colViewPhase
            // 
            this.colViewPhase.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colViewPhase.DataPropertyName = "Phase";
            this.colViewPhase.HeaderText = "Phase";
            this.colViewPhase.MinimumWidth = 10;
            this.colViewPhase.Name = "colViewPhase";
            // 
            // OptionViewManager
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(1179, 974);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.labelViewCount);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(792, 580);
            this.Name = "OptionViewManager";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Option View Maker";
            ((System.ComponentModel.ISupportInitialize)(this.gridViews)).EndInit();
            this.groupBoxRowDisplay.ResumeLayout(false);
            this.groupBoxRowDisplay.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.PerformLayout();
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.DataGridView gridViews;
        private System.Windows.Forms.TextBox textBoxSearchTerms;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonClearSearch;
        private System.Windows.Forms.GroupBox groupBoxRowDisplay;
        private System.Windows.Forms.Label labelViewCount;
        private System.Windows.Forms.CheckedListBox checkedListBoxOptionSets;
        private System.Windows.Forms.TreeView treeViewPreview;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colCheckedRow;
        private System.Windows.Forms.DataGridViewTextBoxColumn colViewName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colViewType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colViewSystemType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colViewTitleOnSheet;
        private System.Windows.Forms.DataGridViewTextBoxColumn colViewScale;
        private System.Windows.Forms.DataGridViewTextBoxColumn colViewPhase;
    }
}