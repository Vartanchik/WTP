using System.Linq;
using System.Threading.Tasks;

namespace WTP.DAL.Repositories.GenericRepository
{
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        Task CreateAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(int id);
        Task<TEntity> GetAsync(int id);
        IQueryable<TEntity> AsQueryable();      
    }
}
