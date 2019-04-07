using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataImport
{
    public static class ExcelHelper
    {
        public static Dictionary<string, int> GetExcelHeader(ExcelWorksheet workSheet, int rowIndex)
        {
            Dictionary<string, int> header = new Dictionary<string, int>();

            if (workSheet != null)
            {
                for (int columnIndex = workSheet.Dimension.Start.Column; columnIndex <= workSheet.Dimension.End.Column; columnIndex++)
                {
                    if (workSheet.Cells[rowIndex, columnIndex].Value != null)
                    {
                        string columnName = workSheet.Cells[rowIndex, columnIndex].Value.ToString();

                        if (!header.ContainsKey(columnName) && !string.IsNullOrEmpty(columnName))
                        {
                            header.Add(columnName, columnIndex);
                        }
                    }
                }
            }

            return header;
        }

        public static string ParseWorksheetValue(ExcelWorksheet workSheet, Dictionary<string, int> header, int rowIndex, string columnName)
        {
            string value = string.Empty;
            var newDictionary = header.ToDictionary(x => x.Key.Trim(), x => x.Value);
            int? columnIndex = newDictionary.ContainsKey(columnName) ? newDictionary[columnName] : (int?)null;

            if (workSheet != null && columnIndex != null && workSheet.Cells[rowIndex, columnIndex.Value].Value != null)
            {
                value = workSheet.Cells[rowIndex, columnIndex.Value].Value.ToString();
            }

            return value;
        }

        public static Decimal ParseDecimalWorksheetValue(ExcelWorksheet workSheet, Dictionary<string, int> header, int rowIndex, string columnName)
        {
            decimal value = 0;
            int? columnIndex = header.ContainsKey(columnName) ? header[columnName] : (int?)null;

            if (workSheet != null && columnIndex != null && workSheet.Cells[rowIndex, columnIndex.Value].Value != null)
            {
                value = Convert.ToDecimal(workSheet.Cells[rowIndex, columnIndex.Value].Value);
            }

            return value;
        }

        public static int ParseIntegerWorksheetValue(ExcelWorksheet workSheet, Dictionary<string, int> header, int rowIndex, string columnName)
        {
            int value = 0;
            int? columnIndex = header.ContainsKey(columnName) ? header[columnName] : (int?)null;

            if (workSheet != null && columnIndex != null && workSheet.Cells[rowIndex, columnIndex.Value].Value != null)
            {
                value = Convert.ToInt32(workSheet.Cells[rowIndex, columnIndex.Value].Value);
            }

            return value;
        }

        public static void TrimLastEmptyRows(this ExcelWorksheet worksheet)
        {
            while (worksheet.IsLastRowEmpty())
                worksheet.DeleteRow(worksheet.Dimension.End.Row);
        }

        public static bool IsLastRowEmpty(this ExcelWorksheet worksheet)
        {
            var empties = new List<bool>();

            for (int i = 1; i <= worksheet.Dimension.End.Column; i++)
            {
                var rowEmpty = worksheet.Cells[worksheet.Dimension.End.Row, i].Value == null ? true : false;
                empties.Add(rowEmpty);
            }

            return empties.All(e => e);
        }
    }
}