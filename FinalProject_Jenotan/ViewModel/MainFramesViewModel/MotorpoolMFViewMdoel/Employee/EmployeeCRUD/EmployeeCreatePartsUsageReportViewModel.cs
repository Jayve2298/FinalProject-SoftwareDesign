using FinalProject_Jenotan.Commands;
using FinalProject_Jenotan.Database;
using FinalProject_Jenotan.Database.Models.MotorPoolSystem.Maintenance;
using FinalProject_Jenotan.Database.Models.MotorPoolSystem.Reservation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace FinalProject_Jenotan.ViewModel.MainFramesViewModel.MotorpoolMFViewMdoel.Employee.EmployeeCRUD
{
    public class EmployeeCreatePartsUsageReportViewModel : INotifyPropertyChanged
    {
        private FinalProject_Jenotan.Database.Models.MotorPoolSystem.Reservation.Employee _employee;
        public Action CloseAction { get; set; }
        private readonly TinyCollegeDbContextSQLServer _context;
        public ICommand CreateCommand { get; }
        public EmployeeCreatePartsUsageReportViewModel(FinalProject_Jenotan.Database.Models.MotorPoolSystem.Reservation.Employee employee) 
        {
            _employee = employee;
            _context = new TinyCollegeDbContextSQLServer();
            PURId = GeneratePURId();
            CreateCommand = new RelayCommand(CreateForm);
            LoadParts();
        }

        public async void CreateForm(object obj)
        {
            if (SelectedPartUsedForm == null)
            {
                StatusMessage = "Please select a part used form.";
                StatusBrush = Brushes.Red;
                return;
            }

            PURId = GeneratePURId();
            var partForm = _context.PartsUsedForms
                .FirstOrDefault(p => p.PUFormId == SelectedPartUsedForm.PUFormId);

            if (partForm == null)
            {
                StatusMessage = "Part form not found.";
                StatusBrush = Brushes.Red;
                return;
            }
            //if (partForm.IsReported)
            //{
            //    StatusMessage = "This part is already reported.";
            //    StatusBrush = Brushes.OrangeRed;
            //    return;
            //}

            var report = new PartsUsageReport
            {
                PURId = PURId,
                TotalPartsUsed = QtyUsed,
                EmployeeId = _employee.EmployeeId
                
            };

            _context.PartsUsageReports.Add(report);

            // mark form as reported
            partForm.IsReported = true;
            partForm.PURId = PURId;

            _context.SaveChanges();

            CloseAction?.Invoke();
        }

        private void LoadParts()
        {
            var data = _context.PartsUsedForms
                .Include(p => p.PartsLink)
                .Where(p => p.IsReported ==  false || p.PURId == null)
                .ToList();

            PartUsedForm = new ObservableCollection<PartsUsedForm>(data);
        }

        private ObservableCollection<PartsUsedForm> _partUsedForm;
        public ObservableCollection<PartsUsedForm> PartUsedForm
        {
            get => _partUsedForm;
            set
            {
                _partUsedForm = value;
                OnPropertyChanged(nameof(PartUsedForm));
            }
        }


        private string _purId;
        public string PURId
        {
            get => _purId;
            set
            {
                _purId = value;
                OnPropertyChanged(nameof(PURId));
            }
        }

        private string GeneratePURId()
        {
            var lastNumber = _context.PartsUsageReports
                .Select(p => p.PURId)
                .AsEnumerable()
                .Where(id => id.StartsWith("PUR-"))
                .Select(id =>
                {
                    if (int.TryParse(id.Substring(4), out int num))
                        return num;
                    return 999;
                })
                .DefaultIfEmpty(999)
                .Max();

            int nextNumber = lastNumber + 1;

            return $"PUR-{nextNumber}";
        }

        private PartsUsedForm _selectedPartUsedForm;
        public PartsUsedForm SelectedPartUsedForm
        {
            get => _selectedPartUsedForm;
            set
            {
                _selectedPartUsedForm = value;
                OnPropertyChanged(nameof(SelectedPartUsedForm));

                if (_selectedPartUsedForm != null)
                {
                    PUFormId = _selectedPartUsedForm.PUFormId;
                    PartId = _selectedPartUsedForm.PartsId;
                    QtyUsed = _selectedPartUsedForm.QtyUsed;
                }
            }
        }

        private int _qtyUsed;
        public int QtyUsed
        {
            get => _qtyUsed;
            set
            {
                _qtyUsed = value;
                OnPropertyChanged(nameof(QtyUsed));
            }
        }

        private string _partId;
        public string PartId
        {
            get => _partId;
            set
            {
                _partId = value;
                OnPropertyChanged(nameof(PartId));
            }
        }

        private string _puFormId;
        public string PUFormId
        {
            get => _puFormId;
            set
            {
                _puFormId = value;
                OnPropertyChanged(nameof(PUFormId));
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

        private Brush _statusBrush = Brushes.Black;
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
