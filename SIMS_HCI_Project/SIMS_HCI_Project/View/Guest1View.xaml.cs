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

        private AccommodationReservationController _accommodationReservationController;
        public ObservableCollection<AccommodationReservation> Reservations { get; set; }

        public AccommodationReservation SelectedReservation;
        public Guest1View(Guest1 guest)
        {
            InitializeComponent();
            this.DataContext = this;
            _accommodationReservationController = new AccommodationReservationController();
            _accommodationReservationController.Load();

            Guest = guest;
            Guest.Reservations = new ObservableCollection<AccommodationReservation>(_accommodationReservationController.GetAllByGuestId(guest.Id));
;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            Window win = new AccommodationSearchView(Guest);
            win.Show();
        }
        public void Update()
        {

        }
    }
}
