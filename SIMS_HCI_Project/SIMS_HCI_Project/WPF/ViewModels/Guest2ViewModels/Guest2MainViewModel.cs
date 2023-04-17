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
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SIMS_HCI_Project.WPF.ViewModels.Guest2ViewModels // TODO: prikazuje samo indeks trenutne TT, a treba da prikaze naziv keyPointa
{
    public class Guest2MainViewModel : IObserver
    {
        private TourReservationService _tourReservationService;
        private TourService _tourService;
        private TourTimeService _tourTimeService;
        private LocationService _locationService;
        private TourKeyPointService _tourKeyPointService;
        private TourVoucherService _tourVoucherService;
        private GuestTourAttendanceService _guestTourAttendanceService;

        public RelayCommand SearchAndReserve { get; set; }
        public RelayCommand ShowImages { get; set; }
        public RelayCommand CancelReservation { get; set; }
        public RelayCommand RateVisitedTours { get; set; }
        public RelayCommand ConfirmAttendance { get; set; }

        public Guest2 Guest { get; set; }
        public TourTime TourTime { get; set; }

        public TourReservation SelectedTourReservation { get; set; }
        public TourReservation SelectedActiveReservation { get; set; }
        public GuestTourAttendance GuestTourAttendance { get; set; }
        public Tour Tour { get; set; }
        public ObservableCollection<TourReservation> ActiveTours { get; set; }
        public Guest2MainView Guest2MainView { get; set; }

        private TourKeyPoint _currentKeyPoint;
        public TourKeyPoint CurrentKeyPointInd
        {
            get { return _currentKeyPoint; }
            set
            {
                _currentKeyPoint = value;
                OnPropertyChanged();
            }
        }
        /*private int _AAA;
        public int AAA
        {
            get {  return _AAA; }
            set
            {
                _AAA = value;
                OnPropertyChanged();
            }
        }*/

        //public Tour Tour { get; set; }
        //public TourTime TourTime { get; set; }

        public string CurrentKeyPoint
        {
            get
            {
                if (TourTime != null && TourTime.Tour != null && TourTime.CurrentKeyPointIndex >= 0 && TourTime.CurrentKeyPointIndex < TourTime.Tour.KeyPoints.Count)
                {
                    return TourTime.Tour.KeyPoints[TourTime.CurrentKeyPointIndex].Title; //nacin sa stringom i .title, ne radi
                }
                else
                {
                    return null;
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Guest2MainViewModel(Guest2MainView guest2MainView, Guest2 guest)
        {
            Guest2MainView = guest2MainView;
            Guest = guest;
            LoadFromFiles();
            InitCommands();

           


            //CurrentKeyPointInd = TourTime.CurrentKeyPoint;
            //TourTime.CurrentKeyPoint = TourTime.Tour.KeyPoints[TourTime.CurrentKeyPointIndex];
            //_currentKeyPointIndex = TourTime.CurrentKeyPointIndex;
            //CurrentKeyPoint = TourTime.Tour.KeyPoints[TourTime.CurrentKeyPointIndex];

            Guest.Reservations = new ObservableCollection<TourReservation>(_tourReservationService.GetAllByGuestId(guest.Id));
            Guest.Vouchers = new ObservableCollection<TourVoucher>(_tourVoucherService.GetValidVouchersByGuestId(guest.Id));

            ActiveTours = new ObservableCollection<TourReservation>(_tourReservationService.GetActiveByGuestId(guest.Id));
            TourTime = new TourTime();
            GuestTourAttendance = new GuestTourAttendance();

            /*foreach (TourReservation tr in ActiveTours) 
            {
                _AAA = TourTime.CurrentKeyPointIndex;

            }*/
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
            _guestTourAttendanceService = new GuestTourAttendanceService();


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
            RateVisitedTours = new RelayCommand(Executed_RateVisitedTours, CanExecute_RateVisitedTours);
            SearchAndReserve = new RelayCommand(Executed_SearchAndReserve, CanExecute_SearchAndReserve);
            ConfirmAttendance = new RelayCommand(Executed_ConfirmAttendance, CanExecute_ConfirmAttendance);
            //ShowImages = new RelayCommand(Executed_ShowImages, CanExecute_ShowImages);
            //CancelReservation = new RelayCommand(Executed_CancelReservation, CanExecute_CancelReservation);
        }

        public void Executed_RateVisitedTours(object obj)
        {
            Window win = new TourRatingView(Guest);
            win.Show();
            //Guest2MainView.Close();
        }
        public bool CanExecute_RateVisitedTours(object obj)
        {
            return true;
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
        public void Executed_ConfirmAttendance(object obj)
        {
            MessageBox.Show("Do you want to confirm attendance on this Tour for all reservations?"); //ili napravi konstruktor GTA koji prihvata i rezervaciju kao parametar pa trazi po rezervaciji?
            MessageBoxButton messageBoxButton = MessageBoxButton.OK;
            if (messageBoxButton == MessageBoxButton.OK)
            {
                _guestTourAttendanceService.ConfirmAttendanceForTourTime(SelectedActiveReservation.Guest2Id, SelectedActiveReservation.TourTimeId);
                MessageBox.Show("Your tour attendance is confirmed.");
            }
        }

        

        public bool CanExecute_ConfirmAttendance(object obj)
        {
            return true;
        }

        public void Update()
        {
            throw new NotImplementedException();
        }

        

    }
}
