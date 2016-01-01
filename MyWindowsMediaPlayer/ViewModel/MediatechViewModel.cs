using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWindowsMediaPlayer.ViewModel
{
    public class MediatechViewModel : INotifyPropertyChanged
    {
        #region Members
        private Model.Mediatech mediatech;
        #endregion

        #region Properties
        public Model.Mediatech Mediatech { get; }

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
