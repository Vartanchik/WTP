using System.Threading.Tasks;
using WTP.DAL.Entities;
using WTP.DAL.Repositories.GenericRepository;

namespace WTP.DAL.Repositories.ConcreteRepositories.RefreshTokenRepository
{
    public interface IRefreshTokenRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        Task DeleteUserTokensAsync(int userId);
    }
}
