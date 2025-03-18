using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAuto.Models.ViewModel
{
    public class ABRMapNhanVienABRHISVM
    {
        public Guid ID { get; set; }
        public int MaNhanVienHIS { get; set; }
        public string ChucDanhHIS { get; set; }
        public string TenNhanVienHIS { get; set; }
        public Guid IDNhanVienABR { get; set; }
        public string MaNhanVienABR { get; set; }
        public string TenNhanVienABR { get; set; }
        public string ChucDanhABR { get; set; }
    }
}
