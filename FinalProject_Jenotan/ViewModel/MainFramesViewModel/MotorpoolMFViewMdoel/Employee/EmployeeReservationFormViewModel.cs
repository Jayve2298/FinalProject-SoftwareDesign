using FinalProject_Jenotan.Commands;
using FinalProject_Jenotan.Database;
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
    class EmployeeReservationFormViewModel : INotifyPropertyChanged
    {
        private FinalProject_Jenotan.Database.Models.MotorPoolSystem.Reservation.Employee _employee;
        private readonly TinyCollegeDbContextSQLServer _context;
        public ICommand ApproveCommand { get; }
        public ICommand DenyCommand { get; }
        public ObservableCollection<ReservationForm> ReservationForms { get; set; }
        public EmployeeReservationFormViewModel(FinalProject_Jenotan.Database.Models.MotorPoolSystem.Reservation.Employee employee) 
        {
            _employee = employee;
            _context = new TinyCollegeDbContextSQLServer();
            ReservationForms = new ObservableCollection<ReservationForm>();
            ApproveCommand = new RelayCommand(ApproveReservation);
            DenyCommand = new RelayCommand(DenyReservation);
            LoadReservations();
        }

        private void LoadReservations()
        {
            var reservations = _context.ReservationForms
                .Where(r => r.EmployeeId == null)
                .OrderBy(r => r.DepartureDateTime)
                .ToList();

            ReservationForms = new ObservableCollection<ReservationForm>(reservations);
            OnPropertyChanged(nameof(ReservationForms));
        }

        private async void ApproveReservation(object obj)
        {
            if (SelectedReservation == null)
            {
                StatusMessage = "No reservation selected.";
                StatusMessageBrush = Brushes.Red;
                return;
            }

            var vehicle = _context.Vehicles
                .FirstOrDefault(v => v.VehicleId == SelectedReservation.VehicleId);

            if (vehicle == null)
            {
                StatusMessage = "Vehicle not found.";
                StatusMessageBrush = Brushes.Red;
                return;
            }
            if (!vehicle.IsAvailable)
            {
                StatusMessage = "Vehicle unavailable.";
                StatusMessageBrush = Brushes.Red;
                return;
            }


            var checkout = new CheckoutForm
            {
                CheckoutId = GenerateCheckoutId(),
                CheckOutDate = null,
                IsVerified = false,

                FacultyId = SelectedReservation.FacultyId,
                EmployeeId = _employee.EmployeeId,
                RFormId = SelectedReservation.RFormId
            };

            
            _context.CheckoutForms.Add(checkout);

            
            SelectedReservation.EmployeeId = _employee.EmployeeId;
            SelectedReservation.ISApproved = true;
            _context.ReservationForms.Update(SelectedReservation);


            //var vehicle = _context.Vehicles
            //    .FirstOrDefault(v => v.VehicleId == SelectedReservation.VehicleId);

            if (vehicle != null)
            {
                vehicle.IsAvailable = false;
                _context.Vehicles.Update(vehicle);
            }

            var faculty = _context.Faculties
                .FirstOrDefault(f => f.FacultyId == SelectedReservation.FacultyId);

            if (faculty != null)
            {
                faculty.CheckoutFormList ??= new List<CheckoutForm>();
                faculty.CheckoutFormList.Add(checkout);
            }

            var employee = _context.Employees
                .FirstOrDefault(e => e.EmployeeId == _employee.EmployeeId);


            if (employee != null)
            {
                employee.CheckoutFormList ??= new List<CheckoutForm>();
                employee.CheckoutFormList.Add(checkout);
            }

            _context.SaveChanges();

            ReservationForms.Remove(SelectedReservation);

            SelectedReservation = null;

            StatusMessage = "Reservation Approved";
            StatusMessageBrush = Brushes.Green;

            await Task.Delay(3000);
            StatusMessage = string.Empty;
        }

        private async void DenyReservation(object obj)
        {
            if (SelectedReservation == null)
            {
                StatusMessage = "No reservation selected.";
                StatusMessageBrush = Brushes.Red;
                return;
            }

            var reservation = _context.ReservationForms
                .FirstOrDefault(r => r.RFormId == SelectedReservation.RFormId);

            if (reservation == null)
            {
                StatusMessage = "Reservation not found.";
                StatusMessageBrush = Brushes.Red;
                return;
            }

            _context.ReservationForms.Remove(reservation);

            _context.SaveChanges();

            ReservationForms.Remove(SelectedReservation);

            SelectedReservation = null;

            LoadReservations();

            StatusMessage = "Reservation Denied";
            StatusMessageBrush = Brushes.DarkRed;

            await Task.Delay(3000);
            StatusMessage = string.Empty;
        }

        private string GenerateCheckoutId()
        {
            var last = _context.CheckoutForms
                .OrderByDescending(c => c.CheckoutId)
                .FirstOrDefault();

            int next = 1000;

            if (last != null)
            {
                var num = last.CheckoutId.Split('-')[1].Trim();

                if (int.TryParse(num, out int lastNum))
                    next = lastNum + 1;
            }

            return $"COI - {next}";
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
