using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Controller;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
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
    internal class AccommodationReservationViewModel
    {
        private AccommodationReservationService _accommodationReservationService;
        public AccommodationReservation SelectedReservation { get; set; }
        public Accommodation Accommodation { get; set; }
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
        private DateTime _start;
        public DateTime Start
        {
            get => _start;
            set
            {
                if (value != _start)
                {
                    _start = value;
                    OnPropertyChanged();
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
                    OnPropertyChanged();
                }
            }
        }
        private bool _isBtnMinusDaysEnabled;
        public bool IsBtnMinusDaysEnabled
        {
            get => _isBtnMinusDaysEnabled;
            set
            {
                if (value != _isBtnMinusDaysEnabled)
                {
                    _isBtnMinusDaysEnabled = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _isBtnPlusDaysEnabled;
        public bool IsBtnPlusDaysEnabled
        {
            get => _isBtnPlusDaysEnabled;
            set
            {
                if (value != _isBtnPlusDaysEnabled)
                {
                    _isBtnPlusDaysEnabled = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _isBtnMinusGuestEnabled;
        public bool IsBtnMinusGuestEnabled
        {
            get => _isBtnMinusGuestEnabled;
            set
            {
                if (value != _isBtnMinusGuestEnabled)
                {
                    _isBtnMinusGuestEnabled = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _isBtnPlusGuestEnabled;
        public bool IsBtnPlusGuestEnabled
        {
            get => _isBtnPlusGuestEnabled;
            set
            {
                if (value != _isBtnPlusGuestEnabled)
                {
                    _isBtnPlusGuestEnabled = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _isClosed;
        public bool IsClosed
        {
            get { return _isClosed; }
            set
            {
                _isClosed = value;
                OnPropertyChanged(nameof(IsClosed));
                if (value)
                {
                    Closed?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        public event EventHandler Closed;
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public AccommodationReservationViewModel(Accommodation accommodation, Guest1 guest)
        {
            _accommodationReservationService = new AccommodationReservationService();
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
                    /*_reservationRescheduleViewModel = new ReservationRescheduleViewModel(SelectedReservation);
                    _reservationRescheduleViewModel.Closed += UnloadUserControl;
                    CurrentViewModel = _reservationRescheduleViewModel;*/
                }
            }
        }
        public void ExecutedBackCommand(object obj)
        {
            IsClosed = true;
        }
        public void ExecutedSearchCommand(object obj)
        {
            //kasnije
        }
        public void ExecutedMinusGuestNumberCommand(object obj)
        {
            IsBtnPlusGuestEnabled = true;

            if (GuestsNumber > 1)
            {
                GuestsNumber -= 1;
            }
        }
        public void ExecutedPlusGuestNumberCommand(object obj)
        {
            if (GuestsNumber >= Accommodation.MaxGuests)
            {
                IsBtnPlusGuestEnabled = false;
            }
            else
            {
                IsBtnPlusGuestEnabled = true;
                GuestsNumber += 1;
            }
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
            MinusGuestNumberCommand = new RelayCommand(ExecutedMinusGuestNumberCommand, CanExecute);
            PlusGuestNumberCommand = new RelayCommand(ExecutedPlusGuestNumberCommand, CanExecute);
            MinusDaysNumberCommand = new RelayCommand(ExecutedMinusDaysNumberCommand, CanExecute);
            PlusDaysNumberCommand = new RelayCommand(ExecutedPlusDaysNumberCommand, CanExecute);
            BackCommand = new RelayCommand(ExecutedBackCommand, CanExecute);
        }
        /*
                private void btnFindAvailable_Click(object sender, RoutedEventArgs e)
                {
                    List<AccommodationReservation> availableReservations = new List<AccommodationReservation>();
                    if (IsValid)
                    {
                        availableReservations = FindAvailableReservations((DateTime)datePickerStart.SelectedDate, (DateTime)datePickerEnd.SelectedDate);
                        CheckIfSuggestionIsNeeded(availableReservations);
                    }
                    else
                    {
                        MessageBox.Show("Fields are not validly filled in");
                    }

                }

                private List<AccommodationReservation> FindAvailableReservations(DateTime start, DateTime end)
                {
                    List<AccommodationReservation> availableReservations = new List<AccommodationReservation>();
                    DateTime potentialStart = start;
                    DateTime endDate = end;
                    while (potentialStart <= endDate)
                    {
                        DateTime potentialEnd = potentialStart.AddDays(int.Parse(txtDays.Text) - 1);

                        if (potentialEnd > endDate)
                        {
                            break;
                        }

                        bool dateRangeOverlaps = false;

                        foreach (AccommodationReservation reservation in Accommodation.Reservations)
                        {
                            if (IsDateRangeOverlapping(potentialStart, potentialEnd, reservation))
                            {
                                dateRangeOverlaps = true;
                                break;
                            }
                        }

                        if (!dateRangeOverlaps)
                        {
                            availableReservations.Add(new AccommodationReservation(Accommodation.Id, Guest.Id, potentialStart, potentialEnd, GuestsNumber));
                        }

                        potentialStart = potentialStart.AddDays(1);

                    }
                    return availableReservations;
                }

                private void CheckIfSuggestionIsNeeded(List<AccommodationReservation> availableReservations)
                {
                    if (availableReservations.Count == 0)
                    {
                        txtSuggestion.Text = "There are no available reservations for the selected dates, here are a few recommendations for dates close to the selected ones";
                        //15 days after end date
                        int lastElementIndex = Accommodation.Reservations.Count - 1;
                        if (Accommodation.Reservations[lastElementIndex].End < DateTime.Now.AddDays(1))
                        {
                            AvailableReservations = FindAvailableReservations(DateTime.Now.AddDays(1), ((DateTime)datePickerEnd.SelectedDate).AddDays(15 + int.Parse(DaysNumber)));
                        }
                        else
                        {
                            AvailableReservations = FindAvailableReservations(Accommodation.Reservations[lastElementIndex].End.AddDays(1), ((DateTime)datePickerEnd.SelectedDate).AddDays(15));
                        }
                    }
                    else
                    {
                        txtSuggestion.Text = "Available reservations for the selected days";
                        AvailableReservations = availableReservations;
                    }
                }
                private static bool IsDateRangeOverlapping(DateTime potentialStart, DateTime potentialEnd, AccommodationReservation reservation)
                {
                    bool isPotentialStartOverlap = potentialStart >= reservation.Start && potentialStart <= reservation.End;
                    bool isPotentialEndOverlap = potentialEnd >= reservation.Start && potentialEnd <= reservation.End;
                    bool isPotentialRangeOverlap = potentialStart <= reservation.Start && potentialEnd >= reservation.End;
                    return isPotentialStartOverlap || isPotentialEndOverlap || isPotentialRangeOverlap;
                }*/
    }
}
