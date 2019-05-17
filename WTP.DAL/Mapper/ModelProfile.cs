using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using WTP.BLL.Models;
using WTP.BLL.Models.AppUserModels;
using WTP.BLL.Models.PlayerModels;
using WTP.BLL.Models.TeamModels;
using WTP.DAL.Entities.AppUserEntities;
using WTP.DAL.Entities.PlayerEntities;
using WTP.DAL.Entities.TeamEntities;

namespace WTP.DAL.Mapper
{
    class ModelProfile : Profile
    {
        public ModelProfile()
        {
            CreateMap<IEntity, IModel>();

            CreateMap<AppUser, AppUserModel>()
                .ForMember(dest => dest.Languages,
                           config => config.MapFrom(src => GetLanguagesDto(src)));

            CreateMap<AppUserModel, AppUser>()
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

        private List<AppUserLanguage> GetUserToLanguages(AppUserModel user)
        {
            return new List<AppUserLanguage>(user.Languages.Select(x => 
                new AppUserLanguage
                {
                    AppUserId = user.Id,
                    LanguageId = x.Id
                }));
        }

        private List<LanguageModel> GetLanguagesDto(AppUser user)
        {
            return new List<LanguageModel>(user.AppUserLanguages.Select(x => 
                new LanguageModel
                {
                    Id = x.LanguageId,
                    Name = x.Language.Name
                }));
        }
    }
}
