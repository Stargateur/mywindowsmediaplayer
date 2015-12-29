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
            try
            {
                var rfile = new System.IO.FileStream(Path, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Read);
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
            }
            catch
            {
                xml = new XmlBDD.Xml();
            }
        }
        private void save()
        {
            var wfile = new System.IO.FileStream(Path, System.IO.FileMode.Open, System.IO.FileAccess.Write);
            xmlSerializer.Serialize(wfile, xml);
        }

        public void AddMedia(string PathMedia)
        {
            Int32 i = 0;

            while (i < xml.MediaList.Count())
            {
                if (xml.MediaList[i].Path == PathMedia)
                    return;
                if (xml.MediaList[i].Id != i)
                    break;
                i++;
            }
            while (i < xml.MediaList.Count())
                if (xml.MediaList[i].Path == PathMedia)
                    return;
            xml.MediaList.Insert(i, new Xml.Media(i, PathMedia));
        }

        public void AddMedia(string PathMedia, string NamePlaylist)
        {
            throw new NotImplementedException();
        }

        public void AddMedia(List<string> PathMedia, string NamePlaylist)
        {
            throw new NotImplementedException();
        }

        public void AddPlaylist(string NamePlaylist)
        {
            Int32 i = 0;

            while (i < xml.PlaylistList.Count())
            {
                if (xml.PlaylistList[i].Name == NamePlaylist)
                    return;
                if (xml.PlaylistList[i].Id != i)
                    break;
                i++;
            }
            while (i < xml.PlaylistList.Count())
                if (xml.PlaylistList[i].Name == NamePlaylist)
                    return;
            xml.PlaylistList.Insert(i, new Xml.Playlist(i, NamePlaylist));
        }

        public void AddPlaylist(string NamePlaylist, string PathMedia)
        {
            throw new NotImplementedException();
        }

        public void AddPlaylist(string NamePlaylist, List<string> PathMedia)
        {
            throw new NotImplementedException();
        }

        public void DeleteMedia(string PathMedia)
        {
            throw new NotImplementedException();
        }

        public void DeletePlaylist(string NamePlaylist)
        {
            throw new NotImplementedException();
        }

        public List<string> GetPlaylist()
        {
            throw new NotImplementedException();
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
                    var i = new List<Int64>();
                    foreach (var link in xml.LinkList)
                        if (link.Playlist == playlist.Id)
                            i.Add(link.Media);
                    foreach (var media in xml.MediaList)
                        if (media.Id == )
                    break;
                }
            return result;
        }

        public class Xml
        {
            public class Media
            {
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
                public Link(Int32 Media, Int32 Playlist)
                {
                    this.Media = Media;
                    this.Playlist = Playlist;
                }
                public Int32 Media;
                public Int32 Playlist;
            }
            public List<Link> LinkList;
        }
    }
}
