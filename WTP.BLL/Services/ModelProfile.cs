using AutoMapper;
using System.Collections.Generic;
using WTP.BLL.Dto.AppUser;
using WTP.BLL.Dto.Country;
using WTP.BLL.Dto.Gender;
using WTP.BLL.Dto.Language;
using WTP.BLL.Dto.Player;
using WTP.BLL.Dto.RefreshToken;
using WTP.BLL.Dto.Team;
using WTP.DAL.Entities;

namespace WTP.BLL.Services.Concrete
{
    public class ModelProfile : Profile
    {
        private readonly string _defaultPhoto;

        public ModelProfile(string defaultPhoto)
        {
            this._defaultPhoto = defaultPhoto;

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
                ? _defaultPhoto
                : user.Photo;
        }

        private string PhotoToSave(AppUserDto user)
        {
            return user.Photo == _defaultPhoto
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
