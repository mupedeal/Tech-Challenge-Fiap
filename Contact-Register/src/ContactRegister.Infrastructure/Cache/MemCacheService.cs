using ContactRegister.Application.Interfaces.Services;
using Microsoft.Extensions.Caching.Memory;

namespace ContactRegister.Infrastructure.Cache
{
    public class MemCacheService(IMemoryCache cache): ICacheService
	{
        private readonly IMemoryCache _cache = cache;

        public object? Get(string key) => _cache.TryGetValue(key, out var cachedValue) ? cachedValue : null;
        
        public void Remove(string key) => _cache.Remove(key);

        public void Set(string key, object value) => _cache.Set(key, value, TimeSpan.FromMinutes(10));
        
        public async Task<T?> GetOrCreateAsync<T>(string key, Func<Task<T>> func)
        {
            if (_cache.TryGetValue(key, out T? cachedValue))
            {
                return cachedValue!;
            }

            T newValue = await func();

            if (newValue is null)
                return default;

            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(5)
            };

            _cache.Set(key, newValue, cacheEntryOptions);

            return newValue;
        }
    }
}
