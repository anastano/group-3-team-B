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
        private string _guest1Id;

        private AccommodationReservationController _accommodationReservationController;
        //private Guest1Controller _guestController;

        public ObservableCollection<Accommodation> Reservations { get; set; }

        public AccommodationReservation SelectedReservation;
        public Guest1View(Guest1 guest)
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            Window win = new AccommodationSearchView();
            win.Show();
        }
        public void Update()
        {

        }
    }
}
