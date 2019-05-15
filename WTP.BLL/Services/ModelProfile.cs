using AutoMapper;
using System.Collections.Generic;
using WTP.BLL.Models.AppUser;
using WTP.BLL.Models.Country;
using WTP.BLL.Models.Gender;
using WTP.BLL.Models.Language;
using WTP.BLL.Models.Player;
using WTP.BLL.Models.RefreshToken;
using WTP.BLL.Models.Team;
using WTP.DAL.Entities;

namespace WTP.BLL.Services.Concrete
{
    public class ModelProfile : Profile
    {
        private readonly string _defaultPhoto;

        public ModelProfile(string defaultPhoto)
        {
            this._defaultPhoto = defaultPhoto;

            CreateMap<AppUser, AppUserModel>()
                .ForMember(dest => dest.Photo,
                           config => config.MapFrom(src => PhotoToView(src)))
                .ForMember(dest => dest.Languages,
                           config => config.MapFrom(src => GetLanguagesDto(src)));

            CreateMap<AppUserModel, AppUser>()
                .ForMember(dest => dest.Photo,
                           config => config.MapFrom(src => PhotoToSave(src)))
                .ForMember(dest => dest.AppUserLanguages,
                           config => config.MapFrom(src => GetUserToLanguages(src)));

            CreateMap<LanguageModel, Language>();
            CreateMap<Language, LanguageModel>();
            CreateMap<CountryModel, Country>();
            CreateMap<Country, CountryModel>();
            CreateMap<GenderModel, Gender>();
            CreateMap<Gender, GenderModel>();
            CreateMap<TeamModel, Team>();
            CreateMap<Team, TeamModel>();
            CreateMap<PlayerModel, Player>();
            CreateMap<Player, PlayerModel>();
            CreateMap<RefreshTokenModel, RefreshToken>();
            CreateMap<RefreshToken, RefreshTokenModel>();
        }

        private string PhotoToView(AppUser user)
        {
            return string.IsNullOrEmpty(user.Photo)
                ? _defaultPhoto
                : user.Photo;
        }

        private string PhotoToSave(AppUserModel user)
        {
            return user.Photo == _defaultPhoto
                ? null
                : user.Photo;
        }

        private List<AppUserLanguage> GetUserToLanguages(AppUserModel user)
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

        private List<LanguageModel> GetLanguagesDto(AppUser user)
        {
            var languagesList = new List<LanguageModel>();
            foreach (var item in user.AppUserLanguages)
            {
                languagesList.Add(new LanguageModel
                {
                    Id = item.LanguageId,
                    Name = item.Language.Name
                });
            }

            return languagesList;
        }
    }
}
