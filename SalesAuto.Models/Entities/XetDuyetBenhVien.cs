using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAuto.Models.Entities
{
    public class XetDuyetBenhVien
    {
        public string MaBenhVien { get; set; }
        public Guid IDXetDuyet { get; set; }
        public TinhTrangXetDuyet TinhTrangXetDuyet { get; set; }
        public TrangThaiLuu TrangThaiLuu { get; set; }
    }

    public enum TrangThaiLuu
    {
        ChuaLuu,
        DaLuu
    }
}
