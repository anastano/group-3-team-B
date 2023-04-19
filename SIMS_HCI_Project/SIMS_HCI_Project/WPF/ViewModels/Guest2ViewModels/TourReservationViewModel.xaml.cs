using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Observer;
using SIMS_HCI_Project.WPF.Commands;
//using SIMS_HCI_Project.WPF.Views;
using SIMS_HCI_Project.WPF.Views.Guest2Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SIMS_HCI_Project.Applications.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace SIMS_HCI_Project.WPF.ViewModels.Guest2ViewModels
{
   
    public partial class TourReservationViewModel : INotifyPropertyChanged
    {
        private TourService _tourService;
        private TourReservationService _tourReservationService;
        private TourVoucherService _tourVoucherService;
        private LocationService _locationService;
        private GuestTourAttendanceService _tourAttendanceService;

        public RelayCommand ShowSuggestions { get; set; }
        public RelayCommand ConfirmReservation { get; set; }

        public TourReservationView TourReservationView { get; set; }
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

        public TourReservationViewModel(TourReservationView tourReservationView, Tour tour, Guest2 guest)
        {

            TourReservationView = tourReservationView;
            Tour = tour;
            Guest2 = guest;

            LoadFromFiles();
            InitCommands();

            SelectedTourTime = Tour.DepartureTimes[0];

            Reservations = new ObservableCollection<TourReservation>(_tourReservationService.GetAll());
            Vouchers = new ObservableCollection<TourVoucher>(_tourVoucherService.GetValidVouchersByGuestId(guest.Id));

           // Closing += TourReservationView_Closing;

           // this.DataContext = this;
        }

        public void LoadFromFiles()
        {
            _tourService = new TourService();
            _tourReservationService = new TourReservationService();
            _tourVoucherService = new TourVoucherService();
            _locationService = new LocationService();
            _tourAttendanceService = new GuestTourAttendanceService();
        }

        public void InitCommands()
        {
            ConfirmReservation = new RelayCommand(Executed_ConfirmReservation, CanExecute_ConfirmReservation);
        }

        private void Executed_ConfirmReservation(object sender) 
        {
            Reserve();
        }
        public bool CanExecute_ConfirmReservation(object sender)
        {
            return true;
        }

        /*private void btnShowSuggestions_Click(object sender, RoutedEventArgs e)
        {
           // Window window = new TourSuggestionsView(Tour.Location, Guest2);
            //window.Show();
           // this.Close();
        }

         private void TourReservationView_Closing(object sender, System.ComponentModel.CancelEventArgs e)
         {
             Window guest2View = new Guest2View(Guest2);
             guest2View.Show();
         }
         */

       

        private void Reserve() //TODO: refactor
        {
           // int requestedPartySize;
            //bool isValidrequestedPartySize = int.TryParse(txtRequestedPartySize.Text, out requestedPartySize);

            TourTime = _tourService.GetTourInstance(SelectedTourTime.Id);
            TourReservation tourReservation = new TourReservation();
            if (SelectedVoucher != null)
            {
                TourVoucher = _tourVoucherService.GetById(SelectedVoucher.Id);
                tourReservation = new TourReservation(SelectedTourTime.Id, Guest2.Id, RequestedPartySize, TourVoucher.Id);
            }
            else
            {
                tourReservation = new TourReservation(SelectedTourTime.Id, Guest2.Id, RequestedPartySize, -1);
            }

            //if (IsValid)
            //{
                if (TourTime.Available == 0)
                {
                    MessageBox.Show("The tour is fully booked. Choose a different deparature time or view suggestions in the same location by clicking the SHOW SUGGESTIONS button.");
                }
                else
                {
                    if (RequestedPartySize <= TourTime.Available)
                    {
                        if (SelectedVoucher != null)
                        {
                            _tourVoucherService.UseVoucher(TourVoucher);
                        }
                        Reservations.Add(tourReservation);
                        _tourReservationService.Add(tourReservation);
                        _tourReservationService.ReduceAvailablePlaces(_tourService,TourTime, RequestedPartySize);

                        MessageBox.Show("Reservation successfully completed.");
                        MessageBoxButton messageBoxButton = MessageBoxButton.OK;
                        if (messageBoxButton == MessageBoxButton.OK)
                        {
                            TourReservationView.Close(); 
                            Window win = new Guest2MainView(Guest2); 
                            win.Show();
                        }

                }
                else if (RequestedPartySize > TourTime.Available)
                    {
                        MessageBox.Show("The number of people entered exceeds the number of available places. Change the entry or deparature time. You can also view the suggestions in the same location by clicking the SHOW SUGGESTIONS button.");
                    }
                }
            
            
            /*else
            {
                MessageBox.Show("Field is not validly filled in");
            }*/
            _reservations = Reservations;
            _vouchers = Vouchers;
            OnPropertyChanged();
        }

        private int _requestedPartySize;
        public int RequestedPartySize
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

 /*       public string Error => throw new NotImplementedException();
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
        */
    }
}
