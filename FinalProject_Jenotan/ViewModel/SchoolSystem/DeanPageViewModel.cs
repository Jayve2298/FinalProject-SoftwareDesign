using FinalProject_Jenotan.Commands;
using FinalProject_Jenotan.Database.Models.EnrollmentSystem;
using FinalProject_Jenotan.UI;
using FinalProject_Jenotan.UI.EditWindow.FormsWindow;
using FinalProject_Jenotan.UI.SchoolSystem.MainFrames;
using FinalProject_Jenotan.UI.SchoolSystem.MainFrames.DeanMainFrames;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FinalProject_Jenotan.ViewModel.SchoolSystem
{
    public class DeanPageViewModel : INotifyPropertyChanged
    {
        private readonly Frame _frame;

        public ICommand LogoutCommand { get; }
        public ICommand OpenDepartmentsCommand { get; }
        public ICommand OpenFacultiesCommand { get; }
        public ICommand ClassesCommand { get; }
        public ICommand FormsCommand { get; }
        public ICommand AdvisoryCommand { get; }
        private void OpenDepartments(object obj)
        {
            _frame.Navigate(new DeanDepartmentsFrames(Faculty));
        }
        private void OpenClasses(object obj)
        {
            _frame.Navigate(new DeanClassPage(Faculty));
        }

        private void OpenFaculties(object obj)
        {
            _frame.Navigate(new DeanFacultiesPage(Faculty));
        }
        private void OpenForms(object obj)
        {
            _frame.Navigate(new FormsPage(Faculty));

        }

        private void OpenAdvisory(object obj)
        {
            _frame.Navigate(new FacultyAdvisoryPage(Faculty));
        }

        private Faculty _faculty;
        public Faculty Faculty
        {
            get => _faculty;
            set
            {
                _faculty = value;
                OnPropertyChanged();
            }
        }

        public string FullName =>
           Faculty != null
              ? $"{Faculty.FName?[0]}. {Faculty.LName}"
                 : "";
        public string Role => Faculty?.Role.ToString();
        public string DepartmentName =>
        Faculty?.DepartmentLink != null
        ? Faculty.DepartmentLink.DepartmentName
        : "No Department Loaded";

        public string SchoolName =>
            Faculty?.DepartmentLink?.SchoolLink != null
                ? Faculty.DepartmentLink.SchoolLink.SchoolName.ToString()
                : "No School Loaded";

        public DeanPageViewModel(Frame frame, Faculty faculty)
        {
            _frame = frame;
            Faculty = faculty;

            LogoutCommand = new RelayCommand(ExecuteLogout);
            OpenDepartmentsCommand = new RelayCommand(OpenDepartments);
            OpenFacultiesCommand = new RelayCommand(OpenFaculties);
            ClassesCommand = new RelayCommand(OpenClasses);
            FormsCommand = new RelayCommand(OpenForms);
            AdvisoryCommand = new RelayCommand(OpenAdvisory);
        }

        private void ExecuteLogout(object obj)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;

            if (mainWindow != null)
            {
                mainWindow.MainFrame.Navigate(new TitlePage());
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
