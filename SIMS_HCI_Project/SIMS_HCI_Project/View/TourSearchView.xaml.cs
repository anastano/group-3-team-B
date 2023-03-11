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
    /// Interaction logic for TourSearchView.xaml
    /// </summary>
    public partial class TourSearchView : Window, IObserver
    {

        private readonly TourController _tourController;
        private readonly LocationController _locationController;

        public ObservableCollection<Tour> Tours { get; set; }
        public ObservableCollection<Location> Locations { get; set; }
        public Tour SelectedTour { get; set; }

        public TourSearchView()
        {
            InitializeComponent();
            DataContext = this;
            _tourController = new TourController();
            _locationController = new LocationController();
            _tourController.Load();

            _tourController.ConnectToursLocations(_locationController);

            Tours = new ObservableCollection<Tour>(_tourController.GetAll());
        }

        private void btnReserve_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnShowImages_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            List<Tour> result = new List<Tour>();

            int guestsNum;
            bool isValidGuestsNum = int.TryParse(txtGuestNumber.Text, out guestsNum);
            int duration; //Assuming that the user enters the maximum duration of the tour. Should discuss whether this is a good way, if so: the name should be changed and a field for the minimum duration added.
            bool isValidDuration = int.TryParse(txtDuration.Text, out duration);

            if (!isValidGuestsNum)
            {
                guestsNum = 0;
            }

            if (!isValidDuration)
            {
                duration = 0;
            }

            result = _tourController.Search(txtCountry.Text, txtCity.Text, duration, txtLanguage.Text, guestsNum);

            dgTours.ItemsSource = result;
        }

        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}
