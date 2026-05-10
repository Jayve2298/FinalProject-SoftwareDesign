using FinalProject_Jenotan.Database.Models.MotorPoolSystem.Maintenance;
using FinalProject_Jenotan.UI.Motorpool.MainFrames.MechanicMainFrames;
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
    /// Interaction logic for MechanicPage.xaml
    /// </summary>
    public partial class MechanicPage : Page
    {
        public MechanicPage(Mechanic mechanic)
        {
            InitializeComponent();

            var mainWindow = Application.Current.MainWindow as MainWindow;

            DataContext = new MechanicViewModel(MechanicMainFrameControl, mechanic);
            MechanicMainFrameControl.Navigate(new MechanicMaintenancePage(mechanic));
        }
    }
}
