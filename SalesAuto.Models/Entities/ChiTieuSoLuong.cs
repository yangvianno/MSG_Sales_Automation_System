using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAuto.Models.Entities
{
    public class ChiTieuSoLuong
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string MaBenhVien { get; set; }
        public int Thang { get; set; }
        public int Nam { get; set; }
        public int MaLoaiChiTieu { get; set; }
        public Int64 SoLuong { get; set; }
        public string GhiChu { get; set; }

    }
}
