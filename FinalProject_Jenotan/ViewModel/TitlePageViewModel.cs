using FinalProject_Jenotan.Commands;
using FinalProject_Jenotan.Database;
using FinalProject_Jenotan.Database.Models.EnrollmentSystem;
using FinalProject_Jenotan.Database.Models.MotorPoolSystem.Maintenance;
using FinalProject_Jenotan.Database.Models.MotorPoolSystem.Reservation;
using FinalProject_Jenotan.UI;
using FinalProject_Jenotan.UI.Motorpool;
using FinalProject_Jenotan.UI.SchoolSystem;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace FinalProject_Jenotan.ViewModel
{

    public class TitlePageViewModel : INotifyPropertyChanged
    {
        private string _username;
        private string _password;

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoginCommand { get; }

        private Page _page;

        public TitlePageViewModel(Page page)
        {
            _page = page;
            LoginCommand = new RelayCommand(ExecuteLogin);
        }

        private void TitleVideo_MediaEnded(object sender, RoutedEventArgs e)
        {
            var media = sender as MediaElement;
            media.Position = TimeSpan.Zero;
            media.Play();
        }

        private void ExecuteLogin(object obj)
        {
            using var context = new TinyCollegeDbContextSQLServer();

            string prefix = Username?.Length >= 3
             ? Username.Substring(0, 3)
             : "";

            if (prefix == "FAC")
            {
                var faculty = context.Faculties
                .Include(f => f.DepartmentLink)
                 .ThenInclude(d => d.SchoolLink)
                 .FirstOrDefault(f => f.FacultyId == Username && f.Password == Password);

                if (faculty != null)
                {
                    IsLoginInvalid = false;
                    NavigateFaculty(faculty);
                }
                else
                {
                    IsLoginInvalid = true;
                }
            }
            else if (prefix == "STU")
            {
                var student = context.Students
                    .Include(s => s.MajorLink)
                        .ThenInclude(m => m.DepartmentLink)
                        .ThenInclude(d => d.SchoolLink)
                    .Include(s => s.FacultyLink)
                    .FirstOrDefault(s => s.StudentId == Username && s.Password == Password);

                if (student != null)
                {
                    IsLoginInvalid = false;
                    NavigateStudent(student);
                }
                else
                {
                    IsLoginInvalid = true;
                }
            }
            else if (prefix == "MEC")
            {
                var mechanic = context.Mechanics
                     .FirstOrDefault(m => m.MechanicId == Username && m.Password == Password);

                if (mechanic != null)
                {
                    IsLoginInvalid = false;
                    NavigateMechanic(mechanic);
                }
                else
                {
                    IsLoginInvalid = true;
                }
            }
            else if (prefix == "EMP")
            {
                var employee = context.Employees
                     .FirstOrDefault(e => e.EmployeeId == Username && e.Password == Password);

                if (employee != null)
                {
                    IsLoginInvalid = false;
                    NavigateEmployee(employee);
                }
                else
                {
                    IsLoginInvalid = true;
                }
            }
            else
            {
                IsLoginInvalid = true;
            }
        }

        private void NavigateStudent(Student student)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.MainFrame.Navigate(new StudentPage(student));
        }
        private void NavigateMechanic(Mechanic mechanic)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;

            if (mechanic.IsAuthorizedToSign == true)
            {
                mainWindow.MainFrame.Navigate(new AuthorizedMechanicPage(mechanic));
            }
            else
            {
                mainWindow.MainFrame.Navigate(new MechanicPage(mechanic));
            }
        }

        private void NavigateEmployee(Employee employee)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.MainFrame.Navigate(new EmployeePage(employee));
        }

        private void NavigateFaculty(Faculty faculty)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;

            switch (faculty.Role)
            {
                case Role.Dean:
                    mainWindow.MainFrame.Navigate(new DeanPage(faculty));
                    break;

                case Role.DepartmentChair:
                    mainWindow.MainFrame.Navigate(new DeptChairPage(faculty));
                    break;

                case Role.Professor:
                    mainWindow.MainFrame.Navigate(new ProfessorPage(faculty));
                    break;

                case Role.Researcher:
                    mainWindow.MainFrame.Navigate(new ResearcherPage(faculty));
                    break;
            }
        }

        private bool _isLoginInvalid;
        private System.Timers.Timer _loginErrorTimer;

        public bool IsLoginInvalid
        {
            get => _isLoginInvalid;
            set
            {
                _isLoginInvalid = value;
                OnPropertyChanged();

                // Only start timer when error becomes true
                if (value)
                {
                    StartErrorTimer();
                }
            }
        }

        private void StartErrorTimer()
        {
            // Prevent multiple timers stacking
            _loginErrorTimer?.Stop();
            _loginErrorTimer?.Dispose();

            _loginErrorTimer = new System.Timers.Timer(3000);
            _loginErrorTimer.AutoReset = false;

            _loginErrorTimer.Elapsed += (s, e) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    IsLoginInvalid = false;
                });

                _loginErrorTimer.Dispose();
            };

            _loginErrorTimer.Start();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
