using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace NewEdenMonitor.Model
{
    public static class Paths
    {
        internal const string DbFileName = "NewEdenMonitor.sqlite";

        internal static readonly string DbLocation =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Offworld Mining Company");

        internal static readonly string DbFullPath = Path.Combine(DbLocation, DbFileName);
    }

    internal class EveContext : IDisposable
    {
        bool _disposed = false;

        private readonly SQLiteConnection _connection;
        private readonly ApiCacheHandler _apiCacheHandler;
        private readonly SavedCharacterHandler _savedCharacterHandler;

        internal EveContext()
        {
            _connection =
                new SQLiteConnection(
                    new SQLiteConnectionStringBuilder() {DataSource = Paths.DbFullPath, ForeignKeys = true}
                        .ConnectionString, true);
            _connection.Open();

            _apiCacheHandler = new ApiCacheHandler(this);
            _savedCharacterHandler = new SavedCharacterHandler(this);
        }

        internal SQLiteConnection Connection
        {
            get { return _connection; }
        }

        internal ApiCacheHandler ApiCacheHandler
        {
            get { return _apiCacheHandler; }
        }

        internal SavedCharacterHandler SavedCharacterHandler
        {
            get { return _savedCharacterHandler; }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _connection.Close();
                _connection.Dispose();
            }

            _disposed = true;
        }
    }
}
