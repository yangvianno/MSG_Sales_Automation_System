using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAuto.Models.Entities.CRM
{
    public class MetaOrder
    {
        public int? total { get; set; }
        public int page { get; set; }
        public int totalPage { get; set; }
        public int? pageSize { get; set; }
        public int? from { get; set; }
        public int? to { get; set; }
        public string? next { get; set; }
        public string? prev { get; set; }
    }
}
