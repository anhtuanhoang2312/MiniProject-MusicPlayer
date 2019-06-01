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
using System.Windows.Threading;
using System.ComponentModel;

namespace MiniProject_MusicPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //static MainWindow()
        //{
        //    Telerik.Windows.Controls.StyleManager.ApplicationTheme = new Telerik.Windows.Controls.FluentTheme();
        //    Telerik.Windows.Controls.RadRibbonWindow.IsWindowsThemeEnabled = false;
        //}

        MediaPlayer _audio = new MediaPlayer();
        DispatcherTimer _timer = new DispatcherTimer();
		BindingList<Info> _infoList = null;
		string currentlyPlayingSong = null;
        bool _isDragging = false;
        bool _isPlaying = false;

        public MainWindow()
        {
            InitializeComponent();
            _timer.Interval = TimeSpan.FromSeconds(0);
            _timer.Tick += timer_Tick;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if(!_isDragging)
            {
                Slider.Value = _audio.Position.TotalSeconds;
                Slider.Maximum = _audio.NaturalDuration.TimeSpan.TotalSeconds;
            }
            Current.Text = String.Format(_audio.Position.ToString(@"mm\:ss"));
            Total.Text = String.Format(_audio.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
			ImageSource Cover = null;
			string Title = null;
			string Artist = null;

            var screen = new Microsoft.Win32.OpenFileDialog();
            screen.Filter = "Audio File|*.mp3";
            screen.Multiselect = true;

            if (screen.ShowDialog() == true)
            {
                var filenames = screen.FileNames;

				browseButton.Visibility = Visibility.Hidden;

				BrowseListView.Visibility = Visibility.Visible;

				_infoList = new BindingList<Info>();

				foreach(var filename in filenames)
				{
					TagLib.File file = TagLib.File.Create(filename);
					
					if(file.Tag.Pictures.Length >= 1)
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
					
					if(file.Tag.AlbumArtists.Length >= 1)
					{
						Artist = file.Tag.AlbumArtists[0].ToString();
					}
					else
					{
						Artist = "Unknown";
					}

					_infoList.Add(new Info(Cover, Title, Artist, filename));
				}

				BrowseListView.ItemsSource = _infoList;
            }
        }

		private void NowPlaying(string song)
		{
			currentlyPlayingSong = song;

			_audio.Open(new Uri(song));

			TagLib.File file = TagLib.File.Create(song);
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

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_isPlaying)
            {
                _audio.Play();
                _isPlaying = true;
                _timer.Start();
            }
            else
            {
                _audio.Pause();
                _isPlaying = false;
                _timer.Stop();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ShuffleButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RepeatButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Slider_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            _isDragging = true;
        }

        private void Slider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            _isDragging = false;
            _audio.Position = TimeSpan.FromSeconds(Slider.Value);
        }

		private void PlayMenuItem_Click(object sender, RoutedEventArgs e)
		{
			var menu = sender as MenuItem;
			var info = menu.DataContext as Info;

			NowPlaying(info.FileName);
		}
	}
}