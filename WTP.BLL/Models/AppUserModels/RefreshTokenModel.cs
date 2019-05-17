using System;
using WTP.BLL.Models.AppUserModels;

namespace WTP.BLL.Models.AppUserModels
{
    public class RefreshTokenModel : IModel
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UserId { get; set; }
        public AppUserModel AppUser { get; set; }
        public DateTime ExpiryTime { get; set; }
    }
}
