using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAuto.Models.Entities
{
    public class MailSettings
    {
        public string Mail { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string SendTo { get; set; } = "thanh.tran@matsaigon.com";
        public string CCTo { get; set; } = "thanh.tran@matsaigon.com";
    }
}
