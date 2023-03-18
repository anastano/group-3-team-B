using SIMS_HCI_Project.Controller;
using SIMS_HCI_Project.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for TourReservationView.xaml
    /// </summary>
    public partial class TourReservationView : Window, INotifyPropertyChanged        
    {
        private TourController _tourController = new TourController();
        private TourTimeController _tourTimeController = new TourTimeController();
        private TourReservationController _tourReservationController = new TourReservationController();
        public Tour Tour { get; set; }
        public Guest2 Guest2 { get; set; }
        public TourTime SelectedTourTime { get; set; }

        private ObservableCollection<TourReservation> _reservations { get; set; }
        public ObservableCollection<TourReservation> Reservations 
        { 
            get
            {
                return _reservations;
            }
            set
            {
                if (value != _reservations)
                {
                    _reservations = value;
                    OnPropertyChanged();
                }
            }
        }


        public TourReservationView(Tour tour, Guest2 guest)
        {
            InitializeComponent();
            
            Tour = tour;
            Guest2 = guest;
            //_tourController = new TourController();
            _tourController.ConnectDepartureTimes();
            _tourController.ConnectToursLocations();
            _tourController.ConnectKeyPoints();

            Reservations = new ObservableCollection<TourReservation>(_tourReservationController.GetAll());

            this.DataContext = this;
        }

        

        private void btnConfirmReservation_Click(object sender, RoutedEventArgs e) //add IsEnabled for Confirm button, deparature time must be selected and RequiredPeople/PartySize
        {
            //TODO:  show message box if it is successfully reserved, or partially/fully reserved
            //      1. if successfull - add tour to myReservations, message that it is added to myReservations
            //      2. partially - show message: how many places are left
            //      3. fully reserved - show window with   

            int requestedPartySize;
            bool isValidrequestedPartySize = int.TryParse(txtRequestedPartySize.Text, out requestedPartySize);
            if (!isValidrequestedPartySize)
            {
                requestedPartySize = 0;
            }

            if(SelectedTourTime.Available == 0)
            {
                MessageBox.Show("The tour is fully booked. Choose a different deparature time or view suggestions in the same location by clicking the SHOW SUGGESTIONS button.");
            }
            else
            {
                if(requestedPartySize <= SelectedTourTime.Available)
                {
                    //                    result.Add(new AccommodationReservation(Accommodation.Id, Guest.Id, currentDate, nextDate, int.Parse(txtGuestNumber.Text), 0));
                    Reservations.Add(new TourReservation(SelectedTourTime.Id, Guest2.Id, requestedPartySize));
                    MessageBox.Show("Reservation successfully completed.");

                }
                else if (requestedPartySize > SelectedTourTime.Available)
                {
                    MessageBox.Show("The number of people entered exceeds the number of available places. Change the entry or deparature time. You can also view the suggestions in the same location by clicking the SHOW SUGGESTIONS button.");
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
