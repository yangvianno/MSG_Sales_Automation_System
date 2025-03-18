using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAuto.Models.Entities.CRM
{
    public class Product
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        public int id_product { get; set; }
        public string name { get; set; }
        public string product_code { get; set; }
        public string product_price { get; set; }
        public string begin_date { get; set; }
        public string end_date { get; set; }
        public Category category_info { get; set; }
        public Type type_info { get; set; }
    }
}
