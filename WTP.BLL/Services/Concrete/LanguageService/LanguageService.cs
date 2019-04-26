using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.BLL.ModelsDto.Language;
using WTP.DAL.DomainModels;
using WTP.DAL.UnitOfWork;

namespace WTP.BLL.Services.Concrete.LanguageService
{
    public class LanguageService : ILanguageService
    {
        private IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public LanguageService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task CreateAsync(LanguageDto languageDto)
        {
            var language = _mapper.Map<Language>(languageDto);

            await _uow.Languages.CreateAsync(language);
        }

        public async Task UpdateAsync(LanguageDto languageDto)
        {
            var language = _mapper.Map<Language>(languageDto);

            await _uow.Languages.UpdateAsync(language);
        }

        public async Task DeleteAsync(int id)
        {
            await _uow.Languages.DeleteAsync(id);
        }

        public async Task<LanguageDto> GetAsync(int id)
        {
            var language = await _uow.Languages.GetAsync(id);

            return _mapper.Map<LanguageDto>(language);
        }

        public async Task<IEnumerable<LanguageDto>> GetAllAsync()
        {
            var languages = await _uow.Languages.GetAllAsync();

            return _mapper.Map<IEnumerable<LanguageDto>>(languages);
        }
    }
}
