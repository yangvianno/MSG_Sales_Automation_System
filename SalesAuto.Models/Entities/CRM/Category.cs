using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAuto.Models.Entities.CRM
{
    public class Category
    {
        [Key]
        public int id_category { get; set; }
        public string category_name { get; set; }
    }
}
