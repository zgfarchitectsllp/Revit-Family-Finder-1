using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Data.Linq.Mapping;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.Reflection;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;

namespace ZGF.Revit
{

    public partial class FamilyBrowser : System.Windows.Forms.Form
    {
        // P R I V A T E   M E M B E R S

        Autodesk.Revit.ApplicationServices.Application m_app;
        Document m_doc;
        UIDocument m_uiDoc;

        public static string _currentModelDiskPath_STATIC = string.Empty;
        private bool m_currentModelPathHasChanged;

        private int m_revit_version_number;

        private ElementType m_currentRevitElementType = null;
        private FamilySymbol m_currentFamilySymbol = null;

        // Custom class used to extract a thumbnail image from a file.


        // items are indexed to Grid rows:
        List<FamilySymbol> m_familySymbols_internal = new List<FamilySymbol>();

        //List<RevitFileUtilities.RfaFileInfo> m_familySymbols_external = new List<RevitFileUtilities.RfaFileInfo>();
        //List<object> m_familyItems_All = new List<object>(); // internal and external

        // ALL available content:
        List<Revit_Content_INTERNAL_Type_Item> m_revit_content_ALL_INTERNAL = null; // new List<RevitContent_Internal_Type_Item>();
        static List<Revit_Content_EXTERNAL_Item> m_revit_content_ALL_EXTERNAL = null; // = new List<RevitExternalContentItem>();

        // All content with un-checked CATEGORIES filtered out:
        List<Revit_Content_INTERNAL_Type_Item> m_revit_content_CATEGORY_FILTERED_INTERNAL = null;
        List<Revit_Content_EXTERNAL_Item> m_revit_content_CATEGORY_FILTERED_EXTERNAL = null;

        // Final FILTERED content bound to the DataGrid:
        MySortableBindingList<Revit_Content_INTERNAL_Type_Item> m_revit_CONTENT_filtered_datasource_INTERNAL;
        MySortableBindingList<Revit_Content_EXTERNAL_Item> m_revit_CONTENT_filtered_datasource_EXTERNAL;

        // lists of Revit categories in-use for filtering content with the category tree checkboxes:
        //List<string> m_revit_CATEGORIES_external = null;
        //List<string> m_revit_CATEGORIES_internal = null;

        //public static SortedDictionary<string, CategoryTreeItem> m_dictionary_all_CATEGORIES = new SortedDictionary<string, CategoryTreeItem>();
        public static SortedDictionary<string, CategoryTreeItem> m_dictionary_INTERNAL_CATEGORIES = null;
        public static SortedDictionary<string, CategoryTreeItem> m_dictionary_EXTERNAL_CATEGORIES = null;

        bool _checked_Internal_CategoryListHasChanged = true;

        // TIMER - Used to pause updating the search field while the user types:ff
        private string comboboxSearchWords;
        private System.Timers.Timer timerSearchTyping;

        // Tooltip for controls:
        ToolTip m_tooltip = new ToolTip();

        // A nice linen colored background in the project dataGrid...   
        System.Drawing.Color m_color_Linen = System.Drawing.Color.White;  //System.Drawing.Color.FromArgb(254, 252, 240);
        //System.Drawing.Color m_topPanelColor =  //System.Drawing.Color.FromArgb(254, 252, 240);
        System.Drawing.Color m_leftPanelColor = System.Drawing.Color.White;  // System.Drawing.Color.FromArgb(232, 235, 240);

        DataGridViewCellStyle m_datagridRowColor_Odd_Internal = new DataGridViewCellStyle();
        DataGridViewCellStyle m_datagridRowColor_Odd_External = new DataGridViewCellStyle();
        DataGridViewCellStyle m_datagridRowColor_Even_Internal = new DataGridViewCellStyle();
        DataGridViewCellStyle m_datagridRowColor_Even_External = new DataGridViewCellStyle();

        DataGridViewCellStyle m_gridCategories_SELECTED;  // = new DataGridViewCellStyle();
        DataGridViewCellStyle m_gridCategories_NOT_SELECTED; // = new DataGridViewCellStyle();

        // Categories to exclude:
        List<string> m_FamilyCategoriesToExclude = new List<string>();

        // Previous settings               
        bool m_startWithLastSearch = ZGF.Revit.Properties.Settings.Default.browserStartWithLastSearch;
        string m_lastSearchString_INTERNAL = string.Empty;
        string m_lastSearchString_EXTERNAL = String.Empty;
        int m_maxHistoryItems = ZGF.Revit.Properties.Settings.Default.browserMaxSearchHistoryItems;

        // Controls display of edit icon on edit button in grid. If content is editable, it is TRUE. 
        // All EXTERNAL content is editable; only RFAs are editable internally.
        private bool m_displayEditButton = true;



        // C O N S T R U C T O R
        public FamilyBrowser(ExternalCommandData commandData)
        {
            InitializeComponent();
            ViewFinder.MakeGridViewDoublebuffered(gridFamilies);

            this.Text += " (v" + Assembly.GetExecutingAssembly().GetName().Version.Major + "." +
                Assembly.GetExecutingAssembly().GetName().Version.Minor +
                Assembly.GetExecutingAssembly().GetName().Version.Build + ")";

            //Debug.WriteLine("Major: " + Assembly.GetExecutingAssembly().GetName().Version.Major);
            //Debug.WriteLine("Minor: " + Assembly.GetExecutingAssembly().GetName().Version.Minor);
            //Debug.WriteLine("Build: " + Assembly.GetExecutingAssembly().GetName().Version.Build);
            //Debug.WriteLine("Major Rev: " + Assembly.GetExecutingAssembly().GetName().Version.MajorRevision);
            //Debug.WriteLine("Minor Rev: " + Assembly.GetExecutingAssembly().GetName().Version.MinorRevision);

            m_app = commandData.Application.Application;
            m_doc = commandData.Application.ActiveUIDocument.Document;
            m_uiDoc = commandData.Application.ActiveUIDocument;

            // Has CURRENT model changed:
            if (m_doc.PathName == _currentModelDiskPath_STATIC)
            {
                m_currentModelPathHasChanged = false;
            }
            else
            {
                m_currentModelPathHasChanged = true;
                _currentModelDiskPath_STATIC = m_doc.PathName;
            }

            // get the Revit VERSION:
            if (m_app.VersionNumber.Length >= 4)
                m_revit_version_number = Convert.ToInt16(m_app.VersionNumber);
            else
                throw new Exception("Revit version is not a number.");
            // ----------------------------------------------
            // F O R M
            // ----------------------------------------------

            this.Size = ZGF.Revit.Properties.Settings.Default.browserLastSize;
            System.Drawing.Rectangle revitScreenBounds = this.GetScreenSize();   //ZGF.Revit.Utilities.GetRevitScreenSize(commandData.Application);
            if (revitScreenBounds.Width < this.Size.Width || revitScreenBounds.Height < this.Size.Height)
                this.Size = this.MinimumSize;
            

            // ZGF Logo PictureBox backcolor
            pictureBoxZgfLogo.BackColor = System.Drawing.Color.FromArgb(254, 252, 240);

            labelSearchInstructions.Text = "← Type to search: \"FURN TABLE ROUND\"";

            dataGridCategories.AutoGenerateColumns = false;

            // ----------------------------------------------
            // G R I D
            // ----------------------------------------------
            gridFamilies.AutoGenerateColumns = false;

            // Row Styles   TODO: How about different back-colors for INTERNAL vs EXTERNAL?
            m_datagridRowColor_Odd_Internal.BackColor = m_datagridRowColor_Odd_External.BackColor = m_color_Linen;
            m_datagridRowColor_Odd_Internal.SelectionBackColor = m_datagridRowColor_Even_Internal.SelectionBackColor = System.Drawing.Color.FromArgb(75, 149, 229);


            this.gridFamilies.DefaultCellStyle = m_datagridRowColor_Even_Internal;
            this.gridFamilies.AlternatingRowsDefaultCellStyle = m_datagridRowColor_Odd_Internal;

            gridFamilies.BackgroundColor = m_datagridRowColor_Even_External.BackColor = m_datagridRowColor_Even_Internal.BackColor = System.Drawing.Color.FromArgb(231, 234, 239);

            //m_datagridRowColor_Odd_Internal.ForeColor = m_datagridRowColor_Even_Internal.ForeColor = System.Drawing.Color.Black;
            m_datagridRowColor_Odd_External.ForeColor = m_datagridRowColor_Even_External.ForeColor = System.Drawing.Color.DarkGray;


            //  CATEGORY LIST CONTROL
            //      Selected = Bold, black text
            //      Deselected = Regular, gray text
            m_gridCategories_SELECTED = new DataGridViewCellStyle(dataGridCategories.DefaultCellStyle);
            m_gridCategories_NOT_SELECTED = new DataGridViewCellStyle(dataGridCategories.DefaultCellStyle);

            //m_gridCategories_SELECTED.Font = new System.Drawing.Font(m_gridCategories_SELECTED.Font, FontStyle.Bold);
            m_gridCategories_SELECTED.ForeColor = System.Drawing.Color.Black;
            m_gridCategories_SELECTED.Alignment = DataGridViewContentAlignment.NotSet;

            //m_gridCategories_NOT_SELECTED.Font = new System.Drawing.Font(m_gridCategories_NOT_SELECTED.Font, FontStyle.Regular);
            m_gridCategories_NOT_SELECTED.ForeColor = System.Drawing.Color.DarkGray;
            m_gridCategories_NOT_SELECTED.Alignment = DataGridViewContentAlignment.NotSet;

            // P R E V I O U S   S E T T I N G S  
            if (null != ZGF.Revit.Properties.Settings.Default.browserLocalSearchHistory)
            {
                foreach (string s in ZGF.Revit.Properties.Settings.Default.browserLocalSearchHistory)
                {
                    comboBoxQuickFind.Items.Add(s);

                    if (comboBoxQuickFind.Items.Count == m_maxHistoryItems) break;
                }
            }

            // Previous search term:
            m_lastSearchString_INTERNAL = ZGF.Revit.Properties.Settings.Default.browserLocalLastSearch;
            if (null == m_lastSearchString_INTERNAL) m_lastSearchString_INTERNAL = string.Empty;
            //m_lastSearchString_EXTERNAL = ZGF.Revit.Properties.Settings.Default.browserExternalLastSearch;

            // Set SEARCH TERM in ComboBox:
            m_startWithLastSearch = buttonClearSearch.Visible = ZGF.Revit.Properties.Settings.Default.browserStartWithLastSearch;



            if (m_startWithLastSearch)
                comboboxSearchWords = comboBoxQuickFind.Text = m_lastSearchString_INTERNAL;
            else
                comboboxSearchWords = comboBoxQuickFind.Text = string.Empty;

            comboBoxQuickFind.TextChanged += comboBoxQuickFind_TextChanged;

            // TIMER - Initialize the timer whose event updates the families grid while the user types search term:
            //          Note: this must happen after comboBoxQuickFind.Text is initialized.
            timerSearchTyping = new System.Timers.Timer();
            timerSearchTyping.Elapsed += timerSearchTyping_Elapsed;
            timerSearchTyping.AutoReset = true;
            timerSearchTyping.Interval = 1000; //<-- #TODO: Base this number on the number of items in ALL FAMILIES Lists?

            // LOAD THE GRID
            //ZGF.Revit.Properties.Settings.Default.Save();
            bool _tmp = ZGF.Revit.Properties.Settings.Default.browserStartWithLocalContent;
            if (ZGF.Revit.Properties.Settings.Default.browserStartWithLocalContent)
                radioButtonRevitModel.Checked = true;
            else
                radioButtonLibrary.Checked = true;



            // Make sure the grid updates.....
            if (radioButtonRevitModel.Checked)
            {
                if ((null == m_revit_content_ALL_INTERNAL) || (null == m_revit_CONTENT_filtered_datasource_INTERNAL))
                    radioButtonRevitContentLibrarySwitch_CheckedChanged(new object(), new EventArgs());
            }
            else
            {
                if ((null == m_revit_content_ALL_EXTERNAL) || (null == m_revit_CONTENT_filtered_datasource_EXTERNAL))
                    radioButtonRevitContentLibrarySwitch_CheckedChanged(new object(), new EventArgs());
            }



            // Update count: 
            labelListCount.Text = UpdateFamilyCount();

            labelTypeDescrip.Text = string.Empty;

            //this.ActiveControl = gridFamilies;
            this.ActiveControl = comboBoxQuickFind;
            comboBoxQuickFind.Focus();
        }



        /// <summary>
        /// Timer intended to control the frequency with which the grid of families is updated while user is typing. For large models, this allows
        /// the user to type in characters quickly without the grid updating upon each keystroke.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerSearchTyping_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {

            timerSearchTyping.Stop();
            timerSearchTyping.Enabled = false;

            // I don't understand how this works, but if I don't "invoke" the method, I get a "Cross-thread operation not valid" error:
            if (gridFamilies.InvokeRequired)
                gridFamilies.Invoke(new MethodInvoker(delegate { Update_GridFamiliesPerSearchWords(comboboxSearchWords); }));

        }
        //
        // A TYPING TIMER here so list doesn't immediately update with each keystroke, but waits specified milliseconds before doing so.
        //



        private void comboBoxQuickFind_TextUpdate(object sender, EventArgs e)
        {
            // This event does not fire when the user selects an item from the ComboBox. 
            // It only fires when the user types. Therefore, if we enable the timer, then control of updating 
            // the grid is handed over to timerSearchTyping_Elapsed event

            //timerSearchTyping.Interval = radioButtonLibrary.Checked ? 1000 : 100;

            if (radioButtonRevitModel.Checked)
            {
                int internalCount = m_revit_content_ALL_INTERNAL.Count;
                if (internalCount < 500)
                    timerSearchTyping.Interval = 100;
                else if (internalCount < 1000)
                    timerSearchTyping.Interval = 200;
                else if (internalCount < 3500)
                    timerSearchTyping.Interval = 250;
                else if (internalCount < 5000)
                    timerSearchTyping.Interval = 500;
                else if (internalCount < 7500)
                    timerSearchTyping.Interval = 750;
                else
                    timerSearchTyping.Interval = 1000;

            }
            else
            {
                timerSearchTyping.Interval = 1000;
            }

            timerSearchTyping.Enabled = true;
            timerSearchTyping.Stop();
            timerSearchTyping.Start();

        }

        /// <summary>
        /// Main SEARCH feature. The text box for searching content.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxQuickFind_TextChanged(object sender, EventArgs e)
        {
            if (comboboxSearchWords.Equals(comboBoxQuickFind.Text.Trim(), StringComparison.CurrentCultureIgnoreCase))
            {
                comboboxSearchWords = comboBoxQuickFind.Text;
                return;
            }

            comboboxSearchWords = comboBoxQuickFind.Text;

            // If the timer is enabled the the user is typing. 
            // If NOT, then the user has selected a previous, saved search term from the ComboBox items.
            // Also, this event fires before the Form Constructor(), so check if timerSearchTyping is null!!
            if (null != timerSearchTyping && !timerSearchTyping.Enabled)
                Update_GridFamiliesPerSearchWords(comboboxSearchWords);
        }


        private void Update_GridFamiliesPerSearchWords(string words)
        {
            //string words = this.comboBoxQuickFind.Text;

            gridFamilies.DataSource = pictureBoxPreview.Image = null;

            if (radioButtonRevitModel.Checked)
            {
                if (null == m_revit_content_CATEGORY_FILTERED_INTERNAL || _checked_Internal_CategoryListHasChanged)
                {

                    UpdateFilteredContent();
                }
                m_revit_CONTENT_filtered_datasource_INTERNAL = new MySortableBindingList<Revit_Content_INTERNAL_Type_Item>(m_revit_content_CATEGORY_FILTERED_INTERNAL.FindAll(item => item.IsMatch(words)));

                gridFamilies.DataSource = m_revit_CONTENT_filtered_datasource_INTERNAL;
            }
            else
            {
                if (null == m_revit_content_CATEGORY_FILTERED_EXTERNAL || _checked_Internal_CategoryListHasChanged)
                {

                    UpdateFilteredContent();
                }
                m_revit_CONTENT_filtered_datasource_EXTERNAL = new MySortableBindingList<Revit_Content_EXTERNAL_Item>(m_revit_content_CATEGORY_FILTERED_EXTERNAL.FindAll(item => item.IsMatch(words)));

                gridFamilies.DataSource = m_revit_CONTENT_filtered_datasource_EXTERNAL;
            }

            labelListCount.Text = UpdateFamilyCount();

            buttonClearSearch.Visible = comboBoxQuickFind.Text.Length > 0;

            buttonOK.Enabled = radioButtonRevitModel.Checked
                ? ((null != m_revit_CONTENT_filtered_datasource_INTERNAL) && (m_revit_CONTENT_filtered_datasource_INTERNAL.Count > 0))
                : ((null != m_revit_CONTENT_filtered_datasource_EXTERNAL) && (m_revit_CONTENT_filtered_datasource_EXTERNAL.Count > 0));

        }


        private void comboBoxQuickFind_MouseEnter(object sender, EventArgs e)
        {
            comboBoxQuickFind.SelectionLength = 0;
            m_tooltip.UseFading = true;
            m_tooltip.Show("Type to search...", this.comboBoxQuickFind, 1500);
        }

        private void comboBoxQuickFind_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void comboBoxQuickFind_KeyUp(object sender, KeyEventArgs e)
        {
            //if ((e.KeyData.Equals(Keys.Tab)) && (gridFamilies.Rows.Count > 0))
            //{
            buttonClearSearch.Visible = comboBoxQuickFind.Text.Trim().Length > 0;
            //}
        }

        private void buttonClearSearch_Click(object sender, EventArgs e)
        {

            buttonClearSearch.Visible = false;

            if (comboBoxQuickFind.Text != string.Empty)
            {
                string current = comboBoxQuickFind.Text.Trim();

                // Now, the rest of the list:
                if (0 < comboBoxQuickFind.Items.Count)
                {
                    if (comboBoxQuickFind.Items.Contains(current))
                        comboBoxQuickFind.Items.Remove(current);

                    comboBoxQuickFind.Items.Insert(0, current);

                    while (comboBoxQuickFind.Items.Count > (m_maxHistoryItems - 1))
                        comboBoxQuickFind.Items.RemoveAt(m_maxHistoryItems - 1);
                }
                else
                    comboBoxQuickFind.Items.Add(current);

                // clear list
                if (m_startWithLastSearch) ZGF.Revit.Properties.Settings.Default.browserLocalLastSearch = comboBoxQuickFind.Text;
                comboBoxQuickFind.Text = comboboxSearchWords = string.Empty;

                // Reset Datasource
                gridFamilies.DataSource = null;
                if (radioButtonRevitModel.Checked)
                {
                    m_revit_CONTENT_filtered_datasource_INTERNAL.Clear();
                    m_revit_CONTENT_filtered_datasource_INTERNAL = new MySortableBindingList<Revit_Content_INTERNAL_Type_Item>(m_revit_content_CATEGORY_FILTERED_INTERNAL);
                    gridFamilies.DataSource = m_revit_CONTENT_filtered_datasource_INTERNAL;
                }
                else
                {
                    m_revit_CONTENT_filtered_datasource_EXTERNAL.Clear();
                    m_revit_CONTENT_filtered_datasource_EXTERNAL = new MySortableBindingList<Revit_Content_EXTERNAL_Item>(m_revit_content_CATEGORY_FILTERED_EXTERNAL);
                    gridFamilies.DataSource = m_revit_CONTENT_filtered_datasource_EXTERNAL;
                }

                if (comboBoxQuickFind.Font.Italic) comboBoxQuickFind.Font = new Font(comboBoxQuickFind.Font, FontStyle.Regular);

                labelListCount.Text = UpdateFamilyCount();

                SaveSettings();
            }

        }


        private void gridFamilies_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            // User double-clicks header or some other non-row area of the control
            if (e.RowIndex < 0) return; // User double-clicked the grid heading...

            this.Hide();

            if (radioButtonRevitModel.Checked)
                m_currentRevitElementType = m_revit_CONTENT_filtered_datasource_INTERNAL[e.RowIndex].TheElementType;
            else
            {
                Revit_Content_EXTERNAL_Item current = m_revit_CONTENT_filtered_datasource_EXTERNAL[e.RowIndex];

                switch (current.FileType)
                {
                    case "Loadable Family":
                        {
                            m_currentRevitElementType = exContent_LoadableFamily.InsertLoadableFamily(m_uiDoc, m_doc, current);
                            break;
                        }
                    case "Drafting View":
                        {
                            RevitContent_External_Utilities.InsertDraftingView(m_uiDoc, current);
                            break;
                        }
                    case "System Family":
                        {
                            m_currentRevitElementType = exContent_SystemTypes.InsertSystemType(m_doc, current);
                            break;
                        }
                    case "Schedule":
                        {
                            RevitContent_External_Utilities.InsertSchedule(m_uiDoc, current);
                            break;
                        }
                    case "Sheet":
                        {
                            RevitContent_External_Utilities.InsertSheet(m_uiDoc, current);
                            break;
                        }
                    case "Material":
                        {

                            break;
                        }
                        // Other stuff.....Settings (Think 'Transfer Project Standards', Materials, )
                }
            }


            // Save List of Searches            
            SaveSettings();

            if (FamilyFinder.isModeless)
            {

                try
                {
                    // this.Hide();
                    if (null != this.CurrentFamilyElementType)
                        m_uiDoc.PostRequestForElementTypePlacement(this.CurrentFamilyElementType);

                    this.Show(FamilyFinder.hwndRevit);
                }
                catch (Exception ex)
                {
                    TaskDialog td = new TaskDialog("ZGF Family Finder");
                    td.MainContent = ex.Message;
                    td.Show();
                }


            }

        }



        private void gridFamilies_Click(object sender, EventArgs e)
        {
            this.ActiveControl = gridFamilies;
        }



        private void gridFamilies_Enter(object sender, EventArgs e)
        {

        }

        private void gridFamilies_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Enter))
                gridFamilies_CellDoubleClick(new object(), new DataGridViewCellEventArgs(0, gridFamilies.CurrentRow.Index));

        }
        private void gridFamilies_Sorted(object sender, EventArgs e)
        {
            int rowCount = 0;
            foreach (DataGridViewRow dr in gridFamilies.Rows)
            {
                if (dr.Visible) gridFamilies.InvalidateRow(rowCount);
                rowCount++;
            }
        }


        public ElementType CurrentFamilyElementType
        {
            get { return m_currentRevitElementType; }
        }

        public FamilySymbol CurrentFamily
        {
            get { return m_currentFamilySymbol; }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            //m_revit_content_filtered_datasource[e.RowIndex]
            gridFamilies_CellDoubleClick(new object(), new DataGridViewCellEventArgs(0, gridFamilies.CurrentRow.Index));

        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            SaveSettings();

            this.Close();
        }




        /// <summary>
        /// Saves form settings. Triggered on close.
        /// </summary>
        private void SaveSettings()
        {
            StringCollection sc = new StringCollection();

            string lastSearchString = comboBoxQuickFind.Text.Trim().ToLower();

            int counter = 0;

            if (comboBoxQuickFind.Items.Count > 0)
            {
                foreach (string s in comboBoxQuickFind.Items)
                {
                    if (counter > m_maxHistoryItems) break;
                    sc.Add(s.ToLower());
                    counter++;
                }
                if ((lastSearchString != string.Empty) && (sc.Contains(lastSearchString)))
                    sc.Remove(lastSearchString);
            }

            if (lastSearchString != string.Empty) sc.Insert(0, lastSearchString);

            // Save settings...            

            ZGF.Revit.Properties.Settings.Default.browserLocalSearchHistory = sc;
            ZGF.Revit.Properties.Settings.Default.browserLocalLastSearch = lastSearchString;

            ZGF.Revit.Properties.Settings.Default.browserLastSize = this.Size;

            // Prefer not reloading with library selected.
            ZGF.Revit.Properties.Settings.Default.browserStartWithLocalContent = true; // radioButtonRevitModel.Checked;
            // TODO: Save browser location...?
            ZGF.Revit.Properties.Settings.Default.Save();
        }




        /// <summary>
        /// Displays number of families filtered from total in upper panel: 23 of 543
        /// </summary>
        /// <returns></returns>
        private string UpdateFamilyCount()
        {
            if (radioButtonRevitModel.Checked)
                return m_revit_CONTENT_filtered_datasource_INTERNAL.Count.ToString() + " of " + m_revit_content_ALL_INTERNAL.Count.ToString();
            else
                return m_revit_CONTENT_filtered_datasource_EXTERNAL.Count.ToString() + " of " + m_revit_content_ALL_EXTERNAL.Count.ToString();
        }

        private void SetFamilyPreviewImage(int RowNumber)
        {
            try
            {
                Bitmap bmp = null;
                if (radioButtonRevitModel.Checked)
                {
                    if (m_revit_CONTENT_filtered_datasource_INTERNAL.Count > 0)
                        bmp = m_revit_CONTENT_filtered_datasource_INTERNAL[RowNumber]
                                        .TheElementType.GetPreviewImage(new Size(pictureBoxPreview.Height, pictureBoxPreview.Height));
                }
                else
                {
                    if (m_revit_CONTENT_filtered_datasource_EXTERNAL.Count > 0)
                    {
                        FileInfo pth = new FileInfo(m_revit_CONTENT_filtered_datasource_EXTERNAL[RowNumber].FullPathName);

                        if (pth.Exists)
                        {
                            // TODO: What if there is an image file?
                            try
                            {
                                using (RMA.Shell.ShellThumbnail m_thumbnailImg = new RMA.Shell.ShellThumbnail())
                                {
                                    bmp = new Bitmap(m_thumbnailImg.GetThumbnail(pth.FullName));
                                }
                            }
                            catch { bmp = null; }
                        }
                        else
                            pth = null;
                    }
                }

                pictureBoxPreview.Image = (null == bmp)
                    ? ZGF.Revit.Properties.Resources.No_preview_150x150
                    : bmp;

            }
            catch
            {
                // TODO: Handle this....
                pictureBoxPreview.Image = ZGF.Revit.Properties.Resources.No_preview_150x150;
            }
        }

#if DEBUG
        DateTime startTime;
        int _debugTime;
#endif

        private void radioButtonRevitContentLibrarySwitch_CheckedChanged(object sender, EventArgs e)
        {
            // This eventHandler fires once for each RadioButton instance. 
            // Following code cause even to fire only once for the "true" value:
            RadioButton rb = sender as RadioButton;
            if (null != rb && !rb.Checked)
                return;

            // Temporarily un-subscribe from after check even so that it doesn't fire for each node change:
            dataGridCategories.CellClick -= dataGridCategories_CellClick;



            if (radioButtonRevitModel.Checked)
            {

#if DEBUG
                startTime = DateTime.Now;
#endif
                Initialize_INTERNAL_content();

#if DEBUG
                _debugTime = (DateTime.Now - startTime).Milliseconds;
                Debug.WriteLine("Populate Grid INTERNAL: " + _debugTime + " ms");
#endif

                SyncCategoryNameLists(false);

                Update_GridFamiliesPerSearchWords(comboboxSearchWords);

                buttonOK.Text = "Place";
                buttonOK.Enabled = ((null != m_revit_CONTENT_filtered_datasource_INTERNAL) && (m_revit_CONTENT_filtered_datasource_INTERNAL.Count > 0));


                SaveSettings();

            }
            else
            {
#if DEBUG
                startTime = DateTime.Now;
#endif

                Initialize_EXTERNAL_content();

#if DEBUG
                _debugTime = (DateTime.Now - startTime).Milliseconds;
                Debug.WriteLine("Populate Grid EXTERNAL: " + _debugTime + " ms");
#endif

                SyncCategoryNameLists(true);

                Update_GridFamiliesPerSearchWords(comboboxSearchWords);

                buttonOK.Text = "Load";
                buttonOK.Enabled = ((null != m_revit_CONTENT_filtered_datasource_EXTERNAL) && (m_revit_CONTENT_filtered_datasource_EXTERNAL.Count > 0));

                SaveSettings();

            }

            comboBoxQuickFind.Select();

            labelTypeDescrip.Visible = !radioButtonLibrary.Checked;

            dataGridCategories.CellClick += dataGridCategories_CellClick;
            //if (radioButtonRevitModel.Checked) dataGridView1_CellClick(new object(), new DataGridViewCellEventArgs(0,0));

        }



        /// <summary>
        /// Syncs the Category lists' IsChecked value to make sure the state of the values are preserved when toggling between Internal and External content.
        /// </summary>
        /// <param name="FromModelToLibrary"></param>
        private void SyncCategoryNameLists(bool FromModelToLibrary)
        {

            if (null != m_dictionary_EXTERNAL_CATEGORIES && null != m_dictionary_EXTERNAL_CATEGORIES)
            {
                if (FromModelToLibrary)
                {
                    // Iterate over INTERNAL categories, making sure external items' IsChecked value matches:
                    foreach (CategoryTreeItem cti in m_dictionary_INTERNAL_CATEGORIES.Values)
                    {
                        if (m_dictionary_EXTERNAL_CATEGORIES.ContainsKey(cti.Name) && cti.IsChecked != m_dictionary_EXTERNAL_CATEGORIES[cti.Name].IsChecked)
                        {
                            m_dictionary_EXTERNAL_CATEGORIES[cti.Name].IsChecked = cti.IsChecked;
                        }
                    }
                }
                else
                {
                    // Iterate over EXTERNAL categories, making sure internal items' IsChecked value matches:
                    foreach (CategoryTreeItem cti in m_dictionary_EXTERNAL_CATEGORIES.Values)
                    {
                        if (m_dictionary_INTERNAL_CATEGORIES.ContainsKey(cti.Name) && cti.IsChecked != m_dictionary_INTERNAL_CATEGORIES[cti.Name].IsChecked)
                        {
                            m_dictionary_INTERNAL_CATEGORIES[cti.Name].IsChecked = cti.IsChecked;
                        }
                    }
                }
                dataGridCategories.Refresh();
            }
        }


        // INITIALIZE CONTENT



        //private List<string> _checkedCategoryNames = new List<string>();

        private void Initialize_EXTERNAL_content()
        {
            m_app.WriteJournalComment("ADDIN: " + this.Text + " - Initializing ZGF Revit Family Database", true);

            // Read the DB and get the entire content library
            // Collect a list of categories and save for the TreeView
            pictureBoxPreview.Image = null;



            if (null == m_dictionary_EXTERNAL_CATEGORIES || null == m_revit_content_ALL_EXTERNAL)
            {
                // Disconnect DataSource
                dataGridCategories.DataSource = null;

#if DEBUG
                DateTime dt_externalContent = DateTime.Now;
#endif

                m_revit_content_ALL_EXTERNAL = RevitContent_External_Utilities.GetRevitExternalContentItems(m_revit_version_number);

#if DEBUG
                int dt_ms = (DateTime.Now - dt_externalContent).Milliseconds;
                Debug.WriteLine("Initial fetch, external content: " + dt_ms + " ms");
#endif

                m_dictionary_EXTERNAL_CATEGORIES = new SortedDictionary<string, CategoryTreeItem>();

                //if (null == m_revit_content_ALL_EXTERNAL)
                //{

                //    // Get all of the content:
                //    m_revit_content_ALL_EXTERNAL = RevitContent_External_Utilities.GetRevitExternalContentItems(m_revit_version_number);


                //}

                // Get a list of external Category names:
                IEnumerable<IGrouping<string, Revit_Content_EXTERNAL_Item>> groupedCategories =
                   m_revit_content_ALL_EXTERNAL.GroupBy(c => c.CategoryName);

                foreach (IGrouping<string, Revit_Content_EXTERNAL_Item> grouping in groupedCategories)
                {
                    CategoryTreeItem cti = new CategoryTreeItem(grouping.Key);
                    cti.StorageType = cti.StorageType | CategoryTreeItem.CategoryItemIntOrExt.External;
                    //m_dictionary_all_CATEGORIES.Add(grouping.Key, cti);

                    // Since this is the first collection of content, uncheck all categories:
                    cti.IsChecked = true;
                    m_dictionary_EXTERNAL_CATEGORIES.Add(cti.Name, cti);
                }
                // Since this is the first collection of content, sync the categories
                SyncCategoryNameLists(FromModelToLibrary: true);
            }
            //else
            //{
            dataGridCategories.DataSource = m_dictionary_EXTERNAL_CATEGORIES.Values.ToList();
            //}

            //this.ResumeLayout(false);

            // Sort the primary list
            //m_revit_content_EXTERNAL.Sort();

            // Change the data bindings and column heading text
            colFamily.DataPropertyName = "FileName"; // 1
            colFamily.HeaderText = "Name";
            colType.DataPropertyName = "Description"; // 2
            colType.HeaderText = "Description";
            //colType.Visible = false;
            colCategory.DataPropertyName = "CategoryName"; // 3
            colCategory.HeaderText = "Category";
            colFileType.DataPropertyName = "FileType"; // 4
            colFileType.HeaderText = "File type";
            colFileType.Visible = true;

            // Load the previous search
            if (m_lastSearchString_INTERNAL == string.Empty)
            {
                m_revit_CONTENT_filtered_datasource_EXTERNAL = new MySortableBindingList<Revit_Content_EXTERNAL_Item>(m_revit_content_ALL_EXTERNAL);
                gridFamilies.DataSource = m_revit_CONTENT_filtered_datasource_EXTERNAL;
                gridFamilies.Sort((DataGridViewColumn)colFamily, ListSortDirection.Ascending);
                buttonClearSearch.Visible = false;
            }
            else
                Update_GridFamiliesPerSearchWords(comboboxSearchWords);

            //m_app.WriteJournalComment("ADDIN: " + this.Text + " - ZGF Revit Family Database", true);
        }

        private void Initialize_INTERNAL_content()
        {
            pictureBoxPreview.Image = null;

            if (null == m_revit_content_ALL_INTERNAL || m_currentModelPathHasChanged)
            {
                m_currentModelPathHasChanged = false;

                FilteredElementCollector elementTypeCollector = new FilteredElementCollector(m_doc);
                List<ElementType> m_elementTypes = elementTypeCollector
                    .OfClass(typeof(ElementType))
                    .Cast<ElementType>()
                    .Where(e => null != e.Category)
                    .Where(e => !e.Category.IsTagCategory)
                    .Where(e => e.Category.CanAddSubcategory)
                    .Where(e => (e.Category.CategoryType == CategoryType.Annotation || e.Category.CategoryType == CategoryType.Model))
                    .ToList();

                // Collect Families so that can filter out certain un-placeable types:
                List<Family> m_families_InPlace = new FilteredElementCollector(m_doc)
                    .OfClass(typeof(Family))
                    .Cast<Family>()
                    .Where(f => f.IsInPlace)
                    .ToList();

                List<Family> m_families_Mass = new FilteredElementCollector(m_doc)
                    .OfClass(typeof(Family))
                    .Cast<Family>()
                    .Where(f => f.IsConceptualMassFamily)
                    .ToList();

                List<Family> m_families_CurtainPanel = new FilteredElementCollector(m_doc)
                    .OfClass(typeof(Family))
                    .Cast<Family>()
                    .Where(f => f.IsCurtainPanelFamily)
                    .ToList();

                List<Family> m_families_IsEditable = new FilteredElementCollector(m_doc)
                    .OfClass(typeof(Family))
                    .Cast<Family>()
                    .Where(f => !f.IsEditable)
                    .ToList();

                // Combine into a single list of families to exclude:
                List<Family> m_families_excluded =
                    m_families_InPlace
                        .Union(m_families_Mass)
                        .Union(m_families_CurtainPanel)
                        .Union(m_families_IsEditable)
                        .ToList();

                // List of Family Names to be excluded:
                List<string> _familyNamesToExclude = m_families_excluded.Select(x => x.Name).Distinct().ToList<string>();

                // Assemble a list of add-able types minus those exclude above
                IEnumerable<Revit_Content_INTERNAL_Type_Item> typesToBeIncluded = m_elementTypes
                    .Where(x => !_familyNamesToExclude
                    .Contains(x.FamilyName))
                    .ToList<ElementType>()
                    .Select(x => new Revit_Content_INTERNAL_Type_Item(x));

                //List<Family> diskallowedFamilies = m_families.Where(f => f.Name.Equals(m_elementTypes.fa)
                // For some reason, cannot figure out how to ID a curtain wall door or window
                //
                // TODO: Can accomplish WITHOUT ITERATING the collection ???
                //

                // #TODO: What if the ACTIVE PROJECT HAS CHANGED??

                // Collect the INTERNAL Content items:
                m_revit_content_ALL_INTERNAL = new List<Revit_Content_INTERNAL_Type_Item>(typesToBeIncluded); //.OrderBy(x => x.FamilyName);  //(typesToBeIncluded);

                // Group by CATEGORY to extract a list of unique categories:
                if (null == m_dictionary_INTERNAL_CATEGORIES) m_dictionary_INTERNAL_CATEGORIES = new SortedDictionary<string, CategoryTreeItem>();

                IEnumerable<IGrouping<string, Revit_Content_INTERNAL_Type_Item>> _tmpGroupedCategories =
                   m_revit_content_ALL_INTERNAL.GroupBy(c => c.CategoryName);

                // Disconnect the data source because we're changing values:
                dataGridCategories.DataSource = null;

                SortedDictionary<string, CategoryTreeItem> _tmpCategoryTreeItems = new SortedDictionary<string, CategoryTreeItem>();
                foreach (IGrouping<string, Revit_Content_INTERNAL_Type_Item> grp in _tmpGroupedCategories)
                {
                    if (m_dictionary_INTERNAL_CATEGORIES.ContainsKey(grp.Key))
                    {
                        _tmpCategoryTreeItems.Add(grp.Key, m_dictionary_INTERNAL_CATEGORIES[grp.Key]);
                    }
                    else
                    {
                        CategoryTreeItem cti = new CategoryTreeItem(grp.Key);
                        cti.StorageType = CategoryTreeItem.CategoryItemIntOrExt.Internal;
                        _tmpCategoryTreeItems.Add(grp.Key, cti);
                    }
                }
                m_dictionary_INTERNAL_CATEGORIES = _tmpCategoryTreeItems;

            }



            //if (null == m_dictionary_INTERNAL_CATEGORIES) //<--Can this be cached?
            //{
            //    // Disconnect DataSource
            //    dataGridCategories.DataSource = null;

            //    //m_revit_content_ALL_EXTERNAL = RevitContent_External_Utilities.GetRevitExternalContentItems(m_revit_version_number);

            //    m_dictionary_INTERNAL_CATEGORIES = new SortedDictionary<string, CategoryTreeItem>();

            //    //IEnumerable<IGrouping<string, Revit_Content_INTERNAL_Type_Item>> groupedCategories =
            //    //   m_revit_content_ALL_INTERNAL.GroupBy(c => c.CategoryName);

            //    foreach (IGrouping<string, Revit_Content_INTERNAL_Type_Item> grouping in groupedCategories)
            //    {
            //        CategoryTreeItem cti = new CategoryTreeItem(grouping.Key);
            //        cti.StorageType = cti.StorageType | CategoryTreeItem.CategoryItemIntOrExt.External;
            //        //m_dictionary_all_CATEGORIES.Add(grouping.Key, cti);

            //        m_dictionary_INTERNAL_CATEGORIES.Add(cti.Name, cti);

            //    }
            //}
            //else
            //{
            dataGridCategories.DataSource = m_dictionary_INTERNAL_CATEGORIES.Values.ToList();
            //}



            // Change the data bindings and column heading text
            colFamily.DataPropertyName = "FamilyName"; // 1
            colFamily.HeaderText = "Family";
            colType.DataPropertyName = "TypeName"; // 2
            colType.HeaderText = "Type";
            //colType.Visible = true;
            colCategory.DataPropertyName = "CategoryName"; // 3
            colCategory.HeaderText = "Category";
            colFileType.DataPropertyName = ""; // 4
            colFileType.HeaderText = "File type";
            colFileType.Visible = false;


            // Load the previous search
            if (m_lastSearchString_INTERNAL == string.Empty)
            {
                m_revit_CONTENT_filtered_datasource_INTERNAL = new MySortableBindingList<Revit_Content_INTERNAL_Type_Item>(m_revit_content_ALL_INTERNAL);
                gridFamilies.DataSource = m_revit_CONTENT_filtered_datasource_INTERNAL;
                gridFamilies.Sort((DataGridViewColumn)colFamily, ListSortDirection.Ascending);
                buttonClearSearch.Visible = false;
            }
            else
                Update_GridFamiliesPerSearchWords(comboboxSearchWords);
        }

        private void pictureBoxZgfLogo_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                gridFamilies.Sort((DataGridViewColumn)colFamily, ListSortDirection.Ascending);
            }
        }

        private void gridFamilies_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridView dgv = (DataGridView)sender;

            int lastColumnIndex = dgv.ColumnCount - 1; // dgv.Columns.GetLastColumn(DataGridViewElementStates.Displayed, DataGridViewElementStates.None).Index;

            // Bail if not the last column clicked (if last then it's the Edit button)
            // If row number is < 0 then user is clicking the heading
            if (lastColumnIndex != e.ColumnIndex)
                return;


            if (radioButtonRevitModel.Checked)
            {
                //m_doc.EditFamily() How to get a family?
                // Is it an EDITABLE family?


                Revit_Content_INTERNAL_Type_Item c = m_revit_CONTENT_filtered_datasource_INTERNAL[e.RowIndex];


                TaskDialog td = new TaskDialog("Open Component File");
                td.MainInstruction = "Do you really want to open this file?";
                td.MainContent = "File:  " + c.FamilyName
                    + "\r\nCategory:  " + c.CategoryName;
                //+ "Version:\t" + c.;
                td.TitleAutoPrefix = false;
                td.CommonButtons = TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No;
                td.DefaultButton = TaskDialogResult.No;

                TaskDialogResult result = TaskDialogResult.No;
                if (m_displayEditButton)
                    result = td.Show();

                if (result == TaskDialogResult.Yes)
                {
                    Debug.WriteLine(c.TheElementType.FamilyName);
                    // Try to get the Family
                    Family f = GetFamilyByNameAndCategory(m_doc, c.FamilyName, c.FamilyCategory);

                    // See if Family is already open...
                    DocumentSet ds = m_doc.Application.Documents;
                    foreach (Document d in ds)
                    {
                        Debug.WriteLine(d.PathName);
                    }

                    if (null != f && !m_doc.IsModifiable && !m_doc.IsReadOnly & f.IsUserCreated)
                    {
                        try
                        {
                            //DirectoryInfo tmpFolder = new DirectoryInfo( Environment.GetEnvironmentVariable("TMP") );
                            //string randomFilePath = Path.Combine(tmpFolder.FullName, Path.GetRandomFileName());

                            Document fDoc = m_doc.EditFamily(f);

                            // Get PartAtom and saved path:
                            //f.ExtractPartAtom(randomFilePath);                            
                            //XDocument xmlPartAtom = XDocument.Load(randomFilePath);
                            //File.Delete(randomFilePath);

                            DirectoryInfo userRevitDocsFolder = new DirectoryInfo(
                                Path.Combine(
                                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                                    "Revit_" + m_revit_version_number.ToString())
                            );
                            if (!userRevitDocsFolder.Exists) userRevitDocsFolder.Create();

                            string familyFileName = fDoc.Title;
                            // Check to see if already open. If so, activate the doc:

                            // Where to put it?
                            string fDocPath = fDoc.PathName;
                            if (string.Empty == fDocPath)
                                fDocPath = Path.Combine(userRevitDocsFolder.FullName, familyFileName);

                            FileInfo famFileInfo = new FileInfo(fDocPath);
                            if (famFileInfo.Exists)
                            {
                                try
                                {
                                    famFileInfo.Delete(); // <--This will fail if revit has the file open...    
                                                          // TODO: Try to activate the file:
                                }
                                catch { }
                            }

                            // Save a copy:
                            try
                            {
                                fDoc.SaveAs(fDocPath);
                                fDoc.Close(false);
                                famFileInfo = new FileInfo(fDocPath); //<--Need to recreate the Fileinfo
                            }
                            catch { }

                            // Open it:                            
                            if (famFileInfo.Exists)
                            {
                                // TODO: How to activate doc if open?
                                m_uiDoc.Application.OpenAndActivateDocument(famFileInfo.FullName);
                            }

                        }
                        catch (System.Exception ex)
                        {
                            // Log this....
                            Debug.WriteLine(ex.Message);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Could not load the family: " + c.CategoryName + " : " + c.FamilyName);
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {
                // TODO: Warning, do you want to open this family?
                // Is it an EDITABLE family?
                Revit_Content_EXTERNAL_Item c = m_revit_CONTENT_filtered_datasource_EXTERNAL[e.RowIndex];


                TaskDialog td = new TaskDialog("Open Component File");
                td.MainInstruction = "Do you really want to open this file?";
                td.MainContent = "File: " + c.FileName
                    + "\r\nCategory: " + c.CategoryName
                    + "\r\nVersion: " + c.FileVersion.ToString();
                td.TitleAutoPrefix = false;
                td.CommonButtons = TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No;
                td.DefaultButton = TaskDialogResult.No;

                TaskDialogResult result = td.Show();

                if (result == TaskDialogResult.Yes)
                {
                    FileInfo rf = new FileInfo(c.FullPathName);
                    try
                    {
                        if (rf.Exists)
                        {

                            m_uiDoc.Application.OpenAndActivateDocument(rf.FullName);

                        }

                        else
                            throw new FileNotFoundException();
                    }
                    catch
                    {
                        string msg = "Error opening: " + rf.FullName;
                    }
                }
                else
                {
                    return;
                }
                //MessageBox.Show(c.FullPathName);
            }

            // TODO: Add a tooltip for when hovering over the edit button
            SaveSettings();
            this.Close();
        }

        /// <summary>
        /// Get a family from the model by name and category
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="familyName"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        private Family GetFamilyByNameAndCategory(Document doc, string familyName, Category category)
        {
            Family returnFamily = null;
            try
            {
                List<Family> fams = new FilteredElementCollector(doc)
                    .OfClass(typeof(Family))
                    //.Where(c => c.Category == category)
                    .Where(n => n.Name == familyName)
                    .Cast<Family>()
                    .ToList();

                if (fams.Count > 0)
                    returnFamily = fams[0];
            }
            catch { }

            return returnFamily;
        }

        private void gridFamilies_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if ((gridFamilies.Rows.Count > 0) && (e.RowIndex > -1))
            {
                Image editImage = Properties.Resources.EditFamily;
                int colEditButtonIndex = gridFamilies.ColumnCount - 1;

                try
                {
                    buttonOK.Enabled = true;
                    SetFamilyPreviewImage(e.RowIndex);
                }
                catch (Exception ex)
                {
                    buttonOK.Enabled = false;
                    string s = ex.Message;
                }

                // Edit Content icon:

                if (radioButtonRevitModel.Checked)
                {
                    Revit_Content_INTERNAL_Type_Item rc = m_revit_CONTENT_filtered_datasource_INTERNAL[e.RowIndex];

                    if (radioButtonRevitModel.Checked)
                        labelTypeDescrip.Text = rc.TypeDescription;

                    Family f = GetFamilyByNameAndCategory(m_doc, rc.FamilyName, rc.FamilyCategory);

                    if (null == f || m_doc.IsModifiable || m_doc.IsReadOnly || !f.IsUserCreated)
                    {
                        try
                        {
                            m_displayEditButton = false;
                        }
                        catch { }
                    }
                    else
                    {
                        m_displayEditButton = true;
                    }
                }
                else
                {
                    // #TODO: Check content type then add editImage button, if appropriate:

                    // Get the content item:
                    if (e.RowIndex > -1 && null != m_revit_content_CATEGORY_FILTERED_EXTERNAL)
                    {
                        if (m_revit_content_CATEGORY_FILTERED_EXTERNAL[e.RowIndex].FileType.Equals("Loadable Family"))
                            m_displayEditButton = true;
                        else
                            m_displayEditButton = false;
                    }
                }

                // Enable Edit Content for external content
                if (m_displayEditButton)
                    gridFamilies[colEditButtonIndex, e.RowIndex].Value = editImage;

            }
            else
                pictureBoxPreview.Image = ZGF.Revit.Properties.Resources.No_preview_150x150;

        }


        private void gridFamilies_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            //Image blankImg = Image.f

            int colEditButtonIndex = dgv.ColumnCount - 1;

            if ((gridFamilies.Rows.Count > 0) && (e.RowIndex > -1))
            {
                gridFamilies[colEditButtonIndex, e.RowIndex].Value = Properties.Resources.EditFamilyBlank;
            }
        }

        private void gridFamilies_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            //// Ignore Header
            //if (e.RowIndex < 0)
            //    return;

            //DataGridView dgv = sender as DataGridView;
            //int colEditButtonIndex = dgv.ColumnCount - 1;

            //if ((gridFamilies.Rows.Count > 0) && (e.RowIndex > -1))
            //{
            //    gridFamilies[colEditButtonIndex, e.RowIndex].Value = Properties.Resources.EditFamilyBlank;
            //    e.Handled = true;
            //}


            //if (e.ColumnIndex == colEditButtonIndex)
            //{
            //    //e.Paint(e.CellBounds, DataGridViewPaintParts.All);

            //    //var w = Properties.Resources.EditFamily.Width;  //.SomeImage.Width;
            //    //var h = Properties.Resources.EditFamily.Height;
            //    //var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
            //    //var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;

            //    //Image editImage = Properties.Resources.EditFamily;
            //    //e.Graphics.DrawImage(editImage, new System.Drawing.Rectangle(x, y, w, h));
            //    e.gr
            //    e.Handled = true;
            //}
        }

        #region Category management

        private void toolStripMenuItem_Isolate()
        {
            // Temporarily un-subscribe from after check even so that it doesn't fire for each node change:
            dataGridCategories.CellClick -= dataGridCategories_CellClick;


            CategoryTreeItem c = gridFamilies.CurrentRow.DataBoundItem as CategoryTreeItem;
            if (null != c)
            {
                // INTERNAL 
                if (null != m_dictionary_INTERNAL_CATEGORIES)
                    foreach (CategoryTreeItem cti in m_dictionary_INTERNAL_CATEGORIES.Values)
                    {
                        cti.IsChecked = cti.Name == c.Name ? true : false;
                    }
                // EXTERNAL
                if (null != m_dictionary_EXTERNAL_CATEGORIES)
                    foreach (CategoryTreeItem cti in m_dictionary_EXTERNAL_CATEGORIES.Values)
                    {
                        cti.IsChecked = cti.Name == c.Name ? true : false;
                    }
            }

            //dataGridCategories.Refresh();
            //dataGridCategories.Update();

            // Update filtered content:
            UpdateFilteredContent();

            dataGridCategories.CellClick += dataGridCategories_CellClick;
        }

        private void toolStripMenuItem_All_Click(object sender, EventArgs e)
        {
            // Temporarily un-subscribe from after check even so that it doesn't fire for each node change:
            dataGridCategories.CellClick -= dataGridCategories_CellClick;

            // INTERNAL 
            if (null != m_dictionary_INTERNAL_CATEGORIES)
                foreach (CategoryTreeItem cti in m_dictionary_INTERNAL_CATEGORIES.Values)
                {
                    cti.IsChecked = true;
                }
            // EXTERNAL
            if (null != m_dictionary_EXTERNAL_CATEGORIES)
                foreach (CategoryTreeItem cti in m_dictionary_EXTERNAL_CATEGORIES.Values)
                {
                    cti.IsChecked = true;
                }

            //dataGridCategories.Refresh();

            // Update filtered content:
            UpdateFilteredContent();

            dataGridCategories.CellClick += dataGridCategories_CellClick;

            Update_GridFamiliesPerSearchWords(comboboxSearchWords);
        }

        private void toolStripMenuItem_None_Click(object sender, EventArgs e)
        {
            dataGridCategories.CellClick -= dataGridCategories_CellClick;

            // INTERNAL 
            if (null != m_dictionary_INTERNAL_CATEGORIES)
                foreach (CategoryTreeItem cti in m_dictionary_INTERNAL_CATEGORIES.Values)
                {
                    cti.IsChecked = false;
                }

            // EXTERNAL
            if (null != m_dictionary_EXTERNAL_CATEGORIES)
                foreach (CategoryTreeItem cti in m_dictionary_EXTERNAL_CATEGORIES.Values)
                {
                    cti.IsChecked = false;
                }



            //dataGridCategories.Refresh();

            // Update filtered content:
            UpdateFilteredContent();

            dataGridCategories.CellClick += dataGridCategories_CellClick;

            Update_GridFamiliesPerSearchWords(comboboxSearchWords);
        }

        private void toolStripMenuItem_Invert_Click(object sender, EventArgs e)
        {
            dataGridCategories.CellClick -= dataGridCategories_CellClick;

            // Invert 

            // INTERNAL 
            if (null != m_dictionary_INTERNAL_CATEGORIES)
                foreach (CategoryTreeItem cti in m_dictionary_INTERNAL_CATEGORIES.Values)
                {
                    cti.IsChecked = !cti.IsChecked;
                }
            // EXTERNAL
            if (null != m_dictionary_EXTERNAL_CATEGORIES)
                foreach (CategoryTreeItem cti in m_dictionary_EXTERNAL_CATEGORIES.Values)
                {
                    cti.IsChecked = !cti.IsChecked;
                }

            //dataGridCategories.Refresh();

            // Update filtered content:
            UpdateFilteredContent();

            dataGridCategories.CellClick += dataGridCategories_CellClick;

            Update_GridFamiliesPerSearchWords(comboboxSearchWords);
        }





        private void dataGridCategories_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // toggle row check property
            CategoryTreeItem cti = (CategoryTreeItem)dataGridCategories.CurrentRow.DataBoundItem;
            cti.IsChecked = !cti.IsChecked;

            _checked_Internal_CategoryListHasChanged = true;

            if (radioButtonRevitModel.Checked)
            {
                SyncCategoryNameLists(true);
                
            }
            else
            {
                SyncCategoryNameLists(false);
                
            }

            //dataGridCategories.Rows[e.RowIndex].DefaultCellStyle = cti.IsChecked ? m_gridCategories_SELECTED : m_gridCategories_NOT_SELECTED;

            dataGridCategories.Refresh();

            // Update filtered content:
            UpdateFilteredContent();

            Update_GridFamiliesPerSearchWords(comboboxSearchWords);
        }



        /// <summary>
        /// Helper function to update category-filtered content Lists
        /// </summary>
        private void UpdateFilteredContent()
        {
            if (radioButtonRevitModel.Checked)
            {
                m_revit_content_CATEGORY_FILTERED_INTERNAL = Get_INTERNAL_filterered_by_CATEGORY();
            }
            else
            {
                m_revit_content_CATEGORY_FILTERED_EXTERNAL = Get_EXTERNAL_filterered_by_CATEGORY();
            }

            dataGridCategories.Refresh();

        }

        /// <summary>
        /// Get a list of strings representing the names of the checked categories from the Tree 
        /// of categories. Used for filtering the content.
        /// </summary>
        /// <returns></returns>
        private List<string> GetCheckedCategoryNames()
        {
            List<string> categories = new List<string>();
            if (radioButtonRevitModel.Checked)
            {
                categories = m_dictionary_INTERNAL_CATEGORIES
                                .Where(t => t.Value.IsChecked)
                                .Select(t => t.Key)
                                .ToList();
            }
            else
            {
                categories = m_dictionary_EXTERNAL_CATEGORIES
                               .Where(t => t.Value.IsChecked)
                               .Select(t => t.Key)
                               .ToList();
            }

            return categories;
        }

        /// <summary>
        /// Gets a sortable binding list of INTERNAL content with unchecked categories filtered out.
        /// </summary>
        /// <returns></returns>
        private List<Revit_Content_INTERNAL_Type_Item> Get_INTERNAL_filterered_by_CATEGORY()
        {
            return new List<Revit_Content_INTERNAL_Type_Item>(m_revit_content_ALL_INTERNAL
                .Where(c => GetCheckedCategoryNames()
                .Contains(c.CategoryName)));
        }

        /// <summary>
        /// Gets a sortable binding list of EXTERNAL content with unchecked categories filtered out.
        /// </summary>
        /// <returns></returns>
        private List<Revit_Content_EXTERNAL_Item> Get_EXTERNAL_filterered_by_CATEGORY()
        {
            return new List<Revit_Content_EXTERNAL_Item>(m_revit_content_ALL_EXTERNAL
                .Where(c => GetCheckedCategoryNames()
                .Contains(c.CategoryName)));
        }




        #endregion


        private void buttonSelectCategories_MouseEnter(object sender, EventArgs e)
        {
            System.Windows.Forms.Control thisBtn = sender as System.Windows.Forms.Control;

            string ttText = string.Empty;

            switch (thisBtn.Name)
            {
                case "buttonSelectAll":
                    ttText = "Select ALL Categories";
                    break;
                case "buttonSelectNone":
                    ttText = "De-select ALL Categories";
                    break;
                case "buttonSelectInverse":
                    ttText = "Invert Categories selection";
                    break;
                case "radioButtonRevitModel":
                    ttText = "Browse CURRENT MODEL content";
                    break;
                case "radioButtonLibrary":
                    ttText = "Browse NETWORK LIBRARY content";
                    break;
            }

            m_tooltip.UseFading = true;
            m_tooltip.Show(ttText, thisBtn, 1500);
        }

        private void buttonSelectCategories_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Control thisBtn = sender as System.Windows.Forms.Control;



            switch (thisBtn.Name)
            {
                case "buttonSelectAll":
                    toolStripMenuItem_All_Click(sender, e);
                    break;
                case "buttonSelectNone":
                    toolStripMenuItem_None_Click(sender, e);
                    break;
                case "buttonSelectInverse":
                    toolStripMenuItem_Invert_Click(sender, e);
                    break;
            }

        }
        private void dataGridCategories_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            

            //bool catIsChecked = (bool)dataGridCategories.Rows[e.RowIndex].Cells[0].Value;

            //if (catIsChecked)
            //    dataGridCategories.Rows[e.RowIndex].DefaultCellStyle = m_gridCategories_SELECTED;
            //else
            //    dataGridCategories.Rows[e.RowIndex].DefaultCellStyle = m_gridCategories_NOT_SELECTED;



        }

        /// <summary>
        /// Returns a Rectangle representing the active screen size. 
        /// </summary>
        /// <returns></returns>
        public System.Drawing.Rectangle GetScreenSize()
        {
            return System.Windows.Forms.Screen.FromControl(this).Bounds;
        }


        private void dataGridCategories_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            System.Windows.Forms.Control thisBtn = sender as System.Windows.Forms.Control;

            if (e.RowIndex < 0) return;

            string catName = dataGridCategories.Rows[e.RowIndex].Cells[1].Value.ToString();
            bool catIsChecked = (bool)dataGridCategories.Rows[e.RowIndex].Cells[0].Value;

            if (catIsChecked)
                dataGridCategories.Rows[e.RowIndex].DefaultCellStyle = m_gridCategories_SELECTED;
            else
                dataGridCategories.Rows[e.RowIndex].DefaultCellStyle = m_gridCategories_NOT_SELECTED;

            dataGridCategories.Update();

            Debug.WriteLine("Control: {0}", thisBtn.Name);

        }
        private void dataGridCategories_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            //if (dataGridCategories.IsCurrentCellDirty)
            //    dataGridCategories.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dataGridCategories_MouseUp(object sender, MouseEventArgs e)
        {
            
        }

        private void dataGridCategories_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            return;

            string catName = dataGridCategories.Rows[e.RowIndex].Cells[1].Value.ToString();
            bool catIsChecked = (bool)dataGridCategories.Rows[e.RowIndex].Cells[0].Value;

            if (catIsChecked)
                dataGridCategories.Rows[e.RowIndex].DefaultCellStyle = m_gridCategories_SELECTED;
            else
                dataGridCategories.Rows[e.RowIndex].DefaultCellStyle = m_gridCategories_NOT_SELECTED;

            //if (radioButtonRevitModel.Checked)
            //{
            //    m_dictionary_INTERNAL_CATEGORIES[catName].IsChecked = (bool)dataGridCategories.Rows[e.RowIndex].Cells[1].Value;
            //}
            //else
            //{
            //    m_dictionary_EXTERNAL_CATEGORIES[catName].IsChecked = (bool)dataGridCategories.Rows[e.RowIndex].Cells[1].Value;
            //}

            Debug.WriteLine(catName);
            Debug.WriteLine("\tRow:{0}, Column:{1}", e.RowIndex, e.ColumnIndex);

        }

        private void dataGridCategories_CurrentCellChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("CurrentCellChanged: {0}", sender.GetType().ToString());
            //dataGridCategories.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

    
    }



    /// <summary>
    /// Wrapper for a Family Type, used for sorting INTERNAL items in browser window
    /// </summary>
    class Revit_Content_INTERNAL_Type_Item : IComparable
    {
        // M E M B E R S
        private string m_family_categoryName;
        private string m_family_name;
        private string m_type_name;
        private string m_search_term;
        private Category m_category;
        private string m_typeDescription = string.Empty;

        // C O N S T R U C T O R

        public Revit_Content_INTERNAL_Type_Item(ElementType theElementType)
        {
            this.TheElementType = theElementType;
            m_family_categoryName = theElementType.Category.Name;
            m_family_name = theElementType.FamilyName;
            m_type_name = theElementType.Name;
            m_category = TheElementType.Category;

            try
            {
                Parameter p = theElementType.get_Parameter(BuiltInParameter.ALL_MODEL_DESCRIPTION);
                if (null != p)
                {
                    m_typeDescription = p.AsString();
                    //Debug.WriteLine(m_family_name + " :: " + m_typeDescription);
                }
            }
            catch { }

            //try
            //{
            //    //Family f = FamilyBrowser.
            //    this.UserEditableImage = ZGF.Revit.Properties.Resources.EditFamily;
            //}
            //catch { }
        }

        // P R O P E R T I E S
        public ElementType TheElementType { get; set; }

        public string CategoryName { get { return m_family_categoryName; } }
        public string FamilyName { get { return m_family_name; } }
        public string TypeName { get { return m_type_name; } }
        public Category FamilyCategory { get { return m_category; } }

        public string TypeDescription { get { return m_typeDescription; } }

        public Image UserEditableImage { get { return ZGF.Revit.Properties.Resources.EditFamily; } }

        private SortingMethod m_sorting_method = SortingMethod.Category;



        public string SearchTerm
        {
            get
            {
                switch (m_sorting_method)
                {
                    case SortingMethod.Category:
                        m_search_term = m_family_categoryName + " " + m_family_name + " " + m_type_name + " " + m_typeDescription;
                        break;
                    case SortingMethod.Family:
                        m_search_term = m_family_name + " " + m_type_name + " " + m_family_categoryName + " " + m_typeDescription;
                        break;
                    case SortingMethod.Type:
                        m_search_term = m_type_name + " " + m_family_name + " " + m_family_categoryName + " " + m_typeDescription;
                        break;
                }
                return m_search_term;
            }
        }

        enum SortingMethod
        {
            Category,
            Family,
            Type
        }

        // M E T H O D S

        public int CompareTo(object obj)
        {
            Revit_Content_INTERNAL_Type_Item item = (Revit_Content_INTERNAL_Type_Item)obj;
            return String.Compare(
                (this.CategoryName + this.FamilyName + this.TypeName + this.TypeDescription),
                (item.CategoryName + item.FamilyName + item.TypeName + item.TypeDescription));
        }


        public bool IsMatch(string searchWords)
        {
            char[] delimiters = new char[] { ' ', System.Convert.ToChar(160) };
            string stringToMatch = this.SearchTerm.ToLower();
            string literal = searchWords.ToLower();
            string[] words = literal.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

            int matches = 0;

            foreach (string s in words)
            {
                if (literal == string.Empty) continue;

                if ((stringToMatch.Contains(literal)) || (stringToMatch.Contains(s)))
                    matches++;
                else
                    matches--;
            }

            return (matches == words.Length);
        }


    }



}