using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAuto.Models.Entities.CRM
{
    public class LoginSuccess
    {
        public int id_user { get; set; }
        public string username { get; set; }
        public string access_token { get; set; }
        public string email { get; set; }
    }
}
