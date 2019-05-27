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
                await _dbset.AddAsync(item);
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

        public async virtual Task Update(TEntity entity)
        {
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            var allItems = _context.Games.AsNoTracking().Where(t => t.Id == entity.Id).FirstOrDefault();//FirstOrDefaultAsync(p => p.Id == entity.Id)/*.LastOrDefaultAsync()*/.ConfigureAwait(false);
            if (allItems == null)
            {
                await _context.Entry(entity).Context.AddAsync(entity).ConfigureAwait(false);
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            else
            {
                //entity.Id = allItems.Id;
                //_context.Entry(entity).Context.Set<TEntity>().Update(entity);
                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            //var t = await _context.AsNoTracking().LastOrDefault();
            //_context.Entry(entity).State = EntityState.Detached;
        }
    }
}
