using System.Linq;
using System.Threading.Tasks;
using WTP.DAL.Entities;
using WTP.DAL.Repositories.GenericRepository;

namespace WTP.DAL.Repositories.ConcreteRepositories
{
    public interface ITokenRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        Task DeleteUserTokensAsync(int userId);
        IQueryable<TEntity> GetUserTokensAsync(int id);
        Task<RefreshToken> GetByUserIdAsync(int userId, string refreshToken);
    }
}
