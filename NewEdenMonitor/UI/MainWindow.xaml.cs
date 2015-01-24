using eZet.EveLib.EveXmlModule;
using NewEdenMonitor.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Image = System.Windows.Controls.Image;

namespace NewEdenMonitor.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> _backgroundImages = new List<string>();
        private int _imageIndex = 1;

        readonly List<Control> _column1Controls = new List<Control>();
        readonly List<Control> _column2Controls = new List<Control>();
        List<Control> _column3Controls = new List<Control>();
        List<Control> _column4Controls = new List<Control>();

        public MainWindow()
        {
            InitializeComponent();
            InitializeImageList();
            InitializeWidgets();

            LoadBackgroundImage();

            KeyUp += OnKeyUp;
        }

        private void InitializeImageList()
        {
            _backgroundImages = Directory.EnumerateFiles("Images").Where(i => i.ToLower().EndsWith(".png")).ToList();
        }

        private void InitializeWidgets()
        {
            Column1.ItemsSource = _column1Controls;

            var serverStatusWidget = new ServerStatusWidget();
            serverStatusWidget.Margin = new Thickness(20, 20, 10, 10);
            _column1Controls.Add(serverStatusWidget);

            var playersOnlineWidget = new PlayersOnlineWidget();
            playersOnlineWidget.Margin = new Thickness(20, 10, 10, 10);
            _column1Controls.Add(playersOnlineWidget);

            var totalKillsWidget = new TotalKillsWidget();
            totalKillsWidget.Margin = new Thickness(20, 10, 10, 10);
            _column1Controls.Add(totalKillsWidget);

            Column2.ItemsSource = _column2Controls;

            using (var db = new EveContext())
            {
                var characters = db.SavedCharacterHandler.GetAll();

                foreach (var character in characters)
                {
                    var characterKey = EveXml.CreateCharacterKey(character.KeyId, character.VerificationCode);
                    //((EveXmlRequestHandler)characterKey.RequestHandler).Cache = SqliteCache.Instance;
                    characterKey.InitAsync().Wait();

                    var characterWidget = new CharacterWidget(characterKey, character.CharacterId);
                    characterWidget.Margin = new Thickness(20, 20, 10, 10);
                    _column2Controls.Add(characterWidget);
                }
            }
        }

        private void MenuItemAddCharacters_Click(object sender, RoutedEventArgs e)
        {
            var addApiKeyWindow = new AddApiKeyWindow();
            addApiKeyWindow.ShowDialog();
        }

        private void MenuItemExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F4:
                    _imageIndex++;

                    if (_imageIndex >= _backgroundImages.Count)
                    {
                        _imageIndex = 1;
                    }

                    SetBackgroundImage();

                    break;
                case Key.F11:
                    if (this.WindowStyle == WindowStyle.None)
                    {
                        MenuBar.Visibility = Visibility.Visible;
                        this.WindowStyle = WindowStyle.SingleBorderWindow;
                        this.ResizeMode = ResizeMode.CanResize;
                        this.WindowState = WindowState.Normal;
                    }
                    else
                    {
                        MenuBar.Visibility = Visibility.Hidden;
                        this.WindowStyle = WindowStyle.None;
                        this.ResizeMode = ResizeMode.NoResize;
                        this.WindowState = WindowState.Maximized;
                    }

                    break;
            }
        }

        private void MainWindowName_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveBackgroundImage(_backgroundImages[_imageIndex]);
        }

        #region Background Image

        private void SetBackgroundImage()
        {
            var myBrush = new ImageBrush();
            var image = new Image();
            image.Source = new BitmapImage(new Uri(Path.GetFullPath(_backgroundImages[_imageIndex])));
            myBrush.ImageSource = image.Source;
            MainWindowName.Background = myBrush;
        }

        private static void SaveBackgroundImage(string str)
        {
            using (var db = new EveContext())
            {
                db.SettingsHandler.Set(new Setting { Key = "backgroundImage", Value = str });
            }
        }

        private void LoadBackgroundImage()
        {
            using (var db = new EveContext())
            {
                var setting = db.SettingsHandler.Get("backgroundImage");

                if (setting != null)
                {
                    _imageIndex = _backgroundImages.IndexOf(setting.Value);
                }
            }

            if (_imageIndex < 0)
            {
                _imageIndex = 1;
            }

            SetBackgroundImage();
        }

        #endregion
    }
}
