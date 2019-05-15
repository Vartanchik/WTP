using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.DAL.Entities;
using WTP.DAL.Repositories.GenericRepository;

namespace WTP.DAL.Repositories.ConcreteRepositories
{
    internal class LanguageRepository : RepositoryBase<Language>, IRepository<Language>
    {
        private readonly ApplicationDbContext _context;

        public LanguageRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task CreateAsync(Language item)
        {
            await _context.Languages.AddAsync(item);

            await _context.SaveChangesAsync();
        }

        public override async Task UpdateAsync(Language item)
        {
            _context.Entry(item).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }

        public override async Task DeleteAsync(int id)
        {
            var entity = await GetAsync(id);

            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Deleted;

                await _context.SaveChangesAsync();
            }
        }

        public override async Task<Language> GetAsync(int id)
        {
            return await _context.Languages.FirstOrDefaultAsync(_ => _.Id == id);
        }

        public override async Task<IEnumerable<Language>> GetAllAsync()
        {
            return await _context.Languages.ToListAsync();
        }
    }
}
