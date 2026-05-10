using FinalProject_Jenotan.Commands;
using FinalProject_Jenotan.Database;
using FinalProject_Jenotan.Database.Models.EnrollmentSystem;
using FinalProject_Jenotan.PromptWindow;
using FinalProject_Jenotan.UI.EditWindow.DeanEditWindows;
using FinalProject_Jenotan.ViewModel.MainFramesViewModel.SchoolSystemMFViewModel.DeanMFViewModels.DeanWindowsViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace FinalProject_Jenotan.ViewModel.MainFramesViewModel.SchoolSystemMFViewModel.DeanMFViewModels
{

    public enum DepartmentFilter
    {
        Active,
        Inactive,
        All
    }

    public class DeanDepartmentMFViewModel : INotifyPropertyChanged
    {
        public bool IsActive { get; set; } = true;
        public ObservableCollection<Department> Departments { get; set; }
        private List<Department> _allDepartments;
        private readonly Faculty _faculty;

        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand AddCommand { get; }

        public DeanDepartmentMFViewModel(Faculty dean)
        {
            using var context = new TinyCollegeDbContextSQLServer();

            
            var fullDean = context.Faculties
                .Include(f => f.DepartmentLink)
                    .ThenInclude(d => d.SchoolLink)
                        .ThenInclude(s => s.DepartmentList)
                            .ThenInclude(d => d.FacultyLink)
                .FirstOrDefault(f => f.FacultyId == dean.FacultyId);


            var schoolId = fullDean?.DepartmentLink?.SchoolId;

            var departments = context.Departments
                .Include(d => d.FacultyLink)
                .Where(d => d.SchoolId == schoolId)
                .ToList();

            _allDepartments = departments.ToList();
            Departments = new ObservableCollection<Department>(
             _allDepartments.Where(d => d.IsActive));

            DeleteCommand = new RelayCommand(DeleteDepartment);

            EditCommand = new RelayCommand(EditDepartment, CanEditDepartment);
            _faculty = dean;
            AddCommand = new RelayCommand(OpenAddDepartmentWindow);
        }

        private bool CanEditDepartment(object obj)
        {
            return SelectedDepartment != null;
        }

        private void OpenAddDepartmentWindow(object obj)
        {
            var window = new DeanAddDepartmentWindow(_faculty);

            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            bool? result = window.ShowDialog();

            if (result == true)
            {
                StatusMessage = "Department Added";
                StatusMessageBrush = Brushes.Green;

                var timer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromSeconds(3)
                };

                timer.Tick += (s, e) =>
                {
                    StatusMessage = string.Empty;
                    timer.Stop();
                };

                timer.Start();
            }

            ReloadDepartments();
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                SearchDepartments();
            }
        }

        private Department _selectedDepartment;
        public Department SelectedDepartment
        {
            get => _selectedDepartment;
            set
            {
                _selectedDepartment = value;
                OnPropertyChanged();

                (EditCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        private void EditDepartment(object obj)
        {
            var window = new DeanDepartmentEditWindow(_faculty);

            var vm = new DeanEditDepartmentVM(_faculty, window.CloseWindow);
            vm.SelectedDepartment = SelectedDepartment;

            window.DataContext = vm;

            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            bool? result = window.ShowDialog();

            if (result == true)
            {
                StatusMessage = "Changes Saved";
                StatusMessageBrush = Brushes.Green;

                var timer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromSeconds(3)
                };
                timer.Tick += (s, e) =>
                {
                    StatusMessage = string.Empty;
                    timer.Stop();
                };
                timer.Start();
            }

            ReloadDepartments();
        }

        public void SearchDepartments()
        {
            RefreshDepartments();
        }

        private async void DeleteDepartment(object obj)
        {
            if (SelectedDepartment == null)
                return;

            using var context = new TinyCollegeDbContextSQLServer();

            bool hasActiveFaculty = context.Faculties
             .Any(f => f.DepartmentId == SelectedDepartment.DepartmentId && f.IsActive);

            if (hasActiveFaculty)
            {
                StatusMessage = "Cannot Delete";
                StatusMessageBrush = Brushes.Red;
                return;
            }

            var confirmWindow = new ConfirmationWindow
            {
                Owner = Application.Current.MainWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            confirmWindow.ShowDialog();


            if (!confirmWindow.result)
                return;


            var dbDept = context.Departments
                .FirstOrDefault(d => d.DepartmentId == SelectedDepartment.DepartmentId);

            if (dbDept != null)
            {
                dbDept.IsActive = false;
                context.SaveChanges();
            }

            
            ReloadDepartments();

            StatusMessage = "Deleted";
            StatusMessageBrush = Brushes.Red;

            await Task.Delay(3000);
            StatusMessage = string.Empty;
        }

        private void ReloadDepartments()
        {
            using var context = new TinyCollegeDbContextSQLServer();

            var schoolId = _faculty.DepartmentLink?.SchoolId;

            var departments = context.Departments
                .Include(d => d.FacultyLink)
                .Where(d => d.SchoolId == schoolId)
                .ToList();

            _allDepartments = departments;

            RefreshDepartments(); 
        }


        private void RefreshDepartments()
        {
            IEnumerable<Department> list = _allDepartments;

            switch (SelectedFilter)
            {
                case DepartmentFilter.Active:
                    list = list.Where(d => d.IsActive);
                    break;

                case DepartmentFilter.Inactive:
                    list = list.Where(d => !d.IsActive);
                    break;

                case DepartmentFilter.All:
   
                    break;
            }


            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                list = list.Where(d =>
                    d.DepartmentName != null &&
                    d.DepartmentName.ToLower().Contains(SearchText.ToLower()));
            }

            Departments.Clear();

            foreach (var dept in list)
            {
                Departments.Add(dept);
            }
        }

        private DepartmentFilter _selectedFilter = DepartmentFilter.Active;

        public DepartmentFilter SelectedFilter
        {
            get => _selectedFilter;
            set
            {
                _selectedFilter = value;
                OnPropertyChanged();
                RefreshDepartments();
            }
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

        private Brush _statusMessageBrush = Brushes.Black;
        public Brush StatusMessageBrush
        {
            get => _statusMessageBrush;
            set
            {
                _statusMessageBrush = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
