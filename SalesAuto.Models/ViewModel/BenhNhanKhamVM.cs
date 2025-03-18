using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAuto.Models.ViewModel
{ 
    [Keyless]
    public class BenhNhanKhamVM 
    {
        public string MaBenhVien { get; set; }
        public string TenBenhVien { get; set; }
        public DateTime Ngay { get; set; }
        public string TenDichVu { get; set; }
        public string LoaiDichVu { get; set; }
        public string LoaiPT { get; set; }
        public string TenLead { get; set; }
        public string TenBenhNhan { get; set; }
        public string NamSinh { get; set; }
        public string DienThoai { get; set; }
        public string  DiaChi { get; set; }
        public long GiamGia { get; set; }
        public long DoanhThu { get; set; }
        public string Nguon { get; set; }
        public string Loai { get; set; }
    }
}
