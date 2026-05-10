using FinalProject_Jenotan.Database.Models.EnrollmentSystem;
using FinalProject_Jenotan.ViewModel.MainFramesViewModel.SchoolSystemMFViewModel.DeanMFViewModels.DeanMFViewModels;
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

namespace FinalProject_Jenotan.UI.SchoolSystem.MainFrames.DeanMainFrames
{
    /// <summary>
    /// Interaction logic for DeanClassPage.xaml
    /// </summary>
    public partial class DeanClassPage : Page
    {
        public DeanClassPage(Faculty dean)
        {
            InitializeComponent();
            DataContext = new DeanClassesMFViewModel(dean);
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
