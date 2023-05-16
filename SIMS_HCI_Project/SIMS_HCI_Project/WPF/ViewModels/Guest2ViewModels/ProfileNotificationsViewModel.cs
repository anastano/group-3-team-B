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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace SIMS_HCI_Project.WPF.ViewModels.Guest2ViewModels
{
    public class ProfileNotificationsViewModel : INotifyPropertyChanged
    {
        #region PropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        public Guest2 Guest { get; set; }
        public NavigationService NavigationService { get; set; }
        public Frame ProfileFrame { get; set; }
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
        private Notification _selectedNotification;
        public Notification SelectedNotification
        {
            get => _selectedNotification;
            set
            {
                _selectedNotification = value;
                OnPropertyChanged();
                NavigateToNotification();
            }
        }

        public Tour Tour { get; set; }
        
        private NotificationService _notificationService;
        private TourService _tourService;
        private GuestTourAttendanceService _guestTourAttendanceService;
        public List<GuestTourAttendance> Attendances { get; set; }


        public RelayCommand BackCommand { get; set; }
        public RelayCommand HelpCommand { get; set; }
        public RelayCommand NotificationSelectedCommand { get; set; }

        public ProfileNotificationsViewModel(Guest2 guest, NavigationService navigationService, Frame profileFrame)
        {
            Guest = guest;
            NavigationService = navigationService;
            ProfileFrame = profileFrame;
            //SelectedNotification = new Notification();
            LoadFromFiles();
            InitCommands();
            Notifications = new ObservableCollection<Notification>(_notificationService.GetUnreadByUserId(Guest.Id));
            Tour = new Tour();
            Attendances = new List<GuestTourAttendance>();
        }

        public void NavigateToNotification()
        {
            int tourId = 0;
            if (SelectedNotification != null)
            {
                switch (SelectedNotification.Type)
                {
                    case NotificationType.TOUR_REQUEST_ACCEPTED:
                        tourId = SelectedNotification.ExtractTourId(SelectedNotification);
                        Tour = _tourService.GetTourInformation(tourId);
                        NavigationService.Navigate(new SearchAndReserveView(Guest, Tour, NavigationService));
                        break;
                    case NotificationType.NEW_TOUR:
                        tourId = SelectedNotification.ExtractTourId(SelectedNotification);
                        Tour = _tourService.GetTourInformation(tourId);
                        NavigationService.Navigate(new SearchAndReserveView(Guest, Tour, NavigationService));
                        break;
                    case NotificationType.CONFIRM_ATTENDANCE:
                        tourId = SelectedNotification.ExtractTourId(SelectedNotification);
                        TourTime tourTime = _tourService.GetTourInstance(tourId);
                        if(ConfirmRequestSubmission(tourTime.Id) == MessageBoxResult.Yes)
                        {
                            _guestTourAttendanceService.ConfirmAttendance(Guest.Id, tourTime.Id);
                            MessageBox.Show("Your tour attendance is confirmed.");
                        }
                        _notificationService.MarkAsRead(SelectedNotification.Id);
                        break;
                    default:
                        break;
                }
            }
        }

        private MessageBoxResult ConfirmRequestSubmission(int tourId)
        {
            string sMessageBoxText = $"Are you sure you want to confirm attendance for tour " + tourId +"?";
            string sCaption = "Confirm attendance";

            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxImage icnMessageBox = MessageBoxImage.Question;

            MessageBoxResult result = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
            return result;
        }

        private void LoadFromFiles()
        {
            _notificationService = new NotificationService();
            _tourService = new TourService();
            _guestTourAttendanceService = new GuestTourAttendanceService();
        }

        //napravi fju da boja elemenata koji su procitani bude druhacija od onih koje nisu
        private void InitCommands()
        {
            BackCommand = new RelayCommand(ExecuteBackCommand, CanExecute);
            //HelpCommand todo

        }

        private void ExecuteBackCommand(object obj)
        {
            ProfileFrame.NavigationService.Navigate(new ProfileMainView(Guest, NavigationService, ProfileFrame));
        }

        private bool CanExecute(object obj)
        {
            return true;
        }
    }
}
