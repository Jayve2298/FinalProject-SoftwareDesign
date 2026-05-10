using FinalProject_Jenotan.Database.Models.EnrollmentSystem;
using FinalProject_Jenotan.ViewModel.MainFramesViewModel.SchoolSystemMFViewModel.DeanMFViewModels;
using FinalProject_Jenotan.ViewModel.MainFramesViewModel.SchoolSystemMFViewModel.DeptChairMFViewModels;
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

namespace FinalProject_Jenotan.UI.SchoolSystem.MainFrames.DeptChairMainFrames
{
    /// <summary>
    /// Interaction logic for DeptChairFacultiesPage.xaml
    /// </summary>
    public partial class DeptChairFacultiesPage : Page
    {
        private Faculty _faculty;
        public DeptChairFacultiesPage(Faculty faculty)
        {
            InitializeComponent();
            _faculty = faculty;
            DataContext = new DeptChairFacultiesViewModel(faculty);
        }

        private void ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var scrollViewer = e.OriginalSource as ScrollViewer;

            if (scrollViewer == null)
                return;

            // if near bottom
            if (scrollViewer.VerticalOffset >= scrollViewer.ScrollableHeight - 1)
            {
                (DataContext as DeanFacultiesMFViewModel)?.LoadMore();
            }
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
