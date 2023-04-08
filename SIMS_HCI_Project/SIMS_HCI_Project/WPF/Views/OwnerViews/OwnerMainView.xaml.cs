using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
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

namespace SIMS_HCI_Project.WPF.Views.OwnerViews
{
    /// <summary>
    /// Interaction logic for OwnerMainView.xaml
    /// </summary>
    public partial class OwnerMainView : Window
    {
        public Owner Owner { get; set; }

        private OwnerService _ownerService;
        private Guest1Service _guest1Service;
        private LocationService _locationService;
        private AccommodationService _accommodationService;
        private AccommodationReservationService _accommodationReservationService;

        public OwnerMainView(Owner owner)
        {
            InitializeComponent();
            DataContext = this;

            Owner = owner;

            LoadFromFiles();
        }

        public void LoadFromFiles()
        {
            _ownerService = new OwnerService();
            _guest1Service = new Guest1Service();
            _locationService = new LocationService();
            _accommodationService = new AccommodationService();
            _accommodationReservationService = new AccommodationReservationService();

            _ownerService.Load();
            _locationService.Load();
            _accommodationService.Load();
            _guest1Service.Load();
            _accommodationReservationService.Load();

            _accommodationService.ConnectAccommodationsWithLocations(_locationService);

            _accommodationService.FillOwnerAccommodationList(Owner.Id, _ownerService);
        }

        private void btnAccommodations_Click(object sender, RoutedEventArgs e)
        {
            Window accommodationsView = new AccommodationsView(_accommodationService, _ownerService, Owner);
            accommodationsView.Show();
        }

    }
}
