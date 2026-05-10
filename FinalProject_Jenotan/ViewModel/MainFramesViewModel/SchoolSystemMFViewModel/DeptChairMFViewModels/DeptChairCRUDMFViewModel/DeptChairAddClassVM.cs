using FinalProject_Jenotan.Commands;
using FinalProject_Jenotan.Database;
using FinalProject_Jenotan.Database.Models.EnrollmentSystem;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace FinalProject_Jenotan.ViewModel.MainFramesViewModel.SchoolSystemMFViewModel.DeptChairMFViewModels.DeptChairCRUDMFViewModel
{
    public class DeptChairAddClassVM : INotifyPropertyChanged
    {
        private readonly Faculty _deptChair;
        public ICommand ConfirmDetailsCommand { get; }
        public ICommand AddCommand { get; }
        public ObservableCollection<Course> Courses { get; set; } = new();
        public ObservableCollection<Room> Rooms { get; set; } = new();
        public ObservableCollection<Faculty> Faculties { get; set; } = new();
        private readonly Action<bool?> _closeWindow;

        private List<Room> _allRooms = new();
        private List<Faculty> _allFaculties = new();
        private List<Class> _allClasses = new();

        public DeptChairAddClassVM(Faculty deptChair, Action<bool?> closeWindow)
        {
            _deptChair = deptChair;
            ConfirmDetailsCommand = new RelayCommand(ConfirmDetails);
            AddCommand = new RelayCommand(AddClass);
            LoadCourses();
            LoadBaseData();
            _closeWindow = closeWindow;
            GeneratePreviewClassId();
        }

        private void AddClass(object obj)
        {
            ErrorMessage = string.Empty;

            if (SelectedCourse == null ||
                SelectedRoom == null ||
                SelectedFaculty == null ||
                SelectedDay == null ||
                SelectedDuration == null ||
                ClassTime == null)
            {
                ShowError("Complete all fields");
                return;
            }

            using var context = new TinyCollegeDbContextSQLServer();

            string newClassId = GenerateClassId(context);

            var newClass = new Class
            {
                ClassId = ClassId,
                CourseId = SelectedCourse.CourseId,
                FacultyId = SelectedFaculty.FacultyId,
                RoomCode = SelectedRoom.RoomCode,
                DayOfWeek = SelectedDay.Value,
                ClassTime = TimeOnly.FromDateTime(ClassTime.Value),
                DurationInMinutes = SelectedDuration.Minutes,
                IsOnGoing = true
            };

            var course = context.Courses
             .FirstOrDefault(c => c.CourseId == SelectedCourse.CourseId);

            if (course != null)
            {
                course.ClassCount += 1;
            }

            var faculty = context.Faculties
             .FirstOrDefault(f => f.FacultyId == SelectedFaculty.FacultyId);

            if (faculty != null)
            {
                faculty.ClassCount += 1;
            }

            context.Classes.Add(newClass);
            context.SaveChanges();

            _closeWindow?.Invoke(true);
        }

        private string GenerateClassId(TinyCollegeDbContextSQLServer context)
        {
            var numbers = context.Classes
                .AsEnumerable()
                .Select(c =>
                {
                    var part = c.ClassId.Split('-').Last().Trim();
                    return int.TryParse(part, out int num) ? num : 0;
                });

            int nextNumber = numbers.Any() ? numbers.Max() + 1 : 1;

            return $"CLASS - {nextNumber}";
        }

        private async void ConfirmDetails(object obj)
        {
            ErrorMessage = string.Empty;

            if (SelectedDay == null ||
                SelectedDuration == null ||
                ClassTime == null)
            {
                ShowError("Fields Missing");
                return;
            }

            var duration = SelectedDuration.Minutes;
            var start = TimeOnly.FromDateTime(ClassTime.Value);

            IsConfirmed = true;
            RefreshAvailability();
        }

        public Course SelectedCourse { get; set; }
        public Room SelectedRoom { get; set; }
        public Faculty SelectedFaculty { get; set; }

        private DayOfWeek? _selectedDay;
        public DayOfWeek? SelectedDay
        {
            get => _selectedDay;
            set
            {
                _selectedDay = value;
                OnPropertyChanged(nameof(SelectedDay));
                
            }
        }

        public DurationOption SelectedDuration { get; set; }

        private DateTime? _classTime;
        public DateTime? ClassTime
        {
            get => _classTime;
            set
            {
                _classTime = value;
                OnPropertyChanged(nameof(ClassTime));
            }
        }
        private void LoadCourses()
        {
            using var context = new TinyCollegeDbContextSQLServer();

            var deptId = _deptChair.DepartmentId;

            var courses = context.Courses
                .Where(c => c.DepartmentId == deptId)
                .ToList();

            Courses = new ObservableCollection<Course>(courses);
            OnPropertyChanged(nameof(Courses));
        }

        private void LoadBaseData()
        {
            using var context = new TinyCollegeDbContextSQLServer();

            var deptId = _deptChair.DepartmentId;

            _allRooms = context.Rooms.ToList();

            _allFaculties = context.Faculties
                .Where(f => f.DepartmentId == deptId &&
                       (f.Role == Role.Professor ||
                        f.Role == Role.Dean ||
                        f.Role == Role.DepartmentChair) && f.ClassCount < 6)
                .ToList();

            Rooms = new ObservableCollection<Room>(); 
            Faculties = new ObservableCollection<Faculty>();

            OnPropertyChanged(nameof(Rooms));
            OnPropertyChanged(nameof(Faculties));
        }

        private void RefreshAvailability()
        {
            if (!IsConfirmed)
                return;

            using var context = new TinyCollegeDbContextSQLServer();

            var start = TimeOnly.FromDateTime(ClassTime.Value);
            var end = start.AddMinutes(SelectedDuration.Minutes);

            var classes = context.Classes.ToList();

            var availableRooms = _allRooms.Where(r =>
                !classes.Any(c =>
                    c.RoomCode == r.RoomCode &&
                    c.DayOfWeek == SelectedDay &&
                    Overlap(start, end,
                        c.ClassTime,
                        c.ClassTime.AddMinutes(c.DurationInMinutes))
                )
            ).ToList();

            Rooms = new ObservableCollection<Room>(availableRooms);
            OnPropertyChanged(nameof(Rooms));

            var availableFaculty = _allFaculties.Where(f =>
                !classes.Any(c =>
                    c.FacultyId == f.FacultyId &&
                    c.DayOfWeek == SelectedDay &&
                    Overlap(start, end,
                        c.ClassTime,
                        c.ClassTime.AddMinutes(c.DurationInMinutes))
                )
            ).ToList();

            Faculties = new ObservableCollection<Faculty>(availableFaculty);
            OnPropertyChanged(nameof(Faculties));
        }

        private bool Overlap(TimeOnly start1, TimeOnly end1, TimeOnly start2, TimeOnly end2)
        {
            return start1 < end2 && start2 < end1;
        }

        public class DurationOption
        {
            public string Label { get; set; }
            public int Minutes { get; set; }
        }
       
        public List<DurationOption> Durations { get; set; } = new()
        {
            new DurationOption { Label = "1 hr 30 mins", Minutes = 90 },
            new DurationOption { Label = "3 hrs", Minutes = 180 }
        };

        private bool _isConfirmed;
        public bool IsConfirmed
        {
            get => _isConfirmed;
            set
            {
                _isConfirmed = value;
                OnPropertyChanged(nameof(IsConfirmed));
            }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }



        private async void ShowError(string message)
        {
            ErrorMessage = message;

            await Task.Delay(3000);

            ErrorMessage = string.Empty;
        }


        private string _classId;
        public string ClassId
        {
            get => _classId;
            set
            {
                _classId = value;
                OnPropertyChanged(nameof(ClassId));
            }
        }

        private void GeneratePreviewClassId()
        {
            using var context = new TinyCollegeDbContextSQLServer();

            var numbers = context.Classes
                .AsEnumerable()
                .Select(c =>
                {
                    var part = c.ClassId.Split('-').Last().Trim();
                    return int.TryParse(part, out int num) ? num : 0;
                });

            int nextNumber = numbers.Any() ? numbers.Max() + 1 : 1;

            ClassId = $"CLASS - {nextNumber}";
        }

        public List<DayOfWeek> Days { get; set; } =
            Enum.GetValues(typeof(DayOfWeek))
                .Cast<DayOfWeek>()
                .Where(d => d >= DayOfWeek.Monday && d <= DayOfWeek.Saturday)
                .ToList();

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
