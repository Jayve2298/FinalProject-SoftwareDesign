using FinalProject_Jenotan.Database.Models.MotorPoolSystem.Maintenance;
using FinalProject_Jenotan.UI.Motorpool.MainFrames.MechanicMainFrames;
using FinalProject_Jenotan.ViewModel.Motorpool;
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
    /// Interaction logic for AuthorizedMechanicPage.xaml
    /// </summary>
    public partial class AuthorizedMechanicPage : Page
    {
        public AuthorizedMechanicPage(Mechanic mechanic)
        {
            InitializeComponent();
            var mainWindow = Application.Current.MainWindow as MainWindow;

            DataContext = new AuthorizedMechanicViewModel(AMechanicMainFrameControl, mechanic);
            AMechanicMainFrameControl.Navigate(new MechanicMaintenancePage(mechanic));
        }
    }
}
