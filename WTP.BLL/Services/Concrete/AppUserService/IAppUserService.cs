using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.BLL.ModelsDto.AppUser;

namespace WTP.BLL.Services.Concrete.AppUserService
{
    public interface IAppUserService
    {
        Task<IdentityResult> CreateAsync(AppUserDto appUserDto, string password, int adminId = 1);
        Task<AppUserDto> GetAsync(int id);
        Task<AppUserDto> GetByEmailAsync(string email);
        Task<AppUserDto> GetByNameAsync(string name);
        Task<IdentityResult> UpdateAsync(AppUserDto appUserDto, int adminId = 1);
        Task<IList<string>> GetRolesAsync(AppUserDto appUserDto);
        Task<bool> CheckPasswordAsync(int id, string password);
        Task<bool> IsEmailConfirmedAsync(AppUserDto appUserDto);
        Task<string> GeneratePasswordResetTokenAsync(AppUserDto appUserDto);
        Task<IdentityResult> ResetPasswordAsync(AppUserDto appUserDto, string token, string newPassword);
        Task<IdentityResult> ChangePasswordAsync(ChangePasswordDto changePasswordDto);
        Task<string> GenerateEmailConfirmationTokenAsync(AppUserDto appUserDto);
        Task<AppUserDto> FindByIdAsync(string id);
        Task<IdentityResult> ConfirmEmailAsync(AppUserDto appUserDto, string token);
        Task<IdentityResult> CreateAdminAsync(AppUserDto appUserDto, string password);
        Task<IdentityResult> CreateModeratorAsync(AppUserDto appUserDto, string password);
        Task<bool> DeleteAsync(int id, int adminId = 1);
        Task<IList<AppUserDto>> GetAllUsersAsync();
        Task<bool> LockAsync(int id, int? days,int adminId = 1);
        Task<bool> UnLockAsync(int id,int adminId=1);
    }
}
