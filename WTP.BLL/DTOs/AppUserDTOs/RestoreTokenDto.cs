using System;

namespace WTP.BLL.DTOs.AppUserDTOs
{
    public class RestoreTokenDto
    {
        public int Id { get; set; }
        public int AppUserId { get; set; }
        public string Value { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
