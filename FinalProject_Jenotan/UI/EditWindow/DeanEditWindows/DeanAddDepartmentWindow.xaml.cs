using FinalProject_Jenotan.Database.Models.EnrollmentSystem;
using FinalProject_Jenotan.ViewModel.MainFramesViewModel.SchoolSystemMFViewModel.DeanMFViewModels;
using FinalProject_Jenotan.ViewModel.MainFramesViewModel.SchoolSystemMFViewModel.DeanMFViewModels.DeanWindowsViewModel;
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

namespace FinalProject_Jenotan.UI.EditWindow.DeanEditWindows
{
    /// <summary>
    /// Interaction logic for DeanAddDepartmentWindow.xaml
    /// </summary>
    public partial class DeanAddDepartmentWindow : Window
    {
        public DeanAddDepartmentWindow(Faculty faculty)
        {
            InitializeComponent();

            DataContext = new DeanAddDepartmentVM(faculty, CloseWithResult);

            this.SourceInitialized += (s, e) =>
            {
                var handle = new System.Windows.Interop.WindowInteropHelper(this).Handle;
                var source = System.Windows.Interop.HwndSource.FromHwnd(handle);
                source.AddHook(WindowProc);
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
                }
            }

            return IntPtr.Zero;
        }

        private void CloseWithResult(bool? result)
        {
            this.DialogResult = result; // ✅ SAFE HERE
            this.Close();
        }

        private void CloseWindow()
        {
            this.Close();
        }

    }
}
