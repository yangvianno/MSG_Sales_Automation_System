using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAuto.Models.Entities
{
    public class ABRNhanVienThucHien
    {
        public string ID_DSCV { get; set; }
        public int IDMapDanhMucABRHIS { get; set; }
        public int IDABRDanhMuc { get; set; }
        public Guid IDABRNhanVien { get; set; }
        public decimal DoanhThuTinhABR { get; set; }
        public decimal SoLuong { get; set; } = 1;

    }
}
