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
        Task<IdentityResult> CreateAdminAsync(AppUser appUser, string password);
        Task<IdentityResult> CreateAsync(AppUser appUser, string password);
        Task<IdentityResult> UpdateAsync(AppUser appUser);
        Task<AppUser> GetByEmailAsync(string email);
        Task<IList<string>> GetRolesAsync(AppUser appUser);
        Task<bool> CheckPasswordAsync(int id, string password);
        Task<AppUser> GetAsync(int id);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<AppUser>> GetAllAsync();
        Task<bool> LockAsync(int id, int? days);
        Task<bool> UnLockAsync(int id);
    }
}
