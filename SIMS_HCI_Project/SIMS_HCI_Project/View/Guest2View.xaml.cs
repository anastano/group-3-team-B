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
        private TourReservationController _tourReservationController= new TourReservationController();
        public Guest2View(Guest2 guest)
        {
            InitializeComponent();
            Guest = guest;
            Guest.Reservations = new ObservableCollection<TourReservation> (_tourReservationController.GetAllByGuestId(guest.Id)); 

            DataContext = this;
        }


        //TODO later: enable tour cancellation

        private void btnSearchReserve_Click(object sender, RoutedEventArgs e)
        {
            Window win = new TourSearchView(Guest);
            win.Show();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e) 
        {
            /// treba li ovo uopste?????
        }

    }

    
}
