using SIMS_HCI_Project.Controller;
using SIMS_HCI_Project.Model;
using SIMS_HCI_Project.Observer;
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
    /// Interaction logic for Gues1View.xaml
    /// </summary>
    public partial class Guest1View : Window, IObserver
    {
        public Guest1 Guest { get; set; }
        private AccommodationController _accommodationController;
        private AccommodationReservationController _accommodationReservationController;
        public ObservableCollection<AccommodationReservation> UpcomingReservations { get; set; }
        public ObservableCollection<AccommodationReservation> CompletedReservations { get; set; }
        public AccommodationReservation SelectedReservation;
        public ICommand MyCommand { get; }
        public Guest1View(Guest1 guest)
        {
            InitializeComponent();
            this.DataContext = this;
            _accommodationController = new AccommodationController();
            _accommodationController.Load();
            _accommodationReservationController = new AccommodationReservationController();
            _accommodationReservationController.Load();
            _accommodationReservationController.ConnectAccommodationsWithReservations(_accommodationController);
            _accommodationReservationController.Subscribe(this);

            Guest = guest;
            Guest.Reservations = new ObservableCollection<AccommodationReservation>(_accommodationReservationController.GetAllByGuestId(guest.Id));
;           UpcomingReservations = new ObservableCollection<AccommodationReservation>(_accommodationReservationController.GetAllByStatusAndGuestId(guest.Id, ReservationStatus.RESERVED));
;           CompletedReservations = new ObservableCollection<AccommodationReservation>(_accommodationReservationController.GetAllByStatusAndGuestId(guest.Id, ReservationStatus.COMPLETED));
        }
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            Window win = new AccommodationSearchView(_accommodationReservationController, Guest);
            win.Show();
        }
        private void btnCancelation_Click(object sender, RoutedEventArgs e)
        {
            
        }
        public bool CanExecuteMyCommand()
        {
            // Add your logic here to determine whether the button should be enabled
            return true;
        }
        public void Update()
        {
            CompletedReservations.Clear();
            foreach (var completedReservation in _accommodationReservationController.GetAllByStatusAndGuestId(Guest.Id, ReservationStatus.COMPLETED))
            {
                CompletedReservations.Add(completedReservation);
            }
            UpcomingReservations.Clear();
            foreach (var upcomingReservation in _accommodationReservationController.GetAllByStatusAndGuestId(Guest.Id, ReservationStatus.RESERVED))
            {
                UpcomingReservations.Add(upcomingReservation);
            }
        }
    }
}
