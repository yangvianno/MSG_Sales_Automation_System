using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAuto.Models.Entities
{
    public class LoaiChiTieu
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaLoaiChiTieu { get; set; }
        public string TenChiTieu { get; set; }
        public string NhomChiTiet { get; set; }
        public string MoTa { get; set; }
    }
}
