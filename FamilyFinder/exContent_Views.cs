using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
	

namespace ZGF.Revit
{
    class exContent_Views
    {

        public static void InsertDraftingView(UIDocument revitTargetUiDoc, Revit_Content_EXTERNAL_Item externalDataRecord)
        {
            Document _doc = revitTargetUiDoc.Document;
            // Should we have a return type?
            string detailViewName = externalDataRecord.FileName;

            // Get Container file:
            FileInfo _sourceFile = new FileInfo(externalDataRecord.FullPathName);

            // Try to get the view in the local Target doc. If it exists. just activate it:
            ViewDrafting sourceDraftingView = GetDraftingViewFromDoc(_doc, detailViewName);

            // does the view already exist in the model?
            if (null == sourceDraftingView)
            {
                // No. Go get it from the Library Doc
                if (_sourceFile.Exists)
                {
                    Document sourceDoc = _doc.Application.OpenDocumentFile(_sourceFile.FullName);

                    try
                    {
	                    sourceDraftingView = GetDraftingViewFromDoc(sourceDoc, detailViewName);
	                    // We need a list for the view utility. Add the first view:
	                    Dictionary<string, ViewDrafting> referencedViews = new Dictionary<string, ViewDrafting>();
	                    referencedViews.Add(detailViewName, sourceDraftingView);
	
	                    // Search view(s) for referenced views and add those views too:
	                    //IEnumerable<ViewDrafting> viewsToImport = new List<ViewDrafting>() { sourceDraftingView };
	
	                    GetReferencedViews(sourceDoc, sourceDraftingView, ref referencedViews);
	
	                    // From Autodesk Sample Apps:
	                    int _viewsImportedCount = DuplicateViewUtilities.DuplicateViewUtils.DuplicateDraftingViews(
	                        sourceDoc, referencedViews.Values, _doc);
                    }                    
                    finally
                    {
                        sourceDoc.Close(false);
                    }
                   
                }
            }

            // Activate the Imported or found Drafting view:
            ViewDrafting importedView = GetDraftingViewFromDoc(_doc, detailViewName);
            if (null != importedView) revitTargetUiDoc.ActiveView = importedView;
        }

        private static void GetReferencedViews(Document doc, ViewDrafting view, ref Dictionary<string, ViewDrafting> capturedViews)
        {
            int viewCount = 0;

            List<Element> viewers = new FilteredElementCollector(doc, view.Id)
                .WherePasses(new ElementCategoryFilter(BuiltInCategory.OST_Viewers))
                .ToElements()
                .ToList<Element>();
                      
            foreach (Element v in viewers)
            {
                // View Tags (Elevations, sections, etc.)
                ViewDrafting dv = doc.GetElement(v.OwnerViewId) as ViewDrafting;
                if ((null != dv) && (!capturedViews.ContainsKey(dv.Name)))
                {
                    capturedViews.Add(dv.Name, dv);
                    viewCount++;
                    GetReferencedViews(doc, dv, ref capturedViews);
                }
            }

            List<Element> referenceViewTags = new FilteredElementCollector(doc, view.Id)
                .WherePasses(new ElementCategoryFilter(BuiltInCategory.OST_ReferenceViewer))
                .ToElements()
                .ToList<Element>();

            foreach (Element r in referenceViewTags)
            {
                ViewDrafting v = doc.GetElement(r.get_Parameter(BuiltInParameter.REFERENCE_VIEWER_TARGET_VIEW).AsElementId()) as ViewDrafting;
                if ((null != v) && (!capturedViews.ContainsKey(v.Name)))
                {
                    capturedViews.Add(v.Name, v);
                    viewCount++;
                    GetReferencedViews(doc, v, ref capturedViews);
                }
            }

            if (viewCount < 1) return;
           
        }

        public static void _InsertDraftingView(UIDocument revitTargetUiDoc, Revit_Content_EXTERNAL_Item externalDataRecord)
        {
            Document _doc = revitTargetUiDoc.Document;
            // Should we have a return type?
            string detailViewName = externalDataRecord.FileName;

            // Get Container file:
            FileInfo _sourceFile = new FileInfo(externalDataRecord.FullPathName);
            
            // Try to get the view in the Target doc. If it exists. just activate it:
            ViewDrafting sourceDraftingView = GetDraftingViewFromDoc(_doc, detailViewName);
            
            // does the view already exist in the model?
            if (null == sourceDraftingView)                
            {
                // No. Go get it from the Library Doc
                if (_sourceFile.Exists)
                {
                    Document sourceDoc = _doc.Application.OpenDocumentFile(_sourceFile.FullName);

                    CopyPasteOptions options = new CopyPasteOptions();
                    options.SetDuplicateTypeNamesHandler(new HideAndAcceptDuplicateTypeNamesHandler());

                    sourceDraftingView = GetDraftingViewFromDoc(sourceDoc, detailViewName);
                    
                    using (Transaction targetTrans = new Transaction(_doc, "Import View: " + detailViewName))
                    {
                        try
                        {
                            targetTrans.Start();

                            // Import the Drafting View:
                            ICollection<ElementId> draftingViewToImport = new List<ElementId>() { sourceDraftingView.Id };
                            ICollection<ElementId> x = ElementTransformUtils.CopyElements(sourceDoc, draftingViewToImport, _doc, null, options);   // (sourceDoc, idsToCopy, _doc, Transform.Identity, options);
                            ElementId newDraftingViewId = x.First<ElementId>();
                            ViewDrafting targetView = _doc.GetElement(newDraftingViewId) as ViewDrafting;

                            // Now, get the detail elements from the view:
                            ElementCategoryFilter invalidItems = new ElementCategoryFilter(ElementId.InvalidElementId, true);
                            ICollection<ElementId> idsOfElementsToCopy = new FilteredElementCollector(sourceDoc, sourceDraftingView.Id)
                                .WherePasses(invalidItems)
                                .ToElementIds();

                            Debug.WriteLine("element count before: " + idsOfElementsToCopy.Count.ToString());

                            //var ids = idsOfElementsToCopy.Distinct();

                            Element el = sourceDoc.GetElement(new ElementId(4478));

                            //ElementCategoryFilter viewRefs = new ElementCategoryFilter(BuiltInCategory.OST_ReferenceViewer);
                            //ElementCategoryFilter viewers = new ElementCategoryFilter(BuiltInCategory.OST_Viewers);
                            IList<ElementFilter> orFltrs = new List<ElementFilter>()
                                    {                                        
                                        new ElementCategoryFilter(BuiltInCategory.OST_ReferenceViewer),
                                        new ElementCategoryFilter(BuiltInCategory.OST_Viewers)
                                    };
                            // LogicalOrFilter logicalOrFilter = new LogicalOrFilter()
                            LogicalOrFilter logicalOrFltr = new LogicalOrFilter(orFltrs);
                                
                            List<Element> refViewItems = new FilteredElementCollector(sourceDoc, sourceDraftingView.Id)
                                .WherePasses(logicalOrFltr)
                                .Cast<Element>()
                                .ToList<Element>();

                            Debug.WriteLine("Invalid items: " + refViewItems.Count.ToString());

                            foreach (Element e in refViewItems)
                            {
                                Debug.WriteLine(e.Id.ToString() + " " + e.GetType().ToString() + " Name: " + e.Name);
                                ElementId vId = ReferenceableViewUtils.GetReferencedViewId(sourceDoc, e.Id);
                                View vw = sourceDoc.GetElement(vId) as View;
                                Debug.WriteLine("\tRef View: " + vw.Name);
                            }




                            // Copy the elements to the target Revit Doc:
                            ElementTransformUtils.CopyElements(sourceDraftingView, idsOfElementsToCopy, targetView, null, options);

                            // Set Failure handler to skip duplicate warnings;
                            FailureHandlingOptions failOptions = targetTrans.GetFailureHandlingOptions();
                            failOptions.SetFailuresPreprocessor(new HidePasteDuplicateTypesPreprocessor());

                            targetTrans.Commit(failOptions);

                            // Close the document
                        }
                        finally { sourceDoc.Close(false); }

                    }
                    

                }
                else
                {
                    // error, no view...
                }
            }

            // Activate it
            ViewDrafting importedView = GetDraftingViewFromDoc(_doc, detailViewName);
            if (null != importedView) revitTargetUiDoc.ActiveView = importedView;
        }

        /// <summary>
        /// Helper Function for getting Drafting Views of a certain name
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="viewName"></param>
        /// <returns></returns>
        private static ViewDrafting GetDraftingViewFromDoc(Document doc, string viewName)
        {
            ElementClassFilter viewsClassFilter = new ElementClassFilter(typeof(ViewDrafting));

            List<ViewDrafting> detailViewInTargetModel = new FilteredElementCollector(doc)
                .WherePasses(viewsClassFilter)
                .Cast<ViewDrafting>()
                //.Where(v => !v.IsTemplate)
                //.Where(v => v is ViewDrafting)
                .Where(v => v.Name.Equals(viewName))
                .ToList<ViewDrafting>();
                
            return (detailViewInTargetModel.Count > 0)
                ? detailViewInTargetModel[0]
                : null;
        }

        /// <summary>
        /// Helper Function for getting Schedules of a certain name
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="viewName"></param>
        /// <returns></returns>
        private static ViewSchedule GetScheduleViewFromDoc(Document doc, string viewName)
        {
            ElementClassFilter viewsClassFilter = new ElementClassFilter(typeof(ViewSchedule));

            List<ViewSchedule> viewsInTargetModel = new FilteredElementCollector(doc)
                .WherePasses(viewsClassFilter)
                .Cast<ViewSchedule>()
                .Where(v => v.Name.Equals(viewName, StringComparison.CurrentCultureIgnoreCase))
                .ToList<ViewSchedule>();

            return (viewsInTargetModel.Count > 0)
                ? viewsInTargetModel[0]
                : null;
        }


        public static void InsertSchedule(UIDocument revitTargetUiDoc, Revit_Content_EXTERNAL_Item externalDataRecord)
        {
            Document _doc = revitTargetUiDoc.Document;
            // Should we have a return type?
            string scheduleName = externalDataRecord.FileName;

            // Get Container file:
            FileInfo _sourceFile = new FileInfo(externalDataRecord.FullPathName);

            // Try to get the view in the local Target doc. If it exists. just activate it:
            //      Yes. Prompt to overwrite it ???
            ViewSchedule sourceScheduleView = GetScheduleViewFromDoc(_doc, scheduleName);

            // does the view already exist in the model?
            if (null == sourceScheduleView)
            {
                // No. Go get it from the Library Doc
                if (_sourceFile.Exists)
                {
                    Document sourceDoc = _doc.Application.OpenDocumentFile(_sourceFile.FullName);

                    try
                    {
                        // No, get View from source model and copy it over:
                        sourceScheduleView = GetScheduleViewFromDoc(sourceDoc, scheduleName);
                                                
                        DuplicateViewUtilities.DuplicateViewUtils.DuplicateSchedules(
                            sourceDoc,
                            new List<ViewSchedule>() { sourceScheduleView },
                            _doc);
                        
                    }
                    finally { sourceDoc.Close(false); }

                   
                }
            }

            // Activate the Imported or found Schedule:
            ViewSchedule importedView = GetScheduleViewFromDoc(_doc, scheduleName);
            if (null != importedView) revitTargetUiDoc.ActiveView = importedView;
        }


        public static void InsertSheet(UIDocument revitTargetDoc, Revit_Content_EXTERNAL_Item externalDataRecord)
        {
            // does the sheet already exist in the model?

            // No. 
            //      Do any of the views already exist?
            //          No. 
            //              Copy it over
            //          Yes.
            //              Warn user that existing views will be duplicated.

            // Activate it
        }

        public static void InsertLegend(UIDocument revitTargetDoc, Revit_Content_EXTERNAL_Item externalDataRecord)
        {
            // does the legend already exist in the model?

            // No. 
            //      Do any of the views already exist?
            //          No. 
            //              Copy it over
            //          Yes.
            //              Warn user that existing views will be duplicated.

            // Activate it
        }

    }
}
