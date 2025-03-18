using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAuto.Models
{
    public class LoginResponse
    {
        public bool Successful { get; set; }
        public string Error { get; set; }
        public DateTime Expiry { get; set; }
        public Guid UserId { get; set; }
        public string Token { get; set; }
    }
}
