using AutoMapper;
using WTP.BLL.ModelsDto.Admin;
using WTP.BLL.ModelsDto.AdminOperation;
using WTP.BLL.ModelsDto.AppUser;
using WTP.BLL.ModelsDto.Country;
using WTP.BLL.ModelsDto.Gender;
using WTP.BLL.ModelsDto.History;
using WTP.BLL.ModelsDto.Language;
using WTP.BLL.ModelsDto.Player;
using WTP.BLL.ModelsDto.Team;
using WTP.DAL.DomainModels;

namespace WTP.BLL.Services.Concrete
{
    public class DtoMapProfile : Profile
    {
        public DtoMapProfile()
        {
            CreateMap<AppUserDto, AppUser>();
            CreateMap<AppUser, AppUserDto>();
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
            CreateMap<Admin, AdminDto>();
            CreateMap<AdminDto,Admin>();
            CreateMap<AdminOperationDto, AdminOperation>();
            CreateMap<AdminOperation,AdminOperationDto>();
            CreateMap<History, HistoryDto>();
            CreateMap<HistoryDto,History>();
        }
    }
}
