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
    /// Interaction logic for OwnerView.xaml
    /// </summary>
    public partial class OwnerView : Window, IObserver
    {
        public Owner Owner { get; set; }

        private OwnerController _ownerController;
        private Guest1Controller _guestController;
        private AccommodationController _accommodationController;
        private LocationController _locationController;
        private AccommodationReservationController _reservationController;
        private OwnerGuestRatingController _ownerGuestRatingController;
        private RescheduleRequestController _requestController;
        private NotificationController _notificationController;

        public ObservableCollection<AccommodationReservation> Reservations { get; set; }
        public ObservableCollection<Notification> Notifications { get; set; }

        public OwnerView(Owner owner)
        {
            InitializeComponent();
            DataContext = this;

            Owner = owner;

            LoadFromFiles();            

            Reservations = new ObservableCollection<AccommodationReservation>(_reservationController.GetReservationsInProgress(Owner.Id));
            Notifications = new ObservableCollection<Notification>(_notificationController.GetUnreadById(Owner.Id));

            ShowNotifications();

            _ownerGuestRatingController.Subscribe(this);
        }

        void LoadFromFiles()
        {
            _ownerController = new OwnerController();
            _guestController = new Guest1Controller();
            _accommodationController = new AccommodationController();           
            _locationController = new LocationController();
            _reservationController = new AccommodationReservationController();
            _ownerGuestRatingController = new OwnerGuestRatingController();
            _requestController = new RescheduleRequestController();
            _notificationController = new NotificationController();

            _ownerController.Load();
            _guestController.Load();
            _accommodationController.Load();          
            _reservationController.Load();
            _ownerGuestRatingController.Load();
            _requestController.Load();
            _notificationController.Load();

            _accommodationController.ConnectAccommodationsWithLocations(_locationController);
            _reservationController.ConnectAccommodationsWithReservations(_accommodationController);
            _reservationController.ConnectGuestsWithReservations(_guestController);
            _requestController.ConnectRequestsWithReservations(_reservationController);

            _accommodationController.FillOwnerAccommodationList(Owner.Id);
            _reservationController.FillOwnerReservationList(Owner.Id);

        }

        private void ShowNotifications()
        {
            int unratedGuestsNumber = _ownerGuestRatingController.GetUnratedReservations(Owner.Id).Count;
            txtUnratedGuestsNotifications.Visibility = unratedGuestsNumber  != 0 ? Visibility.Visible : Visibility.Collapsed;

            int guestRequestsNumber = _requestController.GetPendingRequestsByOwnerId(Owner.Id).Count;
            txtGuestsRequestsNotifications.Visibility = guestRequestsNumber  != 0 ? Visibility.Visible : Visibility.Collapsed;

            int otherNotificationsNumber = Notifications.Count;
            lvNotifications.Visibility = otherNotificationsNumber != 0 ? Visibility.Visible : Visibility.Collapsed;

        }

        private void btnAccommodations_Click(object sender, RoutedEventArgs e)
        {
            Window accommodationsView = new OwnerAccommodationsView(_accommodationController, _ownerController, Owner);
            accommodationsView.Show();
        }

        private void btnReservations_Click(object sender, RoutedEventArgs e)
        {
            Window reservationsView = new OwnerReservationsView(_reservationController, _ownerController, Owner);
            reservationsView.Show();
        }

        private void btnGuestRequests_Click(object sender, RoutedEventArgs e)
        {
            Window requestsView = new OwnerRescheduleRequestsView(_requestController, _notificationController, _reservationController, Owner);
            requestsView.Show();
        }


        private void btnRateGuests_Click(object sender, RoutedEventArgs e)
        {
            Window reservationsView = new OwnerUnratedReservationsView(_ownerController, _ownerGuestRatingController, Owner.Id);
            reservationsView.Show();
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult confirmationResult = ConfirmLogout();
            if (confirmationResult == MessageBoxResult.Yes)
            {
                _notificationController.MarkAsRead(Owner.Id);
                
                Close();

            }         
        }

        private MessageBoxResult ConfirmLogout()
        {

            string sMessageBoxText = $"Are you sure you want to log out?";
            string sCaption = "Confirmation";

            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxImage icnMessageBox = MessageBoxImage.Question;

            MessageBoxResult result = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
            return result;
        }

        public void Update()
        {
            ShowNotifications();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.D1))
                btnAccommodations_Click(sender, e);
            else if (Keyboard.IsKeyDown(Key.D2))
                btnReservations_Click(sender, e);
            else if (Keyboard.IsKeyDown(Key.D4))
                btnGuestRequests_Click(sender, e);
            else if (Keyboard.IsKeyDown(Key.D6))
                btnRateGuests_Click(sender, e);
            else if (Keyboard.IsKeyDown(Key.Escape))
                btnLogout_Click(sender, e);
        }
    }
}
