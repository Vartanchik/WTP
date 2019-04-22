using AutoMapper;
using WTP.BLL.ModelsDto;
using WTP.BLL.TransferModels;
using WTP.DAL.DomainModels;
using WTP.WebApi.WTP.DAL.DomainModels;

namespace WTP.BLL.ServicesDto
{
    public class DtoProfile : Profile
    {
        public DtoProfile()
        {
            CreateMap<AppUserDto, AppUser>();
            CreateMap<AppUser, AppUserDto>();
            CreateMap<LanguageDto, Language>();
            CreateMap<Language, LanguageDto>();
        }
    }
}
