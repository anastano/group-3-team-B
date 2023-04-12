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
        public ObservableCollection<AccommodationReservation> CanceledReservations { get; set; }  //TO DO DODATI TAJ TAB
        public AccommodationReservation SelectedReservation { get; set; }

        public RelayCommand CancelReservationCommand { get; set; }
        public RelayCommand RescheduleReservationCommand { get; set; }
        public RelayCommand RateCommand { get; set; }

        public ReservationsViewModel(ReservationsView reservationsView, Guest1MainView guest1MainView, AccommodationReservationService reseravtionService, NotificationService notificationService, Guest1 guest)
        {
            InitCommands();

            _reservationService = reseravtionService;
            _notificationService = notificationService;
            ReservationsView = reservationsView;
            Guest1MainView = guest1MainView;
            Guest = guest;
            ActiveReservations = new ObservableCollection<AccommodationReservation>(_reservationService.GetAllByStatusAndGuestId(Guest.Id, AccommodationReservationStatus.RESERVED));
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
                    String Message = "Reservation for " + SelectedReservation.Accommodation.Name + " with id: " + SelectedReservation.Id + " has been cancelled";
                    _notificationService.Add(new Notification(Message, SelectedReservation.Accommodation.OwnerId, false));
                    _reservationService.EditStatus(SelectedReservation.Id, AccommodationReservationStatus.CANCELLED);
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

        public bool CanExecute_CancelReservationCommand(object obj)
        {
            return true;
        }
        public void Executed_RescheduleReservationCommand(object obj)
        {
            if (SelectedReservation != null)
            {
                //Potencijalno ako bude trebalo zbog selected
                Guest1MainView.MainGuestFrame.Content = new ReservationRescheduleView(Guest1MainView, ReservationsView, _reservationService, SelectedReservation);
            }
        }

        public bool CanExecute_RescheduleReservationCommand(object obj)
        {
            return true;
        }
        public void Executed_RateCommand(object obj)
        {
            if (SelectedReservation != null)
            {
                //Potencijalno ako bude trebalo zbog selected
                Guest1MainView.MainGuestFrame.Content = new RatingReservationView(Guest1MainView, ReservationsView, _reservationService, SelectedReservation);
            }
        }
        public bool CanExecute_RateCommand(object obj)
        {
            return true;
        }
        #endregion

        public void InitCommands()
        {
            CancelReservationCommand = new RelayCommand(Executed_CancelReservationCommand, CanExecute_CancelReservationCommand);
            RescheduleReservationCommand = new RelayCommand(Executed_RescheduleReservationCommand, CanExecute_RescheduleReservationCommand);
            RateCommand = new RelayCommand(Executed_RateCommand, CanExecute_RateCommand);
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
