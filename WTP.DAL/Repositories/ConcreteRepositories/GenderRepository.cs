using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.DAL.DomainModels;
using WTP.DAL.Repositories.GenericRepository;

namespace WTP.DAL.Repositories.ConcreteRepositories
{
    internal class GenderRepository : RepositoryBase<Gender>, IRepository<Gender>
    {
        private readonly ApplicationDbContext _context;

        public GenderRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task CreateAsync(Gender item)
        {
            await _context.Genders.AddAsync(item);

            await _context.SaveChangesAsync();
        }

        public override async Task UpdateAsync(Gender item)
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

        public override async Task<Gender> GetAsync(int id)
        {
            return await _context.Genders.FirstOrDefaultAsync(_ => _.Id == id);
        }

        public override async Task<IEnumerable<Gender>> GetAllAsync()
        {
            return await _context.Genders.ToListAsync();
        }
    }
}
