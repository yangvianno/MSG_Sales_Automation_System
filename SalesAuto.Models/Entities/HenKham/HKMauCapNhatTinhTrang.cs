using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAuto.Models.Entities.HenKham
{
    public class HKMauCapNhatTinhTrang
    {
        [Key]
        [Required]
        public Guid ID { get; set; }
        public int ThuTuUuTien { get; set; }
        public string? BsKham { get; set; }
        public string? LyDoHen { get; set; }
        public string? LoiDan { get; set; }        
        public string? ChanDoan { get; set; }        
        public string TenPT { get; set; }
        public string Kham { get; set; }
        public int? id_order_status { get; set; }
        public int? new_id_order_status { get; set; }
        public bool TuDongChuyenCRM { get; set; }

    }
}
