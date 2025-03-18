using OfficeOpenXml;
using SalesAuto.Models.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SalesAuto.Wasm.Services
{
    public class ABRLoadFileDanhGiaNhanVien: IABRLoadFileDanhGiaNhanVien
    {
        public async Task<List<ABRDanhGiaNhanVien>> loadFile(Stream input)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var pkg = new ExcelPackage();
            await pkg.LoadAsync(input);
            var sheet = pkg.Workbook.Worksheets.First();
            int Hang = 7;
            var Result = new List<ABRDanhGiaNhanVien>();
            while (sheet.Cells[Hang,1].Text!=""|| sheet.Cells[Hang, 2].Text != "")
            {
                try
                {
                    var NewItem = new ABRDanhGiaNhanVien();
                    NewItem.SoThuTu = int.Parse(sheet.Cells[Hang, 1].Text);
                    NewItem.MaNhanVien = sheet.Cells[Hang, 2].Text.Trim();
                    NewItem.HoVaTen = sheet.Cells[Hang, 3].Text.Trim();
                    NewItem.BenhVien = sheet.Cells[Hang, 4].Text.Trim();
                    NewItem.PhongBan = sheet.Cells[Hang, 5].Text.Trim();
                    NewItem.ChucDanh = sheet.Cells[Hang, 6].Text.Trim();
                    NewItem.LoaiDoiTuong = sheet.Cells[Hang, 7].Text.Trim();
                    if (sheet.Cells[Hang, 8].Text.Trim() != "")
                    {
                        string str = sheet.Cells[Hang, 8].Text.Trim().Replace("%", "").Trim();
                        if (str != "")
                        {
                            NewItem.MucTinhABRTrongThang = int.Parse(str);
                        }
                    }
                    NewItem.GhiChu = sheet.Cells[Hang, 9].Text.Trim();
                    Result.Add(NewItem);
                }
                catch
                { }
                Hang++;
            }
            input.Close();
            return Result;
        }
        public async Task<List<ABRNgayCong>> LoadFileNgayCong(Stream input)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var pkg = new ExcelPackage();
            await pkg.LoadAsync(input);
            var Result = new List<ABRNgayCong>();
            foreach (var sheet in pkg.Workbook.Worksheets)
            {
                if (sheet.Cells[1, 1].Text.Trim().ToLower() == "mã nv")
                {
                    int Hang = 2;
                    while (sheet.Cells[Hang, 1].Text != "" || sheet.Cells[Hang, 2].Text != "")
                    {
                        try
                        {
                            var NewItem = new ABRNgayCong();
                            NewItem.MaNhanVien = sheet.Cells[Hang, 1].Text.Trim();
                            NewItem.TenNhanVien = sheet.Cells[Hang, 2].Text.Trim();
                            NewItem.ChucDanh = sheet.Cells[Hang, 3].Text.Trim();
                            NewItem.NgayCong = Decimal.Parse(sheet.Cells[Hang, 4].Text.Trim());
                            Result.Add(NewItem);
                        }
                        catch
                        { }
                        Hang++;
                    }
                }
            }
            input.Close();
            return Result;
        }
    }
}
