using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace ZGF.Revit
{
    public partial class FamilyImport : System.Windows.Forms.Form
		{
				// Member variables:
				private ExternalCommandData m_CommandData;

				// This var is used with DoEvents to during the processing state to allow the user to 
				// cancel exporting before all families have been processed:
				private bool processingFamilies = false;

				private DialogResult m_returnResult;
				/// <summary>
				/// Constructor
				/// </summary>
				/// <param name="CommandData"></param>
				public FamilyImport(Autodesk.Revit.UI.ExternalCommandData CommandData)
				{
					InitializeComponent();

#if DEBUG
					this.Text += " [_DEBUG_]";
#endif

                    this.Text += " (v" + Assembly.GetExecutingAssembly().GetName().Version.Major + "." +
                        Assembly.GetExecutingAssembly().GetName().Version.Minor +
                        Assembly.GetExecutingAssembly().GetName().Version.Build + ")";	
                    
                    m_CommandData = CommandData;
						
						// Other Settings:
						chkRecursive.Checked = ZGF.Revit.Properties.Settings.Default.ImportRecurseFolders;						
						chkOverwriteFamilyTypes.Checked = ZGF.Revit.Properties.Settings.Default.ImportUpdateTypeProperties;
						chkOnlyFamiliesAlreadyLoaded.Checked = ZGF.Revit.Properties.Settings.Default.ImportNewFamiliesOnly;

						// Get parent folder:
						string importFolderParent;

						try
						{
								if (Directory.Exists(ZGF.Revit.Properties.Settings.Default.LastImportFolder))
										importFolderParent = ZGF.Revit.Properties.Settings.Default.LastImportFolder;
								else
										importFolderParent = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
						}
						catch
						{
								importFolderParent = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
						}

						textBoxImportFolder.Text = importFolderParent;

						textBoxImportFolder_Leave(new object(),new EventArgs());
						
						if (buttonOK.Enabled) buttonOK.Focus();

				}


				//
				//	P R O P E R T I E S
				//
				public DialogResult ReturnValue { get { return m_returnResult;} }

				//
				//	E V E N T   H A N D L E R S
				//

				private void buttonCancel_Click(object sender, EventArgs e)
				{
						if (processingFamilies)
						{
								this.Visible = false;
								int remainingFamilyCount = progressBar1.Maximum - progressBar1.Value;
								DialogResult result = MessageBox.Show(
										"Are you sure you want to cancel importing the remaining " + remainingFamilyCount.ToString() + " families?",
										"Importing Revit Families", MessageBoxButtons.YesNo);

								if (result == DialogResult.Yes)
								{
										processingFamilies = false;
										m_returnResult = DialogResult.Abort;
								}
								else
										this.Visible = true;

						}
						else
						{
								m_returnResult = DialogResult.Cancel;
								this.Close();
						}
				}

				private void buttonOK_Click(object sender, EventArgs e)
				{
						// if user presses cancel during processing, following var is set to false and processing is aborted:
						processingFamilies = true;

								
						// TODO: Check to make sure ExportFolder is valid
						if (!Directory.Exists(textBoxImportFolder.Text))
						{
								try
								{
										Directory.CreateDirectory(textBoxImportFolder.Text);
								}
								catch (Exception ex)
								{
										MessageBox.Show("Import folder is not valid. Please correct and try again\n\n" + ex.Message);
										buttonOK.Enabled = false;
										textBoxImportFolder.Focus();
										//return;
								}
						}
						//-----------------------------------------------------------------------------

						Document doc = m_CommandData.Application.ActiveUIDocument.Document;
						Autodesk.Revit.ApplicationServices.Application app = m_CommandData.Application.Application;

						// Collect families in model:
						List<Family> theFamilies = new List<Family>();
						List<string> inPlaceFamilyNames = new List<string>();
						List<string> familiesNotEditable = new List<string>();
						FilteredElementCollector elemCollector = new FilteredElementCollector(doc);
						FilteredElementIterator iter = elemCollector.OfClass(typeof(Family)).GetElementIterator();
						iter.Reset();
						while (iter.MoveNext())
						{
								Family fam = (Family)iter.Current;
								string category = fam.FamilyCategory.Name;

								if (fam.IsInPlace)
								{
										inPlaceFamilyNames.Add(fam.Name.ToLower());
										continue;
								}

								if (!fam.IsEditable)
								{
										familiesNotEditable.Add(fam.Name.ToLower());
										continue;
								}

								theFamilies.Add(fam);

						}


						// Log file data:
						StringBuilder logInPlaceFamilies = new StringBuilder();
						StringBuilder logNotEditableFamilies = new StringBuilder();
						StringBuilder logSuccess = new StringBuilder();
						StringBuilder logSkipped = new StringBuilder();
						
						// Import:
						try
						{
								// Get the folder where the families reside:

								string importFolder = textBoxImportFolder.Text;
								//Path.GetDirectoryName(doc.PathName) + "\\" + Path.GetFileNameWithoutExtension(doc.PathName) + " Family Library";

								if (Directory.Exists(importFolder))
								{
										FamilyExport.DeleteRevitBackupFiles(importFolder, true);

										// Reconfigure dialog for Processing mode:
										chkOnlyFamiliesAlreadyLoaded.Enabled = chkOverwriteFamilyTypes.Enabled = chkRecursive.Enabled = buttonChooseImportFolder.Enabled = false;
										buttonOK.Enabled = false;

										textBoxImportFolder.Visible = false;
										progressBar1.Size = textBoxImportFolder.Size;
										progressBar1.Location = textBoxImportFolder.Location;
										
										progressBar1.Visible = true;

										// Set import options
										LoadFamilyOption lfo = new LoadFamilyOption();	//<--Create familyLoadOptions												
										bool doOverwrite = true;
										lfo.OnFamilyFound(true, out doOverwrite);

										// Collect list of all RFA files:
										//string[] rfaFiles = Directory.GetFiles(importFolder, "*.rfa", SearchOption.AllDirectories);
										//List<string> rfaFiles = new List<string>(Directory.GetFiles(importFolder, "*.rfa", SearchOption.AllDirectories));
                                        List<FileInfo> rfaFiles = ZGF.Revit.FileSystemHelper.GetFilesRecursive(importFolder, "*.rfa");
										// If no families, then exit...
										if (rfaFiles.Count == 0)
										{
											//message = "There are no families in this model to export.";
											TaskDialog.Show(this.Text, "There are no families to import");
											m_returnResult = DialogResult.Cancel;
											this.Close(); // Nothing to do. Bail...
										}

										// Log 
										logInPlaceFamilies.Append("The following families in the model are In-Place and cannot be overwritten with exportable families:\r\n\r\n");
										logNotEditableFamilies.Append("The following families in the model are not editable and cannot be overwritten with exportable families:\r\n\r\n");
										logSuccess.Append("Families successfully imported:\r\n\r\n");
										logSkipped.Append("Families skipped. They may not have changed:\r\n\r\n");
																				
										// If only refreshing content, then skip family files that are not loaded in the current project:
										if (chkOnlyFamiliesAlreadyLoaded.Checked)
										{
												// Names of families currently loaded into the model
												List<string> famNamesInModel = new List<string>();												
												foreach (Family tribalUnit in theFamilies)
												{
														famNamesInModel.Add(tribalUnit.Name.ToLower());
												}
												// Names of RFA files
												List<string> rfaFilesNameOnly = new List<string>();
												foreach (FileInfo f in rfaFiles)
												{
														rfaFilesNameOnly.Add(Path.GetFileNameWithoutExtension(f.Name.ToLower()));
												}
												// If the name of the family in the model DOES NOT have a corresponding RFA file, then remove that rfa file from "rfaFiles"
												// Collect rfa files to be removed:
												List<FileInfo> itemsToRemove = new List<FileInfo>();
												for (int i = 0; i < rfaFilesNameOnly.Count; i++)
												{	
														string currentName = rfaFilesNameOnly[i];
														if (!famNamesInModel.Contains(currentName))
														{
																itemsToRemove.Add(rfaFiles[i]);
														}
												}
												if (itemsToRemove.Count > 0)
												{
														foreach (FileInfo f in itemsToRemove)
														{
																rfaFiles.Remove(f);
														}
												}

										}

										if (rfaFiles.Count == 0)
										{
												TaskDialog.Show(this.Text, "There are no families to import");
												m_returnResult = DialogResult.Cancel;
												this.Close(); // Nothing to do. Bail...
										}

										progressBar1.Maximum = rfaFiles.Count;
										groupBox1.Text = string.Empty;
										groupBox1.Text = "Importing " + rfaFiles.Count.ToString() + " families. Press Cancel to abort...";

										// Iterate over RFA files and load into project.
										foreach (FileInfo f in rfaFiles)
										{
												// This gives user a chance to abort the process:
												Application.DoEvents();
												if (!processingFamilies)
														break;

												string familyName = Path.GetFileNameWithoutExtension(f.Name);

												// do no import if an in-place family of same name already exists in the model
												if (inPlaceFamilyNames.Contains(familyName.ToLower()))
												{
														logInPlaceFamilies.Append("\t" + f + "\r\n");
														progressBar1.Value++;
														continue;
												}
												// do no import if a non-editable family of same name already exists in the model
												if (familiesNotEditable.Contains(familyName.ToLower()))
												{
														logNotEditableFamilies.Append("\t" + f + "\r\n");
														progressBar1.Value++;
														continue;
												}

												Family fam;
												try
												{
                                                        bool success = doc.LoadFamily(f.FullName, lfo, out fam); //<-families are not reloaded if there are no changes.
                                                        if (success)
														{															
                                                            // Log family output success:
                                                            logSuccess.Append("\t" + familyName + " <- " + f + "\r\n");
                                                            // Update XML file, if exists:
                                                            string partAtomFilename = Path.Combine(
                                                                        Path.GetDirectoryName(f.FullName),
                                                                            Path.GetFileNameWithoutExtension(f.FullName) + ".xml");
                                                            try
                                                            {	
                                                                fam.ExtractPartAtom(partAtomFilename);
                                                            }
                                                            catch 
                                                            {
                                                                logSuccess.Append("\t\tCould not write XML part atom: " + partAtomFilename + "\r\n");
                                                            }
														}
														else
														{
															logSkipped.Append("\t" + familyName + " <- " + f + "\r\n");
														}
												}
												catch (Exception ex)
												{
														Debug.WriteLine(ex.Message);
												}

												progressBar1.Value++;

										}
								}
						}

						catch (Exception ex)
						{
								Debug.WriteLine(ex.Message);
								m_returnResult = DialogResult.Abort;
#if DEBUG
								TaskDialog.Show(this.Text, ex.Message);
#endif
						}




						//-----------------------------------------------------------------------------

						// TODO: Write log file of itemsNotProcessed...
						// Path.
						string logFileName = textBoxImportFolder.Text + "\\Imported Families.log";
						using (StreamWriter sr = new StreamWriter(logFileName))
						{
								sr.WriteLine("Import log: " + doc.PathName);
								sr.WriteLine(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());
								sr.WriteLine();
								sr.WriteLine("\n\n-----------------------------------------------------------");
								sr.WriteLine(logSuccess.ToString());
								sr.WriteLine();
								sr.WriteLine("\n\n-----------------------------------------------------------");
								sr.WriteLine(logSkipped.ToString());
								sr.WriteLine();
								sr.WriteLine("\n\n-----------------------------------------------------------");
								sr.WriteLine(logInPlaceFamilies.ToString());
								sr.WriteLine();
								sr.WriteLine("\n\n-----------------------------------------------------------");
								sr.WriteLine(logNotEditableFamilies.ToString());

								sr.Close();
						}
						
						// Save Settings:
						ZGF.Revit.Properties.Settings.Default.LastImportFolder = textBoxImportFolder.Text;

						ZGF.Revit.Properties.Settings.Default.ImportRecurseFolders = chkRecursive.Checked;
						ZGF.Revit.Properties.Settings.Default.ImportUpdateTypeProperties = chkOverwriteFamilyTypes.Checked;
						ZGF.Revit.Properties.Settings.Default.ImportNewFamiliesOnly = chkOnlyFamiliesAlreadyLoaded.Checked;

						ZGF.Revit.Properties.Settings.Default.Save();

						// Open the Log file:
						this.Hide();
						DialogResult askLogViewLog = MessageBox.Show("View log file?", progressBar1.Value.ToString() + " Families exported", MessageBoxButtons.YesNo);
						if (askLogViewLog == DialogResult.Yes)
								Process.Start("notepad.exe",logFileName);

						m_returnResult = DialogResult.OK;
						this.Close();
				}

				private void buttonChooseImportFolder_Click(object sender, EventArgs e)
				{
						string theFolder = textBoxImportFolder.Text;
						
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
										textBoxImportFolder.Text = fbd.SelectedPath;
										break;
								default:
										TaskDialog.Show("ZGF Revit Family Import Utility", "Error setting folder");
										break;
						}
						
				}

				private void textBoxImportFolder_Leave(object sender, EventArgs e)
				{
						buttonOK.Enabled = Directory.Exists(textBoxImportFolder.Text) ? true : false;					
								
				}

				/// <summary>
				/// Used to configure LoadFamily function
				/// </summary>
                public class LoadFamilyOption : IFamilyLoadOptions
                {
                    public bool OnFamilyFound(bool familyInUse, out bool overWriteParameterValues)
                    {
                        if (familyInUse == true)
                        {
                            Debug.WriteLine("Project contains instances of the selected family.");
                            overWriteParameterValues = true;
                            return overWriteParameterValues;
                        }
                        else
                        {
                            Debug.WriteLine("There are no instances of the family being loaded.");
                            overWriteParameterValues = false;
                            return overWriteParameterValues;
                        }

                    }
                    public bool OnSharedFamilyFound(Family family, bool familyInUse, out FamilySource source, out bool overWriteParamterValues)
                    {
                        
                        overWriteParamterValues = true;
                        source = FamilySource.Family;

                        return overWriteParamterValues;
                    }
                    
                }







            

		}
}
