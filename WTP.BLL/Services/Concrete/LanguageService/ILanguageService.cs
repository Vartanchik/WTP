using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.BLL.Models.Language;

namespace WTP.BLL.Services.Concrete.LanguageService
{
    public interface ILanguageService
    {
        Task CreateAsync(LanguageModel languageDto);
        Task UpdateAsync(LanguageModel languageDto);
        Task DeleteAsync(int id);
        Task<LanguageModel> GetAsync(int id);
        Task<IEnumerable<LanguageModel>> GetAllAsync();
    }
}
