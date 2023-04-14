using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Observer;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Views;
using SIMS_HCI_Project.WPF.Views.Guest2Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SIMS_HCI_Project.Applications.Services;


namespace SIMS_HCI_Project.WPF.ViewModels.Guest2ViewModels
{
    public class Guest2MainViewModel : IObserver
    {
        private TourReservationService _tourReservationService;
        private TourService _tourService;
        private TourTimeService _tourTimeService;
        private LocationService _locationService;
        private TourKeyPointService _tourKeyPointService;
        private TourVoucherService _tourVoucherService;

        public RelayCommand SearchAndReserve { get; set; }
        public RelayCommand ShowImages { get; set; }
        public RelayCommand CancelReservation { get; set; }

        public Guest2 Guest { get; set; }
        public TourTime TourTime { get; set; }
        public TourReservation SelectedTourReservation { get; set; }
        public Tour Tour { get; set; }

        public Guest2MainView Guest2MainView { get; set; }

        public Guest2MainViewModel(Guest2MainView guest2MainView, Guest2 guest)
        {
            Guest2MainView = guest2MainView;
            Guest = guest;
            LoadFromFiles();
            InitCommands();

            Guest.Reservations = new ObservableCollection<TourReservation>(_tourReservationService.GetAllByGuestId(guest.Id));
            Guest.Vouchers = new ObservableCollection<TourVoucher>(_tourVoucherService.GetValidVouchersByGuestId(guest.Id));

            _tourReservationService.Subscribe(this);
            _tourVoucherService.Subscribe(this);
        }

        private void LoadFromFiles()
        {
            _tourTimeService = new TourTimeService();
            _tourVoucherService = new TourVoucherService();
            _tourReservationService = new TourReservationService();
            _tourService = new TourService();
            _locationService = new LocationService();
            _tourKeyPointService = new TourKeyPointService();


            _tourService.ConnectLocations(_locationService);
            _tourService.ConnectKeyPoints(_tourKeyPointService);
            _tourService.ConnectDepartureTimes(_tourTimeService);

            //_tourTimeService.
            _tourReservationService.ConnectTourTimes(_tourTimeService);
            _tourReservationService.ConnectVouchers(_tourVoucherService);
            _tourReservationService.ConnectAvailablePlaces(_tourTimeService);
        }

        public void InitCommands()
        {
            SearchAndReserve = new RelayCommand(Executed_SearchAndReserve, CanExecute_SearchAndReserve);
            //ShowImages = new RelayCommand(Executed_ShowImages, CanExecute_ShowImages);
            //CancelReservation = new RelayCommand(Executed_CancelReservation, CanExecute_CancelReservation);
        }

        public void Executed_SearchAndReserve(object obj)
        {
            Window win = new TourSearchView(Guest);
            win.Show();
            Guest2MainView.Close();
        }
        public bool CanExecute_SearchAndReserve(object obj)
        {
            return true;
        }

        public void ConnectTourByReservation() //TODO: move
        {
            TourTime = _tourTimeService.GetById(SelectedTourReservation.TourTimeId);
            Tour = _tourService.FindById(TourTime.TourId);
        }
        public void Executed_ShowImages(object obj)
        {
           // ConnectTourByReservation();
           // Window window = new TourImagesView(_tourService, Tour);
           // window.Show();
        }
        public bool CanExecute_ShowImages(object obj)
        {
            return true;
        }

        public void Executed_CancelReservation(object obj)
        {
            //TODO later
        }
        public bool CanExecute_CancelReservation(object obj)
        {
            return true;
        }

        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}
