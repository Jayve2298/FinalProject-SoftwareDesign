using FinalProject_Jenotan.Commands;
using FinalProject_Jenotan.Database;
using FinalProject_Jenotan.Database.Models.EnrollmentSystem;
using FinalProject_Jenotan.Database.Models.MotorPoolSystem.Reservation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;

namespace FinalProject_Jenotan.ViewModel.FormsViewModel.FacultyFormsViewModel.CRUDViewModels
{
    public class AddReservationViewModel : INotifyPropertyChanged
    {
        private readonly TinyCollegeDbContextSQLServer _context;
        private Faculty _faculty;
        public ICommand ConfirmCommand { get; }
        public ObservableCollection<string> VehicleTypes { get; set; }
        public ObservableCollection<Vehicle> Vehicles { get; set; }
        public Action CloseAction { get; set; }
        public DateTime MinDate => DateTime.Now;
        public AddReservationViewModel(Faculty faculty) 
        {
            _faculty = faculty;
            _context = new TinyCollegeDbContextSQLServer();

            VehicleTypes = new ObservableCollection<string>
            {
                "All"
            };

            foreach (var type in Enum.GetValues(typeof(VehicleType)))
            {
                VehicleTypes.Add(type.ToString());
            }

            SelectedVehicleType = "All";

            LoadVehicles();

            RFormId = GenerateReservationId();

            ConfirmCommand = new RelayCommand(Confirm);

        }

        private List<Vehicle> _allVehicles;

        private void LoadVehicles()
        {
            _allVehicles = _context.Vehicles
                .Where(v => v.IsAvailable)
                .ToList();

            Vehicles = new ObservableCollection<Vehicle>(_allVehicles);
        }

        private void FilterVehicles()
        {
            if (_allVehicles == null) return;

            if (SelectedVehicleType == "All")
            {
                Vehicles = new ObservableCollection<Vehicle>(_allVehicles);
            }
            else
            {
                var filtered = _allVehicles
                    .Where(v => v.Type.ToString() == SelectedVehicleType)
                    .ToList();

                Vehicles = new ObservableCollection<Vehicle>(filtered);
            }

            OnPropertyChanged(nameof(Vehicles));
        }

        private string GenerateReservationId()
        {
            var lastReservation = _context.ReservationForms
                .OrderByDescending(r => r.RFormId)
                .FirstOrDefault();

            int nextNumber = 1000;

            if (lastReservation != null)
            {
                var numberPart = lastReservation.RFormId.Split('-')[1].Trim();

                if (int.TryParse(numberPart, out int lastNumber))
                {
                    nextNumber = lastNumber + 1;
                }
            }

            return $"RES - {nextNumber}";
        }

        private string _selectedVehicleType;
        public string SelectedVehicleType
        {
            get => _selectedVehicleType;
            set
            {
                _selectedVehicleType = value;
                OnPropertyChanged();

                FilterVehicles();
            }
        }

        private string _rFormId;
        public string RFormId
        {
            get => _rFormId;
            set
            {
                _rFormId = value;
                OnPropertyChanged();
            }
        }

        private Vehicle _selectedVehicle;
        public Vehicle SelectedVehicle
        {
            get => _selectedVehicle;
            set
            {
                _selectedVehicle = value;
                OnPropertyChanged();

                SelectedVehicleId = _selectedVehicle?.VehicleId;
            }
        }

        private string _selectedVehicleId;
        public string SelectedVehicleId
        {
            get => _selectedVehicleId;
            set
            {
                _selectedVehicleId = value;
                OnPropertyChanged();
            }
        }

        private async void Confirm(object obj)
        {
            if (SelectedVehicle == null ||
                string.IsNullOrWhiteSpace(Destination) ||
                DepartureDateTime == null)
            {
                await ShowStatus("Please complete all fields.", Brushes.Red);
                return;
            }

            if (DepartureDateTime < DateTime.Now)
            {
                await ShowStatus("Cannot select a past date/time.", Brushes.Red);
                return;
            }

            var reservation = new ReservationForm
            {
                RFormId = RFormId,
                Destination = Destination,
                DepartureDateTime = DepartureDateTime.Value,
                FacultyId = _faculty.FacultyId,
                VehicleId = SelectedVehicle.VehicleId,
                ISApproved = false
            };

            _context.ReservationForms.Add(reservation);
            _context.SaveChanges();

            CloseAction?.Invoke();
        }

        private string _destination;
        public string Destination
        {
            get => _destination;
            set
            {
                _destination = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _departureDateTime;
        public DateTime? DepartureDateTime
        {
            get => _departureDateTime;
            set
            {
                _departureDateTime = value;
                OnPropertyChanged();
            }
        }

        private string _statusMessage;
        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged();
            }
        }

        private Brush _statusBrush = Brushes.Red;
        public Brush StatusBrush
        {
            get => _statusBrush;
            set
            {
                _statusBrush = value;
                OnPropertyChanged();
            }
        }

        private async Task ShowStatus(string message, Brush color)
        {
            StatusMessage = message;
            StatusBrush = color;

            await Task.Delay(3000);

            StatusMessage = string.Empty;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
