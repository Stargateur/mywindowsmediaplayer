using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWindowsMediaPlayer.Model
{
    class Playlist
    {
        String Name { get; set; }
        List<Media> Medias = new List<Media>();
    }
}
