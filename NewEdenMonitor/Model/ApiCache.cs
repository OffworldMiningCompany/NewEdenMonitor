using System;
using System.Data;
using System.Data.SQLite;
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
        private readonly EveContext _db;

        internal ApiCacheHandler(EveContext db)
        {
            _db = db;
        }

        internal ApiCache Get(Uri uri)
        {
            ApiCache apiCache = null;

            using (var command = _db.Connection.CreateCommand())
            {
                command.CommandText = "SELECT uri, cache_time, data FROM api_cache WHERE uri = @uri;";
                var param = command.Parameters.Add("uri", DbType.String);
                param.Value = uri.ToString();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        apiCache = ReadObject(reader);
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

        internal bool Set(ApiCache apiCache)
        {
            using (var command = _db.Connection.CreateCommand())
            {
                command.CommandText = "INSERT OR REPLACE INTO api_cache VALUES (@uri, @cache_time, @data);";

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

        internal async Task<bool> SetAsync(ApiCache apiCache)
        {
            return await Task.Run(() => Set(apiCache));
        }

        private ApiCache ReadObject(SQLiteDataReader reader)
        {
            var apiCache = new ApiCache();

            apiCache.Uri = reader["uri"].ToString();
            apiCache.CacheTime = reader["cache_time"].ToDateTime();
            apiCache.Data = reader["data"].ToString();

            return apiCache;
        }
    }
}
