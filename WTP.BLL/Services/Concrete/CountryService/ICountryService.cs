using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.BLL.Dto.Country;

namespace WTP.BLL.Services.Concrete.CountryService
{
    public interface ICountryService
    {
        Task CreateAsync(CountryDto countryDto);
        Task UpdateAsync(CountryDto countryDto);
        Task DeleteAsync(int id);
        Task<CountryDto> GetAsync(int id);
        Task<IEnumerable<CountryDto>> GetAllAsync();
    }
}
