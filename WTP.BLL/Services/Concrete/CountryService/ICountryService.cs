using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.BLL.Models.Country;

namespace WTP.BLL.Services.Concrete.CountryService
{
    public interface ICountryService
    {
        Task CreateAsync(CountryModel countryDto);
        Task UpdateAsync(CountryModel countryDto);
        Task DeleteAsync(int id);
        Task<CountryModel> GetAsync(int id);
        Task<IEnumerable<CountryModel>> GetAllAsync();
    }
}
