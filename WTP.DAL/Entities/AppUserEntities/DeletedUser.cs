using System;

namespace WTP.DAL.Entities.AppUserEntities
{
    public class DeletedUser : IEntity
    {
        public int Id { get; set; }
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public DateTime DeleteDate { get; set; }
    }
}
