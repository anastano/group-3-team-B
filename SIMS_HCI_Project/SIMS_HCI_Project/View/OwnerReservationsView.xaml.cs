using SIMS_HCI_Project.Controller;
using SIMS_HCI_Project.Model;
using SIMS_HCI_Project.Observer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.DirectoryServices;
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
using System.Xml.Linq;

namespace SIMS_HCI_Project.View
{
    /// <summary>
    /// Interaction logic for OwnerReservationsView.xaml
    /// </summary>
    public partial class OwnerReservationsView : Window, IObserver
    {
        public Owner Owner { get; set; }

        private AccommodationReservationController _reservationController;
        private OwnerController _ownerController;

        public ObservableCollection<AccommodationReservation> Reservations { get; set; }

        public OwnerReservationsView(AccommodationReservationController reservationController, OwnerController ownerController, Owner owner)
        {
            InitializeComponent();
            DataContext = this;

            Owner = owner;

            _reservationController = reservationController;
            _ownerController = ownerController;

            Reservations = new ObservableCollection<AccommodationReservation>(_ownerController.FindById(Owner.Id).Reservations);

            _reservationController.Subscribe(this);
        }

        public void Update()
        {
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            List<AccommodationReservation> searchResult = _reservationController.Search(txtAccommodationName.Text, txtGuestName.Text, Owner.Id);
            dgReservations.ItemsSource = searchResult;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
