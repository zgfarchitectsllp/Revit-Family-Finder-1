using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ZGF.Revit
{
    /// <summary>
    /// Creates dependent views on a floor or ceiling plan for each of the pre-selected rooms on the plan's
    /// associated level. Views are cropped to the extents of each room.
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class RoomPlanViewMakerCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Result result = Result.Succeeded;

            // VIEW
            // Get select plan and validate
            ViewPlan thePlan = commandData.Application.ActiveUIDocument.ActiveView as ViewPlan;
            // Cannot be an area plan...
            if (null != thePlan)
                if ((thePlan.ViewType.Equals(ViewType.FloorPlan)) || (thePlan.ViewType.Equals(ViewType.CeilingPlan)))
                {
                    if (thePlan.IsTemplate) thePlan = null;
                    if (!thePlan.CanBePrinted) thePlan = null;
                    if (null != thePlan.AreaScheme) thePlan = null;
                }
                else
                    thePlan = null;

            if (null == thePlan)
            {
                RoomPlanViewMaker.ViewMakerTaskdialog("Select rooms in a floor or ceiling plan. Dependent views for each room selected will be created for the active view.", "Wrong view type");
                return Result.Failed; // TODO: MESSAGE HERE {Current View must be floor plan or ceiling plan}
            }

           


            // ROOMS
            List<Autodesk.Revit.DB.Architecture.Room> theRooms = new List<Autodesk.Revit.DB.Architecture.Room>();
            Selection currentSelection = commandData.Application.ActiveUIDocument.Selection;
            ICollection<ElementId> IDs = currentSelection.GetElementIds(); 
            if (IDs.Count > 0)
            {
                
                List<Element> rooms = new FilteredElementCollector(commandData.Application.ActiveUIDocument.Document, IDs)
                    .OfClass(typeof(SpatialElement))
                    .Where(r => r.LevelId.Equals(thePlan.GenLevel.Id))                
                    .ToList();

                ElementId viewPhase = thePlan.get_Parameter(BuiltInParameter.VIEW_PHASE).AsElementId();

                foreach (Element el in rooms)
                {
                    Autodesk.Revit.DB.Architecture.Room rm = el as Autodesk.Revit.DB.Architecture.Room;
                    if (null != rm)
                    {
                        ElementId rm_Phase = rm.get_Parameter(BuiltInParameter.ROOM_PHASE).AsElementId();

                        if (
                                (viewPhase == rm_Phase) && 
                                (rm.Area > 0)
                            )
                            theRooms.Add(rm); 
                    }
                }
            }
            else
            {
                // No plan selected
                RoomPlanViewMaker.ViewMakerTaskdialog("Select rooms in the current view and try again.", "Empty selection");
                return Result.Failed; // TODO: MESAGE HERE { No Rooms Selected }
                
            }

            // Selected element is not a valid floor or ceiling plan
            if (theRooms.Count == 0)
            {
                RoomPlanViewMaker.ViewMakerTaskdialog("The current selection does not include any rooms. Select rooms in the current view and try again.", "Invalid selection");
                return Result.Failed; // TODO: MESSAGE { No valid rooms selected }
            }

            try
            {
                // T R A N S A C T I O N
                using (Transaction t = new Transaction(commandData.Application.ActiveUIDocument.Document, "Create Room Views"))
                {
                    t.Start();
                    RoomPlanViewMaker rpm = new RoomPlanViewMaker(theRooms, thePlan, commandData.Application.ActiveUIDocument.Document);
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
                RoomPlanViewMaker.ViewMakerTaskdialog(message, "Oops.");
            }
            return result;
        }
    }

    class RoomPlanViewMaker
    {
        Document m_doc;
        ViewPlan m_PlanOrCeilingView;
        double m_buffer_offset = 2; // How to use current units?
        
        
        string m_ParentViewName;
        List<Autodesk.Revit.DB.Architecture.Room> m_Rooms = new List<Autodesk.Revit.DB.Architecture.Room>();


        // Validate current plan view
        //      Is floorplan or ceilingplan
        //      Is not Dependent
        //      
        // Get associated level and all (enclosed) rooms
        //
        // Get Boundaries of rooms
        //
        // Create duplicate / dependent views for each room & apply crop
        //      Name = <room_name> on <parent>
     
        //  C O N S T R U C T O R
        public RoomPlanViewMaker(List<Autodesk.Revit.DB.Architecture.Room> rooms, ViewPlan theParentView, Document theActiveDocument)
        {
            m_doc = theActiveDocument;
            m_PlanOrCeilingView = theParentView;
            m_ParentViewName = m_PlanOrCeilingView.Name;
            m_Rooms = rooms;

            
            // Create Dependent Views
            int rm_count = m_Rooms.Count;
        }

        public List<Autodesk.Revit.DB.Architecture.Room> MyProperty { get { return m_Rooms; } }
        public int RoomCount { get { return m_Rooms.Count; } }
        public double BufferOffsetDist { get; set; }

        public List<View> CreateCroppedDependentRoomViews()
        {
            List<View> newDependentViews = new List<View>();

            RemoveViewScopeBox(m_PlanOrCeilingView);

            foreach (Autodesk.Revit.DB.Architecture.Room rm in m_Rooms)
            {
                string rmNumber = rm.get_Parameter(BuiltInParameter.ROOM_NUMBER).AsString();
                string rmName = rm.get_Parameter(BuiltInParameter.ROOM_NAME).AsString();                
                string newRoomViewName = rmNumber + " " + rmName + " - " + m_PlanOrCeilingView.Name;
                // Get Room Boundary (As BoundingBox() or as BoundarySegments[] ?)
                BoundingBoxXYZ rmBoundingBox = rm.get_BoundingBox(m_PlanOrCeilingView);

                XYZ min = new XYZ(rmBoundingBox.Min.X - m_buffer_offset, rmBoundingBox.Min.Y - m_buffer_offset, rmBoundingBox.Min.Z);
                XYZ max = new XYZ(rmBoundingBox.Max.X + m_buffer_offset, rmBoundingBox.Max.Y + m_buffer_offset, rmBoundingBox.Max.Z);

                rmBoundingBox.Min = min;
                rmBoundingBox.Max = max;

                // Convert to Crop
                if(m_PlanOrCeilingView.CanViewBeDuplicated(ViewDuplicateOption.AsDependent))
                {
                    // Duplicate as Dependent
                    ViewPlan vPlan;
                    try { vPlan = m_doc.GetElement(m_PlanOrCeilingView.Duplicate(ViewDuplicateOption.AsDependent)) as ViewPlan; }
                    catch { continue; }
                    // Apply crop
                    vPlan.CropBox = rmBoundingBox;
                    vPlan.CropBoxActive = vPlan.CropBoxVisible = true;
                    ViewCropRegionShapeManager vcrsm = vPlan.GetCropRegionShapeManager();
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
                            string pType = vPlan.ViewType.Equals(ViewType.CeilingPlan) ? "CEILING" : "FLOOR";
                            vPlan.get_Parameter(BuiltInParameter.VIEW_DESCRIPTION).Set(rmNumber + " " + rmName + pType + " PLAN");
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
            TaskDialog td = new TaskDialog("Room Plan View Maker");
            td.TitleAutoPrefix = false;
            td.MainContent = smallMessage;
            td.MainInstruction = bigMessage;
            td.Show();
        }

    }
}
