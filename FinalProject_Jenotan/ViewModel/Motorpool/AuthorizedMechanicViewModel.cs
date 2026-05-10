using FinalProject_Jenotan.Commands;
using FinalProject_Jenotan.Database.Models.MotorPoolSystem.Maintenance;
using FinalProject_Jenotan.UI;
using FinalProject_Jenotan.UI.Motorpool.MainFrames.AuthorizedMechanicMainFrames;
using FinalProject_Jenotan.UI.Motorpool.MainFrames.MechanicMainFrames;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FinalProject_Jenotan.ViewModel.Motorpool
{
    public class AuthorizedMechanicViewModel
    {
        private readonly Frame _frame;
        private Mechanic _mechanic;
        public ICommand LogoutCommand { get; }
        public ICommand InventoryCommand { get; }
        public ICommand PartsCommand { get; }
        public ICommand MaintenanceCommand { get; }
        public ICommand ReleaseCommand { get; }

        public AuthorizedMechanicViewModel(Frame frame, Mechanic mechanic)
        {
            _frame = frame;
            _mechanic = mechanic;
            InventoryCommand = new RelayCommand(OpenInventory);
            MaintenanceCommand = new RelayCommand(OpenMaintenance);
            PartsCommand = new RelayCommand(OpenPartsUsed);
            ReleaseCommand = new RelayCommand(OpenRelease);
            LogoutCommand = new RelayCommand(ExecuteLogout);
        }

        public string FullName => $"{_mechanic.FName[0]}.{_mechanic.LName}";
        public string Email => _mechanic.Email;

        private void OpenMaintenance(object obj)
        {
            _frame.Navigate(new MechanicMaintenancePage(_mechanic));
        }
        private void OpenInventory(object obj)
        {
            _frame.Navigate(new MechanicInventoryPage(_mechanic));
        }
        private void OpenPartsUsed(object obj)
        {
            _frame.Navigate(new MechanicPartsUsedPage(_mechanic));
        }

        private void OpenRelease(object obj)
        {
            _frame.Navigate(new AMechanicReleasePage(_mechanic));
        }

        private void ExecuteLogout(object obj)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;

            if (mainWindow != null)
            {
                mainWindow.MainFrame.Navigate(new TitlePage());
            }
        }
    }
}
