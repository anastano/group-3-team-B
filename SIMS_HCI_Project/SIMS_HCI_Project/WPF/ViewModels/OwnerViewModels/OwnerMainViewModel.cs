using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;

using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Views.OwnerViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace SIMS_HCI_Project.WPF.ViewModels.OwnerViewModels
{
    public class OwnerMainViewModel
    {
        #region Service Fields
        private AccommodationReservationService _reservationService;
        private NotificationService _notificationService;
        private RatingGivenByOwnerService _ownerRatingService;
        private RatingGivenByGuestService _guestRatingService;
        
        #endregion

        public static CancellationTokenSource CTS;
        public static bool Demo { get; set; }
        public OwnerMainView OwnerMainView { get; set; }
        public Owner Owner { get; set; }

        public ObservableCollection<AccommodationReservation> ReservationsInProgress { get; set; }
        public ObservableCollection<Notification> Notifications { get; set; }

        #region RelayCommands
        public RelayCommand ShowAccommodationsCommand { get; set; }
        public RelayCommand ShowReservationsCommand { get; set; }
        public RelayCommand ShowRenovationsCommand { get; set; }
        public RelayCommand ShowPendingRequestsCommand { get; set; }
        public RelayCommand ShowUnratedReservationsCommand { get; set; }
        public RelayCommand ShowGuestReviewsCommand { get; set; }
        public RelayCommand ShowStatisticsCommand { get; set; }
        public RelayCommand ShowForumsCommand { get; set; }
        public RelayCommand StartDemoCommand { get; set; }
        public RelayCommand StopDemoCommand { get; set; }
        public RelayCommand LogoutCommand { get; set; }

        #endregion

        public OwnerMainViewModel(OwnerMainView ownerMainView, Owner owner) 
        {
            OwnerMainView = ownerMainView;
            Owner = owner;
            Demo = false;           
            
            LoadFromFiles();
            InitCommands();

            ReservationsInProgress = new ObservableCollection<AccommodationReservation>(_reservationService.GetInProgressByOwnerId(Owner.Id));
            Notifications = new ObservableCollection<Notification>(_notificationService.GetUnreadByUserId(Owner.Id));

            ShowNotificationsAndSuperFlag();

            CTS = new CancellationTokenSource();
        }

        public void LoadFromFiles()
        {
            _reservationService = new AccommodationReservationService();
            _notificationService = new NotificationService();
            _ownerRatingService = new RatingGivenByOwnerService();
            _guestRatingService = new RatingGivenByGuestService();

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
            Window accommodationsView = new AccommodationsView(Owner);
            accommodationsView.ShowDialog();
        }

        public bool CanExecute_ShowAccommodationsCommand(object obj)
        {
            return true;
        }

        public void Executed_ShowReservationsCommand(object obj)
        {
            Window reservationsView = new GuestReservationsView(Owner);
            reservationsView.ShowDialog();
        }

        public bool CanExecute_ShowReservationsCommand(object obj)
        {
            return true;
        }

        public void Executed_ShowRenovationsCommand(object obj)
        {
            Window renovtionsView = new RenovationsView(Owner);
            renovtionsView.ShowDialog();
        }

        public bool CanExecute_ShowRenovationsCommand(object obj)
        {
            return true;
        }

        public void Executed_ShowPendingRequestsCommand(object obj)
        {
            Window requestsView = new RescheduleRequestsView(Owner);
            requestsView.ShowDialog();
        }

        public bool CanExecute_ShowPendingRequestsCommand(object obj)
        {
            return true;
        }

        public void Executed_ShowUnratedReservationsCommand(object obj)
        {
            Window unratedReservationsView = new UnratedReservationsView(Owner);
            unratedReservationsView.ShowDialog();
        }

        public bool CanExecute_ShowUnratedReservationsCommand(object obj)
        {
            return true;
        }

        public void Executed_ShowGuestReviewsCommand(object obj)
        {
            Window guestReviewsView = new GuestReviewsView(Owner);
            guestReviewsView.ShowDialog();
        }

        public bool CanExecute_ShowGuestReviewsCommand(object obj)
        {
            return true;
        }

        public void Executed_ShowStatisticsCommand(object obj)
        {
            Window statisticsView = new SelectAccommodationForStatisticsView(Owner);
            statisticsView.ShowDialog();
        }

        public bool CanExecute_ShowStatisticsCommand(object obj)
        {
            return true;
        }

        public void Executed_ShowForumsCommand(object obj)
        {
            Window forumsView = new ForumsView(Owner);
            forumsView.ShowDialog();
        }

        public bool CanExecute_ShowForumsCommand(object obj)
        {
            return true;
        }

        private async Task StartDemo() 
        {
            Style selectedButtonStyle = Application.Current.FindResource("OwnerSelectedButtonStyle") as Style;
            Style normalButtonStyle = Application.Current.FindResource("OwnerButtonStyle") as Style;

            if (!Demo)
            {
                CTS = new CancellationTokenSource();
                CancellationToken CT = CTS.Token;
                Demo = true;
                //demo message - start                   
                await Task.Delay(500, CT);
                Window messageDemoOn = new MessageView("The demo mode is on.", "Stop Demo Mode (Ctrl+Q)");
                messageDemoOn.Show();
                await Task.Delay(2500, CT);
                messageDemoOn.Close();
                await Task.Delay(1500, CT);

                //accommodations view           
                OwnerMainView.btnAccommodations.Style = selectedButtonStyle;
                await Task.Delay(1500, CT);
                OwnerMainView.btnAccommodations.Style = normalButtonStyle;
                Window accommodationsView = new AccommodationsView(Owner);
                accommodationsView.ShowDialog();
                await Task.Delay(1500, CT);

                //reservations view
                OwnerMainView.btnReservations.Style = selectedButtonStyle;
                await Task.Delay(1500, CT);
                OwnerMainView.btnReservations.Style = normalButtonStyle;
                Window reservationsView = new GuestReservationsView(Owner);
                reservationsView.ShowDialog();
                await Task.Delay(1500, CT);

                //demo message - end
                Window messageDemoOver = new MessageView("The demo mode is over.", "");
                messageDemoOver.Show();
                await Task.Delay(2500, CT);
                messageDemoOver.Close();

                Demo = false;
            }

        }

        public void Executed_StartDemoCommand(object obj)
        {
            try
            {
                StartDemo();              
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("Error!");
            }
            
        }

        public bool CanExecute_StartDemoCommand(object obj)
        {
            return true;
        }

        private async Task StopDemo()
        {
            Style normalButtonStyle = Application.Current.FindResource("OwnerButtonStyle") as Style;

            if (Demo)
            {
                CTS.Cancel();
                Demo = false;
                OwnerMainView.btnAccommodations.Style = normalButtonStyle;
                OwnerMainView.btnReservations.Style = normalButtonStyle;

                //demo message - end
                Window messageDemoOver = new MessageView("The demo mode is over.", "");
                messageDemoOver.Show();
                await Task.Delay(2500);
                messageDemoOver.Close();

            }
        }

        public void Executed_StopDemoCommand(object obj)
        {
            try
            {
                StopDemo();
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("Error!");
            }

        }

        public bool CanExecute_StopDemoCommand(object obj)
        {
            return true;
        }

        public void Executed_LogoutCommand(object obj)
        {
            
            foreach (Notification notification in _notificationService.GetUnreadByUserId(Owner.Id))
            {
                _notificationService.MarkAsRead(notification.Id);
            }
            

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
            ShowStatisticsCommand = new RelayCommand(Executed_ShowStatisticsCommand, CanExecute_ShowStatisticsCommand);
            ShowForumsCommand = new RelayCommand(Executed_ShowForumsCommand, CanExecute_ShowForumsCommand);
            StartDemoCommand = new RelayCommand(Executed_StartDemoCommand, CanExecute_StartDemoCommand);
            StopDemoCommand = new RelayCommand(Executed_StopDemoCommand, CanExecute_StopDemoCommand);
            LogoutCommand = new RelayCommand(Executed_LogoutCommand, CanExecute_LogoutCommand);
        }

    }
}
