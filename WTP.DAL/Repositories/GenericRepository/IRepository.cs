﻿using System.Linq;
using System.Threading.Tasks;

namespace WTP.DAL.Repositories.GenericRepository
{
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        Task CreateOrUpdate(TEntity item);
        Task DeleteAsync(int id);
        Task<TEntity> GetAsync(int id);
        IQueryable<TEntity> AsQueryable();
        Task Update(TEntity entity);
    }
}
