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
            this.DataContext = Data.EveXmlData.Instance;
        }
    }
}
