using FinalProject_Jenotan.Commands;
using FinalProject_Jenotan.Database;
using FinalProject_Jenotan.Database.Models.EnrollmentSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace FinalProject_Jenotan.ViewModel.MainFramesViewModel.SchoolSystemMFViewModel.DeptChairMFViewModels.DeptChairCRUDMFViewModel
{
    public class DeptChairAddCoursesVM : INotifyPropertyChanged
    {
        public string DepartmentId { get; set; }
        public ICommand ConfirmCommand { get; }

        private readonly Action<bool?> _closeWindow;
        public DeptChairAddCoursesVM(Faculty faculty, Action<bool?> closeWindow)
        {
            DepartmentId = faculty.DepartmentId;
            ConfirmCommand = new RelayCommand(AddCourse);
            CourseId = "CRS - ";
            _closeWindow = closeWindow;

            GeneratePreviewCourseId();
        }

        private string _courseId;
        public string CourseId
        {
            get => _courseId;
            set
            {
                _courseId = value;
                OnPropertyChanged();
            }
        }

        private string _courseName;
        public string CourseName
        {
            get => _courseName;
            set { _courseName = value; OnPropertyChanged(); }
        }

        private void AddCourse(object obj)
        {
            
            if (string.IsNullOrWhiteSpace(CourseId) ||
                string.IsNullOrWhiteSpace(CourseName))
            {
                MessageBox.Show("Please fill all fields.");
                return;
            }

            using var context = new TinyCollegeDbContextSQLServer();
            GeneratePreviewCourseId();

            bool exists = context.Courses
                .Any(c => c.CourseId == CourseId);

            if (exists)
            {
                MessageBox.Show("Course ID already exists.");
                return;
            }

            var newCourse = new Course
            {
                CourseId = CourseId,
                CourseName = CourseName,
                DepartmentId = DepartmentId,
                IsActive = true
            };

            context.Courses.Add(newCourse);
            context.SaveChanges();

            _closeWindow?.Invoke(true);
        }

        private void GeneratePreviewCourseId()
        {
            using var context = new TinyCollegeDbContextSQLServer();

            var numbers = context.Courses
                .AsEnumerable()
                .Select(c =>
                {
                    var part = c.CourseId.Split('-').Last().Trim();
                    return int.TryParse(part, out int num) ? num : 0;
                });

            int nextNumber = numbers.Any() ? numbers.Max() + 1 : 1;

            CourseId = $"CRS - {nextNumber}";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
