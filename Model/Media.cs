using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyWindowsMediaPlayer.Model
{
    /// <summary>
    /// Représentation d'un média dans la médiatech
    /// </summary>
    class Media
    {
        static Dictionary<String, bool> mediaExtensions = new Dictionary<string, bool>() {
            { ".WAV", true }, { ".MID", true }, { ".MIDI", true }, { ".WMA", true }, { ".MP3", true }, { ".OGG", true }, { ".RMA", true },
            { ".AVI", false }, { ".MP4", false }, { ".DIVX", false }, { ".WMV", false }, { ".MKV", false }
        };

        public String Path { get; }
        public String Extention { get; } = "";

        public String Title { get; } = "";
        public String Author { get; } = "";
        public String Album { get; } = "";
        public String Genre { get; } = "";
        public String Composer { get; } = "";
        public int Date { get; } = 0;
        public TimeSpan Duration { get; } = new TimeSpan();

        public bool AudioOnly { get; }

        /// <summary>
        /// Instancie un nouvel objet de la classe Media en prenant en paramètre un chemin vers le média
        /// </summary>
        /// <param name="path">Chemin d'accès du média</param>
        public Media(String path)
        {
            Path = path;
            FileInfo fo = new FileInfo(Path);
            Path = path;
            Extention = fo.Extension;

            if (!fo.Exists)
                throw new FileNotFoundException("Couldn't access the file.", Path);
            if (!isMediaFile(Path))
                throw new FileFormatException("File " + fo.Name + " is not a media file");

            var ffProbe = new NReco.VideoInfo.FFProbe();
            var videoInfo = ffProbe.GetMediaInfo(Path);
            var tags = videoInfo.FormatTags;
            Duration = videoInfo.Duration;
            AudioOnly = mediaExtensions[this.Extention.ToUpperInvariant()];
            String stags = ""; // DEBUG
            foreach (KeyValuePair<string, string> entry in tags)
            {
                switch (entry.Key)
                {
                    case "album":
                        Album = entry.Value;
                        break;
                    case "genre":
                        Genre = entry.Value;
                        break;
                    case "title":
                        Title = entry.Value;
                        break;
                    case "artist":
                        Author = entry.Value;
                        break;
                    case "composer":
                        Composer = entry.Value;
                        break;
                    case "date":
                        int tmp;
                        if (int.TryParse(entry.Value, out tmp) == true)
                            Date = tmp;
                        break;
                }
                stags = stags + entry.Key + " => " + entry.Value + "\n";
            }
            //MessageBox.Show(stags); // DEBUG
        }

        public static bool isMediaFile(String path)
        {
            return mediaExtensions.ContainsKey(System.IO.Path.GetExtension(path).ToUpperInvariant());
        }
    }
}
