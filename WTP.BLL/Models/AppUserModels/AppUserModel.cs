using System;
using System.Collections.Generic;
using WTP.BLL.Models.PlayerModels;
using WTP.BLL.Models.TeamModels;

namespace WTP.BLL.Models.AppUserModels
{
    public class AppUserModel : IModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SequrityStamp { get; set; }
        public string ConcurrencyStamp { get; set; }
        public AppUserRoleModel AppUserRoleModel { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTime LockoutEnd { get; set; }
        public bool DeletedEnabled { get; set; }
        public DateTime DeletedEnd { get; set; }
        public string Photo { get; set; }
        public GenderModel GenderModel { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public CountryModel CountryModel { get; set; }
        public string Steam { get; set; }
        public List<LanguageModel> Languages { get; set; }
        public List<PlayerModel> Players { get; set; }
        public List<TeamModel> Teams { get; set; }
        public virtual List<RefreshTokenModel> Tokens { get; set; }
    }
}
