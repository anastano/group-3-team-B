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
    /// Interaction logic for OwnerRescheduleRequestsView.xaml
    /// </summary>
    public partial class OwnerRescheduleRequestsView : Window, IObserver
    {
        public Owner Owner { get; set; }

        private RescheduleRequestController _requestController;
        private AccommodationReservationController _reservationController;
        private NotificationController _notificationController;

        public ObservableCollection<RescheduleRequest> Requests { get; set; }
        public RescheduleRequest SelectedRequest { get; set; }

        public OwnerRescheduleRequestsView(RescheduleRequestController requestController, NotificationController notificationController, AccommodationReservationController reservationController, Owner owner)
        {
            InitializeComponent();
            DataContext = this;

            Owner = owner;

            _requestController = requestController;
            _reservationController = reservationController;
            _notificationController = notificationController; 

            Requests = new ObservableCollection<RescheduleRequest>(_requestController.GetPendingRequestsByOwnerId(Owner.Id));

            _requestController.Subscribe(this);
            _reservationController.Subscribe(this);
        }

        private void btnAcceptDeclineRequest_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedRequest != null)
            {
                Window requestHandlerView = new RequestHandlerView(_requestController, _reservationController, _notificationController, SelectedRequest, Owner.Id);
                requestHandlerView.Show();
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public void Update()
        {
            UpdateRequests();
        }

        public void UpdateRequests()
        {
            Requests.Clear();
            foreach (RescheduleRequest request in _requestController.GetPendingRequestsByOwnerId(Owner.Id))
            {
                Requests.Add(request);
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.S))
                btnAcceptDeclineRequest_Click(sender, e);
            else if (Keyboard.IsKeyDown(Key.Escape))
                btnClose_Click(sender, e);
        }
    }
}
