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

            Log.LogWindow.appendLog("55");
            MediatechViewModel = new ViewModel.MediatechViewModel();
            //MessageBox.Show(MediatechViewModel.Medias);
            this.DataContext = MediatechViewModel;
            PlaylistViewModel = new ViewModel.PlaylistViewModel(MediatechViewModel.Medias);
            this.pnl_medias.DataContext = PlaylistViewModel;
            CurrentPlaylist = new ViewModel.PlaylistViewModel(MediatechViewModel.CurrentMedias);
            me_player.MediaEnded += Me_player_MediaEnded;
        }

        private void btn_play_Click(object sender, RoutedEventArgs e)
        {
            MediatechViewModel.isMenuShown = !MediatechViewModel.isMenuShown;
            //MessageBox.Show("new menu state: " + MediatechViewModel.isMenuShown.ToString());
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

        private void Me_player_MediaEnded(object sender, RoutedEventArgs e)
        {
            Media toPlay = CurrentPlaylist.NextSong();
            if (toPlay != null)
                me_player.Source = new Uri(toPlay.Path);
        }
    }
}
