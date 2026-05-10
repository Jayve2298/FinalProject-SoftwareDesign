using FinalProject_Jenotan.Commands;
using FinalProject_Jenotan.Database.Models.MotorPoolSystem.Maintenance;
using FinalProject_Jenotan.UI;
using FinalProject_Jenotan.UI.Motorpool.MainFrames.MechanicMainFrames;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FinalProject_Jenotan.ViewModel.Motorpool
{
    public class MechanicViewModel
    {
        private readonly Frame _frame;
        private Mechanic _mechanic;
        public ICommand InventoryCommand { get; }
        public ICommand PartsUsedCommand { get; }
        public ICommand MaintenanceCommand { get; }
        public ICommand LogoutCommand { get; }

        public MechanicViewModel(Frame frame, Mechanic mechanic)
        {
            _mechanic = mechanic;
            _frame = frame;
            InventoryCommand = new RelayCommand(OpenInventory);
            MaintenanceCommand = new RelayCommand(OpenMaintenance);
            PartsUsedCommand = new RelayCommand(OpenPartsUsed);
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
