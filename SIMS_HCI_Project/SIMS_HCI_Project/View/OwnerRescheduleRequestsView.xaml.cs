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
        private OwnerController _ownerController;

        public ObservableCollection<RescheduleRequest> Requests { get; set; }
        public RescheduleRequest SelectedRequest { get; set; }

        public OwnerRescheduleRequestsView(RescheduleRequestController requestController, OwnerController ownerController, AccommodationReservationController reservationController, Owner owner)
        {
            InitializeComponent();
            DataContext = this;

            Owner = owner;

            _requestController = requestController;
            _reservationController = reservationController;
            _ownerController = ownerController; 

            Requests = new ObservableCollection<RescheduleRequest>(_requestController.GetAll());

            _requestController.Subscribe(this);
        }

        private void btnAcceptDeclineRequest_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedRequest != null)
            {
                Window requestHandlerView = new RequestHandlerView(_requestController, _reservationController, SelectedRequest, Owner.Id);
                requestHandlerView.Show();
            }
        }

        public void Update()
        {
        }
    }
}
