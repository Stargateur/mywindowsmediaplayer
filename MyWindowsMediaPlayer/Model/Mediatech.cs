using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyWindowsMediaPlayer.Model
{
    public class Mediatech
    {
        Playlist Running = new Playlist("Running");
        public List<Media> Medias = new List<Media>();
        List<Playlist> Playlists = new List<Playlist>();
        IBDD bdd = new XmlBDD();

        public bool isMenuShown = true;
        public bool isBoderLess = false;
        public bool isFullScreen = false;

        public Mediatech()
        {
            bdd.AddMedia(System.IO.Path.GetFullPath(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), @"..\..\..\Music\allies_music1.mp3"))); // DEBUG
            var paths = bdd.GetMedia();
            MessageBox.Show(paths.First());
            foreach (string path in paths)
            {
                System.Diagnostics.Debug.WriteLine("Addind " + path + " Media object");
                var media = new Media(path);
                Medias.Add(media);
            }
            var playlistsname = bdd.GetPlaylist();
            foreach (string playlistname in playlistsname)
            {
                var playlist = new Playlist(playlistname);
                Playlists.Add(playlist);
                var playlistpaths = bdd.GetMedia(playlistname);
                foreach (var playlistpath in playlistpaths)
                {
                    Media media = Medias.Find(r => r.Path == playlistpath);
                    playlist.addMedia(media);
                }
            }
        }
    }
}
