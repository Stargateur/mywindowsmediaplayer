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
        public Media CurrentlyPlaying {
            get { return currentlyPlaying; }
            set
            {
                if (Medias.IndexOf(value) != -1)
                    currentlyPlaying = value;
            }
        }
        public int Length { get { return Medias.Count; } }
        public bool CanAddMedia { get; }
        #endregion

        #region
        private Media currentlyPlaying;
        #endregion

        /// <summary>
        /// Instancie un nouvel objet de la classe Playlist en prenant en paramètre un nom pour la playlist
        /// </summary>
        /// <param name="PlaylistName">Nom de la playlist</param>
        public Playlist(String PlaylistName, bool CanAddMedia)
        {
            Name = PlaylistName;
            this.CanAddMedia = CanAddMedia;
        }

        public void AddMedia(Model.Media media)
        {
            Medias.Add(media);
        }

        public void ClearMedias()
        {
            Medias.Clear();
        }

        public Media NextSong()
        {
            if (currentlyPlaying == null)
                currentlyPlaying = Medias.First();
            else
                currentlyPlaying = Medias.ElementAtOrDefault(Medias.IndexOf(currentlyPlaying) + 1);
            return currentlyPlaying;
        }
    }
}
