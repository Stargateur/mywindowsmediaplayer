using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System;

namespace MyWindowsMediaPlayer.ViewModel
{
    public class MediatechViewModel : INotifyPropertyChanged
    {
        #region Members
        private Model.Mediatech mediatech;
        private OpenFileDialog fd = new OpenFileDialog();
        #endregion

        #region Properties
        public Model.Mediatech Mediatech { get; }
        public ICommand AddToMediatech { get; set; }

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
        #endregion

        public MediatechViewModel()
        {
            mediatech = Model.Mediatech.getInstance();
            AddToMediatech = new RelayCommand(AddNewMedia);
            fd.Title = "Select a media file";
            fd.Filter = "Media Files|*.mkv;*.wmv;*.mp3;*.wav;*.mp4;*.avi|Audio Files|*.mp3;*.wav|MP3 Files|*.mp3|WAV Files|*.wav|All Files|*.*";
            fd.FilterIndex = 1;
            fd.FileOk += Fd_FileOk;
        }

        private void Fd_FileOk(object sender, CancelEventArgs e)
        {
            if (this.fd.CheckFileExists)
            {
                try {
                    var media = new Model.Media(this.fd.FileName);
                    mediatech.AddMedia(media);
                    RaisePropertyChanged("Medias");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur lors de l'importation du média : " + ex.Message, "Impossible d'importer le média", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                }
            }
        }

        public void AddNewMedia(object obj)
        {
            fd.ShowDialog();
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
