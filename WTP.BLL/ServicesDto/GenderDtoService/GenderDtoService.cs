using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.BLL.ModelsDto;
using WTP.DAL.Services;
using WTP.WebApi.WTP.DAL.DomainModels;

namespace WTP.BLL.Services.GenderDtoService
{
    public class GenderDtoService : IMaintainableDto<GenderDto>
    {
        private readonly IMaintainable<Gender> _genderService;

        public GenderDtoService(IMaintainable<Gender> genderService)
        {
            _genderService = genderService;
        }

        public Task<bool> CreateAsync(GenderDto genderDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<GenderDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<GenderDto> GetAsync(int id)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Gender, GenderDto>()
                    .ForMember("Id", opt => opt.MapFrom(_ => _.Id))
                    .ForMember("Name", opt => opt.MapFrom(_ => _.Name)));

            return Mapper.Map<Gender, GenderDto>(await _genderService.GetAsync(id));
        }

        public Task<bool> UpdateAsync(GenderDto genderDto)
        {
            throw new NotImplementedException();
        }
    }
}
