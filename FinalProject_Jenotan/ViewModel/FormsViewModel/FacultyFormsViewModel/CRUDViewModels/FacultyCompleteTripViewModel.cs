using Bogus.DataSets;
using FinalProject_Jenotan.Commands;
using FinalProject_Jenotan.Database;
using FinalProject_Jenotan.Database.Models.MotorPoolSystem.Maintenance;
using FinalProject_Jenotan.Database.Models.MotorPoolSystem.Reservation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection.Emit;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Vehicle = FinalProject_Jenotan.Database.Models.MotorPoolSystem.Reservation.Vehicle;

namespace FinalProject_Jenotan.ViewModel.FormsViewModel.FacultyFormsViewModel.CRUDViewModels
{
    public class FacultyCompleteTripViewModel : INotifyPropertyChanged
    {
        private CheckoutForm _checkoutForm;
        private Vehicle _vehicle;
        private readonly TinyCollegeDbContextSQLServer _context;
        public Action CloseAction { get; set; }

        public ICommand ConfirmCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public FacultyCompleteTripViewModel(CheckoutForm checkoutForm) 
        {
            _checkoutForm = checkoutForm;
            _context = new TinyCollegeDbContextSQLServer();
            _vehicle = checkoutForm.ReservationFormLink.VehicleLink;

            CFormId = GenerateCFormId();
            CheckoutId = checkoutForm.CheckoutId;
            VehicleId = _vehicle.VehicleId;
            StartOdo = _vehicle.OdoReading;

            ConfirmCommand = new RelayCommand(Confirm);
        }

        public string CFormId { get; set; }

        public string CheckoutId { get; set; }

        public string VehicleId { get; set; }

        public float StartOdo { get; set; }

        private float _endOdo;
        public float EndOdo
        {
            get => _endOdo;
            set
            {
                _endOdo = value;
                OnPropertyChanged(nameof(EndOdo));
                CalculateTripCost();
                OnPropertyChanged(nameof(CanConfirm));
            }
        }

        private float _fuelCost;
        public float FuelCost
        {
            get => _fuelCost;
            set
            {
                _fuelCost = value;
                OnPropertyChanged(nameof(FuelCost));
                CalculateTripCost();
            }
        }

        private string _creditCardNumber;
        public string CreditCardNumber
        {
            get => _creditCardNumber;
            set
            {
                _creditCardNumber = value;
                OnPropertyChanged(nameof(CreditCardNumber));
            }
        }

        private float _tripCost;
        public float TripCost
        {
            get => _tripCost;
            set
            {
                _tripCost = value;
                OnPropertyChanged(nameof(TripCost));
            }
        }
        public bool CanConfirm => EndOdo >= StartOdo;

        private void CalculateTripCost()
        {
            if (_vehicle == null) return;

            if (EndOdo >= StartOdo)
            {
                TripCost = FuelCost + ((EndOdo - StartOdo) * _vehicle.CostPerMilage);
            }
            else
            {
                TripCost = 0;
            }
        }


        private async void Confirm(object obj)
        {
            StatusMessage = "";
            StatusMessageBrush = Brushes.Transparent;

            if (EndOdo == 0 || FuelCost == 0 || string.IsNullOrWhiteSpace(CreditCardNumber))
            {
                await ShowStatus("Missing Input", Brushes.Red);
                return;
            }

            if (EndOdo < StartOdo)
            {
                await ShowStatus("Invalid Odo Input", Brushes.Red);
                return;
            }

            var checkout = _context.CheckoutForms
                .FirstOrDefault(c => c.CheckoutId == CheckoutId);

            var vehicle = _context.Vehicles
                .FirstOrDefault(v => v.VehicleId == VehicleId);

            if (checkout == null || vehicle == null)
            {
                await ShowStatus("Data not found", Brushes.Red);
                return;
            }

            var completion = new CompletionForm
            {
                CFormId = CFormId,
                StartOdo = StartOdo,
                EndOdo = EndOdo,
                FuelCost = FuelCost,
                CreditCardNum = CreditCardNumber,
                TripCost = TripCost,
                VehicleId = VehicleId,
                CheckoutId = CheckoutId,
                FacultyId = checkout.FacultyId,
                EmployeeId = checkout.EmployeeId,
                DateCompleted = DateTime.Now,
                Complaints = string.IsNullOrWhiteSpace(Complaints) ? "None" : Complaints
            };
            _context.CompletionForms.Add(completion);
            checkout.IsOnGoing = false;
            vehicle.OdoReading = EndOdo;
            vehicle.IsAvailable = true;

            var complaintText = string.IsNullOrWhiteSpace(Complaints) ? "None" : Complaints;

            if (!string.IsNullOrWhiteSpace(complaintText) &&
                complaintText.ToLower() != "none")
            {
                var maintenanceLog = new MaintenanceLog
                {
                    MLogId = GenerateMLogId(),
                    Description = complaintText,
                    DateLogged = DateTime.Now,
                    CompleteDate = null,
                    IsCompleted = false,
                    VehicleId = VehicleId
                };

                _context.MaintenanceLogs.Add(maintenanceLog);

                vehicle.IsAvailable = false;
            }

            _context.SaveChanges();

            CloseAction?.Invoke();
        }
        private string GenerateCFormId()
        {
            var lastCompletion = _context.CompletionForms
                .AsEnumerable()
                .OrderByDescending(c =>
                {
                    var parts = c.CFormId.Split('-');
                    return int.TryParse(parts[1].Trim(), out int num) ? num : 0;
                })
                .FirstOrDefault();

            int nextNumber = 1000;

            if (lastCompletion != null)
            {
                var numberPart = lastCompletion.CFormId.Split('-')[1].Trim();

                if (int.TryParse(numberPart, out int lastNumber))
                {
                    nextNumber = lastNumber + 1;
                }
            }

            return $"CF - {nextNumber}";
        }

        private string _complaints;
        public string Complaints
        {
            get => _complaints;
            set
            {
                _complaints = value;
                OnPropertyChanged(nameof(Complaints));
            }
        }

        private string GenerateMLogId()
        {
            var lastLog = _context.MaintenanceLogs
                .AsEnumerable()
                .OrderByDescending(m =>
                {
                    var parts = m.MLogId.Split('C');
                    return int.TryParse(parts.Last(), out int num) ? num : 0;
                })
                .FirstOrDefault();

            int nextNumber = 1000;

            if (lastLog != null)
            {
                var numberPart = lastLog.MLogId.Replace("MTC", "");

                if (int.TryParse(numberPart, out int lastNumber))
                {
                    nextNumber = lastNumber + 1;
                }
            }

            return $"MTC{nextNumber}";
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

        private Brush _statusMessageBrush = Brushes.Transparent;
        public Brush StatusMessageBrush
        {
            get => _statusMessageBrush;
            set
            {
                _statusMessageBrush = value;
                OnPropertyChanged(nameof(StatusMessageBrush));
            }
        }

        private async Task ShowStatus(string message, Brush color)
        {
            StatusMessage = message;
            StatusMessageBrush = color;

            await Task.Delay(3000);

            StatusMessage = string.Empty;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
