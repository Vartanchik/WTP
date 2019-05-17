using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WTP.BLL.Models;

namespace WTP.BLL.UnitOfWork
{
    public interface IRepository<TEntity> where TEntity : class, IModel
    {
        Task CreateAsync(TEntity model);
        Task UpdateAsync(TEntity model);
        Task DeleteAsync(int id);
        Task<TEntity> GetAsync(int id);
        IQueryable<TEntity> AsQueryable();
    }
}
