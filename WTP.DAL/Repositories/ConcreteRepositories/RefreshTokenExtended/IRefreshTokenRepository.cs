using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WTP.DAL.Entities;

namespace WTP.DAL.Repositories.ConcreteRepositories.RefreshTokenExtended
{
    public interface IRefreshTokenRepository
    {
        Task CreateAsync(RefreshToken token);
        Task DeleteAsync(int id);
        Task DeleteRangeAsync(int userId);
        Task<RefreshToken> GetAsync(int id);
        Task<IEnumerable<RefreshToken>> GetRangeAsync(int id);
        Task<RefreshToken> GetByUserIdAsync(int userId, string refreshToken);
    }
}
