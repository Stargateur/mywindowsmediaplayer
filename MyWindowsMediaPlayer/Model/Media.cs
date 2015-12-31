using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyWindowsMediaPlayer.Model
{
    public class Media
    {
        enum Type
        {
            Audio,
            Image,
            Video,
        }

        static Dictionary<String, Type> mediaExtensions = new Dictionary<String, Type>() {
        #region mediaExtensions
            { ".WAV", Type.Audio },
            { ".MP3", Type.Audio },
            { ".WMA", Type.Audio },
            { ".OGG", Type.Audio },
            { ".OGV", Type.Audio },
            { ".OGA", Type.Audio },
            { ".OGX", Type.Audio },
            { ".FLAC", Type.Audio },
            { ".ACC", Type.Audio },
            { ".MID", Type.Audio },
            { ".RMA", Type.Audio },
            { ".JPG", Type.Image},
            { ".TIFF", Type.Image},
            { ".PNG", Type.Image},
            { ".GIF", Type.Image},
            { ".PSD", Type.Image},
            { ".PSP", Type.Image},
            { ".BMP", Type.Image},
            { ".AVI", Type.Video },
            { ".WMV", Type.Video },
            { ".MOV", Type.Video },
            { ".DIVX", Type.Video },
            { ".XVID", Type.Video },
            { ".MKV", Type.Video },
            { ".MKA", Type.Video },
            { ".MKS", Type.Video },
            { ".MP4", Type.Video },
            { ".FLV", Type.Video },
            { ".RMVB", Type.Video },
            #endregion
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
            AudioOnly = mediaExtensions[this.Extention.ToUpperInvariant()] == Type.Audio;
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
                stags = stags + entry.Key + " => " + entry.Value + "\n"; // DEBUG
            }
            if (Title.Equals(""))
                Title = System.IO.Path.GetFileNameWithoutExtension(path);
            //MessageBox.Show(stags); // DEBUG
        }

        public static List<String> ScanDirectory(String Path)
        {
            var result = new List<String>();
            try
            {
                foreach (var file in Directory.EnumerateFiles(Path))
                    if (isMediaFile(file))
                        result.Add(file);
            }
            catch
            {
            }
            return result;
        }

        public static List<String> ScanDirectoryRecursive(String Path)
        {
            var result = ScanDirectory(Path);
            try
            {
                foreach (var directory in Directory.EnumerateDirectories(Path))
                    result.AddRange(ScanDirectoryRecursive(directory));
            }
            catch
            {
            }
            return result;
        }

        public static List<String> ScanDirectoryAll()
        {
            var result = new List<String>();
            try
            {
                foreach (var logicalDrive in Directory.GetLogicalDrives())
                    result.AddRange(ScanDirectoryRecursive(logicalDrive));
            }
            catch
            {
            }
            return result;
        }

        public static bool isMediaFile(String path)
        {
            return mediaExtensions.ContainsKey(System.IO.Path.GetExtension(path).ToUpperInvariant());
        }
    }
}
