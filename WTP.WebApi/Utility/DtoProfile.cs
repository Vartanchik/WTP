using AutoMapper;
using WTP.BLL.Models.AppUserModels;
using WTP.WebAPI.Dto;

namespace WTP.WebAPI.Services
{
    public class DtoProfile : Profile
    {
        public DtoProfile()
        {
            CreateMap<AppUserModel, AppUserApiDto>();
            CreateMap<AppUserApiDto, AppUserModel>();
        }
    }
}
