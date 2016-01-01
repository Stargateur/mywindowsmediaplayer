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

        public MainWindow()
        {
            InitializeComponent();

            MediatechViewModel = new ViewModel.MediatechViewModel();
            //MessageBox.Show(MediatechViewModel.Medias);
            this.DataContext = MediatechViewModel;
            PlaylistViewModel = new ViewModel.PlaylistViewModel(MediatechViewModel.Medias);
            this.pnl_medias.DataContext = PlaylistViewModel;
            CurrentPlaylist = new ViewModel.PlaylistViewModel(MediatechViewModel.CurrentMedias);
        }

        private void Element_MediaOpened(object sender, EventArgs e)
        {
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
            if (selection != null)
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

        private void SeekToMediaPosition(object sender, RoutedPropertyChangedEventArgs<double> args)
        {
            if (me_player.NaturalDuration.HasTimeSpan && lastPosition != me_player.Position)
            {
                lastPosition = new TimeSpan(0, 0, 0, 0, (int)(sldr_media_progress.Value * me_player.NaturalDuration.TimeSpan.TotalMilliseconds / 100));
                me_player.Position = lastPosition;
            }
        }

        private void SetToMediaPosition(object sender, EventArgs e)
        {
            if (me_player.NaturalDuration.HasTimeSpan)
            {
                sldr_media_progress.Value = me_player.Position.TotalMilliseconds * 100 / me_player.NaturalDuration.TimeSpan.TotalMilliseconds;
            }
        }

        private void SetVolume(object sender, RoutedPropertyChangedEventArgs<double> args)
        {
            if (me_player != null && me_player.HasAudio)
                me_player.Volume = sldr_volume.Value;
        }
    }
}
