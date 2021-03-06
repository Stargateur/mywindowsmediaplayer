﻿using System;
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

        public Model.Playlist Playlist
        {
            get { return playlist; }
            set
            {
                if (playlist != null)
                    CurrentlyPlaying = null;
                playlist = value;
                RaisePropertyChanged("Playlist");
                RaisePropertyChanged("Medias");
            }
        }

        public ObservableCollection<Model.Media> Medias { get { return playlist.Medias; } }
        public Model.Media CurrentlyPlaying { get { return playlist.CurrentlyPlaying; } set { playlist.CurrentlyPlaying = value; RaisePropertyChanged("CurrentlyPlaying"); } }
        public bool CanAddMedia { get { return playlist.CanAddMedia; } }

        public PlaylistViewModel(Model.Playlist playlist)
        {
            this.Playlist = playlist;
        }

        public void AddMedia(Model.Media media)
        {
            playlist.AddMedia(media);
            RaisePropertyChanged("Medias");
        }

        public void ClearMedias()
        {
            playlist.ClearMedias();
            RaisePropertyChanged("Medias");
        }

        public Model.Media NextSong()
        {
            Model.Media newSong = playlist.NextSong();
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
