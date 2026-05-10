using FinalProject_Jenotan.Commands;
using FinalProject_Jenotan.Database;
using FinalProject_Jenotan.Database.Models.MotorPoolSystem.Maintenance;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace FinalProject_Jenotan.ViewModel.MainFramesViewModel.MotorpoolMFViewMdoel.AuthorizedMechanicMF.AMechanicCRUD
{
    public class AMechanicEditInventoryViewModel : INotifyPropertyChanged
    {
        private readonly TinyCollegeDbContextSQLServer _context;
        private Inventory _inventory;
        public Action CloseAction { get; set; }
        public RelayCommand ConfirmCommand { get; }
        public AMechanicEditInventoryViewModel(Inventory inventory)
        {
            _inventory = inventory;
            _context = new TinyCollegeDbContextSQLServer();

            InventoryId = _inventory.InventoryId;
            ConfirmCommand = new RelayCommand(ConfirmEdit);
            LoadParts();
        }

        public void ConfirmEdit(object obj)
        {
            if (SelectedPart == null)
                return;

            var partInDb = _context.Parts
                .FirstOrDefault(p => p.PartId == SelectedPart.PartId);

            if (partInDb == null)
                return;

            partInDb.Quantity = Quantity;

            _context.SaveChanges();

            CloseAction?.Invoke();
        }

        private string _inventoryId;
        public string InventoryId
        {
            get => _inventoryId;
            set
            {
                _inventoryId = value;
                OnPropertyChanged(nameof(InventoryId));
            }
        }

        private ObservableCollection<Parts> _parts;
        public ObservableCollection<Parts> Parts
        {
            get => _parts;
            set
            {
                _parts = value;
                OnPropertyChanged(nameof(Parts));
            }
        }

        private void LoadParts()
        {
            var data = _context.Parts
                .Where(p => p.InventoryId == _inventory.InventoryId)
                .ToList();

            Parts = new ObservableCollection<Parts>(data);
        }

        private Parts _selectedPart;
        public Parts SelectedPart
        {
            get => _selectedPart;
            set
            {
                _selectedPart = value;
                OnPropertyChanged(nameof(SelectedPart));

                if (_selectedPart != null)
                {
                    PartId = _selectedPart.PartId;
                    PartName = _selectedPart.PartName;
                    Quantity = _selectedPart.Quantity;
                }
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

        private string _partName;
        public string PartName
        {
            get => _partName;
            set
            {
                _partName = value;
                OnPropertyChanged(nameof(PartName));
            }
        }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
