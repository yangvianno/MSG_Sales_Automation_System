using SalesAuto.Models.Entities.CRM;
using SalesAuto.Models.Entities.HenKham;
using SalesAuto.Models.ViewModel.HenKham;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SalesAuto.Wasm.Services
{
    public interface IHenKhamClient
    {
        Task<Guid?> AddHenKhamFromHis(BenhNhanHenKham benhHenKham);
        Task<bool> DeleteMauCapNhatTinhTrang(Guid id);
        Task<bool> DeleteMauHenKhamTheoToa(Guid id);
        Task<List<HKBenhChuyenKhoa>> GetDanhSachBenhChuyenKhoa();
        Task<List<HKLayDanhSachCapNhatTinhTrang>> GetDanhSachCapNhatTinhTrang(DateTime TuNgay, DateTime DenNgay);
        Task<List<Order_status>> GetDanhSachCRMOrder_status();
        Task<List<Product>> GetDanhSachCRMProduct();
        Task<List<BenhNhanHenKham>> GetDanhSachHenKham(DateTime TuNgay, DateTime DenNgay, bool LayTheoNgayHen = true, bool BacSyHen = true, bool HoSoLasik = true, bool BenhChuaHen = true);
        Task<List<HKMauCapNhatTinhTrang>> GetDanhSachMauCapNhatTinhTrang();
        Task<List<HKMauHenKhamTheoToa>> GetDanhSachMauHenKhamTheoToa();
        Task<BenhNhanHenKham> GetHenKham(Guid ID);
        Task<List<BenhNhanHenKham>> GetHenKhamThucHienCuoi();        
        Task<string> PushHenKhamToCRM(BenhNhanHenKham benhHenKham);
        Task<HKMauCapNhatTinhTrang> SaveMauCapNhatTinhTrang(HKMauCapNhatTinhTrang mau);
        Task<HKMauHenKhamTheoToa> SaveMauHenKhamTheoToa(HKMauHenKhamTheoToa mau);
        Task<string> UpdateTinhTrangHenKhamToCRM(HKLayDanhSachCapNhatTinhTrang hKLayDanhSachCapNhatTinhTrang);
    }
}