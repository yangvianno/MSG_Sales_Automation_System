using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAuto.Models.ViewModel.HenKham
{
    public class HKLayDanhSachCapNhatTinhTrang
    {
        public int CRM_id_order { get; set; }
        public string HoTenBN { get; set; }
        public string DiaChi { get; set; }
        public string DienThoai { get; set; }
        public DateTime? NgayHenKham { get; set;}        
        public DateTime? NgayKham { get; set; }
        public string? BsKham { get; set; }
        public string? ChanDoan  { get; set; }
        public DateTime? NgayPT { get; set; }
        public string TenPT { get; set; }
        public string Kham { get; set; }
        public int? id_order_status { get; set; }
        public int? new_id_order_status { get; set; }
        public DateTime? NgayHenPTHoacTaiKham { get; set; }
        public string? LyDoHen { get; set; }
        public string? LoiDan { get; set; }
        public string? GhiChu { get; set; }
        public string? MaBenhAn { get; set; }

    }
}
