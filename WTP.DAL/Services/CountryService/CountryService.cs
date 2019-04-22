using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.DAL.Services;
using WTP.WebApi.WTP.DAL.DomainModels;

namespace WTP.WebApi.WTP.DAL.Services.CountryService
{
    public class CountryService : IMaintainable<Country>
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public CountryService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<bool> CreateAsync(Country country)
        {
            var flag = false;

            try
            {
                await _applicationDbContext.Countries.AddAsync(country);
                await _applicationDbContext.SaveChangesAsync();

                flag = true;
            }
            catch
            {
                return flag;
            }

            return flag;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var flag = false;

            try
            {
                var country = GetAsync(id);
                _applicationDbContext.Entry(country).State = EntityState.Deleted;
                await _applicationDbContext.SaveChangesAsync();

                flag = true;
            }
            catch
            {
                return flag;
            }

            return flag;
        }

        public async Task<List<Country>> GetAllAsync()
        {
            return await _applicationDbContext.Countries.ToListAsync();
        }

        public async Task<Country> GetAsync(int id)
        {
            return await _applicationDbContext.Countries.FirstOrDefaultAsync(_ => _.Id == id);
        }

        public async Task<bool> UpdateAsync(Country country)
        {
            var flag = false;

            try
            {
                _applicationDbContext.Entry(country).State = EntityState.Modified;
                await _applicationDbContext.SaveChangesAsync();

                flag = true;
            }
            catch
            {
                return flag;
            }

            return flag;
        }
    }
}
