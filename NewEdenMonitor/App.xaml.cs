using NewEdenMonitor.Data;
using NewEdenMonitor.Model;
using NewEdenMonitor.Utils;
using eZet.EveLib.EveXmlModule;
using eZet.EveLib.EveXmlModule.RequestHandlers;
using System.IO;
using System.Windows;

namespace NewEdenMonitor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static ServerStatusUpdater _serverStatusUpdater = new ServerStatusUpdater();
        private static KillsUpdater _killsUpdater = new KillsUpdater();

        public App()
        {
            InitializeDatabase();

            ((EveXmlRequestHandler)EveXml.Eve.RequestHandler).Cache = new SqliteCache();
            ((EveXmlRequestHandler)EveXml.Map.RequestHandler).Cache = new SqliteCache();
        }

        private void InitializeDatabase()
        {
            if (!Directory.Exists(Paths.DbLocation))
            {
                Directory.CreateDirectory(Paths.DbLocation);
            }

            if (!File.Exists(Paths.DbFullPath))
            {
                byte[] database = Resource.GetEmbeddedResource(Paths.DbFileName);

                using (var writer = new BinaryWriter(File.Open(Paths.DbFullPath, FileMode.Create)))
                {
                    writer.Write(database);
                }
            }
        }
    }
}
