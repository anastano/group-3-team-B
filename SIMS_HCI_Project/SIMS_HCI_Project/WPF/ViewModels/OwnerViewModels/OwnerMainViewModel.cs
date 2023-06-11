using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;

using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Validations;
using SIMS_HCI_Project.WPF.Views.OwnerViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace SIMS_HCI_Project.WPF.ViewModels.OwnerViewModels
{
    public class OwnerMainViewModel : INotifyPropertyChanged
    {
        #region Service Fields
        private AccommodationReservationService _reservationService;
        private NotificationService _notificationService;
        private RatingGivenByOwnerService _ownerRatingService;
        private RatingGivenByGuestService _guestRatingService;
        private RescheduleRequestService _requstService;

        #endregion

        public static CancellationTokenSource CTS;
        public static bool Demo { get; set; }
        public OwnerMainView OwnerMainView { get; set; }
        public Owner Owner { get; set; }
        public ObservableCollection<AccommodationReservation> ReservationsInProgress { get; set; }
        public List<Notification> Notifications { get; set; }
        public int NotificationCount { get; set; }
        public Style NormalButtonStyle { get; set; }
        public Style SelectedButtonStyle { get; set; }       

        #region OnPropertyChanged

        private int _unratedGuestsCount;
        public int UnratedGuestsCount
        {
            get => _unratedGuestsCount;
            set
            {
                if (value != _unratedGuestsCount)
                {

                    _unratedGuestsCount = value;
                    OnPropertyChanged(nameof(UnratedGuestsCount));
                }
            }
        }

        private Style _accommodationButtonStyle;
        public Style AccommodationsButtonStyle
        {
            get => _accommodationButtonStyle;
            set
            {
                if (value != _accommodationButtonStyle)
                {

                    _accommodationButtonStyle = value;
                    OnPropertyChanged(nameof(AccommodationsButtonStyle));
                }
            }
        }

        private Style _reservationsButtonStyle;
        public Style ReservationsButtonStyle
        {
            get => _reservationsButtonStyle;
            set
            {
                if (value != _reservationsButtonStyle)
                {

                    _reservationsButtonStyle = value;
                    OnPropertyChanged(nameof(ReservationsButtonStyle));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

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
            Notifications = new List<Notification>(_notificationService.GetUnreadByUserId(Owner.Id));
            NotificationCount = Notifications.Count;
            UnratedGuestsCount = _ownerRatingService.GetUnratedReservations(Owner.Id, _reservationService).Count;
            
            NormalButtonStyle = Application.Current.FindResource("OwnerButtonStyle") as Style;
            SelectedButtonStyle = Application.Current.FindResource("OwnerSelectedButtonStyle") as Style;
            AccommodationsButtonStyle = NormalButtonStyle;
            ReservationsButtonStyle = NormalButtonStyle;

            CTS = new CancellationTokenSource();
        }

        public void LoadFromFiles()
        {
            _reservationService = new AccommodationReservationService();
            _notificationService = new NotificationService();
            _ownerRatingService = new RatingGivenByOwnerService();
            _guestRatingService = new RatingGivenByGuestService();
            _requstService = new RescheduleRequestService();

            _guestRatingService.FillAverageRatingAndSuperFlag(Owner);
        }

        public void UpdateNotifications()
        {
            UnratedGuestsCount = _ownerRatingService.GetUnratedReservations(Owner.Id, _reservationService).Count;
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
            Window unratedReservationsView = new UnratedReservationsView(this, Owner);
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
                AccommodationsButtonStyle = SelectedButtonStyle;
                await Task.Delay(1500, CT);
                AccommodationsButtonStyle = NormalButtonStyle;
                Window accommodationsView = new AccommodationsView(Owner);
                accommodationsView.ShowDialog();
                await Task.Delay(1500, CT);

                //reservations view
                ReservationsButtonStyle = SelectedButtonStyle;
                await Task.Delay(1500, CT);
                ReservationsButtonStyle = NormalButtonStyle;
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
                AccommodationsButtonStyle = normalButtonStyle;
                ReservationsButtonStyle = normalButtonStyle;

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
