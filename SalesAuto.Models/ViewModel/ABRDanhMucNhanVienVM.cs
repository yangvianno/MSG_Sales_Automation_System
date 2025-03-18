using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAuto.Models.ViewModel
{
    public class ABRDanhMucNhanVienVM
    {
        public Guid? ID { get; set; }
        public int IDABRDanhMuc { get; set; }
		public Guid	IDNhanVien { get; set; }
        public string TenCongViec { get; set; }
        public string GhiChu { get; set; }
        public int MucHuongVND { get; set; }
        public decimal MucHuongPhanTram { get; set; }        
        public decimal TyLeGianTiep { get; set; }
        public bool TinhTheoPoolThucHien { get; set; }
        public decimal HuongToiDa { get; set; }
        public string? CacPoolHuong { get; set; }

    }
}
