using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.DAL.Services;
using WTP.WebApi.WTP.DAL.DomainModels;

namespace WTP.WebApi.WTP.DAL.Services.GenderService
{
    public class GenderService : IMaintainable<Gender>
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public GenderService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<bool> CreateAsync(Gender gender)
        {
            var flag = false;

            try
            {
                await _applicationDbContext.Genders.AddAsync(gender);
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
                var gender = GetAsync(id);
                _applicationDbContext.Entry(gender).State = EntityState.Deleted;
                await _applicationDbContext.SaveChangesAsync();

                flag = true;
            }
            catch
            {
                return flag;
            }

            return flag;
        }

        public async Task<List<Gender>> GetAllAsync()
        {
            return await _applicationDbContext.Genders.ToListAsync();
        }

        public async Task<Gender> GetAsync(int id)
        {
            return await _applicationDbContext.Genders.FirstOrDefaultAsync(_ => _.Id == id);
        }

        public async Task<bool> UpdateAsync(Gender gender)
        {
            var flag = false;

            try
            {
                _applicationDbContext.Entry(gender).State = EntityState.Modified;
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
