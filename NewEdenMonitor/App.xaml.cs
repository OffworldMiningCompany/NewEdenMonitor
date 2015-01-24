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
        public App()
        {
            InitializeDatabase();

            ((EveXmlRequestHandler)EveXml.Eve.RequestHandler).Cache = SqliteCache.Instance;
            ((EveXmlRequestHandler)EveXml.Map.RequestHandler).Cache = SqliteCache.Instance;
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
