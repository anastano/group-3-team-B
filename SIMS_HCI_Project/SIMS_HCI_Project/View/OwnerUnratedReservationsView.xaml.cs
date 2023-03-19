using SIMS_HCI_Project.Controller;
using SIMS_HCI_Project.Model;
using SIMS_HCI_Project.Observer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SIMS_HCI_Project.View
{
    /// <summary>
    /// Interaction logic for OwnerReservationsView.xaml
    /// </summary>
    public partial class OwnerUnratedReservationsView : Window, IObserver
    {
        private string _ownerId;

        private OwnerController _ownerController;
        private OwnerGuestRatingController _ownerGuestRatingController;

        public ObservableCollection<AccommodationReservation> UnratedReservations { get; set; }

        public AccommodationReservation SelectedReservation { get; set; }

        public OwnerUnratedReservationsView(OwnerController ownerController, OwnerGuestRatingController ownerGuestRatingController, string ownerId)
        {
            InitializeComponent();
            DataContext = this;

            _ownerId = ownerId;

            _ownerController = ownerController;
            _ownerGuestRatingController = ownerGuestRatingController;

            UnratedReservations = new ObservableCollection<AccommodationReservation>(_ownerController.GetUnratedReservations(_ownerId));

            _ownerGuestRatingController.Subscribe(this);

        }

        public void Update()
        {
            UpdateUnratedReservations();
        }

        public void UpdateUnratedReservations()
        {
            UnratedReservations.Clear();
            foreach (AccommodationReservation reservation in _ownerController.GetUnratedReservations(_ownerId))
            {
                UnratedReservations.Add(reservation);
            }

        }

        private void btnRate_Click(object sender, RoutedEventArgs e)
        {
             Window ratingWindow = new GuestRatingView(_ownerGuestRatingController, SelectedReservation, _ownerId);
             ratingWindow.Show();

        }
    }
}
