using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyWindowsMediaPlayer.Model
{
    public class Mediatech
    {
        public Playlist Running { get; } = new Playlist("En cours");
        public Playlist MediaList { get; } = new Playlist("Tous les médias");
        public ObservableCollection<Playlist> Playlists { get; } = new ObservableCollection<Playlist>();
        IBDD bdd = new XmlBDD();

        public bool isMenuShown = true;
        public bool isBoderLess = false;
        public bool isFullScreen = false;

        public Mediatech()
        {
            //foreach (var i in Enumerable.Range(0, 500))
            //    log.LogWindow.appendLog("TEST MEDIATECH" + i.ToString());
            bdd.AddMedia(System.IO.Path.GetFullPath(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), @"..\..\..\Music\allies_music1.mp3"))); // DEBUG
            bdd.AddPlaylist("Test1");
            bdd.AddMedia(System.IO.Path.GetFullPath(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), @"..\..\..\Music\allies_music1.mp3")), "Test1");
            var paths = bdd.GetMedia();
            foreach (string path in paths)
            {
                Log.LogWindow.appendLog("Addind " + path + " Media object");
                var media = new Media(path);
                MediaList.AddMedia(media);
            }
            Playlists.Add(MediaList);
            Playlists.Add(Running);
            var playlistsname = bdd.GetPlaylist();
            foreach (string playlistname in playlistsname)
            {
                var playlist = new Playlist(playlistname);
                Playlists.Add(playlist);
                var playlistpaths = bdd.GetMedia(playlistname);
                foreach (var playlistpath in playlistpaths)
                {
                    Media media = MediaList.Medias.Where(x => x.Path == playlistpath).First();
                    playlist.AddMedia(media);
                }
            }
        }
    }
}
