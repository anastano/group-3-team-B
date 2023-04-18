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
                                                            

namespace SIMS_HCI_Project.WPF.ViewModels.Guest2ViewModels 
{
    public class Guest2MainViewModel : IObserver
    {
        #region Services
        private TourReservationService _tourReservationService;
        private TourService _tourService;
        private LocationService _locationService;
        private TourVoucherService _tourVoucherService;
        private GuestTourAttendanceService _guestTourAttendanceService;
        private NotificationService _notificationService;
        #endregion
        #region Commands
        public RelayCommand SearchAndReserve { get; set; }
        public RelayCommand RateVisitedTours { get; set; }
        public RelayCommand ConfirmAttendance { get; set; }
        public RelayCommand Logout { get; set; }
        #endregion
        private ObservableCollection<Notification> _notifications;
        public ObservableCollection<Notification> Notifications
        {
            get { return _notifications; }
            set
            {
                _notifications = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<TourReservation> _activeTours;
        public ObservableCollection<TourReservation> ActiveTours //change to GuestTourAttendance
        {
            get { return _activeTours; }
            set
            {
                _activeTours = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<TourReservation> _reservations;
        public ObservableCollection<TourReservation> Reservations
        {
            get { return _reservations; }
            set
            {
                _reservations = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<TourVoucher> _vouchers;
        public ObservableCollection<TourVoucher> Vouchers
        {
            get { return _vouchers; }
            set
            {
                _vouchers = value;
                OnPropertyChanged();
            }
        }


        public List<GuestTourAttendance> Attendances { get; set; }
        public Guest2 Guest { get; set; }
        public TourTime TourTime { get; set; }
        public GuestTourAttendance GuestTourAttendance { get; set; }
        public Guest2MainView Guest2MainView { get; set; }


        private TourReservation _selectedActiveReservation;
        public TourReservation SelectedActiveReservation
        {
            get { return _selectedActiveReservation; }
            set
            {
                _selectedActiveReservation = value;
                OnPropertyChanged();
            }
        }
       
        #region PropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        public Guest2MainViewModel(Guest2MainView guest2MainView, Guest2 guest)
        {
            Guest2MainView = guest2MainView;
            Guest = guest;
            LoadFromFiles();
            InitCommands();
            MakeNotificationsForAttendanceConfirmation(); //add in TourProgressViewModel later

            Attendances = new List<GuestTourAttendance>();
            TourTime = new TourTime();
            GuestTourAttendance = new GuestTourAttendance();
            Reservations = new ObservableCollection<TourReservation>(_tourReservationService.GetAllByGuestId(guest.Id));
            Vouchers = new ObservableCollection<TourVoucher>(_tourVoucherService.GetValidVouchersByGuestId(guest.Id));
            ActiveTours = new ObservableCollection<TourReservation>(_tourReservationService.GetActiveByGuestId(guest.Id));
            Notifications = new ObservableCollection<Notification>(_notificationService.GetUnreadByUserId(Guest.Id));

            ShowNotifications();

            _tourReservationService.Subscribe(this);
            _tourVoucherService.Subscribe(this);
        }

        public void MakeNotificationsForAttendanceConfirmation()
        {
            Attendances = _guestTourAttendanceService.GetByConfirmationRequestedStatus(Guest.Id);
            foreach (GuestTourAttendance attendance in Attendances)
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
        public void InitCommands()
        {
            RateVisitedTours = new RelayCommand(Executed_RateVisitedTours, CanExecute_RateVisitedTours);
            SearchAndReserve = new RelayCommand(Executed_SearchAndReserve, CanExecute_SearchAndReserve);
            ConfirmAttendance = new RelayCommand(Executed_ConfirmAttendance, CanExecute_ConfirmAttendance);
            Logout = new RelayCommand(Executed_Logout, CanExecute_Logout);
        }
        #region Commands
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
        public void Executed_Logout(object obj)
        {
            foreach (Notification notification in _notificationService.GetUnreadByUserId(Guest.Id))
            {
                _notificationService.MarkAsRead(notification.Id);
            }
            Guest2MainView.Close();
        }

        public bool CanExecute_Logout(object obj)
        {
            return true;
        }
        #endregion
        #region ShowNotifications
        public void ShowNotifications()
        {
            int otherNotificationsNumber = Notifications.Count;
            Guest2MainView.lvNotifications.Visibility = otherNotificationsNumber != 0 ? Visibility.Visible : Visibility.Collapsed;
        }
        #endregion
        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}
