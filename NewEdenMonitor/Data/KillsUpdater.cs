using eZet.EveLib.EveXmlModule;
using System.Threading.Tasks;
using System.Linq;
using System.Timers;

namespace NewEdenMonitor.Data
{
    internal class KillsUpdater
    {
        private Timer _timer;

        public KillsUpdater()
        {
            Task.Factory.StartNew(UpdateTask);
        }

        public void UpdateTask()
        {
            _timer = new Timer();
            _timer.Interval = 10*60*1000;
            _timer.Elapsed += Tick;
            _timer.Enabled = true;

            UpdateServerStatus();
        }

        public void Tick(object sender, ElapsedEventArgs args)
        {
            UpdateServerStatus();
        }

        public void UpdateServerStatus()
        {
            var kills = EveXml.Map.GetKills();

            lock (EveXmlData.Instance.Kills)
            {
                var killData = new KillData();
                killData.SolarSystems = kills.Result.SolarSystems;
                killData.ShipKills = killData.SolarSystems.Sum(s => s.ShipKills);
                killData.PodKills = killData.SolarSystems.Sum(s => s.PodKills);
                killData.TotalKills = killData.ShipKills + killData.PodKills;

                EveXmlData.Instance.Kills = killData;
            }
        }
    }
}
