using NewEdenMonitor.Model;
using eZet.EveLib.Core.Cache;
using System;
using System.Threading.Tasks;

namespace NewEdenMonitor.Data
{
    class SqliteCache : IEveLibCache
    {
        public async Task StoreAsync(Uri uri, DateTime cacheTime, string data)
        {
            await StoreDataAsync(uri, cacheTime, data);
        }

        public async Task<string> LoadAsync(Uri uri)
        {
            ApiCache apiCache = await LoadDataAsync(uri);

            if (apiCache != null && DateTime.UtcNow < apiCache.CacheTime)
            {
                return apiCache.Data;
            }

            return null;
        }

        public bool TryGetExpirationDate(Uri uri, out DateTime value)
        {
            using (var db = new EveContext())
            {
                var cache = db.ApiCacheHandler.Get(uri);
                
                if (cache != null)
                {
                    value = cache.CacheTime;
                    return true;
                }
            }

            value = DateTime.MinValue;
            return false;
        }

        private async Task StoreDataAsync(Uri uri, DateTime cacheTime, string data)
        {
            using (var db = new EveContext())
            {
                var obj = db.ApiCacheHandler.Get(uri);

                if (obj == null)
                {
                    await
                        db.ApiCacheHandler.AddAsync(new ApiCache
                            {
                                Uri = uri.ToString(),
                                CacheTime = cacheTime,
                                Data = data
                            });
                }
                else
                {
                    obj.CacheTime = cacheTime;
                    obj.Data = data;
                }
            }
        }

        private async Task<ApiCache> LoadDataAsync(Uri uri)
        {
            using (var db = new EveContext())
            {
                return await db.ApiCacheHandler.GetAsync(uri);
            }
        }
    }
}
