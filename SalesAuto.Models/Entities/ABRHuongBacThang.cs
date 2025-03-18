using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAuto.Models.Entities
{
    public class ABRHuongBacThang
    {
        [Key]
        public Guid ID { get; set; }
        public int IDABRDanhMuc { get; set; }
        public decimal CanDuoi { get; set; }
        public decimal CanTren { get; set; }
        public bool TinhToanVien { get; set; }
        public decimal HuongVND { get; set; }
        public Decimal HuongPhanTram { get; set; }
    }
}
