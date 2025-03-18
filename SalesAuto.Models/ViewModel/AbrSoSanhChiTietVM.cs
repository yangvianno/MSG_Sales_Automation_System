using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAuto.Models.ViewModel
{
    public class AbrSoSanhChiTietVM
    {
        public string MaNhanVien { get; set; }
        public string TenNhanVien { get; set; }
        public string  ChucDanh { get; set; }
        public decimal ABRThangNay { get; set; }
        public decimal ABRThangTruoc { get; set; }
        public decimal TangGiam { get; set; }
        public decimal DieuChinh { get; set; }
        public decimal SauDieuChinh { get; set; }

    }
}
