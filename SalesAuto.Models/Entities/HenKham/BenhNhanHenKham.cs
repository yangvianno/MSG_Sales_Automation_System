using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAuto.Models.Entities.HenKham
{
    public class BenhNhanHenKham
    {
        public Guid? ID { get; set; }
        public string MaBenhNhan { get; set; }
        public string MaBenhAn { get; set; }
        public string HoTen { get; set; }
        public string DiaChi { get; set; }
        public string DienThoai { get; set; }
        public string LyDoHen { get; set; }
        public string LoiDan { get; set; }
        public DateTime NgayKham { get; set; }
        public string BsKham{ get; set; }
        public string? ChanDoan { get; set; }
        public DateTime? NgayHen { get; set; }
        public DateTime? NgayKhamCuoi { get; set; }
        public string LoaiPT{ get; set; }
        public string TenPT{ get; set; }
        public DateTime? NgayPT { get; set; }
        public int STT { get; set; }
        public int? CRM_id_order { get; set; }
        public int Tuoi { get; set; }
        public string GioiTinh { get; set; }
        public string? CRM_product_code { get; set; }
        public int? id_order_status { get; set; }
    }
}
