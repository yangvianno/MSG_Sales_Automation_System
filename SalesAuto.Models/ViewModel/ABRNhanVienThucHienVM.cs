using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAuto.Models.ViewModel
{
    public class ABRNhanVienThucHienVM
    {
		public string ID_DSCV { get; set; }
		public DateTime NgayThu { get; set; }
		public string MaBenhAn { get; set; }
		public string HOTENBN { get; set; }
		public string? NhomCongViecThongKe { get; set; }
		public string TenCongViecHIS { get; set; }
		public string NhomABR { get; set; }
		public string TenCongViecABR { get; set; }
		public Guid? MaNhanVienHIS { get; set; }
		public string TenNhanVienHIS { get; set; }
		public int IDMapABRHIS { get; set; }
		public Guid? MaNhanVienABR { get; set; }
		public int? IDABRDanhMuc { get; set; }
		public string? TenNhanVienABR { get; set; }
		public decimal DoanhThuTinhABR { get; set; }
		public decimal SoLuong { get; set; } = 1;
	}
}
