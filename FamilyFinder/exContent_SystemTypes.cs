using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace ZGF.Revit
{
    public class exContent_SystemTypes
    {
        public static ElementType InsertSystemType(Document revitTargetDoc, Revit_Content_EXTERNAL_Item externalDataRecord)
        {
            ElementType return_ElementType = null;

            FileInfo _sourceFileinfo = new FileInfo( externalDataRecord.FullPathName );
            Category _category = revitTargetDoc.Settings.Categories.get_Item(externalDataRecord.CategoryName);
            
            // If category is null, then bail...
            if ( (null == _category) || (!"System Family".Equals(externalDataRecord.FileType) ))
                { return null; }


            // Does type already exist in Model? If so, just use the one in the model...
            ElementCategoryFilter removeNullCagetory = new ElementCategoryFilter(ElementId.InvalidElementId, true);
            ElementClassFilter elementTypeClassFilter = new ElementClassFilter(typeof(ElementType));
            LogicalAndFilter TypesAndNoNullCagegoryFilter = new LogicalAndFilter(removeNullCagetory, elementTypeClassFilter);

            // TODO: faster using QUICK FILTERS ???
            List<ElementType> elementTypes = new FilteredElementCollector(revitTargetDoc)
                .WherePasses(TypesAndNoNullCagegoryFilter)
                //.WherePasses(removeNullCagetory)
                //.WherePasses(elementTypeClassFilter)
                //.OfClass(typeof(ElementType))  // (typeof(ElementType))
                .Cast<ElementType>()
                //.Where(x => null != x.Category) // <--See WherePasses(...                
                .Where(t => t.Category.Name.Equals(externalDataRecord.CategoryName))
                .Where(t => t.Name.Equals(externalDataRecord.FileName))
                .ToList<ElementType>();

            // Otherwise, open the container and get the wall type...
            if (elementTypes.Count > 0)
            {
                return_ElementType = elementTypes[0];
            }
            else
            {
                // TODO: should copy the wall local first and cache it? 

                // Open the drawing with the content
                if (_sourceFileinfo.Exists)
                {
                    Document sourceDoc = revitTargetDoc.Application.OpenDocumentFile(externalDataRecord.FullPathName);
                    ICollection<ElementId> copiedIDs;

                    // Look for TYPE in SOURCE FILE
                    elementTypes = new FilteredElementCollector(sourceDoc)
                        .WherePasses(TypesAndNoNullCagegoryFilter)
                        //.OfClass(typeof(ElementType))  // (typeof(ElementType))
                        .Cast<ElementType>()
                        //.Where(x => null != x.Category)
                        .Where(t => t.Category.Name.Equals(externalDataRecord.CategoryName))
                        .Where(t => t.Name.Equals(externalDataRecord.FileName))
                        .ToList<ElementType>();

                    // Does type already exist in SOURCE FILE?
                    if (elementTypes.Count > 0)
                    {
                        // Copy to current project
                        using (Transaction targetDocTrans = new Transaction(revitTargetDoc, "Load type"))
                        {
                            targetDocTrans.Start();

                            // Suppress the duplicate types dialog
                            CopyPasteOptions options = new CopyPasteOptions();
                            options.SetDuplicateTypeNamesHandler(new HideAndAcceptDuplicateTypeNamesHandler());

                            List<ElementId> idsToCopy = elementTypes
                                .AsEnumerable<ElementType>()
                                .ToList<ElementType>()
                                .ConvertAll<ElementId>(ElementTypeConvertToElementId);

                            // Copy the library elements into the current doc and return a list of the IDs:
                            copiedIDs = ElementTransformUtils.CopyElements(sourceDoc, idsToCopy, revitTargetDoc, null, options);

                            FailureHandlingOptions failureOpts = targetDocTrans.GetFailureHandlingOptions();
                            failureOpts.SetFailuresPreprocessor(new HidePasteDuplicateTypesPreprocessor());

                            targetDocTrans.Commit(failureOpts);
                            
                            sourceDoc.Close(false);

                            ElementId[] returnIds = copiedIDs.ToArray<ElementId>();
                            return_ElementType = (returnIds.Length > 0)
                                ? revitTargetDoc.GetElement(returnIds[0]) as ElementType
                                : null;
                        }
                    }
                }
            }
            return return_ElementType;
        }

        /// <summary>
        /// Converter delegate for conversion of collections
        /// </summary>
        /// <param name="view">The ElementType.</param>
        /// <returns>The ElementType's ID.</returns>
        public static ElementId ElementTypeConvertToElementId(ElementType elementType)
        {
            return elementType.Id;
        }
    }
   
}
