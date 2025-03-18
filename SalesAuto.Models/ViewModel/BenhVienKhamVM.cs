using Microsoft.EntityFrameworkCore;

namespace SalesAuto.Models.ViewModel
{
    [Keyless]
    public class BenhVienKhamVM
    {
        public string MaBenhVien { get; set; }
        public string TenBenhVien { get; set; }
        public string Loai { get; set; }
        public long SoLuong { get; set; }
        public long GiamGia { get; set; }
        public long DoanhThu { get; set; }
    }
}