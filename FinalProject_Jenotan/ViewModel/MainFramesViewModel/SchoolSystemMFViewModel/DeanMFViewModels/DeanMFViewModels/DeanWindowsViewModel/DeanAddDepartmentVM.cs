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
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace FinalProject_Jenotan.ViewModel.MainFramesViewModel.SchoolSystemMFViewModel.DeanMFViewModels.DeanWindowsViewModel
{
    public class DeanAddDepartmentVM : INotifyPropertyChanged
    {
        private List<Faculty> _allFaculties;
        private readonly string _schoolId;
        public ObservableCollection<Faculty> Faculties { get; set; }
        public ICommand AddCommand { get; }
        public ICommand CancelCommand { get; }

        private readonly Action<bool?> _closeWindow;

        public DeanAddDepartmentVM(Faculty dean, Action<bool?> closeWindow)
        {
            _closeWindow = closeWindow;
            DepartmentId = GenerateDepartmentId(); ;
            CancelCommand = new RelayCommand(Cancel);
            AddCommand = new RelayCommand(AddDepartment);

            using var context = new TinyCollegeDbContextSQLServer();

            var fullDean = context.Faculties
                .Include(f => f.DepartmentLink)
                    .ThenInclude(d => d.SchoolLink)
            .FirstOrDefault(f => f.FacultyId == dean.FacultyId);

            _schoolId = fullDean?.DepartmentLink?.SchoolId;

            LoadFaculties(dean);
        }



        private async void AddDepartment(object obj)
        {
            

            if (SelectedFaculty == null)
            {
                StatusMessage = "Select Faculty";
                StatusMessageBrush = Brushes.Red;
                await Task.Delay(3000);
                StatusMessage = string.Empty;
                return;
            }

            if (DepartmentName == null)
            {
                StatusMessage = "Input Name";
                StatusMessageBrush = Brushes.Red;
                await Task.Delay(3000);
                StatusMessage = string.Empty;
                return;
            }

            using (var context = new TinyCollegeDbContextSQLServer())
            {
                bool exists = context.Departments
                    .Any(d => d.DepartmentId == DepartmentId);

                if (exists)
                {
                    MessageBox.Show("Department ID already exists.");
                    return;
                }

                var faculty = context.Faculties
                    .FirstOrDefault(f => f.FacultyId == SelectedFaculty.FacultyId);

                if (faculty == null)
                    return;

                var newDepartment = new Department
                {
                    DepartmentId = DepartmentId,
                    FacultyId = faculty.FacultyId,
                    SchoolId = _schoolId,
                    DepartmentName = DepartmentName,
                    IsActive = true
                };

                context.Departments.Add(newDepartment);
                faculty.Role = Role.DepartmentChair;
                faculty.DepartmentId = DepartmentId;
                context.SaveChanges();
                context.ChangeTracker.Clear();

                _closeWindow?.Invoke(true);


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
                    f.Role == Role.Professor
                )
                .Distinct()
                .ToList() ?? new List<Faculty>();

            Faculties = new ObservableCollection<Faculty>(_allFaculties);
        }

        private string GenerateDepartmentId()
        {
            using var context = new TinyCollegeDbContextSQLServer();

            var lastDepartment = context.Departments
                .AsEnumerable() // important if string sorting is unreliable
                .Where(d => d.DepartmentId.StartsWith("DEPT - "))
                .OrderByDescending(d =>
                {
                    var numPart = d.DepartmentId.Replace("DEPT - ", "");
                    return int.TryParse(numPart, out int n) ? n : 0;
                })
                .FirstOrDefault();

            int nextNumber = 1000;

            if (lastDepartment != null)
            {
                var numberPart = lastDepartment.DepartmentId.Replace("DEPT - ", "");

                if (int.TryParse(numberPart, out int lastNumber))
                {
                    nextNumber = lastNumber + 1;
                }
            }

            return $"DEPT - {nextNumber}";
        }

        private Faculty _selectedFaculty;

        public string SelectedFacultyId =>
    SelectedFaculty?.FacultyId ?? "";

        public Faculty SelectedFaculty
        {
            get => _selectedFaculty;
            set
            {
                _selectedFaculty = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedFacultyId));
            }
        }

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
