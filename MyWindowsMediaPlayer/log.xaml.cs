using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MyWindowsMediaPlayer
{
    /// <summary>
    /// Logique d'interaction pour log.xaml
    /// </summary>
    public partial class log : Window
    {
        static log logWindow;
        public static log LogWindow {
            get
            {
                if (logWindow == null)
                    logWindow = new log();
                return logWindow;
            }
        }

        public log()
        {
            InitializeComponent();
            this.Show();
        }

        public void appendLog(String log)
        {
            this.debug.Text = this.debug.Text += "\n" + log;
        }

        public void closeLog()
        {
            if (logWindow != null)
                logWindow.Close();
        }
    }
}
