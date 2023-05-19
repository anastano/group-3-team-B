using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Controller;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Views.Guest1Views;
using SIMS_HCI_Project.WPF.Views.OwnerViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AccommodationReservation = SIMS_HCI_Project.Domain.Models.AccommodationReservation;
using System.Windows.Controls;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using LiveCharts;
using LiveCharts.Wpf;
using SIMS_HCI_Project.WPF.Services;

namespace SIMS_HCI_Project.WPF.ViewModels.Guest1ViewModels
{
    internal class ReservationsViewModel: INotifyPropertyChanged
    {
        private NavigationService _navigationService;
        private readonly AccommodationReservationService _reservationService;
        private readonly NotificationService _notificationService;
        public Guest1 Guest { get; set; }
        public ObservableCollection<AccommodationReservation> ActiveReservations { get; set; }
        public ObservableCollection<AccommodationReservation> PastReservations { get; set; }
        public ObservableCollection<AccommodationReservation> CanceledReservations { get; set; }
        public ObservableCollection<AccommodationReservation> Reservations { get; set; }
        public AccommodationReservation SelectedReservation { get; set; }

        public RelayCommand CancelReservationCommand { get; set; }
        public RelayCommand RescheduleReservationCommand { get; set; }
        public RelayCommand RateCommand { get; set; }
        public RelayCommand ShowImagesCommand { get; set; }
        private ChartValues<int> _cancelledCount;
        public ChartValues<int> CancelledCount

        {
            get => new ChartValues<int> { CanceledReservations.Count };  
            set
            {
                if (value != _cancelledCount)
                {
                    _cancelledCount = value;
                    OnPropertyChanged();
                }
            }
        }
        private ChartValues<int> _reservationsCount;
        public ChartValues<int> ReservationsCount

        {
            get => new ChartValues<int> { Reservations.Count };
            set
            {
                if (value != _reservationsCount)
                {
                    _reservationsCount = value;
                    OnPropertyChanged();
                }
            }
        }
        private  int _selectedTabIndex;
        public  int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set
            {
                if (value != _selectedTabIndex)
                {
                    _selectedTabIndex = value;
                    OnPropertyChanged();
                }
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ReservationsViewModel(Guest1 guest, NavigationService navigationService, int selectedTabIndex)
        {
            _navigationService = navigationService;
            _reservationService = new AccommodationReservationService();
            _notificationService = new NotificationService();
            Guest = guest;
            ActiveReservations = new ObservableCollection<AccommodationReservation>(_reservationService.GetAllByStatusAndGuestId(Guest.Id, AccommodationReservationStatus.RESERVED));
            PastReservations = new ObservableCollection<AccommodationReservation>(_reservationService.GetAllByStatusAndGuestId(Guest.Id, AccommodationReservationStatus.COMPLETED));
            CanceledReservations = new ObservableCollection<AccommodationReservation>(_reservationService.GetAllByStatusAndGuestId(Guest.Id,AccommodationReservationStatus.CANCELLED));
            Reservations = new ObservableCollection<AccommodationReservation>(_reservationService.GetByGuestId(Guest.Id));
            InitCommands();
            CanceledReservations.CollectionChanged += (s, e) => UpdateChart();
            Reservations.CollectionChanged += (s, e) => UpdateChart();
            SelectedTabIndex = selectedTabIndex;
            
        }
        private void UpdateChart()
        {
            CancelledCount = new ChartValues<int> { CanceledReservations.Count };
            ReservationsCount = new ChartValues<int> { Reservations.Count };
        }
        #region Commands
        public void ExecutedCancelReservationCommand(object obj)
        {
            if (SelectedReservation != null)
            {
                MessageBoxResult result = ConfirmCancellation();
                if (result == MessageBoxResult.Yes)
                {
                    _reservationService.CancelReservation(_notificationService, SelectedReservation);
                    Update();
                }
            }
        }
        private MessageBoxResult ConfirmCancellation()
        {
            string sMessageBoxText = $"This reservation will be cancelled, are you sure?";
            string sCaption = "Cancellation Confirm";

            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

            MessageBoxResult result = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
            return result;
        }
        public void ExecutedRescheduleReservationCommand(object obj)
        {
            if (SelectedReservation != null)
            {
                _navigationService.Navigate(new ReservationRescheduleViewModel(_navigationService ,SelectedReservation), "Reschedule reservation");
            }
        }
        public void ExecutedRateCommand(object obj)
        {
            if (SelectedReservation != null)
            {
                _navigationService.Navigate(new RatingReservationViewModel(SelectedReservation, _navigationService), "Review");
            }
        }
        public void ExecutedShowImagesCommand(object obj)
        {
            if (SelectedReservation != null)
            {
                _navigationService.Navigate(new AccommodationImagesViewModel(SelectedReservation.Accommodation, _navigationService), "Images");
            }
        }
        public bool CanExecute(object obj)
        {
            return true;
        }
        #endregion
        public void InitCommands()
        {
            CancelReservationCommand = new RelayCommand(ExecutedCancelReservationCommand, CanExecute);
            RescheduleReservationCommand = new RelayCommand(ExecutedRescheduleReservationCommand, CanExecute);
            RateCommand = new RelayCommand(ExecutedRateCommand, CanExecute);
            ShowImagesCommand = new RelayCommand(ExecutedShowImagesCommand, CanExecute);
        }
        public void Update()
        {
            UpdateReservations();
            UpdateChart();
        }
        public void UpdateReservations()
        {
            ActiveReservations.Clear();
            foreach (AccommodationReservation reservation in _reservationService.GetAllByStatusAndGuestId(Guest.Id, AccommodationReservationStatus.RESERVED))
            {
                ActiveReservations.Add(reservation);
            }
            PastReservations.Clear();
            foreach (AccommodationReservation reservation in _reservationService.GetAllByStatusAndGuestId(Guest.Id, AccommodationReservationStatus.COMPLETED))
            {
                PastReservations.Add(reservation);
            }

            CanceledReservations.Clear();
            foreach (AccommodationReservation reservation in _reservationService.GetAllByStatusAndGuestId(Guest.Id, AccommodationReservationStatus.CANCELLED))
            {
                CanceledReservations.Add(reservation);
            }
        }
    }
}
