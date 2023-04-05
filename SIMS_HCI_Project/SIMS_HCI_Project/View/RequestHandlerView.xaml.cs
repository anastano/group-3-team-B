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
        public ObservableCollection<AccommodationReservation> Reservations { get; set; }

        public RequestHandlerView(RescheduleRequestController requestController, AccommodationReservationController reservationController, RescheduleRequest request, int ownerId)
        {
            InitializeComponent();
            DataContext = this;

            Request = request;

            _requestController = requestController;
            _reservationController = reservationController;

            Reservations = new ObservableCollection<AccommodationReservation>(_reservationController.GetAll()); //CHANGE THIS, FIND BY ACCOMMODATION AND DATE

        }
    }
}
