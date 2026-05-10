using FinalProject_Jenotan.Commands;
using FinalProject_Jenotan.Database;
using FinalProject_Jenotan.Database.Models.EnrollmentSystem;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.RightsManagement;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace FinalProject_Jenotan.ViewModel.MainFramesViewModel.SchoolSystemMFViewModel.DeptChairMFViewModels.DeptChairCRUDMFViewModel
{
    public enum CourseStatus
    {
        Active,
        Inactive
    }

    public class DeptChairEditCoursesVM : INotifyPropertyChanged
    {
        private readonly Course _course;

        public string CourseName { get; set; }
        public ICommand ConfirmCommand { get; }
        private readonly Action _closeWindow;
        public DeptChairEditCoursesVM(Course course, Action closeWindow)
        {
            _course = course;
            _closeWindow = closeWindow;
            CourseName = course.CourseName;
            StatusOptions = new ObservableCollection<CourseStatus>
            {
            CourseStatus.Active,
            CourseStatus.Inactive
            };

            SelectedStatus = course.IsActive
                ? CourseStatus.Active
                : CourseStatus.Inactive;

            ConfirmCommand = new RelayCommand(Confirm);
        }

        public ObservableCollection<CourseStatus> StatusOptions { get; set; }

        private CourseStatus _selectedStatus;
        public CourseStatus SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                _selectedStatus = value;
                OnPropertyChanged();
            }
        }

        private void Confirm(object obj)
        {
            using var context = new TinyCollegeDbContextSQLServer();

            var courseInDb = context.Courses
                .FirstOrDefault(c => c.CourseId == _course.CourseId);

            if (courseInDb == null)
                return;

            courseInDb.IsActive = SelectedStatus == CourseStatus.Active;

            context.SaveChanges();

            Application.Current.Windows
                .OfType<Window>()
                .SingleOrDefault(w => w.IsActive).DialogResult = true;

            _closeWindow();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
