using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAuto.Models.Entities
{
    public class ABRUserKetThucCongViecHis
    {
        public Guid ID { get; set; }
        public Guid? IDUser { get; set; }
        public string? MaCV { get; set; }
    }
}
