using FinalProject_Jenotan.Commands;
using FinalProject_Jenotan.Database;
using FinalProject_Jenotan.Database.Models.MotorPoolSystem;
using FinalProject_Jenotan.Database.Models.MotorPoolSystem.Reservation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;

namespace FinalProject_Jenotan.ViewModel.MainFramesViewModel.MotorpoolMFViewMdoel.Employee
{
    public class EmployeeCheckoutFormViewModel : INotifyPropertyChanged
    {
        private FinalProject_Jenotan.Database.Models.MotorPoolSystem.Reservation.Employee _employee;
        public ObservableCollection<CheckoutForm> CheckoutForms { get; set; }
        private List<CheckoutForm> _allCheckouts;
        public ICommand VerifyCommand { get; }
        private readonly TinyCollegeDbContextSQLServer _context;
        public EmployeeCheckoutFormViewModel(FinalProject_Jenotan.Database.Models.MotorPoolSystem.Reservation.Employee employee) 
        {
            _employee = employee;
            _context = new TinyCollegeDbContextSQLServer();

            CheckoutForms = new ObservableCollection<CheckoutForm>();
            VerifyCommand = new RelayCommand(VerifyCheckout);
            LoadCheckouts();
        }

        private async void VerifyCheckout(object obj)
        {
            if (SelectedCheckOut == null)
                return;

            if (SelectedCheckOut.CheckOutDate == null)
            {
                StatusMessage = "Waiting For CheckOut";
                StatusMessageBrush = Brushes.Red;

                await Task.Delay(3000);
                StatusMessage = string.Empty;
                return;
            }

            var checkout = _context.CheckoutForms
                .FirstOrDefault(c => c.CheckoutId == SelectedCheckOut.CheckoutId);

            if (checkout == null)
                return;

            checkout.IsVerified = true;
            checkout.IsOnGoing = true;

            _context.SaveChanges();

            SelectedCheckOut.IsVerified = true;
            SelectedCheckOut.IsOnGoing = true;

            CheckoutForms.Remove(SelectedCheckOut);

            SelectedCheckOut = null;

            LoadCheckouts();
            ApplyFilters();

            StatusMessage = "CheckOut Verified";
            StatusMessageBrush = Brushes.Green;

            await Task.Delay(3000);
            StatusMessage = string.Empty;
            OnPropertyChanged(nameof(CheckoutForms));
        }

        private void LoadCheckouts()
        {
            _allCheckouts = _context.CheckoutForms
                .Where(c => c.EmployeeId == _employee.EmployeeId && 
                c.IsOnGoing == false
                && c.IsVerified == false
                && c.IsCheckedOut == true)
                .OrderByDescending(c => c.CheckOutDate)
                .ToList();

            CheckoutForms = new ObservableCollection<CheckoutForm>(_allCheckouts);
            OnPropertyChanged(nameof(CheckoutForms));
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

            var filtered = _allCheckouts
                .Where(c => c.IsOnGoing == false)
                .AsEnumerable();


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
                filtered = filtered.Where(c => c.IsVerified == false);
            }

            CheckoutForms = new ObservableCollection<CheckoutForm>(filtered);
            OnPropertyChanged(nameof(CheckoutForms));
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
