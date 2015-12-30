using System;
using System.Collections.Generic;
using System.Linq;

namespace MyWindowsMediaPlayer.Model
{
    /// <summary>
    /// Inprémentation de IBDD avec un fichier Xml
    /// </summary>
    public class XmlBDD : IBDD
    {
        /// <summary>
        /// Sert à serialiser/déserialiser le Xml en se servant d'un stream et de la class Xml
        /// </summary>
        private System.Xml.Serialization.XmlSerializer xmlSerializer =
            new System.Xml.Serialization.XmlSerializer(typeof(XmlBDD.Xml));

        /// <summary>
        /// xml contiendra la BDD
        /// </summary>
        private XmlBDD.Xml xml;

        /// <summary>
        /// Path du fichier Xml pour charger et sauvegarder
        /// </summary>
        private String path;

        /// <summary>
        /// Instancie un nouvel objet de la classe XmlBDD ayant pour path %AppData%/%NameProject%/%NameProject%.xml
        /// </summary>
        public XmlBDD()
        {
            String applicationDataFolder =
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            String myWindowsMediaPlayer =
                System.IO.Path.Combine(applicationDataFolder,
                System.Reflection.Assembly.GetCallingAssembly().GetName().Name);

            System.IO.Directory.CreateDirectory(myWindowsMediaPlayer);

            path = System.IO.Path.Combine(myWindowsMediaPlayer,
                System.Reflection.Assembly.GetCallingAssembly().GetName().Name + ".xml");

            load();
        }

        /// <summary>
        /// Instancie un nouvel objet de la classe XmlBDD
        /// </summary>
        /// <param name="XmlPath">Path pour charger/sauvegarder le xml</param>
        public XmlBDD(String XmlPath)
        {
            path = System.IO.Path.GetFullPath(XmlPath);

            load();
        }

        /// <summary>
        /// Détruit un nouvel objet de la classe XmlBDD, necéssaire pour sauvegarder
        /// </summary>
        ~XmlBDD()
        {
            try
            {
                save();
            }
            catch
            { }
        }

        /// <summary>
        /// Destruis les liens en double, et les liens qui relie mêne vers des medias qui n'existe pas
        /// ou vers des playlists qui n'existe pas
        /// </summary>
        public void FixLink()
        {
            Int32 i = 0;
            Int32 j = 0;
            while (i < xml.LinkList.Count() && j < xml.MediaList.Count())
            {
                if (xml.MediaList[j].Id < xml.LinkList[i].IdMedia)
                {
                    j++;
                    continue;
                }
                else if (xml.MediaList[j].Id > xml.LinkList[i].IdMedia)
                    xml.LinkList.RemoveAt(i);
                else
                {
                    Int32 tmpI = i + 1;
                    while (tmpI < xml.LinkList.Count())
                        if (xml.LinkList[tmpI].IdMedia != xml.LinkList[i].IdMedia)
                            break;
                        else if (xml.LinkList[tmpI].IdPlaylist == xml.LinkList[i].IdPlaylist)
                            xml.LinkList.RemoveAt(tmpI);
                        else
                            tmpI++;
                    Int32 k = 0;
                    while (k < xml.PlaylistList.Count())
                    {
                        if (xml.PlaylistList[k].Id < xml.LinkList[i].IdPlaylist)
                        {
                            k++;
                            continue;
                        }
                        else if (xml.PlaylistList[k].Id > xml.LinkList[i].IdPlaylist)
                            xml.LinkList.RemoveAt(i);
                        else
                            i++;
                        break;
                    }
                }
            }
            while (i < xml.LinkList.Count())
                xml.LinkList.RemoveAt(i);
        }

        /// <summary>
        /// Détruit les medias en doublons et les medias qui ont le même Id
        /// </summary>
        private void FixMedia()
        {
            Int32 i = 0;
            while (i < xml.MediaList.Count())
            {
                Int32 j = i + 1;
                while (j < xml.MediaList.Count())
                    if (xml.MediaList[j].Id == xml.MediaList[i].Id)
                        xml.MediaList.RemoveAt(j);
                    else if (xml.MediaList[j].Path == xml.MediaList[i].Path)
                    {
                        Int32 k = 0;
                        while (k < xml.LinkList.Count())
                            if (xml.LinkList[k].IdMedia == xml.MediaList[j].Id)
                                xml.LinkList[k].IdMedia = xml.MediaList[i].Id;
                        xml.MediaList.RemoveAt(j);
                    }
                    else
                        j++;
                i++;
            }
        }

        /// <summary>
        /// Détruit les playlists en doublons et les medias qui ont le même Id
        /// </summary>
        private void FixPlaylist()
        {
            Int32 i = 0;
            while (i < xml.PlaylistList.Count())
            {
                Int32 j = i + 1;
                while (j < xml.PlaylistList.Count())
                    if (xml.PlaylistList[j].Id == xml.PlaylistList[i].Id)
                        xml.PlaylistList.RemoveAt(j);
                    else if (xml.PlaylistList[j].Name == xml.PlaylistList[i].Name)
                    {
                        Int32 k = 0;
                        while (k < xml.LinkList.Count())
                            if (xml.LinkList[k].IdPlaylist == xml.PlaylistList[j].Id)
                                xml.LinkList[k].IdPlaylist = xml.PlaylistList[i].Id;
                        xml.PlaylistList.RemoveAt(j);
                    }
                    else
                        j++;
                i++;
            }
        }

        /// <summary>
        /// En utilisant path comme chemin d'acces charge le fichier Xml dans la variable xml
        /// </summary>
        private void load()
        {
            var rfile = new System.IO.FileStream(path, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Read);
            try
            {
                xml = (XmlBDD.Xml)xmlSerializer.Deserialize(rfile);
                FixMedia();
                FixPlaylist();
                xml.MediaList.Sort(delegate (Xml.Media a, Xml.Media b)
                {
                    if (a.Id < b.Id)
                        return -1;
                    else if (a.Id > b.Id)
                        return 1;
                    else
                        return 0;
                });
                xml.PlaylistList.Sort(delegate (Xml.Playlist a, Xml.Playlist b)
                {
                    if (a.Id < b.Id)
                        return -1;
                    else if (a.Id > b.Id)
                        return 1;
                    else
                        return 0;
                });
                xml.LinkList.Sort(delegate (Xml.Link a, Xml.Link b)
                {
                    if (a.IdMedia < b.IdMedia)
                        return -1;
                    else if (a.IdMedia > b.IdMedia)
                        return 1;
                    else
                        return 0;
                });
                FixLink();
            }
            catch
            {
                xml = new XmlBDD.Xml();
            }
            rfile.Close();
        }

        /// <summary>
        /// En utilisant path comme chemin d'acces sauvegarde la variable xml dans le fichier Xml
        /// </summary>
        private void save()
        {
            var wfile = new System.IO.FileStream(path, System.IO.FileMode.Truncate, System.IO.FileAccess.Write);
            xmlSerializer.Serialize(wfile, xml);
            wfile.Close();
        }

        /// <summary>
        /// Cette fonction demande à la BDD de flush ses données
        /// </summary>
        public void Flush()
        {
            save();
        }

        /// <summary>
        /// Renvoie l'id du media en paramètre, si le media n'est pas présent il est créer
        /// </summary>
        /// <param name="PathMedia">Path du media</param>
        private Int32 GetIdMedia(string PathMedia)
        {
            Int32 i = 0;
            while (i < xml.MediaList.Count())
            {
                if (xml.MediaList[i].Path == PathMedia)
                    return i;
                if (xml.MediaList[i].Id != i)
                    break;
                i++;
            }

            Int32 j = i + 1;
            while (j < xml.MediaList.Count())
                if (xml.MediaList[j].Path == PathMedia)
                    return j;
                else
                    j++;

            xml.MediaList.Insert(i, new Xml.Media(i, PathMedia));

            return i;
        }

        /// <summary>
        /// Ajouter un media à la BDD, si le media est déjà présent ne fait rien
        /// </summary>
        /// <param name="PathMedia">Path du media à ajouter</param>
        public void AddMedia(string PathMedia)
        {
            GetIdMedia(PathMedia);
        }

        /// <summary>
        /// Ajoute un lien entre un media et une playlist, si le lien existe déjà ne fais rien
        /// </summary>
        /// <param name="idMedia">Id du media</param>
        /// <param name="idPlaylist">Id de la playlist</param>
        private void AddLink(Int32 idMedia, Int32 idPlaylist)
        {
            Int32 i = 0;
            while (i < xml.LinkList.Count())
                if (idMedia <= xml.LinkList[i].IdMedia)
                    break;
                else
                    i++;

            Int32 j = i;
            while (j < xml.LinkList.Count())
                if (idMedia != xml.LinkList[j].IdMedia)
                    break;
                else if (idPlaylist == xml.LinkList[j].IdPlaylist)
                    return;
                else
                    j++;

            xml.LinkList.Insert(i, new Xml.Link(idMedia, idPlaylist));
        }

        /// <summary>
        /// Ajoute le media à la playlist, si le media ou la playlist n'existe pas ils sont créés
        /// </summary>
        /// <param name="PathMedia">Path du media</param>
        /// <param name="NamePlaylist">Nom de la playlist</param>
        public void AddMedia(string PathMedia, string NamePlaylist)
        {
            Int32 idMedia = GetIdMedia(PathMedia);
            Int32 idPlaylist = GetIdPlaylist(NamePlaylist);

            AddLink(idMedia, idPlaylist);
        }

        /// <summary>
        /// Ajoute la liste de media à la playlist, si les medias ou la playlist n'existe pas ils sont créés
        /// </summary>
        /// <param name="PathMedia">Liste de path des medias</param>
        /// <param name="NamePlaylist">Nom de la playlist</param>
        public void AddMedia(List<string> PathMedia, string NamePlaylist)
        {
            Int32 idPlaylist = GetIdPlaylist(NamePlaylist);

            foreach (var pathMedia in PathMedia)
            {
                Int32 idMedia = GetIdMedia(pathMedia);

                AddLink(idMedia, idPlaylist);
            }
        }

        /// <summary>
        /// Renvoie l'id de la playlist en paramètre, si la playlist n'est pas présente elle est créé
        /// </summary>
        /// <param name="NamePlaylist">Nom de la playlist</param>
        private Int32 GetIdPlaylist(string NamePlaylist)
        {
            Int32 i = 0;

            while (i < xml.PlaylistList.Count())
            {
                if (xml.PlaylistList[i].Name == NamePlaylist)
                    return i;
                if (xml.PlaylistList[i].Id != i)
                    break;
                i++;
            }

            Int32 j = i + 1;
            while (j < xml.PlaylistList.Count())
                if (xml.PlaylistList[j].Name == NamePlaylist)
                    return j;
                else
                    j++;

            xml.PlaylistList.Insert(i, new Xml.Playlist(i, NamePlaylist));

            return i;
        }

        /// <summary>
        /// Ajoute la playlist, si la playlist existe ne fais rien
        /// </summary>
        /// <param name="NamePlaylist">Nom de la playlist</param>
        public void AddPlaylist(string NamePlaylist)
        {
            GetIdPlaylist(NamePlaylist);
        }

        /// <summary>
        /// Ajoute la playlist et insert le media dedans, si la playlist ou le media n'existent pas ils sont créés
        /// </summary>
        /// <param name="NamePlaylist">Nom de la playlist</param>
        /// <param name="PathMedia">Path du media</param>
        public void AddPlaylist(string NamePlaylist, string PathMedia)
        {
            AddMedia(PathMedia, NamePlaylist);
        }

        /// <summary>
        /// Ajoute la playlist et insert les medias dedans, si la playlist ou les medias n'existe pas ils sont créés
        /// </summary>
        /// <param name="NamePlaylist">Nom de la playlist</param>
        /// <param name="PathMedia">Liste de path des medias</param>
        public void AddPlaylist(string NamePlaylist, List<string> PathMedia)
        {
            AddMedia(PathMedia, NamePlaylist);
        }

        /// <summary>
        /// Destruit le media dans la BDD, si il n'existe pas ne fais rien
        /// </summary>
        /// <param name="PathMedia">Path du media</param>
        public void DeleteMedia(string PathMedia)
        {
            Int32 i = 0;
            while (i < xml.MediaList.Count())
                if (xml.MediaList[i].Path == PathMedia)
                {
                    Int32 j = 0;
                    while (j < xml.LinkList.Count())
                        if (xml.LinkList[j].IdMedia == xml.MediaList[i].Id)
                            xml.LinkList.RemoveAt(j);
                        else
                            j++;
                    xml.MediaList.RemoveAt(i);
                    return;
                }
                else
                    i++;
        }

        /// <summary>
        /// Destruit la playlist dans la BDD, si il n'existe pas ne fais rien
        /// </summary>
        /// <param name="NamePlaylist">Nom de la playlist</param>
        public void DeletePlaylist(string NamePlaylist)
        {
            Int32 i = 0;
            while (i < xml.PlaylistList.Count())
                if (xml.PlaylistList[i].Name == NamePlaylist)
                {
                    Int32 j = 0;
                    while (j < xml.LinkList.Count())
                        if (xml.LinkList[j].IdPlaylist == xml.PlaylistList[i].Id)
                            xml.LinkList.RemoveAt(j);
                        else
                            j++;
                    xml.PlaylistList.RemoveAt(i);
                    return;
                }
                else
                    i++;
        }

        /// <summary>
        /// Renvoie la liste de toute les playlists de la BDD
        /// </summary>
        /// <returns>Liste de nom des playlists</returns>
        public List<string> GetPlaylist()
        {
            var result = new List<String>();
            foreach (var playlist in xml.PlaylistList)
                result.Add(playlist.Name);
            return result;
        }

        /// <summary>
        /// Renvoie la liste de tout les medias de la BDD
        /// </summary>
        /// <returns>Liste de path des medias</returns>
        public List<string> GetMedia()
        {
            var result = new List<String>();
            foreach (var media in xml.MediaList)
                result.Add(media.Path);
            return result;
        }

        /// <summary>
        /// Renvoie la liste des medias d'une playlist, si elle n'existe pas la liste est vide
        /// </summary>
        /// <param name="NamePlaylist">Nom de la playlist</param>
        /// <returns>Liste de path des medias</returns>
        public List<string> GetMedia(string NamePlaylist)
        {
            var result = new List<String>();
            foreach (var playlist in xml.PlaylistList)
                if (playlist.Name == NamePlaylist)
                {
                    Int32 i = 0;
                    foreach (var link in xml.LinkList)
                        if (link.IdPlaylist == playlist.Id)
                            while (i < xml.MediaList.Count())
                            {
                                if (link.IdMedia < xml.MediaList[i].Id)
                                {
                                    i++;
                                    continue;
                                }
                                if (link.IdMedia == xml.MediaList[i].Id)
                                    result.Add(xml.MediaList[i].Path);
                                break;
                            }
                    break;
                }
            return result;
        }

        /// <summary>
        /// Class pour stocker les informations de la BDD
        /// </summary>
        public class Xml
        {
            /// <summary>
            /// Instancie tout les membres de la classe
            /// </summary>
            public Xml()
            {
                MediaList = new List<Xml.Media>();
                PlaylistList = new List<Xml.Playlist>();
                LinkList = new List<Xml.Link>();
            }

            /// <summary>
            /// Classe pour stocker un Media
            /// </summary>
            public class Media
            {
                /// <summary>
                /// Instancie tout les membres de la classe
                /// </summary>
                public Media() :
                    this(0, "")
                { }

                /// <summary>
                /// Instancie la classe
                /// </summary>
                /// <param name="Id">Id du media</param>
                /// <param name="Path">Path du media</param>
                public Media(Int32 Id, String Path)
                {
                    this.Id = Id;
                    this.Path = Path;
                }

                /// <summary>
                /// Id doit être unique dans la BDD
                /// </summary>
                public Int32 Id;

                /// <summary>
                /// Path du fichier media
                /// </summary>
                public String Path;
            }

            /// <summary>
            /// Liste des medias dans la BDD
            /// </summary>
            public List<Media> MediaList;

            /// <summary>
            /// Classe pour stocker une PLaylist
            /// </summary>
            public class Playlist
            {
                /// <summary>
                /// Instancie tout les membres de la classe
                /// </summary>
                public Playlist() :
                    this(0, "")
                { }

                /// <summary>
                /// Instancie la class
                /// </summary>
                /// <param name="Id">Id de la playlist</param>
                /// <param name="Name">Nom de la playlist</param>
                public Playlist(Int32 Id, String Name)
                {
                    this.Id = Id;
                    this.Name = Name;
                }

                /// <summary>
                /// Id de la playlist doit être unique
                /// </summary>
                public Int32 Id;

                /// <summary>
                /// Nom de la playlist
                /// </summary>
                public String Name;
            }

            /// <summary>
            /// Liste des playlist dans la BDD
            /// </summary>
            public List<Playlist> PlaylistList;

            /// <summary>
            /// Classe pour représenter le lien entre un media et une playlist
            /// </summary>
            public class Link
            {
                /// <summary>
                /// Instancie tout les membres de la classe
                /// </summary>
                public Link() :
                    this(0, 0)
                { }

                /// <summary>
                /// Intancie la classe
                /// </summary>
                /// <param name="IdMedia">Id du media</param>
                /// <param name="IdPlaylist">Id de la playlist</param>
                public Link(Int32 IdMedia, Int32 IdPlaylist)
                {
                    this.IdMedia = IdMedia;
                    this.IdPlaylist = IdPlaylist;
                }

                /// <summary>
                /// Id du media
                /// </summary>
                public Int32 IdMedia;

                /// <summary>
                /// Id de la playlist
                /// </summary>
                public Int32 IdPlaylist;
            }

            /// <summary>
            /// Liste des liens dans la BDD
            /// </summary>
            public List<Link> LinkList;
        }
    }
}