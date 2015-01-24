using System.ComponentModel;
using System.Runtime.CompilerServices;
using NewEdenMonitor.Annotations;
using eZet.EveLib.EveXmlModule;
using NewEdenMonitor.Data;
using NewEdenMonitor.Model;
using System.Threading.Tasks;
using System.Windows.Controls;

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
                await ImageCache.GetCharacterPortraitDataAsync(characterId, eZet.EveLib.EveXmlModule.Image.CharacterPortraitSize.X128);
        }

        internal class DataAggregator : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            private byte[] _characterImage;

            public byte[] CharacterImage
            {
                get { return _characterImage; }
                set
                {
                    _characterImage = value;
                    OnPropertyChanged();
                }
            }

            public CharacterInfoUpdater CharacterInfoUpdater { get; set; }
            public CharacterSheetUpdater CharacterSheetUpdater { get; set; }


            [NotifyPropertyChangedInvocator]
            private void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
