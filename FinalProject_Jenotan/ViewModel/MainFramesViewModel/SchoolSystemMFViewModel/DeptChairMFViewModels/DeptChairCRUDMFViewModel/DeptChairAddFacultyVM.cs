using FinalProject_Jenotan.Commands;
using FinalProject_Jenotan.Database;
using FinalProject_Jenotan.Database.Models.EnrollmentSystem;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace FinalProject_Jenotan.ViewModel.MainFramesViewModel.SchoolSystemMFViewModel.DeptChairMFViewModels.DeptChairCRUDMFViewModel
{

    public enum FacultyRole
    {
        Professor,
        Researcher
    }

    public class DeptChairAddFacultyVM : INotifyPropertyChanged
    {
        private readonly Faculty _deptChair;
        private readonly Action<bool?> _closeDialog;

        public ICommand ConfirmCommand { get; }

        public DeptChairAddFacultyVM(Faculty deptChair, Action<bool?> closeDialog)
        {
            _deptChair = deptChair;
            _closeDialog = closeDialog;

            FacultyRole = new ObservableCollection<Role>
            {
                Role.Professor,
                Role.Researcher
            };

            SelectedFacultyRole = Role.Professor; //default

            ConfirmCommand = new RelayCommand(AddFaculty);
        }

        private string _firstName;
        public string FirstName
        {
            get => _firstName;
            set { _firstName = value; OnPropertyChanged(); }
        }

        private string _lastName;
        public string LastName
        {
            get => _lastName;
            set { _lastName = value; OnPropertyChanged(); }
        }

        private string _contactNumber;
        public string ContactNumber
        {
            get => _contactNumber;
            set { _contactNumber = value; OnPropertyChanged(); }
        }

        private void AddFaculty(object obj)
        {

            if (string.IsNullOrWhiteSpace(FirstName) ||
                string.IsNullOrWhiteSpace(LastName) ||
                string.IsNullOrWhiteSpace(ContactNumber))
            {
                MessageBox.Show("Please fill all fields.");
                return;
            }

            using var context = new TinyCollegeDbContextSQLServer();

            var maxNumber = context.Faculties
                .Where(f => f.FacultyId.StartsWith("FAC"))
                .AsEnumerable()
                .Select(f =>
                {
                    var numberPart = f.FacultyId.Substring(3);
                    return int.TryParse(numberPart, out var n) ? n : 0;
                })
                .DefaultIfEmpty(999)
                .Max();

            int nextNumber = maxNumber + 1;

            string facultyId = $"FAC{nextNumber}";
            string password = $"TC{nextNumber}";

            string email = $"{FirstName[0].ToString().ToLower()}.{LastName.ToLower()}@email.com";

            //duplicate check
            bool exists = context.Faculties.Any(f =>
                f.FacultyId == facultyId ||
                f.Email == email);

            if (exists)
            {
                MessageBox.Show("Duplicate faculty detected. Try again.");
                return;
            }
            var newFaculty = new Faculty
            {
                FacultyId = facultyId,
                Password = password,
                FName = Capitalize(FirstName),
                LName = Capitalize(LastName),
                Email = email,
                ContactNumber = ContactNumber,
                DepartmentId = _deptChair.DepartmentId,
                Role = SelectedFacultyRole,
                IsActive = true
            };

            try
            {
                context.Faculties.Add(newFaculty);
                context.SaveChanges();

                MessageBox.Show("Faculty added successfully.");

                _closeDialog?.Invoke(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving faculty: " + ex.Message);
            }
        }



        private string Capitalize(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            return char.ToUpper(input[0]) + input.Substring(1).ToLower();
        }

        public ObservableCollection<Role> FacultyRole { get; set; }

        private Role _selectedFacultyRole;
        public Role SelectedFacultyRole
        {
            get => _selectedFacultyRole;
            set
            {
                _selectedFacultyRole = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
