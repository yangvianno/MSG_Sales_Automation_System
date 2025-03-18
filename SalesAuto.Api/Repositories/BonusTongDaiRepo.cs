using DataAccessLibrary;
using OfficeOpenXml;
using SalesAuto.Models.ViewModel;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace SalesAuto.Api.Repositories
{
    public class BonusTongDaiRepo : IBonusTongDaiRepo
    {
        private readonly ISqlDataAccess sqlDataAccess;

        public BonusTongDaiRepo(ISqlDataAccess sqlDataAccess)
        {
            this.sqlDataAccess = sqlDataAccess;
        }

        public async Task<List<KPIVM>> GetKIPNhanVien(int Thang, int Nam, string MaBenhVien = "O")
        {
            var sql = "exec proc_BonusTongDaiKPINhanVien " + Thang + "," + Nam + ",'" + MaBenhVien + "'";
            return await sqlDataAccess.loadData<KPIVM, dynamic>(sql, new { });
        }
        public async Task<List<KPIVM>> GetKIPPhauThuat(int Thang, int Nam, string MaBenhVien = "O")
        {
            var sql = "exec proc_BonusTongDaiKPIPhauThuat " + Thang + "," + Nam + ",'" + MaBenhVien + "'";
            return await sqlDataAccess.loadData<KPIVM, dynamic>(sql, new { });
        }
        public async Task<List<KPIVM>> GetTarget(int Thang, int Nam, string MaBenhVien = "O")
        {
            var sql = "exec proc_BonusTongDaiTarget " + Thang + "," + Nam + ",'" + MaBenhVien + "'";
            return await sqlDataAccess.loadData<KPIVM, dynamic>(sql, new { });
        }
        public async Task<ExcelPackage> createBonusExcel(int thang, int nam, string MaBenhVien="O")
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var pkg = new ExcelPackage();
            pkg.Workbook.Properties.Title = "ABR tháng " + thang + " năm " + nam;
            pkg.Workbook.Properties.Author = "Tran Thien Thanh 0908325345";
            pkg.Workbook.Properties.Subject = "Bonus call center";
            ExcelWorksheet Sheet;
            var filePath = "BonusTongDaiTemp.xlsx";
            if (File.Exists(filePath))
            {
                var existingFile = new FileInfo(filePath);
                await pkg.LoadAsync(existingFile);
                Sheet = pkg.Workbook.Worksheets[0];
            }
            else
            {
                Sheet = pkg.Workbook.Worksheets.Add("");
            }
            var results= await GetTarget(thang, nam, MaBenhVien);
            foreach (var item in results)
            {
                Sheet.Cells[item.Hang, item.Cot].Value = item.SoLuong;
            }
            results = await GetKIPNhanVien(thang, nam, MaBenhVien);
            const int startColNhanVien=25;
            const int ColTen = 3;
            int hang = startColNhanVien-1;
            Sheet.Cells[4,4].Value = "Tháng " + thang + "/" + nam;
            Sheet.Cells[22, 9].Value = "Target KPI Tháng " + thang + "/" + nam;
            Sheet.Cells[37, 9].Value = "Target KPI Tháng " + thang + "/" + nam;

            string DienGiai = "";
            foreach(var item in results)
            {
                if(item.DienGiai!="")
                {
                    if (item.DienGiai!=DienGiai)
                    {
                        hang++;
                        Sheet.Row(hang).Height = 20;
                        DienGiai = item.DienGiai;
                        Sheet.Cells[hang, ColTen].Value = item.DienGiai;
                        Sheet.Cells[hang, ColTen-1].Value = hang- startColNhanVien+1;
                    }
                    Sheet.Cells[hang, item.Cot].Value=item.SoLuong;
                }                
            }
            results = await GetKIPPhauThuat(thang, nam, MaBenhVien);
            const int startRowBenhVien = 47;
            hang = startRowBenhVien - 1;
            DienGiai = "";
            foreach (var item in results)
            {
                if (item.DienGiai != "")
                {
                    if (item.DienGiai != DienGiai)
                    {
                        hang++;
                        Sheet.Row(hang).Height = 17;
                        DienGiai = item.DienGiai;
                        Sheet.Cells[hang, ColTen].Value = item.DienGiai;
                        Sheet.Cells[hang, ColTen - 1].Value = hang - startRowBenhVien + 1;
                    }
                    Sheet.Cells[hang, item.Cot].Value = item.SoLuong;
                }
            }
            return pkg;
        }

        public async Task<ExcelPackage> createBonusExcelChiTiet(int thang, int nam, string MaBenhVien = "O")
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var pkg = new ExcelPackage();
            pkg.Workbook.Properties.Title = "ABR tháng " + thang + " năm " + nam;
            pkg.Workbook.Properties.Author = "Tran Thien Thanh 0908325345";
            pkg.Workbook.Properties.Subject = "Bonus call center";
            ExcelWorksheet Sheet;
            Sheet = pkg.Workbook.Worksheets.Add("BenhNhanKham");
            DataTable table = await sqlDataAccess.getDataTable("exec proc_BonusGetBenhNhanKhamHoSoLasik " + thang + "," + nam + ",'" + MaBenhVien + "'");
            var range = Sheet.Cells[1, 1].LoadFromDataTable(table, true);
            range.AutoFitColumns();            
            Sheet = pkg.Workbook.Worksheets.Add("BenhNhanPhauThuat");
            table = await sqlDataAccess.getDataTable("exec proc_BonusGetBenhNhanPhauThuatLasik " + thang + "," + nam + ",'" + MaBenhVien + "'");
            Sheet.Cells[1, 1].LoadFromDataTable(table,true).AutoFitColumns();
            Sheet = pkg.Workbook.Worksheets.Add("BenhNhanDatKham");
            table = await sqlDataAccess.getDataTable("exec proc_BonusGetBenhNhanDatKham " + thang + "," + nam + ",'" + MaBenhVien + "'");
            Sheet.Cells[1, 1].LoadFromDataTable(table,true).AutoFitColumns();
            Sheet = pkg.Workbook.Worksheets.Add("Map Bệnh nhân khám");
            table = await sqlDataAccess.getDataTable("exec proc_BonusMapBenhNhanKham " + thang + "," + nam + ",'" + MaBenhVien + "'");
            Sheet.Cells[1, 1].LoadFromDataTable(table,true).AutoFitColumns();
            Sheet = pkg.Workbook.Worksheets.Add("Map Bệnh nhân PT");
            table = await sqlDataAccess.getDataTable("exec proc_BonusMapBenhNhanPhauThuat " + thang + "," + nam + ",'" + MaBenhVien + "'");
            Sheet.Cells[1, 1].LoadFromDataTable(table,true).AutoFitColumns();

            return pkg;
        }
    }
}
