using System.Threading.Tasks;
using WTP.BLL.DTOs.ServicesDTOs;
using WTP.BLL.DTOs.TokensDTOs;

namespace WTP.BLL.Services.Concrete.RefreshTokenService
{
    public interface IRefreshTokenService
    {
        Task<ServiceResult> UserVerifyAsync(LoginDto dto);
        Task<string> GenerateRefreshTokenAsync(int userId);
        Task<string> GenerateAccessTokenAsync(int userId);
        Task<AccessDto> GetAccessAsync(string email);
        Task<AccessDto> UpdateAccessAsync(AccessOperationDto dto);
    }
}
