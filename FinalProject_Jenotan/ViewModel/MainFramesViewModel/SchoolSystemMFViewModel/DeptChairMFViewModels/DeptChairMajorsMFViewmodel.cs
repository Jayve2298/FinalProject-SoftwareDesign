using FinalProject_Jenotan.Commands;
using FinalProject_Jenotan.Database;
using FinalProject_Jenotan.Database.Models.EnrollmentSystem;
using FinalProject_Jenotan.PromptWindow;
using FinalProject_Jenotan.UI.EditWindow.DeptChairEditWindows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace FinalProject_Jenotan.ViewModel.MainFramesViewModel.SchoolSystemMFViewModel.DeptChairMFViewModels
{
    public class DeptChairMajorsMFViewmodel : INotifyPropertyChanged
    {
        private readonly Faculty _deptChair;
        public ICommand DeleteMajorCommand { get; }
        public ICommand EditCommand { get; }
        private ObservableCollection<Major> _majors;
        private List<Major> _allMajors = new();
        public ObservableCollection<Major> Majors
        {
            get => _majors;
            set
            {
                _majors = value;
                OnPropertyChanged(nameof(Majors));
            }
        }
        public ICommand AddCommand { get; }
        private Major _selectedMajor;
        public Major SelectedMajor
        {
            get => _selectedMajor;
            set
            {
                _selectedMajor = value;
                OnPropertyChanged(nameof(SelectedMajor));
            }
        }

        public DeptChairMajorsMFViewmodel(Faculty deptChair)
        {
            _deptChair = deptChair;
            AddCommand = new RelayCommand(OpenAddMajor);
            DeleteMajorCommand = new RelayCommand(DeleteMajor);
            EditCommand = new RelayCommand(OpenEditMajor);
            LoadMajors();
        }

        public void OpenEditMajor(object obj)
        {
            if (SelectedMajor == null)
                return;

            

            var window = new DeptChairMajorEditWindow(_deptChair, SelectedMajor);

            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            bool? result = window.ShowDialog();

            if (result == true)
            {
                StatusMessage = "Major Updated";
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

                LoadMajors();
            }

            LoadMajors();
        }

        public void OpenAddMajor(object obj)
        {
            var window = new DeptChairAddMajorWindow(_deptChair);

            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;



            bool? result = window.ShowDialog();

            if (result == true)
            {
                StatusMessage = "Major Added";
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
                LoadMajors();


            }
        }

        private void DeleteMajor(object obj)
        {
            if (SelectedMajor == null)
                return;

            var confirmWindow = new ConfirmationWindow
            {
                Owner = Application.Current.MainWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            confirmWindow.ShowDialog();


            if (!confirmWindow.result)
                return;

            using (var context = new TinyCollegeDbContextSQLServer())
            {
                
                var major = context.Majors
                    .FirstOrDefault(m => m.MajorId == SelectedMajor.MajorId);

                if (major != null)
                {
                    
                    var students = context.Students
                        .Where(s => s.MajorId == major.MajorId)
                        .ToList();

                    
                    foreach (var student in students)
                    {
                        student.MajorId = null;
                        student.MajorLink = null;
                    }

                    
                    major.StudentCount = 0;

                   
                    major.IsActive = false;

                    context.SaveChanges();
                }
            }

            StatusMessage = "Major Deleted";
            StatusMessageBrush = Brushes.Red;

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

            LoadMajors();
        }

        private void LoadMajors()
        {
            using (var context = new TinyCollegeDbContextSQLServer())
            {
                var majors = context.Majors
                    .Where(m => m.DepartmentId == _deptChair.DepartmentId && m.IsActive)
                    .ToList();

                _allMajors = majors; // 🔥 store full list
                Majors = new ObservableCollection<Major>(majors);
            }
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

        private void FilterMajors()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                Majors = new ObservableCollection<Major>(_allMajors);
            }
            else
            {
                var filtered = _allMajors
                    .Where(m => m.MajorName
                        .Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                Majors = new ObservableCollection<Major>(filtered);
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
                FilterMajors();
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
