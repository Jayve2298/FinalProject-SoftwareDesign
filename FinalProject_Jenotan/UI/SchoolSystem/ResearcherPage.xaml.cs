using FinalProject_Jenotan.Database.Models.EnrollmentSystem;
using FinalProject_Jenotan.UI.EditWindow.FormsWindow;
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
    /// Interaction logic for ResearcherPage.xaml
    /// </summary>
    public partial class ResearcherPage : Page
    {
        public ResearcherPage(Faculty faculty)
        {
            InitializeComponent();

            var mainWindow = Application.Current.MainWindow as MainWindow;

            DataContext = new ResearcherViewModel(ResearcherMainFrameControl, faculty);
            ResearcherMainFrameControl.Navigate(new FormsPage(faculty));
        }
    }
}
