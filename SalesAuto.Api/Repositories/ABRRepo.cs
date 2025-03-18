using DataAccessLibrary;
using DB;
using Google.Apis.Drive.v3.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OfficeOpenXml;
using SalesAuto.Models;
using SalesAuto.Models.Entities;
using SalesAuto.Models.SearchModel;
using SalesAuto.Models.ViewModel;
using Spire.Pdf.Exporting.XPS.Schema;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace SalesAuto.Api.Repositories
{
    public class ABRRepo : IABRRepo
    {
        private readonly SalesAutoDbContext context;
        private readonly ISqlDataAccess sqlDataAccess;


        public ABRRepo(SalesAutoDbContext context, ISqlDataAccess sqlDataAccess)
        {
            this.context = context;
            this.sqlDataAccess = sqlDataAccess;
        }

        public async Task<List<ABRDanhMuc>> LayDanhMucABR(string MaBenhVien)
        {
            var preSql = await GetpreSql(MaBenhVien);
            var strcon = await GetConnectStr(MaBenhVien);
            var str = "exec " + preSql + "proc_ABRLayDanhMuc";
            return await sqlDataAccess.loadData<ABRDanhMuc, dynamic>(str, new { }, strcon);
        }
        public async Task<List<ABRDanhMucXetDuyet>> ABRGetDanhMucXetDuyet(string MaBenhVien)
        {
            try
            {
                var preSql = await GetpreSql(MaBenhVien);
                var strcon = await GetConnectStr(MaBenhVien);
                var str = "exec " + preSql + "proc_ABRGetDanhMucXetDuyet '" + DateTime.Now.AddMonths(-2).ToString("yyyy-MM-dd") + "'";
                return await sqlDataAccess.loadData<ABRDanhMucXetDuyet, dynamic>(str, new { }, strcon);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<ABRDanhMucXetDuyet>> ABRGetDanhMucXetDuyetMaster()
        {
            try
            {                
                var str = "exec proc_ABRGetDanhMucXetDuyet '" + DateTime.Now.AddMonths(-2).ToString("yyyy-MM-dd") + "'";
                return await sqlDataAccess.loadData<ABRDanhMucXetDuyet, dynamic>(str, new { });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ABRDanhMucXetDuyet> SaveTinhTrangXetDuyetMaster(ABRDanhMucXetDuyet aBRDanhMucXetDuyet)
        {
            try
            {
                var str = "exec proc_ABRSaveTinhTrangXetDuyet '" + aBRDanhMucXetDuyet.IDXetDuyet + "','" + (int)aBRDanhMucXetDuyet.TinhTrang + "'";                
                await sqlDataAccess.execNonQuery(str);
                
                str = "exec proc_ABRSaveXetDuyetChinhSachBenhVien '"+ aBRDanhMucXetDuyet.MaBenhVien +"','" + aBRDanhMucXetDuyet.IDXetDuyet + "','" + (int)aBRDanhMucXetDuyet.TinhTrang + "'";
                await sqlDataAccess.execNonQuery(str);
                _ = SaveTinhTrangXetDuyetCapNhatBenhVien();
                return aBRDanhMucXetDuyet;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task SaveTinhTrangXetDuyetCapNhatBenhVien()
        {
            try
            {
                var str = "exec  proc_ABRGetDanhSachXetDuyetChinhSachBenhVienChuaLuu";
                var result = await sqlDataAccess.loadData<XetDuyetBenhVien, dynamic>(str, new { });
                foreach(var item in result)
                {
                    var re = await SaveTinhTrangXetDuyetBenhVien(item.MaBenhVien, item.IDXetDuyet, item.TinhTrangXetDuyet);
                    if (re)
                    {
                        str = "exec proc_ABRSaveXetDuyetChinhSachBenhVien '" + item.MaBenhVien + "','" + item.IDXetDuyet + "','" + (int)item.TinhTrangXetDuyet + "','"+ (int)TrangThaiLuu.DaLuu+ "'";
                        sqlDataAccess.execNonQuery(str);
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> SaveTinhTrangXetDuyetBenhVien(string MaBenhVien, Guid IDXetDuet, TinhTrangXetDuyet tinhtrang)
        {
            try
            {
                var preSql = await GetpreSql(MaBenhVien);
                var strcon = await GetConnectStr(MaBenhVien);
                var str = "exec " + preSql+ "proc_ABRSaveTinhTrangXetDuyet '" + IDXetDuet + "','" + (int)tinhtrang+"'";
                await sqlDataAccess.execNonQuery(str, strcon);                
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ABRDanhMuc> LayDanhMucABRByID(string MaBenhVien, int ID)
        {
            var preSql = await GetpreSql(MaBenhVien);
            var strcon = await GetConnectStr(MaBenhVien);
            var str = "exec " + preSql + "proc_ABRLayDanhMucByID " + ID;
            var kq = await sqlDataAccess.loadData<ABRDanhMuc, dynamic>(str, new { }, strcon);
            if (kq==null)
            {
                return null;
            }
            if (kq.Count==0)
            {
                return null;
            }
            return kq.First();
        }
        public async Task<List<ABRCongViecHisVM>> LayCongViecHis(string MaBenhVien)
        {
            var preSql = await GetpreSql(MaBenhVien);
            var strcon = await GetConnectStr(MaBenhVien);
            var str = "exec " + preSql + "proc_AbrLayCongViecHis";
            return await sqlDataAccess.loadData<ABRCongViecHisVM, dynamic>(str, new { }, strcon);
        }

        public async Task<bool> SaveDanhMucABR(string MaBenhVien, ABRDanhMuc aBRDanhMuc)
        {
            var preSql = await GetpreSql(MaBenhVien);
            var strcon = await GetConnectStr(MaBenhVien);
            var str = "exec " + preSql + "proc_AbrSaveDanhMuc  " + aBRDanhMuc.ID + ", N'" + aBRDanhMuc.Code + "'," + aBRDanhMuc.MaNhomABR + ",N'" + aBRDanhMuc.TenCongViec + "'," + aBRDanhMuc.MucHuongVND + "," + aBRDanhMuc.MucHuongPhanTram + "," + aBRDanhMuc.STT+"," + aBRDanhMuc.TinhTheoPoolThucHien +"," + (int)aBRDanhMuc.LoaiHanhDong + "," + aBRDanhMuc.TinhTheoBenhAn.ToString() + "," + aBRDanhMuc.ChuongTrinhRieng.ToString()
                + (aBRDanhMuc.TyLeGianTiep==null ? ",null " : ",'" + aBRDanhMuc.TyLeGianTiep + "'")
                + (aBRDanhMuc.TinhTheoDoanhThu==null ? ",null ": ",'" + aBRDanhMuc.TinhTheoDoanhThu + "'")
                + (aBRDanhMuc.HuongToiDa==null? ",null ":",'" + aBRDanhMuc.HuongToiDa + "'")
                ;
            try
            { 
                var Cu = await LayDanhMucABRByID(MaBenhVien, aBRDanhMuc.ID);
                await sqlDataAccess.execNonQuery(str, strcon);
                // lưu có thay đổi để xét duyệt
                Guid IDXetDuyet = Guid.NewGuid();
                str = "exec " + preSql + "proc_AbrSaveDanhMucXetDuyet  " + aBRDanhMuc.ID + ", N'" + aBRDanhMuc.Code + "'," + aBRDanhMuc.MaNhomABR + ",N'" + aBRDanhMuc.TenCongViec + "'," + aBRDanhMuc.MucHuongVND + "," + aBRDanhMuc.MucHuongPhanTram + "," + aBRDanhMuc.STT + "," + aBRDanhMuc.TinhTheoPoolThucHien + "," + (int)aBRDanhMuc.LoaiHanhDong + "," + aBRDanhMuc.TinhTheoBenhAn.ToString() + "," + aBRDanhMuc.ChuongTrinhRieng.ToString() + "," + (aBRDanhMuc.TyLeGianTiep==null?"null": aBRDanhMuc.TyLeGianTiep)
                    + ",N'" + MaBenhVien + "','" + IDXetDuyet + "',N'"+ (Cu==null? "Thêm mới" : "Sửa đổi") +"',0,1" ;
                await sqlDataAccess.execNonQuery(str, strcon);
                if (Cu != null)
                {
                    str = "exec proc_AbrSaveDanhMucXetDuyet  " + Cu.ID + ", N'" + Cu.Code + "'," + Cu.MaNhomABR + ",N'" + Cu.TenCongViec + "'," + Cu.MucHuongVND + "," + Cu.MucHuongPhanTram + "," + Cu.STT + "," + Cu.TinhTheoPoolThucHien + "," + (int)Cu.LoaiHanhDong + "," + Cu.TinhTheoBenhAn.ToString() + "," + Cu.ChuongTrinhRieng.ToString() + "," + Cu.TyLeGianTiep
                        + ",N'" + MaBenhVien + "','" + IDXetDuyet + "',N'" + (Cu == null ? "Thêm mới" : "Sửa đổi") + "',0,0";
                    await sqlDataAccess.execNonQuery(str);
                }

                str = "exec proc_AbrSaveDanhMucXetDuyet  " + aBRDanhMuc.ID + ", N'" + aBRDanhMuc.Code + "'," + aBRDanhMuc.MaNhomABR + ",N'" + aBRDanhMuc.TenCongViec + "'," + aBRDanhMuc.MucHuongVND + "," + aBRDanhMuc.MucHuongPhanTram + "," + aBRDanhMuc.STT + "," + aBRDanhMuc.TinhTheoPoolThucHien + "," + (int)aBRDanhMuc.LoaiHanhDong + "," + aBRDanhMuc.TinhTheoBenhAn.ToString() + "," + aBRDanhMuc.ChuongTrinhRieng.ToString() + "," + (aBRDanhMuc.TyLeGianTiep == null ? "null" : aBRDanhMuc.TyLeGianTiep)
                    + ",N'" + MaBenhVien + "','" + IDXetDuyet + "',N'" + (Cu == null ? "Thêm mới" : "Sửa đổi") + "',0,1";
                await sqlDataAccess.execNonQuery(str);
                return true;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return false;
        }        
        #region MasterData
        public async Task<List<ABRDanhMuc>> GetDanhMucABRMasterData()
        {            
            var str = "exec proc_ABRLayDanhMuc";
            return await sqlDataAccess.loadData<ABRDanhMuc, dynamic>(str, new { });
        }

        public async Task<List<ABRNhom>> LayNhomABR()
        {           
            var str = "exec proc_AbrLayNhom";
            return await sqlDataAccess.loadData<ABRNhom, dynamic>(str, new { });
        }        

        public async Task<bool> SaveDanhMucABRMasterData(ABRDanhMuc aBRDanhMuc)
        {            
            var str = "exec proc_AbrSaveDanhMuc  " + aBRDanhMuc.ID + ", N'" + aBRDanhMuc.Code + "'," + aBRDanhMuc.MaNhomABR + ",N'" + aBRDanhMuc.TenCongViec + "'," + aBRDanhMuc.MucHuongVND + "," + aBRDanhMuc.MucHuongPhanTram + "," + aBRDanhMuc.STT + "," + aBRDanhMuc.TinhTheoPoolThucHien + "," + (int)aBRDanhMuc.LoaiHanhDong + "," + aBRDanhMuc.TinhTheoBenhAn.ToString() + "," + aBRDanhMuc.ChuongTrinhRieng.ToString() + "," + aBRDanhMuc.TyLeGianTiep;
            try
            {
                await sqlDataAccess.execNonQuery(str);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return false;
        }
        public async Task<bool> DeleteDanhMucABRMasterData(int iD)
        {
            var str = "exec proc_AbrdeleteDanhMuc " + iD;
            try
            {
                await sqlDataAccess.execNonQuery(str);
                return true;
            }
            catch
            {
            }
            return false;

        }
        #endregion
        public async Task<List<ABRCongViecHisVM>> LayDanhMucCongViecHis(string MaBenhVien)
        {
            var preSql = await GetpreSql(MaBenhVien);
            var strcon = await GetConnectStr(MaBenhVien);
            var str = "exec " + preSql + "proc_AbrLayDanhMucCongViecHis";
            return await sqlDataAccess.loadData<ABRCongViecHisVM, dynamic>(str, new { }, strcon);
        }

        public async Task<bool> DeleteDanhMucABR(string MaBenhVien, int iD)
        {
            var preSql = await GetpreSql(MaBenhVien);
            var strcon = await GetConnectStr(MaBenhVien);
            var str = "exec " + preSql + "proc_AbrdeleteDanhMuc " + iD;
            try
            {
                await sqlDataAccess.execNonQuery(str, strcon);
                return true;
            }
            catch
            {
                throw;
            }
            return false;

        }
        

        #region MapCongViecABRHIS
        public async Task<List<ABRMapCongViecABRHISVM>> LayDanhSachMapCongViecABRHIS(string maBenhVien)
        {
            var preSql = await GetpreSql(maBenhVien);
            var strcon = await GetConnectStr(maBenhVien);
            var str = "exec " + preSql + "proc_AbrLayMapCongViecABRHISVM";
            return await sqlDataAccess.loadData<ABRMapCongViecABRHISVM, dynamic>(str, new { }, strcon);
        }
        public async Task<ABRMapCongViecABRHIS> SaveMapCongViecABRHIS(string maBenhVien, ABRMapCongViecABRHIS item)
        {
            var preSql = await GetpreSql(maBenhVien);
            var strcon = await GetConnectStr(maBenhVien);
            if (item.RowGuid==Guid.Empty || item.RowGuid == null)
            {
                item.RowGuid = Guid.NewGuid();
            }
            var str = "exec " + preSql + "proc_AbrSaveMapCongViecABRHIS " + item.ID + "," + item.IDDanhMucABR + ",N'" + item.MaCV + "'," + item.QuyRa + "," + item.DoanhThuTinhABR + (item.TinhTheoDoanhThu== null?",null" : ",'"+ item.TinhTheoDoanhThu+ "'")
                + ",'" + item.RowGuid+ "'";
            try
            {
                await sqlDataAccess.execNonQuery(str, strcon);                
                return item;
            }
            catch 
            {
                throw;
            }            
        }
        public async Task<bool> DeleteMapCongViecABRHIS(string maBenhVien, int iD)
        {
            var preSql = await GetpreSql(maBenhVien);
            var strcon = await GetConnectStr(maBenhVien);
            var str = "exec " + preSql + "proc_AbrDeleteMapCongViecABRHIS " + iD;
            try
            {
                await sqlDataAccess.execNonQuery(str, strcon);
                return true;
            }
            catch
            {
            }
            return false;

        }
        #endregion

        #region ABR Nhân vien
        public async Task<List<ABRNhanVien>> LayDanhSachABRNhanVien(string maBenhVien)
        {
            var preSql = await GetpreSql(maBenhVien);
            var strcon = await GetConnectStr(maBenhVien);
            var str = "exec " + preSql + "proc_AbrLayDanhSachABRNhanVien";
            return await sqlDataAccess.loadData<ABRNhanVien, dynamic>(str, new { }, strcon);            
        }

        public async Task<bool> SaveABRNhanVien(string maBenhVien, ABRNhanVien item)
        {
            var preSql = await GetpreSql(maBenhVien);
            var strcon = await GetConnectStr(maBenhVien);
            var str = "exec " + preSql + "proc_AbrSaveABRNhanVien '" + item.ID + "', N'" + item.MaNhanVien + "',N'" + item.TenNhanVien + "',N'" + item.ChucDanh + "'," + item.TinhTrucTiep + "," + item.HuongTrucTiep + "," + item.HuongGianTiep +"," +(item.ThuocPool!=null?"'" + item.ThuocPool +"'":"null") + ",N'" + item.PhongBan +"'," + item.HeSoGianTiep + ","+(item.MaBenhVien==null?"null":"'"+ item.MaBenhVien+"'");
            try
            {
                await sqlDataAccess.execNonQuery(str, strcon);
                return true;
            }
            catch
            {
            }
            return false;
        }
        
        public async Task<bool> DeleteABRNhanVien(string maBenhVien, Guid iD)
        {
            var preSql = await GetpreSql(maBenhVien);
            var strcon = await GetConnectStr(maBenhVien);
            var str = "exec " + preSql + "proc_AbrDeleteABRNhanVien '" + iD + "'";
            try
            {
                await sqlDataAccess.execNonQuery(str, strcon);
                return true;
            }
            catch
            {
            }
            return false;

        }
        public async Task<bool> AddNhanVienHuongPool(string maBenhVien, Guid IDABRNhanVien, Guid IDABRPool)
        {
            var preSql = await GetpreSql(maBenhVien);
            var strcon = await GetConnectStr(maBenhVien);
            var str = "exec " + preSql + "proc_AbrAddNhanVienHuongPool '" + IDABRNhanVien + "', '" + IDABRPool + "'";
            try
            {
                await sqlDataAccess.execNonQuery(str, strcon);
                return true;
            }
            catch
            {
            }
            return false;
        }

        public async Task<bool> DeleteNhanVienHuongPool(string maBenhVien, Guid IDABRNhanVien, Guid IDABRPool)
        {
            var preSql = await GetpreSql(maBenhVien);
            var strcon = await GetConnectStr(maBenhVien);
            var str = "exec " + preSql + "proc_AbrDeleteNhanVienHuongPool '" + IDABRNhanVien + "', '" + IDABRPool + "'";
            try
            {
                await sqlDataAccess.execNonQuery(str, strcon);
                return true;
            }
            catch
            {
            }
            return false;
        }

        public async Task<List<AbrPool>> GetNhanVienHuongPool(string maBenhVien, Guid IDABRNhanVien)
        {
            var preSql = await GetpreSql(maBenhVien);
            var strcon = await GetConnectStr(maBenhVien);
            var str = "exec " + preSql + "proc_AbrGetNhanVienHuongPool '" + IDABRNhanVien + "'";
            try
            {
                var results = await sqlDataAccess.loadData<AbrPool, dynamic>(str, new { }, strcon);
                return results;
            }
            catch
            {
            }
            return null;
        }
        #endregion

        #region LuonDuocHuong
        public async Task<bool> AddNhanVienABRLuonDuocHuong(string maBenhVien, Guid IDABRNhanVien, int IDABRDanhMuc)
        {
            var preSql = await GetpreSql(maBenhVien);
            var strcon = await GetConnectStr(maBenhVien);
            var str = "exec " + preSql + "proc_AbrAddNhanVienABRLuonDuocHuong '" + IDABRNhanVien + "', '" + IDABRDanhMuc + "'";
            try
            {
                await sqlDataAccess.execNonQuery(str, strcon);
                return true;
            }
            catch
            {
            }
            return false;
        }

        public async Task<bool> DeleteNhanVienABRLuonDuocHuong(string maBenhVien, Guid IDABRNhanVien, int IDABRDanhMuc)
        {
            try
            { 
                var preSql = await GetpreSql(maBenhVien);
                var strcon = await GetConnectStr(maBenhVien);
                var str = "exec " + preSql + "proc_AbrDeleteNhanVienABRLuonDuocHuong '" + IDABRNhanVien + "', '" + IDABRDanhMuc + "'";           
                await sqlDataAccess.execNonQuery(str, strcon);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }

        public async Task<List<ABRDanhMuc>> GetNhanVienABRLuonDuocHuong(string maBenhVien, Guid IDABRNhanVien)
        {
            try
            {
                var preSql = await GetpreSql(maBenhVien);
                var strcon = await GetConnectStr(maBenhVien);
                var str = "exec " + preSql + "proc_AbrGetNhanVienABRLuonDuocHuong '" + IDABRNhanVien + "'";
            
                var results = await sqlDataAccess.loadData<ABRDanhMuc, dynamic>(str, new { }, strcon);
                return results;
            }
            catch
            {
                throw;
            }      
        }

        #endregion // Luong Duoc Huong

        #region HuongBacThang
        public async Task<ABRHuongBacThang> SaveHuongBacThang(string maBenhVien, ABRHuongBacThang aBRHuongBacThang)
        {
            var preSql = await GetpreSql(maBenhVien);
            var strcon = await GetConnectStr(maBenhVien);
            if (aBRHuongBacThang.ID == Guid.Empty)
            {
                aBRHuongBacThang.ID = Guid.NewGuid();
            }

            var str = "exec " + preSql + @"proc_ABRSaveHuongBacThang
                '" + aBRHuongBacThang.ID + @"'
                ," + aBRHuongBacThang.IDABRDanhMuc + @"
                ," + aBRHuongBacThang.CanDuoi + @"
                ," + aBRHuongBacThang.CanTren + @"
                ," + aBRHuongBacThang.TinhToanVien + @"
                ," + aBRHuongBacThang.HuongVND + @"
                ," + aBRHuongBacThang.HuongPhanTram;

            try
            {
                // Lay bac thang cu
                string BacThangCu = "";
                var bscu = await GetDanhSachHuongBacThang(maBenhVien, aBRHuongBacThang.IDABRDanhMuc);
                if (bscu != null)
                {
                    foreach (var item in bscu)
                    {
                        BacThangCu += item.CanDuoi.ToString() + "-" + item.CanTren.ToString() + ":" + item.HuongVND + " VNĐ, " + item.HuongPhanTram + "%; ";
                    }
                }
                await sqlDataAccess.execNonQuery(str, strcon);
                // lay bang thang moi
                string bacThangMoi = "";

                var bsmoi = await GetDanhSachHuongBacThang(maBenhVien, aBRHuongBacThang.IDABRDanhMuc);
                if (bsmoi!=null)
                { 
                    foreach (var item in bsmoi)
                    {
                        bacThangMoi += item.CanDuoi.ToString() + "-" + item.CanTren.ToString() + ":" + item.HuongVND + " VNĐ, " + item.HuongPhanTram + "%; ";
                    }
                }
                // Luu thay doi bac thang vào xét duyet
                try
                {
                    var Cu = await LayDanhMucABRByID(maBenhVien, aBRHuongBacThang.IDABRDanhMuc);
                    if (Cu != null)
                    {
                        // lưu có thay đổi để xét duyệt
                        Guid IDXetDuyet = Guid.NewGuid();
                        str = "exec " + preSql + "proc_AbrSaveDanhMucXetDuyet  " + Cu.ID + ", N'" + Cu.Code + "'," + Cu.MaNhomABR + ",N'" + Cu.TenCongViec + "'," + Cu.MucHuongVND + "," + Cu.MucHuongPhanTram + "," + Cu.STT + "," + Cu.TinhTheoPoolThucHien + "," + (int)Cu.LoaiHanhDong + "," + Cu.TinhTheoBenhAn.ToString() + "," + Cu.ChuongTrinhRieng.ToString() + "," + Cu.TyLeGianTiep
                            + ",N'" + maBenhVien + "','" + IDXetDuyet + "',N'sửa đổi mức hưởng ',0,1";
                        await sqlDataAccess.execNonQuery(str, strcon);

                        str = "exec proc_AbrSaveDanhMucXetDuyet  " + Cu.ID + ", N'" + Cu.Code + "'," + Cu.MaNhomABR + ",N'" + Cu.TenCongViec + "'," + Cu.MucHuongVND + "," + Cu.MucHuongPhanTram + "," + Cu.STT + "," + Cu.TinhTheoPoolThucHien + "," + (int)Cu.LoaiHanhDong + "," + Cu.TinhTheoBenhAn.ToString() + "," + Cu.ChuongTrinhRieng.ToString() + "," + Cu.TyLeGianTiep
                            + ",N'" + maBenhVien + "','" + IDXetDuyet + "',N' Sửa đổi mức hưởng theo số lượng " + BacThangCu + "',0,0";
                        str = "exec proc_AbrSaveDanhMucXetDuyet  " + Cu.ID + ", N'" + Cu.Code + "'," + Cu.MaNhomABR + ",N'" + Cu.TenCongViec + "'," + Cu.MucHuongVND + "," + Cu.MucHuongPhanTram + "," + Cu.STT + "," + Cu.TinhTheoPoolThucHien + "," + (int)Cu.LoaiHanhDong + "," + Cu.TinhTheoBenhAn.ToString() + "," + Cu.ChuongTrinhRieng.ToString() + "," + Cu.TyLeGianTiep
                            + ",N'" + maBenhVien + "','" + IDXetDuyet + "',N' Sửa đổi mức hưởng theo số lượng " + bacThangMoi + "',0,1";
                        await sqlDataAccess.execNonQuery(str, strcon);
                    }
                }
                catch
                {

                }

                return aBRHuongBacThang;
            }
            catch
            {
                throw;
            }           
        }


        public async Task<bool> DeleteHuongBacThang(string maBenhVien, Guid ID)
        {
            try
            {
                var preSql = await GetpreSql(maBenhVien);
                var strcon = await GetConnectStr(maBenhVien);
                var str = "exec " + preSql + "proc_ABRDeleteHuongBacThang '" + ID + "'";
                await sqlDataAccess.execNonQuery(str, strcon);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<ABRHuongBacThang>> GetDanhSachHuongBacThang(string maBenhVien, int IDABRDanhMuc)
        {
            try
            {
                var preSql = await GetpreSql(maBenhVien);
                var strcon = await GetConnectStr(maBenhVien);
                var str = "exec " + preSql + "proc_ABRGetHuongBacThang " + IDABRDanhMuc ;

                var results = await sqlDataAccess.loadData<ABRHuongBacThang, dynamic>(str, new { }, strcon);
                return results;
            }
            catch
            {
                throw;
            }
        }

        #endregion // Huong Bac Thang

        #region MapNhanVienABRHIS
        public async Task<List<ABRMapNhanVienABRHISVM>> LayDanhSachMapNhanVienABRHIS(string maBenhVien)
        {
            try
            {
                var preSql = await GetpreSql(maBenhVien);
                var strcon = await GetConnectStr(maBenhVien);
                var str = "exec " + preSql + "proc_AbrLayDanhSachMapNhanVienABRHIS";
                return await sqlDataAccess.loadData<ABRMapNhanVienABRHISVM, dynamic>(str, new { }, strcon);
            }
            catch
            {               
                throw;
            }
        }

        public async Task<bool> SaveMapNhanVienABRHIS(string maBenhVien, ABRMapNhanVienABRHIS item)
        {
            var preSql = await GetpreSql(maBenhVien);
            var strcon = await GetConnectStr(maBenhVien);
            var str = "exec " + preSql + "proc_AbrSaveMapNhanVienABRHIS '" + item.ID + "', '" + item.IDNhanVienABR + "'," + item.MaNhanVienHIS;
            try
            {
                await sqlDataAccess.execNonQuery(str, strcon);
                return true;
            }
            catch
            {
                throw;
            }            
        }
        public async Task<bool> DeleteMapNhanVienABRHIS(string maBenhVien, Guid iD)
        {
            var preSql = await GetpreSql(maBenhVien);
            var strcon = await GetConnectStr(maBenhVien);
            var str = "exec " + preSql + "proc_AbrDeleteMapNhanVienABRHIS '" + iD + "'";
            try
            {
                await sqlDataAccess.execNonQuery(str, strcon);
                return true;
            }
            catch
            {
            }
            return false;
        }

        public async Task<List<AbrNhanVienHisVM>> LayDanhSachNhanVienHIS(string maBenhVien)
        {
            var preSql = await GetpreSql(maBenhVien);
            var strcon = await GetConnectStr(maBenhVien);
            var str = "exec " + preSql + "proc_AbrLayDanhSachNhanVienHIS";
            return await sqlDataAccess.loadData<AbrNhanVienHisVM, dynamic>(str, new { }, strcon);
        }

        #endregion


        #region Nhân viên thực hiện
        
        static Dictionary<string, List<ABRNhanVienThucHienVM>> CatchNhanVienThucHien = new Dictionary<string, List<ABRNhanVienThucHienVM>>();
        public async Task<List<ABRNhanVienThucHienVM>> getNhanVienThucHien(string maBenhVien, NhanVienThucHienSM nhanVienThucHienSM)
        {
            var preSql = await GetpreSql(maBenhVien);
            var strcon = await GetConnectStr(maBenhVien);
            string Key = maBenhVien + "-" + nhanVienThucHienSM.TuNgay.ToString("yyyy.MM.dd") + "-" + nhanVienThucHienSM.DenNgay.ToString("yyyy.MM.dd");
            bool LaySoLieu = false;
            if (CatchNhanVienThucHien.ContainsKey(Key))
            {
                if (nhanVienThucHienSM.Trang == 0)
                {
                    LaySoLieu = true;
                }
            }
            else
            {
                LaySoLieu = true;
            }
            if (LaySoLieu)
            {

                var str = "exec " + preSql + "proc_ABRGetNhanVienThucHien '" + nhanVienThucHienSM.TuNgay.ToString("yyyy-MM-dd") + "','" + nhanVienThucHienSM.DenNgay.ToString("yyyy-MM-dd 23:59:59") + "'";                
                try
                {
                    //var result = await sqlDataAccess.getDataDic(str, strcon);
                    var result = await sqlDataAccess.loadData<ABRNhanVienThucHienVM, dynamic>(str, new { }, strcon);
                    
                    if (CatchNhanVienThucHien.ContainsKey(Key))
                    {
                        CatchNhanVienThucHien[Key] = result;
                    }
                    else
                    {
                        CatchNhanVienThucHien.Add(Key, result);
                    }
                }
                catch
                {
                    throw;
                }

            }
            if (CatchNhanVienThucHien.ContainsKey(Key))
            {
                var query = CatchNhanVienThucHien[Key].OrderBy(x => x.NgayThu).OrderBy(x => x.MaBenhAn).OrderBy(x => x.ID_DSCV).AsQueryable();
                if (!string.IsNullOrEmpty(nhanVienThucHienSM.NhomCongViecThongKe) && nhanVienThucHienSM.NhomCongViecThongKe.ToLower().Trim() != "all" && nhanVienThucHienSM.NhomCongViecThongKe.ToLower().Trim() != "tất cả")
                {
                    query = query.Where(x => x.NhomCongViecThongKe == nhanVienThucHienSM.NhomCongViecThongKe);
                }
                if (nhanVienThucHienSM.TinhTrang == ABRLoaiTinhTrangTimKiem.ChuaLuu)
                {
                    query = query.Where(x => String.IsNullOrEmpty(x.TenNhanVienABR) || x.TenNhanVienABR == "");
                }
                else if (nhanVienThucHienSM.TinhTrang == ABRLoaiTinhTrangTimKiem.DaLuu)
                {
                    query = query.Where(x => !String.IsNullOrEmpty(x.TenNhanVienABR));
                }
                if ((bool)nhanVienThucHienSM.NhanVienThucHienKhacHis)
                {
                    query = query.Where(x => (!String.IsNullOrEmpty(x.TenNhanVienABR)) && (!String.IsNullOrEmpty(x.TenNhanVienHIS)) && (x.TenNhanVienHIS != x.TenNhanVienABR));
                }
                if (nhanVienThucHienSM.NumRecords == 0)
                {
                    return query.ToList();
                }
                else
                {
                    return query.Skip(nhanVienThucHienSM.Trang* nhanVienThucHienSM.NumRecords).Take(nhanVienThucHienSM.NumRecords).ToList();
                }
            }
            return null;
        }
        public async Task<IEnumerable<IDictionary<string, object>>> GetNhanVienThucHienHis (string maBenhVien, NhanVienThucHienSM nhanVienThucHienSM)
        {
            var preSql = await GetpreSql(maBenhVien);
            var strcon = await GetConnectStr(maBenhVien);
            var str = "exec " + preSql + "proc_ABRNhanVienThucHienHis '" + nhanVienThucHienSM.TuNgay.ToString("yyyy-MM-dd") + "','" + nhanVienThucHienSM.DenNgay.ToString("yyyy-MM-dd 23:59:59") + "'";
            try
            {
                var result = await sqlDataAccess.getDataDic(str, strcon);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<string>> GetNhomCongViecThongKe(string maBenhVien)
        {
            var preSql = await GetpreSql(maBenhVien);
            var strcon = await GetConnectStr(maBenhVien);
            var str = "exec " + preSql + "proc_ABRGetNhomCongViecThongKe";
            var result = await sqlDataAccess.loadData<string, dynamic>(str, new { }, strcon);
            return result;
        }

        public async Task<bool> SaveNhanVienThucHien(string maBenhVien, ABRNhanVienThucHien aBRNhanVienThucHien)
        {
            var preSql = await GetpreSql(maBenhVien);
            var strcon = await GetConnectStr(maBenhVien);
            var str = "exec " + preSql + "proc_ABRSaveNhanVienThucHien '" + aBRNhanVienThucHien.ID_DSCV
                    + "'," + aBRNhanVienThucHien.IDMapDanhMucABRHIS
                    + "," + aBRNhanVienThucHien.IDABRDanhMuc
                    + ",'" + aBRNhanVienThucHien.IDABRNhanVien + "'"
                    + "," + aBRNhanVienThucHien.DoanhThuTinhABR
                    + "," + aBRNhanVienThucHien.SoLuong
                    ;
            try
            {
                await sqlDataAccess.execNonQuery(str, strcon);
                return true;
            }
            catch
            {
            }
            return false;

        }

        public async Task<bool> DeleteNhanVienThucHien(string maBenhVien, ABRNhanVienThucHien aBRNhanVienThucHien)
        {
            var preSql = await GetpreSql(maBenhVien);
            var strcon = await GetConnectStr(maBenhVien);
            var str = "exec " + preSql + "proc_ABRDeleteNhanVienThucHien '" + aBRNhanVienThucHien.ID_DSCV
                    + "'," + aBRNhanVienThucHien.IDMapDanhMucABRHIS;
            try
            {
                await sqlDataAccess.execNonQuery(str, strcon);
                return true;
            }
            catch
            {
            }
            return false;

        }

        #endregion

        #region Báo cáo
        public async Task<IEnumerable<IDictionary<string, object>>> GetBaoCaoChiTietDaThucHien(string maBenhVien, NhanVienThucHienSM nhanVienThucHienSM)
        {
            var preSql = await GetpreSql(maBenhVien);
            var strcon = await GetConnectStr(maBenhVien);
            var str = "exec " + preSql + "proc_ABRBaoCaoChiTietDaThucHien '" + nhanVienThucHienSM.TuNgay.ToString("yyyy-MM-dd")
                + "','" + nhanVienThucHienSM.DenNgay.ToString("yyyy-MM-dd 23:59:59") + "'";
            if (!string.IsNullOrEmpty(nhanVienThucHienSM.NhomCongViecThongKe) && nhanVienThucHienSM.NhomCongViecThongKe!="All" && nhanVienThucHienSM.NhomCongViecThongKe.ToLower() != "toàn bộ")
            {
                str += ",N'" + nhanVienThucHienSM.NhomCongViecThongKe +"'";
            }
            try
            {
                var result = await sqlDataAccess.getDataDic(str,strcon);
                return result;
            }
            catch
            {
            }
            return null;
        }

        public async Task<IEnumerable<IDictionary<string, object>>> GetBaoCaoDaThucHien(string maBenhVien, NhanVienThucHienSM nhanVienThucHienSM)
        {
            var preSql = await GetpreSql(maBenhVien);
            var strcon = await GetConnectStr(maBenhVien);
            var strproc = "proc_ABRBaoCaoDaThucHienHISFull";
            if (nhanVienThucHienSM.LoaiBaoTongHop== ABRLoaiBaoCaoTongHop.TheoVND)
            {
                strproc = "proc_ABRBaoCaoDaThucHienHuongVND";
            }else if (nhanVienThucHienSM.LoaiBaoTongHop == ABRLoaiBaoCaoTongHop.ThePhanTram)
            {
                strproc = "proc_ABRBaoCaoDaThucHienHuongPhanTram";
            }
            var str = "exec " + preSql + strproc + " '" + nhanVienThucHienSM.TuNgay.ToString("yyyy-MM-dd")
                + "','" + nhanVienThucHienSM.DenNgay.ToString("yyyy-MM-dd 23:59:59") + "'";
            if (!string.IsNullOrEmpty(nhanVienThucHienSM.NhomCongViecThongKe) && nhanVienThucHienSM.NhomCongViecThongKe!="All" && nhanVienThucHienSM.NhomCongViecThongKe != "Tất cả")
            {
                str += ",N'" + nhanVienThucHienSM.NhomCongViecThongKe + "'";
            }
            try
            {
                var result = await sqlDataAccess.getDataDic(str,strcon);
                return result;
            }
            catch
            {
            }
            return null;
        }


        public async Task<ExcelPackage> createABRExcel(int thang, int nam, string MaBenhVien)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var pkg = new ExcelPackage();
            pkg.Workbook.Properties.Title = "ABR tháng " + thang + " năm " + nam;
            pkg.Workbook.Properties.Author = "Tran Thien Thanh 0908325345";
            pkg.Workbook.Properties.Subject = "ABR";
            ExcelWorksheet Sheet;
            var filePath = "ABRTemplate.xlsx";
            if (System.IO.File.Exists(filePath))
            {
                var existingFile = new FileInfo(filePath);
                await pkg.LoadAsync(existingFile);
                Sheet = pkg.Workbook.Worksheets[0];
            }
            else
            {
                Sheet = pkg.Workbook.Worksheets.Add("");
            }
            var preSql = await GetpreSql(MaBenhVien);
            var strcon = await GetConnectStr(MaBenhVien);
            var strproc = "proc_ABRBangLuongChiTietNhanVien " + thang + "," + nam;

            var str = "exec " + preSql + strproc;

            var result = await sqlDataAccess.loadData<ABRLuongNhanVienThangVM, dynamic>(str, new { }, strcon);
            int startrow = 7;
            const int ColSTT= 1;
            const int ColMAN = 2;
            Sheet.Cells[1,1].Value = "BẢNG TỐNG HỢP ABR THÁNG " + thang + "/" + nam;

            Sheet.InsertRow(startrow + 1, result.Count);
            
            for ( int i=startrow+1; i<result.Count+startrow; i++)
            {
                Sheet.Cells[startrow, 1, startrow, 20].Copy(Sheet.Cells[i, 1, i, 20]);
            }
            foreach (var item in result)
            {
                Sheet.Cells[startrow, ColSTT].Value = item.STT;
                Sheet.Cells[startrow, ColMAN].Value = item.MaNhanVien;
                Sheet.Cells[startrow, 3].Value = item.HoVaTen;
                Sheet.Cells[startrow, 4].Value = item.BenhVien;
                Sheet.Cells[startrow, 5].Value = item.PhongBan;
                Sheet.Cells[startrow, 6].Value = item.ChucDanh;
                Sheet.Cells[startrow, 7].Value = item.DoiTuongNhanABRTrucTiepGianTiep;
                Sheet.Cells[startrow, 8].Value = item.MucHuongABR/100;
                Sheet.Cells[startrow, 9].Value = item.GhiChu;
                Sheet.Cells[startrow, 11].Value = item.ABRTrucTiep;
                Sheet.Cells[startrow, 12].Value = item.ABRGianTiep;
                Sheet.Cells[startrow, 13].Value = item.ABRChuongTrinhRieng;
                Sheet.Cells[startrow, 15].Value = item.DieuChinh;
                startrow++;
            }
            Sheet.Cells["Q"+ (startrow+1)].Formula = "=SUM(Q7:Q"+startrow +")";
            return pkg;
        }
        
        public async Task<List<ABRDieuChinh>> GetABRDieuChinh(int Thang, int Nam, string MaBenhVien)
        {
            try
            {
                var preSql = await GetpreSql(MaBenhVien);
                var strcon = await GetConnectStr(MaBenhVien);
                var strproc = "proc_ABRGetDieuChinh " + Thang + "," + Nam;
                var str = "exec " + preSql + strproc;
                var result = await sqlDataAccess.loadData<ABRDieuChinh, dynamic>(str, new { }, strcon);
                return result;
            }
            catch
            {

            }
            return null;
        }
        public async Task<bool> SaveABRDieuChinh(ABRDieuChinh item, string MaBenhVien)
        {
            try
            {
                var preSql = await GetpreSql(MaBenhVien);
                var strcon = await GetConnectStr(MaBenhVien);
                var strproc = "proc_ABRSaveDieuChinh " + item.Thang + "," + item.Nam + ",'"+item.MaNhanVien + "'," + item.DieuChinh;
                var str = "exec " + preSql + strproc;
                await sqlDataAccess.execNonQuery(str, strcon);
                return true;
            }
            catch
            {

            }
            return false;
        }
        public async Task<List<AbrSoSanhChiTietVM>> GetABRSoSanhChiTiet(int Thang, int Nam, string MaBenhVienChiNhanh = "", string MaBenhVien = "")
        {
            try
            {
                var preSql = await GetpreSql(MaBenhVien);
                var strcon = await GetConnectStr(MaBenhVien);
                var strproc = "proc_ABRSoSanhChiTiet " + Thang + "," + Nam + (MaBenhVienChiNhanh == "" || MaBenhVienChiNhanh==null ? "" : ",'" + MaBenhVienChiNhanh + "'");


                var str = "exec " + preSql + strproc;
                var result = await sqlDataAccess.loadData<AbrSoSanhChiTietVM, dynamic>(str, new { }, strcon);
                return result;
            }
            catch
            {
                throw;
            }            
        }
        public async Task<List<ABRSoSanhTongHopVM>> GetABRSoSanhTongHop(int Thang, int Nam, string MaBenhVienChiNhanh = "", string MaBenhVien = "")
        {
            try
            {
                var preSql = await GetpreSql(MaBenhVien);
                var strcon = await GetConnectStr(MaBenhVien);
                var strproc = "proc_ABRSoSanhTongHop " + Thang + "," + Nam + (MaBenhVienChiNhanh == "" || MaBenhVienChiNhanh == null ? "" : ",'" + MaBenhVienChiNhanh + "'");
                var str = "exec " + preSql + strproc;
                var result = await sqlDataAccess.loadData<ABRSoSanhTongHopVM, dynamic>(str, new { }, strcon);
                return result;
            }
            catch
            {
                throw;
            }            
        }
        public async Task<List<ABRSoSanhTongHopVM>> GetABRDoanhThuThangTongHop(int Thang, int Nam, string MaBenhVienChiNhanh = "", string MaBenhVien = "")
        {
            try
            {
                var preSql = await GetpreSql(MaBenhVien);
                var strcon = await GetConnectStr(MaBenhVien);
                var strproc = "proc_ABRDoanhThuThangTongHop " + Thang + "," + Nam + (MaBenhVienChiNhanh == "" || MaBenhVienChiNhanh == null ? "" : ",'" + MaBenhVienChiNhanh + "'");
                var str = "exec " + preSql + strproc;
                var result = await sqlDataAccess.loadData<ABRSoSanhTongHopVM, dynamic>(str, new { }, strcon);
                return result;
            }
            catch
            {
                throw;
            }            
        }
        public async Task<List<ABRSoSanhTongHopVM>> GetABRDieuChinhThangTongHop(int Thang, int Nam, string MaBenhVienChiNhanh = "", string MaBenhVien = "")
        {
            try
            {
                var preSql = await GetpreSql(MaBenhVien);
                var strcon = await GetConnectStr(MaBenhVien);
                var strproc = "proc_ABRDieuChinhThangTongHop " + Thang + "," + Nam + (MaBenhVienChiNhanh == "" || MaBenhVienChiNhanh == null ? "" : ",'" + MaBenhVienChiNhanh + "'");
                var str = "exec " + preSql + strproc;
                var result = await sqlDataAccess.loadData<ABRSoSanhTongHopVM, dynamic>(str, new { }, strcon);
                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<ABRDanhGiaNhanVien>> GetBangDanhGiaNhanVien(string MaBenhVien, int thang, int nam)
        {
            try
            {
                var preSql = await GetpreSql(MaBenhVien);
                var strcon = await GetConnectStr(MaBenhVien);
                var strproc = "proc_ABRGetBangDanhGiaNhanVien " + thang + "," + nam;
                var str = "exec " + preSql + strproc;
                var result = await sqlDataAccess.loadData<ABRDanhGiaNhanVien, dynamic>(str, new { }, strcon);
                return result;
            }
            catch
            {

            }
            return null;
        }
        public async Task<bool> DeleteBangDanhGiaNhanVien(string MaBenhVien, int thang, int nam)
        {
            try
            {
                var preSql = await GetpreSql(MaBenhVien);
                var strcon = await GetConnectStr(MaBenhVien);
                var strproc = "proc_ABRDeleteBangDanhGiaNhanVien " + thang + "," + nam;
                var str = "exec " + preSql + strproc;
                await sqlDataAccess.execNonQuery(str, strcon);
                return true;
            }
            catch
            {

            }
            return false;
        }

        public async Task<List<ABRChiTietNhanVienVM>> GetABRChiTietNhanVien(string MaBenhVien, string MaNhanVien, int thang, int nam)
        {
            try
            {
                var preSql = await GetpreSql(MaBenhVien);
                var strcon = await GetConnectStr(MaBenhVien);
                var strproc = "proc_ABRGetABRChiTietNhanVien '"+ MaNhanVien +"'," + thang + "," + nam;
                var str = "exec " + preSql + strproc;
                var result = await sqlDataAccess.loadData<ABRChiTietNhanVienVM,dynamic> (str, new { }, strcon);
                return result;
            }
            catch
            {

            }
            return null;
        }

        static Dictionary<string, IEnumerable<IDictionary<string, object>>> CatchLuongChiTietDichVu = new Dictionary<string, IEnumerable<IDictionary<string, object>>>();
        public async Task<IEnumerable<IDictionary<string, object>>> GetLuongChiTietDichVu(string MaBenhVien, int thang, int nam, int page, int NumRecords)
        {
            var preSql = await GetpreSql(MaBenhVien);
            var strcon = await GetConnectStr(MaBenhVien);
            string Key = MaBenhVien + "-" + thang + "-" + nam;
            bool LaySoLieu = false;
            if (CatchLuongChiTietDichVu.ContainsKey(Key))
            {
                if(page==0)
                {
                    LaySoLieu=true;
                }
            }
            else
            {
                LaySoLieu = true;
            }
            if (LaySoLieu)
            {
                var str = "exec " + preSql + "proc_ABRGetLuongChiTietDichVu " + thang + "," + nam;
                try
                {
                    //var result = await sqlDataAccess.getDataDic(str, strcon);
                    var tb = await sqlDataAccess.getDataTable(str,strcon);

                    var result = DataTableToDictionary(tb);                        
                    if (CatchLuongChiTietDichVu.ContainsKey(Key))
                    {
                        CatchLuongChiTietDichVu[Key] = result;
                    }
                    else
                    {
                        CatchLuongChiTietDichVu.Add(Key,result);
                    }
                }
                catch
                {
                    throw;
                }
            }
            if (CatchLuongChiTietDichVu.ContainsKey(Key))
            {
                if (NumRecords == 0)
                {
                    return CatchLuongChiTietDichVu[Key];
                }
                else
                {
                    return CatchLuongChiTietDichVu[Key].Skip(page * NumRecords).Take(NumRecords);
                }
            }
            return null;
        }
        private List<Dictionary<string, object>> DataTableToDictionary(DataTable dt)
        {
            var dictionaries = new List<Dictionary<string, object>>();
            foreach (DataRow row in dt.Rows)
            {
                Dictionary<string, object> dictionary = Enumerable.Range(0, dt.Columns.Count).ToDictionary(i => dt.Columns[i].ColumnName, i => row.ItemArray[i]);
                dictionaries.Add(dictionary);
            }

            return dictionaries;
        }
        public async Task<ExcelPackage> GetLuongChiTietDichVu(string MaBenhVien, int thang, int nam)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage pkg;
            pkg = new ExcelPackage();
            pkg.Workbook.Properties.Title = "Weekly Report";
            pkg.Workbook.Properties.Author = "Tran Thien Thanh 0908325345";
            pkg.Workbook.Properties.Subject = "Weekly Report";            
            ExcelWorksheet sheet;                        
            sheet = pkg.Workbook.Worksheets.Add("Weekly Report");
            var preSql = await GetpreSql(MaBenhVien);
            var strcon = await GetConnectStr(MaBenhVien);
            string Key = MaBenhVien + "-" + thang + "-" + nam;                        
            var str = "exec " + preSql + "proc_ABRGetLuongChiTietDichVu " + thang + "," + nam;
            try
            {
                DataTable result = await sqlDataAccess.getDataTable(str, strcon);                
                result.TableName = "ChiTiet";
                sheet.Cells["A1"].LoadFromDataTable(result);
            }
            catch
            {
                throw;
            }            
            return pkg;
        }
        public async Task<IEnumerable<IDictionary<string, object>>> GetGetLuongNhomDichVu(string MaBenhVien, int thang, int nam)
        {
            var preSql = await GetpreSql(MaBenhVien);
            var strcon = await GetConnectStr(MaBenhVien);
            var str = "exec " + preSql + "proc_ABRGetLuongNhomDichVu " + thang + "," + nam;
            try
            {
                var result = await sqlDataAccess.getDataDic(str, strcon);
                return result;
            }
            catch
            {
            }
            return null;
        }
        #endregion Báo cáo

        #region Tính ABR
        public async Task<bool> SaveDanhGiaNhanVien(string MaBenhVien, ABRDanhGiaNhanVien aBRDanhGiaNhanVien)
        {
            var preSql = await GetpreSql(MaBenhVien);
            var strcon = await GetConnectStr(MaBenhVien);
            var str = "exec " + preSql + "proc_AbrSaveDanhGiaNhanVien  " + aBRDanhGiaNhanVien.SoThuTu + "," + aBRDanhGiaNhanVien.Thang + ", " + aBRDanhGiaNhanVien.Nam + ",'" + aBRDanhGiaNhanVien.MaNhanVien + "',"+ aBRDanhGiaNhanVien.MucTinhABRTrongThang +",N'"+ aBRDanhGiaNhanVien.LoaiDoiTuong + "',N'"+ aBRDanhGiaNhanVien.GhiChu + "'";
            try
            {
                await sqlDataAccess.execNonQuery(str, strcon);
                return true;
            }
            catch
            {
            }
            return false;
        }

        public async Task<List<AbrPool>> GetDanhSachPool(string MaBenhVien)
        {
            var preSql = await GetpreSql(MaBenhVien);
            var strcon = await GetConnectStr(MaBenhVien);
            var str = "exec " + preSql + "proc_AbrLayDanhSachPool";
            try
            {
                var results = await sqlDataAccess.loadData<AbrPool, dynamic>(str, new { }, strcon);
                return results;
            }
            catch
            {

            }
            return null;

        }

        public async Task<AbrPool> SavePool(string MaBenhVien, AbrPool abrPool)
        {
            var preSql = await GetpreSql(MaBenhVien);
            var strcon = await GetConnectStr(MaBenhVien);
            if ((abrPool.ID == Guid.Empty))
            {
                abrPool.ID = Guid.NewGuid();
            }
            var str = "exec " + preSql + "proc_AbrSavePool '"+abrPool.ID+"',N'"+abrPool.TenPool+"'";
            try
            {
                var results = await sqlDataAccess.loadData<AbrPool, dynamic>(str, new { }, strcon);
                return abrPool;
            }
            catch
            {

            }
            return null;
        }

        public async Task<bool> DeletePool(string MaBenhVien, Guid id)
        {
            var preSql = await GetpreSql(MaBenhVien);
            var strcon = await GetConnectStr(MaBenhVien);
            var str = "exec " + preSql + "proc_AbrDeletePool '" + id + "'";
            try
            {
                var results = await sqlDataAccess.loadData<AbrPool, dynamic>(str, new { }, strcon);
                return true;
            }
            catch
            {

            }
            return false;

        }

        public async Task<bool> SaveXetDuyet(string MaBenhVien, int Thang, int Nam, int Muc, List<ABRSoSanhTongHopVM> listTongHop)
        {
            var preSql = await GetpreSql(MaBenhVien);
            var strcon = await GetConnectStr(MaBenhVien);
            var str = "exec " + preSql + "proc_AbrSaveXetDuyet ";
            var sqlitem = "";
            try
            {
                foreach (var item in listTongHop)
                {
                    sqlitem = str + Thang +"," + Nam
                        + ", N'" + item.Muc + "'"
                        + ", N'" + item.group1 + "'"
                        + ", N'" + item.NoiDung + "'"
                        + ", " + item.ThucTeThangTruoc
                        + ", " + item.NganSachThangNay
                        + ", " + item.ThucTeThangNay
                        + ", " + item.ChenhLechThang
                        + ", " + item.PhanTramChenhLechThang
                        + ", " + item.ChenhLechThucTeNganSach
                        + ", " + item.PhanTramChenhLechThucTeNganSach
                        + ", " + Muc
                        + (item.MaBenhVien=="" || item.MaBenhVien==null ? "" : ", '" + item.MaBenhVien +"'");
                    await sqlDataAccess.execNonQuery(sqlitem, strcon);
                }
                return true;
            }
            catch
            {

            }
            return false;

        }
        public async Task<bool> DeleteXetDuyet(string MaBenhVien, int Thang, int Nam, int Muc, string MaBenhVienChiNhanh)
        {
            var preSql = await GetpreSql(MaBenhVien);
            var strcon = await GetConnectStr(MaBenhVien);
            var str = "exec " + preSql + "proc_AbrDeleteXetDuyet " + Thang +"," + Nam +"," + Muc +(MaBenhVienChiNhanh=="" || MaBenhVienChiNhanh==null ? "": ",'"+ MaBenhVienChiNhanh + "'" );            
            try
            {                
                await sqlDataAccess.execNonQuery(str, strcon);                
            }
            catch
            {

            }
            return false;

        }
        public async Task<bool> CheckDaTinhSoLuongABR(string MaBenhVien, int Thang, int Nam)
        {
            var preSql = await GetpreSql(MaBenhVien);
            var strcon = await GetConnectStr(MaBenhVien);
            var str = "exec " + preSql + "proc_AbrCheckDaTinhSoLuongABR  " + Thang + "," + Nam;
            try
            {
                var results = await sqlDataAccess.getDataTable(str, strcon);
                if (results != null && results.Rows.Count > 0)
                {
                    return true;
                }
            }
            catch
            {

            }
            return false;
        }

        public async Task<bool> CheckDaUploadBangDanhGia(string MaBenhVien, int Thang, int Nam)
        {
            var preSql = await GetpreSql(MaBenhVien);
            var strcon = await GetConnectStr(MaBenhVien);
            var str = "exec " + preSql + "proc_AbrCheckDaUploadBangDanhGia  " + Thang + "," + Nam;
            try
            {
                var results = await sqlDataAccess.getDataTable(str, strcon);
                if (results != null && results.Rows.Count > 0)
                {
                    return true;
                }
            }
            catch
            {

            }
            return false;
        }

        public async Task<bool> CheckDaTinhABR(string MaBenhVien, int Thang, int Nam)
        {
            var preSql = await GetpreSql(MaBenhVien);
            var strcon = await GetConnectStr(MaBenhVien);
            var str = "exec " + preSql + "proc_AbrCheckDaTinhABR " + Thang + "," + Nam;
            try
            {
                var results = await sqlDataAccess.getDataTable(str, strcon);
                if (results != null && results.Rows.Count > 0)
                {
                    return true;
                }
            }
            catch
            {

            }
            return false;
        }
        public async Task<bool> CheckDaXetDuyet(string MaBenhVien, int Thang, int Nam, int Muc, string MaBenhVienChiNhanh="")
        {
            var preSql = await GetpreSql(MaBenhVien);
            var strcon = await GetConnectStr(MaBenhVien);
            var str = "exec " + preSql + "proc_AbrCheckDaXetDuyet " + Thang + "," + Nam+ "," + Muc +(MaBenhVienChiNhanh=="" || MaBenhVienChiNhanh == null? "":",'" + MaBenhVienChiNhanh + "'");
            try
            {
                var results = await sqlDataAccess.getDataTable(str, strcon);
                if (results != null && results.Rows.Count > 0)
                {
                    return true;
                }
            }
            catch
            {

            }
            return false;
        }

        public async Task<bool> CheckDaXetDuyetTheoNgay(string MaBenhVien, DateTime Ngay, string MaBenhVienChiNhanh ="")
        {
            var preSql = await GetpreSql(MaBenhVien);
            var strcon = await GetConnectStr(MaBenhVien);
            var str = "exec " + preSql + "proc_AbrCheckDaXetDuyetTheoNgay '"+Ngay.ToString("yyyy-MM-dd") +"'" + (MaBenhVienChiNhanh=="" || MaBenhVienChiNhanh == null? "":",'"+ MaBenhVienChiNhanh+"'") ;
            try
            {
                var results = await sqlDataAccess.getDataTable(str, strcon);
                if (results != null && results.Rows.Count > 0)
                {
                    return true;
                }
            }
            catch
            {

            }
            return false;
        }
        public async Task<int> GetSoLanTuChoi(string MaBenhVien, int Thang, int Nam, int Muc)
        {
            var preSql = await GetpreSql(MaBenhVien);
            var strcon = await GetConnectStr(MaBenhVien);
            var str = "exec " + preSql + "proc_AbrGetSoLanTuChoi " + Thang + "," + Nam + "," + Muc;
            try
            {
                var results = await sqlDataAccess.getDataTable(str, strcon);
                if (results != null && results.Rows.Count > 0)
                {
                    return int.Parse(results.Rows[0][0].ToString());
                }
                return 0;
            }
            catch
            {

            }
            return 0;
        }
        public async Task<bool> TinhHuongABR(string MaBenhVien, int Thang, int Nam)
        {
            var preSql = await GetpreSql(MaBenhVien);
            var strcon = await GetConnectStr(MaBenhVien);
            var str = "exec " + preSql + "proc_ABRTinhHuong " + Thang + "," + Nam;
            try
            {
                var results = await sqlDataAccess.getDataTable(str, strcon);
                if (results != null && results.Rows.Count > 0)
                {
                    return true;
                }
            }
            catch
            {

            }
            return false;
        }

        public async Task<List<ABRNganSachThang>> GetNganSachThang(string MaBenhVien, int thang, int nam, string MaBenhVienChiNhanh = "")
        {
            var preSql = await GetpreSql(MaBenhVien);
            var strcon = await GetConnectStr(MaBenhVien);
             var str = "exec " + preSql + "proc_AbrLayNganSachThang " + thang + "," + nam + (MaBenhVienChiNhanh=="" || MaBenhVienChiNhanh==null ?"" : ",'"+ MaBenhVienChiNhanh +"'");
            try
            {
                var results = await sqlDataAccess.loadData<ABRNganSachThang, dynamic>(str, new { }, strcon);
                return results;
            }
            catch
            {

            }
            return null;
        }

        public async Task<bool> SaveNganSachThang(string MaBenhVien, ABRNganSachThang item)
        {
            var preSql = await GetpreSql(MaBenhVien);
            var strcon = await GetConnectStr(MaBenhVien);
            var str = "exec " + preSql + "proc_AbrSaveNganSachThang " + item.Thang + "," + item.Nam + ",N'"+ item.Muc +"',N'"+ item.group1 + "',N'" + item.NoiDung +"'," + item.SoLuong +"," + item.STT + (item.MaBenhVien==""?"": ",'"+ item.MaBenhVien +"'") ;
            try
            {
                await sqlDataAccess.execNonQuery(str, strcon);
                return true;
            }
            catch
            {

            }
            return false;
        }
        public async Task<List<ABRThucHienCuoiVM>> GetThucHienCuoi(string MaBenhVien)
        {
            var preSql = await GetpreSql(MaBenhVien);
            var strcon = await GetConnectStr(MaBenhVien);
            var str = "exec " + preSql + "proc_AbrGetThucHienCuoi ";
            try
            {
                return await sqlDataAccess.loadData<ABRThucHienCuoiVM, dynamic>(str, new { }, strcon);                
            }
            catch
            {

            }
            return null;
        }

        #endregion


        #region Loại vai trò
        public async Task<List<ABRLoaiVaiTro>> GetDanhSachLoaiVaiTro(string MaBenhVien)
        {
            try
            {
                var preSql = await GetpreSql(MaBenhVien);
                var strcon = await GetConnectStr(MaBenhVien);
                var strproc = "proc_ABRGetDanhSachLoaiVaiTro";

                var str = "exec " + preSql + strproc;
                var result = await sqlDataAccess.loadData<ABRLoaiVaiTro, dynamic>(str, new { }, strcon);
                return result;
            }
            catch
            {

            }
            return null;
        }
        #endregion //Loại vai trò

        #region Tháng Năm
        public async Task<ABRThangNam> getNgayTheoThang(string MaBenhVien, int Thang, int Nam)
        {
            try
            {
                var preSql = await GetpreSql(MaBenhVien);
                var strcon = await GetConnectStr(MaBenhVien);
                var strproc = "proc_ABRGetNgayTheoThang "+Thang + "," + Nam;

                var str = "exec " + preSql + strproc;
                var result = await sqlDataAccess.loadData<ABRThangNam, dynamic>(str, new { }, strcon);
                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ABRThangNam> SaveNgayTheoThang(string MaBenhVien, ABRThangNam item)
        {
            try
            {
                var preSql = await GetpreSql(MaBenhVien);
                var strcon = await GetConnectStr(MaBenhVien);
                var strproc = "proc_ABRSaveNgayTheoThang " + item.Thang + "," + item.Nam + ",'"+ item.TuNgay.ToString("yyyy-MM-dd") +"','"+ item.DenNgay.ToString("yyyy-MM-dd 23:59:59") +"'";
                var str = "exec " + preSql + strproc;
                var result = await sqlDataAccess.loadData<ABRThangNam, dynamic>(str, new { }, strcon);
                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Mức tính ABR cho nhân viên
        public async Task<List<ABRDanhMucNhanVienVM>> GetDanhSackDanhMucNhanVien(string MaBenhVien, Guid IDNhanVien)
        {
            try
            {
                var preSql = await GetpreSql(MaBenhVien);
                var strcon = await GetConnectStr(MaBenhVien);
                var strproc = "Proc_ABRGetDanhMucNhanVien '" + IDNhanVien + "'";
                var str = "exec " + preSql + strproc;
                var result = await sqlDataAccess.loadData<ABRDanhMucNhanVienVM, dynamic>(str, new { }, strcon);
                return result;
            }
            catch
            {

            }
            return null;
        }

        public async Task<ABRDanhMucNhanVienVM> SaveDanhMucNhanVien(string MaBenhVien, ABRDanhMucNhanVienVM aBRDanhMucNhanVienVM)
        {
            try
            {
                var preSql = await GetpreSql(MaBenhVien);
                var strcon = await GetConnectStr(MaBenhVien);
                if (!aBRDanhMucNhanVienVM.ID.HasValue || aBRDanhMucNhanVienVM.ID == Guid.Empty)
                {
                    aBRDanhMucNhanVienVM.ID = Guid.NewGuid();
                }
                string strproc = preSql+ "Proc_ABRSaveDanhMucNhanVien '" + aBRDanhMucNhanVienVM.ID + "'"
                        + ",'" + aBRDanhMucNhanVienVM.IDNhanVien + "'"
                        + "," + aBRDanhMucNhanVienVM.IDABRDanhMuc
                        + "," + aBRDanhMucNhanVienVM.MucHuongVND
                        + "," + aBRDanhMucNhanVienVM.MucHuongPhanTram
                        + "," + aBRDanhMucNhanVienVM.TinhTheoPoolThucHien
                        + "," + aBRDanhMucNhanVienVM.TyLeGianTiep
                        + "," + aBRDanhMucNhanVienVM.HuongToiDa
                        + ",N'" + aBRDanhMucNhanVienVM.GhiChu + "'";
                
                await sqlDataAccess.execNonQuery(strproc);
                return aBRDanhMucNhanVienVM;
            }
            catch
            {

            }
            return null;
        }

        public async Task<bool> DeleteDanhMucNhanVien(string MaBenhVien, ABRDanhMucNhanVienVM aBRDanhMucNhanVienVM)
        {
            try
            {
                var preSql = await GetpreSql(MaBenhVien);
                var strcon = await GetConnectStr(MaBenhVien);
                if (!aBRDanhMucNhanVienVM.ID.HasValue || aBRDanhMucNhanVienVM.ID == Guid.Empty)
                {
                    aBRDanhMucNhanVienVM.ID = Guid.NewGuid();
                }
                string strproc = preSql+"Proc_ABRDeleteDanhMucNhanVien '" + aBRDanhMucNhanVienVM.ID + "'";

                await sqlDataAccess.execNonQuery(strproc);
                return true;
            }
            catch
            {

            }
            return false;
        }
        #endregion
        #region Pool theo danh mục
        public async Task<List<AbrPool>> GetPoolHuongTheoDanhMuc(string maBenhVien, int idDanhMuc)
        {
            var preSql = await GetpreSql(maBenhVien);
            var strcon = await GetConnectStr(maBenhVien);
            var str = "exec " + preSql + "proc_ABRGetPoolHuongTheoDanhMuc " + idDanhMuc;
            try
            {
                var result = await sqlDataAccess.loadData<AbrPool, dynamic>(str, new { }, strcon);
                return result;
            }
            catch
            {
                throw;
            }

        }
        public async Task<bool> DeletePoolHuongTheoDanhMuc(string maBenhVien, int idDanhMuc)
        {
            var preSql = await GetpreSql(maBenhVien);
            var strcon = await GetConnectStr(maBenhVien);
            var str = "exec " + preSql + "proc_ABRDeletePoolHuongTheoDanhMuc " + idDanhMuc;
            try
            {
                await sqlDataAccess.execNonQuery(str, strcon);
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> SavePoolHuongTheoDanhMuc(string maBenhVien, int idDanhMuc, Guid idPool)
        {
            var preSql = await GetpreSql(maBenhVien);
            var strcon = await GetConnectStr(maBenhVien);
            var str = "exec " + preSql + "proc_ABRSavePoolHuongTheoDanhMuc " + idDanhMuc + ",'" + idPool + "'";
            try
            {
                await sqlDataAccess.execNonQuery(str, strcon);
                return true;
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region Xác nhận his

        public async Task<List<ABRXacNhanNhanVienThucHienHisVM>> GetXacNhanNhanVienThucHienHis(string MaBenhVien, string IDUser, DateTime TuNgay, DateTime DenNgay)
        {
            try
            {
                var preSql = await GetpreSql(MaBenhVien);
                var strcon = await GetConnectStr(MaBenhVien);

                var strproc = "Proc_ABRGetXacNhanNhanVienThucHienHis '" + IDUser + "','"+TuNgay.ToString("yyyy-MM-dd") + "','" + DenNgay.ToString("yyyy-MM-dd 23:59:59") + "'" ;
                var str = "exec " + preSql + strproc;
                var result = await sqlDataAccess.loadData<ABRXacNhanNhanVienThucHienHisVM, dynamic>(str, new { }, strcon);
                return result;
            }
            catch
            {
                throw;
            }
        }
        public async Task<ABRXacNhanNhanVienThucHienHisVM> SaveXacNhanNhanVienThucHienHis(string MaBenhVien, ABRXacNhanNhanVienThucHienHisVM item)
        {
            try
            {
                var preSql = await GetpreSql(MaBenhVien);
                var strcon = await GetConnectStr(MaBenhVien);
                if (item.ID == Guid.Empty)
                {
                    item.ID = Guid.NewGuid();
                }

                var strproc = "Proc_ABRSaveXacNhanNhanVienThucHienHis '" + item.ID + "','" + item.ID_DSCV + "','" + item.IDDanhMucABR + "','" + item.MaNhanVien + "','"+ item.UserLuu +"'";
                var str = "exec " + preSql + strproc;
                await sqlDataAccess.execNonQuery(str, strcon);
                return item;
            }
            catch
            {
                throw;
            }
        }
        public async Task<bool> DeleteXacNhanNhanVienThucHienHis(string MaBenhVien, ABRXacNhanNhanVienThucHienHisVM item)
        {
            try
            {
                var preSql = await GetpreSql(MaBenhVien);
                var strcon = await GetConnectStr(MaBenhVien);
                if (item.ID == Guid.Empty)
                {
                    item.ID = Guid.NewGuid();
                }

                var strproc = "Proc_ABRDeleteXacNhanNhanVienThucHienHis '" + item.ID + "'";
                var str = "exec " + preSql + strproc;
                await sqlDataAccess.execNonQuery(str, strcon);
                return true;
            }
            catch
            {
                throw;
            }
        }
       
        public async Task<bool> DeleteUserKetThucCongViecHis(string MaBenhVien, Guid ID)
        {
            try
            {
                var preSql = await GetpreSql(MaBenhVien);
                var strcon = await GetConnectStr(MaBenhVien);
                var strproc = "proc_ABRDeleteUserKetThucCongViecHis '" + ID + "'";
                var str = "exec " + preSql + strproc;
                await sqlDataAccess.execNonQuery(str, strcon);
                return true;
            }
            catch
            {
                throw;
            }            
        }

        public async Task<ABRUserKetThucCongViecHis> SaveUserKetThucCongViecHis(string MaBenhVien, ABRUserKetThucCongViecHis item)
        {
            try
            {

                if (item.ID== null || item.ID==Guid.Empty)
                {
                    item.ID = Guid.NewGuid();
                }
                var preSql = await GetpreSql(MaBenhVien);
                var strcon = await GetConnectStr(MaBenhVien);
                var strproc = "proc_ABRSaveUserKetThucCongViecHis '" + item.ID + "', '"+ item.IDUser +"',N'"+ item.MaCV +"'";
                var str = "exec " + preSql + strproc;
                await sqlDataAccess.execNonQuery(str, strcon);
                return item;
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<ABRUserKetThucCongViecHisVM>> GetUserKetThucCongViecHis(string MaBenhVien)
        {
            try
            {
                
                var preSql = await GetpreSql(MaBenhVien);
                var strcon = await GetConnectStr(MaBenhVien);
                var strproc = "proc_ABRGetUserKetThucCongViecHis";
                var str = "exec " + preSql + strproc;
                var result = await sqlDataAccess.loadData<ABRUserKetThucCongViecHisVM, dynamic>(str, new { }, strcon);
                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<UserVM>> LayUserKetThucHis(string MaBenhVien)
        {
            try
            {
                var strproc = "proc_ABRLayUserKetThucHis '" +  MaBenhVien+"'";
                var str = "exec " + strproc;
                var result = await sqlDataAccess.loadData<UserVM, dynamic>(str, new { });
                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<IDictionary<string, object>>> GetBaoCaoXacNhanNhanVienHis(string MaBenhVien, TuNgayDenNgayOneParaSM sM)
        {
            try
            {

                var preSql = await GetpreSql(MaBenhVien);
                var strcon = await GetConnectStr(MaBenhVien);
                var strproc = "proc_ABRGetBaoCaoXacNhanNhanVienHis '"+sM.Para1 +"','"+sM.TuNgay.ToString("yyyy-MM-dd")+"','"+ sM.DenNgay.ToString("yyyy-MM-dd 23:59:59") +"'";
                var str = "exec " + preSql + strproc;
                var result = await sqlDataAccess.getDataDic(str);
                return result;
            }
            catch
            {
                throw;
            }

        }
        public async Task<IEnumerable<IDictionary<string, object>>> GetBaoCaoChiTietXacNhanNhanVienHis(string MaBenhVien, TuNgayDenNgayOneParaSM sM)
        {
            try
            {

                var preSql = await GetpreSql(MaBenhVien);
                var strcon = await GetConnectStr(MaBenhVien);
                var strproc = "proc_ABRGetBaoCaoChiTietXacNhanNhanVienHis '"+sM.Para1 +"','"+sM.TuNgay.ToString("yyyy-MM-dd")+"','"+ sM.DenNgay.ToString("yyyy-MM-dd 23:59:59") +"'";
                var str = "exec " + preSql + strproc;
                var result = await sqlDataAccess.getDataDic(str);
                return result;
            }
            catch
            {
                throw;
            }

        }

        public async Task<List<ABRUserXacNhanNoiLamViec>> GetUserXacNhanNoiLamViecThucHien(string MaBenhVien)
        {
            try
            {

                var preSql = await GetpreSql(MaBenhVien);
                var strcon = await GetConnectStr(MaBenhVien);
                var strproc = "proc_ABRGetUserXacNhanNoiLamViecThucHien";
                var str = "exec " + preSql + strproc;
                var result = await sqlDataAccess.loadData<ABRUserXacNhanNoiLamViec, dynamic>(str, new { }, strcon);
                return result;
            }
            catch
            {
                throw;
            }
        }
        public async Task<ABRUserXacNhanNoiLamViec> SaveUserXacNhanNoiLamViecThucHien(string MaBenhVien, ABRUserXacNhanNoiLamViec aBRUserXacNhanNoiLamViec)
        {
            try
            {
                var preSql = await GetpreSql(MaBenhVien);
                var strcon = await GetConnectStr(MaBenhVien);
                if (aBRUserXacNhanNoiLamViec.ID==Guid.Empty)
                {
                    aBRUserXacNhanNoiLamViec.ID = Guid.NewGuid();
                }
                var strproc = "proc_ABRSaveUserXacNhanNoiLamViecThucHien '" + aBRUserXacNhanNoiLamViec.ID + "','" + aBRUserXacNhanNoiLamViec.IDUser +"','" + aBRUserXacNhanNoiLamViec.MaNoiLamViec + "'" ;
                var str = "exec " + preSql + strproc;
                await sqlDataAccess.execNonQuery(str, strcon);
                return aBRUserXacNhanNoiLamViec;
            }
            catch
            {
                throw;
            }
        }
        public async Task<bool> DeleteUserXacNhanNoiLamViecThucHien(string MaBenhVien, ABRUserXacNhanNoiLamViec aBRUserXacNhanNoiLamViec)
        {
            try
            {
                var preSql = await GetpreSql(MaBenhVien);
                var strcon = await GetConnectStr(MaBenhVien);
                if (aBRUserXacNhanNoiLamViec.ID == Guid.Empty)
                {
                    aBRUserXacNhanNoiLamViec.ID = Guid.NewGuid();
                }
                var strproc = "proc_ABRDeleteUserXacNhanNoiLamViecThucHien '" + aBRUserXacNhanNoiLamViec.ID + "'";
                var str = "exec " + preSql + strproc;
                await sqlDataAccess.execNonQuery(str, strcon);
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<ABRUserXacNhanNoiLamViec>> GetUserXacNhanNoiLamViecChiDinh(string MaBenhVien)
        {
            try
            {

                var preSql = await GetpreSql(MaBenhVien);
                var strcon = await GetConnectStr(MaBenhVien);
                var strproc = "proc_ABRGetUserXacNhanNoiLamViecChiDinh";
                var str = "exec " + preSql + strproc;
                var result = await sqlDataAccess.loadData<ABRUserXacNhanNoiLamViec, dynamic>(str, new { }, strcon);
                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<ABRUserXacNhanNoiLamViec> SaveUserXacNhanNoiLamViecChiDinh(string MaBenhVien, ABRUserXacNhanNoiLamViec aBRUserXacNhanNoiLamViec)
        {
            try
            {
                var preSql = await GetpreSql(MaBenhVien);
                var strcon = await GetConnectStr(MaBenhVien);
                if (aBRUserXacNhanNoiLamViec.ID == Guid.Empty)
                {
                    aBRUserXacNhanNoiLamViec.ID = Guid.NewGuid();
                }
                var strproc = "proc_ABRSaveUserXacNhanNoiLamViecChiDinh '" + aBRUserXacNhanNoiLamViec.ID + "','" + aBRUserXacNhanNoiLamViec.IDUser + "','" + aBRUserXacNhanNoiLamViec.MaNoiLamViec + "'";
                var str = "exec " + preSql + strproc;
                await sqlDataAccess.execNonQuery(str, strcon);
                return aBRUserXacNhanNoiLamViec;
            }
            catch
            {
                throw;
            }
        }
        public async Task<bool> DeleteUserXacNhanNoiLamViecChiDinh(string MaBenhVien, ABRUserXacNhanNoiLamViec aBRUserXacNhanNoiLamViec)
        {
            try
            {
                var preSql = await GetpreSql(MaBenhVien);
                var strcon = await GetConnectStr(MaBenhVien);
                if (aBRUserXacNhanNoiLamViec.ID == Guid.Empty)
                {
                    aBRUserXacNhanNoiLamViec.ID = Guid.NewGuid();
                }
                var strproc = "proc_ABRDeleteUserXacNhanNoiLamViecChiDinh '" + aBRUserXacNhanNoiLamViec.ID + "'";
                var str = "exec " + preSql + strproc;
                await sqlDataAccess.execNonQuery(str, strcon);
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<ABRNoiLamViecVM>> GetNoiLamViec(string MaBenhVien)
        {
            try
            {
                var preSql = await GetpreSql(MaBenhVien);
                var strcon = await GetConnectStr(MaBenhVien);                
                var strproc = "proc_ABRGetNoiLamViec";
                var str = "exec " + preSql + strproc;
                var result = await sqlDataAccess.loadData<ABRNoiLamViecVM, dynamic>(str, new { }, strcon);
                return result;
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region ngày công nhân viên
        public async Task<List<ABRNgayCong>> GetNgayCong(string MaBenhVien, int Thang, int Nam)
        {
            try
            {

                var preSql = await GetpreSql(MaBenhVien);
                var strcon = await GetConnectStr(MaBenhVien);
                var strproc = "proc_ABRGetNgayCong '" + Thang+"','" + Nam + "'";
                var str = "exec " + preSql + strproc;
                var result = await sqlDataAccess.loadData<ABRNgayCong, dynamic>(str, new { }, strcon);
                return result;
            }
            catch
            {
                throw;
            }
        }
        public async Task<ABRNgayCong> SaveNgayCong(string MaBenhVien, ABRNgayCong item)
        {
            try
            {
                var preSql = await GetpreSql(MaBenhVien);
                var strcon = await GetConnectStr(MaBenhVien);
                var strproc = "proc_ABRSaveNgayCong '" + item.Thang + "','" + item.Nam + "','" + item.MaNhanVien+ "'," + item.NgayCong;
                var str = "exec " + preSql + strproc;
                await sqlDataAccess.execNonQuery(str);
                return item;
            }
            catch
            {
                throw;
            }
        }
       
        public async Task<bool> DeleteNgayCong(string MaBenhVien, ABRNgayCong item)
        {
            try
            {
                var preSql = await GetpreSql(MaBenhVien);
                var strcon = await GetConnectStr(MaBenhVien);
                var strproc = "proc_ABRDeleteNgayCong '" + item.Thang + "','" + item.Nam + "','" + item.MaNhanVien + "'";
                var str = "exec " + preSql + strproc;
                await sqlDataAccess.execNonQuery(str);
                return true;
            }
            catch
            {
                throw;
            }
        }
        #endregion
        #region Nhiều bệnh viện        
        public async Task<List<BenhVienVM>> GetBenhVien(string MaBenhVien)
        {
            try
            {

                var preSql = await GetpreSql(MaBenhVien);
                var strcon = await GetConnectStr(MaBenhVien);
                var strproc = "proc_ABRGetBenhVien";
                var str = "exec " + preSql + strproc;
                var result = await sqlDataAccess.loadData<BenhVienVM, dynamic>(str, new { }, strcon);
                return result;
            }
            catch
            {
                throw;
            }
        }

        #endregion

        private async Task<string> GetpreSql(string maBenhVien)
        {

            DataTable tbl = await sqlDataAccess.getDataTable("select max(PreSql) PreSql  from UserBenhVien where MaBenhVien='" + maBenhVien + "'");
            return tbl.Rows[0][0].ToString();
        }
        private async Task<string> GetConnectStr(string maBenhVien)
        {

            DataTable tbl = await sqlDataAccess.getDataTable("select max(ConnectString) ConnectString  from UserBenhVien where MaBenhVien='" + maBenhVien + "'");
            
            string str = tbl.Rows[0][0].ToString();
            if (str=="")
            {
                return "";
            }
            var base64EncodedBytes = System.Convert.FromBase64String(str);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

    }
}
