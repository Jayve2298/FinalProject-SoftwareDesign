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

namespace FinalProject_Jenotan.ViewModel.MainFramesViewModel.SchoolSystemMFViewModel.DeptChairMFViewModels.DeptChairCRUDMFViewModel
{
    public class DeptChairEditClassVM : INotifyPropertyChanged
    {
        private readonly Action<bool?> _closeDialog;
        private readonly Class _selectedClass;
        public RelayCommand ConfirmGradeCommand { get; }
        public RelayCommand CompleteCommand { get; }

        public DeptChairEditClassVM(Faculty deptChair, Class selectedClass, Action<bool?> closeDialog)
        {
            _closeDialog = closeDialog;
            _selectedClass = selectedClass;
            ClassId = selectedClass.ClassId;
            FacultyId = selectedClass.FacultyId;
            ConfirmGradeCommand = new RelayCommand(SaveGrade, CanSaveGrade);
            CompleteCommand = new RelayCommand(CompleteClass);
            LoadEnrollments();
        }
        private void CompleteClass(object obj)
        {
            using var context = new TinyCollegeDbContextSQLServer();

            var classEntity = context.Classes
                .Include(c => c.EnrollmentList)
                .FirstOrDefault(c => c.ClassId == _selectedClass.ClassId);

            if (classEntity == null) return;

            var enrollments = context.Enrollments
                .Where(e => e.ClassId == _selectedClass.ClassId)
                .ToList();

            if (enrollments.Any(e => e.Grade == null))
            {
                StatusMessage = "All Grades Not Filled";
                StatusBrush = Brushes.Red;
                return;
            }
            classEntity.IsOnGoing = false;

            foreach (var e in enrollments)
            {
                e.IsActive = false;
            }

            context.SaveChanges();

            StatusMessage = "Class Completed Successfully";
            StatusBrush = Brushes.Green;

            _closeDialog?.Invoke(true);
        }

        private bool CanSaveGrade(object obj)
        {
            return SelectedEnrollment != null &&
                   float.TryParse(SelectedStudentGrade, out float grade) &&
                   grade >= 0 && grade <= 101;
        }

        private void SaveGrade(object obj)
        {
            if (SelectedEnrollment == null) return;

            if (!float.TryParse(SelectedStudentGrade, out float grade))
                return;

            using var context = new TinyCollegeDbContextSQLServer();

            var enrollment = context.Enrollments
                .FirstOrDefault(e => e.EnrollmentId == SelectedEnrollment.EnrollmentId);

            if (enrollment != null)
            {
                enrollment.Grade = grade;

                context.SaveChanges();

                SelectedEnrollment.Grade = grade;
                OnPropertyChanged(nameof(SelectedEnrollment));
                LoadEnrollments();
            }

        }

        public ObservableCollection<Enrollment> Enrollments { get; set; } = new();

        private Enrollment _selectedEnrollment;
        public Enrollment SelectedEnrollment
        {
            get => _selectedEnrollment;
            set
            {
                _selectedEnrollment = value;
                OnPropertyChanged();

                if (value != null)
                {
                    SelectedStudentId = value.StudentId;
                    SelectedStudentGrade = value.Grade?.ToString();
                }

                (ConfirmGradeCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        private string _selectedStudentId;
        public string SelectedStudentId
        {
            get => _selectedStudentId;
            set { _selectedStudentId = value; OnPropertyChanged(); }
        }

        private string _selectedStudentGrade;
        public string SelectedStudentGrade
        {
            get => _selectedStudentGrade;
            set
            {
                _selectedStudentGrade = value;
                OnPropertyChanged();

                (ConfirmGradeCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        private string _classId;
        public string ClassId
        {
            get => _classId;
            set { _classId = value; OnPropertyChanged(); }
        }

        private string _facultyId;
        public string FacultyId
        {
            get => _facultyId;
            set { _facultyId = value; OnPropertyChanged(); }
        }

        private void LoadEnrollments()
        {
            using var context = new TinyCollegeDbContextSQLServer();

            var enrollments = context.Enrollments
                .Include(e => e.StudentLink)
                .Include(e => e.ClassLink)
                .Where(e => e.ClassId == _selectedClass.ClassId)
                .ToList();

            Enrollments = new ObservableCollection<Enrollment>(enrollments);
            OnPropertyChanged(nameof(Enrollments));
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

        private Brush _statusBrush = Brushes.Black;
        public Brush StatusBrush
        {
            get => _statusBrush;
            set
            {
                _statusBrush = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
