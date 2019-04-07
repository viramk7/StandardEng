using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImport
{
    public class Program
    {
        static void Main(string[] args)
        {

            StandardEngEntities db = new StandardEngEntities();

            var package = new ExcelPackage();


            using (var stream = File.OpenRead("D:\\Data\\rushin\\Machine Data\\ROTARY(810 AND 813) WITH HSN & TAX.xlsx"))
            {
                package.Load(stream);
            }

            ExcelWorksheet workSheet = package.Workbook.Worksheets[1];
            workSheet.TrimLastEmptyRows();

            IEnumerable<tblMachine> listFromExcel = GetDataFromExcelStockCheck(workSheet, true);

            //try
            //{
            //    //using (CosmosEntities context = BaseContext.GetDbContext())
            //    //{
            //    foreach (var modelItem in listFromExcel)
            //    {


            //        // StandardEngEntities db = new StandardEngEntities();

            //        db.tblMachine.Add(modelItem);






            //    }

            //    db.SaveChanges();

            //}

            //catch (DbEntityValidationException dbEx)
            //{
            //    string messages = String.Empty;
            //    foreach (DbEntityValidationResult entityErr in dbEx.EntityValidationErrors)
            //    {
            //        foreach (DbValidationError error in entityErr.ValidationErrors)
            //        {
            //            //Console.WriteLine("Error Property Name {0} : Error Message: {1}",
            //            //                    error.PropertyName, error.ErrorMessage);
            //            messages = messages + "<br/>" + string.Join("<br/>", error.ErrorMessage);

            //        }
            //    }

            //}

        }



        public static IEnumerable<tblMachineModels> GetDataFromExcelStockCheck(ExcelWorksheet workSheet, bool firstRowHeader)
        {
            IList<tblMachineModels> tblItemModel = new List<tblMachineModels>();

            if (workSheet != null)
            {
                Dictionary<string, int> header = new Dictionary<string, int>();

                for (int rowIndex = workSheet.Dimension.Start.Row; rowIndex <= workSheet.Dimension.End.Row; rowIndex++)
                {

                    //Assume the first row is the header. Then use the column match ups by name to determine the index.
                    //This will allow you to have the order of the columns change without any affect.

                    if (rowIndex == 1 && firstRowHeader)
                    {
                        header = ExcelHelper.GetExcelHeader(workSheet, rowIndex);
                    }
                    else
                    {
                        tblItemModel.Add(new tblMachineModels
                        {
                            ProductValue = ExcelHelper.ParseWorksheetValue(workSheet, header, rowIndex, "Product Value"),
                            AlternateProductValue = ExcelHelper.ParseWorksheetValue(workSheet, header, rowIndex, "AlternateProductValue"),
                            AlternateProductValue1 = null,
                            //AlternateProductValue1 = ExcelHelper.ParseWorksheetValue(workSheet, header, rowIndex, "AlternateProductValue1"),
                            Description = ExcelHelper.ParseWorksheetValue(workSheet, header, rowIndex, "Description"),
                            IPL = ExcelHelper.ParseWorksheetValue(workSheet, header, rowIndex, "IPL"),
                            HSNCode = ExcelHelper.ParseWorksheetValue(workSheet, header, rowIndex, "HSN CODE"),
                            STDPrice = Convert.ToDecimal(ExcelHelper.ParseDecimalWorksheetValue(workSheet, header, rowIndex, "STD PRICE")),
                            ProductTypeName = ExcelHelper.ParseWorksheetValue(workSheet, header, rowIndex, "ProductTypeName"),
                            MachineTypeId = 3

                        });

                    }
                }
            }

            return tblItemModel;
        }
    }
}
