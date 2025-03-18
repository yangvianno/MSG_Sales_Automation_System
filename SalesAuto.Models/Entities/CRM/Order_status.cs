using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAuto.Models.Entities.CRM
{
    public class Order_status
    {        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int order_status { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }
}
