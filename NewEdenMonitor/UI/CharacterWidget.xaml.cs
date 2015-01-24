using eZet.EveLib.EveXmlModule;
using System.Collections.Generic;
using System.Windows.Controls;

namespace NewEdenMonitor.UI
{
    /// <summary>
    /// Interaction logic for CharacterWidget.xaml
    /// </summary>
    public partial class CharacterWidget : UserControl
    {
        public CharacterWidget(CharacterKey characterKey, long characterId)
        {
            InitializeComponent();

            var list = new List<Control>();
            Contents.ItemsSource = list;

            var characterHeaderWidget = new CharacterHeaderWidget(characterKey, characterId);
            list.Add(characterHeaderWidget);

            var characterCorporationWidget = new CharacterCorporationWidget(characterKey, characterId);
            list.Add(characterCorporationWidget);

            var characterFactionWidget = new CharacterFactionWidget();
            list.Add(characterFactionWidget);
        }
    }
}
