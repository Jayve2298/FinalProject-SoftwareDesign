using FinalProject_Jenotan.Commands;
using FinalProject_Jenotan.Database.Models.MotorPoolSystem.Reservation;
using FinalProject_Jenotan.UI;
using FinalProject_Jenotan.UI.Motorpool.MainFrames.EmployeeMainFrames;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FinalProject_Jenotan.ViewModel.Motorpool
{
    public class EmployeeViewModel : INotifyPropertyChanged
    {
        private readonly Frame _frame;
        private Employee _employee;

        public ICommand LogoutCommand { get; }
        public ICommand ReservationCommand { get; }
        public ICommand CheckOutCommand { get; }
        public ICommand VehicleReportCommand { get; }
        public ICommand PartsReportCommand { get; }
        public string FullName => $"{_employee.FName[0]}.{_employee.LName}";
        public string Email => _employee.Email;
        public string ContactNumber => _employee.ContactNumber;

        public EmployeeViewModel(Frame frame, Employee employee)
        {
            _frame = frame;
            _employee = employee;
            LogoutCommand = new RelayCommand(ExecuteLogout);
            ReservationCommand = new RelayCommand(OpenReservation);
            CheckOutCommand = new RelayCommand(OpenCheckOut);
            VehicleReportCommand = new RelayCommand(OpenVehicleReport);
            PartsReportCommand = new RelayCommand(OpenPartsReport);
        }

        public void OpenReservation(object obj)
        {
            _frame.Navigate(new EmployeeReservationMainFrame(_employee));
        }
        public void OpenCheckOut(object obj)
        {
            _frame.Navigate(new EmployeeCheckoutsMainFrame(_employee));
        }
        public void OpenVehicleReport(object obj)
        {
            _frame.Navigate(new EmployeeVehicleReportMainFrame(_employee));
        }
        public void OpenPartsReport(object obj)
        {
            _frame.Navigate(new EmployeePartsReport(_employee));
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
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
