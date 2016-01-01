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
        ViewModel.PlaylistViewModel PlaylistViewModel;
        ViewModel.PlaylistViewModel CurrentPlaylist;
        System.Timers.Timer Updater;

        public MainWindow()
        {
            InitializeComponent();

            MediatechViewModel = new ViewModel.MediatechViewModel();
            this.DataContext = MediatechViewModel;
            PlaylistViewModel = new ViewModel.PlaylistViewModel(MediatechViewModel.Medias);
            this.pnl_medias.DataContext = PlaylistViewModel;
            CurrentPlaylist = new ViewModel.PlaylistViewModel(MediatechViewModel.CurrentMedias);
            Updater = new System.Timers.Timer();
            Updater.AutoReset = true;
            Updater.Interval = 1000;
            Updater.Elapsed += Updater_Elapsed;
            Updater.Start();
        }

        ~MainWindow()
        {
            Updater.Stop();
        }

        private void Updater_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                if (me_player.NaturalDuration.HasTimeSpan)
                {
                    sldr_media_progress.Value = me_player.Position.TotalMilliseconds;
                    lbl_current_time.Content = me_player.Position.ToString();
                    //                    lbl_current_time.Content = String.Format("{0}:{1}", me_player.Position.Minutes, me_player.Position.Seconds);
                }
            }));
        }

        private void Element_MediaOpened(object sender, EventArgs e)
        {
            sldr_media_progress.Maximum = me_player.NaturalDuration.TimeSpan.TotalMilliseconds;
            lbl_total_time.Content = me_player.NaturalDuration.TimeSpan.ToString();
            //            lbl_total_time.Content = String.Format("{0}:{1}", me_player.NaturalDuration.TimeSpan.Minutes, me_player.NaturalDuration.TimeSpan.Seconds);
        }

        private void Element_MediaEnded(object sender, EventArgs e)
        {
            Media toPlay = CurrentPlaylist.NextSong();
            if (toPlay != null)
                me_player.Source = new Uri(toPlay.Path);
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

        private void lstbx_medias_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox lbox = (ListBox)e.Source;
            Model.Media selection = (Model.Media)lbox.SelectedItem;
            if (selection != null && PlaylistViewModel.CanAddMedia)
            {
                CurrentPlaylist.AddMedia(selection);
                if (CurrentPlaylist.CurrentlyPlaying == null)
                {
                    CurrentPlaylist.CurrentlyPlaying = selection;
                    me_player.Source = new Uri(CurrentPlaylist.CurrentlyPlaying.Path);
                }
            }
            //this.me_player.Source = new Uri(selection.Path);
            //this.me_player.Play();
            //MessageBox.Show(selection.Title);
        }

        private void lstbx_playlists_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox lbox = (ListBox)e.Source;
            Model.Playlist selection = (Model.Playlist)lbox.SelectedItem;
            this.pnl_medias.DataContext = new ViewModel.PlaylistViewModel(selection);
        }

        TimeSpan lastPosition = new TimeSpan();

        private void SeekToMediaPosition(object sender, EventArgs e)
        {
            if (me_player.NaturalDuration.HasTimeSpan && lastPosition != me_player.Position)
            {
                lastPosition = new TimeSpan(0, 0, 0, 0, (int)(sldr_media_progress.Value * me_player.NaturalDuration.TimeSpan.TotalMilliseconds / sldr_media_progress.Maximum));
                me_player.Position = lastPosition;
            }
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
            var tmp = new TimeSpan(0, 0, 0, 0, (int)(sldr_media_progress.Value * me_player.NaturalDuration.TimeSpan.TotalMilliseconds / sldr_media_progress.Maximum));
            lbl_current_time.Content = tmp.ToString();
        }

        private void SetVolume(object sender, EventArgs e)
        {
            if (me_player != null && me_player.HasAudio)
                me_player.Volume = sldr_volume.Value;
        }
    }
}
