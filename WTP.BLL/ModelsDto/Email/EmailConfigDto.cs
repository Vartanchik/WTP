using System;
using System.Collections.Generic;
using System.Text;

namespace WTP.BLL.ModelsDto.Email
{
    public class EmailConfigDto
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string Host { get; set; }

        public string Port { get; set; }
    }
}
