using AutoMapper;
using WTP.BLL.Models.AppUser;
using WTP.BLL.Models.Azure;
using WTP.BLL.Models.Email;
using WTP.WebAPI.Dto;

namespace WTP.WebAPI.Services
{
    public class DtoProfile : Profile
    {
        public DtoProfile()
        {
            CreateMap<AppUserModel, AppUserApiDto>();
            CreateMap<AppUserApiDto, AppUserModel>();
            CreateMap<AzureBlobStorageConfigDto, AzureBlobStorageConfigModel>();
            CreateMap<EmailConfigDto, EmailConfigModel>();
            CreateMap<FileDataDto, FileDataModel>();
            CreateMap<FileDataModel, FileDataDto>();
        }
    }
}
