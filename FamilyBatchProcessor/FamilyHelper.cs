using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;

namespace ZGF.Revit
{
    class FamilyHelper
    {


        /// <summary>
        /// Returns a Collection of families in the model that failed to open. The assumption is that those families are corrupt.
        /// </summary>
        /// <param name="revitDoc"></param>
        /// <returns></returns>
        public static List<Family> CheckEditableFamiliesForCorruption(Document revitDoc, bool doQuickScan, out int totalFamiliesProcessedCount)
        {
            List<Family> wonkyFamilies = new List<Family>();

            // Ignore In-place and system families

            // Get a the editable Families in the current project:
            FilteredElementCollector familiesToCheck = new FilteredElementCollector(revitDoc)
                .OfClass(typeof(Family));

            int theCount = 0;

            // Using a failure preprocessor without transaction binding:
            // https://forums.autodesk.com/t5/revit-api-forum/delete-warning-worksharingfailures-duplicatenameschanged-when/td-p/6873330

            using (TransactionGroup tGroup = new TransactionGroup(revitDoc, "Scan for corrupt families"))
            {
                tGroup.Start();

                foreach (Element e in familiesToCheck)
                {
                    Family f = e as Family;
                    string originalFamilyName = f.Name;
                    string tmp_suffix = DateTime.Now.ToShortTimeString().Replace(":", string.Empty);

                    if (null != f && f.IsEditable && !f.IsInPlace)
                    {
                        try
                        {
                            revitDoc.Application.WriteJournalComment("[ZGF] Checking family: " + f.FamilyCategory.Name + " :: " + f.Name, true);
                            // If Revit version is 2020 or newer, try to rename the family and then restore the the old name:
                            using (Transaction t1 = new Transaction(revitDoc, "Rename family"))
                            {
                                t1.Start();
                                f.Name = originalFamilyName + "_" + tmp_suffix;
                                t1.Commit();
                            }

                            using (Transaction t2 = new Transaction(revitDoc, "Restore family name"))
                            {
                                t2.Start();
                                f.Name = originalFamilyName;
                                t2.Commit();
                            }

                            // Open the family. If it can be successfully opened than it is likely not corrupt. 
                            if (!doQuickScan)
                            {
                                Document famDoc = revitDoc.EditFamily(f);
                                Debug.WriteLine(theCount.ToString() + " :: " + f.FamilyCategory.Name + " :: " + f.Name);
                                famDoc.Close(false);
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                            revitDoc.Application.WriteJournalComment("[ZGF] Possible corruption discovered while scanning families: " + f.Name, true);
                            wonkyFamilies.Add(f);
                        }
                        theCount++;
                    }
                }

                tGroup.Assimilate();
            }


            totalFamiliesProcessedCount = theCount;

            return wonkyFamilies;
        }

        /// <summary>
        /// Does the FAMILY (in a certain category) exist in the model?
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="familyName"></param>
        /// <param name="familyCategoryName"></param>
        /// <returns></returns>
        public static bool FamilyExistsInProject(Document doc, string familyName, string familyCategoryName)
        {
            IEnumerable<Family> fam = new FilteredElementCollector(doc)
                .OfClass(typeof(Family))
                .Cast<Family>()
                .Where(f => f.Name.Equals(familyName, StringComparison.InvariantCultureIgnoreCase))
                .Where(f => f.FamilyCategory.Name.Equals(familyCategoryName, StringComparison.InvariantCultureIgnoreCase));

            return fam.Count() > 0;

        }

        /// <summary>
        /// Does family symbol (Family Type name) exist in specified family?
        /// </summary>
        /// <param name="family"></param>
        /// <param name="familySymbolName"></param>
        /// <returns></returns>
        public static bool FamilySymbolExists(Family family, string familySymbolName)
        {
            ISet<ElementId> famSymIds = family.GetFamilySymbolIds();

//#if DEBUG
            foreach (ElementId famSymId in famSymIds)
            {
                FamilySymbol fSymb = family.Document.GetElement(famSymId) as FamilySymbol;
                if (fSymb != null && fSymb.Name.Equals(familySymbolName, StringComparison.OrdinalIgnoreCase))
                    return true;
                //{
                    
                //    Debug.WriteLine(famSymId + " - " + fSymb.Name);
                //}
                //else
                //{
                //    Debug.WriteLine(famSymId + " = null");
                //}
            }
            //#endif
            return false;

            FamilySymbol fs = famSymIds.Where(f => family.Document.GetElement(f).Name.Equals(familySymbolName, StringComparison.CurrentCultureIgnoreCase)) as FamilySymbol;

            return null != fs;              
        }

        public static FamilySymbol DuplicateFamilyType(Document doc, string newFamilyTypeName, FamilySymbol familySymbolToDuplicate, List<CellDataItem> itemTypeCatalogData)
        {
            // Parent Family:
            Family fam = familySymbolToDuplicate.Family;

            if (FamilySymbolExists(fam, newFamilyTypeName)) return null;

            // New family symbol using Duplicate:
            // #TODO: What if FS already exists?
            FamilySymbol newFamSym = familySymbolToDuplicate.Duplicate(newFamilyTypeName) as FamilySymbol;
            // Get all of the Parameters:
            ParameterSet allParams = newFamSym.Parameters;

            foreach (Parameter p in allParams)
            {
                CellDataItem cdi = itemTypeCatalogData.FirstOrDefault(x => x.ParameterName.Equals(p.Definition.Name, StringComparison.CurrentCultureIgnoreCase));
                if (null != cdi)
                {
                    string valStr = cdi.ValueString;

                    //if (cdi.DataType.Equals("length", StringComparison.OrdinalIgnoreCase))
                    //{
                    if (null != cdi.UnitType)
                    {
                        // See class: UnitTypeId
                        switch (cdi.UnitType.ToLower())
                        {
                            case "inches":
                                valStr = valStr + "\"";
                                break;
                            case "yards":
                                valStr = valStr + "yd";
                                break;
                            case "millimeters":
                                valStr = valStr + "mm";
                                break;
                            case "centimeters":
                                valStr = valStr + "cm";
                                break;
                            case "meters":
                                valStr = valStr + "m";
                                break;
                            default: // Feet

                                break;
                        }
                    }

                    //}
                    // #TODO: This needs to be much more thorough... Need to check for all unit types.

                    p.SetValueString(valStr);
                }
            }

            return newFamSym;
        }

    }
    [Transaction(TransactionMode.Manual)]
    [Journaling(JournalingMode.UsingCommandData)]
    public class BatchFamilyUpdater : IExternalCommand
    {


        private DirectoryInfo _folderToProcess = new DirectoryInfo(@"C:\Temp\Medical Equipment 2022");



        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {


            if (_folderToProcess.Exists)
            {
                FileInfo[] files = _folderToProcess.GetFiles("*.rfa", SearchOption.TopDirectoryOnly);



                foreach (FileInfo file in files)
                {
                    Document doc = commandData.Application.Application.OpenDocumentFile(file.FullName);
                    if (doc.IsFamilyDocument)
                    {
                        using (Transaction t = new Transaction(doc, "Change Family category"))
                        {


                            Family fam = doc.OwnerFamily;
                            Category category = fam.FamilyCategory;


                            try
                            {
                                t.Start();
                                Debug.WriteLine("Category: " + category.Name);

                                if (category.Name.StartsWith("Specialty Equipment"))
                                {
                                    if (category.Name.Contains("Tags"))
                                    {
                                        fam.FamilyCategory = Category.GetCategory(doc, BuiltInCategory.OST_MedicalEquipmentTags);
                                    }
                                    else
                                    {
                                        fam.FamilyCategory = Category.GetCategory(doc, BuiltInCategory.OST_MedicalEquipment);
                                    }
                                }
                                else
                                {
                                    Debug.WriteLine(file.FullName + "\r\tis " + category.Name);
                                }

                                t.Commit();

                                doc.Close(true);
                            }
                            catch (Exception ex)
                            {
                                doc.Close(false); //<--Log this
                                Debug.WriteLine(file.FullName + "\n\t" + ex.Message);
                            }

                        }
                    }
                }
            }

            Debug.WriteLine("Done!");

            return Result.Succeeded;
        }
    }


    public class LoadableFamilyCheck
    {
        private void Init(Family FamilyToCheck)
        {
            TheFamily = FamilyToCheck;
            CanBeEdited = FamilyCanBeEdited.Unknown;
            FileSizeInBytes = 0;
        }

        public LoadableFamilyCheck(Family theFamily)
        {
            Init(theFamily);
            TypesNames = null;
        }



        public LoadableFamilyCheck(Family theFamily, bool getTypeNames)
        {
            Init(theFamily);
            if (getTypeNames) GetTypeNames();
        }

        public Family TheFamily { get; set; }

        public string CategoryName { get { return TheFamily.FamilyCategory.Name; } }

        public bool HasLargeSketches { get { return TheFamily.HasLargeSketches(); } }

        public int TypesCount { get { return TheFamily.GetFamilySymbolIds().Count; } }
        public string[] TypesNames { get; private set; }

        private void GetTypeNames()
        {
            List<string> names = new List<string>();

            try
            {
                Document doc = TheFamily.Document;
                ISet<ElementId> typeIds = TheFamily.GetFamilySymbolIds();
                foreach (var item in typeIds)
                {
                    Element e = doc.GetElement(item);
                    names.Add(e.Name);
                }
            }
            catch
            {
                CanBeEdited = FamilyCanBeEdited.No;
            }
            finally
            {
                TypesNames = names.ToArray();
            }
        }

        public bool IsParametric { get { return TheFamily.IsParametric; } }

        public ElementId Id { get { return TheFamily.Id; } }

        public string FamilyHostingType { get { return TheFamily.FamilyPlacementType.ToString(); } }

        public long FileSizeInBytes { get; private set; }

        public FamilyCanBeEdited CanBeEdited { get; private set; }

        public bool CheckEditFamily()
        {
            try
            {
                Document famDoc = TheFamily.Document.EditFamily(TheFamily);
                CanBeEdited = FamilyCanBeEdited.Yes;

                DirectoryInfo tmpFolder = new DirectoryInfo(Path.GetTempPath());
                FileInfo tmpFam = new FileInfo(Path.Combine(tmpFolder.FullName, "__tmpRvt.rfa"));

                // Check Size:
                try
                {
                    famDoc.SaveAs(tmpFam.FullName);
                    FileSizeInBytes = tmpFam.Length;
                }
                finally
                {
                    famDoc.Close(false);
                    tmpFam.Delete();
                    tmpFam = null;
                }

                return true;
            }
            catch
            {
                CanBeEdited = FamilyCanBeEdited.No;
                return false;
            }
        }

        public enum FamilyCanBeEdited
        {
            Unknown = -1,
            No = 0,
            Yes = 1
        }



    }

    //public class FamilyExportWarningsProcessor : IFailuresPreprocessor
    //{
    //    // https://thebuildingcoder.typepad.com/blog/2016/09/warning-swallower-and-roomedit3d-viewer-extension.html
    //    // https://thebuildingcoder.typepad.com/blog/2010/04/failure-api.html


    //    public Autodesk.Revit.DB.FailureProcessingResult PreprocessFailures(Autodesk.Revit.DB.FailuresAccessor fa)
    //    {

    //        IList<FailureMessageAccessor> fails = fa.GetFailureMessages();


    //        fa.DeleteAllWarnings();
    //        fa.ResolveFailures(fails);

    //        //foreach (FailureMessageAccessor f in fails)
    //        //{
    //        //    FailureSeverity fs = f.GetSeverity();



    //        //    if (fs == FailureSeverity.Warning)
    //        //    {
    //        //        fa.DeleteWarning(f);
    //        //    }
    //        //    else
    //        //    {
    //        //        fa.ResolveFailure(f);
    //        //        return FailureProcessingResult.ProceedWithCommit;
    //        //    }


    //        //}

    //        //return FailureProcessingResult.Continue;
    //    }

    //}
}
