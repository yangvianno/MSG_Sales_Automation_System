using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAuto.Models.ViewModel
{
    public class ChangePassVM
    {
        public Guid UserID { get; set; }
        public string MatKhauMoi { get; set; }
        public string MatKhauCu { get; set; }
        public string  NhapLaiMatKhauCu { get; set; }
    }
}
