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
            this.DataContext = CurrentPlaylist;
        }
    }
}
