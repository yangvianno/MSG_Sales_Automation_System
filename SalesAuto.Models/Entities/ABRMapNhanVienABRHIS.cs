using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAuto.Models.Entities
{
    public class ABRMapNhanVienABRHIS
    {
        [Key]
        public Guid ID { get; set; }
        public Guid IDNhanVienABR { get; set; }
        public int MaNhanVienHIS { get; set; }
    }
}
