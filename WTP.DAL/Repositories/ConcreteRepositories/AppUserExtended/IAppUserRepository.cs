using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.DAL.DomainModels;

namespace WTP.DAL.Repositories.ConcreteRepositories.AppUserExtended
{
    public interface IAppUserRepository
    {
        Task<IdentityResult> CreateAsync(AppUser appUser, string password);
        Task<IdentityResult> UpdateAsync(AppUser appUser);
        Task<AppUser> GetByEmailAsync(string email);
        Task<AppUser> GetByNameAsync(string userName);
        Task<IList<string>> GetRolesAsync(AppUser appUser);
        Task<bool> CheckPasswordAsync(int userId, string password);
        Task<AppUser> GetAsync(int appUserId);
        Task<bool> IsEmailConfirmedAsync(AppUser appUser);
        Task<string> GeneratePasswordResetTokenAsync(AppUser appUser);
        Task<IdentityResult> ResetPasswordAsync(AppUser appUser, string token, string newPassword);
        Task<IdentityResult> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
        Task<string> GenerateEmailConfirmationTokenAsync(AppUser appUser);
        Task<AppUser> FindByIdAsync(string userId);
        Task<IdentityResult> ConfirmEmailAsync(AppUser appUser, string token);
    }
}
