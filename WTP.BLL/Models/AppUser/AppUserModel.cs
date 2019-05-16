using System;
using System.Collections.Generic;
using WTP.BLL.Models.Country;
using WTP.BLL.Models.Gender;
using WTP.BLL.Models.Language;
using WTP.BLL.Models.Player;
using WTP.BLL.Models.RefreshToken;
using WTP.BLL.Models.Team;

namespace WTP.BLL.Models.AppUser
{
    public class AppUserModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Photo { get; set; }
        public GenderModel Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public CountryModel Country { get; set; }
        public string Steam { get; set; }
        public ICollection<LanguageModel> Languages { get; set; }
        public List<PlayerModel> Players { get; set; }
        public List<TeamModel> Teams { get; set; }
        public string SecurityStamp { get; set; }
        public virtual List<RefreshTokenModel> Tokens { get; set; }
        public bool Enabled { get; set; }
    }
}
