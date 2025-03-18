using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAuto.Models.Entities.CRM
{
    public class Location
    {
        public List<Province> provinces { get; set; }
        public List<District> districts { get; set; }
        public List<Ward> wards { get; set; }

    }
}
