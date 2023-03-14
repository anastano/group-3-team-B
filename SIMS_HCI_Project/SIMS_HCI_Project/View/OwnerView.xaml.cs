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

        public OwnerView(string ownerId)
        {
            InitializeComponent();

            _ownerId = ownerId;

            LoadFromFiles();
        }

        void LoadFromFiles()
        {
            _accommodationController = new AccommodationController();
            _ownerController = new OwnerController();
            _locationController = new LocationController();

            _accommodationController.Load();
            _ownerController.Load();

            _ownerController.FillOwnerAccommodationList(); //fills accommodation list for each owner
            _accommodationController.ConnectAccommodationsWithLocations(_locationController);
        }

        private void btnAccommodationRegistration_Click(object sender, RoutedEventArgs e)
        {
            Window accommodationRegistration = new AccommodationRegistrationView(_accommodationController, _ownerId);
            accommodationRegistration.Show();
        }

        private void btnGuestRating_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnExistingAccommodations_Click(object sender, RoutedEventArgs e)
        {
            Window accommodationView = new OwnerAccommodationsView(_accommodationController, _ownerController, _ownerId);
            accommodationView.Show();
        }
    }
}
