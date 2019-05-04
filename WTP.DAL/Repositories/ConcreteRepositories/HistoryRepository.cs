using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WTP.DAL.DomainModels;
using WTP.DAL.Repositories.GenericRepository;

namespace WTP.DAL.Repositories.ConcreteRepositories
{
    internal class HistoryRepository: RepositoryBase<History>, IRepository<History>
    {
        private readonly ApplicationDbContext _context;

        public HistoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task CreateAsync(History item)
        {
            await _context.Histories.AddAsync(item);

            await _context.SaveChangesAsync();
        }

        public override async Task UpdateAsync(History item)
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

        public override async Task<History> GetAsync(int id)
        {
            return await _context.Histories.FirstOrDefaultAsync(_ => _.Id == id);
        }

        public override async Task<IEnumerable<History>> GetAllAsync()
        {
            return await _context.Histories.ToListAsync();
        }
    }
}
