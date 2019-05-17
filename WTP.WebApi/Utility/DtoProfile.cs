using AutoMapper;
using WTP.BLL.Dto.Azure;
using WTP.BLL.Dto.Email;
using WTP.WebAPI.Models;

namespace WTP.WebAPI.Services
{
    public class DtoProfile : Profile
    {
        public DtoProfile()
        {
            CreateMap<AzureBlobStorageConfigModel, AzureBlobStorageConfigDto>();
            CreateMap<EmailConfigModel, EmailConfigDto>();
        }
    }
}
