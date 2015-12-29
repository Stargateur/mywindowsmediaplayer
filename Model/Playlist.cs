using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWindowsMediaPlayer.Model
{
    public class Playlist
    {
        #region Properties
        String Name { get; set; }
        private List<Media> Medias = new List<Media>();
        #endregion

        /// <summary>
        /// Instancie un nouvel objet de la classe Playlist en prenant en paramètre un nom pour la playlist
        /// </summary>
        /// <param name="PlaylistName">Nom de la playlist</param>
        public Playlist(String PlaylistName)
        {
            Name = PlaylistName;
        }

        public void addMedia(Model.Media media)
        {
            Medias.Add(media);
        }
    }
}
