using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace ZGF.Revit
{
    public class ExternalContentHelpers
    {
        /// <summary>
        /// Returns a Sorted Dictionary of categories. Key = Category Name; Value = True or False 
        /// for use with Family Finder checked category filter
        /// </summary>
        /// <param name="activeDocument"></param>
        /// <param name="includeBound">Includes Bound categories resulting in a much longer list</param>
        /// <returns></returns>
        public static SortedDictionary<string, CategoryTreeItem> AllCategories(Document activeDocument, bool includeBound)
        {
            SortedDictionary<string, CategoryTreeItem> outCategories = new SortedDictionary<string, CategoryTreeItem>();

            Document doc = activeDocument;
            Categories categories = doc.Settings.Categories;

            foreach (Category c in categories)
            {
                if (includeBound)
                {
                    outCategories.Add(c.Name, new CategoryTreeItem(c.Name));
                }
                else
                    if (c.AllowsBoundParameters) outCategories.Add(c.Name, new CategoryTreeItem(c.Name));
            }
            return outCategories;
        }
    }

   

    public class CategoryTreeItem
    {
        CategoryItemIntOrExt _storageType = CategoryItemIntOrExt.Unused;
        bool _isChecked = true;

        public CategoryTreeItem(string name)
        {
            this.Name = name;
        }
        public string Name { get; private set; }
        public bool IsChecked
        {
            get { return _isChecked; }
            set { _isChecked = value; }
        }

        public override string ToString()
        {
            return Name;
        }

        public CategoryItemIntOrExt StorageType
        {
            get { return _storageType; }
            set { _storageType = value; }
        }

        public enum CategoryItemIntOrExt
        {
            Unused = 0,
            Internal = 1,
            External = 2
        }

    }

    /// <summary>
    /// A handler to accept duplicate types names created by the copy/paste operation.
    /// </summary>
    class HideAndAcceptDuplicateTypeNamesHandler : IDuplicateTypeNamesHandler
    {
        /// <summary>
        /// Implementation of the IDuplicateTypeNameHandler
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public DuplicateTypeAction OnDuplicateTypeNamesFound(DuplicateTypeNamesHandlerArgs args)
        {
            // Always use duplicate destination types when asked
            return DuplicateTypeAction.UseDestinationTypes;
        }

    }

    /// <summary>
    /// A failure preprocessor to hide the warning about duplicate types being pasted.
    /// </summary>
    class HidePasteDuplicateTypesPreprocessor : IFailuresPreprocessor
    {
        /// <summary>
        /// Implementation of the IFailuresPreprocessor.
        /// </summary>
        /// <param name="failuresAccessor"></param>
        /// <returns></returns>
        public FailureProcessingResult PreprocessFailures(FailuresAccessor failuresAccessor)
        {
            foreach (FailureMessageAccessor failure in failuresAccessor.GetFailureMessages())
            {
                // Delete any "Can't paste duplicate types.  Only non duplicate types will be pasted." warnings
                if (failure.GetFailureDefinitionId() == BuiltInFailures.CopyPasteFailures.CannotCopyDuplicates)
                {
                    failuresAccessor.DeleteWarning(failure);
                }
            }

            // Handle any other errors interactively
            return FailureProcessingResult.Continue;
        }
    }



}
