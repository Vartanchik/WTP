using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WTP.DAL.Repositories.UserCacheRepositories
{
    public interface IUserCachingRepository
    {
        Task SetObjectAsync<T>(IDistributedCache cache, string key, T value);
        Task<T> GetObjectAsync<T>(IDistributedCache cache, string key);
        Task RemoveObjectAsync(IDistributedCache cache, string key);
        Task<bool> ExistObjectAsync<T>(IDistributedCache cache, string key);

    }
}
