using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Windows.Forms;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ZGF.Revit
{
    public partial class FamilyExport : System.Windows.Forms.Form
    {
        // Member variables:
        private ExternalCommandData m_CommandData;
        private StringCollection m_CheckedCategories;
        private List<string> m_categories_Annotation = new List<string>();
        private List<string> m_categoriesNotToExport = new List<string>();
        private Document m_doc;
        //string m_ExportParentFolder;

        private bool m_OverwriteExistingFiles;

        // This var is used with DoEvents to during the processing state to allow the user to 
        // cancel exporting before all families have been processed:
        bool processingFamilies = false;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="CommandData"></param>
        public FamilyExport(Autodesk.Revit.UI.ExternalCommandData CommandData)
        {
            InitializeComponent();

#if DEBUG
            this.Text += " [_DEBUG_]";
#endif

            this.Text += " (v" + Assembly.GetExecutingAssembly().GetName().Version.Major + "." +
                Assembly.GetExecutingAssembly().GetName().Version.Minor +
                Assembly.GetExecutingAssembly().GetName().Version.Build + ")";

            m_CommandData = CommandData;
            m_doc = m_CommandData.Application.ActiveUIDocument.Document;

            // Checked Categories:
            m_CheckedCategories = ZGF.Revit.Properties.Settings.Default.CheckedCategories;
            if (null == m_CheckedCategories) m_CheckedCategories = new StringCollection();

            foreach (System.Windows.Forms.Control ctrl in groupBox1.Controls)
            {
                if (ctrl.GetType() == typeof(System.Windows.Forms.CheckBox))
                {
                    System.Windows.Forms.CheckBox cb = (CheckBox)ctrl;
                    if (m_CheckedCategories.Contains(cb.Text))
                    {
                        buttonOK.Enabled = cb.Checked = true;
                    }
                    cb.CheckedChanged += new System.EventHandler(this.chkCategories_CheckedChanged); // Add eventhandler
                }
            }

            // Other Settings:
            OverwriteExistingFiles = ZGF.Revit.Properties.Settings.Default.OverwriteExistingFiles;
            chkDeleteBackups.Checked = ZGF.Revit.Properties.Settings.Default.DeleteBackupFiles;

            // Get parent folder:

            // What if exportFolder is local to a central file? What if file is opened 'Detached from Central'?
            string exportFolderParent;
            //string exportFolderName;

            try
            {
                if (Directory.Exists(ZGF.Revit.Properties.Settings.Default.LastExportFolder))
                    exportFolderParent = ZGF.Revit.Properties.Settings.Default.LastExportFolder;
                else
                    exportFolderParent = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            }
            catch
            {
                exportFolderParent = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }

            textBoxExportFolder.Text = exportFolderParent;



            //	Populate private member lists of categories:						

            #region Anno_Categories
            m_categories_Annotation.Add("Area Tags");
            m_categories_Annotation.Add("Brace in Plan View Symbols");
            m_categories_Annotation.Add("Callout Heads");
            m_categories_Annotation.Add("Casework Tags");
            m_categories_Annotation.Add("Ceiling Tags");
            m_categories_Annotation.Add("Connection Symbols");
            m_categories_Annotation.Add("Curtain Panel Tags");
            m_categories_Annotation.Add("Detail Item Tags");
            m_categories_Annotation.Add("Door Tags");
            m_categories_Annotation.Add("Electrical Equipment Tags");
            m_categories_Annotation.Add("Electrical Fixture Tags");
            m_categories_Annotation.Add("Elevation Marks");
            m_categories_Annotation.Add("Floor Tags");
            m_categories_Annotation.Add("Furniture System Tags");
            m_categories_Annotation.Add("Furniture Tags");
            m_categories_Annotation.Add("Generic Annotations");
            m_categories_Annotation.Add("Generic Model Tags");
            m_categories_Annotation.Add("Grid Heads");
            m_categories_Annotation.Add("Keynote Tags");
            m_categories_Annotation.Add("Level Heads");
            m_categories_Annotation.Add("Lighting Fixture Tags");
            m_categories_Annotation.Add("Mass Floor Tags");
            m_categories_Annotation.Add("Mass Tags");
            m_categories_Annotation.Add("Material Tags");
            m_categories_Annotation.Add("Mechanical Equipment Tags");
            m_categories_Annotation.Add("Multi-Category Tags");
            m_categories_Annotation.Add("Parking Tags");
            m_categories_Annotation.Add("Planting Tags");
            m_categories_Annotation.Add("Plumbing Fixture Tags");
            m_categories_Annotation.Add("Property Line Segment Tags");
            m_categories_Annotation.Add("Property Tags");
            m_categories_Annotation.Add("Railing Tags");
            m_categories_Annotation.Add("Revision Cloud Tags");
            m_categories_Annotation.Add("Roof Tags");
            m_categories_Annotation.Add("Room Tags");
            m_categories_Annotation.Add("Section Marks");
            m_categories_Annotation.Add("Site Tags");
            m_categories_Annotation.Add("Specialty Equipment Tags");
            m_categories_Annotation.Add("Spot Elevation Symbols");
            m_categories_Annotation.Add("Stair Tags");
            m_categories_Annotation.Add("Structural Column Tags");
            m_categories_Annotation.Add("Structural Foundation Tags");
            m_categories_Annotation.Add("Structural Framing Tags");
            m_categories_Annotation.Add("Title Blocks");
            m_categories_Annotation.Add("View Reference");
            m_categories_Annotation.Add("View Titles");
            m_categories_Annotation.Add("Wall Tags");
            m_categories_Annotation.Add("Window Tags");
            #endregion


        }


        // Methods:

        public bool IsAnnotationCategory(string theCategory)
        {
            if (m_categories_Annotation.Contains(theCategory)) return true;

            return false;
        }


        private void buttonSelectAll_Click(object sender, EventArgs e)
        {
            foreach (System.Windows.Forms.Control ctrl in this.groupBox1.Controls)
            {
                if (ctrl.GetType() == typeof(System.Windows.Forms.CheckBox))
                {
                    CheckBox cb = (CheckBox)ctrl;
                    cb.Checked = true;
                }
            }
        }

        private void buttonSelectNone_Click(object sender, EventArgs e)
        {
            foreach (System.Windows.Forms.Control ctrl in this.groupBox1.Controls)
            {
                if (ctrl.GetType() == typeof(System.Windows.Forms.CheckBox))
                {
                    CheckBox cb = (CheckBox)ctrl;
                    cb.Checked = false;
                }
            }
        }


        /// <summary>
        /// Determines whether or not to overwrite existing files already exported, or leave them.
        /// </summary>
        public bool OverwriteExistingFiles
        {
            get { return m_OverwriteExistingFiles; }

            set
            {
                m_OverwriteExistingFiles = chkOverwriteExisting.Checked = value;
            }
        }


        private void chkOverwriteExisting_CheckedChanged(object sender, EventArgs e)
        {
            m_OverwriteExistingFiles = chkOverwriteExisting.Checked;
        }

        private void chkCategories_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            if (cb.Checked)
                m_CheckedCategories.Add(cb.Text);
            else
                m_CheckedCategories.Remove(cb.Text);

            buttonOK.Enabled = m_CheckedCategories.Count == 0 ? false : true;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            if (processingFamilies)
            {
                this.Visible = false;
                int remainingFamilyCount = progressBar1.Maximum - progressBar1.Value;
                DialogResult result = MessageBox.Show(
                        "Are you sure you want to cancel processing the remaining " + remainingFamilyCount.ToString() + " families?",
                        "Exporting Revit Families", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                    processingFamilies = false;
                else
                    this.Visible = true;

            }
            else
                this.Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            // if user presses cancel during processing, following var is set to false and processing is aborted:
            processingFamilies = true;

            bool doDelete = false;
            bool doOverwrite = m_OverwriteExistingFiles;

            List<string> inPlaceFamilyNames = new List<string>();  // Log these
            List<string> familiesNotEditable = new List<string>(); // Log these

            // SaveAs Options for when the family.SaveAs() command executes:
            SaveAsOptions saveAsOptions = new SaveAsOptions();
            saveAsOptions.OverwriteExistingFile = true;


            // TODO: Check to make sure ExportFolder is valid
            if (!Directory.Exists(textBoxExportFolder.Text))
            {
                try
                {
                    Directory.CreateDirectory(textBoxExportFolder.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Export folder is not valid. Please correct and try again\n\n" + ex.Message);
                    return;
                }
            }


            List<string> DoExportCategories = new List<string>();
            List<string> DoNotExportCategories = new List<string>();
            foreach (System.Windows.Forms.Control ctrl in this.groupBox1.Controls)
            {
                if (ctrl.GetType() == typeof(System.Windows.Forms.CheckBox))
                {
                    CheckBox cb = (CheckBox)ctrl;
                    string name = cb.Text;
                    if (cb.Checked)
                    {
                        DoExportCategories.Add(name);
                        if (name == "Annotation")
                            foreach (string s in m_categories_Annotation.ToArray())
                                DoExportCategories.Add(s);
                    }
                    else
                    {
                        DoNotExportCategories.Add(name);
                        if (name == "Annotation")
                            foreach (string s in m_categories_Annotation.ToArray())
                                DoNotExportCategories.Add(s);
                    }
                }
            }
            bool ExportEverythingElse = DoExportCategories.Contains("Everything else...");

            // Collect Families...
            Autodesk.Revit.ApplicationServices.Application app = m_CommandData.Application.Application;
            Document doc = m_CommandData.Application.ActiveUIDocument.Document;

            // Get a collection of all Families in the current project:
            List<Family> theFamilies = new List<Family>();
            FilteredElementCollector elemCollector = new FilteredElementCollector(doc);
            FilteredElementIterator iter = elemCollector.OfClass(typeof(Family)).GetElementIterator();
            iter.Reset();
            while (iter.MoveNext())
            {
                Family fam = (Family)iter.Current;
                string category = fam.FamilyCategory.Name;

                if (fam.IsInPlace)
                {
                    inPlaceFamilyNames.Add(category + " : " + fam.Name);
                    continue;
                }

                if (!fam.IsEditable)
                {
                    familiesNotEditable.Add(category + " : " + fam.Name);
                    continue;
                }

                if (ExportEverythingElse)
                {
                    if (!DoNotExportCategories.Contains(category))
                        theFamilies.Add(fam);
                }
                else
                {
                    if (DoExportCategories.Contains(category))
                        theFamilies.Add(fam);
                }
            }

            // If no families, then exit...
            if (theFamilies.Count == 0)
            {
                //message = "There are no families in this model to export.";
                TaskDialog.Show(this.Text, "There are no families to export");
                this.Close(); // Nothing to do. Bail...
            }


            // Clear out the folder...
            if (doDelete)
            {
                //DirectoryInfo di = new DirectoryInfo(textBoxExportFolder.Text);
                //FileInfo[] files = di.GetFiles("*.*", SearchOption.AllDirectories); // Method below is faster
                List<FileInfo> files = ZGF.Revit.FileSystemHelper.GetFilesRecursive(textBoxExportFolder.Text, "*.rfa");

                foreach (FileInfo f in files)
                    File.Delete(f.FullName);
            }

            List<string> itemsNotWritten = new List<string>();

            // Reconfigure dialog for Processing mode:
            foreach (System.Windows.Forms.Control ctrl in groupBox1.Controls)
            {
                if (ctrl.GetType() == typeof(System.Windows.Forms.CheckBox))
                    ctrl.Visible = false;
            }
            this.Height -= 277; // Collapse form
            buttonOK.Enabled = false;
            groupBox1.Text = string.Empty;
            labelExportFolder.Text = "Exporting " + theFamilies.Count.ToString() + " families. Press Cancel to abort...";
            textBoxExportFolder.Visible = false;
            progressBar1.Size = textBoxExportFolder.Size;
            progressBar1.Location = textBoxExportFolder.Location;
            progressBar1.Maximum = theFamilies.Count;
            progressBar1.Visible = true;

            // GET LIST OF library families to check exports against:
            IEnumerable<Revit_Content_EXTERNAL_Item> ListOfStandardFamilies = null;
            StringBuilder familiesSkipped = new StringBuilder();
            StringBuilder familiesThatRaisedAnException = new StringBuilder(); 

            if (checkBoxIgnoreLibraryFamilies.Checked)
            {
                try
                {
                    familiesSkipped = new StringBuilder("Standard families skipped (Category :: Name):\r\n\r\n");
                    familiesThatRaisedAnException = new StringBuilder("Families that failed to export:\r\n\r\n");

                    int revitVer = Convert.ToInt32(doc.Application.VersionNumber);
                    ListOfStandardFamilies = ZGF.Revit.RevitContent_External_Utilities.GetRevitExternalContentItems(revitVer);

                }
                catch
                {
                    ListOfStandardFamilies = null;
                    familiesSkipped = null;
                    familiesThatRaisedAnException = null;
                }
            }

            // PROCESS THE FAMILIES:

            foreach (Family f in theFamilies)
            {
                // if 'Skip ZGF Standard families' is selected, skip over:
                if (checkBoxIgnoreLibraryFamilies.Checked && null != ListOfStandardFamilies)
                {
                    try
                    {
                        int hasFamilyCategoryNameInLibrary = ListOfStandardFamilies
                            .Where(x => (x.CategoryName + x.FileName).Equals(f.FamilyCategory.Name + f.Name, StringComparison.CurrentCultureIgnoreCase))
                            .Count();

                        if (hasFamilyCategoryNameInLibrary > 0) // CategoryName+FamilyName is in list, then skip
                        {
                            familiesSkipped.Append("\r\n\t" + f.FamilyCategory.Name + " :: " + f.Name);
                            continue;
                        }
                    }
                    catch (System.Exception ex)
                    {
                        familiesThatRaisedAnException.Append("\r\n\t" + f.FamilyCategory.Name + " :: " + f.Name + "\r\n\t\tReason: " + ex.Message);
                        continue;
                    }
                }


                // This gives user a chance to abort the process:
                Application.DoEvents();
                if (!processingFamilies)
                    break;

                bool charsWereReplaced;
                string safeName = ReplaceInvalidFileNameChars(f.Name, "_", out charsWereReplaced);
                string category = f.FamilyCategory.Name;
                string categoryFolder;

                if (IsAnnotationCategory(category))
                    categoryFolder = textBoxExportFolder.Text + "\\Annotation"; //\\" + category;
                else
                    categoryFolder = textBoxExportFolder.Text + "\\" + category;

                if (!Directory.Exists(categoryFolder)) Directory.CreateDirectory(categoryFolder); // create sub-folder for each category...

                // TODO: Create a command that audits Family names for file system name compatibility...
                string fileNameToSave = Path.Combine(categoryFolder, safeName + ".rfa");
                string partAtomFilename = Path.Combine(categoryFolder, safeName + ".xml");

                if (charsWereReplaced)
                    itemsNotWritten.Add(f.Name + " -> " + safeName);

                //if (ZGF.Revit.ExportFamilies.IsValidPath(fName))
                //{
                // Time & log saves...	 
                if (doOverwrite || !File.Exists(fileNameToSave))
                {
                    // Log current family in case editing the file crashes Revit:
                    string crashFamilyName = Path.Combine(textBoxExportFolder.Text, safeName + "_CRASH.log");
                    using (StreamWriter crashFamilyLog = new StreamWriter(crashFamilyName))
                    {
                        crashFamilyLog.WriteLine("Category:\t" + category);
                        crashFamilyLog.WriteLine("Name    :\t" + safeName);
                        crashFamilyLog.Close();
                    }

                    // Export Family
                    Debug.WriteLine(fileNameToSave);
                    using (Transaction t = new Transaction(m_doc, "Export family"))


                        try
                        {

                            Debug.WriteLine("Opening: " + fileNameToSave);

                            Document famDoc = doc.EditFamily(f); //<--What if file is opened, but can't be saved/closed?

                            Debug.WriteLine("Saving: " + fileNameToSave);
                            famDoc.SaveAs(fileNameToSave, saveAsOptions); //<-Must be a legal file name!
                            if (checkBoxSavePartAtomXML.Checked) f.ExtractPartAtom(partAtomFilename);
                            Debug.WriteLine("Closing: " + fileNameToSave);
                            famDoc.Close(false);

                            // Successful export, so delete the Crash Family log file:
                            if (File.Exists(crashFamilyName)) File.Delete(crashFamilyName);

                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                            // Write to log file
                            familiesThatRaisedAnException.Append("\r\n\t" + f.FamilyCategory.Name + " :: " + f.Name + "\r\n\t\tReason: " + "CORRUPT");
                            // if something fails and doc couldn't be closed, then close it:												
                            foreach (Document d in m_CommandData.Application.Application.Documents)
                            {
                                Debug.WriteLine("Title: " + d.Title);
                                Debug.WriteLine("PathName: " + d.PathName);
                                if (d.Title.ToLower() == Path.GetFileName(fileNameToSave).ToLower())
                                    d.Close(false);
                            }
                        }



                }


                progressBar1.Value++;
            }

            // TODO: Write log file of itemsNotProcessed...
            // Path.
            char[] invalidChars = Path.GetInvalidFileNameChars();
            StringBuilder badCharString = new StringBuilder();
            foreach (char c in invalidChars)
            {
                badCharString.Append(c.ToString() + " ");
            }
            string disallowedChar = badCharString.ToString().TrimEnd();    //@"\ / : * ? "" < > |";
            string logFileName = Path.Combine(textBoxExportFolder.Text, "Exported Families.log");
            using (StreamWriter sr = new StreamWriter(logFileName))
            {
                sr.WriteLine("The following family names contained illegal file name characters. Those characters were replaced with \"_\" ...");
                sr.WriteLine();
                sr.WriteLine("Illegal characters: " + disallowedChar); //	\ / : * ? " < > |;
                sr.WriteLine();

                if (itemsNotWritten.Count > 0)
                {
                    foreach (string s in itemsNotWritten.ToArray())
                        sr.WriteLine(s);
                }
                else
                {
                    // No Errors
                    //if (itemsNotWritten.Count == 0)
                    sr.WriteLine("\n\n\tNo illegal file names found.");

                }

                // Other errors or info...
                sr.WriteLine("\n\n-----------------------------------------------------------");
                sr.WriteLine("\n\nThese families are not editable and cannot be exported:");
                foreach (string s in familiesNotEditable.ToArray())
                    sr.WriteLine("\t" + s);

                sr.WriteLine();
                sr.WriteLine("\nIn-Place Families cannot be exported:");

                foreach (string s in inPlaceFamilyNames.ToArray())
                    sr.WriteLine("\t" + s);
                sr.WriteLine("\n\n-----------------------------------------------------------\n");

                sr.Write(familiesSkipped.ToString());

                sr.WriteLine("\n\n-----------------------------------------------------------\n");

                sr.Write(familiesThatRaisedAnException.ToString());

                sr.WriteLine("\n\n-----------------------------------------------------------\n");

                // Delete Backup files:
                try
                {
                    if (chkDeleteBackups.Checked)
                    {
                        DeleteRevitBackupFiles(this.textBoxExportFolder.Text, true);
                    }
                }
                catch (Exception ex)
                {
                    sr.WriteLine("Error deleting backup files. Error: \n\t" + ex.Message);
                }

                sr.Close();
            }

            // Save Settings:
            ZGF.Revit.Properties.Settings.Default.LastExportFolder = textBoxExportFolder.Text; // m_ExportParentFolder;
            ZGF.Revit.Properties.Settings.Default.CheckedCategories = m_CheckedCategories;
            ZGF.Revit.Properties.Settings.Default.DeleteBackupFiles = chkDeleteBackups.Checked;
            ZGF.Revit.Properties.Settings.Default.OverwriteExistingFiles = chkOverwriteExisting.Checked;

            ZGF.Revit.Properties.Settings.Default.Save();

            // Open the Log file:
            this.Hide();
            DialogResult askLogViewLog = MessageBox.Show("View log file?", progressBar1.Value.ToString() + " Families exported", MessageBoxButtons.YesNo);
            if (askLogViewLog == DialogResult.Yes)
                Process.Start("notepad.exe", logFileName);

            this.Close();
        }


        /// <summary>
        /// Replaces characters in a filename string with valid substitute character
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="replacementChar"></param>
        /// <returns>The repaired file name string</returns>
        public static string ReplaceInvalidFileNameChars(string fileName, string replacementChar, out bool charsWereReplaced)
        {
            // get an array of illegal characters:
            string invalidChars = Regex.Escape(new string(Path.GetInvalidFileNameChars()));
            // build a regex to check the string
            string invalidRegex = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);
            // replace the chars
            string outstring = Regex.Replace(fileName, invalidRegex, replacementChar);
            // bool indicating whether chars need to be replaced
            charsWereReplaced = !fileName.Equals(outstring);
            // return repaired file name string:
            return outstring;
        }



        private void buttonChooseExportFolder_Click(object sender, EventArgs e)
        {
            string theFolder = textBoxExportFolder.Text;

            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            fbd.Description = "Choose a folder for the component library:";
            fbd.RootFolder = Environment.SpecialFolder.Desktop;
            fbd.ShowNewFolderButton = true;

            if (theFolder != null)
                fbd.SelectedPath = theFolder;
            else
                fbd.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);



            System.Windows.Forms.DialogResult theResult = fbd.ShowDialog();
            switch (theResult)
            {
                case System.Windows.Forms.DialogResult.Cancel:
                    // do nothing....return Autodesk.Revit.UI.Result.Cancelled;
                    break;
                case System.Windows.Forms.DialogResult.OK:
                    textBoxExportFolder.Text = fbd.SelectedPath;
                    break;
                default:
                    TaskDialog.Show("ZGF Revit Component Exporter", "Error setting folder");
                    break;
            }

        }

        private void textBoxExportFolder_Leave(object sender, EventArgs e)
        {
            // TODO: Need to validate this folder:

        }

        /// <summary>
        /// Delete Revit backup files in specified folder
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="recursive"></param>
        public static void DeleteRevitBackupFiles(string folder, bool recursive)
        {
            SearchOption searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

            string[] theBackUpFiles = Directory.GetFiles(folder, "*.????.rfa", searchOption);
            foreach (string f in theBackUpFiles)
                File.Delete(f);
        }

    }
}
