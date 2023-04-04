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
        private NotificationController _notificationController;
        public ObservableCollection<AccommodationReservation> UpcomingReservations { get; set; }
        public ObservableCollection<AccommodationReservation> CompletedReservations { get; set; }
        public AccommodationReservation SelectedReservation { get; set; }
        public Guest1View(Guest1 guest)
        {
            InitializeComponent();
            this.DataContext = this;
            _accommodationController = new AccommodationController();
            _accommodationController.Load();
            _accommodationReservationController = new AccommodationReservationController();
            _accommodationReservationController.Load();
            _notificationController = new NotificationController();
            _notificationController.Load();
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
        private void btnCancellation_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = ConfirmCancellation();
            if (result == MessageBoxResult.Yes)
            {
                String Message = "Reservation for " + SelectedReservation.Accommodation.Name + " with id: " + SelectedReservation.Id + " has been cancelled";
                _notificationController.Add(new Notification(Message, SelectedReservation.Accommodation.OwnerId, false));
                _accommodationReservationController.EditStatus(SelectedReservation.Id, ReservationStatus.CANCELLED);  
            }
        }
        private MessageBoxResult ConfirmCancellation()
        {
            string sMessageBoxText = $"This reservation will be cancelled, are you sure?";
            string sCaption = "Cancellation Confirm";

            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

            MessageBoxResult result = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
            return result;
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
