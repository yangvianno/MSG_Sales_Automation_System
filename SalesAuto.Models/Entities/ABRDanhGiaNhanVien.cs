using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAuto.Models.Entities
{
    public class ABRDanhGiaNhanVien
    {
        public int Thang { get; set; }
        public int Nam { get; set; }
        public int SoThuTu { get; set; }
        public string MaNhanVien { get; set; }
        public string? HoVaTen { get; set; }
        public string? BenhVien { get; set; }
        public string? PhongBan { get; set; }
        public string? ChucDanh { get; set; }
        public string? LoaiDoiTuong { get; set; }
        public int MucTinhABRTrongThang { get; set; }        
        public string? GhiChu { get; set; }
        public string? LuuY { get; set; }
    }
}
