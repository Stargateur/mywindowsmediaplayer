using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWindowsMediaPlayer.Model
{
    public class Playlist
    {
        #region Properties
        public String Name { get; set; }
        public ObservableCollection<Media> Medias { get; } = new ObservableCollection<Media>();
        #endregion

        /// <summary>
        /// Instancie un nouvel objet de la classe Playlist en prenant en paramètre un nom pour la playlist
        /// </summary>
        /// <param name="PlaylistName">Nom de la playlist</param>
        public Playlist(String PlaylistName)
        {
            Name = PlaylistName;
        }

        public void AddMedia(Model.Media media)
        {
            Medias.Add(media);
        }
    }
}
