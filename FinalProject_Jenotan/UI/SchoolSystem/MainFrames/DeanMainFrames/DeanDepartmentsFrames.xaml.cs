using FinalProject_Jenotan.Database.Models.EnrollmentSystem;
using FinalProject_Jenotan.ViewModel.MainFramesViewModel.SchoolSystemMFViewModel.DeanMFViewModels;
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
    /// Interaction logic for DeanDepartmentsFrames.xaml
    /// </summary>
    public partial class DeanDepartmentsFrames : Page
    {
        private Faculty _faculty;
        public DeanDepartmentsFrames(Faculty faculty)
        {
            InitializeComponent();
            _faculty = faculty;
            DataContext = new DeanDepartmentMFViewModel(faculty);
        }

        private void SearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (DataContext is DeanDepartmentMFViewModel vm)
                {
                    vm.SearchDepartments();
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
