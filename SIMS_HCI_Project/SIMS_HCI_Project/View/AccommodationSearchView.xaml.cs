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
    /// Interaction logic for AccommodationSearchView.xaml
    /// </summary>
    public partial class AccommodationSearchView : Window, IObserver
    {
        private readonly AccommodationController _accommodationController;

        private readonly LocationController _locationController;

        public ObservableCollection<Accommodation> Accommodations { get; set; }

        public ObservableCollection<Location> Locations { get; set; }
        public Accommodation SelectedAccommodation { get; set; }

        public AccommodationSearchView()
        {
            InitializeComponent();
            DataContext = this;

            _accommodationController = new AccommodationController();
            _locationController = new LocationController();
            _accommodationController.LoadList();

            Accommodations = new ObservableCollection<Accommodation>(_accommodationController.GetList());

            _accommodationController.ConnectAccommodationsWithLocations(_locationController);
   
        }

        //Accommodation search
        private void  SearchAccommodation(object sender, EventArgs e)
        {
            int maxGuests;
            bool isValidMaxGuests = int.TryParse(txtGuestNumber.Text, out maxGuests);
            int reservationDays;
            bool isValidReservationDays = int.TryParse(txtReservationDays.Text, out reservationDays);
            ComboBoxItem selectedItem = comboboxType.SelectedItem as ComboBoxItem;
            string selectedItemContent=null;
            if (selectedItem != null)
            {
                selectedItemContent = selectedItem.Content.ToString();
            }

            var filtered = from _accommodation in Accommodations
                           where (string.IsNullOrEmpty(txtName.Text) || _accommodation.Name.ToLower().Contains(txtName.Text.ToLower()))
                           && (string.IsNullOrEmpty(txtCountry.Text) || _accommodation.Location.Country.ToLower().Contains(txtCountry.Text))
                           && (string.IsNullOrEmpty(txtCity.Text) || _accommodation.Location.City.ToLower().Contains(txtCity.Text))
                           && (string.IsNullOrEmpty(selectedItemContent) || Accommodation.ConvertAccommodationTypeToString(_accommodation.Type).Equals(selectedItem.Content.ToString()))
                           && (!isValidMaxGuests || maxGuests <= _accommodation.MaxGuests)
                           && (!isValidReservationDays || reservationDays >= _accommodation.MinimumReservationDays)
                           select _accommodation;
           

            DataGridAccommodation.ItemsSource =  filtered.ToList();
        }
            public void Update()
        {
            //throw new NotImplementedException();
        }
    }
}
