using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAuto.Models.Entities.CRM
{
    public class ProductOrderResult
    {
        [Key]
        public string? unit_name { get; set; }
        public string? unit_code { get; set; }
        public int unit_quantity { get; set; }
        public int? unit_price { get; set; }
        public int? unit_cost { get; set; }
        public string? unit_note { get; set; }

    }
}
