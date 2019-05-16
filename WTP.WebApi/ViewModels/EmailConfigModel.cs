using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WTP.WebAPI.ViewModels
{
    public class EmailConfigModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string Host { get; set; }

        public string Port { get; set; }

        public EmailConfigModel(IConfiguration configuration)
        {
            Email = configuration["Email:Email"];

            Password = configuration["Email:Password"];

            Host = configuration["Email:Host"];

            Port = configuration["Email:Port"];
        }
    }
}
