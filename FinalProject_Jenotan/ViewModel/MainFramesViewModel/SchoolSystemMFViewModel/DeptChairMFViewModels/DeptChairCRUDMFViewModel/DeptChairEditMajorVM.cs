using FinalProject_Jenotan.Commands;
using FinalProject_Jenotan.Database;
using FinalProject_Jenotan.Database.Models.EnrollmentSystem;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;

namespace FinalProject_Jenotan.ViewModel.MainFramesViewModel.SchoolSystemMFViewModel.DeptChairMFViewModels.DeptChairCRUDMFViewModel
{
    public class DeptChairEditMajorVM : INotifyPropertyChanged
    {
        private readonly Faculty _faculty;
        private readonly Action<bool?> _closeWindow;
        private readonly TinyCollegeDbContextSQLServer _context;

        private Major _selectedMajor;

        public ObservableCollection<Student> Students { get; set; }

        private Student _selectedStudent;
        public Student SelectedStudent
        {
            get => _selectedStudent;
            set
            {
                _selectedStudent = value;
                OnPropertyChanged(nameof(SelectedStudent));
            }
        }


        public ICommand RemoveStudentCommand { get; }

        public DeptChairEditMajorVM(Faculty faculty, Action<bool?> closeWindow, Major selectedMajor)
        {
            _faculty = faculty;
            _closeWindow = closeWindow;
            _selectedMajor = selectedMajor;

            _context = new TinyCollegeDbContextSQLServer();

            RemoveStudentCommand = new RelayCommand(RemoveStudent);

            LoadStudents();
        }

        private void LoadStudents()
        {
            var students = _context.Students
                .Where(s => s.MajorId == _selectedMajor.MajorId)
                .ToList();

            Students = new ObservableCollection<Student>(students);

            OnPropertyChanged(nameof(Students));
        }

        private void RemoveStudent(object obj)
        {
            if (SelectedStudent == null)
                return;

            var student = _context.Students
                .FirstOrDefault(s => s.StudentId == SelectedStudent.StudentId);

            if (student != null)
            {
                var major = _context.Majors
                    .FirstOrDefault(m => m.MajorId == student.MajorId);

                if (major != null)
                {
                    if (major.StudentCount > 0)
                        major.StudentCount--;
                }
                student.MajorId = null;
                student.MajorLink = null;

                _context.SaveChanges();
            }

            LoadStudents();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
