using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NewEdenMonitor.Data;
using eZet.EveLib.EveXmlModule;

namespace NewEdenMonitor.UI
{
    /// <summary>
    /// Interaction logic for CharacterCorporationWidget.xaml
    /// </summary>
    public partial class CharacterCorporationWidget : UserControl
    {
        public CharacterCorporationWidget()
        {
            InitializeComponent();
        }

        public CharacterCorporationWidget(CharacterKey characterKey, long characterId)
            : this()
        {
            var dataAggregator = new DataAggregator();
            dataAggregator.CharacterInfoUpdater = CharacterInfoUpdater.Instance(characterKey, characterId);

            //Task.Run(() => GetImageAsync(dataAggregator, characterId));

            this.DataContext = dataAggregator;
        }

        //private async void GetImageAsync(DataAggregator dataAggregator, long characterId)
        //{
        //    dataAggregator.CharacterImage =
        //        await EveXml.Image.GetCorporationLogoDataAsync(characterId, eZet.EveLib.EveXmlModule.Image.CorporationLogoSize.X64);
        //}

        internal class DataAggregator
        {
            public byte[] CharacterImage { get; set; }
            public CharacterInfoUpdater CharacterInfoUpdater { get; set; }
        }
    }
}
