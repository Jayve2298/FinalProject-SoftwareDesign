using FinalProject_Jenotan.Commands;
using FinalProject_Jenotan.Database.Models.EnrollmentSystem;
using FinalProject_Jenotan.UI;
using FinalProject_Jenotan.UI.EditWindow.FormsWindow;
using FinalProject_Jenotan.UI.SchoolSystem.MainFrames;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FinalProject_Jenotan.ViewModel.SchoolSystem
{
    public class ResearcherViewModel : INotifyPropertyChanged
    {
        private readonly Frame _frame;

        public ICommand LogoutCommand { get; }
        public ICommand FormsCommand { get; }
        public ICommand AdvisoryCommand { get; }
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
        ? $"{Faculty.FName?[0]}.{Faculty.LName}"
        : "";
        public string Role => Faculty?.Role.ToString();
        public string DepartmentName =>
        Faculty?.DepartmentLink != null
        ? Faculty.DepartmentLink.DepartmentName
        : "No Department Loaded";

        public ResearcherViewModel(Frame frame, Faculty faculty)
        {
            _frame = frame;
            Faculty = faculty;
            LogoutCommand = new RelayCommand(ExecuteLogout);
            FormsCommand = new RelayCommand(OpenForms);
            AdvisoryCommand = new RelayCommand(OpenAdvisory);
        }

        private void OpenAdvisory(object obj)
        {
            _frame.Navigate(new FacultyAdvisoryPage(Faculty));
        }

        private void OpenForms(object obj)
        {
            _frame.Navigate(new FormsPage(Faculty));

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
