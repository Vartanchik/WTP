using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WTP.BLL.ModelsDto.RefreshToken;

namespace WTP.BLL.Services.Concrete.RefreshTokenService
{
    public interface IRefreshTokenService
    {
        Task CreateAsync(RefreshTokenDto tokenDto);
        Task DeleteAsync(int id);
        Task DeleteRangeAsync(int userId);
        Task<RefreshTokenDto> GetAsync(int id);
        Task<IEnumerable<RefreshTokenDto>> GetRangeAsync(int id);
        Task<RefreshTokenDto> GetByUserIdAsync(int userId, string refreshToken);
    }
}
