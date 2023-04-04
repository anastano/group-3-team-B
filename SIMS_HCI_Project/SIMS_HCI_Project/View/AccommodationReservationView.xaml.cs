using SIMS_HCI_Project.Controller;
using SIMS_HCI_Project.Model;
using SIMS_HCI_Project.Observer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SIMS_HCI_Project.View
{
    /// <summary>
    /// Interaction logic for AccommodationReservationView.xaml
    /// </summary>
    public partial class AccommodationReservationView : Window, INotifyPropertyChanged, IDataErrorInfo, IObserver
    {
        private AccommodationReservationController _accommodationReservationController;
        public AccommodationReservation Reservation { get; set; }
        public AccommodationReservation SelectedReservation { get; set; }

        public Accommodation Accommodation { get; set; }
        public Guest1 Guest { get; set; }

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
        
        private string _daysNumber;
        public string DaysNumber
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
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public AccommodationReservationView(AccommodationReservationController accommodationReservationController, Accommodation accommodation, Guest1 guest)
        {
            InitializeComponent();
            this.DataContext = this;
            _accommodationReservationController = accommodationReservationController;
            Accommodation = accommodation;
            Guest = guest;
            AvailableReservations = new List<AccommodationReservation>();
            GuestsNumber = accommodation.MaxGuests;
            DaysNumber = accommodation.MinimumReservationDays.ToString();
            Start = DateTime.Now.AddDays(1);
            End = DateTime.Now.AddDays(accommodation.MinimumReservationDays);
        }

        private Regex _DaysNumberRegex = new Regex("[1-9][0-9]*");
        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                string result = null;
 
                if (columnName == "DaysNumber")
                {
                    Match match = _DaysNumberRegex.Match(DaysNumber.ToString());
                    if (DaysNumber == null)
                        result = "Number of days is required";
                    else if (!match.Success)
                        result = "Only positive numbers that are greater than 0";
                    else if (int.Parse(DaysNumber) < Accommodation.MinimumReservationDays)
                    {
                        result = "Days number must be greater than " + Accommodation.MinimumReservationDays;
                    }
                    /*else if ((End - Start).TotalDays < int.Parse(DaysNumber) - 1)
                    {
                        result = "Date range should be bigger";
                    }*/
                }
                else if(columnName == "Start")
                {      
                    if (Start == null)
                        result = "Start date is required";
                    else if (Start <= DateTime.Now)
                    {
                        result = "Start cannot be a day that has already passed";
                    }
                    else if (End < Start)
                    {
                        result = "End cannot be before the start";
                    }
                    else if ((End - Start).TotalDays < int.Parse(DaysNumber) - 1)
                    {
                        result = "Date range should be bigger, because of days for reseration";
                    }
                }
                else if(columnName == "End")
                {
                    if (End == null)
                        result = "End date is required";
                    else if (End <= DateTime.Now)
                    {
                        result = "End cannot be a day that has already passed";
                    }
                    else if(End < Start)
                    {
                        result = "End cannot be before the start";
                    }
                    else if ((End - Start).TotalDays < int.Parse(DaysNumber) - 1)
                    {
                        result = "Date range should be bigger, because of days for reseration";
                    }
                }

                return result;
            }
        }
        private readonly string[] _validatedProperties = { "DaysNumber", "Start", "End" };

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
        
        private void btnPlusGuest_Click(object sender, RoutedEventArgs e)
        {            
            if (GuestsNumber >= Accommodation.MaxGuests)
            {
                btnPlusGuest.IsEnabled = false;
            }
            else
            {
                btnPlusGuest.IsEnabled = true;
                GuestsNumber += 1;
            }

        }

        private void btnMinusGuest_Click(object sender, RoutedEventArgs e)
        {
            btnPlusGuest.IsEnabled = true;

            if (GuestsNumber > 1)
            {
                GuestsNumber -= 1;
            }
        }
        private void btnReserve_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = ConfirmReservation();
            if ( result == MessageBoxResult.Yes)
            {
                _accommodationReservationController.Add(Accommodation, SelectedReservation, Guest);
                Close();
            }    
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
                    AvailableReservations = FindAvailableReservations(DateTime.Now.AddDays(1), ((DateTime)datePickerEnd.SelectedDate).AddDays(15+ int.Parse(DaysNumber)));
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
        }
        public void Update()
        {

        }
    }
}
