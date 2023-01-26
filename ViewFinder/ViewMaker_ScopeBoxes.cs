using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ZGF.Revit
{
    /// <summary>
    /// Creates dependent views on a floor or ceiling plan for each of the pre-selected ScopeBoxes. 
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class ScopeBoxPlanViewMakerCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Result result = Result.Succeeded;

            // VIEW
            // Get select plan and validate
            ViewPlan thePlan = commandData.Application.ActiveUIDocument.ActiveView as ViewPlan;
            // Cannot be an area plan...
            if (null != thePlan)
                if ((thePlan.ViewType.Equals(ViewType.FloorPlan)) || (thePlan.ViewType.Equals(ViewType.AreaPlan)) || (thePlan.ViewType.Equals(ViewType.CeilingPlan)))
                {
                    if (thePlan.IsTemplate) thePlan = null;
                    if (!thePlan.CanBePrinted) thePlan = null;
                    //if (null != thePlan.AreaScheme) thePlan = null;
                }
                else
                    thePlan = null;

            if (null == thePlan)
            {
                DependentPlanMaker_ScopeBoxes.ViewMakerTaskdialog("Select ScopeBoxes in a floor, ceiling or area plan. " +
                    "Dependent views for each ScopeBox selected will be created for the active view."
                    , "Wrong view type");
                return Result.Failed; // TODO: MESSAGE HERE {Current View must be floor plan or ceiling plan}
            }


            // Scope Box Elements
            List<Element> theSelectedScopeBoxes;
            Selection currentSelection = commandData.Application.ActiveUIDocument.Selection;
            ICollection<ElementId> IDs = currentSelection.GetElementIds(); 
            if (IDs.Count > 0)
            {                
                theSelectedScopeBoxes = new FilteredElementCollector(commandData.Application.ActiveUIDocument.Document, IDs)
                    .OfCategory(BuiltInCategory.OST_VolumeOfInterest)                                    
                    .ToList();
            }
            else
            {
                // No plan selected
                DependentPlanMaker_ScopeBoxes.ViewMakerTaskdialog("Select some ScopeBoxes in the current view and try again.", "Empty selection");
                return Result.Failed; // TODO: MESAGE HERE { No Rooms Selected }
                
            }

            // Selected element is not a valid floor or ceiling plan
            if (theSelectedScopeBoxes.Count == 0)
            {
                DependentPlanMaker_ScopeBoxes.ViewMakerTaskdialog("The current selection does not include any ScopeBoxes. Select ScopeBoxes in the current view and try again.", "Invalid selection");
                return Result.Failed; // TODO: MESSAGE { No valid rooms selected }
            }

            try
            {
                // T R A N S A C T I O N
                using (Transaction t = new Transaction(commandData.Application.ActiveUIDocument.Document, "Create ScopeBox Plan Views"))
                {
                    t.Start();
                    DependentPlanMaker_ScopeBoxes rpm = new DependentPlanMaker_ScopeBoxes(theSelectedScopeBoxes, thePlan, commandData.Application.ActiveUIDocument.Document);
                    rpm.CreateCroppedDependentRoomViews();
                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                result = Result.Failed;
            }

            if (result == Result.Failed)
            {
                DependentPlanMaker_ScopeBoxes.ViewMakerTaskdialog(message, "Oops.");
            }
            return result;
        }
    }

    class ScopeBox
    {
        public string Name { get; set; }
        public Element TheScopeBox { get; set; }
    }

    class DependentPlanMaker_ScopeBoxes
    {
        Document m_doc;
        ViewPlan m_PlanOrCeilingView;
        
        string m_ParentViewName;
        List<Element> m_ScopeBoxeElems = new List<Element>();
 
        //  C O N S T R U C T O R
        public DependentPlanMaker_ScopeBoxes(List<Element> scopeBoxeElements, ViewPlan theParentView, Document theActiveDocument)
        {
            m_doc = theActiveDocument;
            m_PlanOrCeilingView = theParentView;
            m_ParentViewName = m_PlanOrCeilingView.Name;
            m_ScopeBoxeElems = scopeBoxeElements;
        }

        public List<Element> ScopeBoxesSelection { get { return m_ScopeBoxeElems; } }
        public int Count { get { return m_ScopeBoxeElems.Count; } }
        public double BufferOffsetDist { get; set; }

        public List<View> CreateCroppedDependentRoomViews()
        {
            List<View> newDependentViews = new List<View>();

            RemoveViewScopeBox(m_PlanOrCeilingView); // Necessary?

            foreach (Element sb in m_ScopeBoxeElems)
            {
                string newRoomViewName = m_PlanOrCeilingView.Name + "_" + sb.Name;

                // Convert to Crop
                if(m_PlanOrCeilingView.CanViewBeDuplicated(ViewDuplicateOption.AsDependent))
                {
                    // Duplicate as Dependent
                    ViewPlan vPlan;
                    try { vPlan = m_doc.GetElement(m_PlanOrCeilingView.Duplicate(ViewDuplicateOption.AsDependent)) as ViewPlan; }
                    catch { continue; }

                    // Apply ScopeBox

                    // Get the Parameter that holds the ScopeBox
                    Parameter sbParam = vPlan.get_Parameter(BuiltInParameter.VIEWER_VOLUME_OF_INTEREST_CROP);
                    // Set the Plan's ScopeBox property to the ID of the ScopeBox.
                    sbParam.Set(sb.Id);
                    
                    // Rename view
                    int dupeTries = 0; // In case view exists
                    do
                    {
                        try
                        {
                            if (dupeTries > 0)
                                vPlan.Name = newRoomViewName + " - copy " + dupeTries.ToString();
                            else
                                vPlan.Name = newRoomViewName;

                            dupeTries = 0;
                        }
                        catch
                        {
                            dupeTries++;
                        }
                        finally
                        {
                            string pType = vPlan.ViewType.ToString();                            
                        }
                    } while (dupeTries > 0);
                }
            }

            // return
            return newDependentViews;
        }

        private void RemoveViewScopeBox(View revitView)
        {
            Parameter p = revitView.get_Parameter(BuiltInParameter.VIEWER_VOLUME_OF_INTEREST_CROP) as Parameter;
            if ( (null != p) && (!p.AsElementId().Equals(ElementId.InvalidElementId)) )
            {
                p.Set(ElementId.InvalidElementId);
            }
        }

        public static void ViewMakerTaskdialog(string smallMessage, string bigMessage)
        {
            TaskDialog td = new TaskDialog("Dependent Plan View Maker");
            td.TitleAutoPrefix = false;
            td.MainContent = smallMessage;
            td.MainInstruction = bigMessage;
            td.Show();
        }

    }
}
