using FinalProject_Jenotan.Database;
using FinalProject_Jenotan.Database.Models.EnrollmentSystem;
using FinalProject_Jenotan.ViewModel.MainFramesViewModel.SchoolSystemMFViewModel.StudentMFViewModels.StudentCRUDViewModel;
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

namespace FinalProject_Jenotan.UI.EditWindow.StudentEditWindows
{
    /// <summary>
    /// Interaction logic for StudentAddEnrollmentWindow.xaml
    /// </summary>
    public partial class StudentAddEnrollmentWindow : Window
    {
        public StudentAddEnrollmentWindow(Student student)
        {
            InitializeComponent();
            var vm = new StudentAddEnrollmentViewModel(student);
            DataContext = vm;

            this.SourceInitialized += (s, e) =>
            {
                var handle = new System.Windows.Interop.WindowInteropHelper(this).Handle;
                var source = System.Windows.Interop.HwndSource.FromHwnd(handle);
                source.AddHook(WindowProc);
            };

            vm.CloseAction = () =>
            {
                this.DialogResult = true;
                this.Close();
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

        public void CloseWindow(bool? result)
        {
            DialogResult = result;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            CloseWindow(false);
        }
    }
}
