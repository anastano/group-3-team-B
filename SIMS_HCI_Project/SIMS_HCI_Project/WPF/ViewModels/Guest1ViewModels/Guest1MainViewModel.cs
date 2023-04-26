using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Model;
using SIMS_HCI_Project.Observer;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Views.Guest1Views;
using SIMS_HCI_Project.WPF.Views.OwnerViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AccommodationReservation = SIMS_HCI_Project.Domain.Models.AccommodationReservation;
using Guest1 = SIMS_HCI_Project.Domain.Models.Guest1;
using Notification = SIMS_HCI_Project.Domain.Models.Notification;
using Owner = SIMS_HCI_Project.Domain.Models.Owner;

namespace SIMS_HCI_Project.WPF.ViewModels.Guest1ViewModels
{
    internal class Guest1MainViewModel : INotifyPropertyChanged
    {
        private Guest1Service _guest1Service;
        private OwnerService _ownerService;
        private LocationService _locationService;
        private AccommodationService _accommodationService;
        private AccommodationReservationService _reservationService;
        private RescheduleRequestService _requestService;
        private NotificationService _notificationService;
        private RatingGivenByGuestService _guestRatingService;
        private RatingGivenByOwnerService _ownerRatingService;
        public Guest1MainView Guest1MainView { get; set; }
        private ReservationsViewModel reservationsViewModel;
        public Guest1 Guest { get; set; }
        public RelayCommand ShowReservationsCommand { get; set; }
        public RelayCommand SearchAccommodationCommand { get; set; }
        public RelayCommand ShowRatingsCommand { get; set; }
        public RelayCommand ShowProfileCommand { get; set; }
        public RelayCommand LogoutCommand { get; set; }

        private object _currentViewModel;

        public object CurrentViewModel

        {
            get => _currentViewModel;
            set
            {
                if (value != _currentViewModel)
                {
                    _currentViewModel = value;
                    OnPropertyChanged();
                }
            }

        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public Guest1MainViewModel(Guest1MainView guest1MainView, Guest1 guest)
        {
            Guest1MainView = guest1MainView;
            Guest = guest;
            LoadFromFiles();
            InitCommands();
            CurrentViewModel = new ProfileViewModel(Guest);
            reservationsViewModel = new ReservationsViewModel(Guest);
        }

        public void LoadFromFiles()
        {
            _guest1Service = new Guest1Service();
            _ownerService = new OwnerService();
            _locationService = new LocationService();
            _accommodationService = new AccommodationService();
            _reservationService = new AccommodationReservationService();
            _requestService = new RescheduleRequestService();
            _notificationService = new NotificationService();
            _guestRatingService = new RatingGivenByGuestService();
            _ownerRatingService = new RatingGivenByOwnerService();

            _accommodationService.ConnectAccommodationsWithLocations(_locationService);
            _accommodationService.ConnectAccommodationsWithOwners(_ownerService);
            _reservationService.ConnectReservationsWithAccommodations(_accommodationService);
            _reservationService.ConnectReservationsWithGuests(_guest1Service);
            _reservationService.ConvertReservedReservationIntoCompleted(DateTime.Now);
            _reservationService.ConnectReservationsWithGuests(_guest1Service);
            _requestService.ConnectRequestsWithReservations(_reservationService);
            _reservationService.ConvertReservationsIntoRated(_guestRatingService);
            _ownerRatingService.ConnectRatingsWithReservations(_reservationService);
            _guestRatingService.ConnectRatingsWithReservations(_reservationService);
            _guestRatingService.FillAverageRatingAndSuperFlag(_ownerService);
        }
        public void ExecutedShowReservationsCommand(object obj)
        {
            CurrentViewModel = new ReservationsViewModel(Guest);
        }
        public void ExecutedSearchAccommodationCommand(object obj)
        {
            CurrentViewModel = new AccommodationSearchViewModel(Guest);
        }
        public void ExecutedShowRatingsCommand(object obj)
        {
            CurrentViewModel = new MyRatingsViewModel(Guest);
        }

        public bool CanExecute(object obj)
        {
            return true;
        }
        public void ExecutedLogoutCommand(object obj)
        {
            foreach (Notification notification in _notificationService.GetUnreadByUserId(Guest.Id))
            {
                _notificationService.MarkAsRead(notification.Id);
            }
            //Guest1MainView.Close();
        }
        public void ExecutedShowProfileCommand(object obj)
        {
            CurrentViewModel = (object)new ProfileViewModel(Guest);
        }
        public void InitCommands()
        {
            ShowReservationsCommand = new RelayCommand(ExecutedShowReservationsCommand, CanExecute);
            SearchAccommodationCommand = new RelayCommand(ExecutedSearchAccommodationCommand, CanExecute);
            ShowProfileCommand = new RelayCommand(ExecutedShowProfileCommand, CanExecute);
            LogoutCommand = new RelayCommand(ExecutedLogoutCommand, CanExecute);
            ShowRatingsCommand = new RelayCommand(ExecutedShowRatingsCommand, CanExecute);
        }
    }
}
