using OfficeOpenXml;
using OfficeOpenXml.LoadFunctions.Params;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SalesAuto.Wasm.Services
{
    public class ExportFile : IExportFile
    {
        public async Task<ExcelPackage> SaveFile(IEnumerable<IDictionary<string, object>> jsonItems)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var pkg = new ExcelPackage();
            pkg.Workbook.Properties.Title = "Report";
            pkg.Workbook.Properties.Author = "Tran Thien Thanh 0908325345";
            pkg.Workbook.Properties.Subject = "Report";
            var sheet = pkg.Workbook.Worksheets.Add("Sheet1");
            sheet.Cells["A1"].LoadFromDictionaries(jsonItems, c =>
            {
                // Print headers using the property names
                c.PrintHeaders = true;
                // insert a space before each capital letter in the header
                //c.HeaderParsingType = HeaderParsingTypes.CamelCaseToSpace;
                // when TableStyle is not TableStyles.None the data will be loaded into a table with the 
                // selected style.
                c.TableStyle = TableStyles.Medium1;
            });
            //sheet.Cells.AutoFitColumns();            
            return pkg;
        }
        public async Task<ExcelPackage> SaveFile<T>(List<T> jsonItems)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var pkg = new ExcelPackage();
            pkg.Workbook.Properties.Title = "Report";
            pkg.Workbook.Properties.Author = "Tran Thien Thanh 0908325345";
            pkg.Workbook.Properties.Subject = "Report";
            var sheet = pkg.Workbook.Worksheets.Add("Sheet1");
            //sheet.Cells.LoadFromCollection(jsonItems, true);

            sheet.Cells.LoadFromCollection(jsonItems, c =>
            {
                // Print headers using the property names
                c.PrintHeaders = true;
                // insert a space before each capital letter in the header
                //c.HeaderParsingType = HeaderParsingTypes.CamelCaseToSpace;
                // when TableStyle is not TableStyles.None the data will be loaded into a table with the 
                // selected style.
                c.TableStyle = TableStyles.Medium1;
            });
            //sheet.Cells.AutoFitColumns();            
            return pkg;
        }
    }
}
