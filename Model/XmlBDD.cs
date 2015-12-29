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
        private void load()
        {
            var rfile = new System.IO.StreamReader(Path);
            xml = (XmlBDD.Xml)xmlSerializer.Deserialize(rfile);
        }
        private void save()
        {
            var wfile = new System.IO.StreamWriter(Path);
            xmlSerializer.Serialize(wfile, xml);
        }

        public void AddMedia(List<string> PathMedia)
        {
            throw new NotImplementedException();
        }

        public void AddMedia(string PathMedia)
        {
            throw new NotImplementedException();
        }

        public void AddMedia(List<string> PathMedia, string NamePlaylist)
        {
            throw new NotImplementedException();
        }

        public void AddMedia(List<string> PathMedia, List<string> NamePlaylist)
        {
            throw new NotImplementedException();
        }

        public void AddMedia(string PathMedia, List<string> NamePlaylist)
        {
            throw new NotImplementedException();
        }

        public void AddMedia(string PathMedia, string NamePlaylist)
        {
            throw new NotImplementedException();
        }

        public void AddPlaylist(List<string> NamePlaylist)
        {
            throw new NotImplementedException();
        }

        public void AddPlaylist(string NamePlaylist)
        {
            throw new NotImplementedException();
        }

        public void AddPlaylist(string NamePlaylist, List<string> PathMedia)
        {
            throw new NotImplementedException();
        }

        public void AddPlaylist(List<string> NamePlaylist, List<string> PathMedia)
        {
            throw new NotImplementedException();
        }

        public void AddPlaylist(List<string> NamePlaylist, string PathMedia)
        {
            throw new NotImplementedException();
        }

        public void AddPlaylist(string NamePlaylist, string PathMedia)
        {
            throw new NotImplementedException();
        }

        public void DeleteMedia(List<string> PathMedia)
        {
            throw new NotImplementedException();
        }

        public void DeleteMedia(string PathMedia)
        {
            throw new NotImplementedException();
        }

        public void DeletePlaylist(List<string> NamePlaylist)
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
            throw new NotImplementedException();
        }

        public List<string> GetMedia(string NamePlaylist)
        {
            throw new NotImplementedException();
        }

        public class Xml
        {
//            public Dictionary<Int64, String> media;
//            public Dictionary<Int64, String> playlist;
//            public List<Tuple<Int64, Int64>> link;
        }
    }
}
