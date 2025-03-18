using DataAccessLibrary;
using HelperLib;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Razor.Templating.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SalesAuto.Api.Repositories
{
    public class DailyReportRepo: IDailyReportRepo
    {
        private readonly ISqlDataAccess sqlDataAccess;

        public DailyReportRepo(ISqlDataAccess sqlDataAccess)
        {            
            this.sqlDataAccess = sqlDataAccess;
        }
        public async Task<DataTable> GetDailyReporBenhVien(string MaBenhVien)
        {
            DateTime DenNgay = DateTime.Now.AddDays(-1);
            DateTime TuNgay = DenNgay.AddDays(-5);
            return await sqlDataAccess.getDataTable("proc_BaoCaoDailyNhieuNgay '" + TuNgay.ToString("yyyy-MM-dd") + "','" + DenNgay.ToString("yyyy-MM-dd 23:59:59") + "','','" + MaBenhVien + "'");
        }
        public async Task<string> GetDailyReportBenhVienString(string MaBenhVien)
        {
            var table = await GetDailyReporBenhVien(MaBenhVien);
            if (table==null || table.Rows.Count<=0)
            {
                return "";
            }
            var renderedString = await RazorTemplateEngine.RenderAsync("~/Views/DailyReportsMat.cshtml", table);
            return renderedString;
        }
        public async Task<DataTable> GetReportMatTable()
        {
            DateTime TuNgay = DateTimeHelp.LayTuan(DateTime.Now.AddDays(-7)).TuNgay;
            DateTime DenNgay = DateTimeHelp.LayTuan(DateTime.Now.AddDays(-7)).DenNgay;
            return await GetDailyReportMatTable(TuNgay, DenNgay);
        }

        public async Task<DataTable> GetReportDaKhoaTable()
        {
            DateTime TuNgay = DateTimeHelp.LayTuan(DateTime.Now.AddDays(-7)).TuNgay;
            DateTime DenNgay = DateTimeHelp.LayTuan(DateTime.Now.AddDays(-7)).DenNgay;
            
            return await GetDailyReportDaKhoaTable (TuNgay, DenNgay);
        }
        
        


        public async Task<DataTable> GetDailyReportMatSumTable()
        {
            DateTime TuNgay = DateTimeHelp.LayTuan(DateTime.Now.AddDays(-7)).TuNgay;
            DateTime DenNgay = DateTimeHelp.LayTuan(DateTime.Now.AddDays(-7)).DenNgay;
            return await GetDailyReportMatSum(TuNgay, DenNgay);                
        }
        public async Task<DataTable> GetDailyReportDaKhoaSumTable()
        {
            DateTime TuNgay = DateTimeHelp.LayTuan(DateTime.Now.AddDays(-7)).TuNgay;
            DateTime DenNgay = DateTimeHelp.LayTuan(DateTime.Now.AddDays(-7)).DenNgay;
            return await GetDailyReportDaKhoaSumTable(TuNgay, DenNgay);
        }

        public async Task<DataTable> GetDailyReportMatSum(DateTime TuNgay, DateTime DenNgay)
        {            
            return await sqlDataAccess.getDataTable("exec proc_BaoCaoDailyMatGroupSummary '" + TuNgay.ToString("yyyy-MM-dd") + "','" + DenNgay.ToString("yyyy-MM-dd 23:59:59") + "'");
        }

        public async Task<DataTable> GetDailyReportBenhVienMatSumTable()
        {
            DateTime TuNgay = DateTimeHelp.LayTuan(DateTime.Now.AddDays(-7)).TuNgay;
            DateTime DenNgay = DateTimeHelp.LayTuan(DateTime.Now.AddDays(-7)).DenNgay;
            return await GetDailyReportBenhVienMatSumTable(TuNgay, DenNgay);
        }
        public async Task<DataTable> GetDailyReportBenhVienDaKhoaSumTable()
        {
            DateTime TuNgay = DateTimeHelp.LayTuan(DateTime.Now.AddDays(-7)).TuNgay;
            DateTime DenNgay = DateTimeHelp.LayTuan(DateTime.Now.AddDays(-7)).DenNgay;
            return await GetDailyReportBenhVienDaKhoaSumTable(TuNgay, DenNgay);
                         
        }

        public async Task<DataTable> GetDailyReportBenhVienMatSumTable(DateTime TuNgay, DateTime DenNgay)
        {            
            return await sqlDataAccess.getDataTable("exec proc_BaoCaoDailyBenhVienMatSummary '" + TuNgay.ToString("yyyy-MM-dd") + "','" + DenNgay.ToString("yyyy-MM-dd 23:59:59") + "'");
        }

        public async Task<DataTable> GetDailyReportBenhVienDaKhoaSumTable(DateTime TuNgay, DateTime DenNgay)
        {
            return await sqlDataAccess.getDataTable("exec proc_BaoCaoDailyBenhVienDaKhoaSummary '" + TuNgay.ToString("yyyy-MM-dd") + "','" + DenNgay.ToString("yyyy-MM-dd 23:59:59") + "'");
        }

        public async Task<DataTable> GetDailyReportMatTable(DateTime TuNgay, DateTime DenNgay)
        {            
            return await sqlDataAccess.getDataTable("exec proc_BaoCaoDailyMat '" + TuNgay.ToString("yyyy-MM-dd") + "','" + DenNgay.ToString("yyyy-MM-dd 23:59:59") + "'");
        }

        public async Task<DataTable> GetDailyReportDaKhoaTable(DateTime TuNgay, DateTime DenNgay)
        {
            return await sqlDataAccess.getDataTable("exec proc_BaoCaoDailyDaKhoa '" + TuNgay.ToString("yyyy-MM-dd") + "','" + DenNgay.ToString("yyyy-MM-dd 23:59:59") + "'");
        }
        public async Task<DataTable> GetDailyReportDaKhoaTable() 
        {
            DateTime TuNgay = DateTimeHelp.LayTuan(DateTime.Now.AddDays(-7)).TuNgay;
            DateTime DenNgay = DateTimeHelp.LayTuan(DateTime.Now.AddDays(-7)).TuNgay;
            return await sqlDataAccess.getDataTable("exec proc_BaoCaoDailyDaKhoa '" + TuNgay.ToString("yyyy-MM-dd") + "','" + DenNgay.ToString("yyyy-MM-dd 23:59:59") + "'");
        }
       
        public async Task<DataTable> GetDailyReportDaKhoaSumTable(DateTime TuNgay, DateTime DenNgay)
        {
            return await sqlDataAccess.getDataTable("exec proc_BaoCaoDailyDaKhoaGroupSummary '" + TuNgay.ToString("yyyy-MM-dd") + "','" + DenNgay.ToString("yyyy-MM-dd 23:59:59") + "'");
        }

        public async Task<string> GetDailyReportMatSumString()
        {
            var table = await GetDailyReportMatSumTable();
            var para = new Dictionary<string, object>
            {
                ["TangGiamTuan"] = "true",
                ["TangGiamNam"] = "true",
            };

            var renderedString = await RazorTemplateEngine.RenderAsync("~/Views/DailyReportsMat.cshtml", table, para);            
            return renderedString;
        }
        public async Task<string> GetDailyReportMatSumString(DateTime TuNgay, DateTime DenNgay)
        {
            var table = await GetDailyReportMatSum(TuNgay, DenNgay);
            var para = new Dictionary<string, object>
            {
                ["TangGiamTuan"] = "true",
                ["TangGiamNam"] = "true",
            };

            var renderedString = await RazorTemplateEngine.RenderAsync("~/Views/DailyReportsMat.cshtml", table, para);
            return renderedString;
        }

        public async Task<string> GetDailyReportMatString()
        {
            var table = await GetReportMatTable();
            var para = new Dictionary<string, object> { 
                ["TangGiamTuan"] = "true",
                ["TangGiamNam"] = "true",
            };

            var renderedString = await RazorTemplateEngine.RenderAsync("~/Views/DailyReportsMat.cshtml", table, para);
            return renderedString;
        }
        
        public async Task<string> GetDailyReportMatString(DateTime TuNgay, DateTime DenNgay)
        {
            var table = await GetDailyReportMatTable(TuNgay,DenNgay);
            var renderedString = await RazorTemplateEngine.RenderAsync("~/Views/DailyReportsMat.cshtml", table);
            return renderedString;
        }
        
        public async Task<string> GetDailyReportDaKhoaSumString()
        {
            var table = await GetDailyReportDaKhoaSumTable();
            var para = new Dictionary<string, object>
            {
                ["TangGiamTuan"] = "true",
                ["TangGiamNam"] = "true",
            };

            var renderedString = await RazorTemplateEngine.RenderAsync("~/Views/DailyReportsMat.cshtml", table, para);
            return renderedString;
        }
        public async Task<string> GetDailyReportDaKhoaSumString(DateTime TuNgay, DateTime DenNgay)
        {
            var table = await GetDailyReportDaKhoaSumTable(TuNgay, DenNgay);
            var para = new Dictionary<string, object>
            {
                ["TangGiamTuan"] = "true",
                ["TangGiamNam"] = "true",
            };

            var renderedString = await RazorTemplateEngine.RenderAsync("~/Views/DailyReportsMat.cshtml", table, para);
            return renderedString;
        }

        public async Task<string> GetDailyReportDaKhoaString()
        {
            var table = await GetDailyReportDaKhoaTable(); 
            var renderedString = await RazorTemplateEngine.RenderAsync("~/Views/DailyReportsMat.cshtml", table);
            return renderedString;
        }
        public async Task<string> GetDailyReportDaKhoaString(DateTime TuNgay, DateTime DenNgay)
        {
            var table = await GetDailyReportDaKhoaTable(TuNgay, DenNgay);            

            var renderedString = await RazorTemplateEngine.RenderAsync("~/Views/DailyReportsMat.cshtml", table);
            return renderedString;
        }

        public async Task CreateExcel(ExcelWorksheet Sheet, DataTable table, bool TinhTong = false, bool TangGiamTuan = false, bool TangGiamNam = false, bool InDauDong = true, int startDong = 0, int startCot = 0)
        {
            string TenCot1 = "";
            string TenCot2 = "";
            string TenCot3 = "";
            int[] Cot1 = new int[100];
            int[] Cot2 = new int[100];
            int[] Cot3 = new int[100];
            int LuuCot1 = 1;
            int LuuCot2 = 1;
            int LuuCot3 = 1;
            int i = 1;            

            if (TenCot1 != "")
            {
                Cot1[LuuCot1] = i - LuuCot1;
            }
            if (TenCot2 != "")
            {
                Cot2[LuuCot2] = i - LuuCot2;
            }
            if (TenCot3 != "")
            {
                Cot3[LuuCot3] = i - LuuCot3;
            }
            int SoCot = 1;
            List<string> CacNgay = new List<string>();
            int Hang = 1;            
            Sheet.Cells[1, 1, 1, 4].Value = "Bệnh viện";
            Sheet.Cells[1, 1, 1, 4].Merge = true;
            int Cot = 5;            
            // hiện ngày tháng hoặc tên bệnh viện
            foreach (DataColumn col in table.Columns)
            {                
                if (SoCot >= 3)
                {
                    if (col.ColumnName != "STT" && col.ColumnName != "Nhom4" && col.ColumnName != "Nhom3" && col.ColumnName != "Nhom2" && col.ColumnName != "Nhom1")
                    {
                        Sheet.Cells[Hang+startDong, startCot + Cot].Value = col.ColumnName;
                        Sheet.Cells[Hang+ startDong, startCot + Cot].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        Sheet.Cells[startDong + Hang, startCot + Cot].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0x00C489));
                        CacNgay.Add(col.ColumnName);
                        if (TangGiamTuan && col.ColumnName == "Tuần trước")
                        {
                            Sheet.Cells[startDong + Hang, startCot + Cot].Style.Font.Color.SetColor(Color.FromArgb(0x090909));
                            Cot++;
                            Sheet.Cells[startDong + Hang, startCot + Cot].Style.Font.Color.SetColor(Color.FromArgb(0x090909));                            
                            Sheet.Cells[startDong + Hang, startCot + Cot].Value = "Tăng / Giảm";
                        }
                        Cot++;
                    }
                }
                SoCot++;
            }
            if(TinhTong)
            {
                Cot++;
                Sheet.Cells[startDong + Hang, startCot + Cot].Value = "Cộng";
                Sheet.Cells[startDong + Hang, startCot + Cot].Style.Fill.PatternType = ExcelFillStyle.Solid;
                Sheet.Cells[startDong + Hang, startCot + Cot].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0x090909));                
            }            

            // Đổ dữ liệu            
            Hang = 1;
            bool CoCot4 = false;                        
            foreach (DataRow row in table.Rows)
            {
                Hang++;
                Cot = 1;
                if(row["Nhom4"].ToString()!="")
                {
                    CoCot4 = true;
                }
                foreach (DataColumn col in table.Columns)
                {
                    if (col.ColumnName !="STT")
                    {   
                        Sheet.Cells[startDong + Hang, startCot + Cot].Style.Font.Color.SetColor(Color.Blue);
                        if (row["Nhom3"].ToString().IndexOf("%") >= 0)
                        {
                            if (Cot >= 5)
                            {
                                if (row[col.ColumnName].ToString() != "")
                                {
                                    Sheet.Cells[startDong + Hang, startCot + Cot].Value = double.Parse(row[col.ColumnName].ToString()) / 100;
                                }
                                else
                                {
                                    Sheet.Cells[startDong + Hang, startCot + Cot].Value = 0;
                                }
                            }
                            else
                            {
                                Sheet.Cells[startDong + Hang, startCot + Cot].Value = (row[col.ColumnName].ToString()!=""?row[col.ColumnName]:0);
                            }
                        }
                        else
                        {
                            Sheet.Cells[startDong + Hang, startCot + Cot].Value = (row[col.ColumnName].ToString() != "" ? row[col.ColumnName] : 0); ;
                        } 
                        if(col.ColumnName == "LY")
                        {
                            Sheet.Cells[startDong + Hang, startCot + Cot].Style.Font.Color.SetColor(Color.FromArgb(0x090909));
                        }
                        else if(col.ColumnName =="YTD")
                        {
                            Sheet.Cells[startDong + Hang, startCot + Cot].Style.Font.Color.SetColor(Color.FromArgb(0xED7D31));
                        }
                        if (TangGiamTuan && col.ColumnName == "Tuần trước")
                        {
                            Sheet.Cells[startDong + Hang, startCot + Cot].Style.Font.Color.SetColor(Color.FromArgb(0x090909));
                            Cot++;
                            Sheet.Cells[startDong + Hang, startCot + Cot].Style.Font.Color.SetColor(Color.FromArgb(0x090909));
                            Sheet.Cells[startDong + Hang, startCot + Cot].Formula = "=" + Sheet.Cells[Hang, Cot - 2].Address + "-" + Sheet.Cells[Hang, Cot - 1].Address;                           
                        }
                        Cot++;
                    }
                }
            }
            
            //format
            Sheet.Cells[startDong + 1, startCot + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
            Sheet.Cells[startDong + 1, startCot + 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0x00C489));
            int MaxHang = Hang;
            for (int h = 2; h <= MaxHang; h++)
            {
                for (int c = 5; c <= table.Columns.Count; c++)
                {
                    FormatDong(Sheet, startDong + h , startCot + c);
                }
            }

            
            // merge các cột giống nhau            
            for (int CotFor = 1; CotFor <= 4; CotFor++)
            {
                int HangHienTai = 2;
                if (CotFor == 1)
                {
                    Sheet.Cells[startDong + HangHienTai, startCot + CotFor].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    Sheet.Cells[startDong + HangHienTai, startCot + CotFor].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0x00C489));
                }
                else
                {
                    Sheet.Cells[startDong + HangHienTai, startCot + CotFor].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    Sheet.Cells[startDong + HangHienTai, startCot + CotFor].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0xDBF6ED));
                }

                for (int HangFor = 3; HangFor <= MaxHang; HangFor++)
                {
                    if (CotFor == 1)
                    {
                        Sheet.Cells[startDong + HangFor, startCot + CotFor].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        Sheet.Cells[startDong + HangFor, startCot + CotFor].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0x00C489));
                    }
                    else
                    {
                        Sheet.Cells[startDong + HangFor, startCot + CotFor].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        Sheet.Cells[startDong + HangFor, startCot + CotFor].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0xDBF6ED));
                    }

                    if (Sheet.Cells[startDong + HangHienTai, startCot + 2].Value != null &&
                            (Sheet.Cells[startDong + HangHienTai, startCot + 2].Value.ToString().IndexOf("Ngoại trú (OPD)") >= 0
                            || Sheet.Cells[startDong + HangHienTai, startCot + 2].Value.ToString().IndexOf("Nội trú (IPD)") >= 0
                            || Sheet.Cells[startDong + HangHienTai, startCot + 2].Value.ToString().IndexOf("Doanh thu") >= 0
                            || Sheet.Cells[startDong + HangHienTai, startCot + 2].Value.ToString().IndexOf("Tổng cộng") >= 0
                            )
                        )
                    {
                        if (CotFor > 1)
                        {
                            if (Sheet.Cells[startDong + HangHienTai, startCot + 3].Value != null && Sheet.Cells[startDong + HangHienTai, startCot + 3].Value.ToString() == " % OP / IP conversion")
                            { }
                            else
                            {
                                Sheet.Cells[startDong + HangHienTai, startCot + CotFor].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                Sheet.Cells[startDong + HangHienTai, startCot + CotFor].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0x00B0F0));
                            }
                        }
                    }


                    if (Sheet.Cells[startDong + HangHienTai, startCot + CotFor].Value != null
                        && Sheet.Cells[startDong + HangFor, startCot + CotFor].Value != null
                        && Sheet.Cells[startDong + HangHienTai, startCot + CotFor].Value.ToString() == Sheet.Cells[startDong + HangFor, startCot + CotFor].Value.ToString())
                    {
                        if (Sheet.Cells[startDong + HangHienTai, startCot + CotFor].Value != null && !string.IsNullOrEmpty(Sheet.Cells[startDong + HangHienTai, startCot + CotFor].Value.ToString()))
                        {
                            Sheet.Cells[startDong + HangHienTai, startCot + CotFor, startDong + HangFor, startCot + CotFor].Merge = true;
                            Sheet.Cells[startDong + HangHienTai, startCot + CotFor, startDong + HangFor, startCot + CotFor].Style.WrapText = true;
                            Sheet.Cells[startDong + HangHienTai, startCot + CotFor, startDong + HangFor, startCot + CotFor].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        }
                    }
                    else
                    {
                        HangHienTai = HangFor;
                    }
                }
            }
            
            // Format             
            if (TangGiamNam)
            {
                Sheet.Cells[1,table.Columns.Count + 1].Value = "Tăng/giảm";
                for(int htg=2; htg<=MaxHang; htg++)
                { 
                    Sheet.Cells[startDong + htg, startCot + table.Columns.Count + 1].Style.Font.Color.SetColor(Color.FromArgb(0x090909));                
                    Sheet.Cells[startDong + htg, startCot + table.Columns.Count + 1].Style.Font.Color.SetColor(Color.FromArgb(0x090909));
                    Sheet.Cells[startDong + htg, startCot + table.Columns.Count + 1].Formula = "=" + Sheet.Cells[startDong + htg, startCot + table.Columns.Count - 1].Address + "-" + Sheet.Cells[startDong + htg, startCot + table.Columns.Count].Address;
                    FormatDong(Sheet, startDong + htg, startCot + table.Columns.Count + 1);
                }
            }
            int colCuoi = table.Columns.Count - 1;
            if(TinhTong)
            {
                colCuoi++;
            }
            if(TangGiamTuan)
            {
                colCuoi++;
            }
            if (TangGiamNam)
            {
                colCuoi++;
            }

            Sheet.Cells[startDong + 1, startCot + 1, startDong + MaxHang, startCot + colCuoi].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            Sheet.Cells[startDong + 1, startCot + 1, startDong + MaxHang, startCot + colCuoi].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            Sheet.Cells[startDong + 1, startCot + 1, startDong + MaxHang, startCot + colCuoi].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            Sheet.Cells[startDong + 1, startCot + 1, startDong + MaxHang, startCot + colCuoi].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            Sheet.Cells[startDong + 1, startCot + 1, startDong + MaxHang, startCot + colCuoi].AutoFitColumns();

            Sheet.Cells[startDong + 1, startCot + colCuoi + 1, startDong + MaxHang, startCot + colCuoi +2].Clear();
            
            if (!CoCot4)
            {
                Sheet.Column(startCot + 4).Hidden = true;
            }

            if (!InDauDong)
            {
                for (int CotBo = 0; CotBo <4; CotBo++)
                {
                    Sheet.DeleteColumn(startCot+1);
                }
            }
        }

        private static void FormatDong(ExcelWorksheet Sheet, int h, int c)
        {
            if (Sheet.Cells[h, 2].Value != null && Sheet.Cells[h, 2].Value.ToString() == "Nội trú (IPD)"
                                    || Sheet.Cells[h, 2].Value != null && Sheet.Cells[h, 2].Value.ToString() == "Ngoại trú (OPD)"
                                    || Sheet.Cells[h, 2].Value != null && Sheet.Cells[h, 2].Value.ToString() == "Doanh thu"
                                    || Sheet.Cells[h, 2].Value != null && Sheet.Cells[h, 2].Value.ToString() == "Tổng cộng"
                                    )
            {
                if (Sheet.Cells[h, 3].Value != null && Sheet.Cells[h, 3].Value.ToString() == "% OP/IP conversion")
                {

                }
                else
                {
                    Sheet.Cells[h, c].Style.Font.Italic = true;
                    Sheet.Cells[h, c].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    Sheet.Cells[h, c].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0x00B0F0));
                    Sheet.Cells[h, c].Style.Font.Bold = true;
                    Sheet.Cells[h, c].Style.Font.Color.SetColor(Color.Snow);
                }
                if (Sheet.Cells[h, 3].Value != null && Sheet.Cells[h, 3].Value.ToString().IndexOf("Tổng cộng") >= 0)
                {
                    Sheet.Cells[h, c].Style.Numberformat.Format = "#,##0";
                }
                else if (Sheet.Cells[h, 3].Value != null && Sheet.Cells[h, 3].Value.ToString() == "% OP/IP conversion")
                {
                    Sheet.Cells[h, c].Style.Numberformat.Format = "#,##0.0%";
                }
            }
            else if ((Sheet.Cells[h, 3].Value != null && Sheet.Cells[h, 3].Value.ToString().IndexOf("%") >= 0))
            {
                Sheet.Cells[h, c].Style.Numberformat.Format = "#,##0%";
            }
            else
            {
                Sheet.Cells[h, c].Style.Numberformat.Format = "#,##0";
            }
            if (Sheet.Cells[h, 3].Value != null && Sheet.Cells[h, 3].Value.ToString().IndexOf("% OP/IP conversion") >= 0
                || Sheet.Cells[h, 3].Value != null && Sheet.Cells[h, 3].Value.ToString().IndexOf("% Lasik(Smile + FemTo) / Lasik") >= 0
                || Sheet.Cells[h, 3].Value != null && Sheet.Cells[h, 3].Value.ToString().IndexOf("% Phaco (Đa tiêu)/Phaco") >= 0
                )
            {
                Sheet.Cells[h, c].Style.Font.Italic = true;
            }
        }
    }
}
