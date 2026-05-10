using FinalProject_Jenotan.Database.Models.EnrollmentSystem;
using FinalProject_Jenotan.UI.SchoolSystem.MainFrames.StudentMainFrames;
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

namespace FinalProject_Jenotan.UI.SchoolSystem
{
    /// <summary>
    /// Interaction logic for StudentPage.xaml
    /// </summary>
    public partial class StudentPage : Page
    {
        public Frame MainFrame { get; set; }
        public StudentPage(Student student)
        {
            InitializeComponent();

            var mainWindow = Application.Current.MainWindow as MainWindow;

            DataContext = new StudentViewModel(StudentMainFrameControl, student);
            StudentMainFrameControl.Navigate(new StudentEnrollmentsPage(student));
        }
    }
}
