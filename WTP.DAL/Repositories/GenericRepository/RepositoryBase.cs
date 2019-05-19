using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace WTP.DAL.Repositories.GenericRepository
{
    public class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
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
        }

        public virtual async Task UpdateAsync(TEntity item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }

        public virtual async Task CreateOrUpdate(TEntity item)
        {
            if (item.Id == 0)
                await _dbset.AddAsync(item);
            else
                _context.Entry(item).State = EntityState.Modified;
        }

        public virtual async Task DeleteAsync(int id)
        {
            var entity = await GetAsync(id);

            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Deleted;
            }
        }

        public virtual async Task<TEntity> GetAsync(int id)
        {
            return await _dbset.FindAsync(id);
        }

        public virtual IQueryable<TEntity> AsQueryable()
        {
            return _dbset.AsQueryable<TEntity>();
        }
    }
}
