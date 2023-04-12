using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
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
    public class RequestHandlerViewModel
    {
        private readonly RescheduleRequestService _requestService;
        private readonly AccommodationReservationService _reservationService;
        private readonly NotificationService _notificationService;

        public RequestHandlerView RequestHandlerView { get; set; }
        public RescheduleRequest Request { get; set; }
        public ObservableCollection<AccommodationReservation> OverlappingReservations { get; set; }

        public RelayCommand AcceptRequestCommand { get; set; }
        public RelayCommand DeclineRequestCommand { get; set; }

        public RequestHandlerViewModel(RequestHandlerView requestHandlerView, RescheduleRequestService requestService, AccommodationReservationService reservationService, NotificationService notificationService, RescheduleRequest selectedRequest) 
        {
            InitCommands();

            _requestService = requestService;
            _reservationService = reservationService;
            _notificationService = notificationService;

            RequestHandlerView = requestHandlerView;
            Request = selectedRequest;
            
            OverlappingReservations = new ObservableCollection<AccommodationReservation>(_reservationService.GetOverlappingReservations(Request));

            ShowTextBox();
        }


            #region Commands
            public void Executed_AcceptRequestCommand(object obj)
        {
            if (OverlappingReservations != null)
            {
                foreach (AccommodationReservation reservation in OverlappingReservations)
                {
                    _reservationService.EditStatus(reservation.Id, AccommodationReservationStatus.CANCELLED);
                }
            }

            _reservationService.RescheduleReservation(Request);
            _requestService.EditStatus(Request.Id, RescheduleRequestStatus.ACCEPTED);

            String Message = "Request to reschedule the reservation for '" + Request.AccommodationReservation.Accommodation.Name + "' has been ACCEPTED";
            _notificationService.Add(new Notification(Message, Request.AccommodationReservation.GuestId, false));

            RequestHandlerView.Close();           
        }

        public bool CanExecute_AcceptRequestCommand(object obj)
        {
            return true;
        }

        public void Executed_DeclineRequestCommand(object obj)
        {
            Window requestDenialView = new RequestDenialView(RequestHandlerView, _requestService, _notificationService, Request);
            requestDenialView.Show();
        }

        public bool CanExecute_DeclineRequestCommand(object obj)
        {
            return true;
        }
        #endregion

        public void InitCommands()
        {
            AcceptRequestCommand = new RelayCommand(Executed_AcceptRequestCommand, CanExecute_AcceptRequestCommand);
            DeclineRequestCommand = new RelayCommand(Executed_DeclineRequestCommand, CanExecute_DeclineRequestCommand);
        }

        public void ShowTextBox()
        {
            int overlappingReservations = _reservationService.GetOverlappingReservations(Request).Count;
            if (overlappingReservations != 0)
            {
                RequestHandlerView.txtOverlappingReservations.Text = "There are reservations on those days: ";
            }
            else
            {
                RequestHandlerView.txtOverlappingReservations.Text = "There are not any reservations on those days.";
            }

        }
    }
}
