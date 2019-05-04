using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WTP.DAL.DomainModels;
using WTP.DAL.Repositories.GenericRepository;

namespace WTP.DAL.Repositories.ConcreteRepositories
{
    internal class OperationRepository: RepositoryBase<Operation>, IRepository<Operation>
    {
        private readonly ApplicationDbContext _context;

        public OperationRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task CreateAsync(Operation item)
        {
            await _context.Operations.AddAsync(item);

            await _context.SaveChangesAsync();
        }

        public override async Task UpdateAsync(Operation item)
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

        public override async Task<Operation> GetAsync(int id)
        {
            return await _context.Operations.FirstOrDefaultAsync(_ => _.Id == id);
        }

        public override async Task<IEnumerable<Operation>> GetAllAsync()
        {
            return await _context.Operations.ToListAsync();
        }
    }
}
