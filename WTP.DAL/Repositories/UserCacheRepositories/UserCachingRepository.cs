using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WTP.DAL.Repositories.UserCacheRepositories
{
    public class UserCachingRepository : IUserCachingRepository
    {
        //save
        public async Task SetObjectAsync<T> (IDistributedCache cache, string key, T value)
        {
            await cache.SetStringAsync(key, JsonConvert.SerializeObject(value));
        }

        //get
        public async Task<T> GetObjectAsync<T> (IDistributedCache cache, string key)
        {
            var value = await cache.GetStringAsync(key);
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }

        //remove object
        public async Task RemoveObjectAsync (IDistributedCache cache, string key)
        {
            await cache.RemoveAsync(key);
        }

        //verify if an object exists
        public async Task<bool> ExistObjectAsync<T> (IDistributedCache cache, string key)
        {
            var value = await cache.GetStringAsync(key);
            return value == null ? false : true;
        }

    }
}
