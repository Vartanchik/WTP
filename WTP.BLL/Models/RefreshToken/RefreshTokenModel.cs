using System;
using WTP.BLL.Models.AppUser;

namespace WTP.BLL.Models.RefreshToken
{
    public class RefreshTokenModel
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UserId { get; set; }
        public AppUserModel AppUser { get; set; }
        public DateTime ExpiryTime { get; set; }
    }
}
