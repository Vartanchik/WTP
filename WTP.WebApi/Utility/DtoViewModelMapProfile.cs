using AutoMapper;
using WTP.BLL.ModelsDto.AppUser;
using WTP.WebAPI.ViewModels;

namespace WTP.WebAPI.Services
{
    public class DtoViewModelMapProfile : Profile
    {
        public DtoViewModelMapProfile()
        {
            CreateMap<AppUserDto, AppUserDtoViewModel>();
            CreateMap<AppUserDtoViewModel, AppUserDto>();
        }
    }
}
