using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AaravEnterprise.Models
{
    public class SmtpSettings
    {
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
