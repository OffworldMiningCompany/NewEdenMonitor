using NewEdenMonitor.Annotations;
using NewEdenMonitor.Data;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace NewEdenMonitor.UI
{
    /// <summary>
    /// Interaction logic for ServerStatusWidget.xaml
    /// </summary>
    public partial class TotalKillsWidget : UserControl
    {
        public TotalKillsWidget()
        {
            InitializeComponent();

            var dataAggregator = new DataAggregator();
            dataAggregator.KillsUpdater = KillsUpdater.Instance;
            
            this.DataContext = dataAggregator;
        }

        internal class DataAggregator : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            private KillsUpdater _killsUpdater;
            private int _totalPodKills;
            private int _totalShipKills;
            private int _totalKills;

            public KillsUpdater KillsUpdater
            {
                get { return _killsUpdater; }
                set
                {
                    _killsUpdater = value;
                    _killsUpdater.PropertyChanged += KillsUpdaterOnPropertyChanged;
                    OnPropertyChanged();
                }
            }

            private void KillsUpdaterOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
            {
                TotalPodKills = _killsUpdater.Kills.SolarSystems.Sum(s => s.PodKills);
                TotalShipKills = _killsUpdater.Kills.SolarSystems.Sum(s => s.ShipKills);
                TotalKills = TotalPodKills + TotalShipKills;
            }

            public int TotalPodKills
            {
                get { return _totalPodKills; }
                set
                {
                    _totalPodKills = value;
                    OnPropertyChanged();
                }
            }

            public int TotalShipKills
            {
                get { return _totalShipKills; }
                set
                {
                    _totalShipKills = value;
                    OnPropertyChanged();
                }
            }

            public int TotalKills
            {
                get { return _totalKills; }
                set
                {
                    _totalKills = value;
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
        }
    }
}
