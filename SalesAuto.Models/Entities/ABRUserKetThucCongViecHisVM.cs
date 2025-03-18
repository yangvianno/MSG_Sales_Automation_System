using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAuto.Models.Entities
{
    public class ABRUserKetThucCongViecHisVM: ABRUserKetThucCongViecHis
    {
        public string? TenCV { get; set; }
        public string? NhomCongViecThongKe { get; set; }
        public string? Khoa { get; set; }
        public string? NhomPT { get; set; }
        public string? UserName { get; set; }
    }
}
