using FinalProject_Jenotan.Commands;
using FinalProject_Jenotan.Database.Models.EnrollmentSystem;
using FinalProject_Jenotan.UI;
using FinalProject_Jenotan.UI.SchoolSystem.MainFrames.DeptChairMainFrames;
using FinalProject_Jenotan.UI.SchoolSystem.MainFrames.StudentMainFrames;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Serialization;

namespace FinalProject_Jenotan.ViewModel.SchoolSystem
{
    public class StudentViewModel : INotifyPropertyChanged
    {
        private readonly Frame _frame;
        private Student _student;
        public Student Student
        {
            get => _student;
            set
            {
                _student = value;
                OnPropertyChanged();
            }
        }
        public ICommand LogoutCommand { get; }
        public ICommand EnrollmentsCommand { get; }
        public ICommand ClassesCommand { get; }

        public StudentViewModel(Frame frame, Student student)
        {
            _frame = frame;
            _student = student;
            EnrollmentsCommand = new RelayCommand(OpenEnrollments);
            LogoutCommand = new RelayCommand(ExecuteLogout);
            ClassesCommand = new RelayCommand(OpenClasses);
        }

        private void OpenEnrollments(object obj)
        {
            _frame.Navigate(new StudentEnrollmentsPage(Student));
        }

        private void OpenClasses(object obj)
        {
            _frame.Navigate(new StudentClassesPage(Student));
        }

        private void ExecuteLogout(object obj)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;

            if (mainWindow != null)
            {
                mainWindow.MainFrame.Navigate(new TitlePage());
            }
        }

        public string FullName =>
         $"{Student?.FName?[0]}.{Student?.LName}";

        public string MajorName =>
             Student?.MajorLink?.MajorName ?? "No Major";
        public string FacultyName =>
             Student?.FacultyLink != null
        ? $"{Student.FacultyLink.FName[0]}.{Student.FacultyLink.LName}"
        : "No Adviser";

        public string DepartmentName =>
        Student?.MajorLink?.DepartmentLink?.DepartmentName ?? "No Department";
        public string SchoolName =>
          Student?.MajorLink?.DepartmentLink?.SchoolLink?.SchoolName.ToString() ?? "No School";

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
