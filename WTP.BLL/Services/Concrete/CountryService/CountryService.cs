using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.BLL.Models.Country;
using WTP.DAL.Entities;
using WTP.DAL.UnitOfWork;

namespace WTP.BLL.Services.Concrete.CountryService
{
    public class CountryService : ICountryService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CountryService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task CreateAsync(CountryModel countryDto)
        {
            var country = _mapper.Map<Country>(countryDto);

            await _uow.Countries.CreateAsync(country);
        }

        public async Task UpdateAsync(CountryModel countryDto)
        {
            var country = _mapper.Map<Country>(countryDto);

            await _uow.Countries.UpdateAsync(country);
        }

        public async Task DeleteAsync(int id)
        {
            await _uow.Countries.DeleteAsync(id);
        }

        public async Task<CountryModel> GetAsync(int id)
        {
            var country = await _uow.Countries.GetAsync(id);

            return _mapper.Map<CountryModel>(country);
        }

        public async Task<IEnumerable<CountryModel>> GetAllAsync()
        {
            var countries = await _uow.Countries.GetAllAsync();

            return _mapper.Map<IEnumerable<CountryModel>>(countries);
        }
    }
}
