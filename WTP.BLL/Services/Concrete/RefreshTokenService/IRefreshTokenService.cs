using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WTP.BLL.DTOs.AppUserDTOs;
using WTP.BLL.DTOs.ServicesDTOs;

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
        Task<ServiceResult> VerifyUser(LoginDto dto);
        Task<AccessResponseDto> GetAccess(string email);
        Task<ServiceResult> UpdateAccessToken(UpdateRefreshTokenDto dto);
        string GetCurrentTokenByUserId(int userId);
    }
}
