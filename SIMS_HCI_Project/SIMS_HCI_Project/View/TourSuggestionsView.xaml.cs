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
    /// Interaction logic for TourSuggestionsView.xaml
    /// </summary>
    public partial class TourSuggestionsView : Window
    {
        public Location Location { get; set; }
        public Guest2 Guest2 { get; set; }
        public Tour Tour { get; set; }
        public Tour SelectedTour { get; set; }
        public List<Tour> Tours { get; set; }

        private TourController _tourController = new TourController();
        

        public TourSuggestionsView(Location location, Guest2 guest)
        {
            InitializeComponent();
            Location = location;
            Guest2 = guest;

            _tourController.LoadConnections();

            Tours = new List<Tour>(_tourController.Search(Location.City, Location.Country));

            DataContext = this;
        }

        private void btnChooseTour_Click(object sender, RoutedEventArgs e)
        {
            Window window = new TourReservationView(SelectedTour, Guest2);
            window.Show();
            this.Close();
        }

        private void btnShowImages_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnReserve_Click(object sender, RoutedEventArgs e)
        {
            Window window = new TourReservationView(SelectedTour, Guest2);
            window.Show();
            this.Close();
        }
    }
}
