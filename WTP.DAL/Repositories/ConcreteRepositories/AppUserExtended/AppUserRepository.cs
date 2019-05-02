using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.DAL.DomainModels;
using WTP.DAL.Repositories.GenericRepository;

namespace WTP.DAL.Repositories.ConcreteRepositories.AppUserExtended
{
    public class AppUserRepository : RepositoryBase<AppUser>, IRepository<AppUser>, IAppUserRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationDbContext _context;

        public AppUserRepository(ApplicationDbContext context, UserManager<AppUser> userManager) : base(context)
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

        public async Task<IdentityResult> CreateAdminAsync(AppUser appUser, string password)
        {
            var result = await _userManager.CreateAsync(appUser, password);

            await _userManager.AddToRoleAsync(appUser, "Admin");

            return result;
        }

        public new async Task<bool> DeleteAsync(int id)
        {
            var user = await GetAsync(id);
            
            if (user==null || user.DeletedStatus)
                return false;

            user.DeletedStatus = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public new async Task<IdentityResult> UpdateAsync(AppUser appUser)
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

        public async Task<IList<string>> GetRolesAsync(AppUser appUser)
        {
            return await _userManager.GetRolesAsync(appUser);
        }

        public async Task<bool> CheckPasswordAsync(int id, string password)
        {
            var appUser = await _userManager.FindByIdAsync(id.ToString());

            return await _userManager.CheckPasswordAsync(appUser, password);
        }

        public override async Task<AppUser> GetAsync(int id)
        {
            return await _context.AppUsers.Include("Country").Include("Gender")
                .Include(_ => _.AppUserLanguages).FirstOrDefaultAsync(_ => _.Id == id);
        }

        public new async Task<IEnumerable<AppUser>> GetAllAsync()
        {
            return await _userManager.Users.ToListAsync();
            //return await GetAllAsync();
        }

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
