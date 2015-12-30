using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWindowsMediaPlayer.ViewModel
{
    public class PlaylistViewModel
    {
        private Model.Playlist playlist;

        public ObservableCollection<Model.Media> Medias { get { return playlist.Medias; } }

        public PlaylistViewModel(Model.Playlist playlist)
        {
            this.playlist = playlist;
        }
    }
}
