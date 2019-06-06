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

        public void setIMG()
        {
            if (!_isPlaying)
            {
                Image img = new Image();
                string path = string.Format("/Icon/pause.png");
                img.Source = new BitmapImage(new Uri(path, UriKind.Relative));
                img.Height = 40;
                img.Width = 40;

                playButton.Content = img;
            }
            else
            {
                Image img = new Image();
                string path = string.Format("/Icon/play.png");
                img.Source = new BitmapImage(new Uri(path, UriKind.Relative));
                img.Height = 40;
                img.Width = 40;

                playButton.Content = img;
            }
        }

        public static void SetNowPlaying(string song)
        {
            currentlyPlayingSong = song;
            _audio.Open(new Uri(song));
        }
        public static string GetNowPlaying()
        {
            return currentlyPlayingSong;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {

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

        public void Slider_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            _isDragging = true;
        }

        public void Slider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            _isDragging = false;
            _audio.Position = TimeSpan.FromSeconds(Slider.Value);
        }
        private void Check_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PlaylistListView.DataContext = _playlistList;
        }

        public void New_Click(object sender, RoutedEventArgs e)
        {
            var newplaylist = new AddPlaylistWindow();

            if (newplaylist.ShowDialog() == true)
            {
                Playlist newPlaylist = new Playlist(newplaylist.ListName, new BindingList<Info>());
                _playlistList.Add(newPlaylist);
                //PlaylistListView.ItemsSource = _playlistList;
                PlaylistListView.DataContext = _playlistList;
            }
        }

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

        //public int FindPlaylist(BindingList<Playlist> p, string name)
        //{
        //    int index = -1;
        //    for (int i = 0; i < p.Count; i++)
        //    {
        //        if (p[i].Name == name)
        //        {
        //            index = i;
        //            break;
        //        }
        //    }
        //    return index;
        //}

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
                        //Playlist items = (Playlist)PlaylistListView.Items.GetItemAt(index);
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

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            var screen = new Microsoft.Win32.OpenFileDialog();
            screen.Filter = "Text File|*.txt";
            screen.Multiselect = true;

            if (screen.ShowDialog() == true)
            {

            }
        }
    }
}