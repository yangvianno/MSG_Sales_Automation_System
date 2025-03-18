using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAuto.Models.ViewModel
{
    public class ABRXacNhanNhanVienThucHienHisVM
    {
        public Guid ID { get; set; }
        public string? ID_DSCV { get; set; }
        public string? TenBenhNhan { get; set; }
        public string? TenCongViec { get; set; }
        public DateTime Ngay { get; set; }
        public int IDDanhMucABR { get; set; }
        public string TenDanhMucABR { get; set; }
        public int? MaNhanVien { get; set; }
        public string? TenNhanVien { get; set; }
        public int? MaNhanVienHis { get; set; }
        public string? TenNhanVienHis { get; set; }
        public Guid? UserLuu { get; set; }
        public string? NoiChiDinh { get; set; }
        public string? NoiThucHien { get; set; }


    }
}
