using Microsoft.EntityFrameworkCore;
using SalesAuto.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAuto.Models.ViewModel
{
    
    [Keyless]
    public class LeadVM : Lead
    {
        public DateTime NgayKham { get; set; }
        public string BenhVienKham { get; set; }
        public DateTime NgayPhauThuat { get; set; }
        public string BenhVienPhauThuat { get; set; }
    }
}
