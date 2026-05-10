using FinalProject_Jenotan.Database;
using FinalProject_Jenotan.Database.Models.EnrollmentSystem;
using FinalProject_Jenotan.Database.Models.MotorPoolSystem.Reservation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace FinalProject_Jenotan.ViewModel.FormsViewModel.FacultyFormsViewModel
{
    public class FacultyCompletionFormViewModel : INotifyPropertyChanged
    {
        private readonly TinyCollegeDbContextSQLServer _context;
        private List<CompletionForm> _allForms;

        public ObservableCollection<CompletionForm> CompletionFormList { get; set; }

        public FacultyCompletionFormViewModel(Faculty faculty)
        {
            _context = new TinyCollegeDbContextSQLServer();

            LoadData();

            SortOptions = new ObservableCollection<string>
        {
            "Latest",
            "Oldest"
        };

            SelectedSort = "Latest";
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

        public ObservableCollection<string> SortOptions { get; set; }

        private string _selectedSort;
        public string SelectedSort
        {
            get => _selectedSort;
            set
            {
                _selectedSort = value;
                OnPropertyChanged(nameof(SelectedSort));
                ApplyFilter();
            }
        }

        private void LoadData()
        {
            _allForms = _context.CompletionForms
            .Include(c => c.CheckoutFormLink)
            .ThenInclude(c => c.ReservationFormLink)
            .OrderByDescending(c => c.DateCompleted)
            .ToList();

            CompletionFormList = new ObservableCollection<CompletionForm>(_allForms);
            OnPropertyChanged(nameof(CompletionFormList));
        }

        private void ApplyFilter()
        {
            if (_allForms == null) return;

            var query = _allForms.AsQueryable();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                query = query.Where(x =>
                x.CFormId.Contains(SearchText) ||
                (x.CheckoutFormLink.ReservationFormLink != null &&
                 x.CheckoutFormLink.ReservationFormLink.Destination.Contains(SearchText)));
            }
            query = SelectedSort == "Oldest"
                ? query.OrderBy(x => x.DateCompleted)
                : query.OrderByDescending(x => x.DateCompleted);

            CompletionFormList = new ObservableCollection<CompletionForm>(query);
            OnPropertyChanged(nameof(CompletionFormList));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
