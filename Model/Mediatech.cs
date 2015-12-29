using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWindowsMediaPlayer.Model
{
    public class Mediatech
    {
        Playlist Running = new Playlist();
        List<Media> Medias = new List<Media>();
        List<Playlist> Playlists = new List<Playlist>();
        // BDD

        public bool isMenuShown = true;
        public bool isBoderLess = false;
        public bool isFullScreen = false;
    }
}
