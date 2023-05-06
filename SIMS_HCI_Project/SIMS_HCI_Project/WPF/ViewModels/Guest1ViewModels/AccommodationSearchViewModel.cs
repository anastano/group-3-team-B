using SIMS_HCI_Project.Controller;
using SIMS_HCI_Project.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Xml.Linq;
using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SIMS_HCI_Project.WPF.Services;

namespace SIMS_HCI_Project.WPF.ViewModels.Guest1ViewModels
{
    internal class AccommodationSearchViewModel : INotifyPropertyChanged
    {
        private NavigationService _navigationService;
        private readonly AccommodationService _accommodationService;
        private readonly AccommodationReservationService _accommodationReservationService;
        public ObservableCollection<Location> Locations { get; set; }
        public Accommodation SelectedAccommodation { get; set; }
        public Guest1 Guest { get; set; }
        public RelayCommand PlusGuestNumberCommand { get; set; }
        public RelayCommand MinusGuestNumberCommand { get; set; }
        public RelayCommand PlusDaysNumberCommand { get; set; }
        public RelayCommand MinusDaysNumberCommand { get; set; }
        public RelayCommand SearchCommand { get; set; }
        public RelayCommand ShowImagesCommand { get; set; }
        public RelayCommand ReserveAccommodationCommand { get; set; }

        private Accommodation _accommodation;
        public Accommodation Accommodation
        {
            get => _accommodation;
            set
            {
                if (value != _accommodation)
                {

                    _accommodation = value;
                    OnPropertyChanged();
                }
            }
        }
        private List<Accommodation> _accommodations;
        public List<Accommodation> Accommodations
        {
            get => _accommodations;
            set
            {
                if (value != _accommodations)
                {

                    _accommodations = value;
                    OnPropertyChanged();
                }
            }
        }
        public string[] AccommodationTypes
        {
            get { return Enum.GetNames(typeof(AccommodationType)); }
        }
        private string _selectedAccommodationType;
        public string SelectedAccommodationType
        {
            get { return _selectedAccommodationType; }
            set
            {
                if (_selectedAccommodationType != value)
                {
                    _selectedAccommodationType = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _guestsNumber;
        public int GuestsNumber
        {
            get => _guestsNumber;
            set
            {
                if (value != _guestsNumber)
                {
                    _guestsNumber = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _daysNumber;
        public int DaysNumber
        {
            get => _daysNumber;
            set
            {
                if (value != _daysNumber)
                {
                    _daysNumber = value;
                    OnPropertyChanged();
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public AccommodationSearchViewModel(Guest1 guest, NavigationService navigationService)
        {
            _navigationService = navigationService;
            _accommodationService = new AccommodationService();
            _accommodationReservationService = new AccommodationReservationService();
            Accommodation = new Accommodation();
            GuestsNumber = 1;
            DaysNumber = 1;
            Guest = guest;
            Accommodations = _accommodationService.GetAllSortedBySuperFlag();
            InitCommands();
        }
        public AccommodationSearchViewModel(Guest1 guest, int guests, int days)
        {
            _accommodationService = new AccommodationService();
            _accommodationReservationService = new AccommodationReservationService();
            Accommodation = new Accommodation();
            GuestsNumber = guests;
            DaysNumber = days;
            Guest = guest;
            Accommodations = _accommodationService.GetAllSortedBySuperFlag();
            InitCommands();
        }
        public void ExecutedReserveAccommodationCommand(object obj)
        {
            if (SelectedAccommodation != null)
            { 
                _navigationService.Navigate(new AccommodationReservationViewModel(SelectedAccommodation, Guest, _navigationService), "Accommodation reservation");
            }
        }
        public void ExecutedSearchCommand(object obj)
        {
            Accommodations = _accommodationService.Search(Accommodation.Name, Accommodation.Location.Country, Accommodation.Location.City, SelectedAccommodationType, GuestsNumber, DaysNumber);
        }
        public void ExecutedShowImagesCommand(object obj)
        {
            if(SelectedAccommodation != null)
            {
                _navigationService.Navigate(new AccommodationImagesViewModel(SelectedAccommodation, _navigationService), "Accommodation Images");
            }
        }
        public void ExecutedMinusGuestNumberCommand(object obj)
        {
            if (GuestsNumber > 1)
            {
                GuestsNumber -= 1;
            }
        }
        public void ExecutedPlusGuestNumberCommand(object obj)
        {
            GuestsNumber += 1;
        }
        public void ExecutedMinusDaysNumberCommand(object obj)
        {
            if (DaysNumber > 1)
            {
                DaysNumber -= 1;
            }
        }
        public void ExecutedPlusDaysNumberCommand(object obj)
        {
            DaysNumber += 1;
        }
        public bool CanExecute(object obj)
        {
            return true;
        }
        public void InitCommands()
        {
            ReserveAccommodationCommand = new RelayCommand(ExecutedReserveAccommodationCommand, CanExecute);
            SearchCommand = new RelayCommand(ExecutedSearchCommand, CanExecute);
            ShowImagesCommand = new RelayCommand(ExecutedShowImagesCommand, CanExecute);
            MinusGuestNumberCommand = new RelayCommand(ExecutedMinusGuestNumberCommand, CanExecute);
            PlusGuestNumberCommand = new RelayCommand(ExecutedPlusGuestNumberCommand, CanExecute);
            MinusDaysNumberCommand = new RelayCommand(ExecutedMinusDaysNumberCommand, CanExecute);
            PlusDaysNumberCommand = new RelayCommand(ExecutedPlusDaysNumberCommand, CanExecute);
        }
    }
}
