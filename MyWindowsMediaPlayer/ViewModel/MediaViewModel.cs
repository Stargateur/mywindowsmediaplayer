using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWindowsMediaPlayer.ViewModel
{
    public class MediaViewModel : INotifyPropertyChanged
    {
        public String Path { get; private set; } = "";
        public String Extention { get; private set; } = "";
        public String Title { get; private set; } = "";
        public String Author { get; private set; } = "";
        public String Album { get; private set; } = "";
        public String Genre { get; private set; } = "";
        public String Composer { get; private set; } = "";
        public int Date { get; private set; } = 0;
        public TimeSpan Duration { get; private set; } = new TimeSpan();

        public void updateInfos(Model.Media media)
        {
            Path = media.Path;
            Extention = media.Extention;
            Title = media.Title;
            Author = media.Author;
            Album = media.Album;
            Genre = media.Genre;
            Composer = media.Composer;
            Date = media.Date;
            Duration = media.Duration;
            RaisePropertyChanged("Path");
            RaisePropertyChanged("Extention");
            RaisePropertyChanged("Title");
            RaisePropertyChanged("Author");
            RaisePropertyChanged("Album");
            RaisePropertyChanged("Genre");
            RaisePropertyChanged("Composer");
            RaisePropertyChanged("Date");
            RaisePropertyChanged("Duration");
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
