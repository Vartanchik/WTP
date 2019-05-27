using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using WTP.DAL.Entities.AppUserEntities;
using WTP.DAL.Repositories.ConcreteRepositories.AppUserRepository;
using WTP.DAL.UnitOfWork;

namespace WTP.DAL.Repositories.ConcreteRepositories
{
    /*
     * MESSAGE FOR YOU !
     * 
     * Refactor this repository & its creating in UOW like UserRepository / RefreshTokenRepository or something like that ...
     * 
     */

    public class UserCachingRepository : UserRepository
    {
        
        private readonly IDistributedCache _Cache;
        private readonly UserRepository _baseRepository;

        public UserCachingRepository(UserRepository baseRepository, IDistributedCache distributedCache, ApplicationDbContext context, UserManager<AppUser> userManager)
        :base (context, userManager)
        {
            _Cache = distributedCache;
            _baseRepository = baseRepository;
        }

        private async void RemoveUserFromCacheAsync(int id)
        {
            await _Cache.RemoveAsync(id.ToString());
        }

        public override async Task<AppUser> GetByIdAsync(int id)
        {
            string value = await _Cache.GetStringAsync(id.ToString());

            if (value != null)
            {
                return JsonConvert.DeserializeObject<AppUser>(value);
            }
                
            else
            {
                AppUser currentUser = await _baseRepository.GetByIdAsync(id);

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

        public override async Task<IdentityResult> UpdateAsync(AppUser appUser)
        {
            var resultOfBaseUpdate = await _baseRepository.UpdateAsync(appUser);

            RemoveUserFromCacheAsync(appUser.Id);

            return resultOfBaseUpdate;
        }

        public override async Task<IdentityResult> ResetPasswordAsync(AppUser appUser, string token, string newPassword)
        {
            var resultOfBaseResetPasswordAsync = await _baseRepository.ResetPasswordAsync(appUser, token, newPassword);

            RemoveUserFromCacheAsync(appUser.Id);

            return resultOfBaseResetPasswordAsync;
        }

        public override async Task<IdentityResult> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            var ResultOfBaseChangePasswordAsync = await _baseRepository.ChangePasswordAsync(userId, currentPassword, newPassword);

            RemoveUserFromCacheAsync(userId);

            return ResultOfBaseChangePasswordAsync;

        }
        
    }
}
