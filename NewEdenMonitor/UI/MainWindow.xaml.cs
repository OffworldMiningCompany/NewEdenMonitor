using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
        private int _imageIndex = 1;

        public MainWindow()
        {
            InitializeComponent();


            KeyUp += OnKeyUp;

            var list =  new List<Control>();
            Contents.ItemsSource = list;

            var serverStatusWidget = new ServerStatusWidget();
            serverStatusWidget.Margin = new Thickness(20, 20, 10, 10);
            list.Add(serverStatusWidget);

            var playersOnlineWidget = new PlayersOnlineWidget();
            playersOnlineWidget.Margin = new Thickness(20, 10, 10, 10);
            list.Add(playersOnlineWidget);

            var totalKillsWidget = new TotalKillsWidget();
            totalKillsWidget.Margin = new Thickness(20, 10, 10, 10);
            list.Add(totalKillsWidget);
        }

        public void CollectionChanged(Object source, NotifyCollectionChangedEventArgs e)
        {
        }

        private void MenuItemAddCharacters_Click(object sender, RoutedEventArgs e)
        {
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
                    var uriStr = string.Format("pack://application:,,,/Resources/bg{0}.png", _imageIndex);

                    var myBrush = new ImageBrush();
                    var image = new Image();
                    image.Source = new BitmapImage(new Uri(uriStr));
                    myBrush.ImageSource = image.Source;
                    MainWindowName.Background = myBrush;

                    _imageIndex++;

                    if (_imageIndex > 4)
                    {
                        _imageIndex = 1;
                    }

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
    }
}
