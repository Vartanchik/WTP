using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WTP.BLL.Models.RefreshToken;

namespace WTP.BLL.Services.Concrete.RefreshTokenService
{
    public interface IRefreshTokenService
    {
        Task CreateAsync(RefreshTokenModel tokenDto);
        Task DeleteAsync(int id);
        Task DeleteRangeAsync(int userId);
        Task<RefreshTokenModel> GetAsync(int id);
        Task<IEnumerable<RefreshTokenModel>> GetRangeAsync(int id);
        Task<RefreshTokenModel> GetByUserIdAsync(int userId, string refreshToken);
    }
}
