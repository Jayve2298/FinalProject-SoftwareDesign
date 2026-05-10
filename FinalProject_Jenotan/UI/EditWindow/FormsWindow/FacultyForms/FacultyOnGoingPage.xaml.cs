using FinalProject_Jenotan.Database.Models.EnrollmentSystem;
using FinalProject_Jenotan.ViewModel.FormsViewModel.FacultyFormsViewModel;
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

namespace FinalProject_Jenotan.UI.EditWindow.FormsWindow.FacultyForms
{
    /// <summary>
    /// Interaction logic for FacultyOnGoingPage.xaml
    /// </summary>
    public partial class FacultyOnGoingPage : Page
    {
        public FacultyOnGoingPage(Faculty faculty)
        {
            InitializeComponent();
            DataContext = new FacultyOnGoingViewModel(faculty);
        }

        //private void CompleteButton_Click(object sender, RoutedEventArgs e)
        //{
        //    MessageBox.Show("Clicked");
        //}
    }
}
