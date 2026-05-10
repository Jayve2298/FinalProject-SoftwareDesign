using FinalProject_Jenotan.Commands;
using FinalProject_Jenotan.Database;
using FinalProject_Jenotan.Database.Models.MotorPoolSystem.Maintenance;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace FinalProject_Jenotan.ViewModel.MainFramesViewModel.MotorpoolMFViewMdoel.MechanicMF.MechanicCRUD
{
    public class MechanicAddMaintenanceDetailsViewModel : INotifyPropertyChanged
    {
        private readonly TinyCollegeDbContextSQLServer _context;
        private Mechanic _mechanic;
        public Action CloseAction { get; set; }
        public ICommand AddCommand { get; set; }
        public ObservableCollection<MaintenanceLog> MaintenanceLog { get; set; }
        public ObservableCollection<Inventory> Inventory { get; set; }
        public ObservableCollection<Parts> Part { get; set; }
        public string MDetailsId { get; set; }

        public MechanicAddMaintenanceDetailsViewModel(Mechanic mechanic) 
        {
            _mechanic = mechanic;
            _context = new TinyCollegeDbContextSQLServer();

            AddCommand = new RelayCommand(AddMaintenance);
            

            LoadLogs();
            LoadInventories();

            MDetailsId = GenerateMDetailsId();
        }

        public void AddMaintenance(object obj)
        {
            ErrorMessage = "";

            if (SelectedLog == null)
            {
                ErrorMessage = "Select a Maintenance Log";
                return;
            }

            if (SelectedPart == null)
            {
                ErrorMessage = "Select a Part";
                return;
            }

            if (string.IsNullOrWhiteSpace(MaintenancePerformed))
            {
                ErrorMessage = "Enter maintenance details";
                return;
            }

            if (QTYUsed <= 0)
            {
                ErrorMessage = "Invalid quantity";
                return;
            }

            var maintenanceDetails = new MaintenanceDetails
            {
                MDetailsId = MDetailsId,
                MaintenancePerformed = MaintenancePerformed,
                DateLogged = DateTime.Now,
                CompleteDate = DateTime.Now,

                MechanicId = _mechanic.MechanicId,
                MLogId = SelectedLog.MLogId
            };

            _context.MaintenanceDetails.Add(maintenanceDetails);

            var partsUsed = new PartsUsedForm
            {
                PUFormId = GeneratePartsUsedFormId(),
                QtyUsed = QTYUsed,

                PartsId = SelectedPart.PartId,
                MLogId = SelectedLog.MLogId,
                PURId = null,

                MechanicId = _mechanic.MechanicId
            };

            _context.PartsUsedForms.Add(partsUsed);
            SelectedPart.Quantity -= QTYUsed;

            _context.SaveChanges();

            CloseAction?.Invoke();
        }

        private void LoadLogs()
        {
            var logs = _context.MaintenanceLogs
                .Where(m => !m.IsCompleted)
                .ToList();

            MaintenanceLog = new ObservableCollection<MaintenanceLog>(logs);
            OnPropertyChanged(nameof(MaintenanceLog));
        }

        private void LoadInventories()
        {
            var inventories = _context.Inventories
                .ToList();

            Inventory = new ObservableCollection<Inventory>(inventories);
            OnPropertyChanged(nameof(Inventory));
        }

        private void LoadParts()
        {
            if (SelectedInventory == null)
            {
                Part = new ObservableCollection<Parts>();
            }
            else
            {
                var parts = _context.Parts
                    .Where(p => p.InventoryId == SelectedInventory.InventoryId)
                    .ToList();

                Part = new ObservableCollection<Parts>(parts);
            }

            OnPropertyChanged(nameof(Part));
        }

        private string GeneratePartsUsedFormId()
        {
            var last = _context.PartsUsedForms
                .AsEnumerable()
                .OrderByDescending(p =>
                {
                    var num = p.PUFormId.Replace("PFR", "");
                    return int.TryParse(num, out int n) ? n : 0;
                })
                .FirstOrDefault();

            int next = 1000;

            if (last != null)
            {
                var num = last.PUFormId.Replace("PFR", "");
                if (int.TryParse(num, out int n))
                    next = n + 1;
            }

            return $"PFR{next}";
        }

        private MaintenanceLog _selectedLog;
        public MaintenanceLog SelectedLog
        {
            get => _selectedLog;
            set
            {
                _selectedLog = value;
                OnPropertyChanged(nameof(SelectedLog));

                if (_selectedLog != null)
                    MLogId = _selectedLog.MLogId;
            }
        }

        private Inventory _selectedInventory;
        public Inventory SelectedInventory
        {
            get => _selectedInventory;
            set
            {
                _selectedInventory = value;
                OnPropertyChanged(nameof(SelectedInventory));

                LoadParts();
            }
        }

        private Parts _selectedPart;
        public Parts SelectedPart
        {
            get => _selectedPart;
            set
            {
                _selectedPart = value;
                OnPropertyChanged(nameof(SelectedPart));
            }
        }

        private string _mLogId;
        public string MLogId
        {
            get => _mLogId;
            set
            {
                _mLogId = value;
                OnPropertyChanged(nameof(MLogId));
            }
        }

        private string _maintenancePerformed;
        public string MaintenancePerformed
        {
            get => _maintenancePerformed;
            set
            {
                _maintenancePerformed = value;
                OnPropertyChanged(nameof(MaintenancePerformed));
            }
        }

        private int _qtyUsed;
        public int QTYUsed
        {
            get => _qtyUsed;
            set
            {
                if (SelectedPart != null && value > SelectedPart.Quantity)
                {
                    ErrorMessage = "Exceeds available stock";
                    return;
                }

                if (value < 0)
                {
                    ErrorMessage = "Invalid quantity";
                    return;
                }

                _qtyUsed = value;
                ErrorMessage = "";
                OnPropertyChanged(nameof(QTYUsed));
            }
        }

        private string GenerateMDetailsId()
        {
            var last = _context.MaintenanceDetails
                .AsEnumerable()
                .OrderByDescending(m =>
                {
                    var num = m.MDetailsId.Replace("MTD - ", "");
                    return int.TryParse(num, out int n) ? n : 0;
                })
                .FirstOrDefault();

            int next = 1000;

            if (last != null)
            {
                var num = last.MDetailsId.Replace("MTD - ", "");
                if (int.TryParse(num, out int n))
                    next = n + 1;
            }

            return $"MTD - {next}";
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
