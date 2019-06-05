using System;

namespace WTP.BLL.DTOs.AppUserDTOs
{
    public class RefreshTokenDto
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UserId { get; set; }
        public DateTime ExpiryTime { get; set; }
    }
}
