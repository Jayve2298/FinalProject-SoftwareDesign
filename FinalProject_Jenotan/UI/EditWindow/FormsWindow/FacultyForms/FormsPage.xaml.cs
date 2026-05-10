using FinalProject_Jenotan.Database.Models.EnrollmentSystem;
using FinalProject_Jenotan.UI.EditWindow.FormsWindow.FacultyForms;
using FinalProject_Jenotan.ViewModel.FormsViewModel;
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

namespace FinalProject_Jenotan.UI.EditWindow.FormsWindow
{
    /// <summary>
    /// Interaction logic for FormsPage.xaml
    /// </summary>
    public partial class FormsPage : Page
    {
        private Faculty _faculty;
        public FormsPage(Faculty faculty)
        {
            _faculty = faculty;
            InitializeComponent();
            NavigateToReservation();
            DataContext = new FormsViewModel(faculty, this);
        }

        public void NavigateToReservation()
        {
            FormsMainFrameControl.Navigate(new ReservationFormPage(_faculty));
        }

        public void NavigateToCheckOut()
        {
            FormsMainFrameControl.Navigate(new CheckoutFormPage(_faculty));
        }

        public void NavigateToOnGoing()
        {
            FormsMainFrameControl.Navigate(new FacultyOnGoingPage(_faculty));
        }

        public void NavigateToCompletion()
        {
            FormsMainFrameControl?.Navigate(new CompletionFormPage(_faculty));
        }

    }
}
