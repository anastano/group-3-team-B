using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Observer;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Views.Guest2Views;
using System.Windows;
using SIMS_HCI_Project.Applications.Services;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace SIMS_HCI_Project.WPF.ViewModels.Guest2ViewModels
{
    public class TourRatingViewModel : INotifyPropertyChanged
    {
        #region PropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        #region Services
        private TourService _tourService;
        private TourReservationService _tourReservationService;
        private TourVoucherService _tourVoucherService;
        private GuestTourAttendanceService _guestTourAttendanceService;
        private TourRatingService _tourRatingService;
        #endregion
        #region Commands
        public RelayCommand Back { get; set; }
        public RelayCommand Rate { get; set; }
        #endregion
        public TourRatingView TourRatingView { get; set; }
        public TourReservation SelectedReservation { get; set; }
        public Guest2 Guest { get; set; }
        private ObservableCollection<TourReservation> _unratedReservations;
        public ObservableCollection<TourReservation> UnratedReservations
        {
            get { return _unratedReservations; }
            set
            {
                _unratedReservations = value;
                OnPropertyChanged();
            }
        }

        public TourRatingViewModel(TourRatingView tourRatingView, Guest2 guest2)
        {
            TourRatingView = tourRatingView;
            Guest = guest2;

            LoadFromFiles();
            InitCommands();

            UnratedReservations = new ObservableCollection<TourReservation>(_tourReservationService.GetUnratedReservations(Guest.Id, _guestTourAttendanceService, _tourRatingService));
        }

        public void LoadFromFiles()
        {
            _tourService = new TourService();
            _tourReservationService = new TourReservationService();
            _tourVoucherService = new TourVoucherService();
            _guestTourAttendanceService = new GuestTourAttendanceService();
            _tourRatingService = new TourRatingService();

            _tourService.ConnectLocations();
            _tourService.ConnectKeyPoints();
            _tourService.ConnectDepartureTimes();

            _tourReservationService.ConnectVouchers(_tourVoucherService);
            _tourReservationService.ConnectTourTimes(_tourService);
            _tourReservationService.ConnectAvailablePlaces(_tourService);

            _tourService.CheckAndUpdateStatus();
        }

        public void InitCommands()
        {
            Back = new RelayCommand(Executed_Back, CanExecute_Back);
            Rate = new RelayCommand(Executed_Rate, CanExecute_Rate);
        }

        #region Commands
        private void Executed_Back(object sender)
        {
            Window window = new Guest2MainView(Guest);
            window.Show();
            TourRatingView.Close();
        }
        public bool CanExecute_Back(object sender)
        {
            return true;
        }
        private void Executed_Rate(object sender)
        {
            Window window = new RateSelectedReservationView(Guest, SelectedReservation);
            window.Show();
            TourRatingView.Close();
        }
        public bool CanExecute_Rate(object sender)
        {
            return true;
        }
        #endregion
    }
}
