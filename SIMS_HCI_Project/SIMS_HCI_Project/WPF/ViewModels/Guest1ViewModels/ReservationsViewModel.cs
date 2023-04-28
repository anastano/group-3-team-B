using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Controller;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Views.Guest1Views;
using SIMS_HCI_Project.WPF.Views.OwnerViews;
using System;
using SIMS_HCI_Project.Observer;
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

namespace SIMS_HCI_Project.WPF.ViewModels.Guest1ViewModels
{
    internal class ReservationsViewModel : IObserver, INotifyPropertyChanged
    {
        private readonly AccommodationReservationService _reservationService;
        private readonly NotificationService _notificationService;
        private RatingReservationViewModel _ratingReservationViewModel;
        private ReservationRescheduleViewModel _reservationRescheduleViewModel;
        public Guest1 Guest { get; set; }
        public ObservableCollection<AccommodationReservation> ActiveReservations { get; set; }
        public ObservableCollection<AccommodationReservation> PastReservations { get; set; }
        public ObservableCollection<AccommodationReservation> CanceledReservations { get; set; }
        public ObservableCollection<AccommodationReservation> Reservations { get; set; }
        public AccommodationReservation SelectedReservation { get; set; }

        public RelayCommand CancelReservationCommand { get; set; }
        public RelayCommand RescheduleReservationCommand { get; set; }
        public RelayCommand RateCommand { get; set; }

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

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ReservationsViewModel(Guest1 guest)
        {
            _reservationService = new AccommodationReservationService();
            _notificationService = new NotificationService();
            Guest = guest;
            ActiveReservations = new ObservableCollection<AccommodationReservation>(_reservationService.GetAllByStatusAndGuestId(Guest.Id, AccommodationReservationStatus.RESERVED));
            AddRescheduledReservations();
            PastReservations = new ObservableCollection<AccommodationReservation>(_reservationService.GetAllByStatusAndGuestId(Guest.Id, AccommodationReservationStatus.COMPLETED));
            CanceledReservations = new ObservableCollection<AccommodationReservation>(_reservationService.GetAllByStatusAndGuestId(Guest.Id,AccommodationReservationStatus.CANCELLED));
            Reservations = new ObservableCollection<AccommodationReservation>(_reservationService.GetByGuestId(Guest.Id));
            _reservationService.Subscribe(this);
            InitCommands();
            CanceledReservations.CollectionChanged += (s, e) => UpdateChart();
            Reservations.CollectionChanged += (s, e) => UpdateChart();
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
                _reservationRescheduleViewModel = new ReservationRescheduleViewModel(SelectedReservation);
                _reservationRescheduleViewModel.Closed += UnloadUserControl;
                CurrentViewModel = _reservationRescheduleViewModel;
            }
        }
        public void ExecutedRateCommand(object obj)
        {
            if (SelectedReservation != null)
            {
                _ratingReservationViewModel = new RatingReservationViewModel(SelectedReservation);
                _ratingReservationViewModel.Closed += UnloadUserControl;
                CurrentViewModel = _ratingReservationViewModel;
            }
        }
        private void UnloadUserControl(object sender, EventArgs e)
        {
            CurrentViewModel = new ReservationsViewModel(Guest);
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
        }
        
        public void Update()
        {
            UpdateReservations();
        }
        /*private void Image_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Image image = sender as Image;
            

        }*/

        public void UpdateReservations()
        {
            ActiveReservations.Clear();
            foreach (AccommodationReservation reservation in _reservationService.GetAllByStatusAndGuestId(Guest.Id, AccommodationReservationStatus.RESERVED))
            {
                ActiveReservations.Add(reservation);
            }
            AddRescheduledReservations();
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

        private void AddRescheduledReservations()
        {
            foreach (AccommodationReservation reservation in _reservationService.GetAllByStatusAndGuestId(Guest.Id, AccommodationReservationStatus.RESCHEDULED))
            {
                if (_reservationService.IsReservationActive(reservation))
                {
                    ActiveReservations.Add(reservation);
                }
            }
        }
    }
}
