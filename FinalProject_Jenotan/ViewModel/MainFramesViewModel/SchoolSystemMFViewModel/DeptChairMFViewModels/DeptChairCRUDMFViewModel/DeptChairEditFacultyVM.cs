using FinalProject_Jenotan.Commands;
using FinalProject_Jenotan.Database;
using FinalProject_Jenotan.Database.Models.EnrollmentSystem;
using FinalProject_Jenotan.ViewModel.MainFramesViewModel.SchoolSystemMFViewModel.DeanMFViewModels.DeanWindowsViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace FinalProject_Jenotan.ViewModel.MainFramesViewModel.SchoolSystemMFViewModel.DeptChairMFViewModels.DeptChairCRUDMFViewModel
{
    public class DeptChairEditFacultyVM : INotifyPropertyChanged
    {
        private Faculty _faculty;
        private readonly Action _closeWindow;
        public ObservableCollection<Status> FacultyStatus { get; set; } =
            new ObservableCollection<Status> { Status.Active, Status.InActive };
        public ICommand ConfirmCommand { get; }
        public DeptChairEditFacultyVM(Faculty faculty, Action closeWindow)
        {
            _closeWindow = closeWindow;
            _faculty = faculty;
            ConfirmCommand = new RelayCommand(SaveChanges);
            SelectedStatus = _faculty.IsActive ? Status.Active : Status.InActive;
        }

        private Status _selectedStatus;
        public Status SelectedStatus
        {
            get => _selectedStatus;
            set { _selectedStatus = value; OnPropertyChanged(); }
        }

        private void SaveChanges(object obj)
        {
            using var context = new TinyCollegeDbContextSQLServer();

            var facultyInDb = context.Faculties
                .FirstOrDefault(f => f.FacultyId == _faculty.FacultyId);

            if (facultyInDb != null)
            {
                facultyInDb.IsActive = SelectedStatus == Status.Active;
                context.SaveChanges();
            }

            Application.Current.Windows
                .OfType<Window>()
                .SingleOrDefault(w => w.IsActive).DialogResult = true;

            _closeWindow();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
