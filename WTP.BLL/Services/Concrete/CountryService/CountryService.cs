using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.BLL.ModelsDto.Country;
using WTP.DAL.DomainModels;
using WTP.DAL.UnitOfWork;

namespace WTP.BLL.Services.Concrete.CountryService
{
    public class CountryService : ICountryService
    {
        private IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CountryService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task CreateAsync(CountryDto countryDto)
        {
            var country = _mapper.Map<Country>(countryDto);

            await _uow.Countries.CreateAsync(country);
        }

        public async Task UpdateAsync(CountryDto countryDto)
        {
            var country = _mapper.Map<Country>(countryDto);

            await _uow.Countries.UpdateAsync(country);
        }

        public async Task DeleteAsync(int id)
        {
            await _uow.Countries.DeleteAsync(id);
        }

        public async Task<CountryDto> GetAsync(int id)
        {
            var country = await _uow.Countries.GetAsync(id);

            return _mapper.Map<CountryDto>(country);
        }

        public async Task<IEnumerable<CountryDto>> GetAllAsync()
        {
            var countries = await _uow.Countries.GetAllAsync();

            return _mapper.Map<IEnumerable<CountryDto>>(countries);
        }
    }
}
