namespace ZGF.Revit
{
    partial class SheetViewManager
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("LEVEL 1 PLAN");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("PLAN LEGEND");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("A2.1", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2});
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("LEVEL 2 PLAN");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("A2.2", new System.Windows.Forms.TreeNode[] {
            treeNode4});
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("A2.3");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("A3.1");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Sheets", new System.Windows.Forms.TreeNode[] {
            treeNode3,
            treeNode5,
            treeNode6,
            treeNode7});
            this.gridViews = new System.Windows.Forms.DataGridView();
            this.colViewName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colViewType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colViewScale = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colViewDiscipline = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colViewPhase = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colViewRefSheet = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colViewTitleOnSheet = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonClearSearch = new System.Windows.Forms.Button();
            this.textBoxSearchTerms = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.pictureBoxPreview = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridViews)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPreview)).BeginInit();
            this.SuspendLayout();
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
            this.colViewName,
            this.colViewType,
            this.colViewScale,
            this.colViewDiscipline,
            this.colViewPhase,
            this.colViewRefSheet,
            this.colViewTitleOnSheet});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridViews.DefaultCellStyle = dataGridViewCellStyle2;
            this.gridViews.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridViews.Location = new System.Drawing.Point(0, 0);
            this.gridViews.Margin = new System.Windows.Forms.Padding(4);
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
            this.gridViews.Size = new System.Drawing.Size(648, 464);
            this.gridViews.TabIndex = 2;
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
            this.colViewType.HeaderText = "Type";
            this.colViewType.MinimumWidth = 100;
            this.colViewType.Name = "colViewType";
            this.colViewType.ReadOnly = true;
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
            // colViewRefSheet
            // 
            this.colViewRefSheet.DataPropertyName = "ReferenceSheet";
            this.colViewRefSheet.HeaderText = "Ref Sheet";
            this.colViewRefSheet.MinimumWidth = 10;
            this.colViewRefSheet.Name = "colViewRefSheet";
            this.colViewRefSheet.ReadOnly = true;
            this.colViewRefSheet.Width = 80;
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
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            treeNode1.Name = "Node4";
            treeNode1.Text = "LEVEL 1 PLAN";
            treeNode2.Name = "Node8";
            treeNode2.Text = "PLAN LEGEND";
            treeNode3.Name = "Node1";
            treeNode3.Text = "A2.1";
            treeNode4.Name = "Node5";
            treeNode4.Text = "LEVEL 2 PLAN";
            treeNode5.Name = "Node2";
            treeNode5.Text = "A2.2";
            treeNode6.Name = "Node3";
            treeNode6.Text = "A2.3";
            treeNode7.Name = "Node6";
            treeNode7.Text = "A3.1";
            treeNode8.Name = "Node0";
            treeNode8.Text = "Sheets";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode8});
            this.treeView1.Size = new System.Drawing.Size(326, 464);
            this.treeView1.TabIndex = 3;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Location = new System.Drawing.Point(12, 37);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.gridViews);
            this.splitContainer1.Size = new System.Drawing.Size(978, 464);
            this.splitContainer1.SplitterDistance = 326;
            this.splitContainer1.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "Sheets:";
            // 
            // buttonClearSearch
            // 
            this.buttonClearSearch.BackgroundImage = global::ZGF.Revit.Properties.Resources.Clear_Search;
            this.buttonClearSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonClearSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonClearSearch.ForeColor = System.Drawing.SystemColors.Control;
            this.buttonClearSearch.Location = new System.Drawing.Point(279, 10);
            this.buttonClearSearch.Margin = new System.Windows.Forms.Padding(4);
            this.buttonClearSearch.Name = "buttonClearSearch";
            this.buttonClearSearch.Size = new System.Drawing.Size(20, 22);
            this.buttonClearSearch.TabIndex = 9;
            this.buttonClearSearch.TabStop = false;
            this.buttonClearSearch.Text = "...";
            this.buttonClearSearch.UseVisualStyleBackColor = true;
            // 
            // textBoxSearchTerms
            // 
            this.textBoxSearchTerms.BackColor = System.Drawing.Color.Yellow;
            this.textBoxSearchTerms.Location = new System.Drawing.Point(87, 10);
            this.textBoxSearchTerms.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxSearchTerms.Name = "textBoxSearchTerms";
            this.textBoxSearchTerms.Size = new System.Drawing.Size(187, 22);
            this.textBoxSearchTerms.TabIndex = 8;
            // 
            // button1
            // 
            this.button1.BackgroundImage = global::ZGF.Revit.Properties.Resources.Clear_Search;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.ForeColor = System.Drawing.SystemColors.Control;
            this.button1.Location = new System.Drawing.Point(607, 10);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(20, 22);
            this.button1.TabIndex = 11;
            this.button1.TabStop = false;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.Yellow;
            this.textBox1.Location = new System.Drawing.Point(415, 10);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(187, 22);
            this.textBox1.TabIndex = 10;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(346, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "Views:";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(344, 517);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(148, 35);
            this.button2.TabIndex = 12;
            this.button2.Text = "OK";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(511, 517);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(148, 35);
            this.button3.TabIndex = 13;
            this.button3.Text = "Cancel";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(18, 519);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(101, 32);
            this.button4.TabIndex = 14;
            this.button4.Text = "New Sheet";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // pictureBoxPreview
            // 
            this.pictureBoxPreview.Location = new System.Drawing.Point(821, 486);
            this.pictureBoxPreview.Name = "pictureBoxPreview";
            this.pictureBoxPreview.Size = new System.Drawing.Size(142, 82);
            this.pictureBoxPreview.TabIndex = 4;
            this.pictureBoxPreview.TabStop = false;
            // 
            // SheetViewManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1002, 580);
            this.Controls.Add(this.pictureBoxPreview);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.buttonClearSearch);
            this.Controls.Add(this.textBoxSearchTerms);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.splitContainer1);
            this.Name = "SheetViewManager";
            this.Text = "SheetViewManager";
            ((System.ComponentModel.ISupportInitialize)(this.gridViews)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPreview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView gridViews;
        private System.Windows.Forms.DataGridViewTextBoxColumn colViewName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colViewType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colViewScale;
        private System.Windows.Forms.DataGridViewTextBoxColumn colViewDiscipline;
        private System.Windows.Forms.DataGridViewTextBoxColumn colViewPhase;
        private System.Windows.Forms.DataGridViewTextBoxColumn colViewRefSheet;
        private System.Windows.Forms.DataGridViewTextBoxColumn colViewTitleOnSheet;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonClearSearch;
        private System.Windows.Forms.TextBox textBoxSearchTerms;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.PictureBox pictureBoxPreview;
    }
}