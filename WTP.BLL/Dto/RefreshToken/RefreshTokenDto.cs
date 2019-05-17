using System;
using WTP.BLL.Dto.AppUser;

namespace WTP.BLL.Dto.RefreshToken
{
    public class RefreshTokenDto
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UserId { get; set; }
        public AppUserDto AppUser { get; set; }
        public DateTime ExpiryTime { get; set; }
    }
}
