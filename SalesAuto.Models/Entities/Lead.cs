using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SalesAuto.Models.Entities
{
    public class Lead
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set;}
        public int STT { get; set; }
        public string TenKhachHang { get; set; }
        public string SoPhu { get; set; }
        public string Phone { get; set; }
        public DateTime Ngay { get; set; }
        public string Nguon { get; set; }
        public string TinhThanh { get; set; }
        public string file { get; set; }
        public DateTime NgayImport { get; set; }
        public string MaBenhVien { get; set; }

    }
}
