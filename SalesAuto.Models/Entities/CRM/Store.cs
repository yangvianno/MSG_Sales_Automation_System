using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAuto.Models.Entities.CRM
{
    public class Store
    {
        [Key,  DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        public int id_store { get; set; }
        public string store_code { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string address { get; set; }

    }
}
