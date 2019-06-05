using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject_MusicPlayer.Class
{
    public class Check : INotifyPropertyChanged
    {
        private bool _changePlaylist = false;
        public bool ChangePlaylist
        {
            get => _changePlaylist;
            set
            {
                _changePlaylist = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("_ChangePlaylist"));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
