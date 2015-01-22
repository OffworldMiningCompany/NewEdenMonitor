using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using NewEdenMonitor.Annotations;
using eZet.EveLib.EveXmlModule.Models;
using eZet.EveLib.EveXmlModule.Models.Map;
using eZet.EveLib.EveXmlModule.Models.Misc;

namespace NewEdenMonitor.Data
{
    public class KillData
    {
        public EveXmlRowCollection<Kills.SolarSystem> SolarSystems { get; set; }
        public int PodKills { get; set; }
        public int ShipKills { get; set; }
        public int TotalKills { get; set; }
    }

    public sealed class EveXmlData: INotifyPropertyChanged
    {
        private static volatile EveXmlData _instance;
        private static readonly object SyncRoot = new Object();

        private KillData _killData;
        private ServerStatus _serverStatus;

        public event PropertyChangedEventHandler PropertyChanged;

        private EveXmlData()
        {
            _killData = new KillData();
            _serverStatus = new ServerStatus();
        }

        public static EveXmlData Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                            _instance = new EveXmlData();
                    }
                }

                return _instance;
            }
        }

        public KillData Kills
        {
            get { return _killData; }
            set
            {
                _killData = value;
                OnPropertyChanged();
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
    }
}
