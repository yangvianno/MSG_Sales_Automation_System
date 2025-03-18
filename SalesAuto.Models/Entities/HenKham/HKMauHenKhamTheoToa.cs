using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAuto.Models.Entities.HenKham
{
    public class HKMauHenKhamTheoToa
    {
        [Key]
        [Required]
        public Guid ID { get; set; }
        public int ThuTuUuTien { get; set; }
        public string LyDoHen { get; set; }
        public string LoiDan { get; set; }        
        public string BsKham { get; set; }
        public string? ChanDoan { get; set; }
        public string? LoaiPT { get; set; }
        public string? TenPT { get; set; }        
        public bool CoKhamHoSo { get; set; }
        public string? CRM_product_code { get; set; }
        public int? id_order_status { get; set; }
        public bool TuDongChuyenCRM { get; set; }
    }
}
