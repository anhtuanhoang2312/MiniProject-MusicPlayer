using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MiniProject_MusicPlayer
{
	class Info
	{
		private ImageSource _cover;
		private string _title;
		private string _artist;
		private string _filename;

		public ImageSource Cover
		{
			get
			{
				return _cover;
			}
			set
			{
				_cover = value;
			}
		}

		public string Title
		{
			get
			{
				return _title;
			}
			set
			{
				_title = value;
			}
		}

		public string Artist
		{
			get
			{
				return _artist;
			}
			set
			{
				_artist = value;
			}
		}

		public string FileName
		{
			get
			{
				return _filename;
			}
			set
			{
				_filename = value;
			}
		}

		public Info(ImageSource Cover, string Title, string Artist, string FileName)
		{
			this.Cover = Cover;
			this.Title = Title;
			this.Artist = Artist;
			this.FileName = FileName;
		}
	}
}
