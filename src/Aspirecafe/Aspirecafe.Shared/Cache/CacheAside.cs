using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AspireCafe.Shared.Cache
{
    public class CacheAside
    {
        private readonly IDistributedCache _cache;

        public CacheAside(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<T> FetchFromCache<T>(string key, Func<Task<T>> builder, DistributedCacheEntryOptions options = null)
        {
            var cachedData = await _cache.GetAsync(key);
            if (cachedData != null)
            {
                return JsonSerializer.Deserialize<T>(cachedData);
            }
            var result = await builder();
            var serializedData = JsonSerializer.SerializeToUtf8Bytes(result);
            await _cache.SetAsync(key, serializedData, options);
            return result;
        }
    }
}
