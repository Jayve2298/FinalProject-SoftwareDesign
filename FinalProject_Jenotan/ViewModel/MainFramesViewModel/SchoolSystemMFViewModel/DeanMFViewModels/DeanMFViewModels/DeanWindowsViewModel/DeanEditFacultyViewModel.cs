using FinalProject_Jenotan.Commands;
using FinalProject_Jenotan.Database;
using FinalProject_Jenotan.Database.Models.EnrollmentSystem;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace FinalProject_Jenotan.ViewModel.MainFramesViewModel.SchoolSystemMFViewModel.DeanMFViewModels.DeanWindowsViewModel
{
    public enum Status
    {
        Active, InActive
    }


    public class DeanEditFacultyViewModel : INotifyPropertyChanged
    {
        private readonly Action _closeWindow;
        private Faculty _faculty;
        private List<Department> _allDepartments = new();
        public ObservableCollection<Department> ActiveDepartments { get; set; } = new();
        public ObservableCollection<Status> FacultyStatus { get; set; } =
            new ObservableCollection<Status> { Status.Active, Status.InActive };

        public Status SelectedStatus
        {
            get => _selectedStatus;
            set { _selectedStatus = value; OnPropertyChanged(); }
        }
        private Status _selectedStatus;

      private string _displayDepartmentId;
        public string DisplayDepartmentId
        {
            get => _displayDepartmentId;
            set
            {
                _displayDepartmentId = value;
                OnPropertyChanged();
            }
        }

        private bool _canChangeDepartment;
        public bool CanChangeDepartment
        {
            get => _canChangeDepartment;
            set
            {
                _canChangeDepartment = value;
                OnPropertyChanged();
            }
        }
        public ICommand ConfirmCommand { get; }
        public ICommand CancelCommand { get; }
        
        public DeanEditFacultyViewModel(Faculty faculty, Action closeWindow)
        {
            _faculty = faculty;
            _closeWindow = closeWindow;

            ConfirmCommand = new RelayCommand(SaveChanges);
            CancelCommand = new RelayCommand(_ => _closeWindow());
         
            LoadData();
        }

        private void LoadData()
        {
            using var context = new TinyCollegeDbContextSQLServer();

            var schoolId = _faculty.DepartmentLink?.SchoolId;

            var departments = context.Departments
                .Where(d => d.SchoolId == schoolId && d.IsActive)
                .ToList();

            _allDepartments = departments;
            CanChangeDepartment = _faculty.Role != Role.DepartmentChair;
            ActiveDepartments = new ObservableCollection<Department>(_allDepartments);
            OnPropertyChanged(nameof(ActiveDepartments));

            DisplayDepartmentId = _faculty.DepartmentId;
            SelectedStatus = _faculty.IsActive ? Status.Active : Status.InActive;
        }

        private Department _selectedDepartment;
        public Department SelectedDepartment
        {
            get => _selectedDepartment;
            set
            {
                _selectedDepartment = value;
                OnPropertyChanged();

                
                if (!CanChangeDepartment)
                    return;

                if (_selectedDepartment != null)
                {
                    DisplayDepartmentId = _selectedDepartment.DepartmentId;
                }
            }
        }

        private void SaveChanges(object obj)
        {
            using var context = new TinyCollegeDbContextSQLServer();

            var facultyInDb = context.Faculties
                .FirstOrDefault(f => f.FacultyId == _faculty.FacultyId);

            if (facultyInDb == null)
                return;

            facultyInDb.IsActive = SelectedStatus == Status.Active;

            if (facultyInDb.Role != Role.DepartmentChair)
            {
                if (_selectedDepartment != null)
                {
                    facultyInDb.DepartmentId = _selectedDepartment.DepartmentId;
                }
            }

            context.SaveChanges();
            _closeWindow();
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                ApplySearch();
            }
        }

        private void ApplySearch()
        {
            if (_allDepartments == null)
                return;

            var filtered = _allDepartments.Where(d =>
                string.IsNullOrWhiteSpace(SearchText) ||
                (!string.IsNullOrEmpty(d.DepartmentName) &&
                 d.DepartmentName.ToLower().Contains(SearchText.ToLower()))
            ).ToList();

            ActiveDepartments = new ObservableCollection<Department>(filtered);
            OnPropertyChanged(nameof(ActiveDepartments));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

  
}
