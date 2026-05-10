using FinalProject_Jenotan.Database;
using FinalProject_Jenotan.Database.Models.EnrollmentSystem;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Media;

namespace FinalProject_Jenotan.ViewModel.MainFramesViewModel.SchoolSystemMFViewModel.StudentMFViewModels
{
    public class StudentClassesMFViewModel : INotifyPropertyChanged
    {
        private Student _student;
        public ObservableCollection<Enrollment> Enrollments { get; set; }
        private List<Enrollment> _allEnrollments;
        public StudentClassesMFViewModel(Student student) 
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
            ApplyFilters();
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

        private void LoadEnrollments()
        {
            using var context = new TinyCollegeDbContextSQLServer();

            var enrollments = context.Enrollments
                .Where(e => e.StudentId == _student.StudentId)
                .ToList();

            _allEnrollments = enrollments;

            Enrollments = new ObservableCollection<Enrollment>(enrollments);
            OnPropertyChanged(nameof(Enrollments));
        }

        private void ApplyFilters()
        {
            if (_allEnrollments == null)
                return;

            var filtered = _allEnrollments.AsEnumerable();

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

            if (ShowCompletedOnly)
            {
                
                filtered = filtered.Where(e => e.IsActive == false);
            }
            else
            {
                
                filtered = filtered.Where(e => e.IsActive == true);
            }

            Enrollments = new ObservableCollection<Enrollment>(filtered);
            OnPropertyChanged(nameof(Enrollments));
        }

        private bool _showCompletedOnly;
        public bool ShowCompletedOnly
        {
            get => _showCompletedOnly;
            set
            {
                _showCompletedOnly = value;
                OnPropertyChanged(nameof(ShowCompletedOnly));
                ApplyFilters();
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

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    }
}
