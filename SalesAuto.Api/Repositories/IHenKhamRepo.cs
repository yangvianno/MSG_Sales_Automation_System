using SalesAuto.Models.Entities.CRM;
using SalesAuto.Models.Entities.HenKham;
using SalesAuto.Models.ViewModel.HenKham;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SalesAuto.Api.Repositories
{
    public interface IHenKhamRepo
    {
        Task<Guid?> AddHenKhamFromHis(string MaBenhVien, BenhNhanHenKham benhHenKham);
        Task<bool> DeleteMauCapNhatTinhTrang(string MaBenhVien, Guid mauID);
        Task<bool> DeleteMauHenKhamTheoToa(string MaBenhVien, Guid mauID);
        Task<List<HKBenhChuyenKhoa>> GetDanhSachBenhChuyenKhoa(string MaBenhVien);
        Task<List<HKLayDanhSachCapNhatTinhTrang>> GetDanhSachCapNhatTinhTrang(string MaBenhVien, DateTime TuNgay, DateTime DenNgay);
        Task<List<Order_status>> GetDanhSachCRMOrder_status();
        Task<List<Product>> GetDanhSachCRMProduct();
        Task<List<BenhNhanHenKham>> GetDanhSachHenKham(string MaBenhVien, DateTime TuNgay, DateTime DenNgay, bool LayTheoNgayHen = true, bool BacSyHen = true, bool HoSoLasik = true, bool BenhChuaHen = true);
        Task<List<HKMauCapNhatTinhTrang>> GetDanhSachMauCapNhatTinhTrang(string MaBenhVien);
        Task<List<HKMauHenKhamTheoToa>> GetDanhSachMauHenKhamTheoToa(string MaBenhVien);
        Task<BenhNhanHenKham> GetHenKham(string MaBenhVien, Guid ID);
        Task<List<BenhNhanHenKham>> GetHenKhamThucHienCuoi(string MaBenhVien);
        Task<bool>  PushBenhDenKhamToCRM(string MaBenhVien);
        Task PushBenhNhanDenKham(string MaBenhVien, List<HKBenhDenKham> DanhSachDenKham);
        Task<string> PushHenKhamToCRM(string MaBenhVien, Guid ID);
        Task<HKMauCapNhatTinhTrang> SaveMauCapNhatTinhTrang(string MaBenhVien, HKMauCapNhatTinhTrang mau);
        Task<HKMauHenKhamTheoToa> SaveMauHenKhamTheoToa(string MaBenhVien, HKMauHenKhamTheoToa mau);
        Task TuDongDayHenKhamTheoToaToCRM(string MaBenhVien);
        Task TuDongUpdateTinhTrangHenKhamToCRM(string MaBenhVien);
        Task<string> UpdateTinhTrangHenKhamToCRM(string MaBenhVien, HKLayDanhSachCapNhatTinhTrang hKLayDanhSachCapNhatTinhTrang, string GhiChu = "User chuyển");
    }
}