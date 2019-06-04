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

namespace MiniProject_MusicPlayer
{
    /// <summary>
    /// Interaction logic for PlaylistNameWindow.xaml
    /// </summary>
    public partial class AddPlaylistWindow
    {
        public string ListName = "";

        public AddPlaylistWindow()
        {
            InitializeComponent();
        }

        private void CreateBtn_Click(object sender, RoutedEventArgs e)
        {
            ListName = NameBox.Text;
            this.DialogResult = true;
        }
    }
}
