using System;
using System.Data;
using System.Threading.Tasks;

namespace NewEdenMonitor.Model
{
    internal class ApiCache
    {
        public string Uri { get; set; }
        public DateTime CacheTime { get; set; }
        public string Data { get; set; }
    }

    internal class ApiCacheHandler
    {
        private readonly EveContext _parent;

        internal ApiCacheHandler(EveContext parent)
        {
            _parent = parent;
        }

        internal ApiCache Get(Uri uri)
        {
            ApiCache apiCache = null;

            using (var command = _parent.Connection.CreateCommand())
            {
                command.CommandText = "SELECT uri, cache_time, data FROM api_cache WHERE uri = @uri;";
                var param = command.Parameters.Add("uri", DbType.String);
                param.Value = uri.ToString();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        apiCache = new ApiCache();

                        apiCache.Uri = reader["uri"].ToString();
                        apiCache.CacheTime = (DateTime) reader["cache_time"];
                        apiCache.Data = reader["data"].ToString();
                    }

                    reader.Close();
                }
            }

            return apiCache;
        }

        internal async Task<ApiCache> GetAsync(Uri uri)
        {
            return await Task.Run(() => Get(uri));
        }

        internal bool Add(ApiCache apiCache)
        {
            using (var command = _parent.Connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO api_cache VALUES (@uri, @cache_time, @data);";

                var param = command.Parameters.Add("uri", DbType.String);
                param.Value = apiCache.Uri.ToString();

                param = command.Parameters.Add("cache_time", DbType.DateTime);
                param.Value = apiCache.CacheTime;

                param = command.Parameters.Add("data", DbType.String);
                param.Value = apiCache.Data;

                int res = command.ExecuteNonQuery();

                if (res == 1)
                {
                    return true;
                }
            }

            return false;
        }

        internal async Task<bool> AddAsync(ApiCache apiCache)
        {
            return await Task.Run(() => Add(apiCache));
        }
    }
}
