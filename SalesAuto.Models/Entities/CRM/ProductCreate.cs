using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAuto.Models.Entities.CRM
{
    public class ProductCreate
    {       
        public string product_code { get; set; }
        public int quantity { get; set; }
        public float unit_cost { get; set; }
        public string? note { get; set; }
    }
}
