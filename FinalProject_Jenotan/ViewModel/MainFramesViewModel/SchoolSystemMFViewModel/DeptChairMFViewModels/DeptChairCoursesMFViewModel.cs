using FinalProject_Jenotan.Commands;
using FinalProject_Jenotan.Database;
using FinalProject_Jenotan.Database.Models.EnrollmentSystem;
using FinalProject_Jenotan.PromptWindow;
using FinalProject_Jenotan.UI.EditWindow.DeanEditWindows;
using FinalProject_Jenotan.UI.EditWindow.DeptChairEditWindows;
using Microsoft.EntityFrameworkCore;
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
    public enum CourseStatus
    {
        Active, Inactive
    }
    public class DeptChairCoursesMFViewModel : INotifyPropertyChanged
    {
        private readonly Faculty _faculty;

        public ObservableCollection<Course> Courses { get; set; } = new();
        private List<Course> _allCourses = new();

        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand AddCommand { get; }

        private Course _selectedCourse;
        public Course SelectedCourse
        {
            get => _selectedCourse;
            set
            {
                _selectedCourse = value;
                OnPropertyChanged();

               
                (DeleteCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (EditCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        public DeptChairCoursesMFViewModel(Faculty faculty)
        {
            _faculty = faculty;
            AddCommand = new RelayCommand(OpenAddCoursesWindow);
            DeleteCommand = new RelayCommand(DeleteCourse, CanDeleteCourse);
            EditCommand = new RelayCommand(OpenEditCourseWindow, CanEditCourse);
            Filters = new ObservableCollection<CourseStatus>
                {
                    CourseStatus.Active,
                    CourseStatus.Inactive
                };

            SelectedFilter = CourseStatus.Active; //default

            LoadCourses();
        }

        private bool CanEditCourse(object obj)
        {
            return SelectedCourse != null;
        }

        private void OpenEditCourseWindow(object obj)
        {
            if (SelectedCourse == null)
                return;

            var window = new DeptChairEditCoursesWindow(SelectedCourse);

            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            bool? result = window.ShowDialog();

            if (result == true)
            {
                StatusMessage = "Course Updated";
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
                ApplyFilters();
                LoadCourses();
            }
        }

        private bool CanDeleteCourse(object obj)
        {
            return SelectedCourse != null
                   && SelectedCourse.IsActive
                   && SelectedCourse.ClassCount == 0;
        }

        public void OpenAddCoursesWindow(object obj)
        {
            var window = new DeptChairAddCoursesWindow(_faculty);

            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            bool? result = window.ShowDialog();

            if (result == true)
            {
                StatusMessage = "Department Added";
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

            LoadCourses();
        }

        private void DeleteCourse(object obj)
        {
            if (SelectedCourse == null)
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

            var courseInDb = context.Courses
                .FirstOrDefault(c => c.CourseId == SelectedCourse.CourseId);

            if (courseInDb == null)
                return;

            //soft delete
            courseInDb.IsActive = false;
            context.SaveChanges();

            //update UI model
            SelectedCourse.IsActive = false;

            StatusMessage = "Course deactivated";
            StatusMessageBrush = Brushes.Red;

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

            
            LoadCourses();

            SelectedCourse = null;
            (DeleteCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }

        private void LoadCourses()
        {
            using var context = new TinyCollegeDbContextSQLServer();

            _allCourses = context.Courses
                .Include(c => c.DepartmentLink)
                .Where(c => c.DepartmentId == _faculty.DepartmentId)
                .AsNoTracking()
                .ToList();

            ApplyFilters();
        }

        public ObservableCollection<CourseStatus> Filters { get; set; }

        private CourseStatus _selectedFilter;
        public CourseStatus SelectedFilter
        {
            get => _selectedFilter;
            set
            {
                if (_selectedFilter == value)
                {
                    //refresh manually
                    ApplyFilters();
                    return;
                }

                _selectedFilter = value;
                OnPropertyChanged();
                ApplyFilters();
            }
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                ApplyFilters();
            }
        }


        private void ApplyFilters()
        {
            IEnumerable<Course> list = _allCourses;

            list = SelectedFilter switch
            {
                CourseStatus.Active => list.Where(c => c.IsActive),
                CourseStatus.Inactive => list.Where(c => !c.IsActive),
                _ => list
            };

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                list = list.Where(c =>
                    (!string.IsNullOrEmpty(c.CourseName) &&
                     c.CourseName.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(c.CourseId) &&
                     c.CourseId.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                );
            }

            Courses.Clear();

            foreach (var course in list)
            {
                Courses.Add(course);
            }
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
