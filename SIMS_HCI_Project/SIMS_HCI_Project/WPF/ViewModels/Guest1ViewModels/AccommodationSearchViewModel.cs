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
    internal class AccommodationSearchViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private NavigationService _navigationService;
        private readonly AccommodationService _accommodationService;
        private readonly AccommodationReservationService _accommodationReservationService;
        public ObservableCollection<Location> Locations { get; set; }
        public Accommodation SelectedAccommodation { get; set; }
        public Guest1 Guest { get; set; }
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
        private string _guestsNumber;
        public string GuestsNumber
        {
            get => _guestsNumber;
            set
            {
                if (value != _guestsNumber)
                {
                    _guestsNumber = value;
                    OnPropertyChanged(nameof(GuestsNumber));
                    Validate();
                }
            }
        }

        private string _daysNumber;
        public string DaysNumber
        {
            get => _daysNumber;
            set
            {
                if (value != _daysNumber)
                {
                    _daysNumber = value;
                    OnPropertyChanged(nameof(DaysNumber));
                    Validate();
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
            Guest = guest;
            Accommodations = _accommodationService.GetAllSortedBySuperFlag();
            InitCommands();
        }
        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                if (columnName == nameof(GuestsNumber))
                {
                    int result;
                    if (String.IsNullOrEmpty(GuestsNumber))
                    {
                        return null;
                    }
                    else if (!int.TryParse(GuestsNumber, out result))
                    {
                        return "Invalid input. Please enter a valid number.";
                    }
                    else if (result <= 0)
                    {
                        return "Only numbers bigger than 0";
                    }
                    return null;
                }
                else if (columnName == nameof(DaysNumber))
                {
                    int result;
                    if (String.IsNullOrEmpty(DaysNumber))
                    {
                        return null;
                    }
                    else if (!int.TryParse(DaysNumber, out result))
                    {
                        return "Invalid input. Please enter a valid number.";
                    }
                    else if (result <= 0)
                    {
                        return "Only numbers bigger than 0";
                    }
                    return null;
                }
                return null;
            }
        }
        private readonly string[] _validatedProperties = { "DaysNumber", "GuestsNumber" };

        public bool IsValid
        {
            get
            {
                foreach (var property in _validatedProperties)
                {
                    if (this[property] != null)
                        return false;
                }

                return true;
            }
        }
        private void Validate()
        {
            OnPropertyChanged(nameof(GuestsNumber));
            OnPropertyChanged(nameof(DaysNumber));
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
            if (IsValid)
            {
                Accommodations = _accommodationService.Search(Accommodation.Name, Accommodation.Location.Country, Accommodation.Location.City, SelectedAccommodationType, GuestsNumber, DaysNumber);
            }
        }
        public void ExecutedShowImagesCommand(object obj)
        {
            if(SelectedAccommodation != null)
            {
                _navigationService.Navigate(new AccommodationImagesViewModel(SelectedAccommodation, _navigationService), "Accommodation Images");
            }
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
        }
    }
}
