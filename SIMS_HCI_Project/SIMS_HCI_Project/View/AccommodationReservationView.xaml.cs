using SIMS_HCI_Project.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SIMS_HCI_Project.View
{
    /// <summary>
    /// Interaction logic for AccommodationReservationView.xaml
    /// </summary>
    public partial class AccommodationReservationView : Window, INotifyPropertyChanged
    {
        public AccommodationReservation SelectedReservation { get; set; }

        public Accommodation Accommodation { get; set; }
        public Guest1 Guest { get; set; }

        private List<AccommodationReservation> availableReservations;
        public List<AccommodationReservation> AvailableReservations
        {
            get => availableReservations;
            set
            {
                if (value != availableReservations)
                {

                    availableReservations = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public AccommodationReservationView(Accommodation accommodation, Guest1 guest)
        {
            InitializeComponent();
            this.DataContext = this;
            Accommodation = accommodation;
            Guest = guest;
            AvailableReservations = new List<AccommodationReservation>();
        }

        private void btnPlusGuest_Click(object sender, RoutedEventArgs e)
        {
            //VALIDATION FOR MAXGUEST
            int maxGuests = int.Parse(txtGuestNumber.Text);
            maxGuests += 1;
            txtGuestNumber.Text = maxGuests.ToString();
            
        }

        private void btnMinusGuest_Click(object sender, RoutedEventArgs e)
        {
            int maxGuests = int.Parse(txtGuestNumber.Text);

            if (maxGuests > 1)
            {
                maxGuests -= 1;
            }
            txtGuestNumber.Text = maxGuests.ToString();
        }
        private void btnReserve_Click(object sender, RoutedEventArgs e)
        {
            //Window win = new AccommodationReservationView(Accommodation, Guest);
        }

        private void btnFindAvailable_Click(object sender, RoutedEventArgs e)
        {
            List<AccommodationReservation> result = new List<AccommodationReservation>();

            DateTime currentDate = (DateTime)datePickerStart.SelectedDate;

            while (currentDate <= (DateTime)datePickerEnd.SelectedDate)
            {
                DateTime nextDate = currentDate.AddDays(int.Parse(txtDays.Text) - 1);

                if (nextDate > (DateTime)datePickerEnd.SelectedDate)
                {
                    break;
                }

                bool pairExists = false;

                foreach (AccommodationReservation pair in Accommodation.Reservations)
                {
                    if ((currentDate >= pair.Start && currentDate <= pair.End) ||
                        (nextDate >= pair.Start && nextDate <= pair.End) ||
                        (currentDate <= pair.Start && nextDate >= pair.End))
                    {
                        pairExists = true;
                        break;
                    }
                }

                if (!pairExists)
                {
                    result.Add(new AccommodationReservation(Accommodation.Id, Guest.Id, currentDate, nextDate, int.Parse(txtGuestNumber.Text), 0));
                }

                currentDate = currentDate.AddDays(1);
            }

            AvailableReservations = result;
        }
    }
}
