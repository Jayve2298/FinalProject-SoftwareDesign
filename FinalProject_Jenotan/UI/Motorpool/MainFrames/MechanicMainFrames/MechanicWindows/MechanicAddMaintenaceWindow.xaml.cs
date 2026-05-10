using FinalProject_Jenotan.Database.Models.MotorPoolSystem.Maintenance;
using FinalProject_Jenotan.ViewModel.MainFramesViewModel.MotorpoolMFViewMdoel.MechanicMF.MechanicCRUD;
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
using System.Windows.Shapes;

namespace FinalProject_Jenotan.UI.Motorpool.MainFrames.MechanicMainFrames.MechanicWindows
{
    /// <summary>
    /// Interaction logic for MechanicAddMaintenaceWindow.xaml
    /// </summary>
    public partial class MechanicAddMaintenaceWindow : Window
    {
        public MechanicAddMaintenaceWindow(Mechanic mechanic)
        {
            InitializeComponent();
            var vm = new MechanicAddMaintenanceDetailsViewModel(mechanic);
            DataContext = vm;

            this.SourceInitialized += (s, e) =>
            {
                var handle = new System.Windows.Interop.WindowInteropHelper(this).Handle;
                var source = System.Windows.Interop.HwndSource.FromHwnd(handle);
                source.AddHook(WindowProc);
            };

            vm.CloseAction = () =>
            {
                DialogResult = true;
                Close();
            };

        }

        private IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_SYSCOMMAND = 0x0112;
            const int SC_MOVE = 0xF010;

            if (msg == WM_SYSCOMMAND)
            {
                int command = wParam.ToInt32() & 0xFFF0;

                if (command == SC_MOVE)
                {
                    handled = true;
                    return IntPtr.Zero;
                }
            }

            handled = false;
            return IntPtr.Zero;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            CloseWindow(false);
        }
        public void CloseWindow(bool? result)
        {
            DialogResult = result;
            Close();
        }

    }
}
