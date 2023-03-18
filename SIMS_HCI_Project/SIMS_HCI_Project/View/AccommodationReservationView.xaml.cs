using SIMS_HCI_Project.Controller;
using SIMS_HCI_Project.Model;
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
    public partial class AccommodationReservationView : Window, INotifyPropertyChanged, IDataErrorInfo
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
            End = DateTime.Now.AddDays(1);
        }

        private Regex _DaysNumberRegex = new Regex("[1-9][0-9]*");
        private Regex _DateRegex = new Regex("^(0?[1-9]|1[0-2])\\/(0?[1-9]|[1-2][0-9]|3[0-1])\\/\\d{4}$");
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
                        result = "Days number must be greater than "+Accommodation.MinimumReservationDays;
                    }
                }
                else if(columnName == "Start")
                {
                    Match match = _DaysNumberRegex.Match(DaysNumber.ToString());
                    if (Start == null)
                        result = "Start date is required";
                    else if (!match.Success)
                        result = "It must be in MM/dd/yyyy format";
                    else if (Start <= DateTime.Now)
                    {
                        result = "Start cannot be a day that has already passed";
                    }
                }
                else if(columnName == "End")
                {
                    Match match = _DaysNumberRegex.Match(DaysNumber.ToString());
                    if (End == null)
                        result = "End date is required";
                    else if (!match.Success)
                        result = "It must be in MM/dd/yyyy format";
                    else if (End <= DateTime.Now)
                    {
                        result = "End cannot be a day that has already passed";
                    }
                    else if(End < Start)
                    {
                        result = "End cannot be before the start";
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
                _accommodationReservationController.Add(SelectedReservation, Guest);
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
                FindAvailableReservations(availableReservations, (DateTime)datePickerStart.SelectedDate, (DateTime)datePickerEnd.SelectedDate);
            }
            else
            {
                MessageBox.Show("Fields are not validly filled in");
            }

            CheckAvaiableReservations(availableReservations);

        }

        private void FindAvailableReservations(List<AccommodationReservation> availableReservations, DateTime start, DateTime end)
        {
            DateTime currentDate = start;
            DateTime endDate = end;
            while (currentDate <= endDate)
            {
                DateTime nextDate = currentDate.AddDays(int.Parse(txtDays.Text) - 1);

                if (nextDate > endDate)
                {
                    break;
                }

                bool pairExists = false;

                foreach (AccommodationReservation reservation in Accommodation.Reservations)
                {
                    if (IsDateRangeOverlaps(currentDate, nextDate, reservation))
                    {
                        pairExists = true;
                        break;
                    }
                }

                if (!pairExists)
                {
                    availableReservations.Add(new AccommodationReservation(Accommodation.Id, Guest.Id, currentDate, nextDate, GuestsNumber));
                }

                currentDate = currentDate.AddDays(1);

            }
        }

        private void CheckAvaiableReservations(List<AccommodationReservation> availableReservations)
        {
            if (availableReservations.Count == 0)
            {
                List<AccommodationReservation> suggestedReservations = new List<AccommodationReservation>();
                txtSuggestion.Text = "There are no available reservations for the selected dates, here are a few recommendations for dates close to the selected ones";
                //15 days before start date
                FindAvailableReservations(availableReservations, ((DateTime)datePickerStart.SelectedDate).AddDays(-15), (DateTime)datePickerStart.SelectedDate);
                suggestedReservations.AddRange(availableReservations);
                //15 days after end date
                FindAvailableReservations(availableReservations, (DateTime)datePickerEnd.SelectedDate, ((DateTime)datePickerEnd.SelectedDate).AddDays(15));
                suggestedReservations.AddRange(availableReservations);

                AvailableReservations = suggestedReservations;
            }
            else
            {
                txtSuggestion.Text = "Available reservations for the selected days";
                AvailableReservations = availableReservations;
            }
        }

        private static bool IsDateRangeOverlaps(DateTime currentDate, DateTime nextDate, AccommodationReservation reservation)
        {
            return (currentDate >= reservation.Start && currentDate <= reservation.End) ||
                                        (nextDate >= reservation.Start && nextDate <= reservation.End) ||
                                        (currentDate <= reservation.Start && nextDate >= reservation.End);
        }

        private void DatePicker_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // If the key pressed is not a digit or a separator character, suppress the input
            if (!char.IsDigit((char)e.Key) && e.Key != Key.OemPeriod && e.Key != Key.Divide && e.Key != Key.Subtract)
            {
                e.Handled = true;
            }
        }
        private void DatePickerStart_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datePickerEnd.SelectedDate.HasValue && datePickerEnd.SelectedDate.Value < datePickerStart.SelectedDate.Value)
            {
                datePickerEnd.SelectedDate = datePickerStart.SelectedDate;
            }
        }
        private void DatePickerEnd_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datePickerStart.SelectedDate == null)
            {
                datePickerStart.SelectedDate = datePickerEnd.SelectedDate;
            }
            else if (datePickerEnd.SelectedDate.HasValue && datePickerEnd.SelectedDate.Value < datePickerStart.SelectedDate.Value)
            {
                datePickerEnd.SelectedDate = datePickerStart.SelectedDate;
            }
        }
    }
}
