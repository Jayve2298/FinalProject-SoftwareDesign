using FinalProject_Jenotan.Commands;
using FinalProject_Jenotan.Database;
using FinalProject_Jenotan.Database.Models.MotorPoolSystem.Maintenance;
using FinalProject_Jenotan.Database.Models.MotorPoolSystem.Reservation;
using FinalProject_Jenotan.UI.Motorpool.MainFrames.AuthorizedMechanicMainFrames.AMechanicWindow;
using FinalProject_Jenotan.UI.Motorpool.MainFrames.EmployeeMainFrames.EmployeeWindows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace FinalProject_Jenotan.ViewModel.MainFramesViewModel.MotorpoolMFViewMdoel.AuthorizedMechanicMF
{
    public class AuthorizedMechanicInventoryViewModel : INotifyPropertyChanged
    {
        private readonly TinyCollegeDbContextSQLServer _context;
        private Mechanic _mechanic;
        private List<Inventory> _allInventories;
        public ICommand EditCommand { get; }
        public AuthorizedMechanicInventoryViewModel(Mechanic mechanic)
        {
            _mechanic = mechanic;
            _context = new TinyCollegeDbContextSQLServer();
            EditCommand = new RelayCommand(OpenEdit);
            LoadInventories();
        }

        public async void OpenEdit(object obj)
        {
            var window = new AMechanicEditInventoryWindow(SelectedInventory);

            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            var result = window.ShowDialog();

            if (result == true)
            {
                LoadInventories();
                StatusMessage = "Edit Success";
                StatusMessageBrush = Brushes.Green;

            }
        }
        private void LoadInventories()
        {
            _allInventories = _context.Inventories
                .Where(i => i.MechanicId == _mechanic.MechanicId)
                .ToList();

            ApplyFilter();
        }

        private void ApplyFilter()
        {
            if (_allInventories == null)
                return;

            IEnumerable<Inventory> filtered = _allInventories;

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                filtered = filtered.Where(i =>
                    i.InventoryId != null &&
                    i.InventoryId.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
            }

            Inventories = new ObservableCollection<Inventory>(filtered);
        }

        private ObservableCollection<Inventory> _inventories;
        public ObservableCollection<Inventory> Inventories
        {
            get => _inventories;
            set
            {
                _inventories = value;
                OnPropertyChanged(nameof(Inventories));
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
