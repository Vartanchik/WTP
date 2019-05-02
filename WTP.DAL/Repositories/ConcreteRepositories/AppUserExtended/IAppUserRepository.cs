using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WTP.DAL.DomainModels;

namespace WTP.DAL.Repositories.ConcreteRepositories.AppUserExtended
{
    public interface IAppUserRepository
    {
        Task<IdentityResult> CreateAsync(AppUser appUser, string password);
        Task<IdentityResult> UpdateAsync(AppUser appUser);
        Task<AppUser> GetByEmailAsync(string email);
        Task<IList<string>> GetRolesAsync(AppUser appUser);
        Task<bool> CheckPasswordAsync(int id, string password);
        Task<AppUser> GetAsync(int id);
        Task<bool> IsEmailConfirmedAsync(AppUser appUser);
        Task<string> GeneratePasswordResetTokenAsync(AppUser appUser);
        Task<IdentityResult> ResetPasswordAsync(AppUser appUser, string token, string newPassword);
        Task<string> GenerateEmailConfirmationTokenAsync(AppUser appUser);
        Task<AppUser> FindByIdAsync(string id);
        Task<IdentityResult> ConfirmEmailAsync(AppUser appUser, string token);
    }
}
