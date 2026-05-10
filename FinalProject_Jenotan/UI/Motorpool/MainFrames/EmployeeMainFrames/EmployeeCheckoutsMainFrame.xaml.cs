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
    /// Interaction logic for EmployeeCheckoutsMainFrame.xaml
    /// </summary>
    public partial class EmployeeCheckoutsMainFrame : Page
    {
        public EmployeeCheckoutsMainFrame(Employee employee)
        {
            InitializeComponent();
            DataContext = new EmployeeCheckoutFormViewModel(employee);
        }
    }
}
