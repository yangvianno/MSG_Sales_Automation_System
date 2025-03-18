using DataAccessLibrary;
using DB;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using SalesAuto.Models.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SalesAuto.Api.Repositories
{
    public class ChiTieuSoLuongRepo : IChiTieuSoLuongRepo
    {
        private readonly SalesAutoDbContext context;
        private ISqlDataAccess sqlDataAccess;

        public ChiTieuSoLuongRepo(SalesAutoDbContext salesAutoDbContext, ISqlDataAccess sqlDataAccess)
        {
            this.context = salesAutoDbContext;
            this.sqlDataAccess = sqlDataAccess;
        }

        public async Task<List<LoaiChiTieu>> GetAllLoaiThiTieu()
        {
            string sql = "select MaLoaiChiTieu, TenChiTieu, NhomChiTiet, MoTa from LoaiChiTieu";
            var result = await sqlDataAccess.loadData<LoaiChiTieu, dynamic>(sql, new { });
            return result;
        }

        public async Task<List<ChiTieuSoLuong>> GetChiTieuSoLuong(int MaLoaiChiTieu, int Nam, string MaBenhVien)
        {
            var result = context.ChiTieuSoLuongs.AsQueryable().Where(x => x.MaBenhVien == MaBenhVien && x.MaLoaiChiTieu == MaLoaiChiTieu && x.Nam == Nam);
            bool CapNhat = false;
            for (int i=1;i<=12;i++)
            {
                if ( (await result.Where(x=>x.MaBenhVien == MaBenhVien && x.Thang == i && x.Nam == Nam).FirstOrDefaultAsync()) == null)
                {
                    context.ChiTieuSoLuongs.Add(new ChiTieuSoLuong
                    {
                        MaBenhVien = MaBenhVien,
                        Thang = i,
                        Nam = Nam,
                        SoLuong =0,
                        MaLoaiChiTieu = MaLoaiChiTieu
                    }
                        );
                    CapNhat = true;
                }
            }
            if (CapNhat)
            {
                await context.SaveChangesAsync();
                result = context.ChiTieuSoLuongs.AsQueryable().Where(x => x.MaBenhVien == MaBenhVien && x.MaLoaiChiTieu == MaLoaiChiTieu && x.Nam == Nam);
            }
            result = result.OrderBy(x => x.Thang);
            return await result.ToListAsync();
        }

        public async Task SaveChiTieuLasikFileToDataBase(string file)
        {
            if (File.Exists(file))
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                try
                {
                    using (var pck = new OfficeOpenXml.ExcelPackage())
                    {
                        using (var stream = System.IO.File.OpenRead(file))
                        {
                            pck.Load(stream);
                        }
                        // ChiTieu So Luong
                        var ws = pck.Workbook.Worksheets.First();
                        int Nam = int.Parse(ws.Cells["A1"].Text);
                        List<ChiTieuSoLuong> ListChiTieuSoLuong = new List<ChiTieuSoLuong>();
                        int startRow = 2;
                        int MaLoai = 1;
                        for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                        {
                            try
                            {
                                if (ws.Cells[rowNum, 1].Text.Trim() != "")
                                {
                                    if (ws.Cells[rowNum, 3].Text.Trim() == "")
                                    {
                                        if (ws.Cells[rowNum, 1].Text.Trim().ToLower() == "blade")
                                        {
                                            MaLoai = 1;
                                        }
                                        else if (ws.Cells[rowNum, 1].Text.Trim().ToLower() == "femto")
                                        {
                                            MaLoai = 2;
                                        }
                                        else if (ws.Cells[rowNum, 1].Text.Trim().ToLower() == "smile")
                                        {
                                            MaLoai = 3;
                                        }                                        
                                        else if (ws.Cells[rowNum, 1].Text.Trim().ToLower() == "clear")
                                        {
                                            MaLoai = 32;
                                        }
                                        else
                                        {
                                            MaLoai = 31;
                                        }
                                    }
                                    else
                                    {
                                        for (int i = 3; i < 3 + 12; i++)
                                        {
                                            ListChiTieuSoLuong.Add(new ChiTieuSoLuong
                                            {
                                                MaBenhVien = ws.Cells[rowNum, 1].Text,
                                                MaLoaiChiTieu = MaLoai,
                                                Nam = Nam,
                                                Thang = int.Parse(ws.Cells[1, i].Text),
                                                SoLuong = (ws.Cells[rowNum, i].Text=="" || ws.Cells[rowNum, i].Text=="-" ? 0 : int.Parse(ws.Cells[rowNum, i].Value.ToString()))
                                            }
                                            );
                                        }
                                    }
                                }
                            }
                            catch
                            {

                            }
                        }
                        await SaveChiTieu(ListChiTieuSoLuong);
                        //ChiTieu DoanhThu
                        ws = pck.Workbook.Worksheets[1];
                        Nam = int.Parse(ws.Cells["A1"].Text);
                        ListChiTieuSoLuong = new List<ChiTieuSoLuong>();
                        startRow = 2;
                        MaLoai = 1;
                        for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                        {
                            try
                            {
                                if (ws.Cells[rowNum, 1].Text != "")
                                {
                                    if (ws.Cells[rowNum, 3].Text == "")
                                    {
                                        if (ws.Cells[rowNum, 1].Text.ToLower() == "blade")
                                        {
                                            MaLoai = 524;
                                        }
                                        else if (ws.Cells[rowNum, 1].Text.ToLower() == "femto")
                                        {
                                            MaLoai = 523;
                                        }
                                        else if (ws.Cells[rowNum, 1].Text.ToLower() == "smile")
                                        {
                                            MaLoai = 522;
                                        }
                                        else if (ws.Cells[rowNum, 1].Text.Trim().ToLower() == "clear")
                                        {
                                            MaLoai = 526;
                                        }
                                        else
                                        {
                                            MaLoai = 525;
                                        }
                                    }
                                    else
                                    {
                                        for (int i = 3; i < 3 + 12; i++)
                                        {
                                            ListChiTieuSoLuong.Add(new ChiTieuSoLuong
                                            {
                                                MaBenhVien = ws.Cells[rowNum, 1].Text,
                                                MaLoaiChiTieu = MaLoai,
                                                Nam = Nam,
                                                Thang = int.Parse(ws.Cells[1, i].Text),
                                                SoLuong = (ws.Cells[rowNum, i].Text == "" || ws.Cells[rowNum, i].Text == "-" ? 0 : (long)double.Parse(ws.Cells[rowNum, i].Value.ToString()))
                                            }
                                            );
                                        }
                                    }
                                }
                            }
                            catch
                            {

                            }
                        }
                        await SaveChiTieu(ListChiTieuSoLuong);
                    }
                }
                catch
                {

                }
            }
        }

        private async Task SaveChiTieu(List<ChiTieuSoLuong> listChiTieuSoLuong)
        {
            foreach (ChiTieuSoLuong item in listChiTieuSoLuong)
            {
                var found = await context.ChiTieuSoLuongs.FirstOrDefaultAsync<ChiTieuSoLuong>(x => x.MaBenhVien == item.MaBenhVien && x.MaLoaiChiTieu == item.MaLoaiChiTieu && x.Nam == item.Nam && x.Thang == item.Thang);
                if (found == null)
                {
                    context.ChiTieuSoLuongs.Add(item);
                }
                else
                {
                    found.SoLuong = item.SoLuong;
                }
            }
            await context.SaveChangesAsync();
        }
        public async Task<ChiTieuSoLuong> Save(ChiTieuSoLuong chiTieuSoLuong)
        {
            var found = await context.ChiTieuSoLuongs.FirstOrDefaultAsync<ChiTieuSoLuong>(x => x.MaBenhVien == chiTieuSoLuong.MaBenhVien && x.MaLoaiChiTieu == chiTieuSoLuong.MaLoaiChiTieu && x.Nam == chiTieuSoLuong.Nam && x.Thang == chiTieuSoLuong.Thang);
            if (found == null)
            {
                context.ChiTieuSoLuongs.Add(chiTieuSoLuong);
            }
            else
            {
                found.SoLuong = chiTieuSoLuong.SoLuong;
            }        
            await context.SaveChangesAsync();
            return chiTieuSoLuong;
        }

    }
}
