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
using System.Windows.Controls;
using System.Windows.Controls.Primitives;                   // TODO: dodaj logout i tada podesi notifikacije na procitane
                                                            // TODO: napraavi nekako da se odmah cuva u csv kada se izmeni, da bi se moglo podrzati da kada se izloguje, nozi korisnik moze da vidi promene u csv koje je napravio stari korisnik

namespace SIMS_HCI_Project.WPF.ViewModels.Guest2ViewModels // TODO: prikazuje samo indeks trenutne TT, a treba da prikaze naziv keyPointa
{
    public class Guest2MainViewModel : IObserver
    {
        private TourReservationService _tourReservationService;
        private TourService _tourService;
        private LocationService _locationService;
        private TourVoucherService _tourVoucherService;
        private GuestTourAttendanceService _guestTourAttendanceService;
        private NotificationService _notificationService;
        public ObservableCollection<Notification> Notifications { get; set; }
        public RelayCommand SearchAndReserve { get; set; }
        public RelayCommand ShowImages { get; set; }
        public RelayCommand CancelReservation { get; set; }
        public RelayCommand RateVisitedTours { get; set; }
        public RelayCommand ConfirmAttendance { get; set; }
        public List<GuestTourAttendance> attendances { get; set; }
        public Guest2 Guest { get; set; }
        public TourTime TourTime { get; set; }
        public GuestTourAttendance ActiveGuestAttendance { get; set; }
        //public AttendanceStatus AttendanceStatus { get; set; }
        public TourReservation SelectedTourReservation { get; set; }
        //public TourReservation SelectedActiveReservation { get; set; }
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
        private AttendanceStatus _attendanceStatus;

        public AttendanceStatus AttendanceStatus
        {
            get {
                
                    ActiveGuestAttendance = _guestTourAttendanceService.GetByGuestAndTourTimeIds(SelectedActiveReservation.Guest2Id, SelectedActiveReservation.TourTimeId);
                    _attendanceStatus = ActiveGuestAttendance.Status;

                return _attendanceStatus; }
            set
            {
                _attendanceStatus = value;
                OnPropertyChanged();
            }
        }

        private TourReservation _selectedActiveReservation;
        public TourReservation SelectedActiveReservation
        {
            get { return _selectedActiveReservation; }
            set
            {
                _selectedActiveReservation = value;
                ActiveGuestAttendance = _guestTourAttendanceService.GetByGuestAndTourTimeIds(SelectedActiveReservation.Guest2Id, SelectedActiveReservation.TourTimeId);
                _attendanceStatus = ActiveGuestAttendance.Status;
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

            attendances = new List<GuestTourAttendance>();

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
            SelectedActiveReservation = ActiveTours[0];
            if(SelectedActiveReservation != null)
            {
                ActiveGuestAttendance = _guestTourAttendanceService.GetByGuestAndTourTimeIds(SelectedActiveReservation.Guest2Id, SelectedActiveReservation.TourTimeId);
                _attendanceStatus = ActiveGuestAttendance.Status;

            }
            ActiveGuestAttendance = _guestTourAttendanceService.GetByGuestAndTourTimeIds(SelectedActiveReservation.Guest2Id, SelectedActiveReservation.TourTimeId);
            //ActiveGuestAttendance = _guestTourAttendanceService.GetByGuestAndTourTimeIds(SelectedActiveReservation.Guest2Id, SelectedActiveReservation.TourTimeId);
            //AttendanceStatus = ActiveGuestAttendance.Status;
            MakeNotificationsForAttendanceConfirmation(); //mozda podati posle u TourProgressViewModel

            Notifications = new ObservableCollection<Notification>(_notificationService.GetUnreadByUserId(Guest.Id));
            ShowNotifications();

            _tourReservationService.Subscribe(this);
            _tourVoucherService.Subscribe(this);
        }

        public void MakeNotificationsForAttendanceConfirmation()
        {
            attendances = _guestTourAttendanceService.GetByConfirmationRequestedStatus(Guest.Id);
            foreach (GuestTourAttendance attendance in attendances)
            {
                String Message = "You have request to confirm your attendance for tour with id: " + attendance.TourTimeId + ". Confirm your attendance on that tour in the list of active tours.";
                _notificationService.Add(new Notification(Message, Guest.Id, false));

            }
        }

        private void LoadFromFiles()
        {
            _tourVoucherService = new TourVoucherService();
            _tourReservationService = new TourReservationService();
            _tourService = new TourService();
            _locationService = new LocationService();
            _guestTourAttendanceService = new GuestTourAttendanceService();
            _notificationService = new NotificationService();

            _tourService.ConnectLocations();
            _tourService.ConnectKeyPoints();
            _tourService.ConnectDepartureTimes();

            _tourReservationService.ConnectTourTimes(_tourService);
            _tourReservationService.ConnectVouchers(_tourVoucherService);
            _tourReservationService.ConnectAvailablePlaces(_tourService);
        }

        public void ShowNotifications()
        {
            int otherNotificationsNumber = Notifications.Count;
            Guest2MainView.lvNotifications.Visibility = otherNotificationsNumber != 0 ? Visibility.Visible : Visibility.Collapsed;
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
            TourTime = _tourService.GetTourInstance(SelectedTourReservation.TourTimeId);
            Tour = _tourService.GetTourInformation(TourTime.TourId);
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
            MessageBox.Show("Do you want to confirm attendance on this Tour for all reservations?"); //ili napravi konstruktor GTA koji prihvata i rezervaciju kao parametar pa trazi po rezervaciji? okej je ovako ipak
            MessageBoxButton messageBoxButton = MessageBoxButton.OK;
            _guestTourAttendanceService.ConfirmAttendanceForTourTime(SelectedActiveReservation.Guest2Id, SelectedActiveReservation.TourTimeId);
               
            MessageBox.Show("Your tour attendance is confirmed.");
            
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
