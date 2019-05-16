using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WTP.DAL.Repositories.GenericRepository
{
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        Task<TEntity> FindByIdAsync(int id);
        IQueryable<TEntity> FindAllAsync();
        IQueryable<TEntity> FindByConditionAsync(Expression<Func<TEntity, bool>> expression);
        Task CreateAsync(TEntity entity);
        void UpdateAsync(TEntity entity);
        void DeleteAsync(TEntity entity);
    }
}
