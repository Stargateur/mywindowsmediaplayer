using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWindowsMediaPlayer.Model
{
    interface IBDD
    {
        String Path
        {
            get;
            set;
        }
        void AddPlaylist(String NamePlaylist);
        void AddPlaylist(List<String> NamePlaylist);
        void AddPlaylist(String NamePlaylist, String PathMedia);
        void AddPlaylist(String NamePlaylist, List<String> PathMedia);
        void AddPlaylist(List<String> NamePlaylist, String PathMedia);
        void AddPlaylist(List<String> NamePlaylist, List<String> PathMedia);
        void DeletePlaylist(String NamePlaylist);
        void DeletePlaylist(List<String> NamePlaylist);
        void AddMedia(String PathMedia);
        void AddMedia(List<String> PathMedia);
        void AddMedia(String PathMedia, String NamePlaylist);
        void AddMedia(List<String> PathMedia, String NamePlaylist);
        void AddMedia(String PathMedia, List<String> NamePlaylist);
        void AddMedia(List<String> PathMedia, List<String> NamePlaylist);
        void DeleteMedia(String PathMedia);
        void DeleteMedia(List<String> PathMedia);
        List<String> GetPlaylist();
        List<String> GetMedia();
        List<String> GetMedia(String NamePlaylist);
    }
}
