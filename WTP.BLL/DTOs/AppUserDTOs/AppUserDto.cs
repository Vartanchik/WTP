using System.Collections.Generic;
using WTP.BLL.DTOs.PlayerDTOs;
using WTP.BLL.DTOs.TeamDTOs;

namespace WTP.BLL.DTOs.AppUserDTOs
{
    public class AppUserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Photo { get; set; }
        public GenderDto Gender { get; set; }
        public string DateOfBirth { get; set; }
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
