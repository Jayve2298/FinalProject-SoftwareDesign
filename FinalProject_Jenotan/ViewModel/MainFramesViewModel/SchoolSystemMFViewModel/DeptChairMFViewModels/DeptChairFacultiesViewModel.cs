using FinalProject_Jenotan.Commands;
using FinalProject_Jenotan.Database;
using FinalProject_Jenotan.Database.Models.EnrollmentSystem;
using FinalProject_Jenotan.PromptWindow;
using FinalProject_Jenotan.UI.EditWindow.DeanEditWindows;
using FinalProject_Jenotan.UI.EditWindow.DeptChairEditWindows;
using FinalProject_Jenotan.ViewModel.MainFramesViewModel.SchoolSystemMFViewModel.DeanMFViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace FinalProject_Jenotan.ViewModel.MainFramesViewModel.SchoolSystemMFViewModel.DeptChairMFViewModels
{

    public enum FacultyRoleFilter
    {
        All,
        Professor,
        Researcher
    }
    public class DeptChairFacultiesViewModel : INotifyPropertyChanged
    {
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand AddCommand { get; }

        private const int PageSize = 10;
        private int _currentPage = 0;
        private bool _isLoading;
        private bool _hasMoreData = true;

        private List<Faculty> _allProfessors = new();
        private List<Faculty> _filteredProfessors = new();

        private readonly Faculty _deptChair;

        public ObservableCollection<Faculty> Faculties { get; set; }

        public DeptChairFacultiesViewModel(Faculty faculty)
        {
            _deptChair = faculty;
            DeleteCommand = new RelayCommand(DeleteFaculty, CanDeleteFaculty);
            EditCommand = new RelayCommand(Edit);
            AddCommand = new RelayCommand(Add);

            Faculties = new ObservableCollection<Faculty>();
            (EditCommand as RelayCommand)?.RaiseCanExecuteChanged();
            _selectedFilter = FacultiesFilter.Active;
            LoadProfessors();

            RoleFilters = new ObservableCollection<FacultyRoleFilter>
            {
                FacultyRoleFilter.All,
                FacultyRoleFilter.Professor,
                FacultyRoleFilter.Researcher
            };

            SelectedRoleFilter = FacultyRoleFilter.All;
        }

        private bool CanDeleteFaculty(object obj)
        {
            return SelectedFaculty != null && SelectedFaculty.IsActive;
        }

        private void LoadProfessors()
        {
            using var context = new TinyCollegeDbContextSQLServer();

            var query = context.Faculties
                .Where(f => f.DepartmentId == _deptChair.DepartmentId);

            //ROLE FILTER
            query = SelectedRoleFilter switch
            {
                FacultyRoleFilter.Professor => query.Where(f => f.Role == Role.Professor),
                FacultyRoleFilter.Researcher => query.Where(f => f.Role == Role.Researcher),
                _ => query
            };

            //STATUS FILTER
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

            Faculties.Clear();

            foreach (var f in list)
                Faculties.Add(f);
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
                Faculties.Add(f);

            _currentPage++;

            if (Faculties.Count >= _filteredProfessors.Count)
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

        private FacultiesFilter _selectedFilter = FacultiesFilter.Active;
        public FacultiesFilter SelectedFilter
        {
            get => _selectedFilter;
            set
            {
                _selectedFilter = value;
                OnPropertyChanged();
                LoadProfessors();
            }
        }

        private Faculty _selectedFaculty;
        public Faculty SelectedFaculty
        {
            get => _selectedFaculty;
            set
            {
                _selectedFaculty = value;
                OnPropertyChanged();
                (DeleteCommand as RelayCommand)?.RaiseCanExecuteChanged();
                
            }
        }

        private void Add(object obj)
        {
            var window = new DeptChairAddFacultyWindow(_deptChair); //pass faculty

            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;

           

            bool? result = window.ShowDialog();

            if (result == true)
            {
                StatusMessage = "Faculty Added";
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

                LoadProfessors();
            }
        }
        private void DeleteFaculty(object obj)
        {
            if (SelectedFaculty == null || !SelectedFaculty.IsActive)
                return;

            using var context = new TinyCollegeDbContextSQLServer();

            bool hasActiveClasses = context.Classes
             .Any(c => c.FacultyId == SelectedFaculty.FacultyId && c.IsOnGoing);

            if (hasActiveClasses)
            {
                StatusMessage = "Has Active Classes.";
                StatusMessageBrush = Brushes.OrangeRed;
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

            var facultyInDb = context.Faculties
                .FirstOrDefault(f => f.FacultyId == SelectedFaculty.FacultyId);

            if (facultyInDb != null)
            {
                facultyInDb.IsActive = false;
                context.SaveChanges();

                SelectedFaculty.IsActive = false;

                StatusMessage = "Faculty Removed";
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

        private void Edit(object obj)
        {
            if (SelectedFaculty == null)
                return;

            var window = new DeptChairFaultyEditWindow(SelectedFaculty);
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

        public ObservableCollection<FacultyRoleFilter> RoleFilters { get; set; }

        private FacultyRoleFilter _selectedRoleFilter;
        public FacultyRoleFilter SelectedRoleFilter
        {
            get => _selectedRoleFilter;
            set
            {
                _selectedRoleFilter = value;
                OnPropertyChanged();
                LoadProfessors();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
