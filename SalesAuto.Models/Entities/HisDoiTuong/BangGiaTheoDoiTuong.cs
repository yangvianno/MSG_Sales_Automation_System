using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAuto.Models.Entities.HisDoiTuong
{
    public class BangGiaTheoDoiTuong
    {
        public Guid ID_LoaiDoiTuong { get; set; }
        public string MACV { get; set; }
        public string TENCV { get; set; }
        public double GIATIEN { get; set; }
        public double GIABHYT { get; set; }
        public double GIABHYT_PHAITRA { get; set; }


    }
}
