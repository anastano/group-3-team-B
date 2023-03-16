using SIMS_HCI_Project.Controller;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for OwnerView.xaml
    /// </summary>
    public partial class OwnerView : Window
    {
        private string _ownerId;

        private AccommodationController _accommodationController;
        private OwnerController _ownerController;
        private LocationController _locationController;
        private AccommodationReservationController _reservationController;
        private OwnerGuestRatingController _ownerGuestRatingController;

        public OwnerView(string ownerId)
        {
            InitializeComponent();

            _ownerId = ownerId;

            LoadFromFiles();

            if(_ownerController.GetUnratedReservations(_ownerId).Count != 0)
            {
                MessageBox.Show("You have unrated guests! Rate them!");
            }
        }

        void LoadFromFiles()
        {
            _accommodationController = new AccommodationController();
            _ownerController = new OwnerController();
            _locationController = new LocationController();
            _reservationController = new AccommodationReservationController();
            _ownerGuestRatingController = new OwnerGuestRatingController();

            _accommodationController.Load();
            _ownerController.Load();
            _reservationController.Load();
            _ownerGuestRatingController.Load();

            _ownerController.FillOwnerAccommodationList(); //fills accommodation list for each owner
            _ownerController.FillOwnerReservationList(); 
            _accommodationController.ConnectAccommodationsWithLocations(_locationController);
        }

        private void btnAccommodationRegistration_Click(object sender, RoutedEventArgs e)
        {
            Window accommodationRegistration = new AccommodationRegistrationView(_accommodationController, _ownerId);
            accommodationRegistration.Show();
        }

        private void btnGuestRating_Click(object sender, RoutedEventArgs e)
        {
            Window reservationsView = new OwnerUnratedReservationsView(_reservationController, _ownerController, _ownerGuestRatingController, _ownerId);
            reservationsView.Show();
        }

        private void btnExistingAccommodations_Click(object sender, RoutedEventArgs e)
        {
            Window accommodationsView = new OwnerAccommodationsView(_accommodationController, _ownerController, _ownerId);
            accommodationsView.Show();
        }
    }
}
