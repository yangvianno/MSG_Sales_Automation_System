using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAuto.Models.Entities.HenKham
{
    public class HKBenhDenKham
    {
        [Key]
        public Guid ID { get; set; }
        public string MaBenhAn { get; set; }
        public int CRM_id_order { get; set; }
        public DateTime GioTao { get; set; }
        public bool? DaCapNhat { get; set; }
        public DateTime? GioCapNhat { get; set; }
    }
}
