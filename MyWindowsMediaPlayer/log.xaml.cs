using System;
using System.Windows;

namespace MyWindowsMediaPlayer
{
    /// <summary>
    /// Logique d'interaction pour Log.xaml
    /// </summary>
    public partial class Log : Window
    {
        static Log logWindow;
        public static Log LogWindow {
            get
            {
                if (logWindow == null)
                    logWindow = new Log();
                return logWindow;
            }
        }

        private Log()
        {
            InitializeComponent();
            this.Show();
        }

        public void appendLog(String Log)
        {
            this.debug.Text += Log + "\n";
            this.Show();
        }
    }
}
