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
                        sldr_media_progress.Value = me_player.Position.TotalMilliseconds;
                    }));
            }
            catch
            { }
        }

        private void Element_MediaOpened(object sender, EventArgs e)
        {
            if (me_player.NaturalDuration.HasTimeSpan)
            {
                sldr_media_progress.Maximum = me_player.NaturalDuration.TimeSpan.TotalMilliseconds;
                lbl_total_time.Content = me_player.NaturalDuration.TimeSpan.ToString();
            }
            sldr_media_progress.Value = me_player.Position.TotalMilliseconds;
        }

        private void Element_MediaEnded(object sender, EventArgs e)
        {
            /*Media toPlay = CurrentPlaylist.NextSong();
            if (toPlay != null)
                me_player.Source = new Uri(toPlay.Path);
            else
            {
                isPlaying = false;
                btn_play.Content = "Play";
                me_player.Pause();
            }*/
        }

        bool isPlaying = false;

        private void btn_play_Click(object sender, RoutedEventArgs e)
        {
            //MediatechViewModel.isMenuShown = !MediatechViewModel.isMenuShown;

            if (isPlaying == false)
            {
                me_player.Play();
                btn_play.Content = "Pause";
            }
            else
            {
                btn_play.Content = "Play";
                me_player.Pause();
            }
            isPlaying = !isPlaying;
        }

        TimeSpan lastPosition = new TimeSpan();

        private void SeekToMediaPosition(object sender, EventArgs e)
        {
            if (me_player.NaturalDuration.HasTimeSpan)
            {
                lastPosition = new TimeSpan(0, 0, 0, 0, (int)(sldr_media_progress.Value * me_player.NaturalDuration.TimeSpan.TotalMilliseconds / sldr_media_progress.Maximum));
                me_player.Position = lastPosition;
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
            if (lastPosition != me_player.Position)
                SeekToMediaPosition(sender, e);
        }

        private void sldr_md_prgrss_ValueChanged(object sender, EventArgs e)
        {
            lbl_current_time.Content = me_player.Position.ToString();
        }

        private void SetVolume(object sender, EventArgs e)
        {
            if (me_player != null && me_player.HasAudio)
                me_player.Volume = sldr_volume.Value;
        }

    }
}
