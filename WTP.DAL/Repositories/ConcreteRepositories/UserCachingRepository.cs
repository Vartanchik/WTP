/*
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using WTP.DAL.Entities;
using WTP.DAL.Repositories.ConcreteRepositories.AppUserExtended;
using WTP.DAL.Repositories.GenericRepository;
*/
namespace WTP.DAL.Repositories.ConcreteRepositories
{
    /*
     * MESSAGE FOR YOU !
     * 
     * Refactor this repository & its creating in UOW like UserRepository / RefreshTokenRepository or something like that ...
     * 
     */

    public class UserCachingRepository// : AppUserRepository, IRepository<AppUser>, IAppUserRepository
    {
        /*
        private readonly IDistributedCache _Cache;
        private readonly IAppUserRepository _baseRepositoryAccessor;

        public UserCachingRepository(IDistributedCache distributedCache, ApplicationDbContext context, UserManager<AppUser> userManager, Func<string, IAppUserRepository> baseRepositoryAccessor)
        :base (context, userManager)
        {
            _Cache = distributedCache;
            _baseRepositoryAccessor = baseRepositoryAccessor("BASE");
        }

        private async void RemoveUserFromCacheAsync(int id)
        {
            await _Cache.RemoveAsync(id.ToString());
        }

        public override async Task<AppUser> GetAsync(int id)
        {
            string value = await _Cache.GetStringAsync(id.ToString());

            if (value != null)
            {
                return JsonConvert.DeserializeObject<AppUser>(value);
            }
                
            else
            {
                AppUser currentUser = await _baseRepositoryAccessor.GetAsync(id);

                if (currentUser.EmailConfirmed)
                {
                    string CurrentUserStringObject = JsonConvert.SerializeObject(currentUser, Formatting.Indented,
                    new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                    await _Cache.SetStringAsync(id.ToString(), CurrentUserStringObject,
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(600)
                    });
                }

                return currentUser;
            }
        }

        public new async Task<IdentityResult> UpdateAsync(AppUser appUser)
        {
            var resultOfBaseUpdate = await _baseRepositoryAccessor.UpdateAsync(appUser);

            RemoveUserFromCacheAsync(appUser.Id);

            return resultOfBaseUpdate;
        }

        public new async Task<IdentityResult> ResetPasswordAsync(AppUser appUser, string token, string newPassword)
        {
            var resultOfBaseResetPasswordAsync = await _baseRepositoryAccessor.ResetPasswordAsync(appUser, token, newPassword);

            RemoveUserFromCacheAsync(appUser.Id);

            return resultOfBaseResetPasswordAsync;
        }

        public new async Task<IdentityResult> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            var ResultOfBaseChangePasswordAsync = await _baseRepositoryAccessor.ChangePasswordAsync(userId, currentPassword, newPassword);

            RemoveUserFromCacheAsync(userId);

            return ResultOfBaseChangePasswordAsync;

        }
        */
    }
}
