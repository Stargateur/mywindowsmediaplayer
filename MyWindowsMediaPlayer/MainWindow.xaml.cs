using MyWindowsMediaPlayer.Model;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyWindowsMediaPlayer
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ViewModel.MediatechViewModel MediatechViewModel;
        System.Timers.Timer Updater;

        public MainWindow()
        {
            InitializeComponent();

            MediatechViewModel = new ViewModel.MediatechViewModel();
            this.DataContext = MediatechViewModel;
            this.pnl_medias.DataContext = MediatechViewModel.PlaylistViewModel;
            Updater = new System.Timers.Timer();
            Updater.AutoReset = true;
            Updater.Interval = 200;
            Updater.Elapsed += Updater_Elapsed;
            Updater.Start();
        }

        ~MainWindow()
        {
            Updater.Stop();
        }

        private void Updater_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // Try rajouté pour gerer une exception lors de la fermeture durant une vidéo
            try
            {
                Dispatcher.Invoke(new Action(() =>
                    {
                        sldr_media_progress.Value = MediatechViewModel.Player.Position.TotalMilliseconds;
                        if (MediatechViewModel.Player.NaturalDuration.HasTimeSpan)
                        {
                            sldr_media_progress.Maximum = MediatechViewModel.Player.NaturalDuration.TimeSpan.TotalMilliseconds;
                            lbl_total_time.Content = MediatechViewModel.Player.NaturalDuration.TimeSpan.ToString();
                        }
                    }));
            }
            catch
            { }
        }

        TimeSpan lastPosition = new TimeSpan();

        private void SeekToMediaPosition(object sender, EventArgs e)
        {
            if (MediatechViewModel != null && MediatechViewModel.Player.NaturalDuration.HasTimeSpan)
            {
                lastPosition = new TimeSpan(0, 0, 0, 0, (int)(sldr_media_progress.Value * MediatechViewModel.Player.NaturalDuration.TimeSpan.TotalMilliseconds / sldr_media_progress.Maximum));
                MediatechViewModel.Player.Position = lastPosition;
            }
        }

        private void sldr_md_prgrss_MouseUp(object sender, EventArgs e)
        {
            SeekToMediaPosition(sender, e);
        }

        private void sldr_md_prgrss_DragStarted(object sender, EventArgs e)
        {
            Updater.Stop();
        }

        private void sldr_md_prgrss_DragCompleted(object sender, EventArgs e)
        {
            SeekToMediaPosition(sender, e);
            Updater.Start();
        }

        private void sldr_md_prgrss_DragDelta(object sender, EventArgs e)
        {
            if (MediatechViewModel != null && lastPosition != MediatechViewModel.Player.Position)
                SeekToMediaPosition(sender, e);
        }

        private void sldr_md_prgrss_ValueChanged(object sender, EventArgs e)
        {
            if (MediatechViewModel != null)
                lbl_current_time.Content = MediatechViewModel.Player.Position.ToString();
        }

        private void SetVolume(object sender, EventArgs e)
        {
            if (MediatechViewModel != null && MediatechViewModel.Player != null && MediatechViewModel.Player.HasAudio)
                MediatechViewModel.Player.Volume = sldr_volume.Value / sldr_volume.Maximum;
        }

    }
}
