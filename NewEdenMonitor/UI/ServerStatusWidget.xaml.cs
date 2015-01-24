using System.Windows.Controls;

namespace NewEdenMonitor.UI
{
    /// <summary>
    /// Interaction logic for ServerStatusWidget.xaml
    /// </summary>
    public partial class ServerStatusWidget : UserControl
    {
        public ServerStatusWidget()
        {
            InitializeComponent();
            this.DataContext = Data.ServerStatusUpdater.Instance;
        }
    }
}
