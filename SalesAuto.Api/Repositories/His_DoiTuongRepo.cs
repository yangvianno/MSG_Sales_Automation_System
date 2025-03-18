using DataAccessLibrary;
using SalesAuto.Models.Entities.HisDoiTuong;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace SalesAuto.Api.Repositories
{
    public class His_DoiTuongRepo : IHis_DoiTuongRepo
    {
        private readonly ISqlDataAccess sqlDataAccess;
        public His_DoiTuongRepo(ISqlDataAccess sqlDataAccess)
        {
            this.sqlDataAccess = sqlDataAccess;
        }

        public async Task<List<BangGiaTheoDoiTuong>> GetBangGiaTheoDoiTuong(string MaBenhVien, Guid ID_LoaiDoiTuong)
        {
            var preSql = await GetpreSql(MaBenhVien);
            var str = "exec " + preSql + "proc_DoiTuongGetBangGia '" + ID_LoaiDoiTuong + "'";
            return await sqlDataAccess.loadData<BangGiaTheoDoiTuong, dynamic>(str, new { });
        }
        public async Task<List<LoaiDoiTuong>> GetDanhSachDoiTuong(string MaBenhVien)
        {
            var preSql = await GetpreSql(MaBenhVien);
            var str = "exec " + preSql + "proc_DoiTuongGetDanhSachDoiTuong";
            return await sqlDataAccess.loadData<LoaiDoiTuong, dynamic>(str, new { });
        }

        private async Task<string> GetpreSql(string maBenhVien)
        {
            DataTable tbl = await sqlDataAccess.getDataTable("select max(PreSql) PreSql  from UserBenhVien where MaBenhVien='" + maBenhVien + "'");
            return tbl.Rows[0][0].ToString();
        }

        public async Task<BangGiaTheoDoiTuong> SaveBangGiaTheoDoiTuong(string MaBenhVien, BangGiaTheoDoiTuong item)
        {
            var preSql = await GetpreSql(MaBenhVien);
            try
            {
                var sql = "exec " + preSql + @"proc_DoiTuongBangGiaSave  
                         '" + item.ID_LoaiDoiTuong + @"'
                        , N'" + item.MACV + @"'
                        , '" + item.GIATIEN + @"'
                        , '" + item.GIABHYT + @"'
	                    , '" + item.GIABHYT_PHAITRA + "'";

                await sqlDataAccess.execNonQuery(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return item;
        }

        public async Task<BangGiaTheoDoiTuong> DeleteBangGiaTheoDoiTuong(string MaBenhVien, BangGiaTheoDoiTuong item)
        {
            var preSql = await GetpreSql(MaBenhVien);
            try
            {
                var sql = "exec " + preSql + @"proc_DoiTuongBangGiaDelete  
                         '" + item.ID_LoaiDoiTuong + @"'
                        , N'" + item.MACV + @"'";

                await sqlDataAccess.execNonQuery(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return item;
        }


    }
}
