using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace MyWindowsMediaPlayer.Model
{
    public class Mediatech
    {
        public Playlist Running { get; } = new Playlist("En cours", false);
        public Playlist MediaList { get; } = new Playlist("Tous les médias", true);
        public ObservableCollection<Playlist> Playlists { get; } = new ObservableCollection<Playlist>();
        public bool IsMenuShown = true;
        public bool IsBoderLess = false;
        public bool IsFullScreen = false;

        private static Mediatech mediatech;
        private IBDD bdd = new XmlBDD();

        private Mediatech()
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
                var playlist = new Playlist(playlistname, true);
                Playlists.Add(playlist);
                var playlistpaths = bdd.GetMedia(playlistname);
                foreach (var playlistpath in playlistpaths)
                {
                    Media media = MediaList.Medias.Where(x => x.Path == playlistpath).First();
                    playlist.AddMedia(media);
                }
            }
        }

        public static Mediatech getInstance()
        {
            if (mediatech == null)
                mediatech = new Mediatech();
            return mediatech;
        }

        public void AddMedia(Model.Media media)
        {
            try
            {
                bdd.AddMedia(media.Path);
                MediaList.AddMedia(media);
            }
            catch
            { }
        }

        public void AddPlaylist(string name)
        {
            try
            {
                bdd.AddPlaylist(name);
                var playlist = new Playlist(name, true);
                Playlists.Add(playlist);
                Log.appenLog("new playlist");
            }
            catch
            { }
        }
    }
}
