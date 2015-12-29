using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyWindowsMediaPlayer.Model
{
    class Media
    {
        static Dictionary<String, bool> mediaExtensions = new Dictionary<string, bool>() {
            { ".WAV", true }, { ".MID", true }, { ".MIDI", true }, { ".WMA", true }, { ".MP3", true }, { ".OGG", true }, { ".RMA", true },
            { ".AVI", false }, { ".MP4", false }, { ".DIVX", false }, { ".WMV", false }, { ".MKV", false }
        };

        public String Path { get; }
        public String Extention { get; }

        public String Title { get; }
        public String Author { get; }
        public String Album { get; }
        public DateTime Date { get; }
        public TimeSpan Duration { get; }

        public bool audioOnly { get; }

        public Media(String path)
        {
            FileInfo fo = new FileInfo(path);

            MessageBox.Show(System.Reflection.Assembly.GetExecutingAssembly().Location);
            if (!fo.Exists)
                throw new FileNotFoundException("Couldn't access the file.", path);
            if (!isMediaFile(path))
                throw new FileFormatException("File " + fo.Name + " is not a media file");
            this.Path = path;
            this.Title = System.IO.Path.GetFileNameWithoutExtension(fo.Name);
            this.Extention = fo.Extension;
            this.audioOnly = mediaExtensions[this.Extention.ToUpperInvariant()];
            MessageBox.Show(Path + "\n" + Title + "\n" + Extention + "\n" + audioOnly);

            var ffProbe = new NReco.VideoInfo.FFProbe();
            var videoInfo = ffProbe.GetMediaInfo(Path);
            var tags = videoInfo.FormatTags;
            string stags = "";
            foreach (KeyValuePair<string, string> entry in tags)
            {
                stags = stags + "\n" + entry.Key + " => " + entry.Value;
            }
            MessageBox.Show(videoInfo.Duration.ToString() + "\n" + videoInfo.FormatLongName + "\n" + videoInfo.FormatName + "\n" + stags);
        }

        public static bool isMediaFile(String path)
        {
            return mediaExtensions.ContainsKey(System.IO.Path.GetExtension(path).ToUpperInvariant());
        }
    }
}
