using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;


namespace ZGF.Revit.ViewUtilities
{

    // 0B65F19E-7770-4947-BEB1-2DAFFED59CD5
    /// <summary>
    /// R E N U M B E R   V I E W S  -  DOWN > LEFT
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class RenumberViewsDownThenLeft : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;
            ViewSheet activeView = doc.ActiveView as ViewSheet;

            if (null == activeView)
            {
                TaskDialog td = RenumberViewsOnSheet.RenumberViewsErrorMsg();
                td.Show();
                return Result.Cancelled;
            }
            else
            {
                RenumberViewsResult.Result renumberResult = RenumberViewsOnSheet.RenumberViews(doc, SortDirection.DownThenLeft);
                switch (renumberResult)
                {
                    case RenumberViewsResult.Result.NoViewsOnSheet:
                        RenumberViewsResult.ErrorMsg(RenumberViewsResult.NoViewsOnSheet);
                        return Result.Cancelled;
                    case RenumberViewsResult.Result.SomethingElseScrewedUpTheWorks:
                        RenumberViewsResult.ErrorMsg(RenumberViewsResult.SomethingElseScrewedUpTheWorks);
                        return Result.Cancelled;
                    default:
                        return Result.Succeeded;
                }
            }
        }
    }

    // B205F647-846B-4125-87CA-95D682112E07
    /// <summary>
    /// R E N U M B E R   V I E W S  -  LEFT > DOWN
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class RenumberViewsLeftThenDown : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;
            ViewSheet activeView = doc.ActiveView as ViewSheet;

            if (null == activeView)
            {
                TaskDialog td = RenumberViewsOnSheet.RenumberViewsErrorMsg();
                td.Show();
                return Result.Cancelled;
            }
            else
            {
                RenumberViewsResult.Result renumberResult = RenumberViewsOnSheet.RenumberViews(doc, SortDirection.LeftThenDown);
                switch (renumberResult)
                {
                    case RenumberViewsResult.Result.NoViewsOnSheet:
                        RenumberViewsResult.ErrorMsg(RenumberViewsResult.NoViewsOnSheet);
                        return Result.Cancelled;
                    case RenumberViewsResult.Result.SomethingElseScrewedUpTheWorks:
                        RenumberViewsResult.ErrorMsg(RenumberViewsResult.SomethingElseScrewedUpTheWorks);
                        return Result.Cancelled;
                    default:
                        return Result.Succeeded;
                }
            }
        }
    }

    // 60D0DC0-9461-49B0-9E6E-63A406D82AA7
    /// <summary>
    /// R E N U M B E R   V I E W S  -  LETTERS (DOWN > LEFT)
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class RenumberViewsWithLetters : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;
            ViewSheet activeView = doc.ActiveView as ViewSheet;

            if (null == activeView)
            {
                TaskDialog td = RenumberViewsOnSheet.RenumberViewsErrorMsg();
                td.Show();

                return Result.Cancelled;
            }
            else
            {
                RenumberViewsResult.Result renumberResult = RenumberViewsOnSheet.RenumberViews(doc, SortDirection.UseLetters);
                switch (renumberResult)
                {
                    case RenumberViewsResult.Result.NoViewsOnSheet:
                        RenumberViewsResult.ErrorMsg(RenumberViewsResult.NoViewsOnSheet);
                        return Result.Cancelled;
                    case RenumberViewsResult.Result.SomethingElseScrewedUpTheWorks:
                        RenumberViewsResult.ErrorMsg(RenumberViewsResult.SomethingElseScrewedUpTheWorks);
                        return Result.Cancelled;
                    default:
                        return Result.Succeeded;
                }
            }
        }
    }

    #region NON-STANDARD Methods for Boeing team
    // 528E938F-EACD-42FA-849B-12A9710A5E8D
    /// <summary>
    /// R E N U M B E R   V I E W S  -  LETTERS (DOWN > RIGHT)
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class RenumberViewsDownThenRight : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;
            ViewSheet activeView = doc.ActiveView as ViewSheet;

            if (null == activeView)
            {
                TaskDialog td = RenumberViewsOnSheet.RenumberViewsErrorMsg();
                td.Show();

                return Result.Cancelled;
            }
            else
            {
                RenumberViewsResult.Result renumberResult = RenumberViewsOnSheet.RenumberViews(doc, SortDirection.DownThenRight);
                switch (renumberResult)
                {
                    case RenumberViewsResult.Result.NoViewsOnSheet:
                        RenumberViewsResult.ErrorMsg(RenumberViewsResult.NoViewsOnSheet);
                        return Result.Cancelled;
                    case RenumberViewsResult.Result.SomethingElseScrewedUpTheWorks:
                        RenumberViewsResult.ErrorMsg(RenumberViewsResult.SomethingElseScrewedUpTheWorks);
                        return Result.Cancelled;
                    default:
                        return Result.Succeeded;
                }
            }
        }
    }


    // 8A431AB6-37D7-4419-95F9-196C47BD3A17
    /// <summary>
    /// R E N U M B E R   V I E W S  -  LETTERS (RIGHT > DOWN)
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class RenumberViewsRightThenDown : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;
            ViewSheet activeView = doc.ActiveView as ViewSheet;

            if (null == activeView)
            {
                TaskDialog td = RenumberViewsOnSheet.RenumberViewsErrorMsg();
                td.Show();
                return Result.Cancelled;
            }
            else
            {
                RenumberViewsResult.Result renumberResult = RenumberViewsOnSheet.RenumberViews(doc, SortDirection.RightThenDown);
                switch (renumberResult)
                {
                    case RenumberViewsResult.Result.NoViewsOnSheet:
                        RenumberViewsResult.ErrorMsg(RenumberViewsResult.NoViewsOnSheet);
                        return Result.Cancelled;
                    case RenumberViewsResult.Result.SomethingElseScrewedUpTheWorks:
                        RenumberViewsResult.ErrorMsg(RenumberViewsResult.SomethingElseScrewedUpTheWorks);
                        return Result.Cancelled;
                    default:
                        return Result.Succeeded;
                }
            }
        }
    }


    /// <summary>
    /// R E N U M B E R   V I E W S  -  LEFT > UP
    /// BALLINGER Standard for CHoP
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class RenumberViewsLeftThenUp : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;
            ViewSheet activeView = doc.ActiveView as ViewSheet;

            if (null == activeView)
            {
                TaskDialog td = RenumberViewsOnSheet.RenumberViewsErrorMsg();
                td.Show();
                return Result.Cancelled;
            }
            else
            {
                RenumberViewsResult.Result renumberResult = RenumberViewsOnSheet.RenumberViews(doc, SortDirection.LeftThenUp);
                switch (renumberResult)
                {
                    case RenumberViewsResult.Result.NoViewsOnSheet:
                        RenumberViewsResult.ErrorMsg(RenumberViewsResult.NoViewsOnSheet);
                        return Result.Cancelled;
                    case RenumberViewsResult.Result.SomethingElseScrewedUpTheWorks:
                        RenumberViewsResult.ErrorMsg(RenumberViewsResult.SomethingElseScrewedUpTheWorks);
                        return Result.Cancelled;
                    default:
                        return Result.Succeeded;
                }
            }
        }
    }

    /// <summary>
    /// R E N U M B E R   V I E W S  -  UP > LEFT
    /// BALLINGER Standard for CHoP
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class RenumberViewsUpThenLeft : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;
            ViewSheet activeView = doc.ActiveView as ViewSheet;

            if (null == activeView)
            {
                TaskDialog td = RenumberViewsOnSheet.RenumberViewsErrorMsg();
                td.Show();
                return Result.Cancelled;
            }
            else
            {
                RenumberViewsResult.Result renumberResult = RenumberViewsOnSheet.RenumberViews(doc, SortDirection.UpThenLeft);
                switch (renumberResult)
                {
                    case RenumberViewsResult.Result.NoViewsOnSheet:
                        RenumberViewsResult.ErrorMsg(RenumberViewsResult.NoViewsOnSheet);
                        return Result.Cancelled;
                    case RenumberViewsResult.Result.SomethingElseScrewedUpTheWorks:
                        RenumberViewsResult.ErrorMsg(RenumberViewsResult.SomethingElseScrewedUpTheWorks);
                        return Result.Cancelled;
                    default:
                        return Result.Succeeded;
                }
            }
        }
    }

    #endregion

    class RenumberViewsOnSheet
    {
        public static RenumberViewsResult.Result RenumberViews(Document doc, SortDirection sortDirection)
        {
            List<ViewportWrapper> m_viewsOnSheet = new List<ViewportWrapper>();
            List<ViewportWrapper> m_viewsHaveNoTitle = new List<ViewportWrapper>();

            ViewSheet sheet = (ViewSheet)doc.ActiveView;

            // Collect Viewports on current sheet that have Detail Numbers
            foreach (ElementId id in sheet.GetAllViewports())
            {
                // Get Viewport on sheet
                Viewport currentVP = (Viewport)doc.GetElement(id);
                // Check to see if it has a 'Detail Number' parameter. If not, it's a schedule or legend, so ignore:
                Parameter pDetailNumber = currentVP.get_Parameter(BuiltInParameter.VIEWPORT_DETAIL_NUMBER) as Parameter;
                // Add to list of Viewports
                if (null != pDetailNumber)
                {
                    // Legends have a detail number, but it's an empty string:                
                    if (String.IsNullOrEmpty(pDetailNumber.AsString()))
                        continue;

                    ViewportWrapper vpw = new ViewportWrapper(doc, currentVP);
                    if (vpw.HasTitle)
                        m_viewsOnSheet.Add(vpw);
                    else
                        m_viewsHaveNoTitle.Add(vpw); //These will be appended to m_viewsOnSheet after sorting
                }
            }

            // sort by SORTING DIRECTION
            switch (sortDirection)
            {
                case SortDirection.DownThenRight:
                    m_viewsOnSheet.Sort(new RightComparerXthenY());
                    break;
                case SortDirection.RightThenDown:
                    m_viewsOnSheet.Sort(new RightComparerYthenX());
                    break;
                case SortDirection.LeftThenDown:
                    m_viewsOnSheet.Sort(new LeftComparerYthenX());
                    break;
                case SortDirection.LeftThenUp:
                    m_viewsOnSheet.Sort(new LeftComparerXthenY_UP());
                    break;
                case SortDirection.UpThenLeft:
                    m_viewsOnSheet.Sort(new LeftComparerYthenX_UP());
                    break;
                default: // SortDirection.DownThenLeft or SortDirection.UseLetters
                    m_viewsOnSheet.Sort(new LeftComparerXthenY());
                    break;
            }

            if ((m_viewsOnSheet.Count > 0) & (m_viewsHaveNoTitle.Count > 0))
                m_viewsOnSheet.AddRange(m_viewsHaveNoTitle);

            using (Transaction trans = new Transaction(doc, "Renumber views on sheet " + sheet.SheetNumber))
            {
                trans.Start();
                // reset numbers
                try
                {
                    foreach (ViewportWrapper vp in m_viewsOnSheet)
                    {
                        vp.DetailNumber += "_";
                    }
                    // renumber
                    int dtlNumber = 1;

                    if (sortDirection == SortDirection.UseLetters)
                    {
                        //
                        //  TODO: THIS SHOULD BE A RECURSIVE FUNCTION
                        //
                        string prefixString = string.Empty;
                        int prefixInt = 64;

                        int currentNumber = 64;

                        foreach (ViewportWrapper vp in m_viewsOnSheet)
                        {
                            if ((dtlNumber > 26) & ((dtlNumber - 1) % 26 == 0))
                            {
                                prefixInt++;
                                prefixString = Convert.ToChar(prefixInt).ToString();
                                currentNumber = 65;
                            }
                            else
                                currentNumber++;

                            vp.DetailNumber = prefixString + Convert.ToChar(currentNumber).ToString();

                            dtlNumber++;
                        }
                    }
                    else
                    {
                        foreach (ViewportWrapper vp in m_viewsOnSheet)
                        {
                            vp.DetailNumber = dtlNumber.ToString();
                            dtlNumber++;
                        }
                    }

                    trans.Commit();
                }
                catch
                {
                    return RenumberViewsResult.Result.SomethingElseScrewedUpTheWorks;
                }

            }

            return RenumberViewsResult.Result.OK;
        }

        /// <summary>
        /// View renumbering error dialog
        /// </summary>
        /// <returns></returns>
        public static TaskDialog RenumberViewsErrorMsg()
        {
            TaskDialog td = new TaskDialog("Renumber Views on Current Sheet");
            td.MainInstruction = "Current view must be a sheet";
            td.MainContent = "Please activate a sheet view before attempting to renumber views.";
            td.TitleAutoPrefix = false;

            return td;
        }

    }


    class ViewportWrapper
    {
        Document m_doc;
        Viewport m_viewport;
        View m_view;
        XYZ m_VpOriginPt = XYZ.Zero;

        public ViewportWrapper(Document theDoc, Viewport theViewport)
        {
            m_doc = theDoc;
            m_viewport = theViewport;
            m_view = (View)theDoc.GetElement(m_viewport.ViewId);

            try
            {
                Outline vp_labelOutline = m_viewport.GetLabelOutline();

                m_VpOriginPt = vp_labelOutline.MinimumPoint;                
                HasTitle = !vp_labelOutline.IsEmpty;
            }
            catch
            {
                HasTitle = false; // There is no LabelOutline to get if the viewport type has title set to 'none'                
            }
        }

        public View TheView { get { return m_view; } }

        public XYZ ViewOriginPointOnSheet { get { return m_VpOriginPt; } }

        public string DetailNumber
        {
            get
            {
                Parameter p = m_viewport.get_Parameter(BuiltInParameter.VIEWPORT_DETAIL_NUMBER);
                return p.AsString();
            }
            set
            {
                Parameter p = m_viewport.get_Parameter(BuiltInParameter.VIEWPORT_DETAIL_NUMBER);
                try
                {
                    p.Set((string)value);
                }
                catch
                {
                    throw new Exception("Could not set detail number for view: " + m_view.Name);
                }
            }
        }

        public bool HasTitle { get; set; }
    }

    //------------------------------------------------
    // Comparers for sorting view coords
    //------------------------------------------------
    #region RIGHT < LEFT SORTS
    /// <summary>
    /// Sort Down then Right
    /// </summary>
    class RightComparerXthenY : IComparer<ViewportWrapper>
    {
        public int Compare(ViewportWrapper viewportA, ViewportWrapper viewportB)
        {
            System.Drawing.Point a = new System.Drawing.Point(Convert.ToInt32(viewportA.ViewOriginPointOnSheet.X * 12000), Convert.ToInt32(viewportA.ViewOriginPointOnSheet.Y * 12000));
            System.Drawing.Point b = new System.Drawing.Point(Convert.ToInt32(viewportB.ViewOriginPointOnSheet.X * 12000), Convert.ToInt32(viewportB.ViewOriginPointOnSheet.Y * 12000));

            if (RenumberViewsResult.PointIsEqualTo(b.X, a.X, RenumberViewsResult.FuzzFactorForViewportAlignment)) // (b.X != a.X)
            {
                return b.Y - a.Y;
            }
            else
            {
                return a.X - b.X;
            }
        }
    }
    class RightComparerYthenX : IComparer<ViewportWrapper>
    {
        public int Compare(ViewportWrapper viewportA, ViewportWrapper viewportB)
        {
            System.Drawing.Point a = new System.Drawing.Point(Convert.ToInt32(viewportA.ViewOriginPointOnSheet.X * 12000), Convert.ToInt32(viewportA.ViewOriginPointOnSheet.Y * 12000));
            System.Drawing.Point b = new System.Drawing.Point(Convert.ToInt32(viewportB.ViewOriginPointOnSheet.X * 12000), Convert.ToInt32(viewportB.ViewOriginPointOnSheet.Y * 12000));

            if (RenumberViewsResult.PointIsEqualTo(a.Y, b.Y, RenumberViewsResult.FuzzFactorForViewportAlignment)) // (a.Y != b.Y)
            {
                return a.X - b.X;
            }
            else
            {
                return b.Y - a.Y;
            }
        }
    }
    #endregion

    #region LEFT > RIGHT SORTS
    /// <summary>
    /// Sort RIGHT to LEFT by columns then DOWN the sheet
    /// </summary>
    class LeftComparerXthenY : IComparer<ViewportWrapper>
    {
        public int Compare(ViewportWrapper viewportA, ViewportWrapper viewportB)
        {
            System.Drawing.Point a = new System.Drawing.Point(Convert.ToInt32(viewportA.ViewOriginPointOnSheet.X * 12000), Convert.ToInt32(viewportA.ViewOriginPointOnSheet.Y * 12000));
            System.Drawing.Point b = new System.Drawing.Point(Convert.ToInt32(viewportB.ViewOriginPointOnSheet.X * 12000), Convert.ToInt32(viewportB.ViewOriginPointOnSheet.Y * 12000));
            int returnval;
            if (RenumberViewsResult.PointIsEqualTo(b.X, a.X, RenumberViewsResult.FuzzFactorForViewportAlignment)) // (b.X != a.X)
            {
                returnval = b.Y - a.Y;
            }
            else
            {
                returnval = b.X - a.X;
            }
            return returnval;
        }
    }
    /// <summary>
    /// Sorts RIGHT to LEFT by rows then DOWN the sheet
    /// </summary>
    class LeftComparerYthenX : IComparer<ViewportWrapper>
    {
        public int Compare(ViewportWrapper viewportA, ViewportWrapper viewportB)
        {
            System.Drawing.Point a = new System.Drawing.Point(Convert.ToInt32(viewportA.ViewOriginPointOnSheet.X * 12000), Convert.ToInt32(viewportA.ViewOriginPointOnSheet.Y * 12000)); // x 12000 converts to 1000's of an inch
            System.Drawing.Point b = new System.Drawing.Point(Convert.ToInt32(viewportB.ViewOriginPointOnSheet.X * 12000), Convert.ToInt32(viewportB.ViewOriginPointOnSheet.Y * 12000));

            if (RenumberViewsResult.PointIsEqualTo(b.Y, a.Y, RenumberViewsResult.FuzzFactorForViewportAlignment)) // (b.Y != a.Y)
            {
                return b.X - a.X;
            }
            else
            {
                return b.Y - a.Y;
            }
        }
    }

    /// /// <summary>
    /// Sorts DOWN to UP by columns then LEFT the sheet [ BALLINGER ]
    /// </summary>
    class LeftComparerYthenX_UP : IComparer<ViewportWrapper>
    {
        public int Compare(ViewportWrapper viewportA, ViewportWrapper viewportB)
        {

            System.Drawing.Point a = new System.Drawing.Point(Convert.ToInt32(viewportA.ViewOriginPointOnSheet.X * 12000), Convert.ToInt32(viewportA.ViewOriginPointOnSheet.Y * 12000)); // x 12000 converts to 1000's of an inch
            System.Drawing.Point b = new System.Drawing.Point(Convert.ToInt32(viewportB.ViewOriginPointOnSheet.X * 12000), Convert.ToInt32(viewportB.ViewOriginPointOnSheet.Y * 12000));

            if (RenumberViewsResult.PointIsEqualTo(b.X, a.X, RenumberViewsResult.FuzzFactorForViewportAlignment)) // (b.Y != a.Y)
            {
                return a.Y - b.Y;
            }
            else
            {
                return b.X - a.X;
            }
        }
    }


    /// <summary>
    /// Sorts RIGHT to LEFT by rows then UP the sheet [ BALLINGER ]
    /// </summary>
    class LeftComparerXthenY_UP : IComparer<ViewportWrapper>
    {
        public int Compare(ViewportWrapper viewportA, ViewportWrapper viewportB)
        {
            System.Drawing.Point a = new System.Drawing.Point(Convert.ToInt32(viewportA.ViewOriginPointOnSheet.X * 12000), Convert.ToInt32(viewportA.ViewOriginPointOnSheet.Y * 12000)); // x 12000 converts to 1000's of an inch
            System.Drawing.Point b = new System.Drawing.Point(Convert.ToInt32(viewportB.ViewOriginPointOnSheet.X * 12000), Convert.ToInt32(viewportB.ViewOriginPointOnSheet.Y * 12000));

            if (RenumberViewsResult.PointIsEqualTo(b.Y, a.Y, RenumberViewsResult.FuzzFactorForViewportAlignment)) // (b.Y != a.Y)
            {
                return b.X - a.X;
            }
            else
            {
                return a.Y - b.Y;
            }
        }
    }

    #endregion
    //------------------------------------------------

    enum SortDirection
    {
        UseLetters,
        LeftThenDown,
        RightThenDown,
        DownThenLeft,
        DownThenRight,
        LeftThenUp, // BALLINGER for CHoP
        UpThenLeft // BALLINGER for CHoP
    }

    class RenumberViewsResult
    {
        public enum Result
        {
            OK,
            NoViewsOnSheet,
            SomethingElseScrewedUpTheWorks
        }

        public const string NoViewsOnSheet = "Current sheet has no views";
        public const string SomethingElseScrewedUpTheWorks = "Unspecified error";
        public const int FuzzFactorForViewportAlignment = 1000;

        public static void ErrorMsg(string msg)
        {
            TaskDialog td = new TaskDialog("Renumber Views on Current Sheet");
            td.MainInstruction = "Error";
            td.MainContent = msg;
            td.Show();
        }

        // Comparer function with fuzz factor
        public static bool PointIsEqualTo(int P1, int P2, int fuzzAmtMilliInches)
        {
            return (Math.Abs(P1 - P2) < fuzzAmtMilliInches);
        }
    }


}
