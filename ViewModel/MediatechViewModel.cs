using System;
using System.Collections.Generic;
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
        public Model.Mediatech Mediatech {
            get { return mediatech; }
            set { mediatech = value; }
        }

        public bool isMenuShown
        {
            get { return mediatech.isMenuShown; }
            set { mediatech.isMenuShown = value; RaisePropertyChanged("isMenuShown"); }
        }
        public bool isFullScreen
        {
            get { return mediatech.isFullScreen; }
            set { mediatech.isFullScreen = value; RaisePropertyChanged("isFullScreen"); }
        }
        #endregion

        public MediatechViewModel()
        {
            mediatech = new Model.Mediatech();
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
