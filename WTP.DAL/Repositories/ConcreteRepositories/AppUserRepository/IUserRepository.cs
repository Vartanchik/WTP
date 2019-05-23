using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.DAL.Entities.AppUserEntities;
using WTP.DAL.Repositories.GenericRepository;

namespace WTP.DAL.Repositories.ConcreteRepositories.AppUserRepository
{
    public interface IUserRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        Task<IdentityResult> CreateAsync(AppUser appUser, string password);
        Task<IdentityResult> UpdateAsync(AppUser appUser);
        Task<AppUser> GetByEmailAsync(string email);
        Task<AppUser> GetByNameAsync(string userName);
        Task<IList<string>> GetRolesAsync(AppUser appUser);
        Task<bool> CheckPasswordAsync(int userId, string password);
        Task<bool> IsEmailConfirmedAsync(int userId);
        Task<string> GeneratePasswordResetTokenAsync(AppUser appUser);
        Task<IdentityResult> ResetPasswordAsync(AppUser appUser, string token, string newPassword);
        Task<IdentityResult> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
        Task<string> GenerateEmailConfirmationTokenAsync(AppUser appUser);
        Task<AppUser> GetByIdAsync(string userId);
        Task<IdentityResult> ConfirmEmailAsync(int userId, string token);
    }
}
