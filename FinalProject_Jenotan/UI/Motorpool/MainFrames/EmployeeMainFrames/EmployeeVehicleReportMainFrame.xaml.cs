using FinalProject_Jenotan.Database.Models.MotorPoolSystem.Reservation;
using FinalProject_Jenotan.ViewModel.MainFramesViewModel.MotorpoolMFViewMdoel.Employee;
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

namespace FinalProject_Jenotan.UI.Motorpool.MainFrames.EmployeeMainFrames
{
    /// <summary>
    /// Interaction logic for EmployeeVehicleReportMainFrame.xaml
    /// </summary>
    public partial class EmployeeVehicleReportMainFrame : Page
    {
        public EmployeeVehicleReportMainFrame(Employee employee)
        {
            InitializeComponent();
            DataContext = new EmployeeVehicleReportViewModel(employee);
        }
    }
}
