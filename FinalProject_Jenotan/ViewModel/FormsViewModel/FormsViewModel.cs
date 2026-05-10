using FinalProject_Jenotan.Commands;
using FinalProject_Jenotan.Database.Models.EnrollmentSystem;
using FinalProject_Jenotan.UI.EditWindow.FormsWindow;
using FinalProject_Jenotan.UI.EditWindow.FormsWindow.FacultyForms;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace FinalProject_Jenotan.ViewModel.FormsViewModel
{
    public class FormsViewModel
    {
        private FormsPage _page;
        public ICommand ReservationCommand { get; }
        public ICommand CheckOutCommand { get; }
        public ICommand CompletionCommand { get; }
        public ICommand OnGoingCommand { get; }
        public FormsViewModel(Faculty faculty, FormsPage page) 
        {
            _page = page;
            ReservationCommand = new RelayCommand(OpenReservation);
            CheckOutCommand = new RelayCommand(OpenCheckOut);
            CompletionCommand = new RelayCommand(OpenCompletion);
            OnGoingCommand = new RelayCommand(OpenOnGoing);
        }

        public void OpenReservation(object obj)
        {
            _page.NavigateToReservation();
        }

        public void OpenCheckOut(object obj)
        {
            _page.NavigateToCheckOut();
        }

        public void OpenOnGoing(object obj)
        {
            _page.NavigateToOnGoing();
        }

        public void OpenCompletion(object obj)
        {
            _page.NavigateToCompletion();
        }
    }
}
