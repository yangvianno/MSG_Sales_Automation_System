using SalesAuto.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAuto.Models.SearchModel
{
    public class NhanVienThucHienSM
    {
        public DateTime TuNgay { get; set; }
        public DateTime DenNgay { get; set; }
        public string? NhomCongViecThongKe { get; set; }
        public ABRLoaiTinhTrangTimKiem? TinhTrang { get; set; }
        public ABRLoaiBaoCaoTongHop? LoaiBaoTongHop { get; set; }
        public bool? NhanVienThucHienKhacHis { get; set; }
        public int Trang { get; set; } = 0;
        public int NumRecords { get; set; } = 0;
    }
}
