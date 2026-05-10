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
using System.Windows.Input;
using System.Windows.Media;

namespace FinalProject_Jenotan.ViewModel.MainFramesViewModel.SchoolSystemMFViewModel.DeanMFViewModels.DeanWindowsViewModel
{
    public enum DepartmentStatus
    {
        Active, InActive
    }
    public class DeanEditDepartmentVM : INotifyPropertyChanged
    {
        private List<Faculty> _allFaculties;
        private readonly Action<bool?> _closeWindow;
        private readonly string _schoolId;
        public ICommand ConfirmCommand { get; }
        public Array DepartmentStatuses => Enum.GetValues(typeof(DepartmentStatus));
        private string _departmentId;
        public string DepartmentId
        {
            get => _departmentId;
            set
            {
                _departmentId = value;
                OnPropertyChanged();
            }
        }

        private DepartmentStatus _selectedStatus;
        public DepartmentStatus SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                _selectedStatus = value;
                OnPropertyChanged();
            }
        }

        private string _departmentName;
        public string DepartmentName
        {
            get => _departmentName;
            set
            {
                _departmentName = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Faculty> Faculties { get; set; }
        public ICommand CancelCommand { get; }
        public DeanEditDepartmentVM(Faculty dean, Action<bool?> closeWindow)
        {
            _closeWindow = closeWindow;

            CancelCommand = new RelayCommand(Cancel);
            using var context = new TinyCollegeDbContextSQLServer();

            var fullDean = context.Faculties
                .Include(f => f.DepartmentLink)
                    .ThenInclude(d => d.SchoolLink)
            .FirstOrDefault(f => f.FacultyId == dean.FacultyId);
            _schoolId = fullDean?.DepartmentLink?.SchoolId;
            LoadFaculties(dean);
            ConfirmCommand = new RelayCommand(Confirm);

        }

        private Faculty _selectedFaculty;
        public Faculty SelectedFaculty
        {
            get => _selectedFaculty;
            set
            {
                _selectedFaculty = value;
                OnPropertyChanged();

                if (_selectedFaculty != null)
                {
                    SelectedFacultyId = _selectedFaculty.FacultyId;
                }
            }
        }

        private async void Confirm(object obj)
        {
            if (SelectedDepartment == null || SelectedFaculty == null)
                return;

            if (SelectedFaculty.Role == Role.Dean || SelectedFaculty.Role == Role.Researcher || SelectedFaculty.Role == Role.DepartmentChair)
            {
                StatusMessage = "Invalid Chair";
                StatusMessageBrush = Brushes.Red;

                await Task.Delay(3000);
                StatusMessage = string.Empty;
                return;

            }

            using var context = new TinyCollegeDbContextSQLServer();

            var department = context.Departments
                .Include(d => d.FacultyLink)
                .FirstOrDefault(d => d.DepartmentId == SelectedDepartment.DepartmentId);

            if (department == null)
                return;
            

            if (department.FacultyLink != null)
            {
                department.FacultyLink.Role = Role.Professor;
            }

            var newChair = context.Faculties
                .FirstOrDefault(f => f.FacultyId == SelectedFaculty.FacultyId);

            if (newChair == null)
                return;

            newChair.Role = Role.DepartmentChair;

            if (newChair.DepartmentId != department.DepartmentId)
            {
                newChair.DepartmentId = department.DepartmentId;
            }

            department.FacultyLink = newChair;
            department.IsActive = SelectedStatus == DepartmentStatus.Active;

            context.SaveChanges();

            _closeWindow?.Invoke(true);
        }

        private void LoadFaculties(Faculty dean)
        {
            using var context = new TinyCollegeDbContextSQLServer();

            var fullDean = context.Faculties
                .Include(f => f.DepartmentLink)
                    .ThenInclude(d => d.SchoolLink)
                        .ThenInclude(s => s.DepartmentList)
                            .ThenInclude(d => d.FacultyList)
                .FirstOrDefault(f => f.FacultyId == dean.FacultyId);

                 _allFaculties = fullDean?
                .DepartmentLink?
                .SchoolLink?
                .DepartmentList?
                .SelectMany(d => d.FacultyList)
                .Where(f =>
                    f != null &&
                    f.IsActive &&
                    f.Role == Role.Professor &&
                    f.Role == Role.Researcher &&
                    f.Role == Role.DepartmentChair &&
                    f.Role == Role.Dean
                )
                .Distinct()
                .ToList() ?? new List<Faculty>();

            Faculties = new ObservableCollection<Faculty>(_allFaculties);
        }

        private Department _selectedDepartment;
        public Department SelectedDepartment
        {
            get => _selectedDepartment;
            set
            {
                _selectedDepartment = value;
                OnPropertyChanged();

                LoadSelectedDepartment();
                LoadFacultiesByDepartment();
            }
        }

        private string _displayFacultyId;
        public string DisplayFacultyId
        {
            get => _displayFacultyId;
            set
            {
                _displayFacultyId = value;
                OnPropertyChanged();
            }
        }

        private string _selectedFacultyId;
        public string SelectedFacultyId
        {
            get => _selectedFacultyId;
            set
            {
                _selectedFacultyId = value;
                OnPropertyChanged();

            }
        }

        private string _departmentChairId;
        public string DepartmentChairId
        {
            get => _departmentChairId;
            set
            {
                _departmentChairId = value;
                OnPropertyChanged();
            }
        }

        private void LoadSelectedDepartment()
        {
            if (SelectedDepartment == null) return;

            DepartmentId = SelectedDepartment.DepartmentId;
            DepartmentName = SelectedDepartment.DepartmentName;
            SelectedFaculty = Faculties?
                .FirstOrDefault(f => f.FacultyId == SelectedDepartment.FacultyLink?.FacultyId);
            SelectedStatus = SelectedDepartment.IsActive
                ? DepartmentStatus.Active
                : DepartmentStatus.InActive;

            OnPropertyChanged(nameof(DepartmentId));
            OnPropertyChanged(nameof(DepartmentName));
            OnPropertyChanged(nameof(SelectedFacultyId));
        }

        private void LoadFacultiesByDepartment()
        {
            if (SelectedDepartment == null)
                return;

            using var context = new TinyCollegeDbContextSQLServer();

            var faculties = context.Faculties
                .Where(f =>
                    f.DepartmentId == SelectedDepartment.DepartmentId &&
                    f.IsActive &&
                    f.Role == Role.Professor)
                .ToList();

            _allFaculties = faculties;

            Faculties = new ObservableCollection<Faculty>(faculties);
            OnPropertyChanged(nameof(Faculties));
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                ApplySearch();
            }
        }

        private void ApplySearch()
        {
            if (_allFaculties == null)
                return;

            var filtered = _allFaculties.Where(f =>
                string.IsNullOrWhiteSpace(SearchText) ||
                f.LName.ToLower().Contains(SearchText.ToLower())
            ).ToList();

            Faculties = new ObservableCollection<Faculty>(filtered);
            OnPropertyChanged(nameof(Faculties));
        }

        private void Cancel(object obj)
        {
            _closeWindow?.Invoke(false);
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
        private void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
