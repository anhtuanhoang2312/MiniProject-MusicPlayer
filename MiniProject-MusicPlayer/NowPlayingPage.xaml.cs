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
    /// Interaction logic for NowPlayingPage.xaml
    /// </summary>
    public partial class NowPlayingPage : UserControl
    {
        public NowPlayingPage()
        {
            InitializeComponent();
            if(MainWindow.currentlyPlayingSong != null)
			{
				TagLib.File file = TagLib.File.Create(MainWindow.GetNowPlaying());
				TagLib.IPicture pic = file.Tag.Pictures[0];
				System.IO.MemoryStream stream = new System.IO.MemoryStream(pic.Data.Data);
				BitmapFrame bmp = BitmapFrame.Create(stream);
				cover.Source = bmp;

				string title = file.Tag.Title;
				Title.Text = title;
				string[] artist = file.Tag.AlbumArtists;
				Artist.Text = "Artist \t" + artist[0].ToString();
				string album = file.Tag.Album;
				Album.Text = "Album \t" + album;
				string[] genre = file.Tag.Genres;
				Genre.Text = "Genre \t" + genre[0].ToString();
				uint year = file.Tag.Year;
				Year.Text = "Year \t" + year.ToString();
			}
        }
    }
}
