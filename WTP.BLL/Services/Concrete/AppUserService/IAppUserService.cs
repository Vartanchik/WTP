using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.BLL.Models.AppUser;

namespace WTP.BLL.Services.Concrete.AppUserService
{
    public interface IAppUserService
    {
        Task<IdentityResult> CreateAsync(AppUserModel appUserDto, string password);
        Task<AppUserModel> GetAsync(int userId);
        Task<AppUserModel> GetByEmailAsync(string email);
        Task<AppUserModel> GetByNameAsync(string userName);
        Task<IdentityResult> UpdateAsync(AppUserModel appUserDto);
        Task<IList<string>> GetRolesAsync(AppUserModel appUserDto);
        Task<bool> CheckPasswordAsync(int userId, string password);
        Task<bool> IsEmailConfirmedAsync(int userId);
        Task<string> GeneratePasswordResetTokenAsync(AppUserModel appUserDto);
        Task<IdentityResult> ResetPasswordAsync(ResetPasswordModel resetPasswordDto);
        Task<IdentityResult> ChangePasswordAsync(ChangePasswordModel changePasswordDto);
        Task<string> GenerateEmailConfirmationTokenAsync(AppUserModel appUserDto);
        Task<AppUserModel> FindByIdAsync(string userId);
        Task<IdentityResult> ConfirmEmailAsync(int userId, string token);
    }
}
