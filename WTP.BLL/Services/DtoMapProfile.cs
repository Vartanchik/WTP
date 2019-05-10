using AutoMapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using WTP.BLL.ModelsDto.AppUser;
using WTP.BLL.ModelsDto.Country;
using WTP.BLL.ModelsDto.Gender;
using WTP.BLL.ModelsDto.Language;
using WTP.BLL.ModelsDto.Player;
using WTP.BLL.ModelsDto.RefreshToken;
using WTP.BLL.ModelsDto.Team;
using WTP.DAL.DomainModels;

namespace WTP.BLL.Services.Concrete
{
    public class DtoMapProfile : Profile
    {
        private readonly IConfiguration _configuration;

        public DtoMapProfile(IConfiguration configuration)
        {
            _configuration = configuration;

            CreateMap<AppUser, AppUserDto>()
                .ForMember(dest => dest.Photo,
                           config => config.MapFrom(src => PhotoToView(src)))
                .ForMember(dest => dest.Languages,
                           config => config.MapFrom(src => GetLanguagesDto(src)));

            CreateMap<AppUserDto, AppUser>()
                .ForMember(dest => dest.Photo,
                           config => config.MapFrom(src => PhotoToSave(src)))
                .ForMember(dest => dest.AppUserLanguages,
                           config => config.MapFrom(src => GetUserToLanguages(src)));

            CreateMap<LanguageDto, Language>();
            CreateMap<Language, LanguageDto>();
            CreateMap<CountryDto, Country>();
            CreateMap<Country, CountryDto>();
            CreateMap<GenderDto, Gender>();
            CreateMap<Gender, GenderDto>();
            CreateMap<TeamDto, Team>();
            CreateMap<Team, TeamDto>();
            CreateMap<PlayerDto, Player>();
            CreateMap<Player, PlayerDto>();
            CreateMap<RefreshTokenDto, RefreshToken>();
            CreateMap<RefreshToken, RefreshTokenDto>();
        }

        private string PhotoToView(AppUser user)
        {
            return string.IsNullOrEmpty(user.Photo)
                ? _configuration["Photo:DefaultPhoto"]
                : user.Photo;
        }

        private string PhotoToSave(AppUserDto user)
        {
            return user.Photo == _configuration["Photo:DefaultPhoto"] 
                ? null
                : user.Photo;
        }

        private List<AppUserLanguage> GetUserToLanguages(AppUserDto user)
        {
            var userToLanguage = new List<AppUserLanguage>();
            foreach (var item in user.Languages)
            {
                userToLanguage.Add(new AppUserLanguage
                {
                    LanguageId = item.Id,
                    AppUserId = user.Id
                });
            }
            return userToLanguage;
        }

        private List<LanguageDto> GetLanguagesDto(AppUser user)
        {
            var languagesList = new List<LanguageDto>();
            foreach (var item in user.AppUserLanguages)
            {
                languagesList.Add(new LanguageDto
                {
                    Id = item.LanguageId,
                    Name = item.Language.Name
                });
            }

            return languagesList;
        }
    }
}
