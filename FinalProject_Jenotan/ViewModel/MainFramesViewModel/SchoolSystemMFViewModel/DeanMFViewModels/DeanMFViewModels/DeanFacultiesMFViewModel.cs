using FinalProject_Jenotan.Commands;
using FinalProject_Jenotan.Database;
using FinalProject_Jenotan.Database.Models.EnrollmentSystem;
using FinalProject_Jenotan.PromptWindow;
using FinalProject_Jenotan.UI.EditWindow.DeanEditWindows;
using FinalProject_Jenotan.ViewModel.MainFramesViewModel.SchoolSystemMFViewModel.DeanMFViewModels.DeanWindowsViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace FinalProject_Jenotan.ViewModel.MainFramesViewModel.SchoolSystemMFViewModel.DeanMFViewModels
{
    public enum FacultiesFilter
    {
        All,
        Active,
        Inactive
    }


    public class DeanFacultiesMFViewModel : INotifyPropertyChanged
    {
        private const int PageSize = 6;
        private int _currentPage = 0;
        private bool _isLoading;
        private bool _hasMoreData = true;
        private List<Faculty> _filteredProfessors = new();

        public DeanFacultiesMFViewModel(Faculty dean)
        {
            _faculty = dean;
            EditCommand = new RelayCommand(Edit);
            DeleteCommand = new RelayCommand(DeleteFaculty, CanDeleteFaculty);
            Faculties = new ObservableCollection<Faculty>();
            LoadDepartments();
        }

        private List<Faculty> _allProfessors = new();
        private Faculty _faculty;
        public ObservableCollection<Faculty> Faculties { get; set; }
        private readonly Faculty _dean;
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand AddCommand { get; }

        private Faculty _selectedFaculty;
        public Faculty SelectedFaculty
        {
            get => _selectedFaculty;
            set
            {
                _selectedFaculty = value;
                OnPropertyChanged();
                (EditCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (DeleteCommand as RelayCommand)?.RaiseCanExecuteChanged();

            }
        }

        private bool CanDeleteFaculty(object obj)
        {
           // return SelectedFaculty != null && SelectedFaculty.IsActive;
            return SelectedFaculty != null;
        }

        private async void DeleteFaculty(object obj)
        {
            if (SelectedFaculty == null || !SelectedFaculty.IsActive)
                return;

            if (SelectedFaculty.Role == Role.DepartmentChair)
            {
                StatusMessage = "Cannot Delete";
                StatusMessageBrush = Brushes.Red;
                await Task.Delay(3000);
                StatusMessage = string.Empty;
                return;
            }

            var confirmWindow = new ConfirmationWindow
            {
                Owner = Application.Current.MainWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            confirmWindow.ShowDialog();


            if (!confirmWindow.result)
                return;

            using var context = new TinyCollegeDbContextSQLServer();
            var facultyInDb = context.Faculties
                .FirstOrDefault(f => f.FacultyId == SelectedFaculty.FacultyId);


            if (facultyInDb != null)
            {
                facultyInDb.IsActive = false;
                context.SaveChanges();

                SelectedFaculty.IsActive = false;


                StatusMessage = "Faculty Fired!";
                StatusMessageBrush = Brushes.Red;
                var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(3) };
                timer.Tick += (s, e) =>
                {
                    StatusMessage = string.Empty;
                    timer.Stop();
                };
                timer.Start();

                (DeleteCommand as RelayCommand)?.RaiseCanExecuteChanged();

                ApplySearch();
                LoadProfessors();
            }
              
        }

        public ObservableCollection<Department> Departments { get; set; } = new();

        private Department _selectedDepartment;
        public Department SelectedDepartment
        {
            get => _selectedDepartment;
            set
            {
                _selectedDepartment = value;
                OnPropertyChanged();
                LoadProfessors();
            }
        }

        private void LoadProfessors()
        {
            if (SelectedDepartment == null) return;

            using var context = new TinyCollegeDbContextSQLServer();

            var query = context.Faculties
                .Include(f => f.DepartmentLink)
                .Where(f =>
                    f.DepartmentId == SelectedDepartment.DepartmentId &&
                    f.Role != Role.Dean &&
                    f.IsActive == true
                );

            switch (SelectedFilter)
            {
                case FacultiesFilter.Active:
                    query = query.Where(f => f.IsActive);
                    break;

                case FacultiesFilter.Inactive:
                    query = query.Where(f => !f.IsActive);
                    break;
            }

            _allProfessors = query.ToList();

            ApplySearch();
        }

        private void ApplySearch()
        {
            IEnumerable<Faculty> list = _allProfessors;

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                list = list.Where(f =>
                    (!string.IsNullOrEmpty(f.FName) &&
                     f.FName.ToLower().Contains(SearchText.ToLower())) ||
                    (!string.IsNullOrEmpty(f.LName) &&
                     f.LName.ToLower().Contains(SearchText.ToLower()))
                );
            }

            _filteredProfessors = list.ToList();

            Faculties.Clear();
            _currentPage = 0;
            _hasMoreData = true;

            LoadMore();
        }

        private void LoadDepartments()
        {
            using var context = new TinyCollegeDbContextSQLServer();

            var schoolId = context.Faculties
                .Include(f => f.DepartmentLink)
                .FirstOrDefault(f => f.FacultyId == _faculty.FacultyId)?
                .DepartmentLink?
                .SchoolId;

            var departments = context.Departments
                .Where(d => d.SchoolId == schoolId)
                .ToList();

            Departments = new ObservableCollection<Department>(departments);
            OnPropertyChanged(nameof(Departments));

            if (Departments.Any())
            {
                SelectedDepartment = Departments.First();
            }
        }

        public void LoadMore()
        {
            if (_isLoading || !_hasMoreData)
                return;

            _isLoading = true;

            var nextBatch = _filteredProfessors
                .Skip(_currentPage * PageSize)
                .Take(PageSize)
                .ToList();

            foreach (var f in nextBatch)
            {
                Faculties.Add(f);
            }

            _currentPage++;

            if (Faculties.Count >= _allProfessors.Count)
                _hasMoreData = false;

            _isLoading = false;
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

        public ObservableCollection<FacultiesFilter> Filters { get; set; } =
                new ObservableCollection<FacultiesFilter>
                {
                    FacultiesFilter.All,
                    FacultiesFilter.Active,
                    FacultiesFilter.Inactive
                };


        private FacultiesFilter _selectedFilter = FacultiesFilter.All;
        public FacultiesFilter SelectedFilter
        {
            get => _selectedFilter;
            set
            {
                _selectedFilter = value;
                OnPropertyChanged();

                ApplyFilters();
            }
        }

        private void ApplyFilters()
        {
            LoadProfessors();
        }

        private void Edit(object obj)
        {
            if (SelectedFaculty == null)
                return;

            var window = new DeanEditFacultyWindow(SelectedFaculty);
            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            bool? result = window.ShowDialog();

            if (result == true)
            {
                StatusMessage = "Changes Saved";
                StatusMessageBrush = Brushes.Green;

                var timer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromSeconds(3)
                };
                timer.Tick += (s, e) =>
                {
                    StatusMessage = string.Empty;
                    timer.Stop();
                };
                timer.Start();
            }

          
            LoadProfessors();
            ApplySearch();
        }

        private string _statusMessage;
        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged();
            }
        }

        private Brush _statusMessageBrush = Brushes.Black;
        public Brush StatusMessageBrush
        {
            get => _statusMessageBrush;
            set
            {
                _statusMessageBrush = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
