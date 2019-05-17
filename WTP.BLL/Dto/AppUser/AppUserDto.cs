using System;
using System.Collections.Generic;
using WTP.BLL.Dto.Country;
using WTP.BLL.Dto.Gender;
using WTP.BLL.Dto.Language;
using WTP.BLL.Dto.Player;
using WTP.BLL.Dto.RefreshToken;
using WTP.BLL.Dto.Team;

namespace WTP.BLL.Dto.AppUser
{
    public class AppUserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Photo { get; set; }
        public GenderDto Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public CountryDto Country { get; set; }
        public string Steam { get; set; }
        public ICollection<LanguageDto> Languages { get; set; }
        public List<PlayerDto> Players { get; set; }
        public List<TeamDto> Teams { get; set; }
        public string SecurityStamp { get; set; }
        public virtual List<RefreshTokenDto> Tokens { get; set; }
        public bool Enabled { get; set; }
    }
}
