using System;

namespace WTP.BLL.DTOs.ServicesDTOs
{
    public class AccessDto
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public string Refresh_token { get; set; }
        public string Role { get; set; }
    }
}
