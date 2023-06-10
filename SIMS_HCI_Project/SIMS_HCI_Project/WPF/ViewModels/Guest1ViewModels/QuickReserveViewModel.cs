using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SIMS_HCI_Project.WPF.ViewModels.Guest1ViewModels
{
    internal class QuickReserveViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private NavigationService _navigationService;
        private AccommodationReservationService _accommodationReservationService;
        private AccommodationService _accommodationService;
        private SuperGuestTitleService _titleService;
        public AccommodationReservation SelectedReservation { get; set; }
        public Guest1 Guest { get; set; }
        public RelayCommand SearchCommand { get; set; }
        public RelayCommand ReserveAccommodationCommand { get; set; }
        public RelayCommand ShowImagesCommand { get; set; }

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

        private String _daysNumber;
        public String DaysNumber
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
        private DateTime? _start;
        public DateTime? Start
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
        private DateTime? _end;
        public DateTime? End
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

        public QuickReserveViewModel(Guest1 guest, NavigationService navigationService)
        {
            _navigationService = navigationService;
            _accommodationReservationService = new AccommodationReservationService();
            _accommodationService = new AccommodationService();
            _titleService = new SuperGuestTitleService();
            Guest = guest;
            AvailableReservations = new List<AccommodationReservation>();
            GuestsNumber = 1.ToString();
            SuggestionText = "";
            Start = null;
            End = null;
            DaysNumber = 1.ToString();
            InitCommands();
        }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                if (columnName == nameof(Start))
                {
                    if (!Start.HasValue && End.HasValue)
                    {
                        return "Input start";
                    }
                    else if (Start.HasValue && Start <= DateTime.Today )
                    {
                        return "Date must be after today.";
                    }
                }
                else if (columnName == nameof(End))
                {
                    if (!End.HasValue && Start.HasValue)
                    {
                        return "Input end";
                    }
                    else if (End.HasValue && End <= DateTime.Today)
                    {
                        return "Date must be after today.";
                    }
                }
                else if (columnName == nameof(GuestsNumber))
                {
                    int result;
                    if (!int.TryParse(GuestsNumber, out result))
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
                    if (!int.TryParse(DaysNumber, out result))
                    {
                        return "Invalid input. Please enter a valid number.";
                    }
                    else if (result <= 0)
                    {
                        return "Only numbers bigger than 0";
                    }
                }
                if (columnName == nameof(End) || columnName == nameof(Start))
                {
                    if (End.HasValue && Start.HasValue && Start > End)
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
                    if(Start.HasValue && End.HasValue)
                    {
                        if ((((DateTime)End - (DateTime)Start).TotalDays + 1) >= int.Parse(DaysNumber) || (((DateTime)End - (DateTime)Start).TotalDays == 0 && int.Parse(DaysNumber) == 1))
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
                    _titleService.UpdateSuperGuestTitle(Guest);
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
            AvailableReservations = _accommodationReservationService.GetAvailableReservationsForAllAccommodations(Guest, Start, End, int.Parse(DaysNumber), int.Parse(GuestsNumber));
            SuggestionText = "Available reservations";
        }
        public void ExecutedShowImagesCommand(object obj)
        {
            if (SelectedReservation != null)
            {
                _navigationService.Navigate(new AccommodationImagesViewModel(SelectedReservation.Accommodation, _navigationService), "Accommodation Images");
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
