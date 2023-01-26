using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace ZGF.Revit
{
    public class exContent_LoadableFamily
    {
        public static ElementType InsertLoadableFamily(UIDocument uiDoc, Document revitDoc, Revit_Content_EXTERNAL_Item externalDataRecord)
        {
            //FamilySymbol returnFamilySymbol = null;
            ElementType returnFamilyElmentType = null;

            // LoadFamilyOptions fires only if family is already loaded AND had been edited:
            LoadFamilyOptionForContentBrowser lfo = new LoadFamilyOptionForContentBrowser();
            lfo.CurrentFamilyName = Path.GetFileNameWithoutExtension(externalDataRecord.FileName);

            FileInfo typeCatalogFile = externalDataRecord.GetTypeCatalog_FileInfo();

            Dictionary<string, List<CellDataItem>> selectedTypes = null;
            bool hasTypeCatalog = false;
            if ((null != typeCatalogFile) && (typeCatalogFile.Exists))
            {
                ContentTypeSelector ts = new ContentTypeSelector(typeCatalogFile.FullName);

                // get main window handle:
                HwndSource hwndSource = HwndSource.FromHwnd(uiDoc.Application.MainWindowHandle);
                Window wdw = hwndSource.RootVisual as Window;
                ts.Owner = wdw;

                ts.ShowDialog();


                if (!ts.ReturnValue)
                {
                    ts.Close();
                    return null; // <--User presses Cancel in Type Selector dialog
                }

                if (ts.SelectedRecordCount > 0)
                {
                    selectedTypes = ts.SelectedTypeItems;
                    hasTypeCatalog = true;
                }
                else
                {
                    ts.Close();
                    return null; //<--User selected nothing
                }

                ts.Close();
            }

            // if multiple TYPE items selected, then load them all but DON'T initiate PLACE
            if (hasTypeCatalog)
            {
                //ElementType famSymbToPlace = null;

                //if (selectedTypes.Count > 1)  // SEVERAL TYPES SELECTED
                //{
                // This API call results in the family being reloaded multiple times.
                // https://forums.autodesk.com/t5/revit-api-forum/reloading-multiple-family-types/td-p/6452270

                //List<ElementType> famSymbs = new List<ElementType>();
                
                Dictionary<string, FamilySymbol> familyTypesAlreadyCreatedInModel = new Dictionary<string, FamilySymbol>();
                List<string> theKeys = selectedTypes.Keys.ToList();

                using (Transaction trans = new Transaction(revitDoc, "Load family with " + selectedTypes.Count.ToString() + " types"))
                {
                    trans.Start();
                    // The 'List' represents one row of data in a Type Catalog. Each item 
                    // is single 'cellDataItem' representing the data contained in one cell.


                    FamilySymbol fs = null;

                    for (int i = 0; i < theKeys.Count; i++)
                    {
                        
                        string typeName = theKeys[i];

                        if (i > 0) // Remaining of the selected types from catalog
                        {
                            // Duplicate the first loaded parameter to make others in the list:
                            if (!familyTypesAlreadyCreatedInModel.ContainsKey(typeName))
                                familyTypesAlreadyCreatedInModel.Add(typeName, FamilyHelper.DuplicateFamilyType(revitDoc, typeName, fs, selectedTypes[typeName]));
                        }
                        else // First type in the list gets created when the family is loaded. Others (above) are duplicated from this one
                        {
                            // #TODO: What if the family has already been loaded?
                            string familyName = Path.GetFileNameWithoutExtension(externalDataRecord.FileName);

                            if (FamilyHelper.FamilyExistsInProject(revitDoc, familyName, externalDataRecord.CategoryName))
                            {
                                // If the familySymbol exists in doc then RETURN it
                                var famRef = GetFamilyRefByName(revitDoc, familyName);
                                ISet<ElementId> typeIds = famRef.GetFamilySymbolIds();

                                // Get Family types (FamilySymbols) already defined:
                                foreach (ElementId typeId in typeIds)
                                {
                                    FamilySymbol et = revitDoc.GetElement(typeId) as FamilySymbol;
                                    if (!familyTypesAlreadyCreatedInModel.ContainsKey(typeName)) 
                                        familyTypesAlreadyCreatedInModel.Add(et.Name, et);
                                }

                                fs = revitDoc.GetElement(GetFamilyRefByName(revitDoc, familyName).GetFamilySymbolIds().First()) as FamilySymbol;

                                // Otherwise, get the FIRST family symbol and duplicate it:
                                if (!familyTypesAlreadyCreatedInModel.ContainsKey(typeName))                                
                                {                                    
                                    FamilyHelper.DuplicateFamilyType(revitDoc, typeName, fs, selectedTypes[typeName]);
                                    familyTypesAlreadyCreatedInModel.Add(typeName, fs);
                                }
                            }
                            else // Family not already loaded
                            {
                                revitDoc.LoadFamilySymbol(externalDataRecord.FullPathName, typeName, lfo, out fs);
                                if (!familyTypesAlreadyCreatedInModel.ContainsKey(typeName)) 
                                    familyTypesAlreadyCreatedInModel.Add(typeName, fs);
                                //if (null != fs) famSymbs.Add(fs as ElementType);
                            }
                        }

                    }



                    // IS THIS DOING ANYTHING?
                    // User chose not to reload the family when re-importing, so no symbols were collected:
                    if (!lfo.ReloadFamily)
                    {
                        Family fam = GetFamilyRefByName(revitDoc, lfo.CurrentFamilyName);

                        List<ElementId> fss = fam.GetFamilySymbolIds().ToList<ElementId>();
                        //famSymbs.Add(revitDoc.GetElement(fss[0]) as ElementType);
                    }

                    trans.Commit();

                    // Pick the first type to place:
                    //if (famSymbs.Count > 0) returnFamilyElmentType = famSymbs[0];


                }
                //}
                //else // selectedTypes.Count == 0 - ONE TYPE SELECTED
                //{
                //    FamilySymbol fs = null;
                //    using (Transaction trans = new Transaction(revitDoc, "Load family"))
                //    {
                //        trans.Start();
                //        string[] typeName = selectedTypes.Keys.ToArray<string>();
                //        // The Key is the TypeName
                //        revitDoc.LoadFamilySymbol(externalDataRecord.FullPathName, typeName[0], lfo, out fs);
                //        trans.Commit();
                //    }
                //    if (null != fs)
                //        famSymbToPlace = fs as ElementType; //returnFamilySymbol.GetValidTypes
                //    else
                //    {
                //        // Family is already loaded. Get the reference to the loaded Family:
                //        Family fam = GetFamilyRefByName(revitDoc, lfo.CurrentFamilyName);

                //        List<ElementId> fss = fam.GetFamilySymbolIds().ToList<ElementId>();
                //        famSymbToPlace = revitDoc.GetElement(fss[0]) as ElementType;
                //    }
                //}

                // Return the first of the selected family types:
                returnFamilyElmentType = familyTypesAlreadyCreatedInModel[theKeys[0]];
            }
            else // NO TYPE CATALOG
            {
                Family f = null;

                using (Transaction trans = new Transaction(revitDoc, "Load family"))
                {
                    trans.Start();
                    revitDoc.LoadFamily(externalDataRecord.FullPathName, lfo, out f);
                    trans.Commit();

                    // if (f == NULL) then the family is loaded but hasn't changed. no need to prompt user.
                    //      If the family HAS been changed, then LoadFamilyOptions (LoadFamilyOptionForContentBrowser) fires:
                    if (null == f)
                    {
                        // Family is already loaded. Get the reference to the loaded Family:
                        f = GetFamilyRefByName(revitDoc, lfo.CurrentFamilyName);
                    }

                    // Select first Type in the Family (how to find last type used?)
                    // Select All instances, use type if greatest ElementID

                    //ISet<ElementId> famSymbs =  f.GetFamilySymbolIds();
                    List<ElementId> fss = f.GetFamilySymbolIds().ToList<ElementId>();
                    returnFamilyElmentType = revitDoc.GetElement(fss[0]) as ElementType; // First type in the family                      
                }
            }

            // Return the element type to the caller:
            return returnFamilyElmentType;

        }

        /// <summary>
        /// Gets a reference to a specific Family element or returns null.
        /// </summary>
        /// <param name="revitDoc"></param>
        /// <param name="familyName"></param>
        /// <returns></returns>
        private static Family GetFamilyRefByName(Document revitDoc, string familyName)
        {
            List<Element> el = new FilteredElementCollector(revitDoc)
                .OfClass(typeof(Family))
                .Where(n => n.Name.Equals(familyName, StringComparison.CurrentCultureIgnoreCase))
                .ToList();

            return (el.Count > 0)
                ? el[0] as Family
                : null;
        }

    }

    /// <summary>
    /// Used to configure LoadFamily function
    /// </summary>
    public class LoadFamilyOptionForContentBrowser : IFamilyLoadOptions
    {
        private bool _ReloadFamily = true;

        public bool ReloadFamily { get { return _ReloadFamily; } set { _ReloadFamily = value; } }

        public string CurrentFamilyName { get; set; }

        private TaskDialogResult PromptToOverwriteFamily(string FamilyName)
        {
            TaskDialog td = new TaskDialog("ZGF Family Finder");
            td.TitleAutoPrefix = false;
            td.MainInstruction = "Family \"" + FamilyName + "\" is already loaded.";
            td.MainContent = "If it has been changed in this project, reloading it will overwrite those changes."
                + "\nDo you want to reload it?";
            td.CommonButtons = TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No;
            //td.CommonButtons = TaskDialogCommonButtons.No;
            td.DefaultButton = TaskDialogResult.No;
            TaskDialogResult returnVal = td.Show();

            _ReloadFamily = returnVal == TaskDialogResult.Yes;

            return returnVal;
        }

        public bool OnFamilyFound(bool familyInUse, out bool overWriteParameterValues)
        {
            // Never overwrite Parameter values
            overWriteParameterValues = false;

            if (familyInUse)
            {
                Debug.WriteLine("Select family is already loaded into the current Project.");

                TaskDialogResult tdResult = PromptToOverwriteFamily(CurrentFamilyName);

                // Yes, reload family. 
                return (tdResult == TaskDialogResult.Yes);
            }
            else
            {
                Debug.WriteLine("Select family is already loaded into the current Project.");
                return true;
            }

        }


        public bool OnSharedFamilyFound(Family family, bool familyInUse, out FamilySource source, out bool overWriteParamterValues)
        {
            // Never overwrite parameter values:
            overWriteParamterValues = false;
            source = FamilySource.Family;

            if (familyInUse)
            {
                TaskDialogResult tdResult = PromptToOverwriteFamily(CurrentFamilyName);
                return (tdResult == TaskDialogResult.Yes);
            }
            else
            {
                return true;
            }
        }


    }
}
