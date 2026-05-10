using FinalProject_Jenotan.Database.Models.MotorPoolSystem.Maintenance;
using FinalProject_Jenotan.ViewModel.MainFramesViewModel.MotorpoolMFViewMdoel.AuthorizedMechanicMF;
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

namespace FinalProject_Jenotan.UI.Motorpool.MainFrames.MechanicMainFrames
{
    /// <summary>
    /// Interaction logic for MechanicInventoryPage.xaml
    /// </summary>
    public partial class MechanicInventoryPage : Page
    {
        public MechanicInventoryPage(Mechanic mechanic)
        {
            InitializeComponent();
            DataContext = new AuthorizedMechanicInventoryViewModel(mechanic);
        }
    }
}
