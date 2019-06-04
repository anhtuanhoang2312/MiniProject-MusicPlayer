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

namespace MiniProject_MusicPlayer
{
    /// <summary>
    /// Interaction logic for MyMusicPage.xaml
    /// </summary>
    public partial class MyMusicPage : UserControl
    {
        public MyMusicPage()
        {
            InitializeComponent();
            if (BrowseListView.ItemsSource == null)
            {
                browseButton.Visibility = Visibility.Visible;
            }
        }

        void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            ImageSource Cover = null;
            string Title = null;
            string Artist = null;
            string Album = null;

            var screen = new Microsoft.Win32.OpenFileDialog();
            screen.Filter = "Audio File|*.mp3;*.m4a";
            screen.Multiselect = true;

            if (screen.ShowDialog() == true)
            {
                var filenames = screen.FileNames;

                browseButton.Visibility = Visibility.Hidden;
                BrowseListView.Visibility = Visibility.Visible;

                foreach (var filename in filenames)
                {
                    TagLib.File file = TagLib.File.Create(filename);

                    if (file.Tag.Pictures.Length >= 1)
                    {
                        TagLib.IPicture pic = file.Tag.Pictures[0];
                        System.IO.MemoryStream stream = new System.IO.MemoryStream(pic.Data.Data);
                        BitmapFrame bmp = BitmapFrame.Create(stream);
                        Cover = bmp;
                    }
                    else
                    {
                        BitmapImage bi = new BitmapImage();
                        bi.BeginInit();
                        bi.UriSource = new Uri("/Icon/MusicNote.png", UriKind.Relative);
                        bi.EndInit();
                        Cover = bi;
                    }

                    Title = file.Tag.Title;

                    if (file.Tag.AlbumArtists.Length >= 1)
                    {
                        Artist = file.Tag.AlbumArtists[0].ToString();
                    }
                    else
                    {
                        Artist = "Unknown";
                    }

                    Album = file.Tag.Album;

                    MainWindow._infoList.Add(new Info(Cover, Title, Artist, Album, filename));
                }

                BrowseListView.ItemsSource = MainWindow._infoList;
            }
        }

        private void PlayMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var menu = sender as MenuItem;
            var info = menu.DataContext as Info;

            MainWindow.SetNowPlaying(info.FileName);
            MainWindow._audio.Open(new Uri(info.FileName));
        }
    }
}
