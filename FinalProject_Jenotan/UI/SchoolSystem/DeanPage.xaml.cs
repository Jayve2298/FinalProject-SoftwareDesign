using FinalProject_Jenotan.Database.Models.EnrollmentSystem;
using FinalProject_Jenotan.UI.SchoolSystem.MainFrames.DeanMainFrames;
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
    /// Interaction logic for DeanPage.xaml
    /// </summary>
    public partial class DeanPage : Page
    {
        public Frame MainFrame { get; set; }

        public DeanPage(Faculty faculty)
        {
            InitializeComponent();
            DataContext = new DeanPageViewModel(DeanMainFrameControl, faculty);
            DeanMainFrameControl.Navigate(new DeanDepartmentsFrames(faculty));
        }
    }
}
