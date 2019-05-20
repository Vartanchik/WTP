using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WTP.BLL.DTOs.AppUserDTOs;

namespace WTP.BLL.Services.Concrete.RefreshTokenService
{
    public interface IRefreshTokenService
    {
        Task CreateAsync(RefreshTokenDto tokenDto);
        Task DeleteAsync(int id);
        Task DeleteRangeAsync(int userId);
        Task<RefreshTokenDto> GetAsync(int id);
        IQueryable<RefreshTokenDto> GetRangeAsync(int id);
        Task<RefreshTokenDto> GetByUserIdAsync(int userId, string refreshToken);
    }
}
