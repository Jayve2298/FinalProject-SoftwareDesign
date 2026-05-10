using FinalProject_Jenotan.Commands;
using FinalProject_Jenotan.Database;
using FinalProject_Jenotan.Database.Models.MotorPoolSystem.Maintenance;
using FinalProject_Jenotan.Database.Models.MotorPoolSystem.Reservation;
using FinalProject_Jenotan.UI.Motorpool.MainFrames.EmployeeMainFrames.EmployeeWindows;
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
    public class EmployeePartsReportViewModel : INotifyPropertyChanged
    {
        private FinalProject_Jenotan.Database.Models.MotorPoolSystem.Reservation.Employee _employee; 
        private readonly TinyCollegeDbContextSQLServer _context;
        public ICommand CreateCommand { get; }
        public ICommand UpdateCommand { get; }
        private List<PartsUsageReport> _allReports;
        public string PartId { get; set; }
        private List<PartsUsedForm> _allForms;
        public EmployeePartsReportViewModel(FinalProject_Jenotan.Database.Models.MotorPoolSystem.Reservation.Employee employee) 
        {
            _employee = employee;
            _context = new TinyCollegeDbContextSQLServer();
            CreateCommand = new RelayCommand(OpenCreate);
            UpdateCommand = new RelayCommand(UpdateReport);

            LoadReports();
        }

        public async void OpenCreate(object obj)
        {
            var window = new EmployeeCreatePartsUsageReportWindow(_employee);

            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            var result = window.ShowDialog();

            if (result == true)
            {
                LoadReports();
                StatusMessage = "Added Report";
                StatusMessageBrush = Brushes.Green;

            }
        }

        private void LoadReports()
        {
            _allForms = _context.PartsUsedForms
                .Include(p => p.PartsLink)
                .Where(p => p.IsReported == false)
                .ToList();

            _allReports = _context.PartsUsageReports
            .Where(r => r.EmployeeId == _employee.EmployeeId)
            .Include(r => r.PartsUsedFormList)
             .ThenInclude(p => p.PartsLink)
            .ToList();
            
            ApplyFilter();
        }


        public void UpdateReport(object obj)
        {
            if (SelectedReport == null) return;

            var report = _context.PartsUsageReports
                .Include(r => r.PartsUsedFormList)
                .FirstOrDefault(r => r.PURId == SelectedReport.PURId);

            if (report == null) return;

            var unreportedForms = _context.PartsUsedForms
                .Where(p => p.IsReported == false)
                .ToList();

            if (!unreportedForms.Any()) return;

            var partIds = unreportedForms
                .Select(p => p.PartsId)
                .Distinct()
                .ToList();

            var forms = _context.PartsUsedForms
                .Where(p => partIds.Contains(p.PartsId))
                .ToList();

            report.TotalPartsUsed = forms.Sum(p => p.QtyUsed);

            foreach (var form in forms)
            {
                form.PURId = report.PURId;
                form.IsReported = true;
            }

            _context.SaveChanges();

            LoadReports();

            StatusMessage = "Report Updated";
            StatusMessageBrush = Brushes.Green;
        }

        private void ApplyFilter()
        {
            if (_allReports == null) return;

            IEnumerable<PartsUsageReport> filtered = _allReports;

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                filtered = filtered.Where(r =>
                    r.PURId != null &&
                    r.PURId.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
            }

            PartsUsageReport = new ObservableCollection<PartsUsageReport>(filtered);
        }

        private Parts _partName;
        public Parts PartName
        {
            get => _partName;
            set
            {
                _partName = value;
                OnPropertyChanged(nameof(PartName));
            }
        }


        private ObservableCollection<PartsUsageReport> _partsUsageReport;
        public ObservableCollection<PartsUsageReport> PartsUsageReport
        {
            get => _partsUsageReport;
            set
            {
                _partsUsageReport = value;
                OnPropertyChanged(nameof(PartsUsageReport));
            }
        }

        private PartsUsageReport _selectedReport;
        public PartsUsageReport SelectedReport
        {
            get => _selectedReport;
            set
            {
                _selectedReport = value;
                OnPropertyChanged(nameof(SelectedReport));
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
