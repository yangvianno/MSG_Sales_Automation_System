using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAuto.Models.Entities
{
    public class Appointment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int STT { get; set; }
        public string MaLichKham { get; set; }
        public string TenKhachHang { get; set; }
        public string DienThoai { get; set; }
        public string TrangThai { get; set; }
        public DateTime NgayTaoLich { get; set; }
        public DateTime NgayDatLichKham { get; set; }
        public string Loai { get; set; }
        public string DienThoaiVien { get; set; }
        public string TenChiNhanh { get; set; }
        public string MaBenhVien { get; set; }
        public int? id_category { get; set; }
        public int? id_product { get; set; }
        public string? store_code { get; set; }
        public string? DiaChi { get; set; }
        public int? id_order_status { get; set; }
        public string? CRM_productcode { get; set; }
        public int? id_user { get; set; }
    }
}
