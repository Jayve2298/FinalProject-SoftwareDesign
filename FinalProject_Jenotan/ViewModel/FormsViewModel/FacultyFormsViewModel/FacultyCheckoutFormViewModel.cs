using FinalProject_Jenotan.Commands;
using FinalProject_Jenotan.Database;
using FinalProject_Jenotan.Database.Models.EnrollmentSystem;
using FinalProject_Jenotan.Database.Models.MotorPoolSystem.Reservation;
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
    public class FacultyCheckoutFormViewModel : INotifyPropertyChanged
    {
        private readonly TinyCollegeDbContextSQLServer _context;
        private Faculty _faculty;
        public ICommand CheckOutCommand { get; }
        public ICommand PendingCommand { get; }
        private CheckoutForm _selectedCheckOut;
        private List<CheckoutForm> _allCheckouts;
        public ObservableCollection<CheckoutForm> CheckoutFormList { get; set; }

        public FacultyCheckoutFormViewModel(Faculty faculty)
        {
            _faculty = faculty;
            _context = new TinyCollegeDbContextSQLServer();
            CheckOutCommand = new RelayCommand(ExecuteCheckOut);
            CheckoutFormList = new ObservableCollection<CheckoutForm>();
            PendingCommand = new RelayCommand(OpenPending);

            LoadCheckouts();
        }

        private void OpenPending(object obj)
        {
            var window = new FacultyCheckOutsWindow(_faculty);

            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            var result = window.ShowDialog();
        }

        private void LoadCheckouts()
        {
            _allCheckouts = _context.CheckoutForms
                .Where(c => c.FacultyId == _faculty.FacultyId
                         && c.IsOnGoing == false
                         && c.IsVerified == false
                         && c.IsCheckedOut == false)
                .OrderByDescending(c => c.CheckOutDate)
                .ToList();

            CheckoutFormList = new ObservableCollection<CheckoutForm>(_allCheckouts);

            OnPropertyChanged(nameof(CheckoutFormList));
        }

        private async void ExecuteCheckOut(object obj)
        {
            if (SelectedCheckOut == null)
                return;

            var checkout = _context.CheckoutForms
                .FirstOrDefault(c => c.CheckoutId == SelectedCheckOut.CheckoutId);

            if (checkout == null)
                return;

            checkout.CheckOutDate = DateTime.Now;
            checkout.IsVerified = false;
            checkout.IsOnGoing = false;
            checkout.IsCheckedOut = true;
            _context.CheckoutForms.Update(checkout);

            _context.SaveChanges();

            StatusMessage = "Waiting For Verification";
            StatusMessageBrush = Brushes.Green;
            ApplyFilters();
            LoadCheckouts();
            await Task.Delay(3000);
            StatusMessage = string.Empty;

        }

        public CheckoutForm SelectedCheckOut
        {
            get => _selectedCheckOut;
            set
            {
                _selectedCheckOut = value;
                OnPropertyChanged(nameof(SelectedCheckOut));
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

        private void ApplyFilters()
        {
            if (_allCheckouts == null) return;

            IEnumerable<CheckoutForm> filtered = _allCheckouts;

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                filtered = filtered.Where(c =>
                    (!string.IsNullOrEmpty(c.CheckoutId) &&
                     c.CheckoutId.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||

                    (!string.IsNullOrEmpty(c.RFormId) &&
                     c.RFormId.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                );
            }

            if (ShowPendingOnly)
            {
                filtered = filtered.Where(c => c.IsCheckedOut == true);
            }

            CheckoutFormList = new ObservableCollection<CheckoutForm>(filtered);
            OnPropertyChanged(nameof(CheckoutFormList));
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
