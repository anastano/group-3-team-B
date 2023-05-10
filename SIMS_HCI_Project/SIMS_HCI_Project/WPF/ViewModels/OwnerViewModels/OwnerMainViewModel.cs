using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Observer;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Views;
using SIMS_HCI_Project.WPF.Views.Guest1Views;
using SIMS_HCI_Project.WPF.Views.OwnerViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SIMS_HCI_Project.WPF.ViewModels.OwnerViewModels
{
    public class OwnerMainViewModel: IObserver
    {
        #region Service Fields
        private OwnerService _ownerService;
        private Guest1Service _guest1Service;
        private LocationService _locationService;
        private AccommodationService _accommodationService;
        private AccommodationReservationService _reservationService;
        private RescheduleRequestService _requestService;
        private NotificationService _notificationService;
        private RatingGivenByOwnerService _ownerRatingService;
        private RatingGivenByGuestService _guestRatingService;
        private RenovationService _renovationService;
        #endregion

        public OwnerMainView OwnerMainView { get; set; }
        public Owner Owner { get; set; }        
        public ObservableCollection<AccommodationReservation> ReservationsInProgress { get; set; }
        public ObservableCollection<Notification> Notifications { get; set; }
        public RelayCommand ShowAccommodationsCommand { get; set; }
        public RelayCommand ShowReservationsCommand { get; set; }
        public RelayCommand ShowRenovationsCommand { get; set; }
        public RelayCommand ShowPendingRequestsCommand { get; set; }
        public RelayCommand ShowUnratedReservationsCommand { get; set; }
        public RelayCommand ShowGuestReviewsCommand { get; set; }
        public RelayCommand LogoutCommand { get; set; }

        public OwnerMainViewModel(OwnerMainView ownerMainView, Owner owner) 
        {
            OwnerMainView = ownerMainView;
            Owner = owner;
            
            LoadFromFiles();
            InitCommands();

            ReservationsInProgress = new ObservableCollection<AccommodationReservation>(_reservationService.GetInProgressByOwnerId(Owner.Id));
            Notifications = new ObservableCollection<Notification>(_notificationService.GetUnreadByUserId(Owner.Id));

            ShowNotificationsAndSuperFlag();

            _reservationService.Subscribe(this);
        }

        public void LoadFromFiles()
        {
            _ownerService = new OwnerService();
            _guest1Service = new Guest1Service();
            _locationService = new LocationService();
            _accommodationService = new AccommodationService();
            _reservationService = new AccommodationReservationService();
            _requestService = new RescheduleRequestService();
            _notificationService = new NotificationService();
            _ownerRatingService = new RatingGivenByOwnerService();
            _guestRatingService = new RatingGivenByGuestService();
            _renovationService = new RenovationService();

            _accommodationService.ConnectAccommodationsWithLocations(_locationService);
            _reservationService.ConnectReservationsWithAccommodations(_accommodationService);
            _reservationService.ConnectReservationsWithGuests(_guest1Service);
            _requestService.ConnectRequestsWithReservations(_reservationService);
            _ownerRatingService.ConnectRatingsWithReservations(_reservationService);
            _guestRatingService.ConnectRatingsWithReservations(_reservationService);
            _renovationService.ConnectRenovationsWithAccommodations(_accommodationService);

            _guestRatingService.FillAverageRatingAndSuperFlag(Owner);
        }

        private void ShowNotificationsAndSuperFlag()
        {
            int unratedGuestsNumber = _ownerRatingService.GetUnratedReservations(Owner.Id, _reservationService).Count;
            OwnerMainView.txtUnratedGuestsNotifications.Visibility = unratedGuestsNumber != 0 ? Visibility.Visible : Visibility.Collapsed;

           // int guestRequestsNumber = _requestService.GetPendingByOwnerId(Owner.Id).Count;
           // OwnerMainView.txtGuestsRequestsNotifications.Visibility = guestRequestsNumber != 0 ? Visibility.Visible : Visibility.Collapsed;

            int otherNotificationsNumber = Notifications.Count;
            OwnerMainView.lvNotifications.Visibility = otherNotificationsNumber != 0 ? Visibility.Visible : Visibility.Collapsed;

            OwnerMainView.imgSuperFlag.Visibility = Owner.SuperFlag ? Visibility.Visible : Visibility.Collapsed;
        }

        #region Commands
        public void Executed_ShowAccommodationsCommand(object obj)
        {
            Window accommodationsView = new AccommodationsView(_accommodationService, Owner);
            accommodationsView.ShowDialog();
        }

        public bool CanExecute_ShowAccommodationsCommand(object obj)
        {
            return true;
        }

        public void Executed_ShowReservationsCommand(object obj)
        {
            Window reservationsView = new GuestReservationsView(_reservationService, Owner);
            reservationsView.ShowDialog();
        }

        public bool CanExecute_ShowReservationsCommand(object obj)
        {
            return true;
        }

        public void Executed_ShowRenovationsCommand(object obj)
        {
            Window renovtionsView = new RenovationsView(_renovationService, Owner);
            renovtionsView.ShowDialog();
        }

        public bool CanExecute_ShowRenovationsCommand(object obj)
        {
            return true;
        }

        public void Executed_ShowPendingRequestsCommand(object obj)
        {
            Window requestsView = new RescheduleRequestsView(_requestService, _reservationService, _notificationService, Owner);
            requestsView.ShowDialog();
        }

        public bool CanExecute_ShowPendingRequestsCommand(object obj)
        {
            return true;
        }

        public void Executed_ShowUnratedReservationsCommand(object obj)
        {
            Window unratedReservationsView = new UnratedReservationsView(_reservationService, _ownerRatingService, Owner);
            unratedReservationsView.ShowDialog();
        }

        public bool CanExecute_ShowUnratedReservationsCommand(object obj)
        {
            return true;
        }

        public void Executed_ShowGuestReviewsCommand(object obj)
        {
            Window guestReviewsView = new GuestReviewsView(_guestRatingService, _ownerRatingService, Owner);
            guestReviewsView.ShowDialog();
        }

        public bool CanExecute_ShowGuestReviewsCommand(object obj)
        {
            return true;
        }

        public void Executed_LogoutCommand(object obj)
        {
            /*
            foreach (Notification notification in _notificationService.GetUnreadByUserId(Owner.Id))
            {
                _notificationService.MarkAsRead(notification.Id);
            }
            */

            OwnerMainView.Close();
        }

        public bool CanExecute_LogoutCommand(object obj)
        {
            return true;
        }
        #endregion

        public void InitCommands() 
        {
            ShowAccommodationsCommand = new RelayCommand(Executed_ShowAccommodationsCommand, CanExecute_ShowAccommodationsCommand);
            ShowReservationsCommand = new RelayCommand(Executed_ShowReservationsCommand, CanExecute_ShowReservationsCommand);
            ShowRenovationsCommand = new RelayCommand(Executed_ShowRenovationsCommand, CanExecute_ShowRenovationsCommand);
            ShowPendingRequestsCommand = new RelayCommand(Executed_ShowPendingRequestsCommand, CanExecute_ShowPendingRequestsCommand);
            ShowUnratedReservationsCommand = new RelayCommand(Executed_ShowUnratedReservationsCommand, CanExecute_ShowUnratedReservationsCommand);
            ShowGuestReviewsCommand = new RelayCommand(Executed_ShowGuestReviewsCommand, CanExecute_ShowGuestReviewsCommand);
            LogoutCommand = new RelayCommand(Executed_LogoutCommand, CanExecute_LogoutCommand);
        }

        public void Update()
        {
            UpdateReservationsInProgress();
        }

        public void UpdateReservationsInProgress()
        {
            ReservationsInProgress.Clear();
            foreach (AccommodationReservation reservation in _reservationService.GetInProgressByOwnerId(Owner.Id))
            {
                ReservationsInProgress.Add(reservation);
            }
        }
    }
}
