using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WTP.DAL.DomainModels;
using WTP.DAL.Repositories.GenericRepository;

namespace WTP.DAL.Repositories.ConcreteRepositories
{
    internal class AdminOperationRepository : RepositoryBase<AdminOperation>, IRepository<AdminOperation>
    {
        private readonly ApplicationDbContext _context;

        public AdminOperationRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task CreateAsync(AdminOperation item)
        {
            await _context.AdminOperations.AddAsync(item);

            await _context.SaveChangesAsync();
        }

        public override async Task UpdateAsync(AdminOperation item)
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

        public override async Task<AdminOperation> GetAsync(int id)
        {
            return await _context.AdminOperations.FirstOrDefaultAsync(_ => _.Id == id);
        }

        public override async Task<IEnumerable<AdminOperation>> GetAllAsync()
        {
            return await _context.AdminOperations.ToListAsync();
        }
    }
}
