using eZet.EveLib.EveXmlModule;
using eZet.EveLib.EveXmlModule.Models;
using eZet.EveLib.EveXmlModule.Models.Map;
using NewEdenMonitor.Annotations;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Timers;

namespace NewEdenMonitor.Data
{
    internal class KillData
    {
        public EveXmlRowCollection<Kills.SolarSystem> SolarSystems { get; set; }
        public int PodKills { get; set; }
        public int ShipKills { get; set; }
        public int TotalKills { get; set; }
    }

    internal class KillsUpdater : INotifyPropertyChanged
    {
        private static volatile KillsUpdater _instance;
        private static readonly object SyncRoot = new Object();

        private Kills _kills;
        private readonly Timer _timer;

        public event PropertyChangedEventHandler PropertyChanged;

        private KillsUpdater()
        {
            _kills = new Kills();

            _timer = new Timer();
            _timer.AutoReset = true;
            _timer.Elapsed += Tick;

            Task.Factory.StartNew(UpdateTask);
        }

        public static KillsUpdater Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new KillsUpdater();
                        }
                    }
                }

                return _instance;
            }
        }

        public Kills Kills
        {
            get { return _kills; }
            set
            {
                _kills = value;
                OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void UpdateTask()
        {
            UpdateServerStatus();
        }

        private void Tick(object sender, ElapsedEventArgs args)
        {
            UpdateServerStatus();
        }

        private void UpdateServerStatus()
        {
            var kills = EveXml.Map.GetKills();

            if (kills.CachedUntil > DateTime.UtcNow)
            {
                _timer.Interval = (kills.CachedUntil - DateTime.UtcNow).TotalMilliseconds;
            }
            else
            {
                _timer.Interval = 10*60*1000;
            }

            _timer.Start();

            Instance.Kills = kills.Result;
        }
    }
}
