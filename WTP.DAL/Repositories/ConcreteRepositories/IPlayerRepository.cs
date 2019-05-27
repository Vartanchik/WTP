using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WTP.DAL.Repositories.GenericRepository;

namespace WTP.DAL.Repositories.ConcreteRepositories
{
    public interface IPlayerRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
    }
}
