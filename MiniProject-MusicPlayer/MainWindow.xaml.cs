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
using MiniProject_MusicPlayer.Class;
using System.IO;

namespace MiniProject_MusicPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public static MediaPlayer _audio = new MediaPlayer();
        public static DispatcherTimer _timer = new DispatcherTimer();
        public static BindingList<Info> _infoList = new BindingList<Info>();
        public static BindingList<Playlist> _playlistList = new BindingList<Playlist>();
		public static List<Info> _currentlyPlayingPlayList = new List<Info>();
        public static string currentlyPlayingSong = null;
        public static bool _isDragging = false;
        public static bool _isPlaying = false;
        public static bool _isExist = false;
        public static bool _isShuffle = false;
        public static bool _isRepeat = false;
        public static MyMusicPage mymusicpg = new MyMusicPage();
        public static PlaylistPage playlistpg = null;
        public static Check _check = new Check();

		public static BindingList<Info> _tempPlaylist = new BindingList<Info>();

        public MainWindow()
        {
            InitializeComponent();

            Control.Show(MainContent, mymusicpg);

            _check.PropertyChanged += Check_PropertyChanged;

            _timer.Interval = TimeSpan.FromSeconds(0);
            _timer.Tick += timer_Tick;

            Slider.ApplyTemplate();
            System.Windows.Controls.Primitives.Thumb thumb = (Slider.Template.FindName("PART_Track", Slider) as System.Windows.Controls.Primitives.Track).Thumb;
            thumb.MouseEnter += new MouseEventHandler(thumb_MouseEnter);
        }

        //property changed
        private void Check_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PlaylistListView.DataContext = _playlistList;
        }

        //timer
        public void timer_Tick(object sender, EventArgs e)
        {
            if (Current.Text == "00:00")
            {
                Image img = new Image();
                string path = string.Format("/Icon/pause.png");
                img.Source = new BitmapImage(new Uri(path, UriKind.Relative));
                img.Height = 40;
                img.Width = 40;

                playButton.Content = img;
            }

            if (!_isDragging)
            {
                Slider.Value = _audio.Position.TotalSeconds;
                Slider.Maximum = _audio.NaturalDuration.TimeSpan.TotalSeconds;
            }

            Current.Text = String.Format(_audio.Position.ToString(@"mm\:ss"));
            Total.Text = String.Format(_audio.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));

            if (_audio.Position == _audio.NaturalDuration.TimeSpan)
            {
                Current.Text = "00:00";
                Image img = new Image();
                string path = string.Format("/Icon/play.png");
                img.Source = new BitmapImage(new Uri(path, UriKind.Relative));
                img.Height = 40;
                img.Width = 40;

                playButton.Content = img;
            }
        }

        //slider
        public void thumb_MouseEnter(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed
                && e.MouseDevice.Captured == null)
            {
                // the left button is pressed on mouse enter
                // but the mouse isn't captured, so the thumb
                // must have been moved under the mouse in response
                // to a click on the track.
                // Generate a MouseLeftButtonDown event.
                MouseButtonEventArgs args = new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, MouseButton.Left);
                args.RoutedEvent = MouseLeftButtonDownEvent;
                (sender as System.Windows.Controls.Primitives.Thumb).RaiseEvent(args);
            }
        }
        public void Slider_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            _isDragging = true;
        }

        public void Slider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            _isDragging = false;
            _audio.Position = TimeSpan.FromSeconds(Slider.Value);
        }

        //get set
        public static string GetNowPlaying()
        {
            return currentlyPlayingSong;
        }

        public static void SetNowPlaying(string song)
        {
            currentlyPlayingSong = song;
            _audio.Open(new Uri(song));
            NowPlayingPage._check.ChangeNowplaying = true;
        }

        public void setIMG(Button btn, string path)
        {
            Image img = new Image();
            img.Source = new BitmapImage(new Uri(string.Format(path), UriKind.Relative));
            img.Height = 40;
            img.Width = 40;
            btn.Content = img;
        }

        //title bar
        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Maximize_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.WindowState = WindowState.Normal;
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //mouse click
        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MyListView.SelectedItems.Clear();
            PlaylistListView.SelectedItems.Clear();

            ListViewItem item = sender as ListViewItem;
            if (item != null)
            {
                item.IsSelected = true;
                MyListView.SelectedItem = item;
                PlaylistListView.SelectedItem = item;
            }
        }

        private void ListViewItem_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ListViewItem item = sender as ListViewItem;

            if (item != null && item.IsSelected)
            {
                if (item.Name == "MyMusic")
                {
                    Control.Show(MainContent, mymusicpg);
                }
                else if (item.Name == "NowPlaying")
                {
                    Control.Show(MainContent, new NowPlayingPage());
                }
                else if (item.Name == "NowPlaying")
                {
                    Control.Show(MainContent, new NowPlayingPage());
                }
                else
                {
                    int index = PlaylistListView.SelectedIndex;

                    if (index != -1)
                    {
                        Playlist items = item.Content as Playlist;
                        if (items != null)
                        {
                            _tempPlaylist = items._song;
                            playlistpg = new PlaylistPage(_tempPlaylist);
                            Control.Show(MainContent, playlistpg);
                        }
                    }
                }
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        //event
        private void _audio_MediaEnded(object sender, EventArgs e)
        {
            _isPlaying = false;
            _timer.Stop();
            Info nextSong = null;

            if (_isRepeat == true && _isShuffle == true)
            {
                Random rnd = new Random();

                if (PlaylistPage.indexes.Count == 0)
                {
                    PlaylistPage.refillIndexesList();
                }

                int position = rnd.Next(PlaylistPage.indexes.Count);

                nextSong = _currentlyPlayingPlayList[PlaylistPage.indexes[position]];

                PlaylistPage.indexes.RemoveAt(position);
            }
            else if (_isRepeat == true && _isShuffle == false)
            {
                for (int i = 0; i < _currentlyPlayingPlayList.Count; i++)
                {
                    if (currentlyPlayingSong == _currentlyPlayingPlayList[i].FileName)
                    {
                        if (i + 1 < _currentlyPlayingPlayList.Count)
                        {
                            nextSong = _currentlyPlayingPlayList[i + 1];
                        }
                        else if (i + 1 == _currentlyPlayingPlayList.Count)
                        {
                            nextSong = _currentlyPlayingPlayList[0];
                        }
                    }
                }
            }
            else if (_isRepeat == false && _isShuffle == true)
            {
                Random rnd = new Random();

                if (PlaylistPage.indexes.Count != 0)
                {
                    int position = rnd.Next(PlaylistPage.indexes.Count);

                    nextSong = PlaylistPage._Playlist[PlaylistPage.indexes[position]];

                    PlaylistPage.indexes.RemoveAt(position);
                }
            }
            else if (_isRepeat == false && _isShuffle == false)
            {
                for (int i = 0; i < _currentlyPlayingPlayList.Count; i++)
                {
                    if (MainWindow.currentlyPlayingSong == _currentlyPlayingPlayList[i].FileName)
                    {
                        if (i + 1 < _currentlyPlayingPlayList.Count && PlaylistPage.startIndex != i + 1)
                        {
                            nextSong = _currentlyPlayingPlayList[i + 1];
                        }
                        else if (i + 1 == _currentlyPlayingPlayList.Count && PlaylistPage.startIndex != 0)
                        {
                            nextSong = _currentlyPlayingPlayList[0];
                        }
                    }
                }
            }

            if (nextSong != null)
            {
                MainWindow._audio.Close();
                MainWindow.SetNowPlaying(nextSong.FileName);
            }
        }

        private void _audio_MediaOpened(object sender, EventArgs e)
        {
            _audio.Play();
            _isPlaying = true;
            _timer.Start();

            foreach (var info in _currentlyPlayingPlayList)
            {
                if (info.FileName == MainWindow.currentlyPlayingSong)
                {
                    PlaylistPage.indexes.Remove(_currentlyPlayingPlayList.IndexOf(info));
                }
            }
        }

        //button
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_isPlaying)
            {
                Image img = new Image();
                string path = string.Format("/Icon/pause.png");
                img.Source = new BitmapImage(new Uri(path, UriKind.Relative));
                img.Height = 40;
                img.Width = 40;

                playButton.Content = img;

                _audio.Play();
                _isPlaying = true;
                _timer.Start();
            }
            else
            {
                Image img = new Image();
                string path = string.Format("/Icon/play.png");
                img.Source = new BitmapImage(new Uri(path, UriKind.Relative));
                img.Height = 40;
                img.Width = 40;

                playButton.Content = img;

                _audio.Pause();
                _isPlaying = false;
                _timer.Stop();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
			if(currentlyPlayingSong != null)
			{
				_audio.Stop();
				_timer.Stop();
				_audio.Close();
				string previousSong = null;

				if(_isShuffle == true)
				{
					Random rnd = new Random();

					if (PlaylistPage.indexes.Count == 0)
					{
						PlaylistPage.refillIndexesList();
					}

					int position = rnd.Next(PlaylistPage.indexes.Count);

					previousSong = PlaylistPage._Playlist[PlaylistPage.indexes[position]].FileName;

					PlaylistPage.indexes.RemoveAt(position);
				}
				else if (_isShuffle == false)
				{
					for (int i = 0; i < _currentlyPlayingPlayList.Count; i++)
					{
						if (currentlyPlayingSong == _currentlyPlayingPlayList[i].FileName)
						{
							if (i == 0)
							{
								previousSong = _currentlyPlayingPlayList[_currentlyPlayingPlayList.Count - 1].FileName;
							}
							else
							{
								previousSong = _currentlyPlayingPlayList[i - 1].FileName;
							}
						}
					}
				}
				SetNowPlaying(previousSong);
				_audio.MediaOpened += _audio_MediaOpened;
				_audio.MediaEnded += _audio_MediaEnded;
			}
        }	

		private void NextButton_Click(object sender, RoutedEventArgs e)
        {
			if (currentlyPlayingSong != null)
			{
				_audio.Stop();
				_timer.Stop();
				_audio.Close();
				string nextSong = null;

				if (_isShuffle == true)
				{
					Random rnd = new Random();

					if (PlaylistPage.indexes.Count == 0)
					{
						PlaylistPage.refillIndexesList();
					}

					int position = rnd.Next(PlaylistPage.indexes.Count);

					nextSong = PlaylistPage._Playlist[PlaylistPage.indexes[position]].FileName;

					PlaylistPage.indexes.RemoveAt(position);
				}
				else if (_isShuffle == false)
				{
					for (int i = 0; i < _currentlyPlayingPlayList.Count(); i++)
					{
						if (currentlyPlayingSong == _currentlyPlayingPlayList[i].FileName)
						{
							if (i + 1 == _currentlyPlayingPlayList.Count())
							{
								nextSong = _currentlyPlayingPlayList[0].FileName;
							}
							else
							{
								nextSong = _currentlyPlayingPlayList[i + 1].FileName;
							}
						}
					}
				}
				
				SetNowPlaying(nextSong);
				_audio.MediaOpened += _audio_MediaOpened;
				_audio.MediaEnded += _audio_MediaEnded;
			}
        }

        private void ShuffleButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_isShuffle)
            {
                Image img = new Image();
                string path = string.Format("/Icon/shuffle (1).png");
                img.Source = new BitmapImage(new Uri(path, UriKind.Relative));
                img.Height = 40;
                img.Width = 40;

                _isShuffle = true;

                shuffleButton.Content = img;
            }
            else
            {
                Image img = new Image();
                string path = string.Format("/Icon/shuffle.png");
                img.Source = new BitmapImage(new Uri(path, UriKind.Relative));
                img.Height = 40;
                img.Width = 40;

                _isShuffle = false;

                shuffleButton.Content = img;
            }
        }

        private void RepeatButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_isRepeat)
            {
                Image img = new Image();
                string path = string.Format("/Icon/repeat (1).png");
                img.Source = new BitmapImage(new Uri(path, UriKind.Relative));
                img.Height = 40;
                img.Width = 40;

                _isRepeat = true;

                repeatButton.Content = img;
            }
            else
            {
                Image img = new Image();
                string path = string.Format("/Icon/repeat.png");
                img.Source = new BitmapImage(new Uri(path, UriKind.Relative));
                img.Height = 40;
                img.Width = 40;

                _isRepeat = false;

                repeatButton.Content = img;
            }
        }

        public void New_Click(object sender, RoutedEventArgs e)
        {
            var newplaylist = new AddPlaylistWindow();

            if (newplaylist.ShowDialog() == true)
            {
                Playlist newPlaylist = new Playlist(newplaylist.ListName, new BindingList<Info>());
                _playlistList.Add(newPlaylist);
                PlaylistListView.DataContext = _playlistList;
            }
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            var screen = new Microsoft.Win32.OpenFileDialog();
            screen.Filter = "Data File|*.dat";
            screen.Multiselect = true;

            if (screen.ShowDialog() == true)
            {
                var filenames = screen.FileNames;

                foreach (var filename in filenames)
                {
                    var reader = new StreamReader(filename);

                    string name = System.IO.Path.GetFileNameWithoutExtension(filename);
                    BindingList<Info> list = new BindingList<Info>();

                    string line = "";
                    List<string> temp = new List<string>();
                    while ((line = reader.ReadLine()) != null)
                    {
                        temp.Add(line);

                        ImageSource Cover = null;
                        string Title = null;
                        string Artist = null;
                        string Album = null;

                        foreach (var item in temp)
                        {
                            TagLib.File file = TagLib.File.Create(item);

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
                                bi.UriSource = new Uri("/Icon/disc.png", UriKind.Relative);
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

                            list.Add(new Info(Cover, Title, Artist, Album, item));
                        }
                    }

                    reader.Close();

                    Playlist newPlaylist = new Playlist(name, list);
                    MainWindow._playlistList.Add(newPlaylist);
                    MainWindow._check.ChangePlaylist = true;
                }
            }               
        }

        private void SaveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var item = sender as MenuItem;
            BindingList<Playlist> items = item.DataContext as BindingList<Playlist>;

            if (items != null)
            {
                var writer = new StreamWriter(items[0].Name + ".dat");

                foreach (var song in items[0].Song)
                {
                    writer.WriteLine(song.FileName);
                }

                writer.Close();

                MessageBox.Show("Playlist saved successfully.", "Playlist Saved");
            }
        }

        private void RemoveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var item = sender as MenuItem;
            BindingList<Playlist> items = item.DataContext as BindingList<Playlist>;

            MainWindow._playlistList.Remove(items[0]);
            Control.Show(MainContent, mymusicpg);
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}