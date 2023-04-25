using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPITrainingSelection321
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;


            IList<Reference> selectedRef = uidoc.Selection.PickObjects(ObjectType.Element, "Выберите элементы");
            var lengthList = new List<double>();
            foreach (var selectedElement in selectedRef)
            {
                var element = doc.GetElement(selectedElement);


                if (element is Pipe)
                {
                    Parameter lengthParameter = element.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH);
                    if (lengthParameter.StorageType == StorageType.Double)
                    {
                        double length = UnitUtils.ConvertFromInternalUnits(lengthParameter.AsDouble(), UnitTypeId.Meters);
                        lengthList.Add(length);
                    }

                }
            }
            TaskDialog.Show("Length", $"Длина выбранных труб: {lengthList.Sum()} m");
            return Result.Succeeded;
        }
    }
}
