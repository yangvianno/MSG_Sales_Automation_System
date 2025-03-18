using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAuto.Models.Entities.CRM
{
    public class OrderUpdate
    {
        public int id_order { get; set; }
        public int? order_status { get; set; }
        public string? appointment_date_1 { get; set; }
        public string? appointment_date_2 { get; set; }
        public string note { get; set; }
    }
}
