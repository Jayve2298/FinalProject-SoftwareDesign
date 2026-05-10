using FinalProject_Jenotan.Commands;
using FinalProject_Jenotan.Database;
using FinalProject_Jenotan.Database.Models.EnrollmentSystem;
using FinalProject_Jenotan.Database.Models.MotorPoolSystem.Reservation;
using FinalProject_Jenotan.UI.EditWindow.DeptChairEditWindows;
using FinalProject_Jenotan.UI.EditWindow.FormsWindow.FacultyForms.CRUDWindows;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace FinalProject_Jenotan.ViewModel.FormsViewModel.FacultyFormsViewModel
{
    public class FacultyOnGoingViewModel : INotifyPropertyChanged
    {
        private readonly TinyCollegeDbContextSQLServer _context;
        private Faculty _faculty;

        private List<CheckoutForm> _allCheckouts;
        public ICommand CompleteCommand { get; }

        public ObservableCollection<CheckoutForm> CheckoutFormList { get; set; }

        public FacultyOnGoingViewModel(Faculty faculty)
        {
            _faculty = faculty;
            _context = new TinyCollegeDbContextSQLServer();

            CheckoutFormList = new ObservableCollection<CheckoutForm>();
            CompleteCommand = new RelayCommand(OpenCompleteWindow);
            LoadOnGoingTrips();
        }

        private void LoadOnGoingTrips()
        {
            _allCheckouts = _context.CheckoutForms
                .Where(c =>
                    c.FacultyId == _faculty.FacultyId &&
                    c.IsOnGoing == true &&
                    c.IsCheckedOut == true &&
                    c.IsVerified == true)
                .Include(c => c.ReservationFormLink)
                .ThenInclude(r => r.VehicleLink)
                .OrderByDescending(c => c.CheckOutDate)
                .ToList();

            CheckoutFormList = new ObservableCollection<CheckoutForm>(_allCheckouts);

            OnPropertyChanged(nameof(CheckoutFormList));
        }

        public void OpenCompleteWindow(object obj)
        {
            if (SelectedCheckOut == null)
            {
                StatusMessage = "Please select a trip first.";
                StatusMessageBrush = Brushes.Red;
                return;
            }

            var window = new FacultyCompleteWindow(SelectedCheckOut);

            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            var result = window.ShowDialog();

            if (result == true)
            {
                StatusMessage = "Completed Successfully";
                StatusMessageBrush = Brushes.Green;
                LoadOnGoingTrips();
            }
        }

        private CheckoutForm _selectedCheckOut;
        public CheckoutForm SelectedCheckOut
        {
            get => _selectedCheckOut;
            set
            {
                _selectedCheckOut = value;
                OnPropertyChanged(nameof(SelectedCheckOut));
            }
        }

        private string _statusMessage;
        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged(nameof(StatusMessage));
            }
        }

        private Brush _statusMessageBrush = Brushes.Black;
        public Brush StatusMessageBrush
        {
            get => _statusMessageBrush;
            set
            {
                _statusMessageBrush = value;
                OnPropertyChanged(nameof(StatusMessageBrush));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
