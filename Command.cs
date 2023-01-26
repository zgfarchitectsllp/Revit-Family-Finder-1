//
// ZGF Architects, LLP
//
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Text;
using System.Windows;
using System.Windows.Interop;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ZGF.Revit
{
    /// <summary>
    /// F A M I L Y   F I N D E R
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.UsingCommandData)]
    public class FamilyFinder : IExternalCommand
    {

        public static WindowHandle hwndRevit = null;
        public static bool isModeless = false;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Result result = Result.Succeeded;

            Document m_doc = commandData.Application.ActiveUIDocument.Document;
            //-----------------------------------------------------------------
            // If symbols cannot be added to this type of view, then exit.

            ViewType viewType = commandData.Application.ActiveUIDocument.ActiveGraphicalView.ViewType;

            switch (viewType)
            {
                case ViewType.ColumnSchedule:
                case ViewType.CostReport:
                case ViewType.Legend:
                case ViewType.Rendering:
                case ViewType.Report:
                case ViewType.Schedule:
                case ViewType.Walkthrough:
                    TaskDialog td = new TaskDialog("ZGF Family Finder");
                    td.MainContent = "Current view is not valid for placing components.";
                    td.Show();
                    return result;
            }
            //----------------------------------------------------------------------------------

            FamilyBrowser fb = new FamilyBrowser(commandData);
            
            // http://jeremytammik.github.io/tbc/a/0088_revit_window_handle.htm

            if (isModeless)
            {
                IntPtr h = commandData.Application.MainWindowHandle;
                hwndRevit = new WindowHandle(h);

                fb.Show(hwndRevit);
            }
            else
            {
                System.Windows.Forms.DialogResult returnVal = fb.ShowDialog();

                try
                {
                    if (null != fb.CurrentFamilyElementType)
                        commandData.Application.ActiveUIDocument.PostRequestForElementTypePlacement(fb.CurrentFamilyElementType);
                }
                catch (Exception ex)
                {
                    TaskDialog td = new TaskDialog("ZGF Family Finder");
                    td.MainContent = ex.Message;
                    td.Show();
                }

                fb.Close();
            }
            return result;

        }
    }



    /// <summary>
    /// Collects and iterates families to see if they can be opened and closed. Families that cannot be opened are returned as corrupt.
    /// </summary>
    /// [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.UsingCommandData)]
    public class CheckForCorruptFamilies : IExternalCommand
    {
        public Autodesk.Revit.UI.Result Execute(
            Autodesk.Revit.UI.ExternalCommandData commandData,
            ref string message,
            Autodesk.Revit.DB.ElementSet elements)
        {
            // {43E2AE0C-87CB-4A1A-AF6B-9152814A0B44}

            TaskDialog tdQuick = new TaskDialog("Do you want to scan and repair loaded families?");
            tdQuick.TitleAutoPrefix = false;
            tdQuick.ExtraCheckBoxText = "Deep scan? (Opens families, takes longer)";
            tdQuick.MainInstruction = "Scan families for errors";
            tdQuick.MainContent = "Revit will scan and attempt to repair each loaded family. If \"Deep scan\" is checked, Revit will attempt to open each family. Families that are corrupt and cannot be opened will reported at the end of the scan."
                    + "\n\nDeep scan takes much longer, so try without it first.";
            tdQuick.CommonButtons = TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No;
            tdQuick.DefaultButton = TaskDialogResult.No;
            TaskDialogResult tdResult = tdQuick.Show();
            if (tdResult == TaskDialogResult.No) return Result.Cancelled;

            if (tdQuick.WasExtraCheckBoxChecked())
            {
                commandData.Application.Application.FailuresProcessing += Application_FailuresProcessing;

                int total;
                try
                {
                    List<Family> corruptFamilies = new List<Family>();
                    corruptFamilies = ZGF.Revit.FamilyHelper.CheckEditableFamiliesForCorruption(
                        commandData.Application.ActiveUIDocument.Document,
                        false,
                        out total);

                    TaskDialog td = new TaskDialog("Family Integrity Status");
                    td.TitleAutoPrefix = false;

                    if (corruptFamilies.Count > 0)
                    {
                        StringBuilder returnList = new StringBuilder("Category :: Family Name\r\n\r\n");
                        foreach (Family f in corruptFamilies)
                        {
                            returnList.Append(f.FamilyCategory.Name + " :: " + f.Name + "\r\n");
                        }

                        td.MainInstruction = "The following families could not be accessed and are probably corrupt:";
                        td.MainContent = returnList.ToString();
                        td.MainIcon = TaskDialogIcon.TaskDialogIconError;
                    }
                    else
                    {
                        td.MainInstruction = "No corrupt families found";
                        td.MainContent = "You're good...";
                        td.MainIcon = TaskDialogIcon.TaskDialogIconNone;
                    }

                    td.Show();

                    return Result.Succeeded;
                }
                catch (Exception ex)
                {
                    message = ex.StackTrace;
                    return Result.Failed;
                }
                finally
                {
                    commandData.Application.Application.FailuresProcessing -= Application_FailuresProcessing;
                }
            }
            else
            {
                try
                {
                    int total;
                    ZGF.Revit.FamilyHelper.CheckEditableFamiliesForCorruption(
                        commandData.Application.ActiveUIDocument.Document,
                        true,
                        out total);
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    return Result.Failed;
                }

                return Result.Succeeded;
            }


        }


        // Family warning swallower:
        private void Application_FailuresProcessing(object sender, Autodesk.Revit.DB.Events.FailuresProcessingEventArgs e)
        {
            FailuresAccessor fa = e.GetFailuresAccessor();
            IList<FailureMessageAccessor> fails = fa.GetFailureMessages();


            // Inside event handler, get all warnings

            IList<FailureMessageAccessor> a
              = fa.GetFailureMessages();

            int count = 0;

            foreach (FailureMessageAccessor failure in a)
            {
                Debug.WriteLine("Failure", failure.GetDescriptionText());

                fa.ResolveFailure(failure);

                ++count;
            }

            if (0 < count
              && e.GetProcessingResult() == FailureProcessingResult.Continue)
            {
                e.SetProcessingResult(FailureProcessingResult.ProceedWithCommit);
            }

        }
    }


    /// <summary>
    /// E X P O R T   F A M I L I E S
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.UsingCommandData)]
    public class ExportFamilies : IExternalCommand
    {
        public Result Execute(
                ExternalCommandData commandData,
                ref string message,
                ElementSet elements)
        {

            Document doc = commandData.Application.ActiveUIDocument.Document;
            doc.Application.FailuresProcessing += Application_FailuresProcessing;

            TaskDialog confirmExport = new TaskDialog("ZGF Family Export Utility");
            confirmExport.MainInstruction = "Save current project(s) before proceeding to export families?";
            confirmExport.MainContent = "If Revit encounters corrupted components during the process of exporting Revit could crash. " +
                    "Choose \"Yes\" to save your projects before processing, \"No\" to proceed without saving and \"Cancel\" to abort the process of exporting families." +
                    "\n\nNOTE: Unnamed projects and family files will be ignored, so choose \"Cancel\" if you want to save these files before proceeding.";
            confirmExport.CommonButtons = TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No | TaskDialogCommonButtons.Cancel;

            confirmExport.DefaultButton = TaskDialogResult.Cancel;
            confirmExport.MainIcon = TaskDialogIcon.TaskDialogIconWarning;
            confirmExport.TitleAutoPrefix = false;

            TaskDialogResult tdr = confirmExport.Show();

            switch (tdr)
            {
                case TaskDialogResult.Yes:
                    // Save named, editable, dirty files...
                    foreach (Document d in doc.Application.Documents)
                    {
                        Debug.WriteLine("Path: " + d.PathName);
                        if (d.IsModified & d.IsModifiable)
                        {
                            if (d.PathName == string.Empty) // Drawing is unnamed
                            {
                                SaveAsOptions sao = new SaveAsOptions();
                                sao.OverwriteExistingFile = true;
                                // How to kick off a save?
                            }
                            else
                                d.Save();
                        }
                    }
                    break;
                case TaskDialogResult.Cancel:
                    return Result.Cancelled;
                    //		break;
                    //default:

            }

            try
            {
                FamilyExport exportDialog = new FamilyExport(commandData);
                DialogResult dialogResult = exportDialog.ShowDialog();

                switch (dialogResult)
                {
                    case DialogResult.OK:
                        return Result.Succeeded;
                    case DialogResult.Cancel:
                        return Result.Cancelled;
                    default:
                        return Result.Failed;
                }
            }
            finally
            {
                doc.Application.FailuresProcessing -= Application_FailuresProcessing;
            }

        }


        /// <summary>
        /// Gets whether the specified path is a valid absolute file path.
        /// </summary>
        /// <param name="path">Any path. OK if null or empty.</param>
        static public bool IsValidPath(string path)
        {
            Regex r = new Regex(@"^(([a-zA-Z]\:)|(\\))(\\{1}|((\\{1})[^\\]([^/:*?<>""|]*))+)$");
            return r.IsMatch(path);
        }

        // Family warning swallower:
        private void Application_FailuresProcessing(object sender, Autodesk.Revit.DB.Events.FailuresProcessingEventArgs e)
        {
            FailuresAccessor fa = e.GetFailuresAccessor();
            IList<FailureMessageAccessor> fails = fa.GetFailureMessages();

            // Inside event handler, get all warnings

            IList<FailureMessageAccessor> a = fa.GetFailureMessages();

            int count = 0;

            foreach (FailureMessageAccessor failure in a)
            {
                Debug.WriteLine("Failure", failure.GetDescriptionText());

                fa.ResolveFailure(failure);

                ++count;
            }

            if (0 < count && e.GetProcessingResult() == FailureProcessingResult.Continue)
            {
                e.SetProcessingResult(FailureProcessingResult.ProceedWithCommit);
            }
        }



    }


    /// <summary>
    /// I M P O R T   F A M I L I E S
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.UsingCommandData)]
    public class ImportFamilies : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            TaskDialog confirmImport = new TaskDialog("ZGF Family Import Utility");
            confirmImport.MainInstruction = "Are you sure you want to import families?";
            confirmImport.MainContent = "The families in your model will be overwritten by the imported Revit family files. \n\nShould any of the components in your model be corrupt, this process could cause Revit to crash. " +
                    "Therefore, if you have not saved your model recently, please choose \"No\" and do so before importing families.";
            confirmImport.CommonButtons = TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No;
            confirmImport.DefaultButton = TaskDialogResult.No;
            confirmImport.MainIcon = TaskDialogIcon.TaskDialogIconWarning;
            confirmImport.TitleAutoPrefix = false;

            TaskDialogResult tdr = confirmImport.Show();

            if (tdr == TaskDialogResult.No) return Result.Cancelled;

            // Proceed...

            FamilyImport doImport = new FamilyImport(commandData);
            using (Transaction importTrans = new Transaction(commandData.Application.ActiveUIDocument.Document, "Import families"))
            {
                try
                {
                    importTrans.Start();
                    doImport.ShowDialog();
                    importTrans.Commit();
                }
                catch
                {
                    return Result.Failed;
                }
            }
            // Done!
            switch (doImport.ReturnValue)
            {
                case DialogResult.Abort:
                    return Result.Failed;
                case DialogResult.Cancel:
                    return Result.Cancelled;
                default:
                    return Result.Succeeded;
            }
        }
    }



    /// <summary>
    /// V I E W   B R O W S E R
    /// </summary>    
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.UsingCommandData)]
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class ViewBrowser : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Result result = Result.Succeeded;

            try
            {
                ViewFinder vf = new ViewFinder(commandData, ref message);
                vf.ShowDialog();
            }
            catch (Exception ex)
            {
                message = ex.Message;
                result = Result.Failed;
            }
            if (result == Result.Failed)
            {
                TaskDialog td = new TaskDialog("ERROR");

                td.MainInstruction = message;
                td.MainContent = message;
                td.Show();
            }

            return result;
        }
    }

    /// <summary>
    /// O P T I O N   V I E W   M A K E R
    /// </summary>
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.UsingCommandData)]
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class OptionViewMaker : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Result result = Result.Succeeded;

            try
            {
                OptionViewManager optViewMaker = new OptionViewManager(commandData, ref message);
                if (optViewMaker.ProjectHasOptions)
                    optViewMaker.ShowDialog();
                else
                    result = Result.Cancelled;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                result = Result.Failed;
            }
            if (result == Result.Failed)
            {
                TaskDialog td = new TaskDialog("ERROR");

                td.MainInstruction = message;
                td.MainContent = message;
                td.Show();
            }
            return result;
        }
    }


    /// <summary>
    /// Window Handle wrapper
    /// </summary>
    public class WindowHandle : System.Windows.Forms.IWin32Window
    {
        IntPtr _hwnd;

        public WindowHandle(IntPtr h)
        {
            Debug.Assert(IntPtr.Zero != h,
              "expected non-null window handle");

            _hwnd = h;
        }

        public IntPtr Handle
        {
            get
            {
                return _hwnd;
            }
        }
    }

}
