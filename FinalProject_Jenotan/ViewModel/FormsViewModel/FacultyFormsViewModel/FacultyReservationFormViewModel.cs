using FinalProject_Jenotan.Commands;
using FinalProject_Jenotan.Database;
using FinalProject_Jenotan.Database.Models.EnrollmentSystem;
using FinalProject_Jenotan.Database.Models.MotorPoolSystem.Reservation;
using FinalProject_Jenotan.PromptWindow;
using FinalProject_Jenotan.UI.EditWindow.DeptChairEditWindows;
using FinalProject_Jenotan.UI.EditWindow.FormsWindow.FacultyForms.CRUDWindows;
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
    public class FacultyReservationFormViewModel : INotifyPropertyChanged
    {
        private Faculty _faculty;
        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        private List<ReservationForm> _allReservations;

        private readonly TinyCollegeDbContextSQLServer _context;

        public ObservableCollection<ReservationForm> ReservationFormList { get; set; }

        public FacultyReservationFormViewModel(Faculty faculty)
        {
            _faculty = faculty;
            _context = new TinyCollegeDbContextSQLServer();

            LoadReservations();
            AddCommand = new RelayCommand(OpenAdd);
            DeleteCommand = new RelayCommand(DeleteReservation);
        }

        private void LoadReservations()
        {
            _allReservations = _context.ReservationForms
                .Where(r => r.FacultyId == _faculty.FacultyId)
                .OrderByDescending(r => r.DepartureDateTime)
                .ToList();

            ReservationFormList = new ObservableCollection<ReservationForm>(_allReservations);
            OnPropertyChanged(nameof(ReservationFormList));
            ApplyFilters();
        }
        public async void DeleteReservation(object obj)
        {
            if (SelectedReservation == null)
            {
                StatusMessage = "Please select a reservation.";
                StatusMessageBrush = Brushes.Red;
                await Task.Delay(3000);
                StatusMessage = string.Empty;
                return;
            }

            if (SelectedReservation.ISApproved == true)
            {
                StatusMessage = "Already Approved";
                StatusMessageBrush = Brushes.Red;
                await Task.Delay(3000);
                StatusMessage = string.Empty;
                return;
            }

            var confirmWindow = new ConfirmationWindow
            {
                Owner = Application.Current.MainWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            confirmWindow.ShowDialog();


            if (!confirmWindow.result)
                return;

            _context.ReservationForms.Remove(SelectedReservation);
            _context.SaveChanges();

            ReservationFormList.Remove(SelectedReservation);
            ApplyFilters();
            StatusMessage = "Reservation deleted.";
            StatusMessageBrush = Brushes.Red;

            LoadReservations();
            await Task.Delay(3000);
            StatusMessage = string.Empty;

            
        }

        private void ApplyFilters()
        {
            if (_allReservations == null) return;

            var filtered = _allReservations.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                filtered = filtered.Where(r =>
                    !string.IsNullOrEmpty(r.RFormId) &&
                    r.RFormId.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                );
            }

            if (ShowPendingOnly)
            {
                filtered = filtered.Where(r => r.ISApproved == false);
            }

            ReservationFormList = new ObservableCollection<ReservationForm>(filtered);
            OnPropertyChanged(nameof(ReservationFormList));
        }

        public async void OpenAdd(object obj)
        {
            var window = new AddReservationWindow(_faculty);

            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            var result = window.ShowDialog();

            if (result == true)
            {

                StatusMessage = "Reservation Added";
                StatusMessageBrush = Brushes.Green;
                LoadReservations();
                ApplyFilters();
                await Task.Delay(3000);
                StatusMessage = string.Empty;
            }

        }

        private ReservationForm _selectedReservation;
        public ReservationForm SelectedReservation
        {
            get => _selectedReservation;
            set
            {
                _selectedReservation = value;
                OnPropertyChanged(nameof(SelectedReservation));
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

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                ApplyFilters();
            }
        }

        private bool _showPendingOnly;
        public bool ShowPendingOnly
        {
            get => _showPendingOnly;
            set
            {
                _showPendingOnly = value;
                OnPropertyChanged(nameof(ShowPendingOnly));
                ApplyFilters();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
