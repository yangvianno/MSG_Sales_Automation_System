using DataAccessLibrary;
using DB;
using HelperLib;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using SalesAuto.Models;
using SalesAuto.Models.BackEndModel;
using SalesAuto.Models.Entities;
using SalesAuto.Models.SearchModel;
using SalesAuto.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SalesAuto.Api.Repositories
{
    public class SaleAutoReportRepo: ISaleAutoReportRepo
    {
        private readonly IBenhNhansRepo benhNhansRepository;
        private readonly SalesAutoDbContext context;
        private readonly ISqlDataAccess sqlDataAccess;
        private readonly bool useVer2 = true;

        public SaleAutoReportRepo(IBenhNhansRepo benhNhansRepository            
            , SalesAutoDbContext context
            , ISqlDataAccess sqlDataAccess)
        {
            this.benhNhansRepository = benhNhansRepository;
            this.context = context;
            this.sqlDataAccess = sqlDataAccess;            
        }

        public async Task<ExcelPackage> createBenhNhanReport(BenhNhanSM benhNhanSM, string MaBenhVien="O")
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var pkg = new ExcelPackage();
            pkg.Workbook.Properties.Title = "Report BenhNhan kham";
            pkg.Workbook.Properties.Author = "Tran Thien Thanh 0908325345";
            pkg.Workbook.Properties.Subject = "Benh nhan kham";
            benhNhanSM.Loai = "";
            var listBenhNhan = await benhNhansRepository.GetBenhNhanKhamList(benhNhanSM, MaBenhVien);
            var listBenhVien = await benhNhansRepository.GetBenhVienKhamList(benhNhanSM, MaBenhVien);
            var benhNhanKham = listBenhNhan.AsQueryable();
            benhNhanKham = benhNhanKham.Where(x => x.Loai.IndexOf("Khám")>=0);            
            var benhVienKham = listBenhVien.AsQueryable();
            benhVienKham = benhVienKham.Where(x => x.Loai.IndexOf("Khám") >=0);

            var BenhKhamSheet = pkg.Workbook.Worksheets.Add("Benh Kkam");
            BenhKhamSheet.Cells[2, 1].LoadFromCollection(benhNhanKham, true, TableStyles.Light1);
            BenhKhamSheet.Cells[2, 20].LoadFromCollection(benhVienKham, true, TableStyles.Dark9);
            BenhKhamSheet.Cells[3, 2, benhNhanKham.Count()+2, 3].Style.Numberformat.Format = "dd/mm/yyyy";
            BenhKhamSheet.Cells.AutoFitColumns();


            var benhNhanMo = listBenhNhan.AsQueryable();
            benhNhanMo = benhNhanMo.Where(x => x.Loai.IndexOf("Khám") < 0);
            var benhVienMo = listBenhVien.AsQueryable();
            benhVienMo = benhVienMo.Where(x => x.Loai.IndexOf("Khám") < 0);

            var BenhMoSheet = pkg.Workbook.Worksheets.Add("Phau thuat");
            BenhMoSheet.Cells[2, 1].LoadFromCollection(benhNhanMo, true, TableStyles.Light1);
            BenhMoSheet.Cells[2, 20].LoadFromCollection(benhVienMo, true, TableStyles.Dark9);
            BenhMoSheet.Cells[3, 2, benhNhanMo.Count()+2, 3].Style.Numberformat.Format = "dd/mm/yyyy";
            BenhMoSheet.Cells.AutoFitColumns();           
            return pkg;
        }

        int red = 200;
        int green = 250;
        int blue = 240;
        private void NextColor()
        {
            red = (red + 10 > 255 ? 200 : red + 10);
            green = (green - 10 < 200 ? 255 : green - 10);
            blue = (blue + 10 > 255 ? 200 : blue + 10);
        }

        public async Task<ExcelPackage> createCPAReport(int nam,int thang=0,string MaBenhVien="O")
        {
            if (MaBenhVien!="O")
            {
                return await createCPABVReport(nam, thang, MaBenhVien);
            }
            if (thang==0)
            {
                thang = DateTime.Now.Month;
            }    
            red = 209;
            green = 241;
            blue = 218;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage pkg;
            pkg = new ExcelPackage();
            pkg.Workbook.Properties.Title = "Report BenhNhan kham";
            pkg.Workbook.Properties.Author = "Tran Thien Thanh 0908325345";
            pkg.Workbook.Properties.Subject = "Benh nhan kham";

            var query = context.CPAReportVMs.Where(x => x.Nam == nam).AsQueryable();
            query = query.Where(x => x.Thang <= thang);
            query = query.OrderBy(x=>x.STT0).ThenBy(x=>x.STT1).ThenBy(x=>x.STT2).ThenBy(x=>x.Thang);

            var filePath = "CPA.xlsx";
            if (File.Exists("CPA" +nam+ ".xlsx"))
            {
                filePath = "CPA" + nam + ".xlsx";
            }
            ExcelWorksheet BenhMoSheet;
            if (File.Exists(filePath))
            {
                var existingFile = new FileInfo(filePath);
                await pkg.LoadAsync(existingFile);
                BenhMoSheet = pkg.Workbook.Worksheets[0];
            }
            else
            {                
                BenhMoSheet = pkg.Workbook.Worksheets.Add("CPA REPORT");
            }

            BenhMoSheet.Cells[2, 5].Value = (new DateTime(2021, thang, 1)).ToString("MMM", CultureInfo.CreateSpecificCulture("en-US"));
            
            var title1 = "";
            var title2 = "";
            var BenhVien = "";
            int curRow = 8;
            int startTitle1 = 8;
            int oldTile2Row = 8;
            int RowFinVolumn = 0;
            const int colTitle2 = 3;
            const int colTitle3 = 7;
            const int colTitle4 = 8;
            const int colBenhVien = 8;
            const int colThang1 = 9;
            const int colSumTile2 = 5;
            const int startRow = 8;
            var colorColtile2 = Color.FromArgb(255, 225, 204);
            bool TinhTong = true;
            bool DaTinhTongTitle2 = false;
            string sql = @"select BenhVien.TenVietTat as MaBenhVien, sum(ChiTieuSoLuongs.SoLuong) as SoLuong
                        from 
                        ChiTieuSoLuongs
                            inner join BenhVien on ChiTieuSoLuongs.MaBenhVien = BenhVien.MaBenhVien
                            inner join LoaiChiTieu on ChiTieuSoLuongs.MaLoaiChiTieu = LoaiChiTieu.MaLoaiChiTieu                        
                        where Nam=" + nam + @" and LoaiChiTieu.NhomChiTiet='Lasik'
                        group by BenhVien.TenVietTat";

            List<TongBenhVienM> LisTongLasik = await sqlDataAccess.loadData<TongBenhVienM, dynamic>(sql, new { });
            sql = @"select BenhVien.TenVietTat as MaBenhVien, cast(sum(ChiTieuSoLuongs.SoLuong)/1000000 as int) as SoLuong
                        from 
                        ChiTieuSoLuongs
                            inner join BenhVien on ChiTieuSoLuongs.MaBenhVien = BenhVien.MaBenhVien
                            inner join LoaiChiTieu on ChiTieuSoLuongs.MaLoaiChiTieu = LoaiChiTieu.MaLoaiChiTieu   
                        where Nam=" + nam + @" and LoaiChiTieu.NhomChiTiet=N'DoanhThuLasik'
                        group by BenhVien.TenVietTat";

            List<TongBenhVienM> LisTongDoanhThu = await sqlDataAccess.loadData<TongBenhVienM, dynamic>(sql, new { });


            foreach (CPAReportVM item in query.ToList())
            {
                if(item.GroupTitle1!=title1)
                {
                    if (title1 != "")
                    {
                        // Tinh dong tong cho MKT
                        TaoCongThucTinhTong(BenhMoSheet, curRow, oldTile2Row, colThang1);
                        DaTinhTongTitle2 = true;
                        //for (int i = colThang1; i <= colThang1 + 12; i++)
                        //{
                        //    BenhMoSheet.Cells[oldTile2Row, i].Formula = "=sum(" + BenhMoSheet.Cells[oldTile2Row + 1, i].Address + ":" + BenhMoSheet.Cells[curRow, i].Address + ")";                            

                        //    DaTinhTongTitle2 = true;
                        //}
                        curRow++;
                        BenhMoSheet.Cells[curRow, colTitle2].Value = nam.ToString() + " DIGI CR";
                        BenhMoSheet.Cells[curRow, colTitle2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        BenhMoSheet.Cells[curRow, colTitle2].Style.Fill.BackgroundColor.SetColor(colorColtile2);
                        BenhMoSheet.Cells[curRow, colSumTile2].Formula = "=" + BenhMoSheet.Cells[oldTile2Row, colThang1 + 12].Address + "/" + BenhMoSheet.Cells[startRow, colSumTile2].Address ;
                        BenhMoSheet.Cells[curRow, colSumTile2].Style.Numberformat.Format = "0%";
                        BenhMoSheet.Cells[curRow, colTitle3].Value = "Total";
                        BenhMoSheet.Cells[curRow, colTitle4].Value = "DIGI SURS / LEADS";
                        BenhMoSheet.Cells[curRow, colThang1, curRow, colThang1 + 12].Style.Numberformat.Format = "0%";
                        BenhMoSheet.Cells[curRow, colThang1, curRow, colThang1 + 12].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        BenhMoSheet.Cells[curRow, colThang1, curRow, colThang1 + 12].Style.Fill.BackgroundColor.SetColor(colorColtile2);
                        BenhMoSheet.Cells[curRow, colThang1, curRow, colThang1 + 12].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        BenhMoSheet.Cells[curRow, 1, curRow, colThang1 + 12].Style.Font.Bold = true;
                        BenhMoSheet.Cells[curRow, 1, curRow, colThang1 + 12].Style.Font.Size = 13;
                        for (int i = 0; i <= 12; i++)
                        {
                            BenhMoSheet.Cells[curRow, colThang1 + i].Formula = "=" + BenhMoSheet.Cells[oldTile2Row, colThang1 + i].Address + "/" + BenhMoSheet.Cells[startRow, colThang1 + i].Address;
                        }
                        // Merge group Title1
                        BenhMoSheet.Cells[startTitle1, 1].Value = title1;
                        BenhMoSheet.Cells[startTitle1, 1, curRow, 1].Merge = true;
                        BenhMoSheet.Cells[startTitle1, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        curRow = KeDong(BenhMoSheet, curRow, colThang1);
                        startTitle1 = curRow;
                    }
                    else
                    {
                        DaTinhTongTitle2 = false;
                    }
                    title1 = item.GroupTitle1;
                    TinhTong = true;
                }
                else
                {
                    TinhTong = false;
                    DaTinhTongTitle2 = false;
                }

                if (title2!=item.GroupTitle2)
                {
                    // Lap cong thuc cho tile 2 cu:                    
                    if (title2 != "")
                    {
                        if (!DaTinhTongTitle2)
                        {
                            TaoCongThucTinhTong(BenhMoSheet, curRow, oldTile2Row, colThang1);
                        }
                        if (!TinhTong)
                        {
                            BenhMoSheet.Cells[curRow + 1, colSumTile2].Formula = "=" + BenhMoSheet.Cells[curRow + 1, colThang1 + 12].Address + "/" + BenhMoSheet.Cells[oldTile2Row, colThang1 + 12].Address;
                            BenhMoSheet.Cells[curRow + 1, colSumTile2].Style.Numberformat.Format = "0%";
                            if (title1 == "FIN" && (title2 == "REVENUE" || title2 == "VOLUME"))
                            {
                                BenhMoSheet.Cells[oldTile2Row, colSumTile2].Formula = "=" + BenhMoSheet.Cells[oldTile2Row, colThang1 + 12].Address +"/sum(" + BenhMoSheet.Cells[curRow -1, colTitle2].Address +":"+ BenhMoSheet.Cells[oldTile2Row +1, colTitle2].Address + ")" ;

                            }
                            if (title1 == "FIN" && title2 == "VOLUME")
                            {
                                RowFinVolumn = oldTile2Row;
                            }
                            
                        }
                        curRow++;
                        oldTile2Row = curRow;
                    }
                    if (TinhTong)
                    {
                        BenhMoSheet.Cells[curRow, colSumTile2].Formula = "=sum(" + BenhMoSheet.Cells[curRow, colThang1].Address + ":" + BenhMoSheet.Cells[curRow, colThang1 + 11].Address + ")";
                        if (title1 == "FIN" && title2 == "VOLUME")
                        {
                            RowFinVolumn = curRow;
                        }
                    }

                    DaTinhTongTitle2 = true;
                    title2 = item.GroupTitle2;

                    BenhMoSheet.Cells[curRow, colTitle2].Value = item.GroupTitle2;
                    BenhMoSheet.Cells[curRow, colTitle3].Value = item.GroupTitle3;
                    BenhMoSheet.Cells[curRow, colTitle4].Value = item.GroupTitle4;
                    
                    // ToMau
                    BenhMoSheet.Cells[curRow, colBenhVien, curRow, colThang1 + 12].Style.Fill.PatternType = ExcelFillStyle.Solid;                    
                    BenhMoSheet.Cells[curRow, colBenhVien, curRow, colThang1 + 12].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(red,green,blue));
                    BenhMoSheet.Cells[curRow, colBenhVien, curRow, colThang1 + 12].Style.Font.Bold = true;
                    BenhMoSheet.Cells[curRow, colSumTile2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    BenhMoSheet.Cells[curRow, colSumTile2].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(red, green, blue));
                    BenhMoSheet.Cells[curRow, colTitle2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    BenhMoSheet.Cells[curRow, colTitle2].Style.Fill.BackgroundColor.SetColor(colorColtile2);

                    NextColor();

                }
                if (BenhVien!=item.BenhVien)
                {
                    curRow++;
                    BenhVien = item.BenhVien;
                    BenhMoSheet.Cells[curRow, colBenhVien].Value = item.BenhVien;
                    BenhMoSheet.Cells[curRow, colBenhVien].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    BenhMoSheet.Cells[curRow, colThang1 + 12].Formula = "=sum(" + BenhMoSheet.Cells[curRow, colThang1].Address + ":" + BenhMoSheet.Cells[curRow, colThang1 + 11].Address + ")";                    
                    BenhMoSheet.Cells[curRow, colThang1 + 12].Style.Numberformat.Format = "#,##0";
                    if (title1 == "FIN" && title2 == "VOLUME")
                    {
                        if (LisTongLasik != null)
                        {
                            var Volum = LisTongLasik.FirstOrDefault(x => x.MaBenhVien == item.BenhVien);
                            if (Volum != null)
                            {
                                BenhMoSheet.Cells[curRow, colTitle2].Value = Volum.SoLuong;
                            }
                        }
                        // tính phần trăm từng dòng cho từng bệnh viện
                        BenhMoSheet.Cells[curRow, colSumTile2].Formula = "=" + BenhMoSheet.Cells[curRow, colThang1 + 12].Address + "/" + BenhMoSheet.Cells[curRow, colTitle2].Address ;
                        BenhMoSheet.Cells[curRow, colSumTile2].Style.Numberformat.Format = "0%";
                    }

                    if (title1 == "FIN" && title2 == "REVENUE")
                    {

                        if (LisTongDoanhThu != null)
                        {
                            var Volum = LisTongDoanhThu.FirstOrDefault(x => x.MaBenhVien == item.BenhVien);
                            if (Volum != null)
                            {
                                BenhMoSheet.Cells[curRow, colTitle2].Value = Volum.SoLuong;
                            }
                        }
                        // tính phần trăm từng dòng cho từng bệnh viện
                        BenhMoSheet.Cells[curRow, colSumTile2].Formula = "=" + BenhMoSheet.Cells[curRow, colThang1 + 12].Address + "/" + BenhMoSheet.Cells[curRow, colTitle2].Address;
                        BenhMoSheet.Cells[curRow, colSumTile2].Style.Numberformat.Format = "0%";
                    }

                }
                if ((title1 == "FIN" && title2 == "REVENUE"))
                {
                    BenhMoSheet.Cells[curRow, colThang1 + item.Thang - 1].Value = (int)(item.SoLuong/1000000);
                }
                else
                {
                    BenhMoSheet.Cells[curRow, colThang1 + item.Thang - 1].Value = item.SoLuong;
                }
                BenhMoSheet.Cells[curRow, colThang1 + item.Thang - 1].Style.Numberformat.Format = "#,##0";
            }

            if (title1 != "")
            {
                BenhMoSheet.Cells[startTitle1, 1].Value = title1;
                BenhMoSheet.Cells[startTitle1, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                BenhMoSheet.Cells[startTitle1, 1].Style.Fill.BackgroundColor.SetColor(Color.LightSalmon);
                BenhMoSheet.Cells[startTitle1, 1, curRow, 1].Merge = true;
                BenhMoSheet.Cells[startTitle1, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }

            if (title2 != "")
            {
                // Tinh Tong Cu
                TaoCongThucTinhTong(BenhMoSheet, curRow, oldTile2Row, colThang1);
                //for (int i = colThang1; i <= colThang1 + 12; i++)
                //{
                //    BenhMoSheet.Cells[oldTile2Row, i].Formula = "=sum(" + BenhMoSheet.Cells[oldTile2Row + 1, i].Address + ":" + BenhMoSheet.Cells[curRow, i].Address + ")";
                //}
                if (TinhTong)
                {
                    BenhMoSheet.Cells[oldTile2Row, colSumTile2].Formula = "=sum(" + BenhMoSheet.Cells[oldTile2Row, colThang1].Address + ":" + BenhMoSheet.Cells[curRow, colThang1 + 11].Address + ")";
                }
                else
                {
                    if (title1!= "FIN")
                    {
                        BenhMoSheet.Cells[curRow + 1, colSumTile2].Formula = "=" + BenhMoSheet.Cells[curRow + 1, colThang1 + 12].Address + "/" + BenhMoSheet.Cells[oldTile2Row, colThang1 + 12].Address;
                        BenhMoSheet.Cells[curRow + 1, colSumTile2].Style.Numberformat.Format = "0%";
                    }
                    else if (title1 == "FIN" && (title2 == "REVENUE" || title2 == "VOLUME"))
                    {
                        BenhMoSheet.Cells[oldTile2Row, colSumTile2].Formula = "=" + BenhMoSheet.Cells[oldTile2Row, colThang1 + 12].Address + "/sum(" + BenhMoSheet.Cells[curRow - 1, colTitle2].Address + ":" + BenhMoSheet.Cells[oldTile2Row + 1, colTitle2].Address + ")";
                    }

                }
                curRow++;
            }

            curRow = KeDong(BenhMoSheet, curRow, colThang1);
            curRow++;

            BenhMoSheet.Cells[curRow, colTitle2].Value = "% USED BUD";
            BenhMoSheet.Cells[curRow, colTitle4].Value = "MKT BUDGET MTD";
            BenhMoSheet.Cells[curRow, colTitle2].Style.Fill.PatternType = ExcelFillStyle.Solid;            
            BenhMoSheet.Cells[curRow, colTitle2].Style.Fill.BackgroundColor.SetColor(colorColtile2);
            NextColor();
            BenhMoSheet.Cells[curRow, colTitle4,curRow,colThang1+12].Style.Fill.PatternType = ExcelFillStyle.Solid;
            BenhMoSheet.Cells[curRow, colTitle4, curRow, colThang1 + 12].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(red,green,blue));
            if (RowFinVolumn != 0)
            {
                for (int i = 0; i <= 12; i++)
                {
                    BenhMoSheet.Cells[curRow, colThang1 + i].Formula = "=" + BenhMoSheet.Cells[RowFinVolumn, colThang1 + i].Address + "*2000000";                    
                    BenhMoSheet.Cells[curRow, colThang1 + i].Style.Numberformat.Format = "#,##0";
                    BenhMoSheet.Cells[curRow+2, colThang1 + i].Formula = "=" + BenhMoSheet.Cells[curRow + 3, colThang1 + i].Address + "+" + BenhMoSheet.Cells[curRow + 4, colThang1 + i].Address;
                    BenhMoSheet.Cells[curRow+2, colThang1 + i].Style.Numberformat.Format = "#,##0";
                }                
            }
            curRow++;
            BenhMoSheet.Row(curRow).Height = 7;
            curRow++;
            NextColor();
            BenhMoSheet.Cells[curRow, colTitle4].Value = "MKT EXPENSE";
            BenhMoSheet.Cells[curRow, colTitle4, curRow, colThang1 + 12].Style.Fill.PatternType = ExcelFillStyle.Solid;
            BenhMoSheet.Cells[curRow, colTitle4, curRow, colThang1 + 12].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(red, green, blue));            
            curRow++;
            BenhMoSheet.Cells[curRow, colTitle4, curRow+1,colTitle4].Style.Font.Italic = true;
            BenhMoSheet.Cells[curRow, colTitle4, curRow + 1, colTitle4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            var kpiInputs = context.KPIMonthlys.AsQueryable().Where(x => x.Thang <= thang && x.Nam == nam).OrderBy(x => x.Thang);
            foreach ( var item in kpiInputs)
            {
                BenhMoSheet.Cells[curRow, colThang1 + item.Thang - 1].Value = item.ActualBranding;
                BenhMoSheet.Cells[curRow, colThang1 + item.Thang - 1].Style.Numberformat.Format = "#,##0";
                BenhMoSheet.Cells[curRow+1, colThang1 + item.Thang - 1].Value = item.BudgetDigitalCost;
                BenhMoSheet.Cells[curRow+1, colThang1 + item.Thang - 1].Style.Numberformat.Format = "#,##0";
            }
            // cộng 2 bảng cuối cùng
            BenhMoSheet.Cells[curRow, colThang1 + 12].Formula = "=sum(" + BenhMoSheet.Cells[curRow, colThang1].Address + ":" + BenhMoSheet.Cells[curRow, colThang1 + 11].Address + ")";
            BenhMoSheet.Cells[curRow, colThang1 + 12].Style.Numberformat.Format = "#,##0";
            BenhMoSheet.Cells[curRow+1, colThang1 + 12].Formula = "=sum(" + BenhMoSheet.Cells[curRow+1, colThang1].Address + ":" + BenhMoSheet.Cells[curRow+1, colThang1 + 11].Address + ")";
            BenhMoSheet.Cells[curRow+1, colThang1 + 12].Style.Numberformat.Format = "#,##0";

            BenhMoSheet.Cells[curRow, colTitle4].Value = "Branding";
            curRow++;
            BenhMoSheet.Cells[curRow, colTitle4].Value = "Performance";

            return pkg;
        }

        public async Task<ExcelPackage> createCPABVReport(int nam, int thang = 0, string MaBenhVien ="O")
        {
            DateTime Ngay = DateTime.Now;
            if (thang == 0)
            {
                Ngay = DateTimeHelp.LayTuan(Ngay).TuNgay.AddMinutes(-1);
            }
            else
            {
                Ngay = new DateTime(nam, thang, 1).AddMonths(1).AddMinutes(-1);
            }
            red = 209;
            green = 241;
            blue = 218;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage pkg;
            pkg = new ExcelPackage();
            pkg.Workbook.Properties.Title = "Report BenhNhan kham";
            pkg.Workbook.Properties.Author = "Tran Thien Thanh 0908325345";
            pkg.Workbook.Properties.Subject = "Benh nhan kham";

            var query = context.CPAReportVMs.Where(x => x.Nam == nam).AsQueryable();
            query = query.Where(x => x.Thang <= thang);
            query = query.OrderBy(x => x.STT0).ThenBy(x => x.STT1).ThenBy(x => x.STT2).ThenBy(x => x.Thang);
            var filePath = "CPABV.xlsx";
            ExcelWorksheet sheet;
            if (File.Exists(filePath))
            {
                var existingFile = new FileInfo(filePath);
                await pkg.LoadAsync(existingFile);
                sheet = pkg.Workbook.Worksheets[0];
            }
            else
            {
                sheet = pkg.Workbook.Worksheets.Add("CPA REPORT");
            }
            List<KPIVM> listKPI;
            sheet.Cells[2, 5].Value = Ngay.ToString("MMM", CultureInfo.CreateSpecificCulture("en-US"));

            listKPI = await sqlDataAccess.loadData<KPIVM, dynamic>("exec proc_getCPABV '" + MaBenhVien + "','"+ Ngay.ToString("yyyy-MM-dd 23:59:59") + "'", new { });
            foreach (var item in listKPI)
            {                
                sheet.Cells[item.Hang, item.Cot].Value = item.SoLuong;
                if (!string.IsNullOrEmpty(item.DienGiai) && item.SoLuong==0)
                {
                    sheet.Cells[item.Hang, item.Cot].Value = item.DienGiai;
                    sheet.Cells[item.Hang, item.Cot].Style.WrapText = true;
                    sheet.Row(item.Hang).CustomHeight = false;
                }
            }
            return pkg;
        }

        


        private static void TaoCongThucTinhTong(ExcelWorksheet BenhMoSheet, int curRow, int oldTile2Row, int colThang1)
        {
            for (int i = colThang1; i <= colThang1 + 12; i++)
            {
                BenhMoSheet.Cells[oldTile2Row, i].Formula = "=sum(" + BenhMoSheet.Cells[oldTile2Row + 1, i].Address + ":" + BenhMoSheet.Cells[curRow, i].Address + ")";
                BenhMoSheet.Cells[oldTile2Row, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                BenhMoSheet.Cells[oldTile2Row, i].Style.Numberformat.Format = "#,##0";
            }
            
        }

        private static int KeDong(ExcelWorksheet BenhMoSheet, int curRow, int colThang1)
        {
            curRow++;
            BenhMoSheet.Row(curRow).Height = 7;
            BenhMoSheet.Cells[curRow, 1, curRow, colThang1 + 12].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            curRow++;
            BenhMoSheet.Row(curRow).Height = 7;
            return curRow;
        }
        public async Task<ExcelPackage> createLeadFollowReport(int nam, int thang=0, string MaBenhVien="O")
        {
            if (MaBenhVien != "O")
            {
                return await createLeadFollowBVReport(nam, thang, MaBenhVien);
            }
            if (thang == 0)
            {
                thang = DateTime.Now.Month;
            }
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage pkg;
            pkg = new ExcelPackage();
            pkg.Workbook.Properties.Title = "Report lead follow";
            pkg.Workbook.Properties.Author = "Tran Thien Thanh 0908325345";
            pkg.Workbook.Properties.Subject = "lead follow";
            var filePath = "LeadFollow.xlsx";
            if (File.Exists("LeadFollow" + nam + ".xlsx"))
            {
                filePath = "LeadFollow" + nam + ".xlsx";
            }
            ExcelWorksheet BenhMoSheet;
            if (File.Exists(filePath))
            {
                var existingFile = new FileInfo(filePath);
                await pkg.LoadAsync(existingFile);
                BenhMoSheet = pkg.Workbook.Worksheets[0];
            }
            else
            {
                BenhMoSheet = pkg.Workbook.Worksheets.Add("Leads Follow up Report");
            }
            await DoSoLieuLeadFollow(nam, thang, BenhMoSheet);
            return pkg;

        }        

        private async Task DoSoLieuLeadFollow(int nam, int thang, ExcelWorksheet BenhMoSheet)
        {
            BenhMoSheet.Cells[1, 5].Value = (new DateTime(2021, thang, 1)).ToString("MMM", CultureInfo.CreateSpecificCulture("en-US"));
            List<LFUReportVM> listLUF;
            const int LeadsRow = 3;
            const int colThang1 = 7;
            const int booksRow = 7;
            const int examsRow = 14;
            const int sursRow = 21;
            for (int i = 1; i <= thang; i++)
            {
                listLUF = await sqlDataAccess.loadData<LFUReportVM, dynamic>("exec proc_getLeadsFollow " + i + "," + nam, new { });
                foreach (var item in listLUF)
                {
                    if (item.Title1 == "LEADS")
                    {
                        BenhMoSheet.Cells[LeadsRow + item.STT2, colThang1 + i - 1].Value = item.SoLuong;
                    }
                    else if (item.Title1 == "BOOKS")
                    {
                        BenhMoSheet.Cells[booksRow + item.STT2, colThang1 + i - 1].Value = item.SoLuong;
                    }
                    else if (item.Title1 == "EXAMS")
                    {
                        BenhMoSheet.Cells[examsRow + item.STT2, colThang1 + i - 1].Value = item.SoLuong;
                    }
                    else if (item.Title1 == "SURS")
                    {
                        BenhMoSheet.Cells[sursRow + item.STT2, colThang1 + i - 1].Value = item.SoLuong;
                    }
                }
            }
        }

        public async Task<ExcelPackage> createLeadFollowBVReport(int nam, int thang = 0, string MaBenhVien="O")
        {
            DateTime Ngay = DateTime.Now;
            if (thang == 0)
            {
                Ngay = DateTimeHelp.LayTuan(Ngay).TuNgay.AddMinutes(-1);                
            }
            else
            {
                Ngay = new DateTime(nam, thang, 1).AddMonths(1).AddMinutes(-1);
            }
            red = 209;
            green = 241;
            blue = 218;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage pkg;
            pkg = new ExcelPackage();
            pkg.Workbook.Properties.Title = "Report BenhNhan kham";
            pkg.Workbook.Properties.Author = "Tran Thien Thanh 0908325345";
            pkg.Workbook.Properties.Subject = "Benh nhan kham";
            
            var filePath = "LeadFollowBV.xlsx";
            ExcelWorksheet sheet;
            if (File.Exists(filePath))
            {
                var existingFile = new FileInfo(filePath);
                await pkg.LoadAsync(existingFile);
                sheet = pkg.Workbook.Worksheets[0];
            }
            else
            {
                sheet = pkg.Workbook.Worksheets.Add("Leads Following");
            }
            sheet.Cells[1, 5].Value = Ngay.ToString("MMM", CultureInfo.CreateSpecificCulture("en-US"));
            sheet.Cells[2, 2].Value = Ngay.Year;
            sheet.Cells[2, 6].Value = "FY " + (Ngay.Year-1).ToString();

            List<KPIVM> listKPI;            
            listKPI = await sqlDataAccess.loadData<KPIVM, dynamic>("exec proc_getLeadFollowBV '" + MaBenhVien + "','" + Ngay.ToString("yyyy-MM-dd 23:59:59") + "'" , new { });
            foreach (var item in listKPI)
            {
                sheet.Cells[item.Hang, item.Cot].Value = item.SoLuong;
            }
            return pkg;
        }
        public async Task<ExcelPackage> createKPIReport(int thang, int nam, DateTime Ngay, bool GuiTheoNgay = false)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage pkg;
            pkg = new ExcelPackage();
            pkg.Workbook.Properties.Title = "Report for KPI";
            pkg.Workbook.Properties.Author = "Tran Thien Thanh 0908325345";
            pkg.Workbook.Properties.Subject = "KPI";

            var query = context.CPAReportVMs.Where(x => x.Nam == nam).AsQueryable();
            query = query.OrderBy(x => x.STT0).ThenBy(x => x.STT1).ThenBy(x => x.STT2).ThenBy(x => x.Thang);
            var filePath = "KPI.xlsx";            
            if (File.Exists("KPI" + nam + ".xlsx"))
            {
                filePath = "KPI" + nam + ".xlsx";
            }
            ExcelWorksheet BenhMoSheet;
            if (File.Exists(filePath))
            {
                var existingFile = new FileInfo(filePath);
                await pkg.LoadAsync(existingFile);
                BenhMoSheet = pkg.Workbook.Worksheets[0];
            }
            else
            {
                BenhMoSheet = pkg.Workbook.Worksheets.Add("KPI");
            }
            BenhMoSheet.Cells[1,3].Value = new DateTime(nam, thang, 1).ToString("MMM", CultureInfo.CreateSpecificCulture("en-US"));

            List<KPIVM> listKPI;

            if (GuiTheoNgay)
            {
                listKPI = await sqlDataAccess.loadData<KPIVM, dynamic>("exec proc_getKPIVM " + thang + "," + nam +",'"+ Ngay.ToString("yyyy-MM-dd 23:59:59") +"'", new { });
            }
            else
            {
                listKPI = await sqlDataAccess.loadData<KPIVM, dynamic>("exec proc_getKPIVM " + thang + "," + nam, new { });
            }

            foreach (var item in listKPI)
            {                
                BenhMoSheet.Cells[item.Hang, item.Cot].Value = item.SoLuong;
            }

            listKPI = await sqlDataAccess.loadData<KPIVM, dynamic>("exec proc_getKPIVMByMonth " + thang + "," + nam, new { });
            const int KPIMonthStartCol= 12;
            const int KPIMonthStartRow = 0;
            foreach (var item in listKPI)
            {               
                BenhMoSheet.Cells[KPIMonthStartRow + item.Hang, KPIMonthStartCol + item.Cot - 1].Value = item.SoLuong;
            }
            return pkg;
        }

        public async Task<ExcelPackage> createLeadsChannelReport(int thang, int nam, bool GuiTheoTuan=false, string MaBenhVien ="O")
        {

            if (MaBenhVien!="O")
            {
                return await createLeadsChannelBVReport(thang, nam, GuiTheoTuan, MaBenhVien);
            }
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage pkg;
            DateTime NgayBaoCao = DateTime.Now;
            if (GuiTheoTuan)
            {
                NgayBaoCao = DateTimeHelp.LayTuan(NgayBaoCao).TuNgay.AddMinutes(-1);
            }
            pkg = new ExcelPackage();
            pkg.Workbook.Properties.Title = "Report for LeadsChannel";
            pkg.Workbook.Properties.Author = "Tran Thien Thanh 0908325345";
            pkg.Workbook.Properties.Subject = "KPI";
            var filePath = "LeadsChannel.xlsx";
            if (File.Exists("LeadsChannel"+nam+ ".xlsx"))
            {
                filePath = "LeadsChannel" + nam + ".xlsx";
            }
            if (MaBenhVien != "O")
            {
                filePath = "LeadsChannelBV.xlsx";
            }                
            ExcelWorksheet Sheet;
            if (File.Exists(filePath))
            {
                var existingFile = new FileInfo(filePath);
                await pkg.LoadAsync(existingFile);
                Sheet = pkg.Workbook.Worksheets[0];
            }
            else
            {
                Sheet = pkg.Workbook.Worksheets.Add("Leads Channel");
            }

            List<KPIVM> listKPI;
            if (GuiTheoTuan)
            {
                listKPI = await sqlDataAccess.loadData<KPIVM, dynamic>("exec proc_getAutoLeadsChannel " + thang + "," + nam + ",'" + NgayBaoCao.ToString("yyyy-MM-dd 23:59:59") + "'", new { });
            }
            else
            {
                listKPI = await sqlDataAccess.loadData<KPIVM, dynamic>("exec proc_getAutoLeadsChannel " + thang + "," + nam , new { });
            }
            foreach (var item in listKPI)
            {
                Sheet.Cells[item.Hang, 4 + item.Cot].Value = item.SoLuong;
            }
            return pkg;
        }
        public async Task<ExcelPackage> createLeadsChannelBVReport(int thang, int nam, bool GuiTheoTuan = false, string MaBenhVien="O")
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage pkg;
            DateTime NgayBaoCao = DateTime.Now;
            if (GuiTheoTuan)
            {
                NgayBaoCao = DateTimeHelp.LayTuan(NgayBaoCao).TuNgay.AddMinutes(-1);
            }
            pkg = new ExcelPackage();
            pkg.Workbook.Properties.Title = "Report for LeadsChannel";
            pkg.Workbook.Properties.Author = "Tran Thien Thanh 0908325345";
            pkg.Workbook.Properties.Subject = "KPI";
            var filePath = "LeadsChannel.xlsx";
            if (MaBenhVien != "O")
            {
                filePath = "LeadsChannelBV.xlsx";
            }
            ExcelWorksheet Sheet;
            if (File.Exists(filePath))
            {
                var existingFile = new FileInfo(filePath);
                await pkg.LoadAsync(existingFile);
                Sheet = pkg.Workbook.Worksheets[0];
            }
            else
            {
                Sheet = pkg.Workbook.Worksheets.Add("Leads Channel");
            }
            Sheet.Cells[1,3].Value = "YTD "+(NgayBaoCao.Year-2000).ToString();
            Sheet.Cells[1, 4].Value = "FY " + (NgayBaoCao.Year - 1 - 2000).ToString();
            Sheet.Cells[1, 5].Value = (NgayBaoCao.Year);
            Sheet.Cells[1, 17].Value = (NgayBaoCao.Year-1);

            List<KPIVM> listKPI;
            if (GuiTheoTuan)
            {
                listKPI = await sqlDataAccess.loadData<KPIVM, dynamic>("exec proc_getAutoLeadsChannel " + thang + "," + nam + ",'" + NgayBaoCao.ToString("yyyy-MM-dd 23:59:59") + "','" + MaBenhVien + "'", new { });
            }
            else
            {
                listKPI = await sqlDataAccess.loadData<KPIVM, dynamic>("exec proc_getAutoLeadsChannel " + thang + "," + nam + ", null, '" + MaBenhVien + "'", new { });
            }
            foreach (var item in listKPI)
            {
                Sheet.Cells[item.Hang, 4 + item.Cot].Value = item.SoLuong;
            }
            return pkg;
        }

        public async Task<ExcelPackage> createFollowupPatientsReport(int tuan, int nam, string MaBenhVien, int Thang=0)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage pkg;
            pkg = new ExcelPackage();
            pkg.Workbook.Properties.Title = "Report for LeadsChannel";
            pkg.Workbook.Properties.Author = "Tran Thien Thanh 0908325345";
            pkg.Workbook.Properties.Subject = "FollowupPatients";
            var filePath = "FollowupPatients.xlsx";
            var tuanle = HelperLib.DateTimeHelp.LayTuan(tuan, nam);
            DateTime LayDenNgay = tuanle.DenNgay;
            if (Thang != 0)
            {
                LayDenNgay = new DateTime(nam, Thang, 1).AddMonths(1).AddMinutes(-1);
            }
            ExcelWorksheet Sheet;
            if (File.Exists("FollowupPatients" + MaBenhVien + "" + nam + ".xlsx"))
            {
                filePath = "FollowupPatients" + MaBenhVien + "" + nam + ".xlsx";
            }
            if (File.Exists(filePath))
            {
                var existingFile = new FileInfo(filePath);
                await pkg.LoadAsync(existingFile);
                Sheet = pkg.Workbook.Worksheets[0];
            }
            else
            {
                Sheet = pkg.Workbook.Worksheets.Add("Follow Up Patients");
            }
            await DoSoLuongFollowUpPatiens(MaBenhVien, Thang, pkg, LayDenNgay, Sheet, true);
            return pkg;
        }

        public async Task<ExcelPackage> createFollowupPatientsReportGroup(int tuan, int nam,int Thang = 0)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage pkg;
            pkg = new ExcelPackage();
            pkg.Workbook.Properties.Title = "Report for LeadsChannel";
            pkg.Workbook.Properties.Author = "Tran Thien Thanh 0908325345";
            pkg.Workbook.Properties.Subject = "FollowupPatients";
            var filePath = "GroupFollowupPatients.xlsx";
            var tuanle = HelperLib.DateTimeHelp.LayTuan(tuan, nam);
            DateTime LayDenNgay = tuanle.DenNgay;
            if (Thang != 0)
            {
                LayDenNgay = new DateTime(nam, Thang, 1).AddMonths(1).AddMinutes(-1);
            }
           
            if (File.Exists("GroupFollowupPatients" + nam + ".xlsx"))
            {
                filePath = "GroupFollowupPatients" + nam + ".xlsx";
            }
            if (File.Exists(filePath))
            {
                var existingFile = new FileInfo(filePath);
                await pkg.LoadAsync(existingFile);
                foreach(ExcelWorksheet Sheet in pkg.Workbook.Worksheets)
                {                    
                    string MaBenhVien = await LayMaBenhVienTheoTenVietTat(Sheet.Name);
                    if (MaBenhVien!="")
                    {
                        await DoSoLuongFollowUpPatiens(MaBenhVien, Thang, pkg, LayDenNgay, Sheet, false);
                    }
                }
                pkg.Workbook.Worksheets[0].Cells[2, 3].Value = tuanle.DenNgay.ToString("MMM");
            }          
            
            return pkg;
        }

        private async Task<string> LayMaBenhVienTheoTenVietTat(string name)
        {
            string sql = @"select BenhVien.MaBenhVien, BenhVien.TenBenhVien, PreSql 
                        from
                            BenhVien                            
                        where BenhVien.TenVietTat = '" + name + "'";
            var result = await sqlDataAccess.loadData<BenhVienVM, dynamic>(sql, new { });
            if (result==null)
            {
                return "";
            }
            if (result.Count<=0)
            {
                return "";
            }
            return result.FirstOrDefault().MaBenhVien;
        }

        private async Task DoSoLuongFollowUpPatiens(string MaBenhVien, int Thang, ExcelPackage pkg, DateTime LayDenNgay, ExcelWorksheet Sheet, bool LayDanhSachChiTiet=true)
        {
            List<KPIVM> listKPI = await sqlDataAccess.loadData<KPIVM, dynamic>("exec proc_AutoFollowupPatients '" + LayDenNgay.ToString("yyyy-MM-dd 23:59:59") + "','" + MaBenhVien + "'", new { });
           
            foreach (var item in listKPI)
            {
                Sheet.Cells[item.Hang, item.Cot].Value = item.SoLuong;
            }

            // dien thong tin benh vien
            {
                var result = await sqlDataAccess.getDataTable("select TenVietTat from BenhVien where MaBenhVien='" + MaBenhVien + "'");
                foreach (DataRow item in result.Rows)
                {
                    Sheet.Cells[16, 2].Value = item["TenVietTat"].ToString();
                    Sheet.Cells[18, 2].Value = item["TenVietTat"].ToString();
                    Sheet.Cells[24, 2].Value = item["TenVietTat"].ToString();
                    Sheet.Cells[25, 2].Value = item["TenVietTat"].ToString();
                    Sheet.Cells[26, 2].Value = item["TenVietTat"].ToString();
                    Sheet.Cells[27, 2].Value = item["TenVietTat"].ToString();
                    Sheet.Cells[31, 2].Value = item["TenVietTat"].ToString();
                    Sheet.Cells[32, 2].Value = item["TenVietTat"].ToString();
                    Sheet.Cells[33, 2].Value = item["TenVietTat"].ToString();
                    Sheet.Cells[34, 2].Value = item["TenVietTat"].ToString();
                    Sheet.Cells[35, 2].Value = item["TenVietTat"].ToString();
                    Sheet.Cells[40, 2].Value = item["TenVietTat"].ToString();
                    Sheet.Cells[41, 2].Value = item["TenVietTat"].ToString();
                    Sheet.Cells[42, 2].Value = item["TenVietTat"].ToString();
                    Sheet.Cells[43, 2].Value = item["TenVietTat"].ToString();

                }
            }
            if (Thang != 0)
            {
                Sheet.Cells[16, 1].Value = "Tháng này";
                Sheet.Cells[18, 1].Value = "Tháng trước";
            }

            if (LayDanhSachChiTiet)
            {
                try
                {
                    var SheetHenChuaKham = pkg.Workbook.Worksheets.Add("Hẹn chưa Khám");
                    var result = await sqlDataAccess.getDataTable("exec proc_AutoBenhNhanHenChuaKham '" + LayDenNgay.ToString("yyyy-MM-dd 23:59:59") + "','" + MaBenhVien + "'");
                    result.TableName = "HenChuaKham";
                    SheetHenChuaKham.Cells[1, 1].LoadFromDataTable(result, true, TableStyles.Light1);
                    SheetHenChuaKham.Cells.AutoFitColumns();
                }
                catch
                {

                }
                try
                {
                    var SheetHenChuaMo = pkg.Workbook.Worksheets.Add("Hẹn chưa mổ");
                    var resultChuaMo = await sqlDataAccess.getDataTable("exec proc_AutoBenhNhanHenChuaMo '" + LayDenNgay.ToString("yyyy-MM-dd 23:59:59") + "','" + MaBenhVien + "'");
                    resultChuaMo.TableName = "HenChuaMo";
                    SheetHenChuaMo.Cells[1, 1].LoadFromDataTable(resultChuaMo, true, TableStyles.Light2);
                    SheetHenChuaMo.Cells.AutoFitColumns();
                }
                catch
                {

                }
            }
        }

        public async Task<string> TaoDuLieuCPA(int nam, int thang, bool TheoNgay = false)
        {
            try
            {
                DateTime TuNgay = new DateTime(nam, thang, 1);
                DateTime DenNgay = TuNgay.AddMonths(1).AddMinutes(-1);
                if (TheoNgay)
                {
                    DenNgay = DateTimeHelp.LayTuan(DateTime.Now).TuNgay.AddMinutes(-1) ;
                }
                await sqlDataAccess.execNonQuery("exec proc_TaoDuLieuCPA '"+TuNgay.ToString("yyyy-MM-dd")+"','" + DenNgay.ToString("yyyy-MM-dd 23:59:57") +"'");
                return "Thành công";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public async Task<string> TaoDuLieuLeadFollow(int nam, int thang, bool TheoNgay = false)
        {
            try
            {
                DateTime TuNgay = new DateTime(nam, thang, 1);
                DateTime DenNgay = TuNgay.AddMonths(1).AddMinutes(-1);
                if (TheoNgay)
                {
                    DenNgay = DateTimeHelp.LayTuan(DateTime.Now).TuNgay.AddMinutes(-1);
                }

                await sqlDataAccess.execNonQuery("exec proc_CreateLeadsFollow '" + TuNgay.ToString("yyyy-MM-dd") + "','" + DenNgay.ToString("yyyy-MM-dd 23:59:57") + "'");
                return "Thành công";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> LuuThangGuiMail(int nam, int thang)
        {
            try
            {
                await sqlDataAccess.execNonQuery("insert into ThangGuiMail (Thang,Nam,Ngay) values (" + thang + "," + nam +",GetDate())");
                return "Thành công";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #region Bao cao tu dong
        #region Bao cao toan
        public async Task<ExcelPackage> TaoBaoCaoTuan (int tuan, int nam)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage pkg;
            pkg = new ExcelPackage();
            pkg.Workbook.Properties.Title = "Weekly Report";
            pkg.Workbook.Properties.Author = "Tran Thien Thanh 0908325345";
            pkg.Workbook.Properties.Subject = "Weekly Report";
            var filePath = "WeeklyReports.xlsx";
            ExcelWorksheet sheet;
            ExcelWorksheet sheet2;
            if (File.Exists(filePath))
            {
                var existingFile = new FileInfo(filePath);
                await pkg.LoadAsync(existingFile);
                sheet = pkg.Workbook.Worksheets[0];
                sheet2 = pkg.Workbook.Worksheets[1];                
            }
            else
            {
                sheet = pkg.Workbook.Worksheets.Add("Weekly Report");
                sheet2 = pkg.Workbook.Worksheets.Add("FollowupPatients");
            }

            const int StartLeadGenWeekRow = 6;
            const int StartLeadGenWeekCol = 1;
            string sql = "";
            DateTime DauTuan = HelperLib.DateTimeHelp.LayTuan(tuan, nam).TuNgay;
            for (int i = -7; i <= 0; i++)
            {
                var NgayTam = DauTuan.AddDays(i * 7);
                var tuanlelead = HelperLib.DateTimeHelp.LayTuan(NgayTam);
                sql = "Exec proc_AutoLeadGenWeek '"+ tuanlelead.TuNgay.ToString("yyyy-MM-dd") + "', '" + tuanlelead.DenNgay.ToString("yyyy-MM-dd 23:59:59") + "'"; 
                var resultslead =  await sqlDataAccess.loadData<LeadGenWeekVM, dynamic>(sql, new { });
                sheet.Cells[StartLeadGenWeekRow + (i * 2 + 14), StartLeadGenWeekCol].Value = "W" + (tuan + i);
                sheet.Cells[StartLeadGenWeekRow + (i * 2 + 15), StartLeadGenWeekCol].Value = tuanlelead.TuNgay.ToString("dd/MM") + "-" + tuanlelead.DenNgay.ToString("dd/MM/yyyy");
                foreach ( var item in resultslead)
                {                    
                    sheet.Cells[StartLeadGenWeekRow + (i*2 +14), StartLeadGenWeekCol+item.Cot-1].Value = item.SoLuong;
                }
            }
            const int StartExamWeekRow = 4;
            const int StartExamWeekCol = 13;
            var tuanle = HelperLib.DateTimeHelp.LayTuan(tuan, nam);
            sql = "Exec proc_AutoExamWeek '" + tuanle.TuNgay.ToString("yyyy-MM-dd") + "', '" + tuanle.DenNgay.ToString("yyyy-MM-dd 23:59:59") + "'"; 
            var resultsExam = await sqlDataAccess.loadData<ExamWeekVM, dynamic>(sql, new { });            
            sheet.Cells[StartExamWeekRow-3, StartExamWeekCol].Value = "Exam case by Hospital WoW " + tuan + "/" + nam + " (" + tuanle.TuNgay.ToString("dd/MM") + " - " + tuanle.DenNgay.ToString("dd/ MM/yyyy") + ")";
            int ExamWeekCol = 0;
            string TenVietTat = "";
            foreach (var item in resultsExam)
            {
                if (item.TenVietTat!= TenVietTat)
                {
                    if (TenVietTat!="")
                    {
                        ExamWeekCol++;
                    }
                    TenVietTat = item.TenVietTat;
                    sheet.Cells[StartExamWeekRow + ExamWeekCol, StartExamWeekCol].Value = TenVietTat;
                }
                sheet.Cells[StartExamWeekRow + ExamWeekCol, StartExamWeekCol + item.Cot-1].Value = item.SoLuong;
            }
            const int StartSurWeekRow = 21;
            const int StartSurWeekCol = 13;
            sql = "Exec proc_AutoSurWeek '" + tuanle.TuNgay.ToString("yyyy-MM-dd") + "', '" + tuanle.DenNgay.ToString("yyyy-MM-dd 23:59:59") + "'";
            var resultsSur = await sqlDataAccess.loadData<ExamWeekVM, dynamic>(sql, new { });            
            sheet.Cells[StartSurWeekRow - 3, StartSurWeekCol].Value = "Sur case by Hospital W-o-W " + tuan + "/" + nam + " (" + tuanle.TuNgay.ToString("dd/MM") + " - " + tuanle.DenNgay.ToString("dd/ MM/yyyy") + ")";
            int SurRow = 0;
            string TenVietTatSur = "";
            foreach (var item in resultsSur)
            {
                if (item.TenVietTat != TenVietTatSur)
                {
                    if (TenVietTatSur != "")
                    {
                        SurRow++;
                    }
                    TenVietTatSur = item.TenVietTat;
                    sheet.Cells[StartSurWeekRow + SurRow, StartSurWeekCol].Value = TenVietTatSur;
                }
                sheet.Cells[StartSurWeekRow + SurRow, StartSurWeekCol + item.Cot-1].Value = item.SoLuong;
            }

            if (useVer2)
            {
                {
                    const int StartRowWeeklyYTD = 5;
                    const int StartColWeeklyYTD = 2;
                    int curRow = 0;
                    TenVietTat = "";
                    sql = "Exec proc_AutoWeeklyBookYTD '" + tuanle.DenNgay.ToString("yyyy-MM-dd 23:59:59") + "'";

                    var result = await sqlDataAccess.loadData<MonthlyReportVM, dynamic>(sql, new { });
                    foreach (var item in result)
                    {
                        if (item.TenVietTat != TenVietTat)
                        {
                            if (TenVietTat != "")
                            {
                                curRow++;
                            }
                            TenVietTat = item.TenVietTat;
                            sheet2.Cells[StartRowWeeklyYTD + curRow, StartColWeeklyYTD ].Value = TenVietTat;
                        }
                        if (item.Cot > 0)
                        {
                            sheet2.Cells[StartRowWeeklyYTD + curRow, StartColWeeklyYTD + item.Cot - 1].Value = item.SoLuong;
                        }
                    }
                }

                {
                    const int HangBatDau = 20;
                    const int CotBatDau = 2;
                    int curRow = 0;
                    TenVietTat = "";
                    sql = "Exec proc_AutoWeeklyBookChuaKhamTheoThang '" + tuanle.DenNgay.ToString("yyyy-MM-dd 23:59:59") + "'";

                    var result = await sqlDataAccess.loadData<MonthlyReportVM, dynamic>(sql, new { });
                    foreach (var item in result)
                    {
                        if (item.TenVietTat != TenVietTat)
                        {
                            if (TenVietTat != "")
                            {
                                curRow++;
                            }
                            TenVietTat = item.TenVietTat;
                            sheet2.Cells[HangBatDau + curRow, CotBatDau].Value = TenVietTat;
                        }
                        if (item.Cot > 0)
                        {
                            sheet2.Cells[HangBatDau + curRow, CotBatDau + item.Cot].Value = item.SoLuong;
                        }
                    }
                }

                {
                    const int HangBatDau = 34;
                    const int CotBatDau = 2;
                    int curRow = 0;
                    TenVietTat = "";
                    sql = "Exec proc_AutoWeeklyHenChuaKhamTheoThang '" + tuanle.DenNgay.ToString("yyyy-MM-dd 23:59:59") + "'";

                    var result = await sqlDataAccess.loadData<MonthlyReportVM, dynamic>(sql, new { });
                    foreach (var item in result)
                    {
                        if (item.TenVietTat != TenVietTat)
                        {
                            if (TenVietTat != "")
                            {
                                curRow++;
                            }
                            TenVietTat = item.TenVietTat;
                            sheet2.Cells[HangBatDau + curRow, CotBatDau].Value = TenVietTat;
                        }
                        if (item.Cot > 0)
                        {
                            sheet2.Cells[HangBatDau + curRow, CotBatDau + item.Cot].Value = item.SoLuong;
                        }
                    }
                }

                {
                    const int HangBatDau = 48;
                    const int CotBatDau = 2;
                    int curRow = 0;
                    TenVietTat = "";
                    sql = "Exec proc_AutoWeeklyHenMoChuaMoTheoThang '" + tuanle.DenNgay.ToString("yyyy-MM-dd 23:59:59") + "'";

                    var result = await sqlDataAccess.loadData<MonthlyReportVM, dynamic>(sql, new { });
                    foreach (var item in result)
                    {
                        if (item.TenVietTat != TenVietTat)
                        {
                            if (TenVietTat != "")
                            {
                                curRow++;
                            }
                            TenVietTat = item.TenVietTat;
                            sheet2.Cells[HangBatDau + curRow, CotBatDau].Value = TenVietTat;
                        }
                        if (item.Cot > 0)
                        {
                            sheet2.Cells[HangBatDau + curRow, CotBatDau + item.Cot].Value = item.SoLuong;
                        }
                    }
                }
            }
            return pkg;
        }

        #endregion

        #region Bao cao thang
        public async Task<ExcelPackage> TaoBaoCaoThang(int thang, int nam, DateTime Ngay, bool TinhTheoNgay = false)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage pkg;
            pkg = new ExcelPackage();
            pkg.Workbook.Properties.Title = "Monthly Report";
            pkg.Workbook.Properties.Author = "Tran Thien Thanh 0908325345";
            pkg.Workbook.Properties.Subject = "Monthly Report";
            var filePath = "MonthlyReport.xlsx";
            var TuNgay = new DateTime(nam, thang, 1);
            ExcelWorksheet sheet;
            if (File.Exists(filePath))
            {
                var existingFile = new FileInfo(filePath);
                await pkg.LoadAsync(existingFile);
                sheet = pkg.Workbook.Worksheets[0];
            }
            else
            {
                sheet = pkg.Workbook.Worksheets.Add("Monthly Report");
            }

            if (TinhTheoNgay)
            {
                // Tinh ngay                
                sheet.Cells[1, 2].Value = "Note: " + new DateTime(nam, thang, 1).ToString("MMM", CultureInfo.CreateSpecificCulture("en-US")) + "(" + Ngay.ToString("MM/dd/yyyy") + ")";
                thang = Ngay.Month;
                nam = Ngay.Year;
            }
            else
            {
                Ngay = TuNgay.AddMonths(1).AddDays(-1);
            }

            {
                const int StartRowExamDigitalVSWalk = 5;
                const int StartColExamDigitalVSWalk = 2;
                int ExamWeekCol = 0;
                string TenVietTat = "";
                sheet.Cells[StartRowExamDigitalVSWalk - 2, StartColExamDigitalVSWalk + 1].Value = new DateTime(nam, thang, 1).ToString("MMM", CultureInfo.CreateSpecificCulture("en-US"));

                string sql = "Exec proc_AutoExamDigitalVSWalkMonth '"+TuNgay.ToString("yyyy-MM-dd") + "','" + Ngay.ToString("yyyy-MM-dd 23:59:59") + "'";
                var resultExamDigitalVSWalk = await sqlDataAccess.loadData<MonthlyReportVM, dynamic>(sql, new { });
                foreach (var item in resultExamDigitalVSWalk)
                {
                    if (item.TenVietTat != TenVietTat)
                    {
                        if (TenVietTat != "")
                        {
                            ExamWeekCol++;
                        }
                        TenVietTat = item.TenVietTat;
                        sheet.Cells[StartRowExamDigitalVSWalk + ExamWeekCol, StartColExamDigitalVSWalk].Value = TenVietTat;
                    }
                    sheet.Cells[StartRowExamDigitalVSWalk + ExamWeekCol, StartColExamDigitalVSWalk + item.Cot - 1].Value = item.SoLuong;
                }
            }

            {
                const int StartRowSurDigitalVSWalk = 20;
                const int StartColSurDigitalVSWalk = 2;
                int SurWeekCol = 0;
                string TenVietTat = "";
                sheet.Cells[StartRowSurDigitalVSWalk - 2, StartColSurDigitalVSWalk + 1].Value = new DateTime(nam, thang, 1).ToString("MMM", CultureInfo.CreateSpecificCulture("en-US"));

                string sql = "Exec proc_AutoSurDigitalVSWalkMonth '" + TuNgay.ToString("yyyy-MM-dd") + "','" + Ngay.ToString("yyyy-MM-dd 23:59:59") + "'";
                var resultExamDigitalVSWalk = await sqlDataAccess.loadData<MonthlyReportVM, dynamic>(sql, new { });
                foreach (var item in resultExamDigitalVSWalk)
                {
                    if (item.TenVietTat != TenVietTat)
                    {
                        if (TenVietTat != "")
                        {
                            SurWeekCol++;
                        }
                        TenVietTat = item.TenVietTat;
                        sheet.Cells[StartRowSurDigitalVSWalk + SurWeekCol, StartColSurDigitalVSWalk].Value = TenVietTat;
                    }
                    sheet.Cells[StartRowSurDigitalVSWalk + SurWeekCol, StartColSurDigitalVSWalk + item.Cot - 1].Value = item.SoLuong;
                }
            }

            {
                const int StartRowConvertMonth = 5;
                const int StartColConvertMonth = 9;
                int ConvertCol = 0;
                string TenVietTat = "";
                sheet.Cells[StartRowConvertMonth - 2, StartColConvertMonth].Value = new DateTime(nam, thang, 1).ToString("MMM", CultureInfo.CreateSpecificCulture("en-US"));

                string sql = "Exec proc_AutoConvertMonth '" + TuNgay.ToString("yyyy-MM-dd") + "','" + Ngay.ToString("yyyy-MM-dd 23:59:59") + "'";
                var resultExamDigitalVSWalk = await sqlDataAccess.loadData<MonthlyReportVM, dynamic>(sql, new { });
                foreach (var item in resultExamDigitalVSWalk)
                {
                    if (item.TenVietTat != TenVietTat)
                    {
                        if (TenVietTat != "")
                        {
                            ConvertCol++;
                        }
                        TenVietTat = item.TenVietTat;
                        //sheet.Cells[StartRowConvertMonth + ConvertCol, StartColConvertMonth].Value = TenVietTat;
                    }
                    sheet.Cells[StartRowConvertMonth + ConvertCol, StartColConvertMonth + item.Cot - 1].Value = item.SoLuong;
                }
            }

            {
                const int StartRowSurMonth = 19;
                const int StartColSurMonth = 9;
                const int lastRow = 11;
                int SurMonthCol = 0;
                string TenVietTat = "";
                
                sheet.Cells[StartRowSurMonth + lastRow-1, StartColSurMonth].Value = "GROUP " + new DateTime(nam, thang, 1).ToString("MMM", CultureInfo.CreateSpecificCulture("en-US"));
                if (thang==1)
                {
                    sheet.Cells[StartRowSurMonth + lastRow, StartColSurMonth].Value = "GROUP "  + new DateTime(nam-1, 12, 1).ToString("MMM", CultureInfo.CreateSpecificCulture("en-US"));
                }
                else
                {
                    sheet.Cells[StartRowSurMonth + lastRow, StartColSurMonth].Value = "GROUP " + new DateTime(nam, thang-1, 1).ToString("MMM", CultureInfo.CreateSpecificCulture("en-US"));
                }

                string sql = "Exec proc_AutoSurMonth '" + TuNgay.ToString("yyyy-MM-dd") + "','" + Ngay.ToString("yyyy-MM-dd 23:59:59") + "'";
                var resultExamDigitalVSWalk = await sqlDataAccess.loadData<MonthlyReportVM, dynamic>(sql, new { });
                foreach (var item in resultExamDigitalVSWalk)
                {
                    if (item.TenVietTat== null || item.TenVietTat == "")
                    {
                        sheet.Cells[StartRowSurMonth + lastRow, StartColSurMonth + item.Cot - 1].Value = item.SoLuong;
                    }
                    else if(item.TenVietTat== "GroupYTD")
                    {
                        sheet.Cells[StartRowSurMonth + lastRow+1, StartColSurMonth + item.Cot - 1].Value = item.SoLuong;
                    }
                    else
                    {
                        if (item.TenVietTat != TenVietTat)
                        {
                            if (TenVietTat != "")
                            {
                                SurMonthCol++;
                            }
                            TenVietTat = item.TenVietTat;
                            sheet.Cells[StartRowSurMonth + SurMonthCol, StartColSurMonth].Value = TenVietTat;
                        }
                        sheet.Cells[StartRowSurMonth + SurMonthCol, StartColSurMonth + item.Cot - 1].Value = item.SoLuong;
                    }
                }
            }

            {
                const int StartRowSurActualVSTarget = 37;
                const int StartColSurActualVSTarget = 2;                
                int ConvertCol = 0;
                string TenVietTat = "";

                sheet.Cells[StartRowSurActualVSTarget - 3, StartColSurActualVSTarget + 1].Value = new DateTime(nam, thang, 1).ToString("MMM", CultureInfo.CreateSpecificCulture("en-US"));                

                string sql = "Exec proc_AutoSurActualVSTargetMonth '" + TuNgay.ToString("yyyy-MM-dd") + "','" + Ngay.ToString("yyyy-MM-dd 23:59:59") + "'";
                var resultExamDigitalVSWalk = await sqlDataAccess.loadData<MonthlyReportVM, dynamic>(sql, new { });
                foreach (var item in resultExamDigitalVSWalk)
                {                   
                    if (item.TenVietTat != TenVietTat)
                    {
                        if (TenVietTat != "")
                        {
                            ConvertCol++;
                        }
                        TenVietTat = item.TenVietTat;
                        sheet.Cells[StartRowSurActualVSTarget + ConvertCol, StartColSurActualVSTarget].Value = TenVietTat;
                    }
                    sheet.Cells[StartRowSurActualVSTarget + ConvertCol, StartColSurActualVSTarget + item.Cot - 1].Value = item.SoLuong;
                }
            }

            {
                const int StartRowSurbyMonth = 50;
                const int StartColSurbyMonth = 3;
                int SurCol = 0;
                string TenVietTat = "";
                

                string sql = "Exec proc_AutoSurByMonth '" + TuNgay.ToString("yyyy-MM-dd") + "','" + Ngay.ToString("yyyy-MM-dd 23:59:59") + "'";
                var resultExamDigitalVSWalk = await sqlDataAccess.loadData<MonthlyReportVM, dynamic>(sql, new { });
                foreach (var item in resultExamDigitalVSWalk)
                {
                    if (item.TenVietTat != TenVietTat)
                    {
                        if (TenVietTat != "")
                        {
                            SurCol++;
                        }
                        TenVietTat = item.TenVietTat;
                        sheet.Cells[StartRowSurbyMonth + SurCol, StartColSurbyMonth-1].Value = TenVietTat;
                    }
                    sheet.Cells[StartRowSurbyMonth + SurCol, StartColSurbyMonth + item.Cot - 1].Value = item.SoLuong;

                }
            }

            {
                const int StartRowExamByMonth = 54;
                const int StartColExamByMonth = 3;
                int currol = 0;
                string TenVietTat = "";

                string sql = "Exec proc_AutoExamByMonth '" + TuNgay.ToString("yyyy-MM-dd") + "','" + Ngay.ToString("yyyy-MM-dd 23:59:59") + "'";
                var result = await sqlDataAccess.loadData<MonthlyReportVM, dynamic>(sql, new { });
                foreach (var item in result)
                {
                    if (item.TenVietTat != TenVietTat)
                    {
                        if (TenVietTat != "")
                        {
                            currol++;
                        }
                        TenVietTat = item.TenVietTat;
                        sheet.Cells[StartRowExamByMonth + currol, StartColExamByMonth-1].Value = TenVietTat;
                    }
                    sheet.Cells[StartRowExamByMonth + currol, StartColExamByMonth + item.Cot - 1].Value = item.SoLuong;
                }

                // sửa lại char ChartGroupExamSur
                // 
                //ExcelChart chart = sheet.Drawings["ChartGroupExamSur"] as ExcelChart;
                //chart.Series[0].Series = sheet.Cells[StartRowExamByMonth, StartColExamByMonth, StartRowExamByMonth, StartColExamByMonth+thang].FullAddress;
                //chart.Series[1].Series = sheet.Cells[StartRowExamByMonth-2, StartColExamByMonth, StartRowExamByMonth-2, StartColExamByMonth + thang].FullAddress;
                //chart.Series[2].Series = sheet.Cells[StartRowExamByMonth +1, StartColExamByMonth, StartRowExamByMonth +1, StartColExamByMonth + thang].FullAddress;

            }
            return pkg;
        }
        #endregion
        #region Gui mail tu dong
        public async Task<bool> CheckDaGuiMailTuan(int tuanBC, int namBC)
        {
            string sql = "select * from ThoiGianGuiMailWeeklys where KetQua=" + (int)KetQuaGuiMail.ThanhCong + " and Tuan=" + tuanBC + " and Nam=" + namBC;

            var result = await sqlDataAccess.loadData<ThoiGianGuiMailWeekly, dynamic>(sql, new { });
            if (result!= null && result.Count>0)
            {
                return true;
            }
            return false;
        }
        public async Task LuuThongTinGuiMailTuan(int tuanBC, int namBC, KetQuaGuiMail thanhCong)
        {
            string sql = @"insert into ThoiGianGuiMailWeeklys (            
                        Tuan
                        ,Nam
                        ,NgayGui
                        ,KetQua)
                        values
                        (
                        " + tuanBC + @"
                        ," + namBC+ @"
                        ,'" +DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + @"'
                        ," + (int)thanhCong+ @")";
            await sqlDataAccess.execNonQuery(sql);
        }
        public async Task LuuThongTinGuiMailFollowupPatientTuanBenhVien(int tuanBC, int namBC, KetQuaGuiMail thanhCong, string MaBenhVien)
        {
            string sql = @"insert into ThoiGianGuiMailWeeklyBenhViens (            
                        Tuan
                        ,Nam
                        ,NgayGui
                        ,KetQua
                        ,MaBenhVien)
                        values
                        (
                        " + tuanBC + @"
                        ," + namBC + @"
                        ,'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + @"'
                        ," + (int)thanhCong 
                        +",'" + MaBenhVien
                        + @"')";
            await sqlDataAccess.execNonQuery(sql);
        }
        public async Task LuuThongTinGuiMailFollowupPatientThangBenhVien(int Thang, int namBC, KetQuaGuiMail thanhCong, string MaBenhVien)
        {
            string sql = @"insert into ThoiGianGuiMailWeeklyBenhViens (            
                        Tuan
                        ,Thang
                        ,Nam
                        ,NgayGui
                        ,KetQua
                        ,MaBenhVien)
                        values
                        (
                        0
                        ," + Thang + @"
                        ," + namBC + @"
                        ,'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + @"'
                        ," + (int)thanhCong
                        + ",'" + MaBenhVien
                        + @"')";
            await sqlDataAccess.execNonQuery(sql);
        }
        public async Task<bool> CheckDaGuiMailThang(int thangBC, int namBC)
        {
            string sql = "select * from ThoiGianGuiMailMonthlys where KetQua=" + (int)KetQuaGuiMail.ThanhCong + " and Thang=" + thangBC + " and Nam=" + namBC;
            var result = await sqlDataAccess.loadData<ThoiGianGuiMailMonthly, dynamic>(sql, new { });
            if (result != null && result.Count > 0)
            { 
                return true;
            }
            return false;
        }
        public async Task<bool> CheckDaGuiMailThangTheoTuan(int tuanBC, int namBC)
        {
            string sql = "select * from ThoiGianGuiMailMonthlys where KetQua=" + (int)KetQuaGuiMail.ThanhCong + " and Tuan=" + tuanBC + " and Nam=" + namBC;
            var result = await sqlDataAccess.loadData<ThoiGianGuiMailMonthly, dynamic>(sql, new { });
            if (result != null && result.Count > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> CheckDaGuiMailCPAAndCallTheoTuan(int tuanBC, int namBC, string MaBenhVien="O")
        {
            string sql = "select * from ThoiGianGuiMailCPAs where KetQua=" + (int)KetQuaGuiMail.ThanhCong + " and Tuan=" + tuanBC + " and Nam=" + namBC + " and MaBenhVien='"+ MaBenhVien +"'";
            var result = await sqlDataAccess.loadData<ThoiGianGuiMailMonthly, dynamic>(sql, new { });
            if (result != null && result.Count > 0)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> CheckDaGuiMailCPAAndCall(int thangBC, int namBC, string MaBenhVien = "O")
        {
            string sql = "select * from ThoiGianGuiMailCPAs where KetQua=" + (int)KetQuaGuiMail.ThanhCong + " and Thang=" + thangBC + " and Nam=" + namBC + " and MaBenhVien='" + MaBenhVien + "'";
            var result = await sqlDataAccess.loadData<ThoiGianGuiMailMonthly, dynamic>(sql, new { });
            if (result != null && result.Count > 0)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> CheckDaGuiMailFollowupPatientTuanBenhVien(int tuanBC, int namBC, string MaBenhVien)
        {
            string sql = "select * from ThoiGianGuiMailWeeklyBenhViens where KetQua=" + (int)KetQuaGuiMail.ThanhCong + " and Tuan=" + tuanBC + " and Nam=" + namBC + " and MaBenhVien='"+ MaBenhVien +"'";
            var result = await sqlDataAccess.loadData<ThoiGianGuiMailMonthly, dynamic>(sql, new { });
            if (result != null && result.Count > 0)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> CheckDaGuiMailFollowupPatientThangBenhVien(int thangBC, int namBC, string MaBenhVien)
        {
            string sql = "select * from ThoiGianGuiMailWeeklyBenhViens where KetQua=" + (int)KetQuaGuiMail.ThanhCong + " and Thang=" + thangBC + " and Nam=" + namBC + " and MaBenhVien='" + MaBenhVien + "'";
            var result = await sqlDataAccess.loadData<ThoiGianGuiMailMonthly, dynamic>(sql, new { });
            if (result != null && result.Count > 0)
            {
                return true;
            }
            return false;
        }
        public async Task LuuThongTinGuiMailThang(int thangBC, int namBC, KetQuaGuiMail thanhCong)
        {
            string sql = @"insert into ThoiGianGuiMailMonthlys (            
                        Thang
                        ,Nam
                        ,NgayGui
                        ,KetQua )
                        values
                        (
                        " + thangBC + @"
                        ," + namBC + @"
                        ,'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + @"'
                        ," + (int)thanhCong + @")";
            await sqlDataAccess.execNonQuery(sql);
        }
        public async Task LuuThongTinGuiMailThangTheoTuan(int tuanBC, int namBC, KetQuaGuiMail thanhCong)
        {
            string sql = @"insert into ThoiGianGuiMailMonthlys (            
                        Tuan
                        ,Nam
                        ,NgayGui
                        ,KetQua )
                        values
                        (
                        " + tuanBC + @"
                        ," + namBC + @"
                        ,'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + @"'
                        ," + (int)thanhCong + @")";
            await sqlDataAccess.execNonQuery(sql);
        }


        public async Task LuuThongTinGuiMailCPA(int thangBC, int namBC, KetQuaGuiMail thanhCong, string MaBenhVien ="O")
        {
            string sql = @"insert into ThoiGianGuiMailCPAs (            
                        Thang
                        ,Nam
                        ,NgayGui
                        ,KetQua
                        , MaBenhVien)
                        values
                        (
                        " + thangBC + @"
                        ," + namBC + @"
                        ,'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + @"'
                        ," + (int)thanhCong + @"
                        ,'" + MaBenhVien + "'"
                        + @")";
            await sqlDataAccess.execNonQuery(sql);
        }
        public async Task LuuThongTinGuiMailCPATheoTuan(int tuanBC, int namBC, KetQuaGuiMail thanhCong, string MaBenhVien = "O")
        {
            string sql = @"insert into ThoiGianGuiMailCPAs (            
                        Tuan
                        ,Nam
                        ,NgayGui
                        ,KetQua
                        , MaBenhVien)
                        values
                        (
                        " + tuanBC + @"
                        ," + namBC + @"
                        ,'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + @"'
                        ," + (int)thanhCong + @"
                        ,'" + MaBenhVien + "'"
                        + @")";
            await sqlDataAccess.execNonQuery(sql);
        }
        #endregion
        #endregion

        public async Task<List<ThongTinGuiMailBenhVien>> LayThongTinGuiMailWeeklyBenhVien(string MaBenhVien="")
        {
            string sql = "select * from ThongTinGuiMailBenhViens where MaBenhVien='" + MaBenhVien +"'";
            if (string.IsNullOrEmpty(MaBenhVien))
            {
                sql = "select * from ThongTinGuiMailBenhViens";
            }
         
            return await sqlDataAccess.loadData<ThongTinGuiMailBenhVien, dynamic>(sql, new { });
            //return await sdfsdf context.ThongTinGuiMailBenhViens.ToListAsync();
        }

        public async Task<List<ThongTinGuiMailBenhVien>> LayThongTinGuiMailCPAAndCallBenhVien(string MaBenhVien)
        {
            string sql = "select * from ThongTinGuiMaiCPAAndCallBenhViens where MaBenhVien='"+ MaBenhVien +"'";
            return await sqlDataAccess.loadData<ThongTinGuiMailBenhVien, dynamic>(sql, new { });

            //return await context.ThongTinGuiMaiCPAAndCallBenhViens.ToListAsync();
        }

        #region Gửi mail Daily
        public async Task<ThongTinGuiMailBenhVien> LayThongTinGuiMailDailyTuan(string MaBenhVien="O" , LoaiDailyReportTuan LoaiReport = LoaiDailyReportTuan.Mat)
        {
            string sql = "select * from ThongTinGuiMailDailyTuan where MaBenhVien='"+MaBenhVien+"' and  isnull(LoaiReport,0)=" + (int)LoaiReport;
            var result = await sqlDataAccess.loadData<ThongTinGuiMailBenhVien, dynamic>(sql, new { });
            return result.FirstOrDefault();
        }

        public async Task<ThongTinGuiMailBenhVien> LayThongTinGuiMailWeekly(string MaBenhVien ="O")
        {
            string sql = "select * from ThongTinGuiMailWeekly Where MaBenhVien='"+MaBenhVien+"'";
            var result = await sqlDataAccess.loadData<ThongTinGuiMailBenhVien, dynamic>(sql, new { });
            return result.FirstOrDefault();
        }

        public async Task<ThongTinGuiMailBenhVien> LayThongTinGuiMailMonthly(string MaBenhVien = "O")
        {
            string sql = "select * from ThongTinGuiMailMonthly Where MaBenhVien='" + MaBenhVien + "'";
            var result = await sqlDataAccess.loadData<ThongTinGuiMailBenhVien, dynamic>(sql, new { });
            return result.FirstOrDefault();
        }

        public async Task<ThongTinGuiMailBenhVien> LayThongTinGuiMailDailyBenhVien(string MaBenhVien)
        {
            string sql = @" select GuiDen as SendTo, CC as CCTo, NoiGui as GuiTu
                            from ThongTinGuiMailDaiLy
                            where MaBenhVien = '" + MaBenhVien+"'";
            var result = await sqlDataAccess.loadData<ThongTinGuiMailBenhVien, dynamic>(sql, new { });
            return result.FirstOrDefault();
        }

        public async Task LuuThongTinGuiMailDailyTuan(LoaiDailyReportTuan loaiDailyReportTuan, int tuan, int nam, KetQuaGuiMail loi)
        {
            string sql = @"insert into ThoiGianGuiMailDailyTuan (            
                        Tuan
                        ,Nam
                        ,NgayGui
                        ,KetQua
                        , LoaiReport)
                        values
                        (
                        " + tuan + @"
                        ," + nam + @"
                        ,'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + @"'
                        ," + (int)loi + @"
                        ," + (int)loaiDailyReportTuan
                        + @")";
            await sqlDataAccess.execNonQuery(sql);
        }
        public async Task LuuThongTinGuiMailDailyBenhVien(string MaBenhVien)
        {
            string sql = @"insert into ThoiGianGuiDaiLyReport (            
                        ThoiGian
                        ,Loai
                        ,MaBenhVien)
                        values
                        (
                        '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + @"'
                        ," + 1 + @"
                        ,'" + MaBenhVien + @"')";
            await sqlDataAccess.execNonQuery(sql);
        }

        public async Task<bool> CheckDaGuiThongTinGuiMailDailyTuan(LoaiDailyReportTuan loaiDailyReportTuan, int tuan, int nam)
        {
            string sql = "select * from ThoiGianGuiMailDailyTuan where KetQua=" + (int)KetQuaGuiMail.ThanhCong + " and Tuan=" + tuan + " and Nam=" + nam + " and LoaiReport =" + (int)loaiDailyReportTuan ;
            var result = await sqlDataAccess.loadData<ThoiGianGuiMailMonthly, dynamic>(sql, new { });
            if (result != null && result.Count > 0)
            {
                return true;
            }
            return false;

        }
        public async Task<bool> CheckDaGuiMailDailyBenhVien(string MaBenhVien)
        {
            string sql = @" select * from ThoiGianGuiDaiLyReport
                            where MaBenhVIen = '"+MaBenhVien+"' and Loai = 1 and DateDiff(day,ThoiGian,'"+ DateTime.Now.ToString("yyyy-MM-dd") +"') = 0";
            var result = await sqlDataAccess.getDataTable(sql);
            if (result != null && result.Rows.Count> 0)
            {
                return true;
            }
            return false;
        }

        #endregion

        public async Task<DataTable> GetBookingTable(DateTime tuNgay, DateTime denNgay, string maBenhVien)
        {
            string sql = "exec proc_AutoGetBenhNhanhBooking '" + tuNgay.ToString("yyyy-MM-dd") + "','" + denNgay.ToString("yyyy-MM-dd 23:59:59") + "',N'" + maBenhVien + "'";
            return await sqlDataAccess.getDataTable(sql);
        }

        public async Task<string> GetTenVietTatBenhVien(string MaBenhVien)
        {
            string sql = "select TenVietTat from BenhVien where MaBenhVien = '" + MaBenhVien + "'";
            try
            {
                return (await sqlDataAccess.getDataTable(sql)).Rows[0][0].ToString();
            }
            catch
            {

            }
            return "";
        }

        public async Task<ExcelPackage> GetChiTietLeadsChuaBook(DateTime TuNngay, DateTime DenNgay, string maBenhVien="O")
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var pkg = new ExcelPackage();
            pkg.Workbook.Properties.Title = "Report BenhNhan kham";
            pkg.Workbook.Properties.Author = "Tran Thien Thanh 0908325345";
            pkg.Workbook.Properties.Subject = "Leads chưa khám";
            int SoLuongLeadDaBookRowTrongTuan = 0;           
            int SoLuongLeadSoSanhRowTrongTuan = 0;
            int SoLuongHangTrongTuan = 0;
            int SoLuongCotTrongTuan = 0;

            int SoLuongLeadChuaBookRowYTD = 0;
            int SoLuongLeadDaBookRowYTD = 0;
            int SoLuongLeadSoSanhRowYTD = 0;
            int SoLuongHangYTD = 0;
            int SoLuongCotYTD = 0;

            var SheetSum = pkg.Workbook.Worksheets.Add("Summary");
            {
                var DanhSachTongChuaBook = await sqlDataAccess.getDataTable("exec proc_SaleAutoGetSumLeadsChuaBook '" + TuNngay.ToString("yyyy-MM-dd") + "','" + DenNgay.ToString("yyyy-MM-dd 23:59:59") + "','" + maBenhVien + "'");


                DanhSachTongChuaBook.TableName = "SummaryChuaBook";
                SheetSum.Cells[1, 1].Value = "SỐ LƯỢNG LEAD CHƯA BOOK TRONG TUẦN";
                SheetSum.Cells[1, 1].Style.Font.Bold = true;
                SheetSum.Cells[2, 1].LoadFromDataTable(DanhSachTongChuaBook, true, TableStyles.Light2);

                for (int i = 0; i < DanhSachTongChuaBook.Rows.Count; i++)
                {
                    SheetSum.Cells[i + 2, DanhSachTongChuaBook.Columns.Count].Style.Font.Bold = true;
                }
                SoLuongLeadDaBookRowTrongTuan = DanhSachTongChuaBook.Rows.Count + 5;
                SoLuongHangTrongTuan = DanhSachTongChuaBook.Rows.Count;
                SoLuongCotTrongTuan = DanhSachTongChuaBook.Columns.Count;

                // Tính dòng tổng
                for (int i = 1; i < DanhSachTongChuaBook.Columns.Count; i++)
                {
                    SheetSum.Cells[DanhSachTongChuaBook.Rows.Count + 3, i+1 ].Formula = "=sum(" + SheetSum.Cells[3, i+1].Address + ":" + SheetSum.Cells[DanhSachTongChuaBook.Rows.Count + 2, i+1].Address + ")";
                    
                }
                SheetSum.Cells[DanhSachTongChuaBook.Rows.Count + 3, 1].Value = "Tổng";
                SheetSum.Cells[DanhSachTongChuaBook.Rows.Count + 3, 1, DanhSachTongChuaBook.Rows.Count + 3, DanhSachTongChuaBook.Columns.Count].Style.Font.Bold =true;
                SheetSum.Cells[DanhSachTongChuaBook.Rows.Count + 3, 1, DanhSachTongChuaBook.Rows.Count + 3, DanhSachTongChuaBook.Columns.Count].Style.Font.Color.SetColor(Color.Blue);
                SheetSum.Cells[DanhSachTongChuaBook.Rows.Count + 3, 1, DanhSachTongChuaBook.Rows.Count + 3, DanhSachTongChuaBook.Columns.Count].Style.Border.BorderAround(ExcelBorderStyle.Thin);

            }

            {
                var DanhSachTongDaBook = await sqlDataAccess.getDataTable("exec proc_SaleAutoGetSumLeadsDaBook '" + TuNngay.ToString("yyyy-MM-dd") + "','" + DenNgay.ToString("yyyy-MM-dd 23:59:59") + "','" + maBenhVien + "'");
                DanhSachTongDaBook.TableName = "SummaryDaBook";
                SheetSum.Cells[SoLuongLeadDaBookRowTrongTuan, 1].Value = "SỐ LƯỢNG LEADS ĐÃ BOOK TRONG TUẦN";
                SheetSum.Cells[SoLuongLeadDaBookRowTrongTuan, 1].Style.Font.Bold = true;
                SheetSum.Cells[SoLuongLeadDaBookRowTrongTuan+1, 1].LoadFromDataTable(DanhSachTongDaBook, true, TableStyles.Light3);

                for (int i = 0; i < DanhSachTongDaBook.Rows.Count; i++)
                {
                    SheetSum.Cells[i + SoLuongLeadDaBookRowTrongTuan + 1, DanhSachTongDaBook.Columns.Count].Style.Font.Bold = true;
                }
                SoLuongLeadSoSanhRowTrongTuan = SoLuongLeadDaBookRowTrongTuan + DanhSachTongDaBook.Rows.Count + 4;
                // Tính dòng tổng
                for (int i = 1; i < DanhSachTongDaBook.Columns.Count; i++)
                {
                    SheetSum.Cells[SoLuongLeadDaBookRowTrongTuan+DanhSachTongDaBook.Rows.Count + 2, i+1].Formula = "=sum(" + SheetSum.Cells[SoLuongLeadDaBookRowTrongTuan+2, i+1].Address + ":" + SheetSum.Cells[SoLuongLeadDaBookRowTrongTuan+DanhSachTongDaBook.Rows.Count + 1, i+1].Address + ")";
                    
                }
                SheetSum.Cells[SoLuongLeadDaBookRowTrongTuan + DanhSachTongDaBook.Rows.Count + 2, 1].Value = "Tổng";
                SheetSum.Cells[SoLuongLeadDaBookRowTrongTuan+DanhSachTongDaBook.Rows.Count + 2, 1, SoLuongLeadDaBookRowTrongTuan+DanhSachTongDaBook.Rows.Count + 2, DanhSachTongDaBook.Columns.Count].Style.Font.Bold = true;
                SheetSum.Cells[SoLuongLeadDaBookRowTrongTuan+DanhSachTongDaBook.Rows.Count + 2, 1, SoLuongLeadDaBookRowTrongTuan+DanhSachTongDaBook.Rows.Count + 2, DanhSachTongDaBook.Columns.Count].Style.Font.Color.SetColor(Color.Blue);
                SheetSum.Cells[SoLuongLeadDaBookRowTrongTuan + DanhSachTongDaBook.Rows.Count + 2, 1, SoLuongLeadDaBookRowTrongTuan + DanhSachTongDaBook.Rows.Count + 2, DanhSachTongDaBook.Columns.Count].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }

            {
                SheetSum.Cells[SoLuongLeadSoSanhRowTrongTuan, 1].Value = "TỶ LỆ BOOK/TỔNG LEADS PHÁT SINH TRONG TUẦN";
                SheetSum.Cells[SoLuongLeadSoSanhRowTrongTuan, 1].Style.Font.Bold = true;
                for (int hang=1; hang <= SoLuongHangTrongTuan+2; hang++)
                {
                    for (int cot=1; cot <=SoLuongCotTrongTuan; cot++)
                    {
                        if (hang==1)
                        {
                            if(SheetSum.Cells[2, cot].Value.ToString() !="Tổng")
                            {
                                SheetSum.Cells[SoLuongLeadSoSanhRowTrongTuan + hang, cot].Value = SheetSum.Cells[2, cot].Value;

                            }
                            else
                            {
                                SheetSum.Cells[SoLuongLeadSoSanhRowTrongTuan + hang, cot].Value = "Trung bình";
                            }
                            SheetSum.Cells[SoLuongLeadSoSanhRowTrongTuan + hang, cot].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            SheetSum.Cells[SoLuongLeadSoSanhRowTrongTuan + hang, cot].Style.Fill.SetBackground(Color.Green);
                            SheetSum.Cells[SoLuongLeadSoSanhRowTrongTuan + hang, cot].Style.Font.Color.SetColor(Color.White);
                            SheetSum.Cells[SoLuongLeadSoSanhRowTrongTuan + hang, cot].Style.Font.Bold = true;
                            SheetSum.Cells[SoLuongLeadSoSanhRowTrongTuan + hang, cot].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            
                        }
                        else if (cot==1)
                        {
                            if (SheetSum.Cells[hang + 1, cot].Value != null)
                            {
                                if (SheetSum.Cells[hang + 1, cot].Value.ToString() != "Tổng")
                                {
                                    SheetSum.Cells[SoLuongLeadSoSanhRowTrongTuan + hang, cot].Value = SheetSum.Cells[hang + 1, cot].Value;

                                }
                                else
                                {
                                    SheetSum.Cells[SoLuongLeadSoSanhRowTrongTuan + hang, cot].Value = "Trung bình";
                                }
                            }
                            SheetSum.Cells[SoLuongLeadSoSanhRowTrongTuan + hang, cot].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            SheetSum.Cells[SoLuongLeadSoSanhRowTrongTuan + hang, cot].Style.Font.Bold = true;
                        }
                        else
                        {
                            SheetSum.Cells[SoLuongLeadSoSanhRowTrongTuan + hang,cot].Formula = "=IFERROR(" + SheetSum.Cells[SoLuongLeadDaBookRowTrongTuan+hang, cot].Address 
                                                                                        +"/(" + SheetSum.Cells[SoLuongLeadDaBookRowTrongTuan + hang, cot].Address + "+" + SheetSum.Cells[1 + hang, cot].Address + "),\"n.a\")";
                            SheetSum.Cells[SoLuongLeadSoSanhRowTrongTuan + hang, cot].Style.Numberformat.Format = "0%";
                            SheetSum.Cells[SoLuongLeadSoSanhRowTrongTuan + hang, cot].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            SheetSum.Cells[SoLuongLeadSoSanhRowTrongTuan + hang, cot].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        }
                    }
                }
                SoLuongLeadChuaBookRowYTD = SoLuongLeadSoSanhRowTrongTuan + SoLuongHangTrongTuan + 3;
            } // Xong Phần tính ty lệ trong tuan

            {

                var DanhSachTongChuaBookYTD = await sqlDataAccess.getDataTable("exec proc_SaleAutoGetSumLeadsChuaBook '" + TuNngay.ToString("yyyy-01-01") + "','" + DenNgay.ToString("yyyy-MM-dd 23:59:59") + "','" + maBenhVien + "'");


                DanhSachTongChuaBookYTD.TableName = "SummaryChuaBookYTD";
                SheetSum.Cells[SoLuongLeadChuaBookRowYTD+1, 1].Value = "SỐ LƯỢNG LEAD CHƯA BOOK YTD";
                SheetSum.Cells[SoLuongLeadChuaBookRowYTD + 1, 1].Style.Font.Bold = true;
                SheetSum.Cells[SoLuongLeadChuaBookRowYTD+2, 1].LoadFromDataTable(DanhSachTongChuaBookYTD, true, TableStyles.Light2);

                for (int i = 0; i < DanhSachTongChuaBookYTD.Rows.Count; i++)
                {
                    SheetSum.Cells[SoLuongLeadChuaBookRowYTD+i + 2, DanhSachTongChuaBookYTD.Columns.Count].Style.Font.Bold = true;
                }

                SoLuongLeadDaBookRowYTD = SoLuongLeadChuaBookRowYTD+DanhSachTongChuaBookYTD.Rows.Count + 5;
                SoLuongHangYTD = DanhSachTongChuaBookYTD.Rows.Count;
                SoLuongCotYTD = DanhSachTongChuaBookYTD.Columns.Count;

                // Tính dòng tổng
                for (int i = 1; i < DanhSachTongChuaBookYTD.Columns.Count; i++)
                {
                    SheetSum.Cells[SoLuongLeadChuaBookRowYTD+DanhSachTongChuaBookYTD.Rows.Count + 3, i + 1].Formula = "=sum(" + SheetSum.Cells[SoLuongLeadChuaBookRowYTD + 2, i + 1].Address + ":" + SheetSum.Cells[SoLuongLeadChuaBookRowYTD + DanhSachTongChuaBookYTD.Rows.Count + 2, i + 1].Address + ")";

                }
                SheetSum.Cells[SoLuongLeadChuaBookRowYTD + DanhSachTongChuaBookYTD.Rows.Count + 3, 1].Value = "Tổng";
                SheetSum.Cells[SoLuongLeadChuaBookRowYTD + DanhSachTongChuaBookYTD.Rows.Count + 3, 1, SoLuongLeadChuaBookRowYTD + DanhSachTongChuaBookYTD.Rows.Count + 3, DanhSachTongChuaBookYTD.Columns.Count].Style.Font.Bold = true;
                SheetSum.Cells[SoLuongLeadChuaBookRowYTD + DanhSachTongChuaBookYTD.Rows.Count + 3, 1, SoLuongLeadChuaBookRowYTD + DanhSachTongChuaBookYTD.Rows.Count + 3, DanhSachTongChuaBookYTD.Columns.Count].Style.Font.Color.SetColor(Color.Blue);
                SheetSum.Cells[SoLuongLeadChuaBookRowYTD + DanhSachTongChuaBookYTD.Rows.Count + 3, 1, SoLuongLeadChuaBookRowYTD + DanhSachTongChuaBookYTD.Rows.Count + 3, DanhSachTongChuaBookYTD.Columns.Count].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }

            {
                var DanhSachTongDaBook = await sqlDataAccess.getDataTable("exec proc_SaleAutoGetSumLeadsDaBook '" + TuNngay.ToString("yyyy-01-01") + "','" + DenNgay.ToString("yyyy-MM-dd 23:59:59") + "','" + maBenhVien + "'");
                DanhSachTongDaBook.TableName = "SummaryDaBookYTD";
                SheetSum.Cells[SoLuongLeadDaBookRowYTD, 1].Value = "SỐ LƯỢNG LEADS ĐÃ BOOK YTD";
                SheetSum.Cells[SoLuongLeadDaBookRowYTD, 1].Style.Font.Bold = true;
                SheetSum.Cells[SoLuongLeadDaBookRowYTD + 1, 1].LoadFromDataTable(DanhSachTongDaBook, true, TableStyles.Light3);

                for (int i = 0; i < DanhSachTongDaBook.Rows.Count; i++)
                {
                    SheetSum.Cells[i + SoLuongLeadDaBookRowYTD + 1, DanhSachTongDaBook.Columns.Count].Style.Font.Bold = true;
                }
                SoLuongLeadSoSanhRowYTD = SoLuongLeadDaBookRowYTD + DanhSachTongDaBook.Rows.Count + 4;
                // Tính dòng tổng
                for (int i = 1; i < DanhSachTongDaBook.Columns.Count; i++)
                {
                    SheetSum.Cells[SoLuongLeadDaBookRowYTD + DanhSachTongDaBook.Rows.Count + 2, i + 1].Formula = "=sum(" + SheetSum.Cells[SoLuongLeadDaBookRowYTD + 2, i + 1].Address + ":" + SheetSum.Cells[SoLuongLeadDaBookRowYTD + DanhSachTongDaBook.Rows.Count + 1, i + 1].Address + ")";

                }
                SheetSum.Cells[SoLuongLeadDaBookRowYTD + DanhSachTongDaBook.Rows.Count + 2, 1].Value = "Tổng";
                SheetSum.Cells[SoLuongLeadDaBookRowYTD + DanhSachTongDaBook.Rows.Count + 2, 1, SoLuongLeadDaBookRowYTD + DanhSachTongDaBook.Rows.Count + 2, DanhSachTongDaBook.Columns.Count].Style.Font.Bold = true;
                SheetSum.Cells[SoLuongLeadDaBookRowYTD + DanhSachTongDaBook.Rows.Count + 2, 1, SoLuongLeadDaBookRowYTD + DanhSachTongDaBook.Rows.Count + 2, DanhSachTongDaBook.Columns.Count].Style.Font.Color.SetColor(Color.Blue);
                SheetSum.Cells[SoLuongLeadDaBookRowYTD + DanhSachTongDaBook.Rows.Count + 2, 1, SoLuongLeadDaBookRowYTD + DanhSachTongDaBook.Rows.Count + 2, DanhSachTongDaBook.Columns.Count].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }

            {
                SheetSum.Cells[SoLuongLeadSoSanhRowYTD, 1].Value = "TỶ LỆ BOOK/TỔNG LEADS PHÁT SINH YTD";
                SheetSum.Cells[SoLuongLeadSoSanhRowYTD, 1].Style.Font.Bold = true;

                for (int hang = 1; hang <= SoLuongHangYTD + 2; hang++)
                {
                    for (int cot = 1; cot <= SoLuongCotYTD; cot++)
                    {
                        if (hang == 1)
                        {
                            if (SheetSum.Cells[SoLuongLeadChuaBookRowYTD + 2, cot].Value != null)
                            {
                                if (SheetSum.Cells[SoLuongLeadChuaBookRowYTD + 2, cot].Value.ToString() != "Tổng")
                                {
                                    SheetSum.Cells[SoLuongLeadSoSanhRowYTD + hang, cot].Value = SheetSum.Cells[SoLuongLeadChuaBookRowYTD + 2, cot].Value;

                                }
                                else
                                {
                                    SheetSum.Cells[SoLuongLeadSoSanhRowYTD + hang, cot].Value = "Trung bình";
                                }
                            }
                            SheetSum.Cells[SoLuongLeadSoSanhRowYTD + hang, cot].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            SheetSum.Cells[SoLuongLeadSoSanhRowYTD + hang, cot].Style.Fill.SetBackground(Color.Green);
                            SheetSum.Cells[SoLuongLeadSoSanhRowYTD + hang, cot].Style.Font.Color.SetColor(Color.White);
                            SheetSum.Cells[SoLuongLeadSoSanhRowYTD + hang, cot].Style.Font.Bold = true;
                            SheetSum.Cells[SoLuongLeadSoSanhRowYTD + hang, cot].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        }
                        else if (cot == 1)
                        {
                            if (SheetSum.Cells[SoLuongLeadChuaBookRowYTD+hang + 1, cot].Value != null)
                            {
                                if (SheetSum.Cells[SoLuongLeadChuaBookRowYTD+hang + 1, cot].Value.ToString() != "Tổng")
                                {
                                    SheetSum.Cells[SoLuongLeadSoSanhRowYTD + hang, cot].Value = SheetSum.Cells[hang + 1, cot].Value;

                                }
                                else
                                {
                                    SheetSum.Cells[SoLuongLeadSoSanhRowYTD + hang, cot].Value = "Trung bình";
                                }
                            }
                            SheetSum.Cells[SoLuongLeadSoSanhRowYTD + hang, cot].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            SheetSum.Cells[SoLuongLeadSoSanhRowYTD + hang, cot].Style.Font.Bold = true;
                        }
                        else
                        {
                            SheetSum.Cells[SoLuongLeadSoSanhRowYTD + hang, cot].Formula = "=IFERROR(" + SheetSum.Cells[SoLuongLeadDaBookRowYTD + hang, cot].Address
                                                                                        + "/(" + SheetSum.Cells[SoLuongLeadDaBookRowYTD  + hang, cot].Address + "+" + SheetSum.Cells[SoLuongLeadChuaBookRowYTD + 1 + hang, cot].Address + "),\"n.a\")";
                            SheetSum.Cells[SoLuongLeadSoSanhRowYTD + hang, cot].Style.Numberformat.Format = "0%";
                            SheetSum.Cells[SoLuongLeadSoSanhRowYTD + hang, cot].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            SheetSum.Cells[SoLuongLeadSoSanhRowYTD + hang, cot].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        }
                    }
                }
                SoLuongLeadChuaBookRowYTD = SoLuongLeadSoSanhRowTrongTuan + SoLuongHangTrongTuan + 2;
            } // Xong Phần tính ty lệ trong năm



            SheetSum.Cells.AutoFitColumns();



            var DanhSachChiTietChuaBook = await sqlDataAccess.getDataTable("exec proc_SaleAutoGetChiTietLeadsChuaBook '" + TuNngay.ToString("yyyy-MM-dd") + "','" + DenNgay.ToString("yyyy-MM-dd 23:59:59") + "','"+maBenhVien+"'");

            var BenhKhamSheetChuaBook = pkg.Workbook.Worksheets.Add("Danh sách lead chưa books");
            DanhSachChiTietChuaBook.TableName = "DetailChuaBook";
            BenhKhamSheetChuaBook.Cells[1, 1].LoadFromDataTable(DanhSachChiTietChuaBook, true,TableStyles.Light1);
            BenhKhamSheetChuaBook.Cells[2, 3, DanhSachChiTietChuaBook.Rows.Count + 1, 3].Style.Numberformat.Format = "m/d/yyyy";
            BenhKhamSheetChuaBook.Cells.AutoFitColumns();

            var DanhSachChiTietDaBook = await sqlDataAccess.getDataTable("exec proc_SaleAutoGetChiTietLeadsDaBook '" + TuNngay.ToString("yyyy-MM-dd") + "','" + DenNgay.ToString("yyyy-MM-dd 23:59:59") + "','" + maBenhVien + "'");

            var BenhKhamSheetDaBook = pkg.Workbook.Worksheets.Add("Danh sách lead đã books");
            DanhSachChiTietDaBook.TableName = "DetailDaBook";
            BenhKhamSheetDaBook.Cells[1, 1].LoadFromDataTable(DanhSachChiTietDaBook, true, TableStyles.Light2);
            //BenhKhamSheetDaBook.Cells[2, 3, DanhSachChiTietDaBook.Rows.Count + 1, 3].Style.Numberformat.Format = "m/d/yyyy";
            BenhKhamSheetDaBook.Cells.AutoFitColumns();

            return pkg;
        }

        public async Task<bool> CheckDaGuiMailChiTietLeadsChuaBook(int tuanBC, int namBC)
        {
            string sql = "select * from ThoiGianGuiMailChiTietLeadsChuaBook where KetQua=" + (int)KetQuaGuiMail.ThanhCong + " and Tuan=" + tuanBC + " and Nam=" + namBC;

            var result = await sqlDataAccess.loadData<ThoiGianGuiMailWeekly, dynamic>(sql, new { });
            if (result != null && result.Count > 0)
            {
                return true;
            }
            return false;
        }
        public async Task<ThongTinGuiMailBenhVien> LayThongTinGuiMailChiTietLeadsChuaBook(string MaBenhVien = "O")
        {
            string sql = "select * from ThongTinGuiMailLeadsChuaBook Where MaBenhVien='" + MaBenhVien + "'";
            var result = await sqlDataAccess.loadData<ThongTinGuiMailBenhVien, dynamic>(sql, new { });
            return result.FirstOrDefault();
        }
        public async Task LuuThongTinGuiMailChiTietLeadsChuaBook(int Tuan, int Nam, KetQuaGuiMail thanhCong, string MaBenhVien = "O")
        {
            string sql = @"insert into ThoiGianGuiMailChiTietLeadsChuaBook (            
                        Tuan
                        ,Nam
                        ,NgayGui
                        ,KetQua
                        ,MaBenhVien)
                        values
                        (
                        " + Tuan + @"
                        ," + Nam + @"
                        ,'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + @"'
                        ," + (int)thanhCong + @", '"+MaBenhVien+"')";
            await sqlDataAccess.execNonQuery(sql);
        }

        public async Task<List<KPIVM>> GetLeadBookAnalysisLeadSource(DateTime TuNgay, DateTime DenNgay, string MaBenhVien="O")
        {
            var sql = "exec proc_SaleAutoGetLeadBookAnalysisLeadSource '" + TuNgay.ToString("yyyy-MM-dd") + "','" + DenNgay.ToString("yyyy-MM-dd") + "','" + MaBenhVien + "'";
            return await sqlDataAccess.loadData<KPIVM, dynamic>(sql, new { });
        }
        public async Task<List<KPIVM>> GetLeadBookAnalysisSuccessfulBooking(DateTime TuNgay, DateTime DenNgay, string MaBenhVien = "O")
        {
            var sql = "exec proc_SaleAutoGetLeadBookAnalysisSuccessfulBooking '" + TuNgay.ToString("yyyy-MM-dd") + "','" + DenNgay.ToString("yyyy-MM-dd") + "','" + MaBenhVien + "'";
            return await sqlDataAccess.loadData<KPIVM, dynamic>(sql, new { });
        }
        public async Task<List<KPIVM>> GetLeadBookAnalysisLeads(DateTime TuNgay, DateTime DenNgay, string MaBenhVien = "O")
        {
            var sql = "exec proc_SaleAutoGetLeadBookAnalysisLeads '" + TuNgay.ToString("yyyy-MM-dd") + "','" + DenNgay.ToString("yyyy-MM-dd") + "','" + MaBenhVien + "'";
            return await sqlDataAccess.loadData<KPIVM, dynamic>(sql, new { });
        }

        public async Task<ExcelPackage> GetLeadBookAnalysisTuan(DateTime Ngay)
        {
            var TuanCuoi = DateTimeHelp.LayTuan(Ngay);
            int CotBatDau = 10;

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var pkg = new ExcelPackage();
            pkg.Workbook.Properties.Title = "Report BenhNhan kham";
            pkg.Workbook.Properties.Author = "Tran Thien Thanh 0908325345";
            pkg.Workbook.Properties.Subject = "Benh nhan kham";
            var filePath = "LeadBookAnalysis.xlsx";
            if (File.Exists("LeadBookAnalysis" + TuanCuoi.Nam + ".xlsx"))
            {
                filePath = "LeadBookAnalysis" + TuanCuoi.Nam + ".xlsx";
            }
            ExcelWorksheet TuanSheet;
            if (File.Exists(filePath))
            {
                var existingFile = new FileInfo(filePath);
                await pkg.LoadAsync(existingFile);
                TuanSheet = pkg.Workbook.Worksheets["Week"];
            }
            else
            {
                TuanSheet = pkg.Workbook.Worksheets.Add("Week");
            }

            for (int i =1; i<=TuanCuoi.Tuan; i++)
            {
                var tuan = DateTimeHelp.LayTuan(i,TuanCuoi.Nam);
                var leads = await GetLeadBookAnalysisLeads(tuan.TuNgay, tuan.DenNgay);
                foreach (var item in leads)
                {
                    TuanSheet.Cells[item.Hang, i+CotBatDau].Value = item.SoLuong;
                }
                var SuccessfulBooking = await GetLeadBookAnalysisSuccessfulBooking(tuan.TuNgay, tuan.DenNgay);
                foreach (var item in SuccessfulBooking)
                {
                    TuanSheet.Cells[item.Hang, i + CotBatDau].Value = item.SoLuong;
                    TuanSheet.Row(item.Hang).Height = 10;
                }
                var LeadSources = await GetLeadBookAnalysisLeadSource(tuan.TuNgay, tuan.DenNgay);
                foreach (var item in LeadSources)
                {
                    TuanSheet.Cells[item.Hang, i + CotBatDau].Value = item.SoLuong;
                    TuanSheet.Row(item.Hang).Height = 10;
                }

            }
            return pkg;

        }

        #region Xuất File cho OP
        public async Task<IEnumerable<IDictionary<string, object>>> GetDanhSachLead(DateTime TuNgay , DateTime DenNgay)
        {            
            var str = "exec proc_AutoGetLeadOP '" + TuNgay.ToString("yyyy-MM-dd")
                + "','" + DenNgay.ToString("yyyy-MM-dd 23:59:59") + "'";            
            try
            {
                var result = await sqlDataAccess.getDataDic(str);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(str, ex);
            }            
        }
        public async Task<IEnumerable<IDictionary<string,object>>> GetDanhSachBook(DateTime TuNgay, DateTime DenNgay)
        {
            var str = "exec proc_AutoGetBookOP '" + TuNgay.ToString("yyyy-MM-dd")
                + "','" + DenNgay.ToString("yyyy-MM-dd 23:59:59") + "'";
            try
            {
                var result = await sqlDataAccess.getDataDic(str);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(str, ex);
            }
        }
        public async Task<IEnumerable<IDictionary<string, object>>> GetDanhSachKham(DateTime TuNgay, DateTime DenNgay)
        {
            var str = "exec proc_AutoGetKhamOP '" + TuNgay.ToString("yyyy-MM-dd")
                + "','" + DenNgay.ToString("yyyy-MM-dd 23:59:59") + "'";
            try
            {
                var result = await sqlDataAccess.getDataDic(str);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(str, ex);
            }
        }
        public async Task<IEnumerable<IDictionary<string, object>>> GetDanhSachPhauThuat(DateTime TuNgay, DateTime DenNgay)
        {
            var str = "exec proc_AutoGetPhauThuatOP '" + TuNgay.ToString("yyyy-MM-dd")
                + "','" + DenNgay.ToString("yyyy-MM-dd 23:59:59") + "'";
            try
            {
                var result = await sqlDataAccess.getDataDic(str);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(str, ex);
            }
        }

        public async Task<IEnumerable<IDictionary<string, object>>> GetQuaTrinhKham(DateTime TuNgay, DateTime DenNgay)
        {
            var str = "exec proc_AutoGetQuaTrinhKhamOP '" + TuNgay.ToString("yyyy-MM-dd")
                + "','" + DenNgay.ToString("yyyy-MM-dd 23:59:59") + "'";
            try
            {
                var result = await sqlDataAccess.getDataDic(str);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(str, ex);
            }
        }
        public async Task<IEnumerable<IDictionary<string, object>>> GetQuaTrinhPhauThuat(DateTime TuNgay, DateTime DenNgay)
        {
            var str = "exec proc_AutoGetQuaTrinhPhauThuatOP '" + TuNgay.ToString("yyyy-MM-dd")
                + "','" + DenNgay.ToString("yyyy-MM-dd 23:59:59") + "'";
            try
            {
                var result = await sqlDataAccess.getDataDic(str);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(str, ex);
            }
        }
        #endregion
    }
}
