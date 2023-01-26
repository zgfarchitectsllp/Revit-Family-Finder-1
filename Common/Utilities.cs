using Autodesk.Revit.DB;
using System;
using System.Drawing;
using System.Linq;

namespace ZGF.Revit
{
    class Utilities
    {

        public static Autodesk.Revit.DB.View GetDraftingViewByName(Document theDocument, string viewName, bool CreateNewView)
        {            
            BuiltInParameter bip = BuiltInParameter.VIEW_NAME;
            ParameterValueProvider pvp = new ParameterValueProvider(new ElementId(bip));

            FilterStringRuleEvaluator fnrvStr = new FilterStringContains();

            // if ver < 2022
            //FilterRule paramFr = new FilterStringRule(pvp, fnrvStr, viewName, false);
            // else
            FilterRule paramFr = new FilterStringRule(pvp, fnrvStr, viewName); 

            ElementParameterFilter epf = new ElementParameterFilter(paramFr);

            FilteredElementCollector collector = new FilteredElementCollector(theDocument);
            collector.OfClass(typeof(Autodesk.Revit.DB.ViewDrafting)).WherePasses(epf);

            Autodesk.Revit.DB.View theReturnView = collector.FirstElement() as Autodesk.Revit.DB.View;
            if (CreateNewView)
            {
                if (null == theReturnView)
                {
                    Transaction tr = new Transaction(theDocument, "Create New Title Drafting View");
                    tr.Start();
                    



                    // TODO: Make this a HELPER FUNCTION
                    ViewFamilyType vft = new FilteredElementCollector(theDocument)
                       .OfClass(typeof(ViewFamilyType))
                       .Cast<ViewFamilyType>()
                       .FirstOrDefault<ViewFamilyType>(x => ViewFamily.Drafting == x.ViewFamily);
                    theReturnView = ViewDrafting.Create(theDocument, vft.Id);

                    theReturnView.Name = viewName;
                    //theReturnView.Scale = 4;
                    tr.Commit();
                }
            }


            return theReturnView;


        }

        /// <summary>
        /// Matches a string against a delimited array of strings. If each of the strings in the delimited array are contained within the master string the result is true.
        /// </summary>
        /// <param name="stringToMatch"></param>
        /// <param name="stringOfWords"></param>
        /// <param name="delimiters"></param>
        /// <returns></returns>
        public static bool StringArrayMatch(string stringToMatch, string stringOfWords, char[] delimiters)
        {
            stringToMatch = stringToMatch.ToLower();
            string literal = stringOfWords.ToLower();
            string[] words = literal.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

            int matches = 0;

            foreach (string s in words)
            {
                if (literal == string.Empty) continue;

                if ((stringToMatch.Contains(literal)) || (stringToMatch.Contains(s)))
                    matches++;
                else
                    matches--;
            }

            return (matches == words.Length);
        }
        
        /// <summary>
        /// Returns the first Family Instance from a type reference
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="familySymbol"></param>
        /// <returns></returns>
        public static FamilyInstance FamilySymbolToFamilyInstance(Document doc, FamilySymbol familySymbol)
        {
            // built-in parameter storing this FamilyInstance's type element id:

            BuiltInParameter bip = BuiltInParameter.ELEM_TYPE_PARAM;

            ParameterValueProvider provider= new ParameterValueProvider(
                new ElementId(bip));

            FilterNumericRuleEvaluator evaluator = new FilterNumericEquals();

            FilterRule rule = new FilterElementIdRule(provider, evaluator, familySymbol.Id);

            ElementParameterFilter filter = new ElementParameterFilter(rule);

            FilteredElementCollector collector = new FilteredElementCollector(doc)
                .OfClass(typeof(FamilyInstance))
                .WherePasses(filter);

            return collector.FirstElement() as FamilyInstance;
        }

        /// <summary>
        /// Returns the first Annotation Symbol Instance from a type reference
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="annoSymbol"></param>
        /// <returns></returns>
        public static AnnotationSymbol AnnotationSymbolToFamilyInstance(Document doc, FamilySymbol annoSymbol)
        {
            // built-in parameter storing this FamilyInstance's type element id:

            BuiltInParameter bip = BuiltInParameter.ELEM_TYPE_PARAM;

            ParameterValueProvider provider = new ParameterValueProvider(
                new ElementId(bip));

            FilterNumericRuleEvaluator evaluator = new FilterNumericEquals();

            FilterRule rule = new FilterElementIdRule(provider, evaluator, annoSymbol.Id);

            ElementParameterFilter filter = new ElementParameterFilter(rule);

            FilteredElementCollector collector = new FilteredElementCollector(doc)
                .OfClass(typeof(FamilyInstance))
                .WherePasses(filter);

            return collector.FirstElement() as AnnotationSymbol;
        }

        /// <summary>
        /// Returns a Rectangle representing the bounds of the screen of the specified control
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        public static System.Drawing.Rectangle GetRevitScreenSize(Autodesk.Revit.UI.UIApplication uiApp)
        {
            int top = uiApp.MainWindowExtents.Top;
            int left = uiApp.MainWindowExtents.Left;

            int w =  uiApp.MainWindowExtents.Right = uiApp.MainWindowExtents.Left;
            int h = uiApp.MainWindowExtents.Top = uiApp.MainWindowExtents.Bottom;

            return new System.Drawing.Rectangle(left, top, w, h);
        }

       

    }

}
