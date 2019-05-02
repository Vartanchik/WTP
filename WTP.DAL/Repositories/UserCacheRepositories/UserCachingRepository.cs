using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
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
                return await Task.FromResult((AppUser)JsonConvert.DeserializeObject(value));
            else
                return await base.GetAsync(id);
        }

        //save
        //public async Task SetObjectAsync<T>(string key, T value)
        //{
        //    await _Cache.SetStringAsync(key, JsonConvert.SerializeObject(value));
        //}

        //get
        //public async Task<T> GetObjectAsync<T>(string key)
        //{
        //    var value = await _Cache.GetStringAsync(key);
        //    return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        //}

        //remove object
        //public async Task RemoveObjectAsync(string key)
        //{
        //    await _Cache.RemoveAsync(key);
        //}

        //verify if an object exists
        //public async Task<bool> ExistObjectAsync<T>(string key)
        //{
        //    var value = await _Cache.GetStringAsync(key);
        //    return value == null ? false : true;
        //}

    }
}
