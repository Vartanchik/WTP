using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WTP.BLL.ModelsDto;

namespace WTP.BLL.Services.CountryDtoService
{
    public class CountryDtoService : IMaintainableDto<CountryDto>
    {
        public Task<bool> CreateAsync(CountryDto countryDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<CountryDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<CountryDto> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(CountryDto countryDto)
        {
            throw new NotImplementedException();
        }
    }
}
