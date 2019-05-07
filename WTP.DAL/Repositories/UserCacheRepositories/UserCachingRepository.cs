using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using WTP.DAL.DomainModels;
using WTP.DAL.Repositories.ConcreteRepositories.AppUserExtended;
using WTP.DAL.Repositories.GenericRepository;

namespace WTP.DAL.Repositories.UserCacheRepositories
{
    public class UserCachingRepository : AppUserRepository, IRepository<AppUser>, IAppUserRepository
    {
        private readonly IDistributedCache _Cache;

        public UserCachingRepository(IDistributedCache distributedCache, ApplicationDbContext context, UserManager<AppUser> userManager) :base (context, userManager)
        {
            _Cache = distributedCache;
        }

        public override async Task<AppUser> GetAsync(int id)
        {
            var value = await _Cache.GetStringAsync(id.ToString());

            if (value != null)
            {
                return JsonConvert.DeserializeObject<AppUser>(value);
            }
                
            else
            {
                AppUser currentUser = await base.GetAsync(id);

                await _Cache.SetStringAsync(id.ToString(), JsonConvert.SerializeObject(currentUser, Formatting.Indented,
                new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));

                return currentUser;
            }
        }

        public new async Task<IdentityResult> UpdateAsync(AppUser appUser)
        {
            var resultOfUpdate = await base.UpdateAsync(appUser);

            await _Cache.RemoveAsync(appUser.Id.ToString());

            return resultOfUpdate;
        }
    }
}
