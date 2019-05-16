﻿using AutoMapper;
using WTP.BLL.ModelsDto.AppUser;
using WTP.BLL.ModelsDto.Azure;
using WTP.BLL.ModelsDto.Email;
using WTP.WebAPI.ViewModels;

namespace WTP.WebAPI.Services
{
    public class DtoViewModelMapProfile : Profile
    {
        public DtoViewModelMapProfile()
        {
            CreateMap<AppUserDto, AppUserDtoViewModel>();
            CreateMap<AppUserDtoViewModel, AppUserDto>();
            CreateMap<AzureBlobStorageConfigModel, AzureBlobStorageConfigDto>();
            CreateMap<EmailConfigModel, EmailConfigDto>();
            CreateMap<FileDataModel, FileDataDto>();
            CreateMap<FileDataDto, FileDataModel>();
        }
    }
}
