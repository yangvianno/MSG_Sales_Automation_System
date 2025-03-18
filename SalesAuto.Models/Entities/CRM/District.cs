using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAuto.Models.Entities.CRM
{
    public class District
    {
        [Key]
        public int id_district { get; set; }
        public string name { get; set; }
        public int id_parent { get; set; }
    }
}
