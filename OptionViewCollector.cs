using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ZGF.Revit
{

    class OptionViewCollector : IEnumerable<RevitView>
    {
        //----------------------------------------------------
        // P R I V A T E   M E M B E R S
        //----------------------------------------------------
        private Document m_doc;
        private UIDocument m_ui_doc;

        private int m_count;
        private List<RevitView> m_RevitViews = new List<RevitView>();
                
        private System.Windows.Forms.DataGridView m_viewDataGrid;

        //----------------------------------------------------
        // C O N S T R U C T O R
        //----------------------------------------------------
        public OptionViewCollector(ExternalCommandData commandData, System.Windows.Forms.DataGridView theViewsDataGrid, ref string errorMessage)
        {
            m_ui_doc = commandData.Application.ActiveUIDocument;
            m_doc = m_ui_doc.Document;

            m_viewDataGrid = theViewsDataGrid;

            FilteredElementCollector collector = new FilteredElementCollector(m_doc).OfClass(typeof(View));
            FilteredElementIterator iter = collector.GetElementIterator();
            while (iter.MoveNext())
            {
                View v = (View)iter.Current;
                
                bool viewIsUnwanted = false;

                // ViewTypes to filter out:
                switch (v.ViewType)
                {
                    //case ViewType.AreaPlan:
                    case ViewType.DraftingView:
                    case ViewType.DrawingSheet:
                    case ViewType.Internal:
                    case ViewType.Legend:
                    case ViewType.Rendering:                        
                    case ViewType.ProjectBrowser:
                    case ViewType.SystemBrowser:
                    case ViewType.Walkthrough:
                    case ViewType.Undefined:                        
                        viewIsUnwanted = true;
                        break;
                    default:
                    {
                        if (v.ViewType == ViewType.Schedule)
                        {
                            ViewSchedule vs = (ViewSchedule)v;
                            if (vs.IsInternalKeynoteSchedule || vs.IsTitleblockRevisionSchedule)
                                viewIsUnwanted = true;
                        }
                        if (!v.IsTemplate && !viewIsUnwanted)
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
                        break;
                }
            }

            m_count = m_RevitViews.Count;

            // Collect referencing sheet Views

            //m_RevitViews.Sort(); //Doesn't work...?
        }

        //----------------------------------------------------
        // M E T H O D S
        //----------------------------------------------------
        public RevitView[] ToArray()
        {
            return m_RevitViews.ToArray();
        }

        
        /// <summary>
        /// Test func from Harry Mattison
        /// http://boostyourbim.wordpress.com/2013/12/03/au2013-wish-2-granted/
        /// </summary>
        /// <param name="commandData"></param>
        public static void TestOptionViewThingy(ExternalCommandData commandData)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;    // this.ActiveUIDocument.Document;
            UIDocument uidoc = commandData.Application.ActiveUIDocument;         //this.ActiveUIDocument;
            IEnumerable<DesignOption> designOptions = new FilteredElementCollector(doc).OfClass(typeof(DesignOption)).Cast<DesignOption>().Where(q => !q.IsPrimary);
            if (designOptions.Count() == 1)
            {
                TaskDialog.Show("Error","Only one design option exists. No views duplicated.");
                return;
            }

            IList<ViewSheet> newSheets = new List<ViewSheet>();

            IList<ViewSheet> viewSheets = new FilteredElementCollector(doc).OfClass(typeof(ViewSheet)).Cast<ViewSheet>().ToList();

            using (Transaction t = new Transaction(doc,"Duplicate Views by Option"))
            {
                t.Start();
                foreach (ViewSheet vs in viewSheets)
                {
                    IEnumerable<Viewport> viewportsOnSheet = 
                        new FilteredElementCollector(doc).OfClass(typeof(Viewport)).Cast<Viewport>().Where(q => q.SheetId == vs.Id);
                    if (viewportsOnSheet.Count() == 0)
                        continue;

                    FamilyInstance titleblock = new FilteredElementCollector(doc).OfClass(typeof(FamilyInstance)).OfCategory(BuiltInCategory.OST_TitleBlocks)
                        .Cast<FamilyInstance>().First(q => q.OwnerViewId == vs.Id);

                    foreach (DesignOption dOpt in designOptions)
                    {
                        ViewSheet newSheet = ViewSheet.Create(doc, titleblock.GetTypeId());
                        newSheet.Name = vs.Name + " - " + dOpt.Name;
                        newSheets.Add(newSheet);
                        foreach (Viewport vp in viewportsOnSheet)
                        {
                            View view = doc.GetElement(vp.ViewId) as View;
                            XYZ vpCenter = vp.GetBoxCenter();
                            View newView = doc.GetElement(view.Duplicate(ViewDuplicateOption.WithDetailing)) as View;
                            newView.Name = view.Name.Replace("{","").Replace("}","") + " - " + dOpt.Name;
                            newView.get_Parameter(BuiltInParameter.VIEWER_OPTION_VISIBILITY).Set(dOpt.Id); // "Visible In Option").Set(dOpt.Id);
                            Viewport newVp = Viewport.Create(doc, newSheet.Id, newView.Id, vpCenter);
                        }
                    }
                }
                t.Commit();
            }

            foreach(ViewSheet v in newSheets)
            {
                uidoc.ActiveView = v;
            }
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

}
