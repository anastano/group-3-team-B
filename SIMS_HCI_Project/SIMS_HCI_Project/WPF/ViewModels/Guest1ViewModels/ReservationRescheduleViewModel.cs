using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Controller;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Views.Guest1Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SIMS_HCI_Project.WPF.ViewModels.Guest1ViewModels
{
    internal class ReservationRescheduleViewModel : INotifyPropertyChanged
    {
        private AccommodationReservationService _accommodationReservationService;
        private RescheduleRequestService _rescheduleRequestService;
        public ReservationRescheduleView ReservationRescheduleView { get; set; }
        public Guest1MainView Guest1MainView { get; set; }
        public ReservationsView ReservationsView { get; set; }
        public AccommodationReservation Reservation { get; set; }
        public ObservableCollection<RescheduleRequest> RescheduleRequests { get; set; }
        public RelayCommand SendReservationRescheduleRequestCommand { get; set; }

        private DateTime _wantedStart;
        public DateTime WantedStart
        {
            get => _wantedStart;
            set
            {
                if (value != _wantedStart)
                {
                    _wantedStart = value;
                    OnPropertyChanged();
                }
            }
        }
        private DateTime _wantedEnd;
        public DateTime WantedEnd
        {
            get => _wantedEnd;
            set
            {
                if (value != _wantedEnd)
                {
                    _wantedEnd = value;
                    OnPropertyChanged();
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ReservationRescheduleViewModel(Guest1MainView guest1MainView, ReservationsView reservationsView, ReservationRescheduleView reservationRescheduleView, AccommodationReservationService reservationService, AccommodationReservation reservation)
        {
            _accommodationReservationService = reservationService;
            _rescheduleRequestService = new RescheduleRequestService();   //samo mi ovdje treba bezveze da ga ne prosledjujem
            ReservationRescheduleView = reservationRescheduleView;
            Guest1MainView = guest1MainView;
            ReservationsView = reservationsView;
            Reservation = reservation;
            RescheduleRequests = new ObservableCollection<RescheduleRequest>(_rescheduleRequestService.GetAllByOwnerId(Reservation.Accommodation.OwnerId));
            InitCommands();
        }
        public void Executed_SendReservationRescheduleRequestCommand(object obj)
        {
            MessageBoxResult result = ConfirmRescheduleRequest();
            if (result == MessageBoxResult.Yes)
            {
                _rescheduleRequestService.Add(new RescheduleRequest(Reservation, WantedStart, WantedEnd));
                Guest1MainView.MainGuestFrame.Content = ReservationsView;

            }
        }
        private MessageBoxResult ConfirmRescheduleRequest()
        {
            string sMessageBoxText = $"This reservation will be rescheduled, are you sure?";
            string sCaption = "Reschedule Confirm";

            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

            MessageBoxResult result = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
            return result;
        }

        public bool CanExecute_SendReservationRescheduleRequestCommand(object obj)
        {
            return true;
        }
        public void InitCommands()
        {
            SendReservationRescheduleRequestCommand = new RelayCommand(Executed_SendReservationRescheduleRequestCommand, CanExecute_SendReservationRescheduleRequestCommand);
        }        
    }
}
