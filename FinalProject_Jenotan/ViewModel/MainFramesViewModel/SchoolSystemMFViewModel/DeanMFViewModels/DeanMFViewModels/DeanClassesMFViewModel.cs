using FinalProject_Jenotan.Commands;
using FinalProject_Jenotan.Database;
using FinalProject_Jenotan.Database.Models.EnrollmentSystem;
using FinalProject_Jenotan.UI.EditWindow.DeptChairEditWindows;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace FinalProject_Jenotan.ViewModel.MainFramesViewModel.SchoolSystemMFViewModel.DeanMFViewModels.DeanMFViewModels
{
    public class DeanClassesMFViewModel : INotifyPropertyChanged
    {
        private readonly Faculty _dean;

        public ObservableCollection<Class> Classes { get; set; } = new();
        public ICommand EditCommand { get; }
        public DeanClassesMFViewModel(Faculty dean)
        {
            _dean = dean;
            EditCommand = new RelayCommand(OpenEdit);

            Filters = new ObservableCollection<ClassFilter>
            {
                ClassFilter.All,
                ClassFilter.Active,
                ClassFilter.Inactive
            };

            SelectedFilter = ClassFilter.Active;
            LoadClasses();
        }

        private void OpenEdit(object obj)
        {
            if (SelectedClass == null) return;

            var window = new DeptChairEditClassesWindow(_dean, SelectedClass);

            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            var result = window.ShowDialog();

            if (result == true)
            {
                LoadClasses();

                StatusMessage = "Success";
                StatusMessageBrush = Brushes.Green;
            }
        }

        private void LoadClasses()
        {
            using var context = new TinyCollegeDbContextSQLServer();

            _allClasses = context.Classes
                .Include(c => c.CourseLink)
                .Where(c => c.FacultyId == _dean.FacultyId)
                .ToList();

            ApplyFilter();
        }

        private List<Class> _allClasses = new();

        private Class _selectedClass;
        public Class SelectedClass
        {
            get => _selectedClass;
            set
            {
                _selectedClass = value;
                OnPropertyChanged(nameof(SelectedClass));
            }
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                ApplyFilter();
            }
        }

        private void ApplyFilter()
        {
            IEnumerable<Class> filtered = _allClasses;

            // 🔹 Filter by status
            filtered = SelectedFilter switch
            {
                ClassFilter.Active => filtered.Where(c => c.IsOnGoing),
                ClassFilter.Inactive => filtered.Where(c => !c.IsOnGoing),
                _ => filtered
            };

            // 🔹 Filter by search
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var search = SearchText.ToLower();

                filtered = filtered.Where(c =>
                    !string.IsNullOrEmpty(c.ClassId) &&
                    c.ClassId.ToLower().Contains(search)
                );
            }

            Classes.Clear();
            foreach (var item in filtered)
                Classes.Add(item);
        }

        private string _statusMessage;
        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged(nameof(StatusMessage));
            }
        }

        private Brush _statusMessageBrush = Brushes.Black;
        public Brush StatusMessageBrush
        {
            get => _statusMessageBrush;
            set
            {
                _statusMessageBrush = value;
                OnPropertyChanged(nameof(StatusMessageBrush));
            }
        }

        public ObservableCollection<ClassFilter> Filters { get; set; }

        private ClassFilter _selectedFilter;
        public ClassFilter SelectedFilter
        {
            get => _selectedFilter;
            set
            {
                _selectedFilter = value;
                OnPropertyChanged(nameof(SelectedFilter));
                ApplyFilter();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
    public enum ClassFilter
    {
        All,
        Active,
        Inactive
    }
}
