using System.Threading.Tasks;
using WTP.DAL.Entities;
using WTP.DAL.Repositories.GenericRepository;

namespace WTP.DAL.Repositories.ConcreteRepositories.RestoreTokenRepository
{
    public interface IRestoreTokenRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        Task<TEntity> GetByUserId(int userId); 
    }
}
