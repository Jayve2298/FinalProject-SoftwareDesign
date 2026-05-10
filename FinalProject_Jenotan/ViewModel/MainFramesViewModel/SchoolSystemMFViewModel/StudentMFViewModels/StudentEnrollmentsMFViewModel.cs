using FinalProject_Jenotan.Commands;
using FinalProject_Jenotan.Database;
using FinalProject_Jenotan.Database.Models.EnrollmentSystem;
using FinalProject_Jenotan.PromptWindow;
using FinalProject_Jenotan.UI.EditWindow.StudentEditWindows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace FinalProject_Jenotan.ViewModel.MainFramesViewModel.SchoolSystemMFViewModel.StudentMFViewModels
{
    public class StudentEnrollmentsMFViewModel : INotifyPropertyChanged
    {
        private readonly Student _student;

        public ObservableCollection<Enrollment> AllEnrollments { get; set; }
        public ObservableCollection<Enrollment> Enrollments { get; set; }
        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }

        private Enrollment _selectedEnrollment;
        public Enrollment SelectedEnrollment
        {
            get => _selectedEnrollment;
            set
            {
                _selectedEnrollment = value;
                OnPropertyChanged(nameof(SelectedEnrollment));
            }
        }

        public StudentEnrollmentsMFViewModel(Student student)
        {
            _student = student;

            Filters = new ObservableCollection<string>
            {
                "All",
                "First",
                "Second"
            };
            SelectedFilter = "All";
            LoadEnrollments();
            AddCommand = new RelayCommand(OpenAddWindow);
            DeleteCommand = new RelayCommand(DeleteEnrollment);
        }

        private void LoadEnrollments()
        {
            using (var context = new TinyCollegeDbContextSQLServer())
            {
                var enrollments = context.Enrollments
                    .Where(e => e.StudentId == _student.StudentId && e.IsActive == true)
                    .ToList();

                AllEnrollments = new ObservableCollection<Enrollment>(enrollments);
                Enrollments = new ObservableCollection<Enrollment>(enrollments);

                OnPropertyChanged(nameof(AllEnrollments));
                OnPropertyChanged(nameof(Enrollments));
            }
        }

        private async void DeleteEnrollment(object obj)
        {
            if (SelectedEnrollment == null)
                return;

            var confirmWindow = new ConfirmationWindow
            {
                Owner = Application.Current.MainWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            confirmWindow.ShowDialog();

           
            if (!confirmWindow.result)
                return;

            using (var context = new TinyCollegeDbContextSQLServer())
            {

                var enrollment = context.Enrollments
                    .FirstOrDefault(e => e.EnrollmentId == SelectedEnrollment.EnrollmentId);

                if (enrollment == null)
                    return;

                var classEntity = context.Classes
                    .FirstOrDefault(c => c.ClassId == enrollment.ClassId);

                if (classEntity != null && classEntity.ClassCount > 0)
                {
                    classEntity.ClassCount -= 1;
                }


                context.Enrollments.Remove(enrollment);

                await context.SaveChangesAsync();
            }

            StatusMessage = "Enrollment Deleted";
            StatusMessageBrush = Brushes.Red;

            LoadEnrollments();

            await Task.Delay(3000);
            StatusMessage = string.Empty;

        }

        private void OpenAddWindow(object obj)
        {

            using (var context = new TinyCollegeDbContextSQLServer())
            {
                int enrollmentCount = context.Enrollments
                    .Count(e => e.StudentId == _student.StudentId);

                if (enrollmentCount >= 6)
                {
                    StatusMessage = "Max Enrollment";
                    StatusMessageBrush = Brushes.Red;
                    return;
                }
            }

            var window = new StudentAddEnrollmentWindow(_student);

            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            bool? result = window.ShowDialog();

            if (result == true) 
            {
                StatusMessage = "Added Enrollment";
                StatusMessageBrush = Brushes.Green;

                LoadEnrollments();
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

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(SearchText);
                ApplyFilters();
            }
        }

        private void ApplyFilters()
        {
            if (AllEnrollments == null) return;

            var filtered = AllEnrollments.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                filtered = filtered.Where(e =>
                    e.EnrollmentId != null &&
                    e.EnrollmentId.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrWhiteSpace(SelectedFilter) && SelectedFilter != "All")
            {
                filtered = filtered.Where(e => e.Semester == SelectedFilter);
            }

            Enrollments = new ObservableCollection<Enrollment>(filtered);
            OnPropertyChanged(nameof(Enrollments));
        }

        public ObservableCollection<string> Filters { get; set; }

        private string _selectedFilter;
        public string SelectedFilter
        {
            get => _selectedFilter;
            set
            {
                _selectedFilter = value;
                OnPropertyChanged(SelectedFilter);
                ApplyFilters();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
