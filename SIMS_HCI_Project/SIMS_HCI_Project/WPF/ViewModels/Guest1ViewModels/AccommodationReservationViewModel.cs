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
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace SIMS_HCI_Project.WPF.ViewModels.Guest1ViewModels
{
    internal class AccommodationReservationViewModel : INotifyPropertyChanged
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

        private int _guestsNumber;
        public int GuestsNumber
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
            GuestsNumber = accommodation.MaxGuests;
            DaysNumber = accommodation.MinimumReservationDays;
            Start = DateTime.Now.AddDays(1);
            End = DateTime.Now.AddDays(accommodation.MinimumReservationDays);
            InitCommands();
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
                    UpdateAvailableReservations();
                }
            }
        }
        public void ExecutedSearchCommand(object obj)
        {
            UpdateAvailableReservations();

        }
        private void UpdateAvailableReservations()
        {
            AvailableReservations = _accommodationReservationService.GetAvailableReservations(Accommodation, Guest, Start, End, DaysNumber, GuestsNumber);
            if (_accommodationReservationService.CheckIfSuggestionIsNeeded(AvailableReservations) == true)
            {
                SuggestionText = "There are no available reservations for the selected dates, here are a few recommendations for dates close to the selected ones";
                AvailableReservations = _accommodationReservationService.GetSuggestedAvailableReservations(Accommodation, Guest, Start, End, DaysNumber, GuestsNumber);
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
        public void ExecutedMinusGuestNumberCommand(object obj)
        {
           GuestsNumber -= 1;
        }
        public void ExecutedPlusGuestNumberCommand(object obj)
        {
           GuestsNumber += 1;
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
            MinusGuestNumberCommand = new RelayCommand(ExecutedMinusGuestNumberCommand, CanExecute);
            PlusGuestNumberCommand = new RelayCommand(ExecutedPlusGuestNumberCommand, CanExecute);
            MinusDaysNumberCommand = new RelayCommand(ExecutedMinusDaysNumberCommand, CanExecute);
            PlusDaysNumberCommand = new RelayCommand(ExecutedPlusDaysNumberCommand, CanExecute);
            BackCommand = new RelayCommand(ExecutedBackCommand, CanBackExecute);
        }
    }
}
