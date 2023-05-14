using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Views.Guest2Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace SIMS_HCI_Project.WPF.ViewModels.Guest2ViewModels
{
    public class ProfileMainViewModel : INotifyPropertyChanged
    {
        //912, 518
        #region PropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        public Guest2 Guest { get; set; }
        public NavigationService NavigationService { get; set; }
        #region Services
        private TourReservationService _tourReservationService;
        private TourVoucherService _tourVoucherService;
        private GuestTourAttendanceService _guestTourAttendanceService;
        private NotificationService _notificationService;
        #endregion

        #region Properties

        public List<GuestTourAttendance> Attendances { get; set; }
        public GuestTourAttendance GuestTourAttendance { get; set; }

        private int _unreadCount;
        public int UnreadCount
        {
            get { return _unreadCount; }
            set
            {
                _unreadCount = value;
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
        #endregion

        public string UnreadNotificationsMessage { get; set; }
        public Frame ProfileFrame { get; set; }
        #region Commands
        public RelayCommand ShowNotificationsCommand { get; set; }
        //TODO HELP CMD

        #endregion
        public ProfileMainViewModel(Guest2 guest, NavigationService navigationService, Frame profileFrame)
        {
            Guest = guest;
            NavigationService = navigationService;
            ProfileFrame = profileFrame;
            InitCommands();
            LoadFromFiles();
            ShowNotificationsCount();

            Attendances = new List<GuestTourAttendance>(); //trebace kada pravi obavestenja da potvrdi attendance na turi, sredi drugacije nekako, MakeNotificationsForAttendanceConfirmation() u Guest2MainViewModel 
            GuestTourAttendance = new GuestTourAttendance(); //ne znam sta ce, mozda u xamlu? vidi?

            Reservations = new ObservableCollection<TourReservation>(_tourReservationService.GetAllByGuestId(guest.Id));

            Vouchers = new ObservableCollection<TourVoucher>(_tourVoucherService.GetValidVouchersByGuestId(guest.Id));

            ActiveTours = new ObservableCollection<TourReservation>(_tourReservationService.GetActiveByGuestId(guest.Id));

            Notifications = new ObservableCollection<Notification>(_notificationService.GetUnreadByUserId(Guest.Id)); //izmeni da prosledi sve notifikacije tog gosta u novi prof Notif page, a ne samo neporcitane,
                                                                                                                      //ili ovo ne treba uopste, vec da samo prikaze broj enporcitanih, a u narednom ce sam napraviti ovu listu notifikacija na osnovu id gosta
                                                                                                                      //a da u ovom page prikaze broj neprocitanih

            //sredi xaml kod za ovaj frame
            //MakeNotificationsForAttendanceConfirmation();
            //makeNotifPls();
        }

        public void ShowNotificationsCount()
        {
            //uradi kao labela bindovana
            UnreadCount = _notificationService.GetUnreadByUserId(Guest.Id).Count();
            UnreadNotificationsMessage = "You have " + UnreadCount + " unread notifications.";
        }


        private void LoadFromFiles()
        {
            _tourVoucherService = new TourVoucherService();
            _tourReservationService = new TourReservationService();
            _guestTourAttendanceService = new GuestTourAttendanceService();
            _notificationService = new NotificationService(); //proveri da li je u startap servisu sve povezano za obavestenja 
        }

        private void InitCommands()
        {
            ShowNotificationsCommand = new RelayCommand(ExecuteShowNotifications, CanExecute);    
        }

        private void ExecuteShowNotifications(object obj)
        {
            ProfileFrame.NavigationService.Navigate(new ProfileNotificationsView(Guest, NavigationService, ProfileFrame)); //check if ok?
        }

        private bool CanExecute(object obj)
        {
            return true;
        }

        public void MakeNotificationsForAttendanceConfirmation()
        {
            //move to where guide sends invitaton
            Attendances = _guestTourAttendanceService.GetByConfirmationRequestedStatus(Guest.Id);
            foreach (GuestTourAttendance attendance in Attendances)
            {
                if (_notificationService.GetAll().Select(n => n.Message).ToList().Contains(attendance.TourTimeId.ToString()) == false)
                {

                    string Message = "You have request to confirm your attendance for tour with id: [" + attendance.TourTimeId + "].";
                    _notificationService.Add(new Notification(Message, Guest.Id, false, NotificationType.CONFIRM_ATTENDANCE));
                }
            }
        }
        public void makeNotifPls()
        {
            foreach (TourReservation reservation in Reservations)
            {
                if (reservation.TourTime.Status == TourStatus.IN_PROGRESS)
                {
                    string Message = "You have request to confirm your attendance for tour with id: [" + reservation.TourTimeId + "].";
                    _notificationService.Add(new Notification(Message, Guest.Id, false, NotificationType.CONFIRM_ATTENDANCE));
                }
            }
        }
    }
}
