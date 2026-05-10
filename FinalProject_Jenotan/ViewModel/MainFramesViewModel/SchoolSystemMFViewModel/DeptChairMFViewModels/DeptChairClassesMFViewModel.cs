using FinalProject_Jenotan.Commands;
using FinalProject_Jenotan.Database;
using FinalProject_Jenotan.Database.Models.EnrollmentSystem;
using FinalProject_Jenotan.PromptWindow;
using FinalProject_Jenotan.UI.EditWindow.DeptChairEditWindows;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace FinalProject_Jenotan.ViewModel.MainFramesViewModel.SchoolSystemMFViewModel.DeptChairMFViewModels
{

    public class DeptChairClassesMFViewModel : INotifyPropertyChanged
    {
        private readonly Faculty _deptChair;
        public ICommand AddCommand { get;  }
        public ICommand DeleteCommand { get; }
        public ICommand EditCommand { get; }
        public ObservableCollection<Class> Classes { get; set; } = new();

        private List<Class> _allClasses = new();
        public DeptChairClassesMFViewModel(Faculty deptChair)
        {
            _deptChair = deptChair;
            AddCommand = new RelayCommand(OpenAddClass);
            DeleteCommand = new RelayCommand(DeleteClass, CanDeleteClass);
            EditCommand = new RelayCommand(OpenEditClass, CanEditClass);
            LoadClasses();
        }

        private bool CanDeleteClass(object obj)
        {
            return SelectedClass != null;
        }

        public void OpenEditClass(object obj)
        {
            if (SelectedClass == null) return;

            var window = new DeptChairEditClassesWindow(_deptChair, SelectedClass);

            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            var result = window.ShowDialog();

            if (result == true)
            {
                LoadClasses();

                StatusMessage = "Success";
                StatusMessageBrush = Brushes.Green;
            }
        }

        public void OpenAddClass(object obj)
        {
            var window = new DeptChairAddClassWindow(_deptChair);

            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            var result = window.ShowDialog();

            if (result == true)
            {
                LoadClasses();

                StatusMessage = "Added Class";
                StatusMessageBrush = Brushes.Green;
            }
        }

        private void DeleteClass(object obj)
        {
            if (SelectedClass == null)
                return;

            var confirmWindow = new ConfirmationWindow
            {
                Owner = Application.Current.MainWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            confirmWindow.ShowDialog();


            if (!confirmWindow.result)
                return;

            using var context = new TinyCollegeDbContextSQLServer();

            var classToDelete = context.Classes
                .FirstOrDefault(c => c.ClassId == SelectedClass.ClassId);

            if (classToDelete != null)
            {

                classToDelete.IsOnGoing = false;

                context.SaveChanges();
            }
            LoadClasses();

            StatusMessage = "Class Deleted";
            StatusMessageBrush = Brushes.Red;
        }

        public List<string> Filters { get; set; } = new()
        {
            "All",
            "On Going",
            "Finished"
        };

        private string _selectedFilter = "On Going";
        public string SelectedFilter
        {
            get => _selectedFilter;
            set
            {
                _selectedFilter = value;
                OnPropertyChanged(nameof(SelectedFilter));
                ApplyFilter();
            }
        }

        private Class _selectedClass;
        public Class SelectedClass
        {
            get => _selectedClass;
            set
            {
                _selectedClass = value;
                OnPropertyChanged(nameof(SelectedClass));
                OnPropertyChanged(nameof(CanEditOwnClass));

                (DeleteCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (EditCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }


        private void ApplyFilter()
        {
            IEnumerable<Class> filtered = _allClasses;

            if (SelectedFilter == "On Going")
            {
                filtered = filtered.Where(c => c.IsOnGoing);
            }
            else if (SelectedFilter == "Finished")
            {
                filtered = filtered.Where(c => !c.IsOnGoing);
            }

            if (IsOwnClassOnly)
            {
                filtered = filtered.Where(c => c.FacultyId == _deptChair.FacultyId);
            }

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var search = SearchText.ToLower();

                filtered = filtered.Where(c =>
                    (c.ClassId != null && c.ClassId.ToLower().Contains(search)) ||
                    (c.FacultyId != null && c.FacultyId.ToLower().Contains(search)) ||
                    c.DayOfWeek.ToString().ToLower().Contains(search)
                );
            }

            Classes.Clear();
            foreach (var item in filtered)
                Classes.Add(item);
        }

        private void LoadClasses()
        {
            using var context = new TinyCollegeDbContextSQLServer();

            var deptId = _deptChair.DepartmentId;

            _allClasses = context.Classes
                .Include(c => c.CourseLink)
                .Where(c => c.CourseLink.DepartmentId == deptId)
                .ToList();

            ApplyFilter();
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

        private bool _isOwnClassOnly;
        public bool IsOwnClassOnly
        {
            get => _isOwnClassOnly;
            set
            {
                _isOwnClassOnly = value;
                OnPropertyChanged(nameof(IsOwnClassOnly));
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

        private bool CanEditClass(object obj)
        {
            return SelectedClass != null &&
                   SelectedClass.FacultyId == _deptChair.FacultyId;
        }

        public bool CanEditOwnClass
        {
            get
            {
                return SelectedClass != null &&
                       SelectedClass.FacultyId == _deptChair.FacultyId;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
