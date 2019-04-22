using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WTP.BLL.ModelsDto;
using WTP.DAL.Services;
using WTP.WebApi.WTP.DAL.DomainModels;

namespace WTP.BLL.Services.LanguageDtoService
{
    public class LanguageDtoService : IMaintainableDto<LanguageDto>
    {
        private readonly IMaintainable<Language> _maintainable;
        private readonly IMapper _mapper;

        public LanguageDtoService(IMaintainable<Language> maintainable, IMapper mapper)
        {
            _maintainable = maintainable;
            _mapper = mapper;
    }

        public async Task<bool> CreateAsync(LanguageDto languageDto)
        {
            var language = _mapper.Map<Language>(languageDto);

            return await _maintainable.CreateAsync(language);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _maintainable.DeleteAsync(id);
        }

        public async Task<List<LanguageDto>> GetAllAsync()
        {
            var languages = await _maintainable.GetAllAsync();

            return _mapper.Map<List<LanguageDto>>(languages);
        }

        public async Task<LanguageDto> GetAsync(int id)
        {
            var language = await _maintainable.GetAsync(id);

            return _mapper.Map<LanguageDto>(language);
        }

        public async Task<bool> UpdateAsync(LanguageDto languageDto)
        {
            var language = _mapper.Map<Language>(languageDto);

            return await _maintainable.UpdateAsync(language);
        }
    }
}
