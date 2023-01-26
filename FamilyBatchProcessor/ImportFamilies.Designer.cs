namespace ZGF.Revit
{
		partial class FamilyImport
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
						this.groupBox1 = new System.Windows.Forms.GroupBox();
						this.chkOverwriteFamilyTypes = new System.Windows.Forms.CheckBox();
						this.chkRecursive = new System.Windows.Forms.CheckBox();
						this.chkOnlyFamiliesAlreadyLoaded = new System.Windows.Forms.CheckBox();
						this.progressBar1 = new System.Windows.Forms.ProgressBar();
						this.buttonChooseImportFolder = new System.Windows.Forms.Button();
						this.textBoxImportFolder = new System.Windows.Forms.TextBox();
						this.buttonOK = new System.Windows.Forms.Button();
						this.buttonCancel = new System.Windows.Forms.Button();
						this.groupBox1.SuspendLayout();
						this.SuspendLayout();
						// 
						// groupBox1
						// 
						this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
												| System.Windows.Forms.AnchorStyles.Left)
												| System.Windows.Forms.AnchorStyles.Right)));
						this.groupBox1.Controls.Add(this.chkOverwriteFamilyTypes);
						this.groupBox1.Controls.Add(this.chkRecursive);
						this.groupBox1.Controls.Add(this.chkOnlyFamiliesAlreadyLoaded);
						this.groupBox1.Controls.Add(this.progressBar1);
						this.groupBox1.Controls.Add(this.buttonChooseImportFolder);
						this.groupBox1.Controls.Add(this.textBoxImportFolder);
						this.groupBox1.Location = new System.Drawing.Point(12, 8);
						this.groupBox1.Name = "groupBox1";
						this.groupBox1.Size = new System.Drawing.Size(444, 122);
						this.groupBox1.TabIndex = 0;
						this.groupBox1.TabStop = false;
						this.groupBox1.Text = "Import from folder:";
						// 
						// chkOverwriteFamilyTypes
						// 
						this.chkOverwriteFamilyTypes.Anchor = System.Windows.Forms.AnchorStyles.Top;
						this.chkOverwriteFamilyTypes.AutoSize = true;
						this.chkOverwriteFamilyTypes.Location = new System.Drawing.Point(22, 71);
						this.chkOverwriteFamilyTypes.Name = "chkOverwriteFamilyTypes";
						this.chkOverwriteFamilyTypes.Size = new System.Drawing.Size(133, 17);
						this.chkOverwriteFamilyTypes.TabIndex = 3;
						this.chkOverwriteFamilyTypes.Text = "Update type properties";
						this.chkOverwriteFamilyTypes.UseVisualStyleBackColor = true;
						// 
						// chkRecursive
						// 
						this.chkRecursive.Anchor = System.Windows.Forms.AnchorStyles.Top;
						this.chkRecursive.AutoSize = true;
						this.chkRecursive.Location = new System.Drawing.Point(22, 49);
						this.chkRecursive.Name = "chkRecursive";
						this.chkRecursive.Size = new System.Drawing.Size(115, 17);
						this.chkRecursive.TabIndex = 2;
						this.chkRecursive.Text = "Include sub-folders";
						this.chkRecursive.UseVisualStyleBackColor = true;
						// 
						// chkOnlyFamiliesAlreadyLoaded
						// 
						this.chkOnlyFamiliesAlreadyLoaded.Anchor = System.Windows.Forms.AnchorStyles.Top;
						this.chkOnlyFamiliesAlreadyLoaded.AutoSize = true;
						this.chkOnlyFamiliesAlreadyLoaded.Location = new System.Drawing.Point(22, 93);
						this.chkOnlyFamiliesAlreadyLoaded.Name = "chkOnlyFamiliesAlreadyLoaded";
						this.chkOnlyFamiliesAlreadyLoaded.Size = new System.Drawing.Size(250, 17);
						this.chkOnlyFamiliesAlreadyLoaded.TabIndex = 4;
						this.chkOnlyFamiliesAlreadyLoaded.Text = "Refresh families (do not import any new families)";
						this.chkOnlyFamiliesAlreadyLoaded.UseVisualStyleBackColor = true;
						// 
						// progressBar1
						// 
						this.progressBar1.Anchor = System.Windows.Forms.AnchorStyles.Top;
						this.progressBar1.Location = new System.Drawing.Point(380, 86);
						this.progressBar1.Name = "progressBar1";
						this.progressBar1.Size = new System.Drawing.Size(48, 25);
						this.progressBar1.TabIndex = 6;
						this.progressBar1.Visible = false;
						// 
						// buttonChooseImportFolder
						// 
						this.buttonChooseImportFolder.Anchor = System.Windows.Forms.AnchorStyles.Top;
						this.buttonChooseImportFolder.Location = new System.Drawing.Point(269, 45);
						this.buttonChooseImportFolder.Name = "buttonChooseImportFolder";
						this.buttonChooseImportFolder.Size = new System.Drawing.Size(159, 23);
						this.buttonChooseImportFolder.TabIndex = 1;
						this.buttonChooseImportFolder.Text = "Choose import folder...";
						this.buttonChooseImportFolder.UseVisualStyleBackColor = true;
						this.buttonChooseImportFolder.Click += new System.EventHandler(this.buttonChooseImportFolder_Click);
						this.buttonChooseImportFolder.Leave += new System.EventHandler(this.textBoxImportFolder_Leave);
						// 
						// textBoxImportFolder
						// 
						this.textBoxImportFolder.Anchor = System.Windows.Forms.AnchorStyles.Top;
						this.textBoxImportFolder.Location = new System.Drawing.Point(17, 19);
						this.textBoxImportFolder.Name = "textBoxImportFolder";
						this.textBoxImportFolder.Size = new System.Drawing.Size(411, 20);
						this.textBoxImportFolder.TabIndex = 0;
						this.textBoxImportFolder.Leave += new System.EventHandler(this.textBoxImportFolder_Leave);
						// 
						// buttonOK
						// 
						this.buttonOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
						this.buttonOK.Enabled = false;
						this.buttonOK.Location = new System.Drawing.Point(150, 153);
						this.buttonOK.Name = "buttonOK";
						this.buttonOK.Size = new System.Drawing.Size(77, 29);
						this.buttonOK.TabIndex = 5;
						this.buttonOK.Text = "Import...";
						this.buttonOK.UseVisualStyleBackColor = true;
						this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
						// 
						// buttonCancel
						// 
						this.buttonCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
						this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
						this.buttonCancel.Location = new System.Drawing.Point(233, 153);
						this.buttonCancel.Name = "buttonCancel";
						this.buttonCancel.Size = new System.Drawing.Size(77, 29);
						this.buttonCancel.TabIndex = 6;
						this.buttonCancel.Text = "Cancel";
						this.buttonCancel.UseVisualStyleBackColor = true;
						this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
						// 
						// FamilyImport
						// 
						this.AcceptButton = this.buttonOK;
						this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
						this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
						this.CancelButton = this.buttonCancel;
						this.ClientSize = new System.Drawing.Size(468, 194);
						this.Controls.Add(this.buttonCancel);
						this.Controls.Add(this.buttonOK);
						this.Controls.Add(this.groupBox1);
						this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
						this.MaximizeBox = false;
						this.MinimizeBox = false;
						this.Name = "FamilyImport";
						this.ShowInTaskbar = false;
						this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
						this.Text = "ZGF Family Import Utility";
						this.groupBox1.ResumeLayout(false);
						this.groupBox1.PerformLayout();
						this.ResumeLayout(false);

				}

				#endregion

				private System.Windows.Forms.GroupBox groupBox1;
				private System.Windows.Forms.Button buttonChooseImportFolder;
				private System.Windows.Forms.TextBox textBoxImportFolder;
				private System.Windows.Forms.CheckBox chkOverwriteFamilyTypes;
				private System.Windows.Forms.Button buttonOK;
				private System.Windows.Forms.Button buttonCancel;
				private System.Windows.Forms.CheckBox chkOnlyFamiliesAlreadyLoaded;
				private System.Windows.Forms.CheckBox chkRecursive;
				private System.Windows.Forms.ProgressBar progressBar1;
		}
}