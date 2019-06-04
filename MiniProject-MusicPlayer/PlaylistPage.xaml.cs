using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for PlaylistPage.xaml
    /// </summary>
    public partial class PlaylistPage : UserControl
    {
        public BindingList<Info> _currentPlaylist = new BindingList<Info>();

        public BindingList<Info> CurrentPlaylist
        {
            get
            {
                return _currentPlaylist;
            }
            set
            {
                _currentPlaylist = value;
            }
        }

        public PlaylistPage()
        {
            InitializeComponent();
        }

        //public void BrowseButton_Click(object sender, RoutedEventArgs e)
        //{
        //    ListViewItem item = sender as ListViewItem;

        //    if (item != null && item.IsSelected)
        //    {
        //        if (item.Name == "MyMusic")
        //        {
        //            //Control.Show(MainContent, mymusicpg);
        //        }
        //    }

        //    var filenames = screen.FileNames;

        //    foreach (var filename in filenames)
        //    {
        //        TagLib.File file = TagLib.File.Create(filename);

        //        if (file.Tag.Pictures.Length >= 1)
        //        {
        //            TagLib.IPicture pic = file.Tag.Pictures[0];
        //            System.IO.MemoryStream stream = new System.IO.MemoryStream(pic.Data.Data);
        //            BitmapFrame bmp = BitmapFrame.Create(stream);
        //            //Cover = bmp;
        //        }
        //        else
        //        {
        //            BitmapImage bi = new BitmapImage();
        //            bi.BeginInit();
        //            bi.UriSource = new Uri("/Icon/MusicNote.png", UriKind.Relative);
        //            bi.EndInit();
        //            //Cover = bi;
        //        }

        //        Title = file.Tag.Title;

        //        if (file.Tag.AlbumArtists.Length >= 1)
        //        {
        //            Artist = file.Tag.AlbumArtists[0].ToString();
        //        }
        //        else
        //        {
        //            Artist = "Unknown";
        //        }

        //        Album = file.Tag.Album;

        //        MainWindow._infoList.Add(new Info(Cover, Title, Artist, Album, filename));
        //    }

        //    PlayListListView.ItemsSource = MainWindow._infoList;
        //}
    }
}
