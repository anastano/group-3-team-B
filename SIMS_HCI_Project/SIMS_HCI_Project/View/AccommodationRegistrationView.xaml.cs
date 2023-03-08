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
    /// Interaction logic for AccommodationRegistrationView.xaml
    /// </summary>
    public partial class AccommodationRegistrationView : Window
    {
        private string _ownerId;
        private AccommodationController _accommodationController;
        private LocationController _locationController = new LocationController(); //should this be passed as parameter???
        public Accommodation temp { get; set; }

        public AccommodationRegistrationView(AccommodationController accommodationController, string ownerId)
        {
            InitializeComponent();
            DataContext = this;

            _ownerId = ownerId;

            _accommodationController = accommodationController;
            temp = new Accommodation();   //check if it is needed to initialize default values
            
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            //non binded accommodation properties handled in this method: Id, OwnerId, LocationId, Location

            temp.Id = _accommodationController.GenerateId();
            temp.OwnerId = _ownerId;

            string locationCountry = txtLocationCountry.Text.ToString();
            string locationCity = txtLocationCity.Text.ToString();

            Location tempLocation = new Location(locationCity, locationCountry);
            Location location = _locationController.Save(tempLocation);
            temp.Location = location;
            temp.LocationId = location.Id;

            _accommodationController.Add(temp);
            Close();
        }
    }
}
