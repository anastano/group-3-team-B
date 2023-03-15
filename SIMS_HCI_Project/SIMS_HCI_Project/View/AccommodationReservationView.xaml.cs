using SIMS_HCI_Project.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
    public partial class AccommodationReservationView : Window
    {
        public ObservableCollection<AccommodationReservation> AvailableReservations { get; set; }
        public AccommodationReservation SelectedReservation { get; set; }

        public Accommodation Accommodation { get; set; }
        public Guest1 Guest { get; set; }
        public AccommodationReservationView(Accommodation accommodation, Guest1 guest)
        {
            InitializeComponent();
            this.DataContext = this;
            Accommodation = accommodation;
            Guest = guest;
            AvailableReservations = new ObservableCollection<AccommodationReservation>();
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

        }
    }
}
