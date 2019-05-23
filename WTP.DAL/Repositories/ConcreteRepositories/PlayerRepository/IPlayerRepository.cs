using System;
using System.Collections.Generic;
using System.Text;
using WTP.DAL.Repositories.GenericRepository;

namespace WTP.DAL.Repositories.ConcreteRepositories.PlayerRepository
{
    public interface IPlayerRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        IList<TEntity> GetPlayersByUserId(int userId);
    }
}
