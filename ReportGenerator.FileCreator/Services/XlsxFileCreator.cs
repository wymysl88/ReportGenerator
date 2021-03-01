using OfficeOpenXml;
using ReportGenerator.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReportGenerator.FileCreator
{
    public class XlsxFileCreator : IFileCreator
    {

        public async Task<byte[]> GetReportDataAsByteArrayAsync(ReportRequest reportRequest)
        {
            var excelData = GetExcelData(reportRequest);

            return await excelData.GetAsByteArrayAsync();
        }

        private ExcelPackage GetExcelData(ReportRequest reportRequest)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var excelPackage = new ExcelPackage();
            var worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");
            var headers = GetHeaders(reportRequest.Data);

            var headerNo = 0;
            foreach (var headerTitle in headers)
            {
                using (var cell = worksheet.Cells[1, ++headerNo])
                {
                    cell.Value = headerTitle;
                    cell.Style.Font.Size = 10;
                    cell.Style.Font.Bold = true;
                }
            }

            var rowNo = 1;
            foreach (var reportRow in reportRequest.Data)
            {
                rowNo++;
                var cellNo = 0;
                foreach (var prop in reportRow)
                {
                    using (var cell = worksheet.Cells[rowNo, ++cellNo])
                    {
                        cell.Value = prop.Value.ToString();
                    }
                }
            }

            worksheet.Protection.IsProtected = false;
            worksheet.Protection.AllowSelectLockedCells = false;

            return excelPackage;
        }

        private List<string> GetHeaders(List<dynamic> reportRows)
        {
            var headers = new List<string>();

            foreach(var property in reportRows[0].Properties())
            {
                headers.Add(property.Name);
            }

            return headers;
        }
    }
}
