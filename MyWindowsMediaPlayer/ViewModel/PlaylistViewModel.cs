using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWindowsMediaPlayer.ViewModel
{
    public class PlaylistViewModel : INotifyPropertyChanged
    {
        private Model.Playlist playlist;

        public ObservableCollection<Model.Media> Medias { get { return playlist.Medias; } }
        public Model.Media CurrentlyPlaying { get { return playlist.CurrentlyPlaying; } set { playlist.CurrentlyPlaying = value; } }

        public PlaylistViewModel(Model.Playlist playlist)
        {
            this.playlist = playlist;
        }

        public void AddMedia(Model.Media media)
        {
            playlist.AddMedia(media);
            RaisePropertyChanged("Medias");
        }

        public Model.Media NextSong()
        {
            Model.Media newSong = playlist.NextSong();
            RaisePropertyChanged("CurrentlyPlaying");
            return newSong;
        }

        #region INotifyPopertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
