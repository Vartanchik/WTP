using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.WebApi.WTP.DAL.DomainModels;
using WTP.WebApi.WTP.DAL.Services.AppUserService;

namespace WTP.WebApi.WTP.DAL.Services.AppUserServicea
{
    public class AppUserService : IAppUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationDbContext _applicationDbContext;

        public AppUserService(UserManager<AppUser> userManager, ApplicationDbContext applicationDbContext)
        {
            _userManager = userManager;
            _applicationDbContext = applicationDbContext;
        }
        
        public async Task<IdentityResult> CreateAsync(AppUser appUser, string password)
        {
            var result = await _userManager.CreateAsync(appUser, password);

            await _userManager.AddToRoleAsync(appUser, "User");

            return result;
        }
        
        public async Task<AppUser> GetAsync(string id)
        {
            return await _applicationDbContext.AppUsers.Include("Country").Include("Gender")
                .Include(_ => _.AppUserLanguages).FirstOrDefaultAsync(_ => _.Id == id);
        }
        
        public async Task<AppUser> GetByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
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
            user.Steam = user.Steam;

            var result = await _userManager.UpdateAsync(user);
            return result;
        }
        
        public async Task<IList<string>> GetRolesAsync(AppUser appUser)
        {
            return await _userManager.GetRolesAsync(appUser);
        }
        
        public async Task<bool> CheckPasswordAsync(string id, string password)
        {
            var appUser = await _userManager.FindByIdAsync(id);

            return await _userManager.CheckPasswordAsync(appUser, password);
        }

        public async Task<string> GetPasswordResetTokenAsync(AppUser applicationUser)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(applicationUser);
        }

        public async Task<bool> IsEmailConfirmedAsync(AppUser appUser)
        {
            return await _userManager.IsEmailConfirmedAsync(appUser);
        }

        public async Task<IdentityResult> ResetPasswordAsync(AppUser applicationUser, string token, string newPassword)
        {
            return await _userManager.ResetPasswordAsync(applicationUser, token, newPassword);
        }
    }
}
