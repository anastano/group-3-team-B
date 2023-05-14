using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.WPF.ViewModels.GuideViewModels
{
    class TourRequestsStatisticsViewModel : INotifyPropertyChanged
    {
        #region PropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Commands

        #endregion

        private RegularTourRequestService _tourRequestsService;
        private TourRequestsStatisticsService _tourRequestsStatisticsService;

        public List<string> PossibleLanguages { get; set; }
        public List<Location> PossibleLocations { get; set; }

        private string _selectedLanguage;
        public string SelectedLanguage
        {
            get { return _selectedLanguage; }
            set
            {
                _selectedLanguage = value;
                OnPropertyChanged();
                LoadRequestsCount();
            }
        }

        private Location _selectedLocation;
        public Location SelectedLocation
        {
            get { return _selectedLocation; }
            set
            {
                _selectedLocation = value;
                OnPropertyChanged();
                LoadRequestsCount();
            }
        }

        private int _selectedYear;
        public int SelectedYear
        {
            get { return _selectedYear; }
            set
            {
                _selectedYear = value;
                OnPropertyChanged();
                LoadRequestsCountPerMonth();
            }
        }

        private Dictionary<int, int> _requestsPerYear;
        public Dictionary<int, int> RequestsPerYear
        {
            get { return _requestsPerYear; }
            set
            {
                _requestsPerYear = value;
                OnPropertyChanged();
            }
        }

        private Dictionary<int, int> _requestsPerMonth;
        public Dictionary<int, int> RequestsPerMonth
        {
            get { return _requestsPerMonth; }
            set
            {
                _requestsPerMonth = value;
                OnPropertyChanged();
            }
        }

        public TourRequestsStatisticsViewModel()
        {
            _tourRequestsService = new RegularTourRequestService();
            _tourRequestsStatisticsService = new TourRequestsStatisticsService();

            PossibleLanguages = _tourRequestsService.GetAll().Select(r => r.Language).Distinct().ToList();
            PossibleLocations = _tourRequestsService.GetAll().Select(r => r.Location).Distinct().ToList();
        }

        public void LoadRequestsCount()
        {
            LoadRequestsCountPerYear();
            SelectedYear = RequestsPerYear == null ? 0 : RequestsPerYear.First().Key;
            LoadRequestsCountPerMonth();
        }

        private void LoadRequestsCountPerYear()
        {
            RequestsPerYear = _tourRequestsStatisticsService.GetTourRequesPerYear(SelectedLanguage, SelectedLocation);
        }

        private void LoadRequestsCountPerMonth()
        {
            RequestsPerMonth = _tourRequestsStatisticsService.GetTourRequesPerMonth(SelectedYear, SelectedLanguage, SelectedLocation);
        }
    }
}
