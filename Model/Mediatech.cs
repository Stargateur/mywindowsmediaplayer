using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var paths = bdd.GetMedia();
            foreach (string path in paths)
            {
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
