using DataAccessLibrary;
using DB;
using Microsoft.EntityFrameworkCore;
using SalesAuto.Models.Entities.CRM;
using SalesAuto.Models.Entities.HenKham;
using SalesAuto.Models.ViewModel.HenKham;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SalesAuto.Api.Repositories
{
    public class HenKhamRepo : IHenKhamRepo
    {
        private readonly SalesAutoDbContext context;
        private readonly ISqlDataAccess sqlDataAccess;
        private readonly IBenhViensRepo benhViensRepository;
        private readonly ICRMClientRepo cRMClientRep;

        public HenKhamRepo(SalesAutoDbContext context, ISqlDataAccess sqlDataAccess, IBenhViensRepo benhViensRepository, ICRMClientRepo cRMClientRep)
        {
            this.context = context;
            this.sqlDataAccess = sqlDataAccess;
            this.benhViensRepository = benhViensRepository;
            this.cRMClientRep = cRMClientRep;
        }
        private async Task<string> GetpreSql(string maBenhVien)
        {
            DataTable tbl = await sqlDataAccess.getDataTable("select PreSql  from BenhVien where MaBenhVien='" + maBenhVien + "'");
            return tbl.Rows[0][0].ToString();
        }
        public async Task<List<BenhNhanHenKham>> GetDanhSachHenKham(string MaBenhVien, DateTime TuNgay, DateTime DenNgay, bool LayTheoNgayHen = true, bool BacSyHen = true, bool HoSoLasik = true, bool BenhChuaHen = true)
        {
            try
            {
                var preSql = await GetpreSql(MaBenhVien);
                var sql = "exec " + preSql + "proc_HenKhamLayTheoToaNgayKham '" + TuNgay.ToString("yyyy-MM-dd") + "','" + DenNgay.ToString("yyyy-MM-dd 23:59:59") + "', " + (BacSyHen ? "1" : "0") + "," + (HoSoLasik ? "1" : "0") + "," + (BenhChuaHen ? "1" : "0");
                if (LayTheoNgayHen)
                {
                    sql = "exec " + preSql + "proc_HenKhamLayTheoToaNgayHen '" + TuNgay.ToString("yyyy-MM-dd") + "','" + DenNgay.ToString("yyyy-MM-dd 23:59:59") + "', " + (BacSyHen ? "1" : "0") + "," + (HoSoLasik ? "1" : "0") + "," + (BenhChuaHen ? "1" : "0");
                }
                var result = await sqlDataAccess.loadData<BenhNhanHenKham, dynamic>(sql, new { });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<BenhNhanHenKham>> GetHenKhamThucHienCuoi(string MaBenhVien)
        {
            try
            {
                var preSql = await GetpreSql(MaBenhVien);
                var sql = "exec " + preSql + "proc_HenKhamLayThuHienCuoi ";
               
                var result = await sqlDataAccess.loadData<BenhNhanHenKham, dynamic>(sql, new { });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<BenhNhanHenKham> GetHenKham(string MaBenhVien, Guid ID)
        {
            try
            {
                var preSql = await GetpreSql(MaBenhVien);
                var sql = "exec " + preSql + "proc_HenKhamLayTheoID '" + ID + "'";

                var result = await sqlDataAccess.loadData<BenhNhanHenKham, dynamic>(sql, new { });
                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<Guid?> AddHenKhamFromHis(string MaBenhVien, BenhNhanHenKham benhHenKham)
        {
            var preSql = await GetpreSql(MaBenhVien);
            try
            {
                if (!benhHenKham.ID.HasValue)
                {
                    benhHenKham.ID = Guid.NewGuid();
                }
                var sql = "exec " + preSql + @"proc_HenKhamSaveDSHenKham  
                         N'" + benhHenKham.ID + @"'
                        , '" + benhHenKham.MaBenhNhan + @"'
                        ,N'" + benhHenKham.HoTen + @"'
                        , " + benhHenKham.Tuoi + @"
	                    , N'" + benhHenKham.GioiTinh + @"'
                        , N'" + benhHenKham.DienThoai + @"'
                        , N'" + benhHenKham.DiaChi + @"'
                        , " + (benhHenKham.NgayHen != null ? "'" + ((DateTime)benhHenKham.NgayHen).ToString("yyyy-MM-dd HH:mm:ss") + @"'" : "null") + @"
                        , " + (benhHenKham.NgayHen != null ? "'" + ((DateTime)benhHenKham.NgayHen).ToString("yyyy-MM-dd HH:mm:ss") + @"'" : "null") + @"
                        , N'" + benhHenKham.LyDoHen + @"'
                        , null
                        , N'" + benhHenKham.MaBenhAn + @"'
                        , null 
                        , null
                        , N'" + benhHenKham.BsKham + @"'
                        , N'" + benhHenKham.CRM_product_code + @"'
                        , " + (benhHenKham.id_order_status==null ? 0: benhHenKham.id_order_status);
                await sqlDataAccess.execNonQuery(sql);
                return benhHenKham.ID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> PushHenKhamToCRM(string MaBenhVien, Guid ID)
        {
            try
            {
                var BenhVien = (await benhViensRepository.GetBenhVienByMaBenhVien(MaBenhVien)).FirstOrDefault();
                if (BenhVien != null)
                {
                    var HenKham = await GetHenKham(MaBenhVien, ID);
                    var Order = new OrderCreate();
                    Order.customer_name = HenKham.HoTen;
                    Order.customer_phone = HenKham.DienThoai;
                    Order.email = "";
                    Order.address = HenKham.DiaChi;
                    Order.customer_phone = HenKham.DienThoai;
                    Order.id_province = 0;
                    Order.id_district = 0;
                    Order.id_ward = 0;
                    Order.store_code = BenhVien.CRM_store_code;
                    Order.order_status = (HenKham.id_order_status==null? 0: HenKham.id_order_status);
                    Order.total_price = 0;
                    Order.order_date = (DateTime)(HenKham.NgayHen == null ? DateTime.Now : HenKham.NgayHen);
                    Order.delivery_to = BenhVien.TenBenhVien;
                    Order.cart_list = new List<ProductCreate>();
                    Order.cart_list.Add(new ProductCreate() {
                        product_code = HenKham.CRM_product_code,
                        quantity = 1,
                        unit_cost = 300,
                        note="Tạo từ bv"
                        });
                    var result = await cRMClientRep.order_create(Order);
                    if (result!="")
                    {
                        try
                        {
                            var Ma= result.Replace("new order:","").Trim();
                            HenKham.CRM_id_order = int.Parse(Ma);
                            await CapNhatMaHenKhamTuCRM(MaBenhVien, HenKham);
                            return Ma;
                        }
                        catch
                        {
                            throw new Exception("Mã trà về CRM không đúng " + result);
                        }
                    }
                    else
                    {
                        throw new Exception("Chuyển lên CRM không thành công!");
                    }
                }
                else
                {
                    throw new Exception("Không có thông tin bệnh viện");
                }                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return "OK";
        }

        public async Task TuDongDayHenKhamTheoToaToCRM(string MaBenhVien)
        {
            try
            {
                // Lấy danh sách cập nhật mau cập nhật;
                List<HKMauHenKhamTheoToa> DanhSachMauCapNhat = await GetDanhSachMauHenKhamTheoToa(MaBenhVien);
                var MauTuDong = DanhSachMauCapNhat.Where(x => x.TuDongChuyenCRM).ToList();
                var DanhSachChuaCapNhat = await GetDanhSachHenKham(MaBenhVien, DateTime.Now, DateTime.Now,false,false,true,true);
                foreach (var item in DanhSachChuaCapNhat)
                {
                    if (item.ID == null || item.CRM_id_order==null)
                    {
                        var found = MauTuDong.Find(x =>
                            (string.IsNullOrEmpty(x.LyDoHen) ? "" : x.LyDoHen.Trim()) == (string.IsNullOrEmpty(item.LyDoHen) ? "" : item.LyDoHen.Trim())
                            && (string.IsNullOrEmpty(x.LoiDan) ? "" : x.LoiDan.Trim()) == (string.IsNullOrEmpty(item.LoiDan) ? "" : item.LoiDan.Trim())
                            && (string.IsNullOrEmpty(x.BsKham) ? "" : x.BsKham.Trim()) == (string.IsNullOrEmpty(item.BsKham) ? "" : item.BsKham.Trim())
                            && (string.IsNullOrEmpty(x.ChanDoan) ? "" : x.ChanDoan) == (string.IsNullOrEmpty(item.ChanDoan) ? "" : item.ChanDoan)
                            && (string.IsNullOrEmpty(x.LoaiPT) ? "" : x.LoaiPT.Trim()) == (string.IsNullOrEmpty(item.LoaiPT) ? "" : item.LoaiPT.Trim())
                            && (string.IsNullOrEmpty(x.TenPT) ? "" : x.TenPT.Trim()) == (string.IsNullOrEmpty(item.TenPT) ? "" : item.TenPT.Trim())
                            ); ;
                        if (found != null)
                        {
                            if (item.ID == null)
                            {
                                item.CRM_product_code = found.CRM_product_code;
                                item.id_order_status = found.id_order_status;
    
                                var result = await AddHenKhamFromHis(MaBenhVien, item);

                                if (result != null)
                                {
                                    await PushHenKhamToCRM(MaBenhVien, (Guid)result);
                                }
                            }
                            else
                            {
                                if (item.CRM_id_order == null)
                                {
                                    await PushHenKhamToCRM(MaBenhVien, (Guid)item.ID);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public async Task TuDongUpdateTinhTrangHenKhamToCRM(string MaBenhVien)
        {
            try
            {
                // Lấy danh sách cập nhật mau cập nhật;
                List<HKMauCapNhatTinhTrang> DanhSachMauCapNhat = await GetDanhSachMauCapNhatTinhTrang(MaBenhVien);
                var MauTuDong = DanhSachMauCapNhat.Where(x => x.TuDongChuyenCRM).ToList();
                var DanhSachChuaCapNhat = await GetDanhSachCapNhatTinhTrang(MaBenhVien,DateTime.Now, DateTime.Now);
                foreach (var item in DanhSachChuaCapNhat)
                {
                    if (item.GhiChu == null && item.id_order_status != item.new_id_order_status)
                    {
                        var found = MauTuDong.Find(x =>
                            (string.IsNullOrEmpty(x.ChanDoan) ?"" : x.ChanDoan.Trim()) == (string.IsNullOrEmpty(item.ChanDoan) ? "" : item.ChanDoan.Trim())
                            && (string.IsNullOrEmpty(x.LoiDan) ? "" : x.LoiDan.Trim()) == (string.IsNullOrEmpty(item.LoiDan) ? "" : item.LoiDan.Trim())
                            && (string.IsNullOrEmpty(x.Kham) ? "" : x.Kham.Trim()) == (string.IsNullOrEmpty(item.Kham) ? "" : item.Kham?.Trim())
                            && (string.IsNullOrEmpty(x.BsKham) ? "" : x.BsKham.Trim()) == (string.IsNullOrEmpty(item.BsKham) ? "" : item.BsKham.Trim())
                            && x.id_order_status == item.id_order_status
                            && (string.IsNullOrEmpty(x.LyDoHen) ? "" : x.LyDoHen.Trim()) == (string.IsNullOrEmpty(item.LyDoHen) ? "" : item.LyDoHen.Trim())
                            && (string.IsNullOrEmpty(x.TenPT) ? "" : x.TenPT.Trim()) == (string.IsNullOrEmpty(item.TenPT) ? "" : item.TenPT.Trim())
                            );
                        if (found != null)
                        {

                            OrderUpdate orderUpdate = new OrderUpdate()
                            {
                                id_order = item.CRM_id_order,
                                order_status = found.new_id_order_status,
                                note = "Cập nhật tự động *"
                            };

                            if (orderUpdate.order_status == 3)
                            {
                                orderUpdate.appointment_date_1 = (item.NgayHenPTHoacTaiKham == null ? DateTime.Now.ToString("yyyy-MM-dd") : item.NgayHenPTHoacTaiKham?.ToString("yyyy-MM-dd"));
                            }
                            if (orderUpdate.order_status == 6)
                            {
                                orderUpdate.appointment_date_2 = (item.NgayHenPTHoacTaiKham == null ? DateTime.Now.ToString("yyyy-MM-dd") : item.NgayHenPTHoacTaiKham?.ToString("yyyy-MM-dd"));
                            }
                            var result = await cRMClientRep.order_update(orderUpdate);
                            if (result)
                            {
                                try
                                {
                                    await CapNhatTinhTrangHenKhamTuCRM(MaBenhVien, orderUpdate, "Tự động cập nhật",item.MaBenhAn);
                                }
                                catch
                                {

                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }            

        }


        public async Task<string> UpdateTinhTrangHenKhamToCRM(string MaBenhVien, HKLayDanhSachCapNhatTinhTrang hKLayDanhSachCapNhatTinhTrang,string GhiChu="User chuyển")
        {
            try
            {
                OrderUpdate orderUpdate = new OrderUpdate()
                {
                    id_order = hKLayDanhSachCapNhatTinhTrang.CRM_id_order,
                    order_status = hKLayDanhSachCapNhatTinhTrang.new_id_order_status,
                    note ="Cập nhật tự động"
                };

                if (orderUpdate.order_status==3)
                {
                    orderUpdate.appointment_date_1 = (hKLayDanhSachCapNhatTinhTrang.NgayHenPTHoacTaiKham == null ? DateTime.Now.ToString("yyyy-MM-dd") : hKLayDanhSachCapNhatTinhTrang.NgayHenPTHoacTaiKham?.ToString("yyyy-MM-dd"));
                }
                if (orderUpdate.order_status == 6)
                {
                    orderUpdate.appointment_date_2 = (hKLayDanhSachCapNhatTinhTrang.NgayHenPTHoacTaiKham == null ? DateTime.Now.ToString("yyyy-MM-dd") : hKLayDanhSachCapNhatTinhTrang.NgayHenPTHoacTaiKham?.ToString("yyyy-MM-dd"));
                }
                var result = await cRMClientRep.order_update(orderUpdate);
                if (result)
                {
                    try
                    {                        
                        await CapNhatTinhTrangHenKhamTuCRM(MaBenhVien, orderUpdate, GhiChu, hKLayDanhSachCapNhatTinhTrang.MaBenhAn);                        
                        return "OK";
                    }
                    catch
                    {
                        throw new Exception("Mã trà về CRM không đúng " + result);
                    }
                }
                else
                {
                    throw new Exception("Chuyển lên CRM không thành công!");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return "OK";

        }

        private async Task CapNhatMaHenKhamTuCRM(string maBenhVien, BenhNhanHenKham henKham)
        {
            var preSql = await GetpreSql(maBenhVien);
            var sql = "exec " + preSql + "proc_HenKhamSaveMaHenKhamCRM '" + henKham.ID + "','" + henKham.CRM_id_order +"'";
            await sqlDataAccess.execNonQuery(sql); 
        }
        private async Task CapNhatTinhTrangHenKhamTuCRM(string maBenhVien, OrderUpdate orderUpdate, string GhiChu="User cập nhật", string MaBenhAn="")
        {
            var preSql = await GetpreSql(maBenhVien);
            var sql = "exec " + preSql + "proc_HenKhamSaveTinhTrangHenKhamCRM '" + orderUpdate.id_order + "','" + orderUpdate.order_status + "', N'"+GhiChu+"',N'"+ MaBenhAn + "'";
            await sqlDataAccess.execNonQuery(sql);
        }
        public async Task<List<Product>> GetDanhSachCRMProduct()
        {
            return  await context.CRM_Products.ToListAsync();
        }

        public async Task<List<Order_status>> GetDanhSachCRMOrder_status()
        {
            return await context.CRM_Order_status.ToListAsync();
        }
        public async Task<List<HKBenhChuyenKhoa>> GetDanhSachBenhChuyenKhoa(string MaBenhVien)
        {
            try
            {
                var preSql = await GetpreSql(MaBenhVien);
                var sql = "exec " + preSql + "proc_HenKhamLayDanhSachBenhChuyenKhoa";

                var result = await sqlDataAccess.loadData<HKBenhChuyenKhoa, dynamic>(sql, new { });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<HKLayDanhSachCapNhatTinhTrang>> GetDanhSachCapNhatTinhTrang(string MaBenhVien,DateTime TuNgay, DateTime DenNgay)
        {
            try
            {
                var preSql = await GetpreSql(MaBenhVien);
                var sql = "exec " + preSql + "proc_HenKhamLayDanhSachCapNhat '" + TuNgay.ToString("yyyy-MM-dd") + "','" + DenNgay.ToString("yyyy-MM-dd 23:59:59") + "'";

                var result = await sqlDataAccess.loadData<HKLayDanhSachCapNhatTinhTrang, dynamic>(sql, new { });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        #region MauCapNhatTinhTrang
        public async Task<List<HKMauCapNhatTinhTrang>> GetDanhSachMauCapNhatTinhTrang(string MaBenhVien)
        {
            try
            {
                var preSql = await GetpreSql(MaBenhVien);
                var sql = "exec " + preSql + "proc_HenKhamLayDanhSachMauCapNhatTinhTrang";

                var result = await sqlDataAccess.loadData<HKMauCapNhatTinhTrang, dynamic>(sql, new { });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<HKMauCapNhatTinhTrang> SaveMauCapNhatTinhTrang(string MaBenhVien, HKMauCapNhatTinhTrang mau)
        {
            try
            {
                if (mau.ID == Guid.Empty)
                {
                    mau.ID = Guid.NewGuid();
                }
                var preSql = await GetpreSql(MaBenhVien);
                var sql = "exec " + preSql + @"proc_HenKhamSaveMauCapNhatTinhTrang 
                            N'" + mau.BsKham?.Trim() + @"'
                            ,N'" + mau.LyDoHen?.Trim() + @"'
                            ,N'" + mau.LoiDan?.Trim() + @"'
                            ,N'" + mau.ChanDoan?.Trim() + @"'
                            ,N'" + mau.TenPT?.Trim() + @"'
                            ,N'" + mau.Kham?.Trim() + @"'
                            ," + (mau.id_order_status == null ? "null" : mau.id_order_status) + @"
                            ," + (mau.new_id_order_status == null ? "null" : mau.new_id_order_status) + @"
                            ," + (mau.ThuTuUuTien) + @"
                            ,'"+ mau.ID+ @"'
                            ," + mau.TuDongChuyenCRM ;
                        await sqlDataAccess.execNonQuery(sql);
                return mau;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteMauCapNhatTinhTrang(string MaBenhVien, Guid mauID)
        {
            try
            {
              
                var preSql = await GetpreSql(MaBenhVien);
                var sql = "exec " + preSql + @"proc_HenKhamDeleteMauCapNhatTinhTrang 
                            '" + mauID + @"'";
                await sqlDataAccess.execNonQuery(sql);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion //MauCapNhatTinhTrang

        #region MauHenKhamTheoToa
        public async Task<List<HKMauHenKhamTheoToa>> GetDanhSachMauHenKhamTheoToa(string MaBenhVien)
        {
            try
            {
                var preSql = await GetpreSql(MaBenhVien);
                var sql = "exec " + preSql + "proc_HenKhamLayDanhSachMauHenKhamTheoToa";

                var result = await sqlDataAccess.loadData<HKMauHenKhamTheoToa, dynamic>(sql, new { });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<HKMauHenKhamTheoToa> SaveMauHenKhamTheoToa(string MaBenhVien, HKMauHenKhamTheoToa mau)
        {
            try
            {
                
                if (mau.ID == Guid.Empty)
                {
                    mau.ID = Guid.NewGuid();
                }
                var preSql = await GetpreSql(MaBenhVien);                

                var sql = "exec " + preSql + @"proc_HenKhamSaveMauHenKhamTheoToa 
                            '" + mau.ID + @"'
                            ," + mau.ThuTuUuTien + @"
                            ,N'" + mau.LyDoHen?.Trim() + @"'
                            ,N'" + mau.LoiDan?.Trim() + @"'
                            ,N'" + mau.BsKham?.Trim() + @"'
                            ,N'" + mau.ChanDoan?.Trim() + @"'
                            ,N'" + mau.LoaiPT?.Trim() + @"'
                            ,N'" + mau.TenPT + @"'
                            ," + mau.CoKhamHoSo + @"
                            ,N'" + mau.CRM_product_code + @"'
                            ,'" + mau.id_order_status + @"'
                            ," + mau.TuDongChuyenCRM;
                await sqlDataAccess.execNonQuery(sql);
                return mau;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteMauHenKhamTheoToa(string MaBenhVien, Guid mauID)
        {
            try
            {

                var preSql = await GetpreSql(MaBenhVien);
                var sql = "exec " + preSql + @"proc_HenKhamDeleteMauHenKhamTheoToa 
                            '" + mauID + @"'";
                await sqlDataAccess.execNonQuery(sql);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion //MauCapNhatTinhTrang

        #region Bệnh đến khám
        public async Task<List<HKBenhDenKham>> GetDanhSachBenhDenKham(string MaBenhVien)
        {
            try
            {
                var preSql = await GetpreSql(MaBenhVien);
                var sql = "exec " + preSql + "proc_HenKhamLayBenhNhanDenKham";

                var result = await sqlDataAccess.loadData<HKBenhDenKham, dynamic>(sql, new { });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task SaveBenhNhanDenKham(string MaBenhVien , Guid id)
        {
            try
            {
                var preSql = await GetpreSql(MaBenhVien);
                var sql = "exec " + preSql + "proc_HenKhamSaveBenhNhanDenKham '" + id + "'";
                await sqlDataAccess.execNonQuery(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> PushBenhDenKhamToCRM(string MaBenhVien)
        {
            try
            {
                var DanhSachDenKham = await GetDanhSachBenhDenKham(MaBenhVien);
                await PushBenhNhanDenKham(MaBenhVien, DanhSachDenKham);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task PushBenhNhanDenKham(string MaBenhVien , List<HKBenhDenKham> DanhSachDenKham)
        {
            foreach (var item in DanhSachDenKham)
            {
                try
                {
                    var re = await cRMClientRep.GetorderByCRM_id_order(item.CRM_id_order);
                    if (re != null)
                    {
                        if (re.order_status == 1)
                        {
                            var ketQua = await cRMClientRep.order_update(new OrderUpdate()
                            {
                                id_order = item.CRM_id_order,
                                order_status = 2,
                                note = "Cập nhật tự động bệnh đến khám"
                            }
                                );
                            if (ketQua)
                            {
                                await SaveBenhNhanDenKham(MaBenhVien, item.ID);
                            }

                        }
                        else
                        {
                            await SaveBenhNhanDenKham(MaBenhVien, item.ID);
                        }
                    }
                }
                catch (Exception ex)
                {                    
                }
            }
        }

        #endregion // Bệnh đến khám

    }
}
