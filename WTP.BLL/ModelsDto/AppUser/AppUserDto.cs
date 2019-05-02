﻿using System;
using System.Collections.Generic;
using WTP.BLL.ModelsDto.AppUserLanguage;
using WTP.BLL.ModelsDto.Country;
using WTP.BLL.ModelsDto.Gender;
using WTP.BLL.ModelsDto.Player;
using WTP.BLL.ModelsDto.Team;

namespace WTP.BLL.ModelsDto.AppUser
{
    public class AppUserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Photo { get; set; }
        public int? GenderId { get; set; }
        public GenderDto Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? CountryId { get; set; }
        public CountryDto Country { get; set; }
        public string Steam { get; set; }
        public ICollection<AppUserDtoLanguageDto> AppUserLanguages { get; set; }
        public List<PlayerDto> Players { get; set; }
        public List<TeamDto> Teams { get; set; }
        public string SecurityStamp { get; set; }
        public bool DeletedStatus { get; set; }
    }
}
