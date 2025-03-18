using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAuto.Models.Entities.CRM
{
    public class OrderResultAPI
    {
        public MetaOrder meta { get; set; }
        public List<OrderResult> data { get; set; }
        
    }
}
