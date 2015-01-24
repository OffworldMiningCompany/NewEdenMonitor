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
using System.Windows.Shapes;
using NewEdenMonitor.Model;
using eZet.EveLib.EveXmlModule;

namespace NewEdenMonitor.UI
{
    /// <summary>
    /// Interaction logic for AddApiKeyWindow.xaml
    /// </summary>
    public partial class AddApiKeyWindow : Window
    {
        public AddApiKeyWindow()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty CharacterName1Property =
            DependencyProperty.Register("CharacterName1", typeof (string),
                                        typeof (AddApiKeyWindow), new FrameworkPropertyMetadata(""));

        public static readonly DependencyProperty CharacterName2Property =
            DependencyProperty.Register("CharacterName2", typeof (string),
                                        typeof (AddApiKeyWindow), new FrameworkPropertyMetadata(""));

        public static readonly DependencyProperty CharacterName3Property =
            DependencyProperty.Register("CharacterName3", typeof (string),
                                        typeof (AddApiKeyWindow), new FrameworkPropertyMetadata(""));

        Character[] _characters;

        public int KeyId { get; set; }
        public string VerificationCode { get; set; }

        public string CharacterName1
        {
            get { return (string)GetValue(CharacterName1Property); }
            set { SetValue(CharacterName1Property, value); }
        }

        public string CharacterName2
        {
            get { return (string)GetValue(CharacterName2Property); }
            set { SetValue(CharacterName2Property, value); }
        }

        public string CharacterName3
        {
            get { return (string)GetValue(CharacterName3Property); }
            set { SetValue(CharacterName3Property, value); }
        }

        private void ButtonLoadCharacters_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var key = new ApiKey(KeyId, VerificationCode);

                if (key.KeyType == ApiKeyType.Corporation)
                {
                    MessageBox.Show(this, "Corporation keys are not supported.", "Wrong key type", MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                    return;
                }

                var characterKey = (CharacterKey) key.GetActualKey();
                _characters = characterKey.Characters.ToArray();

                int i;
                for (i = 0; i < _characters.Count(); i++)
                {
                    switch (i)
                    {
                        case 0:
                            CharacterName1 = _characters[i].CharacterName;
                            CheckBox1.Visibility = Visibility.Visible;
                            break;
                        case 1:
                            CharacterName2 = _characters[i].CharacterName;
                            CheckBox2.Visibility = Visibility.Visible;
                            break;
                        case 2:
                            CharacterName3 = _characters[i].CharacterName;
                            CheckBox3.Visibility = Visibility.Visible;
                            break;
                    }
                }

                AddCharactersButton.IsEnabled = true;
            }
            catch (AggregateException ex)
            {
                MessageBox.Show(ex.InnerException.Message, "Add API Key", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonAddCharacters_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new EveContext())
            {
                if (CheckBox1.IsChecked ?? false)
                {
                    db.SavedCharacterHandler.Set(new SavedCharacter
                        {
                            CharacterId = _characters[0].CharacterId,
                            Name = _characters[0].CharacterName,
                            KeyId = KeyId,
                            VerificationCode = VerificationCode
                        });
                }
                
                if (CheckBox2.IsChecked ?? false)
                {
                    db.SavedCharacterHandler.Set(new SavedCharacter
                    {
                        CharacterId = _characters[1].CharacterId,
                        Name = _characters[1].CharacterName,
                        KeyId = KeyId,
                        VerificationCode = VerificationCode
                    });
                }

                if (CheckBox2.IsChecked ?? false)
                {
                    db.SavedCharacterHandler.Set(new SavedCharacter
                    {
                        CharacterId = _characters[2].CharacterId,
                        Name = _characters[2].CharacterName,
                        KeyId = KeyId,
                        VerificationCode = VerificationCode
                    });
                }

                Close();
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
