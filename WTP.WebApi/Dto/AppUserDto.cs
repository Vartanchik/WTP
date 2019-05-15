using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WTP.BLL.Models.AppUser;
using WTP.BLL.Models.Country;
using WTP.BLL.Models.Gender;
using WTP.BLL.Models.Language;
using WTP.BLL.Models.Player;
using WTP.BLL.Models.Team;

namespace WTP.WebAPI.Dto
{
    public class AppUserApiDto
    {
        [Required]
        public int Id { get; set; } 

        [Required]
        [Column(TypeName = "nvarchar(30)")]
        public string UserName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Photo { get; set; }

        [Required]
        public GenderModel Gender { get; set; }

        [Required]
        public string DateOfBirth { get; set; }

        [Required]
        public CountryModel Country { get; set; }

        public string Steam { get; set; }

        [Required]
        public List<LanguageModel> Languages { get; set; }

        public List<PlayerModel> Players { get; set; }

        public List<TeamModel> Teams { get; set; }
    }
}
