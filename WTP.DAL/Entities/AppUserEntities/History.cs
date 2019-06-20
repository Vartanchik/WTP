using System;
using System.Collections.Generic;
using System.Text;

namespace WTP.DAL.Entities.AppUserEntities
{
    public class History : IEntity
    {
        public int Id { get; set; }
        public DateTime DateOfOperation { get; set; }
        public string Description { get; set; }
        public string PreviousUserEmail { get; set; }
        public string PreviousUserName { get; set; }
        public string NewUserEmail { get; set; }
        public string NewUserName { get; set; }
        public int? AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public int? AdminId { get; set; }
        //public AppUser Admin { get; set; }
        public int OperationId { get; set; }
        public Operation Operation { get; set; }
    }
}
