using System;
using System.Collections.Generic;
using WTP.DAL.Entities.PlayerEntities;
using WTP.DAL.Entities.TeamEntities;

namespace WTP.DAL.Entities.AppUserEntities
{
    public class AppUser : IEntity
    {
        public  int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SequrityStamp { get; set; }
        public string ConcurrencyStamp { get; set; }
        public int UserRoleId { get; set; }
        public AppUserRole AppUserRole { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTime LockoutEnd { get; set; }
        public bool DeletedEnabled { get; set; }
        public DateTime DeletedEnd { get; set; }
        public string Photo { get; set; }
        public int? GenderId { get; set; }
        public Gender Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? CountryId { get; set; }
        public Country Country { get; set; }
        public string Steam { get; set; }
        public ICollection<AppUserLanguage> AppUserLanguages { get; set; }
        public List<Player> Players { get; set; }
        public List<Team> Teams { get; set; }
        public virtual List<RefreshToken> Tokens { get; set; }
    }
}
