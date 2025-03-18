using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAuto.Models.ViewModel
{
    public class ABRMapCongViecABRHISVM
    {
        public int ID { get; set; }
        public int IDDanhMucABR { get; set; }
        public string? MaCV { get; set; }
        public string? TenCongViec { get; set; }
        public string? NhomCongViecThongKe { get; set; }
        public string? KhoaPhauThuat { get; set; }
        public string? LoaiPhauThuat { get; set; }
        public string? GiaTien { get; set; }
        public decimal QuyRa { get; set; }
        public string? Code { get; set; }
        public string? NhomABR { get; set; }
        public string? TenCongViecABR { get; set; }
        public int MucHuongVND { get; set; }
        public decimal MucHuongPhanTram { get; set; }
        public int DoanhThuTinhABR { get; set; }
        public bool? TinhTheoDoanhThu { get; set; }
        public Guid? RowGuid { get; set; }

    }
}
