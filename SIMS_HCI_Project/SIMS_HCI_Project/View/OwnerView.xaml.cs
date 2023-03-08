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

        AccommodationController accommodationController;
        OwnerController ownerController;

        public OwnerView(string ownerId)
        {
            InitializeComponent();

            _ownerId = ownerId;

            LoadFromFiles();
        }

        void LoadFromFiles()
        {
            accommodationController = new AccommodationController();
            ownerController = new OwnerController();

            accommodationController.Load();
            ownerController.Load();
            ownerController.FillOwnerAccommodationList(); //fills accommodation list for each owner
        }

        private void btnAccommodationRegistration_Click(object sender, RoutedEventArgs e)
        {
            Window accommodationRegistration = new AccommodationRegistrationView(accommodationController, _ownerId);
            accommodationRegistration.Show();
        }

        private void btnGuestRating_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnExistingAccommodations_Click(object sender, RoutedEventArgs e)
        {
            Window accommodationView = new AccommodationView(accommodationController, ownerController, _ownerId);
            accommodationView.Show();
        }
    }
}
