using DataAccessLibrary;
using DB;
using Microsoft.EntityFrameworkCore;
using SalesAuto.Models.SearchModel;
using SalesAuto.Models.ViewModel;
using SalesAuto.Models.ViewModel.SeekWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesAuto.Api.Repositories
{
    public class BenhNhansRepo : IBenhNhansRepo
    {
        private readonly SalesAutoDbContext _context;
        private readonly ISqlDataAccess sqlDataAccess;

        public BenhNhansRepo(SalesAutoDbContext context, ISqlDataAccess sqlDataAccess)
        {
            this._context = context;
            this.sqlDataAccess = sqlDataAccess;
        }       

        public async Task<PageList<BenhNhanKhamVM>> GetBenhNhanKhamListPage(BenhNhanSM benhNhanSM, string MaBenhVien = "O")
        {
            string sql = "exec proc_getBenhNhanVM '" + benhNhanSM.TuNgay.ToString("yyyy-MM-dd") + "', '" + benhNhanSM.DenNgay.ToString("yyyy-MM-dd 23:59:59") + "','" + MaBenhVien + "'";
            var a = await sqlDataAccess.loadData<BenhNhanKhamVM, dynamic>(sql, new { });
            var query = a.AsQueryable();

            if (!string.IsNullOrEmpty(benhNhanSM.Nguon))
            {
                query = query.Where(x => x.Nguon.ToLower().Contains(benhNhanSM.Nguon.ToLower()));
            }

            if (!string.IsNullOrEmpty(benhNhanSM.Loai))
            {
                query = query.Where(x => x.Loai.ToLower().Contains(benhNhanSM.Loai.ToLower()));
            }
            if (!string.IsNullOrEmpty(benhNhanSM.MaBenhVien))
            {
                query = query.Where(x => x.MaBenhVien.ToLower().Contains(benhNhanSM.MaBenhVien.ToLower()));
            }

            
            var count = query.Count();
            var data = query.OrderByDescending(x => x.Ngay)
                .Skip((benhNhanSM.pageNumber - 1) * benhNhanSM.PageSize)
                .Take(benhNhanSM.PageSize)
                .ToList();
            return new PageList<BenhNhanKhamVM>(data, count, benhNhanSM.pageNumber, benhNhanSM.PageSize);
        }

        public async Task<IEnumerable<BenhNhanKhamVM>> GetBenhNhanKhamList(BenhNhanSM benhNhanSM, string MaBenhVien = "O")
        {
            string sql = "exec proc_getBenhNhanVM '" + benhNhanSM.TuNgay.ToString("yyyy-MM-dd") + "', '" + benhNhanSM.DenNgay.ToString("yyyy-MM-dd 23:59:59") + "','" + MaBenhVien + "'";
            var a = await sqlDataAccess.loadData<BenhNhanKhamVM, dynamic>(sql, new { });
            var query = a.AsQueryable();

            if (!string.IsNullOrEmpty(benhNhanSM.Nguon))
            {
                query = query.Where(x => x.Nguon.ToLower().Contains(benhNhanSM.Nguon.ToLower()));
            }

            if (!string.IsNullOrEmpty(benhNhanSM.Loai))
            {
                query = query.Where(x => x.Loai.ToLower().Contains(benhNhanSM.Loai.ToLower()));
            }
            if (!string.IsNullOrEmpty(benhNhanSM.MaBenhVien))
            {
                query = query.Where(x => x.MaBenhVien.ToLower().Contains(benhNhanSM.MaBenhVien.ToLower()));
            }

            return query.ToList();
        }
        public async Task<IEnumerable<BenhVienKhamVM>> GetBenhVienKhamList(BenhNhanSM benhNhanSM, string MaBenhVien = "O")
        {
            string sql = "exec proc_getBenhVienKhamVM '" + benhNhanSM.TuNgay.ToString("yyyy-MM-dd") + "', '" + benhNhanSM.DenNgay.ToString("yyyy-MM-dd 23:59:59") + "','" + MaBenhVien + "'";
            var a = await sqlDataAccess.loadData<BenhVienKhamVM, dynamic>(sql, new { });
            var query = a.AsQueryable();        

            if (!string.IsNullOrEmpty(benhNhanSM.Loai))
            {
                query = query.Where(x => x.Loai.ToLower().Contains(benhNhanSM.Loai.ToLower()));
            }
            if (!string.IsNullOrEmpty(benhNhanSM.MaBenhVien))
            {
                query = query.Where(x => x.MaBenhVien.ToLower().Contains(benhNhanSM.MaBenhVien.ToLower()));
            }
            //query = query.Where(x => x.Ngay >= leadSM.TuNgay && x.Ngay <= leadSM.DenNgay);
            return query.ToList();
        }
       
    }
}
