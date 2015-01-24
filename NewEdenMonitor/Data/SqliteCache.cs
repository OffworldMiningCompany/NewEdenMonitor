using NewEdenMonitor.Model;
using eZet.EveLib.Core.Cache;
using System;
using System.Threading.Tasks;

namespace NewEdenMonitor.Data
{
    class SqliteCache : IEveLibCache
    {
        private static volatile SqliteCache _instance;
        private static readonly object SyncRoot = new Object();

        public static SqliteCache Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                            _instance = new SqliteCache();
                    }
                }

                return _instance;
            }
        }

        public async Task StoreAsync(Uri uri, DateTime cacheTime, string data)
        {
            using (var db = new EveContext())
            {
                await db.ApiCacheHandler.SetAsync(new ApiCache
                    {
                        Uri = uri.ToString(),
                        CacheTime = cacheTime,
                        Data = data
                    });
            }
        }

        public async Task<string> LoadAsync(Uri uri)
        {
            ApiCache apiCache;

            using (var db = new EveContext())
            {
                apiCache = await db.ApiCacheHandler.GetAsync(uri);
            }

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
    }
}
