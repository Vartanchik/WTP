using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WTP.BLL.ModelsDto;

namespace WTP.WebAPI.ViewModels
{
    public class AppUserDtoViewModel
    {
        [Required]
        [Column(TypeName = "nvarchar(30)")]
        public string UserName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Photo { get; set; }

        [Required]
        public GenderDto Gender { get; set; }

        [Required]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        public CountryDto Country { get; set; }

        public string Steam { get; set; }

        [Required]
        public List<LanguageDto> Languages { get; set; }

        public List<PlayerDto> Players { get; set; }

        public List<TeamDto> Teams { get; set; }
    }
}
