using eZet.EveLib.EveXmlModule;
using System.Threading.Tasks;
using System.Timers;

namespace NewEdenMonitor.Data
{
    public class ServerStatusUpdater
    {
        private Timer _timer;

        public ServerStatusUpdater()
        {
            Task.Factory.StartNew(UpdateTask);
        }

        public void UpdateTask()
        {
            _timer = new Timer();
            _timer.Interval = 1*60*1000;
            _timer.Elapsed += Tick;
            _timer.Enabled = true;

            UpdateServerStatus();
        }

        public static void Tick(object sender, ElapsedEventArgs args)
        {
            UpdateServerStatus();
        }

        public static void UpdateServerStatus()
        {
            var serverStatus = EveXml.Eve.GetServerStatusAsync();

            lock (EveXmlData.Instance.ServerStatus)
            {
                EveXmlData.Instance.ServerStatus = serverStatus.Result.Result;
            }
        }
    }
}
