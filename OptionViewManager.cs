using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ZGF.Revit
{
    public partial class OptionViewManager : System.Windows.Forms.Form
    {
        private List<RevitView> m_views_all = new List<RevitView>();
        
        private MySortableBindingList<RevitView> m_views_sortable;
        private List<RevitView> m_SelectedViews = new List<RevitView>();
        private List<OptionSetWrapper> m_SelectedOptionSets = new List<OptionSetWrapper>();

        private Dictionary<string, OptionSetWrapper> m_DesignOptionSets = new Dictionary<string, OptionSetWrapper>();
        
        private UIDocument m_ui_doc;
        private Document m_doc;

        private TreeNode m_treeView_RootNode = new TreeNode("Design Option Sets");
        
        private Autodesk.Revit.DB.View m_active_view;

        // Tooltip for hoverhelp
        private ToolTip m_toolTip = new ToolTip();

        OptionViewCollector m_ViewCollector;

        //  C O N S T R U C T O R
        public OptionViewManager(ExternalCommandData commandData, ref string errorMessage)
        {
            InitializeComponent();

            treeViewPreview.Nodes.Add(m_treeView_RootNode);

            this.Text += " (v" + Assembly.GetExecutingAssembly().GetName().Version.Major + "." +
                Assembly.GetExecutingAssembly().GetName().Version.Minor +
                Assembly.GetExecutingAssembly().GetName().Version.Build + ")";
                        
            MakeGridViewDoublebuffered(gridViews);
            
            m_toolTip.UseFading = true;
                        
            this.gridViews.AutoGenerateColumns = false;
            m_ui_doc = commandData.Application.ActiveUIDocument;
            m_doc = commandData.Application.ActiveUIDocument.Document;
            m_active_view = m_ui_doc.ActiveView;

            // Center Checkbox columns:
           

            #region ROW FORMAT
            
            DataGridViewCellStyle altRowStyle = new DataGridViewCellStyle(gridViews.DefaultCellStyle);            
            altRowStyle.BackColor = System.Drawing.Color.FromArgb(231, 234, 239);
            
            gridViews.AlternatingRowsDefaultCellStyle = altRowStyle;
            
            #endregion


            // COLLECT VIEWS
            m_ViewCollector = new OptionViewCollector(commandData,this.gridViews, ref errorMessage);
            
            m_views_all.AddRange(m_ViewCollector.ToArray());            
            
            // Sortable list:
            m_views_sortable = new MySortableBindingList<RevitView>(m_views_all);
            gridViews.DataSource = m_views_sortable;    //m_views_all;

                       
            //if (textBoxSearchTerms.Text.Length > 0) textBoxSearchTerms.SelectAll();
            
            // Column AutoSize settings:
            // http://msdn.microsoft.com/en-us/library/74b2wakt.aspx
            //foreach (DataGridViewColumn c in gridViews.Columns)
            //{
            //    c.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //}
            colViewName.FillWeight = 200;
            

            

            // Which col to sort by?
            gridViews.Sort(colViewName, ListSortDirection.Ascending);
            
            
            //if (gridViews.RowCount > 0)
            //{
            //    RevitView view = m_views_sortable[0];
               
            //}

            SetViewCountLabel();


            // Collect Option Sets
            // Collect all design options

            List<DesignOption> designOptions = new List<DesignOption>(
                new FilteredElementCollector(m_doc)
                .OfCategory(BuiltInCategory.OST_DesignOptions)
                .Cast<DesignOption>()
                .ToList<DesignOption>()                
                );


            ICollection<ElementId> ids = new FilteredElementCollector(m_doc).OfCategory(BuiltInCategory.OST_DesignOptionSets).ToElementIds();
            foreach (ElementId id in ids)
            {
                OptionSetWrapper optionSet = new OptionSetWrapper(m_doc.GetElement(id), designOptions);

                m_DesignOptionSets.Add(optionSet.Name, optionSet);
                checkedListBoxOptionSets.Items.Add(optionSet);
            }

            this.ProjectHasOptions = m_DesignOptionSets.Count > 0;
            if (!this.ProjectHasOptions)
            {
                TaskDialog td = new TaskDialog(this.Text);
                td.TitleAutoPrefix = false;
                td.MainInstruction = "Project has no options";
                td.MainContent = "Closing...";
                td.Show();
                return;
            }
            
            checkedListBoxOptionSets.Sorted = true;

            //  If no option sets
            //   show dialog and bail
            //  else
            //    Collect options and store w/ parent set
            //
            // Populate Option Sets List

        }

        /// <summary>
        /// Updates View count message in dialog
        /// </summary>
        private void SetViewCountLabel()
        {
            labelViewCount.Text = m_views_sortable.Count + " of " + m_views_all.Count.ToString() + " Views";
        }

        


        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.SaveSettings();
            this.Close();
        }

       


        private void buttonOK_Click(object sender, EventArgs e)
        {            
            //m_ui_doc.ActiveView = m_views_sortable[gridViews.CurrentRow.Index].theView;
            //Parameter p = m_ui_doc.ActiveView.get_Parameter(BuiltInParameter.VIS_GRAPHICS_DESIGNOPTIONS);
            Parameter ppp = m_ui_doc.ActiveView.get_Parameter(BuiltInParameter.VIEWER_OPTION_VISIBILITY);
            //p = m_ui_doc.ActiveView.get_Parameter(BuiltInParameter.VIEW_VISIBLE_CATEGORIES);

            using (Transaction trans = new Transaction(m_doc, "Create duplicate option views"))
            {
                if ( (m_SelectedViews.Count + m_SelectedOptionSets.Count) > 0)
                {
                    trans.Start();
                    foreach (OptionSetWrapper osw in m_SelectedOptionSets)
                    {
                        foreach (DesignOption opt in osw.Options)
                        {
                            foreach (RevitView rv in m_SelectedViews)
                            {
                                ElementId newViewID;

                                if (rv.theView.CanBePrinted)
                                    newViewID = rv.theView.Duplicate(ViewDuplicateOption.WithDetailing) as ElementId;
                                else
                                    newViewID = rv.theView.Duplicate(ViewDuplicateOption.Duplicate) as ElementId; // e.g., SCHEDULE

                                if (null != newViewID)
                                {
                                    string newViewName = rv.Name + " - " + osw.Name + " - " + opt.Name;
                                    Autodesk.Revit.DB.View newView = (Autodesk.Revit.DB.View)m_doc.GetElement(newViewID);
                                    try 
                                    { 
                                        // Set Duplicated View's name:
                                        newView.Name = newViewName; 
                                        // Set the View's Visible in Option Property:
                                        Parameter p = newView.get_Parameter(BuiltInParameter.VIEWER_OPTION_VISIBILITY) as Parameter;   //("Visible In Option") as Parameter;
                                        // Some views don't have "visible in option" property (e.g.: Schedules)
                                        if (null != p)
                                        {
                                            string s = p.Definition.Name + " = " + p.AsValueString();
                                            p.Set(opt.Id);
                                            //p.Set(new ElementId(BuiltInParameter.INVALID)); // Sets value of 'Visible in Option' back to "All", but leaves Vis Graphics override in place
                                        }
                                    }
                                    catch { m_doc.Delete(newViewID); }
                                }
                            }
                        }
                    }
                    trans.Commit();
                }
            }
           
            SaveSettings();
            this.Close();
            
        }

        

        private void buttonClearSearch_Click(object sender, EventArgs e)
        {
            this.textBoxSearchTerms.Clear();
           
            m_views_sortable = new MySortableBindingList<RevitView>(m_views_all);

            SaveSettings();
            SetViewCountLabel();
        }

        private void textBoxSearchTerms_TextChanged(object sender, EventArgs e)
        {
            string words = this.textBoxSearchTerms.Text;            
            gridViews.DataSource = null;           
            m_views_sortable = new MySortableBindingList<RevitView>(m_views_all.FindAll(item => item.IsMatch(words)));
            gridViews.DataSource = m_views_sortable;
            
            SetViewCountLabel();            
        }

       

        private void textBoxSearchTerms_MouseHover(object sender, EventArgs e)
        {
            m_toolTip.Show("Enter part of a view name. e.g. FLOOR PLAN", this.textBoxSearchTerms, 2000);
        }

       
        

        private void SaveSettings()
        {   
            
            // Save
            ZGF.Revit.Properties.Settings.Default.Save();
            
        }

        

        private void gridViews_MouseHover(object sender, EventArgs e)
        {
            // returns mouse position:
            System.Drawing.Point pt = this.PointToClient(System.Windows.Forms.Control.MousePosition);

            int X = 50; // gridViews.Left + (gridViews.Width / 2);
            int Y = -60; // gridViews.Top + (gridViews.Height / 2);

            pt = new System.Drawing.Point(X, Y);

            m_toolTip.Show("Select views to duplicate", this.textBoxSearchTerms, pt, 2000);

        }



        // This speeds up the DataGridView's scrolling:
        public static void MakeGridViewDoublebuffered(DataGridView dgv)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv,true, null);
        }


        public bool ProjectHasOptions { get; private set; }

        private void checkedListBoxOptionSets_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            OptionSetWrapper w = checkedListBoxOptionSets.Items[e.Index] as OptionSetWrapper;

            if (e.NewValue == CheckState.Checked)
                m_SelectedOptionSets.Add(w);
            else
                m_SelectedOptionSets.Remove(w);

            RefreshOptionViewsPreview();
            
        }

        
        private void gridViews_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex > 0) return; // skip if user picks a cell other than checkbox in first column
            
            DataGridViewCheckBoxCell chk = gridViews.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewCheckBoxCell;
            RevitView cv = m_views_sortable[e.RowIndex];
            bool isChecked = (bool) chk.EditedFormattedValue;
            if (isChecked)
                { if (!m_SelectedViews.Contains(cv)) m_SelectedViews.Add(cv); }
            else
                { if (m_SelectedViews.Contains(cv)) m_SelectedViews.Remove(cv); }

            RefreshOptionViewsPreview();
            
        }

        private void RefreshOptionViewsPreview()
        {
            Font boldFont = new Font(treeViewPreview.Font, FontStyle.Bold);            

            m_treeView_RootNode.Nodes.Clear();
            foreach (OptionSetWrapper optionSet in m_SelectedOptionSets)
            {
                TreeNode optionSetNode = new TreeNode(optionSet.Name);
                List<TreeNode> optionNodes = new List<TreeNode>();

                foreach (DesignOption opt in optionSet.Options2.Values)
                {
                    TreeNode optNode = new TreeNode(opt.Name);                    
                    // Duplicated view names...
                    if (m_SelectedViews.Count > 0)
                    {
                        //List<TreeNode> viewNodes = new List<TreeNode>();
                        foreach (RevitView rv in m_SelectedViews)
                        {                            //  sub-node = view_name + Option Set + Option
                            TreeNode tn = new TreeNode(rv.Name + ", " + optionSet.Name + ", " + opt.Name);
                            //rv.NameOnSheet += " " + optionSet.Name.ToUpper() + " " + opt.Name.ToUpper();
                            tn.NodeFont = boldFont;
                            optNode.Nodes.Add(tn);
                        }
                    }
                    optionSetNode.Nodes.Add(optNode);
                }
                m_treeView_RootNode.Nodes.Add(optionSetNode);
            }
            treeViewPreview.ExpandAll();
        }
    }


    class OptionSetWrapper
    {
        public OptionSetWrapper(Element theOptionSet, List<DesignOption> ProjectDesignOptions)
        {
            TheDesignOptionSet = theOptionSet;
            Name = TheDesignOptionSet.Name;
            foreach (DesignOption opt in ProjectDesignOptions)
            {
                Parameter OptSetID = opt.get_Parameter(BuiltInParameter.OPTION_SET_ID);
                
                if (OptSetID.AsElementId().Equals(theOptionSet.Id))
                {
                    if (null == Options) Options = new List<DesignOption>();
                    Options.Add(opt);
                    if (null == Options2) Options2 = new SortedList<string, DesignOption>();
                    Options2.Add(opt.Name, opt);
                    OptionCount++;
                }
            }
            
        }
        public Element TheDesignOptionSet { get; private set; }
        public string Name { get; private set; }
        public int OptionCount { get; private set; }
        public List<DesignOption> Options { get; private set; }
        public SortedList<string,DesignOption> Options2 { get; private set; }
        public override string ToString()
        {
            return Name;
        }
    }


 
}
