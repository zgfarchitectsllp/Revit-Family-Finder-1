# define REVIT_2019
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ZGF.Revit
{    
    public partial class ViewFinder : System.Windows.Forms.Form
    {
        #region P R I V A T E   M E M B E R S
        private List<RevitView> m_views_all = new List<RevitView>();
        
        private MySortableBindingList<RevitView> m_views_sortable_filtered;
        private MySortableBindingList<RevitView> m_views_open = new MySortableBindingList<RevitView>();
        
        private UIDocument m_ui_doc;
        private Document m_doc;
        
        private Autodesk.Revit.DB.View m_active_view;

        // Tooltip for hoverhelp
        private ToolTip m_toolTip = new ToolTip();

        private ViewCollector m_ViewCollector;
        private int m_sortedColumn = 0;
        private ListSortDirection m_sortDirection = ListSortDirection.Ascending;
        private int m_firstDisplayedRow = 0;
        
        private RevitView m_lastView = null;

        private Keys m_mode_key = Keys.ShiftKey;

        private SearchHistory m_search_history;

        
        #endregion

        public ViewFinder(ExternalCommandData commandData, ref string errorMessage)
        {
            InitializeComponent();
                        
            this.Text += " (v" + Assembly.GetExecutingAssembly().GetName().Version.Major + "." +
                Assembly.GetExecutingAssembly().GetName().Version.Minor +
                Assembly.GetExecutingAssembly().GetName().Version.Build + ")";
            
            //gridViews.DoubleBuffered(true);  //<--Uses Extension method: http://bitmatic.com/c/fixing-a-slow-scrolling-datagridview
            MakeGridViewDoublebuffered(gridViews);
            //gridViews.AutoGenerateColumns = true;

            m_toolTip.UseFading = true;

            //string test = Application.CompanyName + "\n" + Application.LocalUserAppDataPath;
            
            this.gridViews.AutoGenerateColumns = false;
            m_ui_doc = commandData.Application.ActiveUIDocument;
            m_doc = commandData.Application.ActiveUIDocument.Document;
            m_active_view = m_ui_doc.ActiveView;

            #region ROW FORMAT
            DataGridViewCellStyle altRowStyle = new DataGridViewCellStyle(gridViews.DefaultCellStyle);
            altRowStyle.BackColor = System.Drawing.Color.FromArgb(231, 234, 239);
            //altRowStyle.ForeColor = System.Drawing.Color.Gray;
            //Font f = new Font(altRowStyle.Font, FontStyle.Italic);
            //altRowStyle.Font = f;
            gridViews.AlternatingRowsDefaultCellStyle = altRowStyle;
            #endregion

            // Open Views:            
            GetOpenViews(ref m_views_open, ref errorMessage);
            

            buttonPlaceOnSheet.Enabled = m_active_view.ViewType == ViewType.DrawingSheet;

            // Collect All Views and initialize private member variables:
            errorMessage = GetAllProjectViews();

            // Connect View list to DataGrid Control:
            gridViews.DataSource = m_views_sortable_filtered;


            // Column AutoSize settings:
            // http://msdn.microsoft.com/en-us/library/74b2wakt.aspx
            //foreach (DataGridViewColumn c in gridViews.Columns)
            //{
            //    c.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //}
            //colViewName.FillWeight = 500;
            

            // Retrieve Column visibility Settings:
            chkColViewTypeName.Checked = colViewType.Visible = ZGF.Revit.Properties.Settings.Default.ViewFinderColViewTypeName;
            chkColScale.Checked = colViewScale.Visible = ZGF.Revit.Properties.Settings.Default.ViewFinderColScale;
            chkColDiscipline.Checked = colViewDiscipline.Visible = ZGF.Revit.Properties.Settings.Default.ViewFinderColDiscipline;
            chkColPhaseName.Checked = colViewPhase.Visible = ZGF.Revit.Properties.Settings.Default.ViewFinderColPhase;
            chkColReferencingSheet.Checked = colViewRefSheet.Visible = false; // colViewRefSheet.Visible = ZGF.Revit.Properties.Settings.Default.ViewFinderColRefSheet;
            //if (chkColReferencingSheet.Checked) m_ViewCollector.CollectSheetReferenceViews(ref errorMessage);
            chkColTitleOnSheet.Checked = colViewTitleOnSheet.Visible = ZGF.Revit.Properties.Settings.Default.ViewFinderColTitleOnSheet;

            Size savedSize = ZGF.Revit.Properties.Settings.Default.ViewFinderSize;
            this.Size = ( (savedSize.Width < this.MinimumSize.Width) || (savedSize.Height < this.MinimumSize.Height) ) ? this.MinimumSize : this.Size;
            
            System.Drawing.Rectangle screenBounds = ZGF.Revit.Utilities.GetRevitScreenSize(commandData.Application);
            if (screenBounds.Width < this.Size.Width || screenBounds.Height < this.Size.Height)
                this.Size = this.MinimumSize;

            // Column Widths
            gridViews.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            for (int i = 0; i < gridViews.ColumnCount; i++)
            {
                DataGridViewColumn col = gridViews.Columns[i];
                switch (i)
                {
                    case 0: // View Name                        
                    case 7: // Title On Sheet
                        col.MinimumWidth = 150;
                        col.FillWeight = 200;
                        break;
                    case 6:
                        col.MinimumWidth = 150;
                        col.FillWeight = 100;
                        break;
                    default:
                        col.MinimumWidth = 50;
                        col.FillWeight = 50;
                        break;
                }
            }
            //if (colViewName.Width > 0) colViewName.Width = ZGF.Revit.Properties.Settings.Default.colViewCWidName;
            //if (colViewDiscipline.Width > 0) colViewDiscipline.Width = ZGF.Revit.Properties.Settings.Default.colViewCWidDiscipline;
            //if (colViewPhase.Width > 0) colViewPhase.Width = ZGF.Revit.Properties.Settings.Default.colViewCWidPhase;
            //if (colViewRefSheet.Width > 0) colViewRefSheet.Width = ZGF.Revit.Properties.Settings.Default.colViewCWidRefSht;
            //if (colViewScale.Width > 0) colViewScale.Width = ZGF.Revit.Properties.Settings.Default.colViewCWidScale;
            //if (colViewTitleOnSheet.Width > 0) colViewTitleOnSheet.Width = ZGF.Revit.Properties.Settings.Default.colViewCWidTitleOnSheet;

            // Set preview toggle:
            chkPreview.Checked = ZGF.Revit.Properties.Settings.Default.ViewFinderPreviewToggle;            
            labelPreviewDisabled.BackColor = labelPreviewIsSchedule.BackColor = elementHostThePreview.BackColor;
            labelPreviewDisabled.Visible = !chkPreview.Checked;

            // Column sorting
            m_sortDirection = ZGF.Revit.Properties.Settings.Default.ViewFinderSortedColumnSortDirection;
            Debug.WriteLine("A - Sort direction: " + m_sortDirection.ToString());
            m_sortedColumn = ZGF.Revit.Properties.Settings.Default.ViewFinderSortedColumnIndex;
            if (m_sortDirection == ListSortDirection.Descending) gridViews.Sort(gridViews.Columns[m_sortedColumn], m_sortDirection);
            Debug.WriteLine("B - Sort direction: " + m_sortDirection.ToString());

            // comboBox search history items
            checkBoxRememberLastSearch.Checked = ZGF.Revit.Properties.Settings.Default.ViewFinderRememberSearch;

            if (null == ZGF.Revit.Properties.Settings.Default.ViewFinderSearchHistory)
                m_search_history = new SearchHistory();
            else
                m_search_history = new SearchHistory(ZGF.Revit.Properties.Settings.Default.ViewFinderSearchHistory);

            comboBoxSearchTerms.Items.AddRange(m_search_history.AsStringArray());
            //if (m_search_history.Count > 0)
            //    comboBoxSearchTerms.Text = m_search_history.AsList()[0];
            if (ZGF.Revit.Properties.Settings.Default.ViewFinderRememberSearch)
                comboBoxSearchTerms.Text = ZGF.Revit.Properties.Settings.Default.ViewFinderLastSearch;
            else
                comboBoxSearchTerms.Text = string.Empty;

            // ZGF Logo PictureBox backcolor
            pictureBoxZgfLogo.BackColor = System.Drawing.Color.FromArgb(254, 252, 240);

            // Close ViewFinder after Activate view setting:
            //chkCloseAfterActivateView.Visible = false;
            chkCloseAfterActivateView.Checked = ZGF.Revit.Properties.Settings.Default.ViewFinderCloseAfterActivate;

            if (gridViews.RowCount > 0)
            {
                Application.DoEvents();
                RevitView view = m_lastView = m_views_sortable_filtered[0];
                buttonPlaceOnSheet.Enabled = view.CanAddToSheet;
                if (chkPreview.Checked) UpdatePreview(view);
            }

            SetViewCountLabel();

            // Trying if can pre-collect views...
            if (m_views_all.Count < 1000)
            {
                chkColReferencingSheet.Checked = true;
                chkColReferencingSheet.Enabled = false;
            }
            //

            // Add an EventHandler for Selectionchanged:
            this.gridViews.SelectionChanged += new System.EventHandler(this.gridViews_SelectionChanged);

            //textBoxSearchTerms.Focus();
            comboBoxSearchTerms.Focus();
        }

        // Collects open views for when shift key is pressed over grid view
        private void GetOpenViews(ref MySortableBindingList<RevitView> openViews, ref string errorMessage)
        {
            openViews.Clear();
            foreach (UIView uiv in m_ui_doc.GetOpenUIViews())
            {
                Autodesk.Revit.DB.View cv = m_doc.GetElement(uiv.ViewId) as Autodesk.Revit.DB.View;
                RevitView rv = new RevitView(cv, m_ui_doc, gridViews, ref errorMessage);
                Debug.WriteLine(rv.Name + rv.theView.Document.PathName);
                openViews.Add(rv);
            }
        }

        private MySortableBindingList<RevitView> GetOpenViews()
        {
            string msg = "";
            MySortableBindingList<RevitView> tmpViewList = new MySortableBindingList<RevitView>();
            foreach (UIView uiv in m_ui_doc.GetOpenUIViews())
            {
                Autodesk.Revit.DB.View cv = m_doc.GetElement(uiv.ViewId) as Autodesk.Revit.DB.View;
                RevitView rv = new RevitView(cv, m_ui_doc, gridViews, ref msg);
                tmpViewList.Add(rv);
            }
            return tmpViewList;
        }

        /// <summary>
        /// Collects Views in a helper class and initializes member variables
        /// </summary>
        /// <param name="commandData"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        private string GetAllProjectViews()
        {
            string errMsg = string.Empty;
            m_ViewCollector = new ViewCollector(m_ui_doc, this.gridViews, ref errMsg);

            m_views_all.AddRange(m_ViewCollector.ToArray());
            // Sortable list:
            m_views_sortable_filtered = new MySortableBindingList<RevitView>(m_views_all);
            return errMsg;
        }

        /// <summary>
        /// Updates View count message in dialog
        /// </summary>
        private void SetViewCountLabel()
        {
            //labelViewCount.Text = m_views_sortable_filtered.Count + " of " + m_views_all.Count.ToString() + " Views";
            labelViewCount.Text = gridViews.Rows.Count + " of " + m_views_all.Count.ToString() + " Views";
        }

        private void buttonPlaceOnSheet_Click(object sender, EventArgs e)
        {            
            // Place Selected view on sheet
            RevitView selectedView = m_views_sortable_filtered[gridViews.CurrentRow.Index];

            Debug.WriteLine(selectedView.Name);
            
            this.Hide();

            selectedView.PlaceOnSheet();
            //buttonPlaceOnSheet.Enabled = false;
            
            SaveSettings();

            if (chkCloseAfterActivateView.Checked)             
                this.Close();
            else
                this.Show();
            
        }



        private void buttonClose_Click(object sender, EventArgs e)
        {
            
            this.SaveSettings();
            this.Close();
        }

        

        private void gridViews_KeyDown(object sender, KeyEventArgs e)
        {
            // This disables the default keypress behavior.
            // In the case of the datagrid control, the enter key advances to the next row.
            //e.Handled = true;
            if ((e.KeyCode == Keys.Enter || e.KeyCode == Keys.Space))
            {
                if (gridViews.Rows.Count > 0)
                    buttonActivate_Click(sender, new EventArgs());
                return;
            }

//            if ((e.KeyCode == m_mode_key) && m_modeKey_Up)
//            {
//                m_modeKey_Up = false;
//#if DEBUG
//                labelViewCount.Text = "Shift key down...";
//#endif
//                comboBoxSearchTerms.Enabled = false;
//                gridViews.DataSource = null;
//                m_temp_views_mode_key_down = m_views_sortable_filtered;
//                m_views_sortable_filtered = m_views_open;
//                gridViews.DataSource = m_views_sortable_filtered;
//            }
        }

        private bool m_mode_viewlist_show_all = true;
        private void gridViews_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == m_mode_key)
            {
                m_mode_viewlist_show_all = !m_mode_viewlist_show_all; // Toggle
                gridViews.DataSource = null;
                if (m_mode_viewlist_show_all)
                    gridViews.DataSource = m_views_sortable_filtered;
                else
                    gridViews.DataSource = GetOpenViews();

                SetViewCountLabel();

                comboBoxSearchTerms.Enabled = m_mode_viewlist_show_all;
                
            }
            //if (e.KeyCode == m_mode_key)
            //{
            //    m_modeKey_Up = true;
            //    m_views_sortable_filtered = m_temp_views_mode_key_down;
            //    SetViewCountLabel();
            //    comboBoxSearchTerms.Enabled = true;
            //    gridViews.DataSource = null;
            //    gridViews.DataSource = m_views_sortable_filtered;
            //}
        }

       

        private void comboBoxSearchTerms_TextChanged(object sender, EventArgs e)
        {
            string searchWords = this.comboBoxSearchTerms.Text;

            gridViews.DataSource = null;
            if (comboBoxSearchTerms.Text.Equals(string.Empty))
            {
                m_views_sortable_filtered = new MySortableBindingList<RevitView>(m_views_all);
            }
            else
                m_views_sortable_filtered = new MySortableBindingList<RevitView>(m_views_all.FindAll(item => item.IsMatch(searchWords)));

            gridViews.DataSource = m_views_sortable_filtered;

            buttonActivate.Enabled = m_views_sortable_filtered.Count > 0;
            if (m_views_sortable_filtered.Count > 0)
            {
                gridViews.Sort(gridViews.Columns[m_sortedColumn], m_sortDirection);
                if (chkPreview.Checked)
                {                    
                    UpdatePreview(m_views_sortable_filtered[gridViews.CurrentRow.Index]);
                }
            }
            else
                if (chkPreview.Checked) DisposePreview();
            
            SetViewCountLabel();
        }

               

        private void gridViews_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                gridViews_CellEnter(new object(), new DataGridViewCellEventArgs(e.ColumnIndex, e.RowIndex));
                //gridViews_RowEnter(new object(), new DataGridViewRowEventArgs(gridViews.CurrentRow));
            }
            
        }

        private void gridViews_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            // Get view associated with current row
            RevitView view = m_views_sortable_filtered[gridViews.CurrentRow.Index];                
            buttonPlaceOnSheet.Enabled = (m_active_view.ViewType == ViewType.DrawingSheet) & view.CanAddToSheet;

            UpdateLabelPreview(view);
            
        }

        private void gridViews_Click(object sender, EventArgs e)
        {
            if (textBoxRenameView.Visible)
            {
                RenameView(theSelectedRevitView.theView, textBoxRenameView.Text);
                FinishEditViewName();
            }
        }

   

        private void UpdateLabelPreview(RevitView view)
        {
            if (chkPreview.Checked && (view.theView.ViewType == ViewType.Schedule))
            {
                //labelPreviewDisabled.Text = "No preview available";
                labelPreviewDisabled.Visible = false;
                labelPreviewIsSchedule.Visible = true;
            }
            else if (chkPreview.Checked)
            {
                //labelPreviewDisabled.Text = "Preview disabled";
                labelPreviewDisabled.Visible = labelPreviewIsSchedule.Visible = false;
            }
            else
            {
                //labelPreviewDisabled.Text = "Preview disabled";
                labelPreviewDisabled.Visible = true;
                labelPreviewIsSchedule.Visible = false;
            }
        }

       
        private void gridViews_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //RevitView v = m_view_filtered[e.RowIndex];
            //m_view = v.theView;
            if (e.RowIndex < 0)
                //user double-clicked column heading.
                // maybe can turn this into a column sort func
                return;
            else
            {
                DisposePreview();
            }

            m_ui_doc.ActiveView = m_views_sortable_filtered[gridViews.CurrentRow.Index].theView;

            SaveSettings();
            if (chkCloseAfterActivateView.Checked) this.Close();
            
        }

       

        private void buttonActivate_Click(object sender, EventArgs e)
        {
            //RevitView v = m_view_filtered[gridViews.CurrentRow.Index];
            //m_view = v.theView;

            DisposePreview();
            //m_ui_doc.ActiveView = m_views_sortable_filtered[gridViews.CurrentRow.Index].theView;

            MySortableBindingList<RevitView> b = (MySortableBindingList<RevitView>) gridViews.DataSource;
            m_ui_doc.ActiveView = b[gridViews.CurrentRow.Index].theView;
    
            SaveSettings();

            if (chkCloseAfterActivateView.Checked) this.Close();
            
        }

        

        private void buttonClearSearch_Click(object sender, EventArgs e)
        {
            this.comboBoxSearchTerms.Text = string.Empty;
            
            //m_views_sortable = new MySortableBindingList<RevitView>(m_views_all);
            SaveSettings();
            SetViewCountLabel();
            
        }

        private void checkBoxRememberLastSearch_MouseHover(object sender, EventArgs e)
        {
            m_toolTip.Show("Remember search term",this.checkBoxRememberLastSearch, 2000);
        }

        
        private void comboBoxSearchTerms_MouseHover(object sender, EventArgs e)
        {
            m_toolTip.Show("Enter part of a view name. e.g. SHEET", this.comboBoxSearchTerms, 2000);
        }

        // Column autosize settings
        //http://msdn.microsoft.com/en-us/library/74b2wakt.aspx
        private void checkBoxRememberLastSearch_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBoxRememberLastSearch.Checked)
            {
                ZGF.Revit.Properties.Settings.Default.ViewFinderLastSearch = string.Empty;
                ZGF.Revit.Properties.Settings.Default.Save();
            }
        }

        private void chkColDiscipline_CheckedChanged(object sender, EventArgs e)
        {
            colViewDiscipline.Visible = chkColDiscipline.Checked;
            //if (!comboBoxSearchTerms.Text.Equals(String.Empty))
                comboBoxSearchTerms_TextChanged(new object(), new EventArgs());
        }

        private void chkColViewTypeName_CheckedChanged(object sender, EventArgs e)
        {
            colViewType.Visible = chkColViewTypeName.Checked;
            //if (!comboBoxSearchTerms.Text.Equals(String.Empty))
                comboBoxSearchTerms_TextChanged(new object(), new EventArgs());
        }
        
        private void chkColPhaseName_CheckedChanged(object sender, EventArgs e)
        {
            colViewPhase.Visible = chkColPhaseName.Checked;
            //if (!comboBoxSearchTerms.Text.Equals(String.Empty))
                comboBoxSearchTerms_TextChanged(new object(), new EventArgs());
        }

        private void chkColReferencingSheet_CheckedChanged(object sender, EventArgs e)
        {
            colViewRefSheet.Visible = chkColReferencingSheet.Checked;
            string msg = string.Empty;
            m_ViewCollector.CollectSheetReferenceViews(ref msg);

            comboBoxSearchTerms_TextChanged(new object(), new EventArgs());
        }

        private void chkColTitleOnSheet_CheckedChanged(object sender, EventArgs e)
        {
            colViewTitleOnSheet.Visible = chkColTitleOnSheet.Checked;
            //if (!comboBoxSearchTerms.Text.Equals(String.Empty))
                comboBoxSearchTerms_TextChanged(new object(), new EventArgs());
        }

        private void chkColScale_CheckedChanged(object sender, EventArgs e)
        {
            colViewScale.Visible = chkColScale.Checked;
            //if (!comboBoxSearchTerms.Text.Equals(String.Empty))
                comboBoxSearchTerms_TextChanged(new object(), new EventArgs());
        }

        private void SaveSettings()
        {            
            // Search Settings
            ZGF.Revit.Properties.Settings.Default.ViewFinderRememberSearch = checkBoxRememberLastSearch.Checked;
            if (checkBoxRememberLastSearch.Checked)
            {
                ZGF.Revit.Properties.Settings.Default.ViewFinderLastSearch = comboBoxSearchTerms.Text;                
            }
            else
                ZGF.Revit.Properties.Settings.Default.ViewFinderLastSearch = string.Empty;

            if (comboBoxSearchTerms.Text != string.Empty)
                m_search_history.Add(comboBoxSearchTerms.Text, true);
            ZGF.Revit.Properties.Settings.Default.ViewFinderSearchHistory = m_search_history.AsStringCollection();

            // Property Columns            
            //ZGF.Revit.Properties.Settings.Default.ViewFinderLastSearch = this.textBoxSearchTerms.Text;
            //ZGF.Revit.Properties.Settings.Default.ViewFinderRememberSearch = checkBoxRememberLastSearch.Checked;
            ZGF.Revit.Properties.Settings.Default.ViewFinderColViewTypeName = chkColViewTypeName.Checked;
            ZGF.Revit.Properties.Settings.Default.ViewFinderColScale = chkColScale.Checked;
            ZGF.Revit.Properties.Settings.Default.ViewFinderColDiscipline = chkColDiscipline.Checked;
            ZGF.Revit.Properties.Settings.Default.ViewFinderColPhase = chkColPhaseName.Checked;
            //ZGF.Revit.Properties.Settings.Default.ViewFinderColRefSheet = chkColReferencingSheet.Checked;
            ZGF.Revit.Properties.Settings.Default.ViewFinderColTitleOnSheet = chkColTitleOnSheet.Checked;
            ZGF.Revit.Properties.Settings.Default.ViewFinderColScale = chkColScale.Checked;

            // Column Widthsties.Settings.Default.colViewCWidName = colViewName.Width;
            //ZGF.Revit.Properties.Settings.Default.colViewCWidDiscipline = colViewDiscipline.Width;
            //ZGF.Revit.Properties.Settings.Default.colViewCWidPhase = colViewPhase.Width;
            //ZGF.Revit.Properties.Settings.Default.colViewCWidRefSht = colViewRefSheet.Width;
            //ZGF.Revit.Properties.Settings.Default.colViewCWidScale = colViewScale.Width;
            //ZGF.Revit.Properties.Settings.Default.colViewCWidTitleOnSheet = colViewTitleOnSheet.Width;

            // Sorted Column
            ZGF.Revit.Properties.Settings.Default.ViewFinderSortedColumnIndex = null != gridViews.SortedColumn ? gridViews.SortedColumn.Index : 0;
            ZGF.Revit.Properties.Settings.Default.ViewFinderSortedColumnSortDirection = m_sortDirection; // == SortOrder.Ascending ? ListSortDirection.Ascending : ListSortDirection.Descending;
            // Size
            ZGF.Revit.Properties.Settings.Default.ViewFinderSize = this.Size;
            // Preview
            ZGF.Revit.Properties.Settings.Default.ViewFinderPreviewToggle = chkPreview.Checked;
            // Close ViewFinder after Activate view setting:
            ZGF.Revit.Properties.Settings.Default.ViewFinderCloseAfterActivate = chkCloseAfterActivateView.Checked;
            // Save
            ZGF.Revit.Properties.Settings.Default.Save();
            
        }


        private void gridViews_MouseHover(object sender, EventArgs e)
        {
            if (!gridViews.Focused) gridViews.Focus();
            // returns mouse position:
            System.Drawing.Point pt = this.PointToClient(System.Windows.Forms.Control.MousePosition);

            string metaKey = m_mode_key.ToString().ToUpper();
            m_toolTip.Show("Press SPACEBAR to activate selected view\nPress " + metaKey + " to display only OPEN views", this.comboBoxSearchTerms, pt, 3000);
        }



        // This speeds up the DataGridView's scrolling:
        public static void MakeGridViewDoublebuffered(DataGridView dgv)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv,true, null);
        }

       


        private void UpdatePreview(RevitView rv)
        {
            if ( null == rv ) return;
            // Refresh preview...
            
            DisposePreview();

            if (rv.theView.CanBePrinted)
            {
                using (PreviewControl pc = new PreviewControl(m_doc, rv.theView.Id))
                {   
                    this.elementHostThePreview.Child = pc;
                    pc.Dispose();
                }
            }           
        }

        private void DisposePreview()
        {
            // Deactivate Preview:
            PreviewControl pc = elementHostThePreview.Child as PreviewControl;
            if (null != pc)
            {
                pc.Dispose();
                elementHostThePreview.Child = null;
            }
            m_lastView = null;
        }

        private void chkPreview_CheckedChanged(object sender, EventArgs e)
        {
            labelPreviewDisabled.Visible = !chkPreview.Checked;
            if (!chkPreview.Checked)
            {
                DisposePreview();
                labelPreviewDisabled.Visible = true;
                labelPreviewIsSchedule.Visible = false;
            }
            else
            {
                if (m_views_sortable_filtered.Count > 0)
                {
                    UpdatePreview(m_views_sortable_filtered[gridViews.CurrentRow.Index]);
                    UpdateLabelPreview(m_views_sortable_filtered[gridViews.CurrentRow.Index]);
                }                
                labelPreviewDisabled.Visible = false;
            }
        }


        private void gridViews_SelectionChanged(object sender, EventArgs e)
        {
            RevitView currentView = null;

            if (m_views_sortable_filtered.Count > 0)
                currentView = m_views_sortable_filtered[gridViews.CurrentRow.Index] as RevitView;
                        
            if (m_lastView != currentView)
            {
                // MessageBox.Show(m_views_sortable[gridViews.CurrentRow.Index].Name);
                if (chkPreview.Checked && (null != currentView) )
                    UpdatePreview(currentView);

                m_lastView = currentView;
            }
        }

        private void gridViews_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //m_sortedColumn = e.ColumnIndex;
        }

        private void gridViews_Sorted(object sender, EventArgs e)
        {
            m_sortedColumn = gridViews.SortedColumn.Index;
            m_sortDirection = ConvertGridSortToListSort( gridViews.SortOrder );
            
            Debug.WriteLine("\nGridview.SortOrder = " + gridViews.SortOrder.ToString() + "\nC - Sort direction: " + m_sortDirection.ToString());
        }

        private ListSortDirection ConvertGridSortToListSort(SortOrder gridSortDirection)
        {
            //  0 = SortOrder.None
            //  1 = SortOrder.Ascending
            //  2 = SortOrder.Descending

            //  0 = ListSortDirection.Ascending
            //  1 = ListSortDirection.Descending
            
            switch (gridSortDirection)
            {
                case SortOrder.Ascending:
                    return ListSortDirection.Ascending;
                case SortOrder.Descending:
                    return ListSortDirection.Descending;
                default:
                    return m_sortDirection;
            }


        }
        
#region View List Context Menu

        // This variable is for sharing the currently select view between methods:
        private RevitView theSelectedRevitView;
        private int theCurrentRowIndex;

        private void gridViews_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            if (e.RowIndex < 0) return; // User right-clicked heading cell. Do nothing.

            tsmItemGoToSheet.Text = "Go to sheet";
            // Set the current Context Menu:
            e.ContextMenuStrip = contextMenuViewFinder;
            // Set right-clicked row current:
            gridViews.Rows[e.RowIndex].Selected = true;
            theCurrentRowIndex = e.RowIndex;
            // Get view referenced by current row:            
            theSelectedRevitView = m_views_sortable_filtered[e.RowIndex];
            
            bool viewIsOpen = false;            
            foreach (UIView openView in m_ui_doc.GetOpenUIViews())
            {
                if (theSelectedRevitView.theView.Id.Equals(openView.ViewId))
                {
                    viewIsOpen = true;
                    break;
                }
            }
            bool viewCanAddToSheet = theSelectedRevitView.CanAddToSheet;
            bool viewSelectedIsActive = theSelectedRevitView.theView.Id.Equals(m_doc.ActiveView.Id);
            bool viewCanBeDuplicated = theSelectedRevitView.theView.CanViewBeDuplicated(ViewDuplicateOption.Duplicate);
            bool viewCanBeDuplicatedWithDetailing = theSelectedRevitView.theView.CanViewBeDuplicated(ViewDuplicateOption.WithDetailing);
            bool viewCanBeDuplicatedAsDependent = theSelectedRevitView.theView.CanViewBeDuplicated(ViewDuplicateOption.AsDependent);

            bool viewIsWorksetAvailable = true;
            if (m_doc.IsWorkshared)
            {
                string editedBy = theSelectedRevitView.theView.get_Parameter(BuiltInParameter.EDITED_BY).AsString();
                if (!string.IsNullOrEmpty(editedBy))
                    viewIsWorksetAvailable = false;
            }

            //tsmItemOpen.Enabled = !viewIsOpen;
            tsmItemGoToSheet.Enabled = this.chkColReferencingSheet.Checked && !string.IsNullOrEmpty(theSelectedRevitView.ReferenceSheet);
            if (tsmItemGoToSheet.Enabled)
            {
                try
                {
                    string shtRef = theSelectedRevitView.ReferenceSheet; // TODO: What if ref sheets haven't been harvested?
                    if (!string.IsNullOrEmpty(shtRef))
                        tsmItemGoToSheet.Text = "Go to: " + shtRef.Split(new string[] { " - " }, StringSplitOptions.None)[0];                    
                }
                catch
                {
                    tsmItemGoToSheet.Text = "Go to sheet";
                }
            }
            
            tsmItemClose.Enabled = viewIsOpen;
            tsmItemDuplicate.Enabled = viewCanBeDuplicated;
            tsmItemDuplicateWithDetailing.Enabled = viewCanBeDuplicatedWithDetailing;
            tsmItemDuplicateAsDependent.Enabled = viewCanBeDuplicatedAsDependent;
            tsmItemRename.Enabled = tsmItemDelete.Enabled = viewIsWorksetAvailable;
            tsmItemAddToSheet.Enabled = viewCanAddToSheet;
            tsmItemDelete.Enabled = (viewIsWorksetAvailable && !viewSelectedIsActive);
        }

        

        private void tsmItemOpen_Click(object sender, EventArgs e)
        {
            m_ui_doc.ActiveView = theSelectedRevitView.theView;
            if (chkCloseAfterActivateView.Checked) this.Close();

            string msg = "Error collecting views after opening view from right-click menu";
            GetOpenViews(ref m_views_open, ref msg);

        }

       

        private void tsmItemGoToSheet_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(theSelectedRevitView.ReferenceSheet);

            string shtNumberNoDetail = theSelectedRevitView.ReferenceSheet.Split(new string[] { "/"," - " }, StringSplitOptions.None)[1];

            List<ViewSheet> sheets = new FilteredElementCollector(m_doc)
                .OfClass(typeof(ViewSheet))
                .Cast<ViewSheet>()
                .Where(n => n.SheetNumber.Equals(shtNumberNoDetail))
                .ToList<ViewSheet>();
                
            if (sheets.Count > 0)
            {
                Autodesk.Revit.DB.View viewToActivate = sheets[0] as Autodesk.Revit.DB.View;
                m_ui_doc.ActiveView = viewToActivate;
                if (chkCloseAfterActivateView.Checked) this.Close();
            }
                

        }

  
        private void tsmItemClose_Click(object sender, EventArgs e)
        {           
            IList<UIView> uiViews = m_ui_doc.GetOpenUIViews();
            UIView uiView = null;
            foreach (UIView uiV in uiViews)
            {
                if (uiV.ViewId.Equals(theSelectedRevitView.theView.Id))
                {
                    uiView = uiV;
                    break;
                }
            }
            if (null != uiView)
                uiView.Close();

            //if (checkBoxCloseAfterActivateView.Checked) this.Close();

            string msg = "Error collecting views after closing view from right-click menu";
            DisconectDataSourceBeforeModifyingViewList();
            GetOpenViews(ref m_views_open, ref msg);
            ReconnectDataAfterModifyViewlist();
            
        }

        private void tsmItemDuplicate_Click(object sender, EventArgs e)
        {

            DuplicateView(theSelectedRevitView, ViewDuplicateOption.Duplicate);
                        
        }

        private void DisconectDataSourceBeforeModifyingViewList()
        {
            gridViews.DataSource = null;
            m_ViewCollector = null;
            m_views_sortable_filtered.Clear();            
            m_views_all.Clear();
        }

        private void ReconnectDataAfterModifyViewlist()
        {            

            GetAllProjectViews();
            comboBoxSearchTerms_TextChanged(new object(), new EventArgs());
            string msg = "Error reconnecting to Datasource (001)";
            GetOpenViews(ref m_views_open, ref msg);

        }


        private void tsmItemDuplicateWithDetailing_Click(object sender, EventArgs e)
        {

            DuplicateView(theSelectedRevitView, ViewDuplicateOption.WithDetailing);
           
        }

        private void tsmItemDuplicateAsDependent_Click(object sender, EventArgs e)
        {

            DuplicateView(theSelectedRevitView, ViewDuplicateOption.AsDependent);
            
        }

        private bool DuplicateView(RevitView theView, ViewDuplicateOption viewDuplicateOption)
        {
            m_firstDisplayedRow = gridViews.FirstDisplayedScrollingRowIndex;

            DisconectDataSourceBeforeModifyingViewList();

            Autodesk.Revit.DB.View theDuplicateView = null;

            using (Transaction t = new Transaction(m_doc, "Duplicate view"))
            {
                t.Start();
            
                ElementId returnID = theView.theView.Duplicate(viewDuplicateOption) as ElementId;
                theDuplicateView = m_doc.GetElement(returnID) as Autodesk.Revit.DB.View;

                bool tryAgain = true;
                string suffix = string.Empty;

                do
                {
                    try
                    {
                        suffix += " - Copy";
                        // Rename View:
#if (REVIT_2019)
                        theDuplicateView.Name = theView.Name + suffix;
#else
                        theDuplicateView.ViewName = theView.Name + suffix;
#endif
                        t.Commit();
                        tryAgain = false;
                        break;
                    }
                    catch
                    {
                        //m_NumberOfTriesToDuplicate++;
                    }
                } while (tryAgain);  //(m_NumberOfTriesToDuplicate > 1);

            }
                        
            ReconnectDataAfterModifyViewlist();

            if (theCurrentRowIndex >= m_views_sortable_filtered.Count)
                theCurrentRowIndex--;

            // Now Rename View:
            if (null != theDuplicateView)
                theCurrentRowIndex = FindViewInGrid(theDuplicateView);

            if (theCurrentRowIndex > 0)  // && (theCurrentRowIndex < gridViews.RowCount - 2) )
            {
                theSelectedRevitView = m_views_sortable_filtered[theCurrentRowIndex]; // And get the new view
                bool isOnscr = gridViews.Rows[theCurrentRowIndex].Displayed;
                if (!isOnscr)
                    Debug.WriteLine(theView.Name);
                gridViews.Rows[theCurrentRowIndex].Selected = true; // Select it
                // scroll to selected row:
                //if (!gridViews.Rows[theCurrentRowIndex].Displayed)
                //{
                    gridViews.FirstDisplayedScrollingRowIndex = theCurrentRowIndex > 0 ? theCurrentRowIndex - 1 : theCurrentRowIndex;
                //}
                tsmItemRename_Click(new Object(), new EventArgs());
            }

            

            return true;
        }

        /// <summary>
        /// Searches for the RevitView object in the view list.
        /// </summary>
        /// <param name="rv"></param>
        /// <returns>Returns the row number. (-1 if not found)</returns>
        private int FindViewInGrid(Autodesk.Revit.DB.View viewToFind)
        {
            ElementId id = viewToFind.Id;
            for (int i = 0; i < m_views_sortable_filtered.Count; i++)
            {
                RevitView rv = m_views_sortable_filtered[i];
                if (rv.theView.Id.Equals(id))
                    return i;
            }
            return -1;
        }

        private void tsmItemDelete_Click(object sender, EventArgs e)
        {
            if (theSelectedRevitView.theView.Id.Equals(m_active_view.Id))
            {
                // try to delete active view:
                TaskDialog td = new TaskDialog("ZGF View Finder");
                td.TitleAutoPrefix = false;
                td.MainContent = "Cannot delete current view.";
                //td.MainInstruction = "Cannot delete view";
                td.Show();
                
                return;
            }

            m_firstDisplayedRow = gridViews.FirstDisplayedScrollingRowIndex;

            DisconectDataSourceBeforeModifyingViewList();
            
            try
            {
                using (Transaction t = new Transaction(m_doc, "Deleted view: " + theSelectedRevitView.Name))
                {
                    // Delete the View
                    t.Start();
                    ICollection<ElementId> viewCollection = new Collection<ElementId>();
                    viewCollection.Add(theSelectedRevitView.theView.Id);

                    theSelectedRevitView = null;                                        

                    m_doc.Delete(viewCollection);
                    t.Commit();
                }
            }
            catch 
            {                
                
                TaskDialog td = new TaskDialog("ZGF View Finder");
                td.TitleAutoPrefix = false;
                td.MainInstruction = "Cannot delete view";
                if ((m_doc.IsWorkshared) &&
                       (theSelectedRevitView.EditedBy.Equals(m_doc.Application.Username) || theSelectedRevitView.EditedBy.Equals(string.Empty))
                   )
                    td.MainContent = "The view is currently in use by " + theSelectedRevitView.EditedBy + ".";
                else
                    td.MainContent = "Cannot delete view.";
                
                td.Show();
            }
            finally
            {
                ReconnectDataAfterModifyViewlist();

                if (theCurrentRowIndex >= m_views_sortable_filtered.Count)
                    theCurrentRowIndex--;

                if (theCurrentRowIndex > 0)  // && (theCurrentRowIndex < gridViews.RowCount - 2) )
                {
                    theSelectedRevitView = m_views_sortable_filtered[theCurrentRowIndex]; // And get the new view
                    
                    gridViews.Rows[theCurrentRowIndex].Selected = true; // Select it
                    // scroll to selected row:
                    if (!gridViews.Rows[theCurrentRowIndex].Displayed)
                    {
                        gridViews.FirstDisplayedScrollingRowIndex = m_firstDisplayedRow; // (m_firstDisplayedRow < gridViews.RowCount) ? m_firstDisplayedRow : m_firstDisplayedRow - 1;
                    }                   
                }
            }            
        }

        

        private void tsmItemRename_Click(object sender, EventArgs e)
        {
            //disable form and gridview events...
            ViewRenameFreezeThawControls(RenameViewFocus.Freeze);

            DataGridViewCell cell = gridViews.Rows[theCurrentRowIndex].Cells[0];
            System.Drawing.Rectangle cellMap = gridViews.GetCellDisplayRectangle(0, theCurrentRowIndex, false);
            cellMap.Location = new System.Drawing.Point(gridViews.Left + 1, cellMap.Top + cellMap.Height / 2);

            textBoxRenameView.Bounds = cellMap;
            textBoxRenameView.Enabled = textBoxRenameView.Visible = true;
            this.ActiveControl = textBoxRenameView;

            textBoxRenameView.Text = theSelectedRevitView.Name;
            textBoxRenameView.Select(0, textBoxRenameView.Text.Length);           
        }

        /// <summary>
        /// Freezes View Browser until view has been renamed or canceled.
        /// F Freezes, T Thaws
        /// </summary>
        /// <param name="FreezeOrThaw"></param>
        private void ViewRenameFreezeThawControls(RenameViewFocus FreezeOrThaw)
        {                        
            // Controls
            //gridViews.Enabled = FreezeOrThaw == RenameViewFocus.Thaw;
            
            if (FreezeOrThaw == RenameViewFocus.Freeze)
            {

                // OK / Cancel Buttons
                this.AcceptButton = null;
                this.CancelButton = null;
            }
            else
            {
                textBoxRenameView.Visible = textBoxRenameView.Enabled = false;

                // OK / Cancel Buttons
                this.AcceptButton = buttonActivate;
                this.CancelButton = buttonClose;                           }
        }

        private enum RenameViewFocus
        {
            Freeze, Thaw
        }


        private void textBoxRenameView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Escape))
            {
                 FinishEditViewName();                 
                 textBoxRenameView_Leave(new Object(), new EventArgs());
            }
        }


        private void textBoxRenameView_KeyUp(object sender, KeyEventArgs e)
        {

            TaskDialog TdRenameView = new TaskDialog("ZGF View Finder");
                        
            TdRenameView.MainInstruction = "";
            TdRenameView.MainContent = "View names cannot contain any of the following characters:\n" + 
                "\\ ; { } [ ] | ; < > ? ` ~" + 
                "\nor any of the non-printable characters.";
          
            switch (e.KeyValue)
            {
                case 220: // \ or |
                case 186: // ; or :
                case 219: // [ or }
                case 221: // ] or }
                case 192: // ` or ~
                    Debug.WriteLine(e.KeyData);
                    TdRenameView.Show();
                    //string s1 = textBoxRenameView.Text;
                    return;
                case 188: // Shift + < 
                case 190: // Shift + > 
                case 191: // Shift + ?
                    if (e.Shift)
                    {
                        Debug.WriteLine(e.KeyData);
                        TdRenameView.Show();
                        //string s2 = textBoxRenameView.Text;                       
                    }
                    return;
            }

            switch (e.KeyCode)
            {
                case Keys.Tab:
                case Keys.Enter:
                    // Rename the view:
                    if (!RenameView(theSelectedRevitView.theView, textBoxRenameView.Text.Trim() ))
                    {
                        TdRenameView.MainContent = "\"" + textBoxRenameView.Text.Trim() + "\" cannot be used as a new view name. A view with that name may already exist.";
                        TdRenameView.Show();
                        textBoxRenameView.Text = theSelectedRevitView.Name;
                        textBoxRenameView.SelectAll();
                        textBoxRenameView.Focus();
                    }
                    else
                    {
                        FinishEditViewName();
                    }
                    break;
                case Keys.Escape:
                    // Bail
                    textBoxRenameView_Leave(new Object(), new EventArgs());
                    break;
            }
        }

        private void textBoxRenameView_Leave(object sender, EventArgs e)
        {
            //FinishEditViewName();
            textBoxRenameView.Focus();
        }

     
        private bool RenameView(Autodesk.Revit.DB.View theView, string NewViewName)
        {
#if (REVIT_2019)
            string tmpName = theView.Name;
#else
            string tmpName = theView.ViewName;
#endif
            if ( (tmpName.Equals(NewViewName, StringComparison.InvariantCultureIgnoreCase)) ||
                (String.IsNullOrWhiteSpace(NewViewName)) )
                return true; // Name hasn't changed. pass through
            
            // Try to rename:
            bool isSuccess = true;
            using (Transaction t = new Transaction(m_doc, "Rename View"))
            {
                try
                {
                    t.Start();
#if (REVIT_2019)
                    theView.Name = NewViewName;
#else
                    theView.ViewName = NewViewName;
#endif

                    t.Commit();
                }
                catch
                {
                    isSuccess = false;
                }
            } 

            return isSuccess;
        }

        private void FinishEditViewName()
        {
            textBoxRenameView.Clear();
            textBoxRenameView.Visible = false;
            ViewRenameFreezeThawControls(RenameViewFocus.Thaw);
        }


#endregion

        private void chkCloseAfterActivateView_CheckedChanged(object sender, EventArgs e)
        {
            //gridViews.MultiSelect = checkBoxMulti.Checked;
        }

        private void pictureBoxZgfLogo_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                chkCloseAfterActivateView.Visible = true;
            }
        }

    }

  
}
