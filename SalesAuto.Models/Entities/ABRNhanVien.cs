using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAuto.Models.Entities
{
    public class ABRNhanVien
    {
        [Key]
        public Guid ID { get; set; }
        public string MaNhanVien { get; set; }
        public string TenNhanVien { get; set; }
        public string ChucDanh { get; set; }
        public bool TinhTrucTiep { get; set; }
        public bool HuongTrucTiep { get; set; }
        public bool HuongGianTiep { get; set; }
        public Guid? ThuocPool { get; set; }
        public List<AbrPool> PoolDuocHuong { get; set; }
        public string? PhongBan { get; set; }

        public List<ABRDanhMuc> ABRLuonDuocHuong { get; set; }
        public decimal HeSoGianTiep { get; set; }
        public string? MaBenhVien { get; set; }
        public string? CacPoolHuong { get; set; }
    }
}
