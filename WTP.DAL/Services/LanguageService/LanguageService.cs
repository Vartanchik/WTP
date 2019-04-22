using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WTP.WebApi.WTP.DAL.DomainModels;
using WTP.DAL.Services;

namespace WTP.WebApi.WTP.DAL.Services.LanguageService
{
    public class LanguageService : IMaintainable<Language>
    {
        private ApplicationDbContext _applicationDbContext;

        public LanguageService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }      

        public async Task<bool> CreateAsync(Language language)
        {
            var flag = false;

            try
            {
                await _applicationDbContext.Languages.AddAsync(language);
                await _applicationDbContext.SaveChangesAsync();

                flag = true;
            }
            catch
            {
                return flag;
            }

            return flag;
        }

        public async Task<Language> GetAsync(int id)
        {
            return await _applicationDbContext.Languages.FirstOrDefaultAsync(_ => _.Id == id);
        }

        public async Task<List<Language>> GetAllAsync()
        {
            return await _applicationDbContext.Languages.ToListAsync();
        }

        public async Task<bool> UpdateAsync(Language language)
        {
            var flag = false;

            try
            {
                _applicationDbContext.Entry(language).State = EntityState.Modified;
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
                var language = GetAsync(id);
                _applicationDbContext.Entry(language).State = EntityState.Deleted;
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
