using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WTP.BLL.ModelsDto.Country;
using WTP.BLL.ModelsDto.Gender;
using WTP.BLL.ModelsDto.Language;
using WTP.BLL.ModelsDto.Player;
using WTP.BLL.ModelsDto.Team;

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
