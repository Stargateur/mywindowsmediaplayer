using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System;
using System.Linq;
using System.Windows.Controls;

namespace MyWindowsMediaPlayer.ViewModel
{
    public class MediatechViewModel : INotifyPropertyChanged
    {
        #region Members
        private Model.Mediatech mediatech;
        private OpenFileDialog fd = new OpenFileDialog();
        private PlaylistViewModel currentPlaylist;
        private PlaylistViewModel playlistViewModel;

        private bool isPlayerShown = false;
        private bool isPlaylistShown = true;

        private String playPauseContent = "Play";
        #endregion

        #region Properties
        public Model.Mediatech Mediatech { get; }
        public PlaylistViewModel CurrentPlaylist
        {
            get { return currentPlaylist; }
        }
        public PlaylistViewModel PlaylistViewModel
        {
            get { return playlistViewModel; }
            set { playlistViewModel = value; RaisePropertyChanged("PlaylistViewModel"); }
        }
        public MediaElement Player { get; }
        public ICommand AddToMediatech { get; set; }
        public ICommand PlayMedia { get; set; }
        public ICommand SelectPlaylist { get; set; }
        public ICommand AddToCurrent { get; set; }
        public ICommand TogglePlayPause { get; set; }

        public bool isMenuShown
        {
            get { return mediatech.IsMenuShown; }
            set { mediatech.IsMenuShown = value; RaisePropertyChanged("isMenuShown"); }
        }
        public bool isFullScreen
        {
            get { return mediatech.IsFullScreen; }
            set { mediatech.IsFullScreen = value; RaisePropertyChanged("isFullScreen"); }
        }
        public bool IsPlayerShown
        {
            get { return isPlayerShown; }
            set { isPlayerShown = value; RaisePropertyChanged("IsPlayerShown"); }
        }
        public bool IsPlaylistShown
        {
            get { return isPlaylistShown; }
            set { isPlaylistShown = value; RaisePropertyChanged("IsPlaylistShown"); }
        }

        public String PlayPauseContent
        {
            get { return playPauseContent; }
            set { playPauseContent = value; RaisePropertyChanged("PlayPauseContent"); }
        }

        public ObservableCollection<Model.Playlist> Playlists
        {
            get { return mediatech.Playlists; }
        }
        public Model.Playlist Medias
        {
            get { return mediatech.MediaList; }
        }
        public Model.Playlist CurrentMedias
        {
            get { return mediatech.Running; }
        }
        public MediaViewModel CurrentMedia { get; } = new MediaViewModel();
        #endregion

        public MediatechViewModel()
        {
            mediatech = Model.Mediatech.getInstance();
            Player = new MediaElement();
            Player.LoadedBehavior = MediaState.Manual;
            Player.MediaEnded += Player_MediaEnded;

            AddToMediatech = new RelayCommand(AddNewMedia);
            PlayMedia = new RelayCommand(PlayNewMedia);
            SelectPlaylist = new RelayCommand(ShowPlaylist);
            AddToCurrent = new RelayCommand(AddMediaToCurrentPlaylist);
            TogglePlayPause = new RelayCommand(PlayPauseCurrentMedia);

            PlaylistViewModel = new PlaylistViewModel(Medias);
            currentPlaylist = new PlaylistViewModel(mediatech.Running);
            CurrentPlaylist.PropertyChanged += CurrentPlaylist_PropertyChanged;

            fd.Title = "Select a media file";
            String mediaExt = "";
            String audioExt = "";
            String videoExt = "";
            String imageExt = "";
            foreach (var ext in Model.Media.mediaExtensions)
            {
                if (mediaExt != "")
                    mediaExt += ";";
                mediaExt += "*" + ext.Key.ToLowerInvariant();
                switch (ext.Value)
                {
                    case Model.Media.Type.Audio:
                        if (audioExt != "")
                            audioExt += ";";
                        audioExt += "*" + ext.Key.ToLowerInvariant();
                        break;
                    case Model.Media.Type.Video:
                        if (videoExt != "")
                            videoExt += ";";
                        videoExt += "*" + ext.Key.ToLowerInvariant();
                        break;
                    case Model.Media.Type.Image:
                        if (imageExt != "")
                            imageExt += ";";
                        imageExt += "*" + ext.Key.ToLowerInvariant();
                        break;
                }
            }
            fd.Filter = "Media Files|"+ mediaExt + "|Audio Files|" + audioExt + "|Video Files|" + videoExt + "|Image Files|" + imageExt + "|All Files|*";
            fd.FilterIndex = 1;
            fd.FileOk += Fd_FileOk;
        }

        private void CurrentPlaylist_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //RaisePropertyChanged("CurrentMedia");
        }

        private void Fd_FileOk(object sender, CancelEventArgs e)
        {
            if (this.fd.CheckFileExists)
            {
                try {
                    if (mediatech.MediaList.Medias.Where(x => x.Path == fd.FileName).Count() == 0)
                    {
                        var media = new Model.Media(this.fd.FileName);
                        mediatech.AddMedia(media);
                        RaisePropertyChanged("Medias");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur lors de l'importation du média : " + ex.Message, "Impossible d'importer le média", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                }
            }
        }

        /// <summary>
        /// Display a FileChooser Dialog and then add it to the mediatech
        /// </summary>
        /// <param name="obj"></param>
        public void AddNewMedia(object obj)
        {
            fd.ShowDialog();
        }

        public void ShowPlaylist(object obj)
        {
            if (obj != null)
            {
                IsPlayerShown = false;
                IsPlaylistShown = true;
                Model.Playlist selection = (Model.Playlist)obj;
                PlaylistViewModel.Playlist = selection;
            }
        }

        public void PlayNewMedia(object obj)
        {
            if (obj != null)
            {   
                Model.Media selection = (Model.Media)obj;
                if (selection != null && PlaylistViewModel.CanAddMedia)
                {
                    CurrentPlaylist.ClearMedias();
                    CurrentPlaylist.AddMedia(selection);
                    if (CurrentPlaylist.CurrentlyPlaying == null)
                    {
                        CurrentPlaylist.CurrentlyPlaying = selection;
                        PlaySong(selection);
                        if (!selection.AudioOnly)
                        {
                            IsPlaylistShown = false;
                            IsPlayerShown = true;
                        }
                    }
                }
            }
        }

        public void AddMediaToCurrentPlaylist(object obj)
        {
            if (obj != null)
            {
                Model.Media selection = (Model.Media)obj;
                if (selection != null && PlaylistViewModel.CanAddMedia)
                {
                    CurrentPlaylist.AddMedia(selection);
                    if (CurrentPlaylist.CurrentlyPlaying == null)
                    {
                        CurrentPlaylist.CurrentlyPlaying = selection;
                        PlaySong(selection);
                    }
                }
            }
        }

        private void Player_MediaEnded(object sender, RoutedEventArgs e)
        {
            Model.Media toPlay = CurrentPlaylist.NextSong();
            if (toPlay != null)
                PlaySong(toPlay);
            else
            {
                isPlaying = true;
                PlayPauseCurrentMedia(null);
            }
        }

        bool isPlaying = false;
        public void PlayPauseCurrentMedia(object obj)
        {
            if (CurrentPlaylist.CurrentlyPlaying == null)
            {
                Model.Media media = CurrentPlaylist.NextSong();
                if (media != null)
                    Player.Source = new Uri(media.Path);
            }
            if (isPlaying == false)
            {
               Player.Play();
               PlayPauseContent = "Pause";
            }
            else
            {
                Player.Pause();
                PlayPauseContent = "Play";
            }
            isPlaying = !isPlaying;
        }

        private void PlaySong(Model.Media song)
        {
            if (song != null)
            {
                Player.Source = new Uri(song.Path);
                isPlaying = true;
                Player.Play();
                CurrentMedia.updateInfos(song);
            }
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
