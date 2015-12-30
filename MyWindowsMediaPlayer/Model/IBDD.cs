using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWindowsMediaPlayer.Model
{
    /// <summary>
    /// Interface pour une BDD doit pour contenir des paths vers des medias. Des playlists pour regrouper
    /// plusieurs media.
    /// </summary>
    public interface IBDD
    {
        /// <summary>
        /// Ajouter un media à la BDD, si le media est déjà présent ne fait rien
        /// </summary>
        /// <param name="PathMedia">Path du media à ajouter</param>
        void AddMedia(String PathMedia);
        /// <summary>
        /// Ajoute le media à la playlist, si le media ou la playlist n'existe pas ils sont créés
        /// </summary>
        /// <param name="PathMedia">Path du media</param>
        /// <param name="NamePlaylist">Nom de la playlist</param>
        void AddMedia(String PathMedia, String NamePlaylist);
        /// <summary>
        /// Ajoute la liste de media à la playlist, si les medias ou la playlist n'existe pas ils sont créés
        /// </summary>
        /// <param name="PathMedia">Liste de path des medias</param>
        /// <param name="NamePlaylist">Nom de la playlist</param>
        void AddMedia(List<String> PathMedia, String NamePlaylist);
        /// <summary>
        /// Ajoute la playlist, si la playlist existe ne fais rien
        /// </summary>
        /// <param name="NamePlaylist">Nom de la playlist</param>
        void AddPlaylist(String NamePlaylist);
        /// <summary>
        /// Ajoute la playlist et insert le media dedans, si la playlist ou le media n'existent pas ils sont créés
        /// </summary>
        /// <param name="NamePlaylist">Nom de la playlist</param>
        /// <param name="PathMedia">Path du media</param>
        void AddPlaylist(String NamePlaylist, String PathMedia);
        /// <summary>
        /// Ajoute la playlist et insert les medias dedans, si la playlist ou les medias n'existe pas ils sont créés
        /// </summary>
        /// <param name="NamePlaylist">Nom de la playlist</param>
        /// <param name="PathMedia">Liste de path des medias</param>
        void AddPlaylist(String NamePlaylist, List<String> PathMedia);
        /// <summary>
        /// Destruit le media dans la BDD, si il n'existe pas ne fais rien
        /// </summary>
        /// <param name="PathMedia">Path du media</param>
        void DeleteMedia(String PathMedia);
        /// <summary>
        /// Destruit la playlist dans la BDD, si il n'existe pas ne fais rien
        /// </summary>
        /// <param name="NamePlaylist">Nom de la playlist</param>
        void DeletePlaylist(String NamePlaylist);
        /// <summary>
        /// Renvoie la liste de toute les playlists de la BDD
        /// </summary>
        /// <returns>Liste de nom des playlists</returns>
        List<String> GetPlaylist();
        /// <summary>
        /// Renvoie la liste de tout les medias de la BDD
        /// </summary>
        /// <returns>Liste de path des medias</returns>
        List<String> GetMedia();
        /// <summary>
        /// Renvoie la liste des medias d'une playlist, si elle n'existe pas la liste est vide
        /// </summary>
        /// <param name="NamePlaylist">Nom de la playlist</param>
        /// <returns>Liste de path des medias</returns>
        List<String> GetMedia(String NamePlaylist);
    }
}
