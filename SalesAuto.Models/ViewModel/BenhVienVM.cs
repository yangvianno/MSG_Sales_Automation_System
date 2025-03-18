using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAuto.Models.ViewModel
{
    [Keyless]
    public class BenhVienVM
    {
        public string MaBenhVien { get; set; }
        public string TenBenhVien { get; set; }
        public string PreSql { get; set; } = "";
        public string? CRM_store_code { get; set; }
    }
}
