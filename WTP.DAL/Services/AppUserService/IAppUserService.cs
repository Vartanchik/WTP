using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.WebApi.WTP.DAL.DomainModels;

namespace WTP.WebApi.WTP.DAL.Services.AppUserService
{
    public interface IAppUserService
    {
        Task<IdentityResult> CreateAsync(AppUser applicationUser, string password);
        Task<AppUser> GetAsync(string id);
        Task<AppUser> GetByManAsync(string id);
        Task<AppUser> GetByEmailAsync(string email);
        Task<IdentityResult> UpdateAsync(AppUser applicationUser);
        Task<IList<string>> GetRolesAsync(AppUser applicationUser);
        Task<bool> CheckPasswordAsync(string id, string password);
        Task<string> GetPasswordResetTokenAsync(AppUser applicationUser);
        Task<bool> IsEmailConfirmedAsync(AppUser applicationUser);
        Task<IdentityResult> ResetPasswordAsync(AppUser applicationUser, string token, string newPassword);
        void DeleteEntityState(AppUser applicationUser);
    }
}
