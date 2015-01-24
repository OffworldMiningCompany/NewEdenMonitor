using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewEdenMonitor.Model
{
    public class Setting
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    internal class SettingsHandler
    {
        private readonly EveContext _db;

        internal SettingsHandler(EveContext db)
        {
            _db = db;
        }

        internal Setting Get(string key)
        {
            Setting setting = null;

            using (var command = _db.Connection.CreateCommand())
            {
                command.CommandText = "SELECT key, value FROM settings WHERE key = @key;";
                var param = command.Parameters.Add("key", DbType.String);
                param.Value = key;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        setting = ReadObject(reader);
                    }

                    reader.Close();
                }
            }

            return setting;
        }

        internal async Task<Setting> GetAsync(string key)
        {
            return await Task.Run(() => Get(key));
        }

        internal bool Set(Setting setting)
        {
            using (var command = _db.Connection.CreateCommand())
            {
                command.CommandText = "INSERT OR REPLACE INTO settings VALUES (@key, @value);";

                var param = command.Parameters.Add("key", DbType.String);
                param.Value = setting.Key;

                param = command.Parameters.Add("value", DbType.String);
                param.Value = setting.Value;

                int res = command.ExecuteNonQuery();

                if (res == 1)
                {
                    return true;
                }
            }

            return false;
        }

        internal async Task<bool> SetAsync(Setting setting)
        {
            return await Task.Run(() => Set(setting));
        }

        private Setting ReadObject(SQLiteDataReader reader)
        {
            var setting = new Setting();

            setting.Key = reader["key"].ToString();
            setting.Value = reader["value"].ToString();

            return setting;
        }
    }
}
