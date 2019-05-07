using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WTP.WebAPI.ViewModels
{
    public class AccessResponseModel
    {
        public string Token { get; set; } //JWT token
        public DateTime Expiration { get; set; } //expiry time
        public string Refresh_token { get; set; } //refresh token
        public string Role { get; set; } //user role
        public string UserName { get; set; } //user name
        public string Photo { get; set; } //user's avatar
    }
}
