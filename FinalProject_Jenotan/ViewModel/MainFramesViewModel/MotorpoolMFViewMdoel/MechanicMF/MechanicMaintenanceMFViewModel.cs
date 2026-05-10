using FinalProject_Jenotan.Commands;
using FinalProject_Jenotan.Database;
using FinalProject_Jenotan.Database.Models.MotorPoolSystem.Maintenance;
using FinalProject_Jenotan.Database.Models.MotorPoolSystem.Reservation;
using FinalProject_Jenotan.UI.Motorpool.MainFrames.EmployeeMainFrames.EmployeeWindows;
using FinalProject_Jenotan.UI.Motorpool.MainFrames.MechanicMainFrames.MechanicWindows;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace FinalProject_Jenotan.ViewModel.MainFramesViewModel.MotorpoolMFViewMdoel.MechanicMF
{
    public class MechanicMaintenanceMFViewModel : INotifyPropertyChanged
    {
        private Mechanic _mechanic;
        private readonly TinyCollegeDbContextSQLServer _context;
        public ICommand AddCommand { get; }
        private List<MaintenanceDetails> _allMaintenanceDetails;
        public MechanicMaintenanceMFViewModel(Mechanic mechanic) 
        {
            _mechanic = mechanic;
            _context = new TinyCollegeDbContextSQLServer();
            AddCommand = new RelayCommand(OpenAdd);

            Filters = new ObservableCollection<string>
            {
                "All",
                "Latest",
                "Oldest"
            };

            SelectedFilter = "All";

            LoadMaintenanceDetails();
        }

        public async void OpenAdd(object obj)
        {
            var window = new MechanicAddMaintenaceWindow(_mechanic);

            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            var result = window.ShowDialog();

            if (result == true)
            {
                LoadMaintenanceDetails();
                StatusMessage = "Maintenance Completed";
                StatusMessageBrush = Brushes.Green;

            }
        }

        private void LoadMaintenanceDetails()
        {
            _allMaintenanceDetails = _context.MaintenanceDetails
                .Where(m => m.MechanicId == _mechanic.MechanicId)
                .ToList();

            ApplyFilter();
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

        private ObservableCollection<MaintenanceDetails> _maintenanceDetails;
        public ObservableCollection<MaintenanceDetails> MaintenanceDetails
        {
            get => _maintenanceDetails;
            set
            {
                _maintenanceDetails = value;
                OnPropertyChanged(nameof(MaintenanceDetails));
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

        public ObservableCollection<string> Filters { get; set; }

        private string _selectedFilter;
        public string SelectedFilter
        {
            get => _selectedFilter;
            set
            {
                _selectedFilter = value;
                OnPropertyChanged(nameof(SelectedFilter));
                ApplyFilter(); // 🔥 trigger filtering
            }
        }

        private void ApplyFilter()
        {
            if (_allMaintenanceDetails == null) return;

            IEnumerable<MaintenanceDetails> filtered = _allMaintenanceDetails;
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                filtered = filtered.Where(x =>
                    x.MDetailsId != null &&
                    x.MDetailsId.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
            }

            switch (SelectedFilter)
            {
                case "Latest":
                    filtered = filtered.OrderByDescending(x => x.DateLogged);
                    break;

                case "Oldest":
                    filtered = filtered.OrderBy(x => x.DateLogged);
                    break;
            }

            MaintenanceDetails = new ObservableCollection<MaintenanceDetails>(filtered);
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
