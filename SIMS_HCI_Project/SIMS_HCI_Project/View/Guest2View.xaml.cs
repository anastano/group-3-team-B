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
    /// Interaction logic for Guest2View.xaml
    /// </summary>
    public partial class Guest2View : Window
    {
        public Guest2 Guest { get; set; }
        public TourTime TourTime { get; set; }
        public TourReservation SelectedTourReservation { get; set; }
        public Tour Tour { get; set; }
        private TourReservationController _tourReservationController= new TourReservationController();
        private TourController _tourController = new TourController();
        private TourTimeController _tourTimeController = new TourTimeController();
        private TourVoucherController _tourVoucherController = new TourVoucherController();
        public List<TourReservation> TourReservations { get; set; } //ne treba?
        public Guest2View(Guest2 guest)
        {
            InitializeComponent();
            Guest = guest;
            

            _tourController.LoadConnections();
            _tourTimeController.ConnectAvailablePlaces();
            _tourReservationController.LoadConnections();
            
            Guest.Reservations = new ObservableCollection<TourReservation> (_tourReservationController.GetAllByGuestId(guest.Id));
            Guest.Vouchers = new ObservableCollection<TourVoucher> (_tourVoucherController.GetValidVouchersByGuestId(guest.Id));

            this.DataContext = this;
        }

        private void btnSearchReserve_Click(object sender, RoutedEventArgs e)
        {
            Window win = new TourSearchView(Guest);
            win.Show();
            this.Close();
        }

        private void btnShowImages_Click(object sender, RoutedEventArgs e)
        {
            ConnectTourByReservation();
            Window window = new TourImagesView(_tourController, Tour);
            window.Show();
        }

        public void ConnectTourByReservation()
        {
            TourTime = _tourTimeController.FindById(SelectedTourReservation.TourTimeId);
            Tour = _tourController.FindById(TourTime.TourId);
        }

        private void btnCancelReservation_Click(object sender, RoutedEventArgs e)
        {
            //TODO: add reservation cancellation option
            //status change in CANCELLED and available -= partysize and remove from list OR change loadinf method for MyReservations to show only GOING, and to not remove from the list. Second option is better
        }
    }

    
}
