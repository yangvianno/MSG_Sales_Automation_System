using SalesAuto.Models.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAuto.Models.SearchModel
{
    public class BenhNhanSM : PagingParameters
    {
        public string MaBenhVien { get; set; }
        public DateTime TuNgay { get; set; }
        public DateTime DenNgay { get; set; }
        public string Loai { get; set; }
        public string Nguon { get; set; }        
    }
}
