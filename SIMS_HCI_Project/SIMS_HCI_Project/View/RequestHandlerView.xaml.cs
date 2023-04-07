using SIMS_HCI_Project.Controller;
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
    /// Interaction logic for RequestHandlerView.xaml
    /// </summary>
    public partial class RequestHandlerView : Window
    {
        public RescheduleRequest Request { get; set; }

        private RescheduleRequestController _requestController;
        private AccommodationReservationController _reservationController;
        private NotificationController _notificationController;
        public ObservableCollection<AccommodationReservation> Reservations { get; set; }

        public RequestHandlerView(RescheduleRequestController requestController, AccommodationReservationController reservationController, NotificationController notificationController, RescheduleRequest request, int ownerId)
        {
            InitializeComponent();
            DataContext = this;

            Request = request;

            _requestController = requestController;
            _reservationController = reservationController;
            _notificationController = notificationController;

            Reservations = new ObservableCollection<AccommodationReservation>(_reservationController.GetOverlappingReservations(request));

            ShowDataGrid();
        }

        public void ShowDataGrid() 
        {
            int overlappingReservations = _reservationController.GetOverlappingReservations(Request).Count;
            if (overlappingReservations != 0)
            {
                txtOverlappingReservations.Text = "There are reservations on those days: ";
            }
            else
            {
                dgReservations.Visibility= Visibility.Collapsed;
                txtOverlappingReservations.Text = "There are not any reservations on those days.";
            }

        }

        private void btnAcceptRequest_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult confirmationResult = ConfirmRequestAcceptance();

            if (confirmationResult == MessageBoxResult.Yes)
            {
                _reservationController.Reschedule(Request, _requestController, Reservations.ToList());

                String Message = "Request to reschedule the reservation for '" + Request.AccommodationReservation.Accommodation.Name + "' has been ACCEPTED";
                _notificationController.Add(new Notification(Message, Request.AccommodationReservation.GuestId, false));

                Close();
            }

        }

        private MessageBoxResult ConfirmRequestAcceptance()
        {

            string sMessageBoxText = $"Are you sure you want to accept this request?";
            string sCaption = "Confirmation";

            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxImage icnMessageBox = MessageBoxImage.Question;

            MessageBoxResult result = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
            return result;
        }

        private void btnDeclineRequest_Click(object sender, RoutedEventArgs e)
        {
            Window requestDenialView = new RequestDenialView(Request, _requestController, _notificationController, this);
            requestDenialView.Show();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.A))
                btnAcceptRequest_Click(sender, e);
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.D))
                btnDeclineRequest_Click(sender, e);
            else if (Keyboard.IsKeyDown(Key.Escape))
                btnCancel_Click(sender, e);
        }
    }
}
