using FinalProject_Jenotan.Commands;
using FinalProject_Jenotan.Database;
using FinalProject_Jenotan.Database.Models.EnrollmentSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace FinalProject_Jenotan.ViewModel.MainFramesViewModel.SchoolSystemMFViewModel.DeptChairMFViewModels.DeptChairCRUDMFViewModel
{
    public class DeptChairAddMajorVM : INotifyPropertyChanged
    {
        private readonly Action<bool?> _closeWindow;
        private readonly Faculty _faculty;
        private readonly TinyCollegeDbContextSQLServer _context;

        public event PropertyChangedEventHandler PropertyChanged;

        private string _majorId;
        public string MajorId
        {
            get => _majorId;
            set
            {
                _majorId = value;
                OnPropertyChanged(nameof(MajorId));
            }
        }

        private string _majorName;
        public string MajorName
        {
            get => _majorName;
            set
            {
                _majorName = value;
                OnPropertyChanged(nameof(MajorName));
            }
        }

        public ICommand ConfirmCommand => new RelayCommand(Confirm);

        public DeptChairAddMajorVM(Faculty faculty, Action<bool?> closeWindow)
        {
            _faculty = faculty;
            _closeWindow = closeWindow;
            _context = new TinyCollegeDbContextSQLServer();
            GenerateMajorId();
        }

        private void GenerateMajorId()
        {
            var majors = _context.Majors.ToList();

            int nextNumber = 1000;

            if (majors.Any())
            {
                var lastNumber = majors
                    .Select(m => int.Parse(m.MajorId.Replace("MAJ - ", "")))
                    .Max();

                nextNumber = lastNumber + 1;
            }

            MajorId = $"MAJ - {nextNumber}";
        }

        private void Confirm(object obj)
        {
            var newMajor = new Major
            {
                MajorId = MajorId,
                MajorName = MajorName,
                IsActive = true,
                StudentCount = 0,
                DepartmentId = _faculty.DepartmentId,
                StudentList = new List<Student>()
            };

            _context.Majors.Add(newMajor);
            _context.SaveChanges(); 

            _closeWindow(true);
        }

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
