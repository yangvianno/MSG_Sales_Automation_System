using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAuto.Models.Entities
{
    public class ABRUserXacNhanNoiLamViec
    {
        public Guid ID { get; set; }
        public Guid IDUser { get; set; }
        public int  MaNoiLamViec { get; set; }
    }
}
