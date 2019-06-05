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
        private bool _isPlaying = false;
        public bool IsPlaying
        {
            get => _isPlaying;
            set
            {
                _isPlaying = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("IsPlaying"));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
