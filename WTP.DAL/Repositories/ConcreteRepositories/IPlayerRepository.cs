using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WTP.DAL.Entities;
using WTP.DAL.Repositories.GenericRepository;

namespace WTP.DAL.Repositories.ConcreteRepositories
{
    public interface IPlayerRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        Task<IList<Player>> GetAllPlayersAsync();
    }
}
