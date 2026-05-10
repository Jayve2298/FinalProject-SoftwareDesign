using FinalProject_Jenotan.Database.Models.MotorPoolSystem.Reservation;
using FinalProject_Jenotan.UI.Motorpool.MainFrames.EmployeeMainFrames;
using FinalProject_Jenotan.ViewModel.Motorpool;
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

namespace FinalProject_Jenotan.UI.Motorpool
{
    /// <summary>
    /// Interaction logic for EmployeePage.xaml
    /// </summary>
    public partial class EmployeePage : Page
    {
        public EmployeePage(Employee employee)
        {
            InitializeComponent();
            var mainWindow = Application.Current.MainWindow as MainWindow;

            DataContext = new EmployeeViewModel(EmployeeMainFrameControl, employee);
            EmployeeMainFrameControl.Navigate(new EmployeeReservationMainFrame(employee));
        }

        private void LogInButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
