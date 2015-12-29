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
        ViewModel.MediatechViewModel MediatechViewModel = new ViewModel.MediatechViewModel();

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = MediatechViewModel;
        }

        private void btn_play_Click(object sender, RoutedEventArgs e)
        {
            MediatechViewModel.isMenuShown = !MediatechViewModel.isMenuShown;
            //MessageBox.Show("new menu state: " + MediatechViewModel.isMenuShown.ToString());
        }
    }
}
