using Microsoft.Extensions.Configuration;
using System;

namespace WTP.BLL.DTOs.ServicesDTOs
{
    public class TokenSettings
    {
        public TokenSettings(IConfiguration configuration)
        {
            ExpireTime = new TimeSpan(0, Convert.ToInt32(configuration["Token:ExpireTime"]), 0);
            Secret = configuration["Token:Key"];
        }

        public TimeSpan ExpireTime { get; set; }
        public string Secret { get; set; }
    }
}
