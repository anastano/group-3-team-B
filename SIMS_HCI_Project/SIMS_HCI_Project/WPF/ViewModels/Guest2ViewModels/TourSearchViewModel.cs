using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Observer;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Views;
using SIMS_HCI_Project.WPF.Views.Guest2Views;
using System.Windows;
using SIMS_HCI_Project.Applications.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Windows.Navigation;

namespace SIMS_HCI_Project.WPF.ViewModels.Guest2ViewModels
{
    public class TourSearchViewModel : INotifyPropertyChanged, IObserver
    {
        #region Services
        private TourReservationService _tourReservationService;
        private TourService _tourService;
        private LocationService _locationService;
        private TourVoucherService _tourVoucherService;
        #endregion
        #region Commands
        public RelayCommand SearchCommand { get; set; }
        public RelayCommand SeeDetailsAndReserveCommand { get; set; }
        public RelayCommand ResetSearchCommand { get; set; }
        #endregion
        public TourSearchView  TourSearchView { get; set; }
        public Guest2 Guest { get; set; }
        public Tour SelectedTour { get; set; }
        private List<Tour> _tours;
        public List<Tour> Tours
        {
            get { return _tours; }
            set
            {
                _tours = value;
                OnPropertyChanged();
            }
        }
        public SearchAndReserveView SearchAndReserveView { get; set; }
        private NavigationService NavigationService { get; set; }
        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        #region SearchFields
        private string _country;
        public string Country
        {
            get => _country;
            set
            {
                if (_country != value)
                {
                    _country = value;
                }
                OnPropertyChanged();
            }
        }

        private string _city;
        public string City
        {
            get => _city;
            set
            {
                if (_city != value)
                {
                    _city = value;
                }
                OnPropertyChanged();
            }
        }

        private string _duration;
        public string Duration
        {
            get => _duration;
            set
            {
                if (_duration != value)
                {
                    _duration = value;
                }
                OnPropertyChanged();
            }
        }

        private string _language;
        public string Language
        {
            get => _language;
            set
            {
                if (_language != value)
                {
                    _language = value;
                }
                OnPropertyChanged();
            }
        }

        private string _guestNumber;
        public string GuestNumber
        {
            get => _guestNumber;
            set
            {
                if (value != _guestNumber)
                {
                    _guestNumber = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion
        public TourSearchViewModel(TourSearchView tourSearchView, Guest2 guest2, NavigationService navigationService)
        {
            TourSearchView = tourSearchView;
            Guest = guest2;
            NavigationService = navigationService;
            InitCommands();
            LoadFromFiles();

            Tours = new List<Tour>(_tourService.GetAllTourInformation());
        }
        public void LoadFromFiles()
        {
            _tourService = new TourService();
            _locationService = new LocationService();
        }
        public void InitCommands()
        {
            SeeDetailsAndReserveCommand = new RelayCommand(ExecutedReserve, CanExecute);
            SearchCommand = new RelayCommand(ExecuteSearch, CanExecute);
            ResetSearchCommand = new RelayCommand(ExecuteResetSearchCommand, CanExecute);
        }

        private void ExecuteResetSearchCommand(object obj)
        {
            Tours = new List<Tour>(_tourService.GetAllTourInformation());
        }
        #region Commands
        public void ExecuteSearch(object sender)
        {
            int guestsNum;
            bool isValidGuestsNum = int.TryParse(GuestNumber, out guestsNum);
            int duration; 
            bool isValidDuration = int.TryParse(Duration, out duration);

            if (!isValidGuestsNum)
            {
                guestsNum = 0;
            }

            if (!isValidDuration)
            {
                duration = 0;
            }

            Tours = _tourService.Search(Country, City, duration, Language, guestsNum);
        }

        public void ExecutedReserve(object obj)
        {
        
            if (SelectedTour == null)
                {
                    SelectedTour = Tours[0];
                }
            NavigationService.Navigate(new SearchAndReserveView(Guest, SelectedTour, NavigationService));
     
        }
         
        public bool CanExecute(object obj)
        {
            return true;
        }
        
        #endregion
        
        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}
