using FinalProject_Jenotan.Database.Models.EnrollmentSystem;
using FinalProject_Jenotan.UI.SchoolSystem.MainFrames.DeptChairMainFrames;
using FinalProject_Jenotan.ViewModel;
using FinalProject_Jenotan.ViewModel.SchoolSystem;
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
    /// Interaction logic for DeptChairPage.xaml
    /// </summary>
    public partial class DeptChairPage : Page
    {
        public Frame MainFrame { get; set; }
        public DeptChairPage(Faculty faculty)
        {
            InitializeComponent();

            var mainWindow = Application.Current.MainWindow as MainWindow;

            DataContext = new DeptChairViewModel(DeptChairMainFrameControl, faculty);
            DeptChairMainFrameControl.Navigate(new DeptChairFacultiesPage(faculty));
        }

        private void MajorButton_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Button Clicked");
        }
    }
}
