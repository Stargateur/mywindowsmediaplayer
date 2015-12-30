﻿using System;
using System.Windows;

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

        private log()
        {
            InitializeComponent();
            this.Show();
        }

        public void appendLog(String log)
        {
            this.debug.Text = this.debug.Text += "\n" + log;
            this.Show();
        }

        public void closeLog()
        {
            if (logWindow != null)
                logWindow.Close();
        }
    }
}
