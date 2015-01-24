using eZet.EveLib.EveXmlModule;
using eZet.EveLib.EveXmlModule.Models.Misc;
using NewEdenMonitor.Annotations;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Timers;

namespace NewEdenMonitor.Data
{
    public class ServerStatusUpdater : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static volatile ServerStatusUpdater _instance;
        private static readonly object SyncRoot = new Object();

        private readonly Timer _timer;
        private ServerStatus _serverStatus;

        private ServerStatusUpdater()
        {
            _timer = new Timer();
            _timer.AutoReset = true;
            _timer.Elapsed += Tick;

            Task.Factory.StartNew(UpdateServerStatus);
        }

        public static ServerStatusUpdater Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new ServerStatusUpdater();
                        }
                    }
                }

                return _instance;
            }
        }

        public ServerStatus ServerStatus
        {
            get { return _serverStatus; }
            set
            {
                _serverStatus = value;
                OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Tick(object sender, ElapsedEventArgs args)
        {
            UpdateServerStatus();
        }

        private void UpdateServerStatus()
        {
            var serverStatus = EveXml.Eve.GetServerStatus();

            if (serverStatus.CachedUntil > DateTime.UtcNow)
            {
                _timer.Interval = (serverStatus.CachedUntil - DateTime.UtcNow).TotalMilliseconds + 30000;
            }
            else
            {
                _timer.Interval = 1*60*1000;
            }

            _timer.Start();

            ServerStatus = serverStatus.Result;
        }
    }
}