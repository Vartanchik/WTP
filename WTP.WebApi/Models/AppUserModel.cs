using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WTP.BLL.Dto.Country;
using WTP.BLL.Dto.Gender;
using WTP.BLL.Dto.Language;
using WTP.BLL.Dto.Player;
using WTP.BLL.Dto.Team;


namespace WTP.WebAPI.Models
{
    public class AppUserModel
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
        public GenderDto Gender { get; set; }

        [Required]
        public string DateOfBirth { get; set; }

        [Required]
        public CountryDto Country { get; set; }

        public string Steam { get; set; }

        [Required]
        public List<LanguageDto> Languages { get; set; }

        public List<PlayerDto> Players { get; set; }

        public List<TeamDto> Teams { get; set; }
    }

}
