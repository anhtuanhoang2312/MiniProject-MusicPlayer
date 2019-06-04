using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject_MusicPlayer
{
    public class Playlist
    {
        public string _name;
        public BindingList<Info> _song;

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public BindingList<Info> Song
        {
            get
            {
                return _song;
            }
            set
            {
                _song = value;
            }
        }
      
        public Playlist(string Name, BindingList<Info> Song)
        {
            this._name = Name;
            this._song = Song;
        }
    }
}
