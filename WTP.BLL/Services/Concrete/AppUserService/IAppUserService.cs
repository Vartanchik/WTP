using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.BLL.DTOs.AppUserDTOs;
using WTP.BLL.DTOs.ServicesDTOs;

namespace WTP.BLL.Services.Concrete.AppUserService
{
    public interface IAppUserService
    {
        Task<IdentityResult> CreateAsync(AppUserDto dto, string password);
        Task<AppUserDto> GetByIdAsync(int userId);
        Task<AppUserDto> GetByEmailAsync(string email);
        Task<AppUserDto> GetByNameAsync(string userName);
        Task<IdentityResult> UpdateAsync(AppUserDto dto);
        Task<IList<string>> GetRolesAsync(AppUserDto dto);
        Task<bool> CheckPasswordAsync(int userId, string password);
        Task<bool> IsEmailConfirmedAsync(int userId);
        Task<string> GeneratePasswordResetTokenAsync(AppUserDto dto);
        Task<IdentityResult> ResetPasswordAsync(ResetPasswordDto dto);
        Task<IdentityResult> ChangePasswordAsync(ChangeUserPasswordDto dto);
        Task<string> GenerateEmailConfirmationTokenAsync(AppUserDto dto);
        Task<AppUserDto> FindByIdAsync(string userId);
        Task<IdentityResult> ConfirmEmailAsync(int userId, string token);
        Task DeleteAccountAsync(int userId);
        Task<string> CreateRestoreAccountToken(int userId);
        Task<bool> RestoreAccountAsync(int userId, string token);
        Task<UserIconDto> GetUserIconAsync(int userId);
    }
}
