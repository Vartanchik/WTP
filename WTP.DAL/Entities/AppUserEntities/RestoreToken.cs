using System;

namespace WTP.DAL.Entities.AppUserEntities
{
    public class RestoreToken : IEntity
    {
        public int Id { get; set; }
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public string Value { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
