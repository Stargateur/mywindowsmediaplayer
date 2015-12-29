using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyWindowsMediaPlayer.Model
{
    public class XmlBDD : IBDD
    {
        private String path;
        public String Path
        {
            get
            {
                return path;
            }

            set
            {
                save();
                path = value;
                load();
            }
        }

        private System.Xml.Serialization.XmlSerializer xmlSerializer;
        private XmlBDD.Xml xml;

        public XmlBDD()
        {
            xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(XmlBDD.Xml));
            String applicationDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            String myWindowsMediaPlayer = System.IO.Path.Combine(applicationDataFolder, Assembly.GetCallingAssembly().GetName().Name);
            System.IO.Directory.CreateDirectory(myWindowsMediaPlayer);
            path = System.IO.Path.Combine(myWindowsMediaPlayer, @"MyWindowsMediaPlayer.xml");
            load();
        }

        public XmlBDD(String XmlPath)
        {
            xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(XmlBDD.Xml));
            path = XmlPath;
            load();
        }

        ~XmlBDD()
        {
            save();
        }

        private void load()
        {
            var rfile = new System.IO.FileStream(Path, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Read);
            try
            {
                xml = (XmlBDD.Xml)xmlSerializer.Deserialize(rfile);
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
            }
            catch
            {
                xml = new XmlBDD.Xml();
                xml.MediaList = new List<Xml.Media>();
                xml.PlaylistList = new List<Xml.Playlist>();
                xml.LinkList = new List<Xml.Link>();
            }
            rfile.Close();
        }
        private void save()
        {
            var wfile = new System.IO.FileStream(Path, System.IO.FileMode.Truncate, System.IO.FileAccess.Write);
            xmlSerializer.Serialize(wfile, xml);
            wfile.Close();
        }

        public Int32 AddMedia(string PathMedia)
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

        public void AddMedia(string PathMedia, string NamePlaylist)
        {
            Int32 idMedia = AddMedia(PathMedia);
            Int32 idPlaylist = AddPlaylist(NamePlaylist);

            Int32 i = 0;
            while (i < xml.LinkList.Count())
                if (idMedia >= xml.LinkList[i].IdMedia)
                    break;
                else
                    i++;

            Int32 j = i;
            while (j < xml.LinkList.Count())
                if (idMedia != xml.LinkList[j].IdMedia)
                    break;
                else if (idPlaylist == xml.LinkList[j].IdMedia)
                    return;
                else
                    j++;

            xml.LinkList.Insert(i, new Xml.Link(idMedia, idPlaylist));
        }

        public void AddMedia(List<string> PathMedia, string NamePlaylist)
        {
            Int32 idPlaylist = AddPlaylist(NamePlaylist);

            foreach (var pathMedia in PathMedia)
            {
                Start:
                Int32 idMedia = AddMedia(pathMedia);

                Int32 i = 0;
                while (i < xml.LinkList.Count())
                    if (idMedia >= xml.LinkList[i].IdMedia)
                        break;
                    else
                        i++;

                Int32 j = i;
                while (j < xml.LinkList.Count())
                    if (idMedia != xml.LinkList[j].IdMedia)
                        break;
                    else if (idPlaylist == xml.LinkList[j].IdMedia)
                        goto Start;
                    else
                        j++;

                xml.LinkList.Insert(i, new Xml.Link(idMedia, idPlaylist));
            }
        }

        public Int32 AddPlaylist(string NamePlaylist)
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

        public void AddPlaylist(string NamePlaylist, string PathMedia)
        {
            AddMedia(PathMedia, NamePlaylist);
        }

        public void AddPlaylist(string NamePlaylist, List<string> PathMedia)
        {
            AddMedia(PathMedia, NamePlaylist);
        }

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

        public List<string> GetPlaylist()
        {
            var result = new List<String>();
            foreach (var playlist in xml.PlaylistList)
                result.Add(playlist.Name);
            return result;
        }

        public List<string> GetMedia()
        {
            var result = new List<String>();
            foreach (var media in xml.MediaList)
                result.Add(media.Path);
            return result;
        }

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

        public class Xml
        {
            public Xml()
            { }
            public class Media
            {
                public Media()
                { }
                public Media(Int32 Id, String Path)
                {
                    this.Id = Id;
                    this.Path = Path;
                }
                public Int32 Id;
                public String Path;
            }
            public List<Media> MediaList;
            public class Playlist
            {
                public Playlist()
                { }
                public Playlist(Int32 Id, String Name)
                {
                    this.Id = Id;
                    this.Name = Name;
                }
                public Int32 Id;
                public String Name;
            }
            public List<Playlist> PlaylistList;
            public class Link
            {
                public Link()
                { }
                public Link(Int32 IdMedia, Int32 IdPlaylist)
                {
                    this.IdMedia = IdMedia;
                    this.IdPlaylist = IdPlaylist;
                }
                public Int32 IdMedia;
                public Int32 IdPlaylist;
            }
            public List<Link> LinkList;
        }
    }
}
