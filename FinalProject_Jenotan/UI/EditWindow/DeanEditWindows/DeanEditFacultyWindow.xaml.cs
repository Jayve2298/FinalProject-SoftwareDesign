using FinalProject_Jenotan.Database.Models.EnrollmentSystem;
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
    /// Interaction logic for DeanEditFacultyWindow.xaml
    /// </summary>
    public partial class DeanEditFacultyWindow : Window
    {
        private Faculty _selectedFaculty;
        public DeanEditFacultyWindow(Faculty selectedFaculty)
        {
            InitializeComponent();
            _selectedFaculty = selectedFaculty;
            DataContext = new DeanEditFacultyViewModel(selectedFaculty, Close);

            // Set full name directly
            TxtFullName.Text = $"{selectedFaculty.FName} {selectedFaculty.LName}".Trim();
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

        public void CloseWindow()
        {
            this.Close();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
