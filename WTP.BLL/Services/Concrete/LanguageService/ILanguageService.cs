using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.BLL.ModelsDto.Language;

namespace WTP.BLL.Services.Concrete.LanguageService
{
    public interface ILanguageService
    {
        Task CreateAsync(LanguageDto languageDto);
        Task UpdateAsync(LanguageDto languageDto);
        Task DeleteAsync(int id);
        Task<LanguageDto> GetAsync(int id);
        Task<IEnumerable<LanguageDto>> GetAllAsync();
    }
}
