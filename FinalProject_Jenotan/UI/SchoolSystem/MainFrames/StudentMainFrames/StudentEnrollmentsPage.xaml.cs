using FinalProject_Jenotan.Database.Models.EnrollmentSystem;
using FinalProject_Jenotan.ViewModel.MainFramesViewModel.SchoolSystemMFViewModel.DeanMFViewModels;
using FinalProject_Jenotan.ViewModel.MainFramesViewModel.SchoolSystemMFViewModel.StudentMFViewModels;
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

namespace FinalProject_Jenotan.UI.SchoolSystem.MainFrames.StudentMainFrames
{
    /// <summary>
    /// Interaction logic for StudentEnrollmentsPage.xaml
    /// </summary>
    public partial class StudentEnrollmentsPage : Page
    {
        public StudentEnrollmentsPage(Student student)
        {
            InitializeComponent();
            DataContext = new StudentEnrollmentsMFViewModel(student);
        }
    }
}
