using FinalProject_Jenotan.Commands;
using FinalProject_Jenotan.Database;
using FinalProject_Jenotan.Database.Models.MotorPoolSystem.Maintenance;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;

namespace FinalProject_Jenotan.ViewModel.MainFramesViewModel.MotorpoolMFViewMdoel.AuthorizedMechanicMF
{
    public class AuthorizedMechanicReleaseViewModel : INotifyPropertyChanged
    {
        private Mechanic _mechanic;
        private readonly TinyCollegeDbContextSQLServer _context;
        private List<MaintenanceLog> _allMaintenanceLogs;
        public ICommand ReleaseCommand { get; }
        public AuthorizedMechanicReleaseViewModel(Mechanic mechanic) 
        {
            _mechanic = mechanic;
            _context = new TinyCollegeDbContextSQLServer();
            ReleaseCommand = new RelayCommand(ReleaseVehicle);

            Filters = new ObservableCollection<string>
            {
                "All",
                "On Going",
                "Completed"
            };

            SelectedFilter = "All";
            LoadLogs();
        }
        private void LoadLogs()
        {
            _allMaintenanceLogs = _context.MaintenanceLogs
                .OrderByDescending(m => m.DateLogged)
                .ToList();

            ApplyFilter();
        }

        public void ReleaseVehicle(object obj)
        {
            if (SelectedMaintenance == null)
                return;

            var log = _context.MaintenanceLogs
                .FirstOrDefault(m => m.MLogId == SelectedMaintenance.MLogId);

            if (log == null)
                return;

            log.IsCompleted = true;
            log.CompleteDate = DateTime.Now;

            var vehicle = _context.Vehicles
                .FirstOrDefault(v => v.VehicleId == log.VehicleId);

            if (vehicle != null)
            {
                vehicle.IsAvailable = true;
            }

            var lastNumber = _context.VehicleReleaseForms
            .Select(v => v.VRFormId)
            .AsEnumerable()
            .Where(id => id.StartsWith("VRF-"))
            .Select(id =>
            {
                if (int.TryParse(id.Substring(4), out int num))
                    return num;
                return 999;
            })
            .DefaultIfEmpty(999)
            .Max();

            int nextNumber = lastNumber + 1;
            string newVRFormId = $"VRF-{nextNumber}";

            var releaseForm = new VehicleReleaseForm
            {
                VRFormId = newVRFormId,
                DateSigned = DateTime.Now,
                MechanicId = _mechanic.MechanicId,
                MLogId = log.MLogId
            };

            _context.VehicleReleaseForms.Add(releaseForm);

            _context.SaveChanges();

            LoadLogs();

            StatusMessage = "Vehicle Released";
            StatusMessageBrush = Brushes.Green;
        }

        private void ApplyFilter()
        {
            if (_allMaintenanceLogs == null)
                return;

            IEnumerable<MaintenanceLog> filtered = _allMaintenanceLogs;

            // 🔍 SEARCH
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                filtered = filtered.Where(m =>
                    m.MLogId != null &&
                    m.MLogId.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
            }

            // 🔽 FILTER DROPDOWN
            switch (SelectedFilter)
            {
                case "On Going":
                    filtered = filtered.Where(m => !m.IsCompleted);
                    break;

                case "Completed":
                    filtered = filtered.Where(m => m.IsCompleted);
                    break;

                case "All":
                default:
                    break;
            }

            MaintenanceLog = new ObservableCollection<MaintenanceLog>(filtered);
        }

        private ObservableCollection<MaintenanceLog> _maintenanceLog;
        public ObservableCollection<MaintenanceLog> MaintenanceLog
        {
            get => _maintenanceLog;
            set
            {
                _maintenanceLog = value;
                OnPropertyChanged(nameof(MaintenanceLog));
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
                ApplyFilter();
            }
        }

        private MaintenanceLog _selectedMaintenance;
        public MaintenanceLog SelectedMaintenance
        {
            get => _selectedMaintenance;
            set
            {
                _selectedMaintenance = value;
                OnPropertyChanged(nameof(SelectedMaintenance));
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

        public ObservableCollection<string> Filters { get; set; }

        private string _selectedFilter;
        public string SelectedFilter
        {
            get => _selectedFilter;
            set
            {
                _selectedFilter = value;
                OnPropertyChanged(nameof(SelectedFilter));
                ApplyFilter();
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
