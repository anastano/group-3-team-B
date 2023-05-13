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
using System.Windows.Controls.Primitives;                   
using System.Windows.Input;
using System.Windows.Navigation;

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
        //public ICommand CloseWindow { get; set; }
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
        public ObservableCollection<TourReservation> ActiveTours
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

        public List<GuestTourAttendance> Attendances { get; set; }
        public Guest2 Guest { get; set; }
        public TourTime TourTime { get; set; }
        public GuestTourAttendance GuestTourAttendance { get; set; }
        public Guest2MainView Guest2MainView { get; set; }
        public NavigationService NavigationService { get; set; }
        #region PropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public Guest2MainViewModel(Guest2MainView guest2MainView, Guest2 guest, NavigationService navigationService)
        {
            Guest2MainView = guest2MainView;
            Guest = guest;
            NavigationService = navigationService;
            LoadFromFiles();
            InitCommands();
            MakeNotificationsForAttendanceConfirmation();

            Attendances = new List<GuestTourAttendance>();
            TourTime = new TourTime();
            GuestTourAttendance = new GuestTourAttendance();
            Reservations = new ObservableCollection<TourReservation>(_tourReservationService.GetAllByGuestId(guest.Id));
            Vouchers = new ObservableCollection<TourVoucher>(_tourVoucherService.GetValidVouchersByGuestId(guest.Id));
            ActiveTours = new ObservableCollection<TourReservation>(_tourReservationService.GetActiveByGuestId(guest.Id));
            Notifications = new ObservableCollection<Notification>(_notificationService.GetUnreadByUserId(Guest.Id));

            ShowNotifications(); //izmeni da prikaze broj neprocitanih i da klikom vodi na page za notif

            _tourReservationService.Subscribe(this); //brisi
            _tourVoucherService.Subscribe(this); //brisi
        }


        private void LoadFromFiles()
        {
            _tourVoucherService = new TourVoucherService();
            _tourReservationService = new TourReservationService();
            _tourService = new TourService();
            _locationService = new LocationService();
            _guestTourAttendanceService = new GuestTourAttendanceService();
            _notificationService = new NotificationService(); //proveri da li je u startap servisu sve povezano za obavestenja 
        }

        public void InitCommands()
        {
            ConfirmAttendance = new RelayCommand(ExecutedConfirmAttendance, CanExecuteConfirmAttendance); // to notif page move
            Logout = new RelayCommand(ExecutedLogout, CanExecuteLogout); //not needed?
        }
        #region Commands
        

        

        public void ExecutedConfirmAttendance(object obj)
        {
            //move to notif profile page
            MessageBox.Show("Do you want to confirm attendance on this Tour for all reservations?");
            MessageBoxButton messageBoxButton = MessageBoxButton.OK;
            _guestTourAttendanceService.ConfirmAttendanceForTourTime(SelectedActiveReservation.Guest2Id, SelectedActiveReservation.TourTimeId);
               
            MessageBox.Show("Your tour attendance is confirmed.");
        }

        public bool CanExecuteConfirmAttendance(object obj)
        {
            return true;
        }
        public void ExecutedLogout(object obj) // prebaci da ide na listu obavestenja u novi page, pa da tu moze da oznaci da je procitano i da vidi detalje
        {
            foreach (Notification notification in _notificationService.GetUnreadByUserId(Guest.Id))
            {
                _notificationService.MarkAsRead(notification.Id);
            }
        }

        public bool CanExecuteLogout(object obj)
        {
            return true;
        }

        #endregion
        #region Notifications
        public void ShowNotifications()
        {
            int otherNotificationsNumber = Notifications.Count;
            //count treba da prikaze broj neprocitanih, izmeni da bude za neprocitane neka metoda
            //zavisibility ne treba sada
            Guest2MainView.lvNotifications.Visibility = otherNotificationsNumber != 0 ? Visibility.Visible : Visibility.Collapsed;
        }
        #endregion
        public void MakeNotificationsForAttendanceConfirmation()
        {
            //move to where guide sends invitaton
            Attendances = _guestTourAttendanceService.GetByConfirmationRequestedStatus(Guest.Id);
            foreach (GuestTourAttendance attendance in Attendances)
            {
                if( _notificationService.GetAll().Select(n => n.Message).ToList().Contains(attendance.TourTimeId.ToString()) == false)
                { 
                
                    String Message = "You have request to confirm your attendance for tour with id: " + attendance.TourTimeId + ". Confirm your attendance on that tour in the list of active tours.";
                    _notificationService.Add(new Notification(Message, Guest.Id, false));
                }
            }
        }
        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}
