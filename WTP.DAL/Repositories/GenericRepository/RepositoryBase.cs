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

        public virtual async Task CreateOrUpdate(TEntity item)
        {
            if (item.Id == 0)
            {
                await _dbset.AddAsync(item);
            }
            else
            {
                //  _context.Entry(item).State = EntityState.Modified;
                var entity = await _dbset.FindAsync(item.Id); //To Avoid tracking error
                var attachedEntry = _context.Entry(entity);
                attachedEntry.CurrentValues.SetValues(item);
            }
        }

        public virtual async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);

            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Deleted;
            }
        }

        public virtual void Delete(TEntity entity)
        {
            _dbset.Remove(entity);
        }


        public virtual async Task<TEntity> GetByIdAsync(int id)
        {
            return await _dbset.FindAsync(id);
        }

        public virtual IQueryable<TEntity> AsQueryable()
        {
            return _dbset.AsQueryable<TEntity>();
        }
        
    }
}
