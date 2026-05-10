using FinalProject_Jenotan.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FinalProject_Jenotan.UI
{
    /// <summary>
    /// Interaction logic for TitlePage.xaml
    /// </summary>
    public partial class TitlePage : Page
    {

        public TitlePage()
        {
            InitializeComponent();
            DataContext = new TitlePageViewModel(this);
            TitleVideo.Play();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is TitlePageViewModel vm)
            {
                vm.Password = PasswordBox.Password;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            TitleVideo.Play();
        }

        private void TitleVideo_MediaEnded(object sender, RoutedEventArgs e)
        {
            var media = sender as MediaElement;
            media.Position = TimeSpan.Zero; // rewind
            media.Play(); // play again
        }
    }
}
