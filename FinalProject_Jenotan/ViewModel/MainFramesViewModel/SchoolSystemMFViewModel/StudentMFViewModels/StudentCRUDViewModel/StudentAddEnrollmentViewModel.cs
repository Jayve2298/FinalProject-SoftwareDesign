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

namespace FinalProject_Jenotan.ViewModel.MainFramesViewModel.SchoolSystemMFViewModel.StudentMFViewModels.StudentCRUDViewModel
{
    public class StudentAddEnrollmentViewModel : INotifyPropertyChanged
    {
        private readonly TinyCollegeDbContextSQLServer _context;
        private Student _student;
        public ICommand AddCommand { get;}
        public ObservableCollection<Class> Classes { get; set; }
        public Action CloseAction { get; set; }

        public StudentAddEnrollmentViewModel(Student student)
        {
            _context = new TinyCollegeDbContextSQLServer();
            _student = student;
            EnrollmentId = GenerateEnrollmentId();

            Semesters = new ObservableCollection<string> { "First", "Second" };
            SelectedSemester = Semesters.First();
            AddCommand = new RelayCommand(AddEnrollment);
            LoadClasses();
        }

        public async void AddEnrollment(object obj)
        {
            if (SelectedClass == null)
                return;
            var existingEnrollments = _context.Enrollments
                .Include(e => e.ClassLink)
                .Where(e => e.StudentId == _student.StudentId)
                .ToList();

            if (existingEnrollments.Any(e => e.ClassId == SelectedClass.ClassId))
            {
                StatusMessage = "Already Enrolled";
                StatusMessageBrush = Brushes.Red;
                return;
            }

            bool hasConflict = existingEnrollments.Any(e =>
                e.ClassLink.DayOfWeek == SelectedClass.DayOfWeek &&
                (
                    SelectedClass.ClassTime < e.ClassLink.ClassTime.AddMinutes(e.ClassLink.DurationInMinutes) &&
                    SelectedClass.ClassTime.AddMinutes(SelectedClass.DurationInMinutes) > e.ClassLink.ClassTime
                )
            );

            if (hasConflict)
            {
                StatusMessage = "Time Conflict";
                StatusMessageBrush = Brushes.Red;
                return;
            }
            var classEntity = _context.Classes
                .FirstOrDefault(c => c.ClassId == SelectedClass.ClassId);

            if (classEntity == null)
                return;

            // ✅ Create Enrollment
            var enrollment = new Enrollment
            {
                EnrollmentId = EnrollmentId,
                StudentId = _student.StudentId,
                ClassId = SelectedClass.ClassId,

                Semester = SelectedSemester,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(3),

                IsActive = true,
                Grade = null
            };

            // ✅ Increase ClassCount
            classEntity.ClassCount += 1;

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            // Optional: keep local student list updated
            _student.EnrollmentList ??= new List<Enrollment>();
            _student.EnrollmentList.Add(enrollment);

            CloseAction?.Invoke();
        }

        private Class _selectedClass;
        public Class SelectedClass
        {
            get => _selectedClass;
            set
            {
                _selectedClass = value;
                OnPropertyChanged();

                if (_selectedClass != null)
                {
                    FacultyId = _selectedClass.FacultyId;
                    Time = _selectedClass.TimeRange;
                    Day = _selectedClass.DayOfWeek.ToString();
                }
            }
        }

        private string GenerateEnrollmentId()
        {
            var lastNumber = _context.Enrollments
                .AsEnumerable()
                .Where(e => e.EnrollmentId.StartsWith("ENR - "))
                .Select(e =>
                {
                    var numberPart = e.EnrollmentId.Replace("ENR - ", "");

                    if (int.TryParse(numberPart, out int num))
                        return num;

                    return 999;
                })
                .DefaultIfEmpty(999)
                .Max();

            int nextNumber = lastNumber + 1;

            return $"ENR - {nextNumber}";
        }

        private string _facultyId;
        public string FacultyId
        {
            get => _facultyId;
            set
            {
                _facultyId = value;
                OnPropertyChanged();
            }
        }

        private string _time;
        public string Time
        {
            get => _time;
            set
            {
                _time = value;
                OnPropertyChanged();
            }
        }

        private string _day;
        public string Day
        {
            get => _day;
            set
            {
                _day = value;
                OnPropertyChanged();
            }
        }

        

        private void LoadClasses()
        {
            if (_student.MajorId == null) return;

            // Get student's department
            var departmentId = _context.Majors
                .Where(m => m.MajorId == _student.MajorId)
                .Select(m => m.DepartmentId)
                .FirstOrDefault();

            // Filter classes based on department
            var classes = _context.Classes
                .Include(c => c.CourseLink)
                .Where(c => c.CourseLink.DepartmentId == departmentId && c.IsOnGoing == true)
                .ToList();

            Classes = new ObservableCollection<Class>(classes);
            OnPropertyChanged(nameof(Classes));
        }

        public ObservableCollection<string> Semesters { get; set; }

        private string _selectedSemester;
        public string SelectedSemester
        {
            get => _selectedSemester;
            set
            {
                _selectedSemester = value;
                OnPropertyChanged();
            }
        }

        private string _enrollmentId;
        public string EnrollmentId
        {
            get => _enrollmentId;
            set
            {
                _enrollmentId = value;
                OnPropertyChanged();
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
