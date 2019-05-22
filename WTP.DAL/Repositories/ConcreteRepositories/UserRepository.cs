using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.DAL.Entities;
using WTP.DAL.Repositories.GenericRepository;

namespace WTP.DAL.Repositories.ConcreteRepositories
{
    public class UserRepository<IEntity> : RepositoryBase<AppUser>, IUserRepository<AppUser>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context, UserManager<AppUser> userManager) : base(context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IdentityResult> CreateAsync(AppUser appUser, string password)
        {
            var result = await _userManager.CreateAsync(appUser, password);

            await _userManager.AddToRoleAsync(appUser, "User");

            return result;
        }

        public async Task<IdentityResult> UpdateAsync(AppUser appUser)
        {
            var user = await GetAsync(appUser.Id);

            user.AppUserLanguages = appUser.AppUserLanguages;
            user.Photo = appUser.Photo;
            user.UserName = appUser.UserName;
            user.GenderId = appUser.GenderId;
            user.DateOfBirth = appUser.DateOfBirth;
            user.CountryId = appUser.CountryId;
            user.Steam = appUser.Steam;

            return await _userManager.UpdateAsync(user);
        }

        public async Task<AppUser> GetByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<AppUser> GetByNameAsync(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        public async Task<IList<string>> GetRolesAsync(AppUser appUser)
        {
            return await _userManager.GetRolesAsync(appUser);
        }

        public async Task<bool> CheckPasswordAsync(int userId, string password)
        {
            var appUser = await _userManager.FindByIdAsync(userId.ToString());

            return await _userManager.CheckPasswordAsync(appUser, password);
        }

        public override async Task<AppUser> GetAsync(int userId)
        {
            var user = await _context.AppUsers.Include(x => x.Country).Include(x => x.Gender)
                .Include(userInc => userInc.AppUserLanguages).ThenInclude(a => a.Language)
                .FirstOrDefaultAsync(userInc => userInc.Id == userId);

            return user;
        }

        public async Task<bool> IsEmailConfirmedAsync(int userId)
        {
            var user = await GetAsync(userId);

            return await _userManager.IsEmailConfirmedAsync(user);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(AppUser appUser)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(appUser);
        }

        public async Task<IdentityResult> ResetPasswordAsync(AppUser appUser, string token, string newPassword)
        {
            var user = await GetAsync(appUser.Id);

            return await _userManager.ResetPasswordAsync(user,
                                                         token,
                                                         newPassword);
        }

        public async Task<IdentityResult> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            var user = await GetAsync(userId);

            return await _userManager.ChangePasswordAsync(user,
                                                          currentPassword,
                                                          newPassword);
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(AppUser appUser)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
        }

        public async Task<AppUser> FindByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(int userId, string token)
        {
            var user = await GetAsync(userId);

            return await _userManager.ConfirmEmailAsync(user, token);
        }

        private async Task<IdentityResult> CreatePersonAsync(AppUser appUser, string password, string role)
        {
            var result = await _userManager.CreateAsync(appUser, password);

            await _userManager.AddToRoleAsync(appUser, role);

            return result;
        }

        public async Task<IdentityResult> CreateAdminAsync(AppUser appUser, string password)
        {
            return await CreatePersonAsync(appUser, password, "Admin");
        }

        public async Task<IdentityResult> CreateModeratorAsync(AppUser appUser, string password)
        {
            return await CreatePersonAsync(appUser, password, "Moderator");
        }

        public new async Task<bool> DeleteAsync(int id)
        {
            var user = await GetAsync(id);

            if (user == null || user.IsDeleted)
                return false;

            user.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IList<AppUser>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            List<AppUser> result = new List<AppUser>();
            foreach (var t in users)
            {
                var role = await _userManager.GetRolesAsync(t);
                if (role.Contains("User"))
                    result.Add(t);
            }
            return result;

        }

        //TODO
        public async Task<bool> LockAsync(int id, int? days)
        {
            var user = await GetAsync(id);
            if (user == null)
                return false;

            await _userManager.SetLockoutEnabledAsync(user, true);

            if (days == null)
                await _userManager.SetLockoutEndDateAsync(user, null);
            else
            {
                if (days < 0)
                    days = 0;
                await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddDays(days.Value));
            }
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UnLockAsync(int id)
        {
            return await LockAsync(id, null);
        }


    }
}
