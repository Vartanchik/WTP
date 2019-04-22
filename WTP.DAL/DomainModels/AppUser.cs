using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using WTP.DAL.DomainModels;

namespace WTP.WebApi.WTP.DAL.DomainModels
{
    public class AppUser : IdentityUser
    {
        public override string Id { get { return base.Id; } set { base.Id = value; } }
        public override string UserName { get { return base.UserName; } set { base.UserName = value; } }
        public override string Email { get { return base.Email; } set { base.Email = value; } }
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
    }
}
