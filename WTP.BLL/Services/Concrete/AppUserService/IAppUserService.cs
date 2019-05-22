using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.BLL.DTOs.AppUserDTOs;
using WTP.BLL.DTOs.ServicesDTOs;
using WTP.BLL.Shared;

namespace WTP.BLL.Services.Concrete.AppUserService
{
    public interface IAppUserService
    {
        Task<IdentityResult> CreateAsync(AppUserDto appUserDto, string password);
        Task<AppUserDto> GetAsync(int userId);
        Task<AppUserDto> GetByEmailAsync(string email);
        Task<AppUserDto> GetByNameAsync(string userName);
        Task<IdentityResult> UpdateAsync(AppUserDto appUserDto);
        Task<IList<string>> GetRolesAsync(AppUserDto appUserDto);
        Task<bool> CheckPasswordAsync(int userId, string password);
        Task<bool> IsEmailConfirmedAsync(int userId);
        Task<string> GeneratePasswordResetTokenAsync(AppUserDto appUserDto);
        Task<IdentityResult> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
        Task<IdentityResult> ChangePasswordAsync(ChangePasswordDto changePasswordDto);
        Task<string> GenerateEmailConfirmationTokenAsync(AppUserDto appUserDto);
        Task<AppUserDto> FindByIdAsync(string userId);
        Task<IdentityResult> ConfirmEmailAsync(int userId, string token);
        Task DeleteAsync(int userId);

        Task<bool> DeleteAsync(int id, int adminId = 1);
        Task<IList<AppUserDto>> GetAllUsersAsync();
        Task<bool> LockAsync(int id, int? days, int adminId = 1);
        Task<bool> UnLockAsync(int id, int adminId = 1);

        List<AppUserDto> Filter(List<AppUserDto> users, string name);
        List<AppUserDto> Sort(List<AppUserDto> users, SortState sortOrder, bool enableDeleted, bool enableLocked);

    }
}
