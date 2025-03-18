using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAuto.Models.Entities.CRM
{
    public class Province
    {
        [Key]
        public int id_province { get; set; }
        public string name { get; set; }
    }
}
