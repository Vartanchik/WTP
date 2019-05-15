using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.BLL.Models.Language;
using WTP.DAL.Entities;
using WTP.DAL.UnitOfWork;

namespace WTP.BLL.Services.Concrete.LanguageService
{
    public class LanguageService : ILanguageService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public LanguageService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task CreateAsync(LanguageModel languageDto)
        {
            var language = _mapper.Map<Language>(languageDto);

            await _uow.Languages.CreateAsync(language);
        }

        public async Task UpdateAsync(LanguageModel languageDto)
        {
            var language = _mapper.Map<Language>(languageDto);

            await _uow.Languages.UpdateAsync(language);
        }

        public async Task DeleteAsync(int id)
        {
            await _uow.Languages.DeleteAsync(id);
        }

        public async Task<LanguageModel> GetAsync(int id)
        {
            var language = await _uow.Languages.GetAsync(id);

            return _mapper.Map<LanguageModel>(language);
        }

        public async Task<IEnumerable<LanguageModel>> GetAllAsync()
        {
            var languages = await _uow.Languages.GetAllAsync();

            return _mapper.Map<IEnumerable<LanguageModel>>(languages);
        }
    }
}
