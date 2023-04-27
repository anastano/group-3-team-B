using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Observer;
using SIMS_HCI_Project.WPF.Commands;
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
        #region Services
        private TourService _tourService;
        private TourReservationService _tourReservationService;
        private TourVoucherService _tourVoucherService;
        #endregion
        #region Commands
        public RelayCommand ShowSuggestions { get; set; }
        public RelayCommand ConfirmReservation { get; set; }
        #endregion

        public TourReservationView TourReservationView { get; set; }
        public TourVoucher TourVoucher { get; set; }
        public TourTime TourTime { get; set; }
        public Tour Tour { get; set; }
        public Guest2 Guest2 { get; set; }
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
        
        #region PropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        public TourReservationViewModel(Tour tour, Guest2 guest, TourReservationView tourReservationView)
        {

            TourReservationView = tourReservationView;
            Tour = tour;
            Guest2 = guest;

            LoadFromFiles();
            InitCommands();

            SelectedTourTime = Tour.DepartureTimes[0];

            Reservations = new ObservableCollection<TourReservation>(_tourReservationService.GetAll());
            Vouchers = new ObservableCollection<TourVoucher>(_tourVoucherService.GetValidVouchersByGuestId(guest.Id));
        }

        public void LoadFromFiles()
        {
            _tourService = new TourService();
            _tourReservationService = new TourReservationService();
            _tourVoucherService = new TourVoucherService();
        }

        public void InitCommands()
        {
            ConfirmReservation = new RelayCommand(ExecutedConfirmReservation, CanExecuteConfirmReservation);
        }
        #region Commands
        private void ExecutedConfirmReservation(object sender) 
        {
            Reserve();
        }
        public bool CanExecuteConfirmReservation(object sender)
        {
            return true;
        }

        
        #endregion

        private void Reserve() 
        {

            TourTime = _tourService.GetTourInstance(SelectedTourTime.Id);
            if (IsBooked(TourTime))
            {
                MessageBox.Show("The tour is fully booked. Choose a different deparature time or view suggestions in the same location by clicking the SHOW SUGGESTIONS button.");
            }
            else if (IsAvailableExceeded(TourTime))
            {
                MessageBox.Show("The number of people entered exceeds the number of available places. Change the entry or deparature time. You can also view the suggestions in the same location by clicking the SHOW SUGGESTIONS button.");
            }   
            else
            {
                TourReservation tourReservation = MakeReservation();
                Reservations.Add(tourReservation);
                _tourReservationService.Add(tourReservation);
                _tourReservationService.ReduceAvailablePlaces(_tourService,TourTime, RequestedPartySize);

                ConfirmationMessage();
            }
            _reservations = Reservations;
            _vouchers = Vouchers;
            OnPropertyChanged();
        }
        #region ReserveFunctions
        private TourReservation MakeReservation()
        {
            TourReservation tourReservation = new TourReservation();

            if (SelectedVoucher != null)
            {
                TourVoucher = _tourVoucherService.GetById(SelectedVoucher.Id);
                _tourVoucherService.UseVoucher(TourVoucher);
                tourReservation = new TourReservation(SelectedTourTime.Id, Guest2.Id, RequestedPartySize, TourVoucher.Id);
            }
            else
            {
                tourReservation = new TourReservation(SelectedTourTime.Id, Guest2.Id, RequestedPartySize, -1);
            }
            return tourReservation;
        }
        private bool IsBooked(TourTime tour)
        {
            return tour.Available == 0 ? true : false;
        }
        private bool IsAvailableExceeded(TourTime tour)
        {
            return RequestedPartySize > tour.Available ? true : false;
        }
        private void ConfirmationMessage()
        {
            MessageBox.Show("Reservation successfully completed. You can see it in the list of your reservations, on your profile.");
            MessageBoxButton messageBoxButton = MessageBoxButton.OK;
            
        }
        #endregion
    }
}
