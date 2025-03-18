using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAuto.Models.Entities.CRM
{
    public class OrderCreate
    {
        public string customer_name { get; set; }
        public string customer_phone { get; set; }
        public string? email { get; set; }
        public string address { get; set; }
        public int? id_province { get; set; }
        public int? id_district { get; set; }
        public int? id_ward { get; set; }
        public string store_code { get; set;}
        public int? order_status { get; set; }
        public float total_price {get; set;}
        public DateTime order_date { get; set; }
        public string delivery_to { get; set; }
        public List<ProductCreate> cart_list { get; set; }
    }
}
