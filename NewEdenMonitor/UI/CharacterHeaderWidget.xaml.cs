using eZet.EveLib.EveXmlModule;
using NewEdenMonitor.Data;
using System.Threading.Tasks;
using System.Windows.Controls;
using Image = eZet.EveLib.EveXmlModule.Image;

namespace NewEdenMonitor.UI
{
    /// <summary>
    /// Interaction logic for CharacterHeaderWidget.xaml
    /// </summary>
    public partial class CharacterHeaderWidget : UserControl
    {
        public CharacterHeaderWidget()
        {
            InitializeComponent();
        }

        public CharacterHeaderWidget(CharacterKey characterKey, long characterId) : this()
        {
            var dataAggregator = new DataAggregator();
            dataAggregator.CharacterInfoUpdater = CharacterInfoUpdater.Instance(characterKey, characterId);
            dataAggregator.CharacterSheetUpdater = CharacterSheetUpdater.Instance(characterKey, characterId);

            Task.Run(() => GetImageAsync(dataAggregator, characterId));

            this.DataContext = dataAggregator;
        }

        private async void GetImageAsync(DataAggregator dataAggregator, long characterId)
        {
            dataAggregator.CharacterImage =
                await EveXml.Image.GetCharacterPortraitDataAsync(characterId, Image.CharacterPortraitSize.X128);
        }

        internal class DataAggregator
        {
            public byte[] CharacterImage { get; set; }
            public CharacterInfoUpdater CharacterInfoUpdater { get; set; }
            public CharacterSheetUpdater CharacterSheetUpdater { get; set; }
        }
    }
}
