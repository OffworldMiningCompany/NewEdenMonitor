using System.Windows.Controls;

namespace NewEdenMonitor.UI
{
    /// <summary>
    /// Interaction logic for ServerStatusWidget.xaml
    /// </summary>
    public partial class PlayersOnlineWidget : UserControl
    {
        public PlayersOnlineWidget()
        {
            InitializeComponent();
            this.DataContext = Data.ServerStatusUpdater.Instance;
        }
    }
}
