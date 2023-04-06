using SIMS_HCI_Project.Controller;
using SIMS_HCI_Project.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
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
    public partial class TourReservationView : Window, INotifyPropertyChanged, IDataErrorInfo
    {
        private TourController _tourController = new TourController();
        private TourTimeController _tourTimeController = new TourTimeController();
        private TourReservationController _tourReservationController = new TourReservationController();
        private TourVoucherController _tourVoucherController = new TourVoucherController();

        public TourVoucher TourVoucher { get; set; }
        public TourTime TourTime { get; set; }

        public Tour Tour { get; set; }
        public Guest2 Guest2 { get; set; }
        private TourTime _selectedTourTime;
        public TourTime SelectedTourTime
        {
            get { return _selectedTourTime; }
            set
            {
                _selectedTourTime = value;
                OnPropertyChanged();
            }
        }
        private TourVoucher _selectedTourVoucher;
        public TourVoucher SelectedVoucher 
        {
            get { return _selectedTourVoucher; }
            set { 
                _selectedTourVoucher = value;
                OnPropertyChanged();
                } 
        }

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
        private ObservableCollection<TourVoucher> _vouchers { get; set; }
        public ObservableCollection<TourVoucher > Vouchers
        {
            get
            {
                return _vouchers;
            }
            set
            {
                if (value != _vouchers)
                {
                    _vouchers = value;
                    OnPropertyChanged();
                }
            }
        }
       

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TourReservationView(Tour tour, Guest2 guest)
        {
            InitializeComponent();
            
            Tour = tour;
            Guest2 = guest;
            _tourController.LoadConnections();
            _tourTimeController.ConnectAvailablePlaces();
            SelectedTourTime = Tour.DepartureTimes[0];

            Reservations = new ObservableCollection<TourReservation>(_tourReservationController.GetAll());
            //Guest2.Vouchers = new ObservableCollection<TourVoucher>(_tourVoucherController.GetValidVouchersByGuestId(guest.Id));
            Vouchers = new ObservableCollection<TourVoucher>(_tourVoucherController.GetValidVouchersByGuestId(guest.Id));

            Closing += TourReservationView_Closing;

            this.DataContext = this;
        }

        

        private void btnConfirmReservation_Click(object sender, RoutedEventArgs e) 
        {
            int requestedPartySize;
            bool isValidrequestedPartySize = int.TryParse(txtRequestedPartySize.Text, out requestedPartySize);

            TourTime = _tourTimeController.FindById(SelectedTourTime.Id);
            TourVoucher = _tourVoucherController.FindById(SelectedVoucher.Id);
            TourReservation tourReservation = new TourReservation(SelectedTourTime.Id, Guest2.Id, requestedPartySize);

            if (IsValid)
            {
                if (TourTime.Available == 0)
                {
                    MessageBox.Show("The tour is fully booked. Choose a different deparature time or view suggestions in the same location by clicking the SHOW SUGGESTIONS button.");
                }
                else
                {
                    if (requestedPartySize <= TourTime.Available)
                    {
                        _tourVoucherController.UseVoucher(TourVoucher);
                        Reservations.Add(tourReservation);
                        _tourReservationController.Add(tourReservation);
                        _tourTimeController.ReduceAvailablePlaces(TourTime, requestedPartySize);

                        MessageBox.Show("Reservation successfully completed.");

                    }
                    else if (requestedPartySize > TourTime.Available)
                    {
                        MessageBox.Show("The number of people entered exceeds the number of available places. Change the entry or deparature time. You can also view the suggestions in the same location by clicking the SHOW SUGGESTIONS button.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Field is not validly filled in");
            }
            _reservations = Reservations;
            _vouchers = Vouchers;
            OnPropertyChanged();
        }

        private void btnShowSuggestions_Click(object sender, RoutedEventArgs e)
        {
            Window window = new TourSuggestionsView(Tour.Location, Guest2);
            window.Show();
            this.Close();
        }

        private void TourReservationView_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Window guest2View = new Guest2View(Guest2);
            guest2View.Show();
        }
        

        private string _requestedPartySize;
        public string RequestedPartySize
        {
            get => _requestedPartySize;
            set
            {
                if (value != _requestedPartySize)
                {
                    _requestedPartySize = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Error => throw new NotImplementedException();
        private Regex _PartySizeRegex = new Regex("[1-9][0-9]*");
        public string this[string columnName]
        {
            get
            {
                string result = null;
                if (columnName == "RequestedPartySize")
                {
                    Match match = _PartySizeRegex.Match(txtRequestedPartySize.Text.ToString());
                    if (RequestedPartySize == null)
                        result = "Number of guests is required";
                    else if (!match.Success)
                        result = "Enter number which is greater than 0";
                 }
                return result;
            }
        }
        private readonly string[] _validatedProperties = { "RequestedPartySize" };
        public bool IsValid
        {
            get
            {
                foreach (var property in _validatedProperties)
                {
                    if (this[property] != null)
                        return false;
                }

                return true;
            }
        }
        
    }
}
