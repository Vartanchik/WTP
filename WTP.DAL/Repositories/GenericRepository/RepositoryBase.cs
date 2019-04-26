using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WTP.DAL.Repositories.GenericRepository
{
    public class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class , IEntity
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<TEntity> _dbset;

        public RepositoryBase(ApplicationDbContext context)
        {
            _context = context;
            _dbset = context.Set<TEntity>();
        }

        public virtual async Task CreateAsync(TEntity item)
        {
            await _dbset.AddAsync(item);

            await _context.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(TEntity item)
        {
            _dbset.Update(item);

            await _context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(int id)
        {
            var entity = await GetAsync(id);

            if(entity != null)
            {
                _context.Entry(entity).State = EntityState.Deleted;

                await _context.SaveChangesAsync();
            }
        }

        public virtual async Task<TEntity> GetAsync(int id)
        {
            return await _dbset.FindAsync(id);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return _dbset;
        }
    }
}
