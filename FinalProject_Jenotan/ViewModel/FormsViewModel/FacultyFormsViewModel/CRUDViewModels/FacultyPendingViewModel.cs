using FinalProject_Jenotan.Database;
using FinalProject_Jenotan.Database.Models.EnrollmentSystem;
using FinalProject_Jenotan.Database.Models.MotorPoolSystem.Reservation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace FinalProject_Jenotan.ViewModel.FormsViewModel.FacultyFormsViewModel.CRUDViewModels
{
    public class FacultyPendingViewModel : INotifyPropertyChanged
    {
        private readonly TinyCollegeDbContextSQLServer _context;
        private Faculty _faculty;

        public ObservableCollection<CheckoutForm> PendingCheckoutForms { get; set; }

        public FacultyPendingViewModel(Faculty faculty)
        {
            _faculty = faculty;
            _context = new TinyCollegeDbContextSQLServer();

            LoadPendingForms();
        }

        private void LoadPendingForms()
        {
            var pending = _context.CheckoutForms
                .Where(c =>
                    c.FacultyId == _faculty.FacultyId &&
                    c.IsVerified == false)
                .OrderByDescending(c => c.CheckOutDate)
                .ToList();

            PendingCheckoutForms = new ObservableCollection<CheckoutForm>(pending);

            OnPropertyChanged(nameof(PendingCheckoutForms));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
