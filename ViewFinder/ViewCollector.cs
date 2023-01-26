# define REVIT_2019
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ZGF.Revit
{


    class ViewCollector : IEnumerable<RevitView>
    {
        //----------------------------------------------------
        // P R I V A T E   M E M B E R S
        //----------------------------------------------------
        private Document m_doc;
        private UIDocument m_ui_doc;

        private int m_count;
        private List<RevitView> m_RevitViews = new List<RevitView>();

        private bool m_SheetReferenceViewsCollected = false;

        private System.Windows.Forms.DataGridView m_viewDataGrid;

        //----------------------------------------------------
        // C O N S T R U C T O R
        //----------------------------------------------------
        public ViewCollector(UIDocument currentRevitUIDoc, System.Windows.Forms.DataGridView theViewsDataGrid, ref string errorMessage)
        {
            m_ui_doc = currentRevitUIDoc;
            m_doc = currentRevitUIDoc.Document;

            m_viewDataGrid = theViewsDataGrid;


            FilteredElementCollector collector = new FilteredElementCollector(m_doc).OfClass(typeof(View));
            FilteredElementIterator iter = collector.GetElementIterator();
            while (iter.MoveNext())
            {
                View v = (View)iter.Current;

                ViewSheet sheet = null;
                bool viewIsUnwanted = false;

                // Viewtypes to filter out:                
                if (v.ViewType == ViewType.DrawingSheet)
                {
                    sheet = v as ViewSheet;

                    viewIsUnwanted = sheet.IsPlaceholder;
                    /*
                     Add to list of sheets, will use to collect sheet references for views later. 
                     (Legends will reference multiple views, so ignore.)
                    */
                }

                if (v.ViewType == ViewType.Schedule)
                {
                    ViewSchedule schedule = (ViewSchedule)v;
                    viewIsUnwanted = schedule.IsTitleblockRevisionSchedule; //<-Don't add Revision schedules.
                }

                switch (v.ViewType)
                {
                    case ViewType.ProjectBrowser:
                    case ViewType.SystemBrowser:
                        viewIsUnwanted = true;
                        break;
                    case ViewType.ThreeD:
                        // view created by workplane viewer. seems to be just a plain old 3D view. Can't find any
                        // distinguishing characteristics beyond viewname:
#if (REVIT_2019)
                        viewIsUnwanted = v.Name.StartsWith("Workplane Viewer", StringComparison.InvariantCultureIgnoreCase);
#else
                        viewIsUnwanted = v.ViewName.StartsWith("Workplane Viewer", StringComparison.InvariantCultureIgnoreCase);
#endif


                        break;
                }

                if ((!v.IsTemplate) && (v.ViewType != ViewType.Internal) && !viewIsUnwanted)
                {
                    try
                    {
                        RevitView theView = new RevitView(v, m_ui_doc, m_viewDataGrid, ref errorMessage);
                        m_RevitViews.Add(theView);

                    }
                    catch (System.Exception ex)
                    {
                        errorMessage = "Could not add view: " + v.Name + "\n\nStack Trace: \n\n" + ex.StackTrace;
                    }

                }
            }

            m_count = m_RevitViews.Count;
        }

        //----------------------------------------------------
        // M E T H O D S
        //----------------------------------------------------
        public RevitView[] ToArray()
        {
            return m_RevitViews.ToArray();
        }



        //----------------------------------------------------
        // P R O P E R T I E S
        //----------------------------------------------------
        public int Count { get { return m_count; } }

        //----------------------------------------------------
        // P R I V A T E   H E L P E R   M E T H O D S
        //----------------------------------------------------
        public IEnumerator<RevitView> GetEnumerator()
        {
            foreach (RevitView rv in m_RevitViews)
            {
                yield return rv;
            }
        }

        public void CollectSheetReferenceViews(ref string errorMessage)
        {
            // for views that have been placed on sheets, collect the referencing sheets.     
            if (!m_SheetReferenceViewsCollected)
            {
                foreach (RevitView rv in this)
                {
                    if (rv.theView.ViewType == ViewType.DrawingSheet)
                    {
                        ViewSheet sht = (ViewSheet)rv.theView;
                        View v = null;

                        try
                        {
                            // collect view referencing sheet                            
                            foreach (ElementId id in sht.GetAllViewports())
                            {
                                Viewport viewport = (Viewport)m_doc.GetElement(id);
                                v = (View)m_doc.GetElement(viewport.ViewId);
                                
                                // Same as PrimaryKey for Class 'RevitView'
                                RevitView revitView = this[m_doc.GetElement(v.GetTypeId()).Name + v.Name];

                                string detailNbrOnSheet;
                                if (null == revitView.DetailNumberOnSheet)
                                    detailNbrOnSheet = string.Empty;
                                else
                                    detailNbrOnSheet = revitView.DetailNumberOnSheet + "/";

                                if (
                                    (v.ViewType == ViewType.Legend) ||
                                    (v.ViewType == ViewType.Schedule)
                                   )
                                {
                                    detailNbrOnSheet = string.Empty;
                                    continue;
                                }



                                if (revitView.ReferenceSheet == null)
                                    revitView.ReferenceSheet = detailNbrOnSheet + sht.SheetNumber + " - " + sht.Name;
                                else
                                    revitView.ReferenceSheet += ", " + sht.SheetNumber + " - " + sht.Name;
                                //else
                                //revitView.ReferenceSheet = sht.SheetNumber + " - " + sht.Name;

                            }
                        }
                        catch (System.Exception ex)
                        {
                            errorMessage = "Sheet: " + sht.Name + "\n\nView: " + v.Name + "\n\nError: \n\n" + ex.Message + "\n\n" + ex.StackTrace;
                        }
                    }
                }
                m_SheetReferenceViewsCollected = true;
            }
        }


        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        // Indexer: Ordinal
        public RevitView this[int index]
        {
            get
            {
                if (index < 0 || index >= m_count)
                {
                    throw new System.IndexOutOfRangeException();
                }
                else
                {
                    return m_RevitViews[index];
                }
            }
        }

        // Indexer: String (View name)
        public RevitView this[string index]
        {
            get
            {
                RevitView returnObj = null;
                if (index.Length == 0)
                {
                    throw new System.ArgumentOutOfRangeException();
                }
                else
                {
                    returnObj = m_RevitViews.Single(item => item.PrimaryKey == index);
                }
                return returnObj;
            }
        }
    }


    [DataObject]
    class RevitView : IComparable
    {
        //----------------------------------------------------
        // P R I V A T E   M E M B E R S
        //----------------------------------------------------
        private UIDocument m_ui_doc;
        private Document m_doc;

        private View m_revit_view;

        private string m_search_term;

        private string m_ViewType;
        private string m_ViewDiscipline = null;
        //private string m_ViewLevel;  <--Too slow, too useless
        private string m_ViewPhase = null;
        private string m_ViewRefSheet = null;
        private string m_ViewNameOnSheet = null;
        private string m_ViewNumberOnsheet = null;
        private string m_ViewScale = null;

        private System.Windows.Forms.DataGridView m_viewDataGrid;

        string messageBackToCaller;

        //----------------------------------------------------
        // C O N S T R U C T O R
        //----------------------------------------------------
        public RevitView(View revitView, UIDocument RevitUIDocument, System.Windows.Forms.DataGridView theViewsDataGrid, ref string errorMessage)
        {
            m_ui_doc = RevitUIDocument;
            m_doc = m_ui_doc.Document;
            m_revit_view = revitView;

            // ViewType
            ElementType elType = m_doc.GetElement(theView.GetTypeId()) as ElementType;
            //Debug.Assert(null == elType, revitView.Name +  " :: elType is null");

            m_ViewType = null == elType ? string.Empty : elType.Name;

            if (revitView.ViewType == ViewType.ThreeD)
                ViewTypeSystemName = "3D";
            else
                ViewTypeSystemName = revitView.ViewType.ToString();

            // Level
            //try { m_ViewLevel = m_revit_view.GenLevel.Name; }
            //catch { m_ViewLevel = string.Empty; }

            try
            {
                Parameter p = m_revit_view.get_Parameter(BuiltInParameter.VIEWER_DETAIL_NUMBER) as Parameter;
                if (null != p)
                {
                    string nbr = p.AsString();
                    if (!string.IsNullOrEmpty(nbr))
                        m_ViewNumberOnsheet = nbr;
                }
            }
            catch { }

            // This is used for the search string. It allows searchTerm to use only visible columns
            m_viewDataGrid = theViewsDataGrid;

            messageBackToCaller = errorMessage;

            this.Selected = false; // For use in a grid with checkboxes
        }

        //----------------------------------------------------
        // M E T H O D S
        //----------------------------------------------------
        public void ActivateView()
        {
            m_ui_doc.ActiveView = m_revit_view;
        }

        public override string ToString()
        {
            return this.Name;
        }

        public void PlaceOnSheet() // TODO: Need to let user drag, or place in center of sheet
        {
            if (CanAddToSheet)
            {

                // Get the titleblock
                ViewSheet currentSheet = (ViewSheet)m_ui_doc.ActiveView;


                double sheetOriginU;
                double sheetOriginV;

                // this function deletes all the view ports then returns the true bottom left origin of the sheet.
                // if we don't delete the view ports and one of those view ports is outside sheet bounds it adds
                // to the total sheet area
                using (Transaction sheetTrans = new Transaction(m_doc, "Delete All VPs"))
                {
                    sheetTrans.Start();
                    foreach (ElementId eId in currentSheet.GetAllViewports())
                    {
                        m_doc.Delete(eId);
                    }

                    sheetOriginU = currentSheet.Outline.Min.U;
                    sheetOriginV = currentSheet.Outline.Min.V;

                    sheetTrans.Dispose();
                }


                // Try placing the view
                using (Transaction tr = new Transaction(m_doc, "Add view to sheet"))
                {
                    tr.Start();

                    try
                    {
                        if (IsScheduleView)
                            ScheduleSheetInstance.Create(m_doc, m_ui_doc.ActiveView.Id, m_revit_view.Id, XYZ.Zero);
                        else
                        {
                            XYZ viewPlacementPoint = new XYZ(sheetOriginU - m_revit_view.Outline.Min.U, sheetOriginV - m_revit_view.Outline.Min.V, 0);
                            // Create viewport at origin of sheet view:
                            Viewport viewport = Viewport.Create(m_doc, m_ui_doc.ActiveView.Id, m_revit_view.Id, XYZ.Zero);
                            // Get center of view                                                         
                            if (null != viewport)
                                ElementTransformUtils.MoveElement(m_doc, viewport.Id, viewPlacementPoint);
                        }

                        tr.Commit();
                    }
                    catch (Exception ex)
                    {
                        tr.RollBack();
                        messageBackToCaller = ex.Message;
                    }


                }

            }
            else
                TaskDialog.Show("Place view on sheet", this.ViewTypeName + " \"" + this.Name + "\" has already been placed on a sheet.");

        }

        //----------------------------------------------------
        // P R O P E R T I E S
        //----------------------------------------------------
        public View theView { get { return m_revit_view; } }
        /// <summary>
        /// this is used because name can be same for floor and ceiling plan views:
        /// </summary>
        public string PrimaryKey { get { return this.ViewTypeName + this.Name; } }

        public string ViewTypeName
        {
            get
            {
                return m_ViewType;
            }
        }
        public string Name { get { return SheetNumber + m_revit_view.Name; } }

        public string ViewTypeSystemName { get; private set; }

        public string ViewCategoryNode { get; set; }
        public string Discipline
        {
            get
            {
                if (null == m_ViewDiscipline)
                {
                    // Discipline
                    try
                    {
                        m_ViewDiscipline = Enum.GetName(typeof(ViewDiscipline), theView.Discipline);
                    }
                    catch
                    {
                        m_ViewDiscipline = string.Empty;
                    }
                }


                return m_ViewDiscipline;
            }

        }
        public string EditedBy
        {
            get
            {
                string returnVal = string.Empty;
                if (m_doc.IsWorkshared)
                {
                    returnVal = m_revit_view.get_Parameter(BuiltInParameter.EDITED_BY).AsString(); // Could this Parameter possibly not exist?
                }

                return returnVal;
            }
        }
        //public string DetailLevel { get; set; }
        public bool IsViewTemplate { get { return m_revit_view.IsTemplate; } }
        public bool IsScheduleView { get { return m_revit_view.ViewType == ViewType.Schedule; } }

        public string Phase
        {
            get
            {
                if (null == m_ViewPhase)
                {
                    // Phase
                    try
                    {
                        Parameter p = m_revit_view.get_Parameter(BuiltInParameter.VIEW_PHASE);
                        Phase phase = (Phase)m_doc.GetElement(p.AsElementId());

                        m_ViewPhase = phase.Name;
                    }
                    catch
                    {
                        m_ViewPhase = string.Empty;
                    }
                }

                return m_ViewPhase;
            }
        }

        public string ReferenceSheet
        {
            get
            { return m_ViewRefSheet; }
            set
            { m_ViewRefSheet = value; }
        }

        public string SheetNumber
        {
            get
            {
                if (m_revit_view.ViewType == ViewType.DrawingSheet)
                {
                    ViewSheet sht = (ViewSheet)m_revit_view;
                    return sht.SheetNumber + " - ";
                }
                else
                    return string.Empty;
            }
        }

        public string NameOnSheet
        {
            get
            {
                if (null == m_ViewNameOnSheet)
                {
                    // Name on Sheet
                    try
                    {
                        Parameter p = m_revit_view.get_Parameter(BuiltInParameter.VIEW_DESCRIPTION);
                        m_ViewNameOnSheet = p.AsString();
                    }
                    catch
                    {
                        m_ViewNameOnSheet = string.Empty;
                    }
                }

                return m_ViewNameOnSheet;
            }
            set
            {
                Parameter p = m_revit_view.get_Parameter(BuiltInParameter.VIEW_DESCRIPTION) as Parameter;
                if (null != p)
                    p.Set(value);
            }
        }

        public string DetailNumberOnSheet
        {
            get
            {

                return m_ViewNumberOnsheet;

            }
        }

        public string Scale
        {
            get
            {
                if (null == m_ViewScale)
                {
                    try
                    {
                        int scale = theView.Scale;
                        switch (theView.ViewType)
                        {
                            case ViewType.ColumnSchedule:
                            case ViewType.CostReport:
                            case ViewType.DrawingSheet:
                            case ViewType.Internal:
                            case ViewType.LoadsReport:
                            case ViewType.PanelSchedule:
                            case ViewType.PresureLossReport:
                            case ViewType.Rendering:
                            case ViewType.Report:
                            case ViewType.Schedule:
                            case ViewType.ThreeD:
                            case ViewType.Undefined:
                            case ViewType.Walkthrough:
                            case ViewType.ProjectBrowser:
                            case ViewType.SystemBrowser:
                                m_ViewScale = string.Empty;
                                break;
                            default:
                                m_ViewScale = ZGF.Revit.Strings.ScaleToString.GetScaleString(theView.Scale);
                                break;
                        }
                    }
                    catch
                    {
                        m_ViewScale = string.Empty;
                    }
                }
                return m_ViewScale;
            }
        }

        public bool CanAddToSheet
        {
            get
            {
                if (this.ViewTypeName == "Schedule")
                    return true;
                else
                    return Viewport.CanAddViewToSheet(m_doc, m_ui_doc.ActiveView.Id, m_revit_view.Id);
            }
        }

        public bool Selected { get; set; }

        //----------------------------------------------------
        // P R I V A T E   H E L P E R   M E T H O D S
        //----------------------------------------------------

        public string SearchTerm
        {
            get
            {
                StringBuilder sb = new StringBuilder(Name + " " + this.ViewTypeSystemName);
                if (m_viewDataGrid.Columns["colViewType"].Visible) sb.Append(this.ViewTypeName);
                if ((null != m_ViewDiscipline) && (m_viewDataGrid.Columns["colViewDiscipline"].Visible)) sb.Append(m_ViewDiscipline);
                if ((null != m_ViewNameOnSheet) && (m_viewDataGrid.Columns["colViewTitleOnSheet"].Visible)) sb.Append(m_ViewNameOnSheet);
                if ((null != m_ViewPhase) && (m_viewDataGrid.Columns["colViewPhase"].Visible)) sb.Append(m_ViewPhase);
                if ((null != m_ViewRefSheet) && (m_viewDataGrid.Columns["colViewRefSheet"].Visible)) sb.Append(m_ViewRefSheet);
                if ((null != m_ViewScale) && (m_viewDataGrid.Columns["colViewScale"].Visible)) sb.Append(m_ViewScale);

                m_search_term = sb.ToString();

                return m_search_term;
            }

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


        public int CompareTo(object obj)
        {

            RevitView item = (RevitView)obj;
            string s1 = item.ViewTypeName + item.Name;
            string s2 = this.ViewTypeName + this.Name;
            return string.Compare(s2, s1);

        }
    }
}
