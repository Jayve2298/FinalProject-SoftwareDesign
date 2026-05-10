using FinalProject_Jenotan.Commands;
using FinalProject_Jenotan.Database;
using FinalProject_Jenotan.Database.Models.MotorPoolSystem.Reservation;
using FinalProject_Jenotan.UI.EditWindow.DeptChairEditWindows;
using FinalProject_Jenotan.UI.Motorpool.MainFrames.EmployeeMainFrames.EmployeeWindows;
using FinalProject_Jenotan.ViewModel.MainFramesViewModel.MotorpoolMFViewMdoel.Employee.EmployeeCRUD;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace FinalProject_Jenotan.ViewModel.MainFramesViewModel.MotorpoolMFViewMdoel.Employee
{
    public class EmployeeVehicleReportViewModel : INotifyPropertyChanged
    {
        private readonly TinyCollegeDbContextSQLServer _context;
        public ICommand CreateCommand { get; }
        private FinalProject_Jenotan.Database.Models.MotorPoolSystem.Reservation.Employee _employee;
        public ICommand UpdateCommand { get; }
        public EmployeeVehicleReportViewModel(FinalProject_Jenotan.Database.Models.MotorPoolSystem.Reservation.Employee employee) 
        {
            _employee = employee;
            CreateCommand = new RelayCommand(OpenCreate);
            UpdateCommand = new RelayCommand(UpdateReport);
            _context = new TinyCollegeDbContextSQLServer();
            LoadVehicleReports();
        }

        public void OpenCreate(object obj)
        {
            var window = new EmployeeCreateVehicleReportWindow(_employee);

            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            var result = window.ShowDialog();

            if (result == true)
            {
                LoadVehicleReports();
                StatusMessage = "Added Report";
                StatusMessageBrush = Brushes.Green;
                
            }
        }

        public void UpdateReport(object obj)
        {
            if (SelectedVehicleReport == null)
                return;

            var employeeId = _employee.EmployeeId;
            var vehicleId = SelectedVehicleReport.VehicleId;

            var completions = _context.CompletionForms
                .Where(c =>
                    c.VehicleId == vehicleId &&
                    c.EmployeeId == employeeId)
                .ToList();

            int timesUsed = completions.Count;

            float totalMileage = completions
                .Where(c => c.EndOdo >= c.StartOdo)
                .Sum(c => c.EndOdo - c.StartOdo);

            var reportInDb = _context.VehicleReports
                .FirstOrDefault(r => r.VReportId == SelectedVehicleReport.VReportId);

            if (reportInDb != null)
            {
                reportInDb.TimesUsed = timesUsed;
                reportInDb.TotalMileageGained = totalMileage;

                _context.SaveChanges();
            }

            LoadVehicleReports();

            StatusMessage = "Report Updated";
            StatusMessageBrush = Brushes.Green;
        }
        private void LoadVehicleReports()
        {
            _allReports = _context.VehicleReports
                .Where(r => r.EmployeeId == _employee.EmployeeId)
                .ToList();

            VehicleReports = new ObservableCollection<VehicleReport>(_allReports);
        }

        private ObservableCollection<VehicleReport> _vehicleReports;
        public ObservableCollection<VehicleReport> VehicleReports
        {
            get => _vehicleReports;
            set
            {
                _vehicleReports = value;
                OnPropertyChanged(nameof(VehicleReports));
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
        private List<VehicleReport> _allReports;
        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                FilterReports();
            }
        }

        private void FilterReports()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                VehicleReports = new ObservableCollection<VehicleReport>(_allReports);
                return;
            }

            var filtered = _allReports
                .Where(r => r.VReportId != null &&
                            r.VReportId.ToLower().Contains(SearchText.ToLower()))
                .ToList();

            VehicleReports = new ObservableCollection<VehicleReport>(filtered);
        }

        private VehicleReport _selectedVehicleReport;
        public VehicleReport SelectedVehicleReport
        {
            get => _selectedVehicleReport;
            set
            {
                _selectedVehicleReport = value;
                OnPropertyChanged(nameof(SelectedVehicleReport));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
