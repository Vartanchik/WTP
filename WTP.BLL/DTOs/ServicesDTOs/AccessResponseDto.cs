using System;

namespace WTP.BLL.DTOs.ServicesDTOs
{
    public class AccessResponseDto
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public string Refresh_token { get; set; }
        public string Role { get; set; }
        public string UserName { get; set; }
        public string Photo { get; set; }
    }
}
