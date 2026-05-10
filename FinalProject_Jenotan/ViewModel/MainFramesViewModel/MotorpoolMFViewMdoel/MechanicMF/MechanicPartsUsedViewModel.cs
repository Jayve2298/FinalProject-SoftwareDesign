using FinalProject_Jenotan.Database;
using FinalProject_Jenotan.Database.Models.MotorPoolSystem.Maintenance;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace FinalProject_Jenotan.ViewModel.MainFramesViewModel.MotorpoolMFViewMdoel.MechanicMF
{
    public class MechanicPartsUsedViewModel : INotifyPropertyChanged
    {
        private readonly TinyCollegeDbContextSQLServer _context;
        private Mechanic _mechanic;

        public MechanicPartsUsedViewModel(Mechanic mechanic)
        {
            _mechanic = mechanic;
            _context = new TinyCollegeDbContextSQLServer();
            LoadForms();
        }

        public void LoadForms()
        {
            var data = _context.PartsUsedForms
                .Where(m => m.MechanicId == _mechanic.MechanicId)
                .ToList();

            PartsUsedForms = new ObservableCollection<PartsUsedForm>(data);
        }

        private PartsUsedForm _partsUsedForm;
        public PartsUsedForm PartsUsedForm
        {
            get => _partsUsedForm;
            set
            {
                _partsUsedForm = value;
                OnPropertyChanged(nameof(PartsUsedForm));
            }
        }

        private ObservableCollection<PartsUsedForm> _partsUsedForms;
        public ObservableCollection<PartsUsedForm> PartsUsedForms
        {
            get => _partsUsedForms;
            set
            {
                _partsUsedForms = value;
                OnPropertyChanged(nameof(PartsUsedForms));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
