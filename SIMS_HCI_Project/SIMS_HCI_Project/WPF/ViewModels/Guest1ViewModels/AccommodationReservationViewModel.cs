using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Controller;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace SIMS_HCI_Project.WPF.ViewModels.Guest1ViewModels
{
    internal class AccommodationReservationViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private NavigationService _navigationService;
        private AccommodationReservationService _accommodationReservationService;
        private SuperGuestTitleService _titleService;
        public AccommodationReservation SelectedReservation { get; set; }
        public Accommodation Accommodation { get; }
        public Guest1 Guest { get; set; }
        public RelayCommand PlusGuestNumberCommand { get; set; }
        public RelayCommand MinusGuestNumberCommand { get; set; }
        public RelayCommand PlusDaysNumberCommand { get; set; }
        public RelayCommand MinusDaysNumberCommand { get; set; }
        public RelayCommand SearchCommand { get; set; }
        public RelayCommand ReserveAccommodationCommand { get; set; }
        public RelayCommand BackCommand { get; set; }

        private List<AccommodationReservation> _availableReservations;
        public List<AccommodationReservation> AvailableReservations
        {
            get => _availableReservations;
            set
            {
                if (value != _availableReservations)
                {

                    _availableReservations = value;
                    OnPropertyChanged(nameof(AvailableReservations));
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
                    OnPropertyChanged(nameof(DaysNumber));
                    Validate();
                }
            }
        }
        private DateTime _start;
        public DateTime Start
        {
            get => _start;
            set
            {
                if (value != _start)
                {
                    _start = value;
                    OnPropertyChanged(nameof(Start));
                    Validate();
                }
            }
        }
        private DateTime _end;
        public DateTime End
        {
            get => _end;
            set
            {
                if (value != _end)
                {
                    _end = value;
                    OnPropertyChanged(nameof(End));
                    Validate();
                }
            }
        }
        private string _suggestionText;
        public string SuggestionText
        {
            get => _suggestionText;
            set
            {
                if (value != _suggestionText)
                {

                    _suggestionText = value;
                    OnPropertyChanged(nameof(SuggestionText));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public AccommodationReservationViewModel(Accommodation accommodation, Guest1 guest, NavigationService navigationService)
        {
            _navigationService = navigationService;
            _accommodationReservationService = new AccommodationReservationService();
            _titleService = new SuperGuestTitleService();
            Accommodation = accommodation;
            Guest = guest;
            AvailableReservations = new List<AccommodationReservation>();
            GuestsNumber = 1.ToString();
            DaysNumber = accommodation.MinimumReservationDays;
            Start = DateTime.Today.AddDays(1);
            End = DateTime.Today.AddDays(accommodation.MinimumReservationDays);
            InitCommands();
        }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                if (columnName == nameof(Start))
                {
                    if (Start <= DateTime.Today)
                    {
                        return "Date must be after today.";
                    }
                }
                else if (columnName == nameof(End))
                {
                    if (End <= DateTime.Today)
                    {
                        return "Date must be after today.";
                    }
                }
                else if (columnName == nameof(DaysNumber))
                {
                    if ((DaysNumber >= Accommodation.MinimumReservationDays) == false)
                    {
                        return $"Minimum reservation duration is {Accommodation.MinimumReservationDays} days.";
                    }
                }
                else if (columnName == nameof(GuestsNumber))
                {
                    int result;
                    if (!int.TryParse(GuestsNumber, out result))
                    {
                        return "Invalid input. Please enter a valid number.";
                    }
                    else if(result <= 0)
                    {
                        return "Only numbers bigger than 0";
                    }
                    else if(result > Accommodation.MaxGuests)
                    {
                        return $"Max guests number is {Accommodation.MaxGuests}";
                    }
                    return null;
                }
                if (columnName == nameof(End) || columnName == nameof(Start))
                {
                    if (Start > End)
                    {
                        return "Start date must be before the end date.";
                    }
                    else
                    {
                        return null;
                    }

                }

                // Custom cross-field validation
                if (columnName == nameof(Start) || columnName == nameof(End) || columnName == nameof(DaysNumber))
                {
                    if (((End - Start).TotalDays + 1) >= DaysNumber || ((End - Start).TotalDays  == 0 && DaysNumber == 1))
                    {
                        return null;
                    }
                    else
                    {
                        return $"Start-end date range must be greater than number of days.";
                    }
                }

                return null;
            }
        }
        private readonly string[] _validatedProperties = { "DaysNumber", "Start", "End", "GuestsNumber" };

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
            OnPropertyChanged(nameof(Start));
            OnPropertyChanged(nameof(End));
            OnPropertyChanged(nameof(DaysNumber));
        }

        private MessageBoxResult ConfirmReservation()
        {
            string sMessageBoxText = $"This reservation will be made, are you sure?";
            string sCaption = "Reservation Confirm";

            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

            MessageBoxResult result = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
            return result;
        }
        public void ExecutedReserveAccommodationCommand(object obj)
        {
            if (SelectedReservation != null)
            {
                MessageBoxResult result = ConfirmReservation();
                if (result == MessageBoxResult.Yes)
                {
                    _accommodationReservationService.Add(new AccommodationReservation(SelectedReservation));
                    _titleService.UpdateSuperGuestTitle(_accommodationReservationService ,Guest);
                    //UpdateAvailableReservations();
                    _navigationService.Navigate(new ReservationsViewModel(Guest, _navigationService, 0), "My Reservations");
                }
            }
        }
        public void ExecutedSearchCommand(object obj)
        {
            if (IsValid)
            {
                UpdateAvailableReservations();
            }

        }
        private void UpdateAvailableReservations()
        {
            AvailableReservations = _accommodationReservationService.GetAvailableReservations(Accommodation, Guest, Start, End, DaysNumber, int.Parse(GuestsNumber));
            if (_accommodationReservationService.CheckIfSuggestionIsNeeded(AvailableReservations) == true)
            {
                SuggestionText = "There are no available reservations for the selected dates, here are a few recommendations for dates close to the selected ones";
                AvailableReservations = _accommodationReservationService.GetSuggestedAvailableReservations(Accommodation, Guest, Start, End, DaysNumber, int.Parse(GuestsNumber));
            }
            else
            {
                SuggestionText = "Available reservations for the selected days";
            }
        }

        public void ExecutedBackCommand(object obj)
        {
            _navigationService.NavigateBack();
        }

        public void ExecutedMinusDaysNumberCommand(object obj)
        {
           DaysNumber -= 1;
        }
        public void ExecutedPlusDaysNumberCommand(object obj)
        {
           DaysNumber += 1;
        }

        public bool CanExecute(object obj)
        {
            return true;
        }
        public bool CanBackExecute(object obj)
        {
            return true;
        }
        public void InitCommands()
        {
            ReserveAccommodationCommand = new RelayCommand(ExecutedReserveAccommodationCommand, CanExecute);
            SearchCommand = new RelayCommand(ExecutedSearchCommand, CanExecute);
            MinusDaysNumberCommand = new RelayCommand(ExecutedMinusDaysNumberCommand, CanExecute);
            PlusDaysNumberCommand = new RelayCommand(ExecutedPlusDaysNumberCommand, CanExecute);
            BackCommand = new RelayCommand(ExecutedBackCommand, CanBackExecute);
        }
    }
}
