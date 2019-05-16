using System;
using System.Collections.Generic;
using System.Text;

namespace WTP.BLL.Models.Email
{
    public class EmailConfigModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string Host { get; set; }

        public string Port { get; set; }
    }
}
