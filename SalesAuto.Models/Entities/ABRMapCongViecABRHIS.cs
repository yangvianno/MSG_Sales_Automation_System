using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAuto.Models.Entities
{
    public class ABRMapCongViecABRHIS
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int IDDanhMucABR { get; set; }
        public string MaCV { get; set; }
        public decimal QuyRa { get; set; }
        public int DoanhThuTinhABR { get; set; }
        public int loaiMapHIS { get; set; }
        public bool? TinhTheoDoanhThu { get; set; }
        public Guid? RowGuid { get; set; }
    }
}
