using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAuto.Models.Entities.CRM
{
    public class Type
    {
        [Key]
        public int id_type { get; set; }
        public string type_name { get; set; }
    }
}
