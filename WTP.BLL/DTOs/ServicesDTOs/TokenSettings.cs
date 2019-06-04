using System;

namespace WTP.BLL.DTOs.ServicesDTOs
{
    public class TokenSettings
    {
        public TimeSpan ExpireTime { get; set; }
        public string Secret { get; set; }
        public string Site { get; set; }
        public string Audience { get; set; }
    }
}
