using Microsoft.EntityFrameworkCore;
using DB;
using SalesAuto.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLibrary;

namespace SalesAuto.Api.Repositories
{
    public class BenhViensRepo : IBenhViensRepo
    {
        private readonly SalesAutoDbContext _context;
        private readonly ISqlDataAccess sqlDataAccess;
        public BenhViensRepo(SalesAutoDbContext salesAutoDbContext, ISqlDataAccess sqlDataAccess)
        {
            _context = salesAutoDbContext;
            this.sqlDataAccess = sqlDataAccess;
        }

        public async Task<IEnumerable<BenhVienVM>> GetAllBenhVienList()
        {
            
            
            string sql = "select BenhVien.MaBenhVien, BenhVien.TenBenhVien, PreSql, CRM_store_code from BenhVien ";
            var result = await sqlDataAccess.loadData<BenhVienVM, dynamic>(sql, new { });
            return result;
            ////var a = await _context.BenhVienVMs.FromSqlRaw("select MaBenhVien, TenBenhVien,'' as PreSQL from BenhVien").AsNoTracking().ToListAsync();  
            //var a = await _context.BenhVienVMs.ToListAsync();
            //return a;
        }

        public async Task<IEnumerable<BenhVienVM>> GetBenhVienByUserID(Guid userID)
        {
            string uid = userID.ToString();
            string sql = "select BenhVien.MaBenhVien, BenhVien.TenBenhVien, UserBenhVien.PreSql from BenhVien inner join UserBenhVien on BenhVien.MaBenhVien=UserBenhVien.MaBenhVien where UserBenhVien.UserID='" + uid + "'";
            var result= await sqlDataAccess.loadData<BenhVienVM, dynamic>(sql, new { });            
            return result;
        }

        public async Task<IEnumerable<BenhVienVM>> GetBenhVienByEmail(string Email)
        {   
            string sql = @"select BenhVien.MaBenhVien, BenhVien.TenBenhVien, UserBenhVien.PreSql 
                        from
                            BenhVien
                            inner join UserBenhVien on BenhVien.MaBenhVien = UserBenhVien.MaBenhVien
                            inner
                        join AspNetUsers on UserBenhVien.UserID = AspNetUsers.Id
                        where AspNetUsers.Email = '" + Email + "'";
            var result = await sqlDataAccess.loadData<BenhVienVM, dynamic>(sql, new { });
            return result;
        }
        public async Task<IEnumerable<BenhVienVM>> GetBenhVienByTenVietTat(string TenVietTat)
        {
            string sql = @"select BenhVien.MaBenhVien, BenhVien.TenBenhVien, PreSql, CRM_store_code
                        from
                            BenhVien                            
                        where BenhVien.TenVietTat = '" + TenVietTat + "'";
            var result = await sqlDataAccess.loadData<BenhVienVM, dynamic>(sql, new { });
            return result;
        }
        public async Task<IEnumerable<BenhVienVM>> GetBenhVienByMaBenhVien(string MaBenhVien)
        {
            string sql = @"select BenhVien.MaBenhVien, BenhVien.TenBenhVien, PreSql, CRM_store_code
                        from
                            BenhVien                            
                        where BenhVien.MaBenhVien = '" + MaBenhVien + "'";
            var result = await sqlDataAccess.loadData<BenhVienVM, dynamic>(sql, new { });
            return result;
        }
    }
}
