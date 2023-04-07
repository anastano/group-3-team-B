using SIMS_HCI_Project.Controller;
using SIMS_HCI_Project.Model;
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
    /// Interaction logic for RequestDenialView.xaml
    /// </summary>
    public partial class RequestDenialView : Window
    {
        public RescheduleRequest Request { get; set; }

        private RescheduleRequestController _requestController;
        private NotificationController _notificationController;

        private RequestHandlerView _requestHandlerView;

        public RequestDenialView(RescheduleRequest request, RescheduleRequestController requestController, NotificationController notificationController, RequestHandlerView requestHandlerView)
        {
            InitializeComponent();
            DataContext = this;

            _requestController = requestController;
            _notificationController = notificationController;
            _requestHandlerView = requestHandlerView;

            Request= request;

        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            _requestController.EditStatus(Request, RescheduleRequestStatus.DENIED);

            String Message = "Request to reschedule the reservation for '" + Request.AccommodationReservation.Accommodation.Name + "' has been DENIED";
            _notificationController.Add(new Notification(Message, Request.AccommodationReservation.GuestId, false));

            _requestHandlerView.Close();
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.S))
                btnSubmit_Click(sender, e);
            else if (Keyboard.IsKeyDown(Key.Escape))
                btnCancel_Click(sender, e);
        }
    }
}
