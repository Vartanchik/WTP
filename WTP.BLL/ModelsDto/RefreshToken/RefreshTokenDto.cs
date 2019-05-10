using System;
using WTP.BLL.ModelsDto.AppUser;

namespace WTP.BLL.ModelsDto.RefreshToken
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
