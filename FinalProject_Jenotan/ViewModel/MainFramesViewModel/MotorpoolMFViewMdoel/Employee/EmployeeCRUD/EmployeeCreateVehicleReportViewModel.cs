using FinalProject_Jenotan.Commands;
using FinalProject_Jenotan.Database;
using FinalProject_Jenotan.Database.Models.MotorPoolSystem;
using FinalProject_Jenotan.Database.Models.MotorPoolSystem.Reservation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;

namespace FinalProject_Jenotan.ViewModel.MainFramesViewModel.MotorpoolMFViewMdoel.Employee.EmployeeCRUD
{
    public class EmployeeCreateVehicleReportViewModel : INotifyPropertyChanged
    {
        private readonly TinyCollegeDbContextSQLServer _context;
        private FinalProject_Jenotan.Database.Models.MotorPoolSystem.Reservation.Employee _employee;
        public ICommand ConfirmCommand { get; }
        public ObservableCollection<VehicleReport> Reports { get; set; }
        public Action CloseAction { get; set; }
        public ObservableCollection<Vehicle> Vehicles { get; set; }

        public EmployeeCreateVehicleReportViewModel(
            FinalProject_Jenotan.Database.Models.MotorPoolSystem.Reservation.Employee employee)
        {
            _employee = employee;
            _context = new TinyCollegeDbContextSQLServer();
           ConfirmCommand = new RelayCommand(CreateReport);
            LoadVehicles();
            LoadReports();
        }
        private void LoadVehicles()
        {
            var vehicles = _context.Vehicles.ToList();

            Vehicles = new ObservableCollection<Vehicle>(vehicles);
            OnPropertyChanged(nameof(Vehicles));
        }

        private void LoadReports()
        {
            var employeeId = _employee.EmployeeId;

            var reports = _context.VehicleReports
                .Where(r => r.EmployeeId == employeeId)
                .ToList();

            Reports = new ObservableCollection<VehicleReport>(reports);
            OnPropertyChanged(nameof(Reports));
        }

        private async void CreateReport(object obj)
        {
            if (SelectedVehicle == null)
                return;

            var employeeId = _employee.EmployeeId;

            
            var existing = _context.VehicleReports.FirstOrDefault(r =>
                r.EmployeeId == employeeId &&
                r.VehicleId == SelectedVehicle.VehicleId
            );

            if (existing != null)
            {
                await ShowStatus("Report Exists", Brushes.Red);
                return;
            }

           
            var report = new VehicleReport
            {
                VReportId = GenerateVReportId(),
                EmployeeId = employeeId,
                VehicleId = SelectedVehicle.VehicleId,
                TimesUsed = TimesUsed,
                TotalMileageGained = TotalMileage
            };

            _context.VehicleReports.Add(report);
            _context.SaveChanges();

            CloseAction?.Invoke();
        }



        private Vehicle _selectedVehicle;
        public Vehicle SelectedVehicle
        {
            get => _selectedVehicle;
            set
            {
                _selectedVehicle = value;
                OnPropertyChanged(nameof(SelectedVehicle));

                if (_selectedVehicle != null)
                    VReportId = GenerateVReportId();

                CalculateReport();
            }
        }
        private int _timesUsed;
        public int TimesUsed
        {
            get => _timesUsed;
            set
            {
                _timesUsed = value;
                OnPropertyChanged(nameof(TimesUsed));
            }
        }

        private float _totalMileage;
        public float TotalMileage
        {
            get => _totalMileage;
            set
            {
                _totalMileage = value;
                OnPropertyChanged(nameof(TotalMileage));
            }
        }

        private string _vReportId;
        public string VReportId
        {
            get => _vReportId;
            set
            {
                _vReportId = value;
                OnPropertyChanged(nameof(VReportId));
            }
        }

        private string _vehicleId;
        public string VehicleId
        {
            get => _vehicleId;
            set
            {
                _vehicleId = value;
                OnPropertyChanged(nameof(VehicleId));
            }
        }

        private void CalculateReport()
        {
            if (SelectedVehicle == null)
            {
                TimesUsed = 0;
                TotalMileage = 0;
                VehicleId = string.Empty;
                return;
            }

            VehicleId = SelectedVehicle.VehicleId;

            var employeeId = _employee.EmployeeId;

            var completions = _context.CompletionForms
                .Where(c =>
                    c.VehicleId == SelectedVehicle.VehicleId &&
                    c.EmployeeId == employeeId
                )
                .ToList();

            TimesUsed = completions.Count;

            TotalMileage = completions
                .Where(c => c.EndOdo >= c.StartOdo)
                .Sum(c => c.EndOdo - c.StartOdo);
        }

        private string GenerateVReportId()
        {
            var lastReport = _context.VehicleReports
                .OrderByDescending(v => v.VReportId)
                .FirstOrDefault();

            if (lastReport == null)
                return "VR - 1000";

            var numberPart = int.Parse(lastReport.VReportId.Split('-')[1].Trim());
            return $"VR - {numberPart + 1}";
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

        private Brush _statusMessageBrush;
        public Brush StatusMessageBrush
        {
            get => _statusMessageBrush;
            set
            {
                _statusMessageBrush = value;
                OnPropertyChanged(nameof(StatusMessageBrush));
            }
        }

        private Brush _statusBrush;
        public Brush StatusBrush
        {
            get => _statusBrush;
            set
            {
                _statusBrush = value;
                OnPropertyChanged(nameof(StatusBrush));
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
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
