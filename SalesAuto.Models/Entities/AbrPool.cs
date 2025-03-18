using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAuto.Models.Entities
{
    public class AbrPool
    {
        [Key]
        public Guid ID { get; set; }
        public string TenPool { get; set; }
    }
}
