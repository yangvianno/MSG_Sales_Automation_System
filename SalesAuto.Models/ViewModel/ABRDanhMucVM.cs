using SalesAuto.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAuto.Models.ViewModel
{
    public class ABRDanhMucVM: ABRDanhMuc
    {
        public TinhTrangXetDuyet? TinhTrang { get; set; }
        public string TenPoolDuocHuong { get; set; }
        public string BacThangDuocHuong { get; set; }
    }
}
