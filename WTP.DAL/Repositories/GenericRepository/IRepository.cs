using System.Linq;
using System.Threading.Tasks;

namespace WTP.DAL.Repositories.GenericRepository
{
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        Task CreateOrUpdate(TEntity item);
        Task DeleteAsync(int id);
        void Delete(TEntity entity);
        Task<TEntity> GetByIdAsync(int id);
        IQueryable<TEntity> AsQueryable();
    }
}
