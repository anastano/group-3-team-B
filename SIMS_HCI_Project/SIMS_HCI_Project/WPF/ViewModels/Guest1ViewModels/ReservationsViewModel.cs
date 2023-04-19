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

namespace SIMS_HCI_Project.WPF.ViewModels.Guest1ViewModels
{
    internal class ReservationsViewModel : IObserver
    {
        private readonly AccommodationReservationService _reservationService;
        private readonly NotificationService _notificationService;
        public ReservationsView ReservationsView { get; set; }
        public Guest1MainView Guest1MainView { get; set; }

        public Guest1 Guest { get; set; }
        public ObservableCollection<AccommodationReservation> ActiveReservations { get; set; }
        public ObservableCollection<AccommodationReservation> PastReservations { get; set; }
        public ObservableCollection<AccommodationReservation> CanceledReservations { get; set; }
        public AccommodationReservation SelectedReservation { get; set; }

        public RelayCommand CancelReservationCommand { get; set; }
        public RelayCommand RescheduleReservationCommand { get; set; }
        public RelayCommand RateCommand { get; set; }

        private Frame _frame;
        public Frame Frame
        {
            get { return _frame; }
            set { _frame = value; }
        }

        public ReservationsViewModel(Frame currentFrame, AccommodationReservationService reseravtionService, Guest1 guest)
        {
            InitCommands();

            _reservationService = reseravtionService;
            _notificationService = new NotificationService();
            this.Frame = currentFrame;
            Guest = guest;
            ActiveReservations = new ObservableCollection<AccommodationReservation>(_reservationService.GetAllByStatusAndGuestId(Guest.Id, AccommodationReservationStatus.RESERVED));
            AddRescheduledReservations();
            PastReservations = new ObservableCollection<AccommodationReservation>(_reservationService.GetAllByStatusAndGuestId(Guest.Id, AccommodationReservationStatus.COMPLETED));
            CanceledReservations = new ObservableCollection<AccommodationReservation>(_reservationService.GetAllByStatusAndGuestId(Guest.Id,AccommodationReservationStatus.CANCELLED));
            _reservationService.Subscribe(this);
        }

        #region Commands
        public void Executed_CancelReservationCommand(object obj)
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
                this.Frame.Navigate(new ReservationRescheduleView(_reservationService, SelectedReservation));
            }
        }
        public void ExecutedRateCommand(object obj)
        {
            if (SelectedReservation != null)
            {
                this.Frame.Navigate(new RatingReservationView(_reservationService, SelectedReservation));  
            }
        }
        public bool CanExecute(object obj)
        {
            return true;
        }
        #endregion

        public void InitCommands()
        {
            CancelReservationCommand = new RelayCommand(Executed_CancelReservationCommand, CanExecute);
            RescheduleReservationCommand = new RelayCommand(ExecutedRescheduleReservationCommand, CanExecute);
            RateCommand = new RelayCommand(ExecutedRateCommand, CanExecute);
        }
        
        public void Update()
        {
            UpdateReservations();
        }
        
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
