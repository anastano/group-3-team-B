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
        public RescheduleRequestsViewModel RequestsVM { get; set; }
        public RescheduleRequest Request { get; set; }
        public ObservableCollection<AccommodationReservation> OverlappingReservations { get; set; }

        public RelayCommand AcceptRequestCommand { get; set; }
        public RelayCommand DeclineRequestCommand { get; set; }
        public RelayCommand CloseRequestHandlerViewCommand { get; set; }

        public RequestHandlerViewModel(RequestHandlerView requestHandlerView, RescheduleRequestsViewModel requestsVM, RescheduleRequestService requestService, 
            AccommodationReservationService reservationService, NotificationService notificationService, RescheduleRequest selectedRequest) 
        {
            InitCommands();

            _requestService = requestService;
            _reservationService = reservationService;
            _notificationService = notificationService;

            RequestHandlerView = requestHandlerView;
            RequestsVM = requestsVM;
            Request = selectedRequest;
            
            OverlappingReservations = new ObservableCollection<AccommodationReservation>(_requestService.GetOverlappingReservations(Request, _reservationService));

            ShowTextBox();
        }


        #region Commands

        private MessageBoxResult ConfirmAcceptRequest()
        {
            string sMessageBoxText = $"Are you sure you want to accept this request?";
            string sCaption = "Accept Request Confirmation";

            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

            MessageBoxResult result = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
            return result;
        }

        public void Executed_AcceptRequestCommand(object obj)
        {
            if (ConfirmAcceptRequest() == MessageBoxResult.Yes)
            {
                if (OverlappingReservations != null)
                {
                    foreach (AccommodationReservation reservation in OverlappingReservations)
                    {
                        _reservationService.EditStatus(reservation.Id, AccommodationReservationStatus.CANCELLED);
                    }
                }

                _reservationService.EditReservation(Request);
                _requestService.EditStatus(Request.Id, RescheduleRequestStatus.ACCEPTED);

                String Message = "Request to reschedule the reservation for '" + Request.AccommodationReservation.Accommodation.Name + "' has been ACCEPTED";
                _notificationService.Add(new Notification(Message, Request.AccommodationReservation.GuestId, false));

                RequestHandlerView.Close();
                RequestsVM.UpdatePendingRequests();
            }
        }

        public bool CanExecute_AcceptRequestCommand(object obj)
        {
            return true;
        }

        public void Executed_DeclineRequestCommand(object obj)
        {
            Window requestDenialView = new RequestDenialView(RequestHandlerView, RequestsVM, _requestService, _notificationService, Request);
            requestDenialView.Show();
        }

        public bool CanExecute_DeclineRequestCommand(object obj)
        {
            return true;
        }

        public void Executed_CloseRequestHandlerViewCommand(object obj)
        {
            RequestHandlerView.Close();
        }

        public bool CanExecute_CloseRequestHandlerViewCommand(object obj)
        {
            return true;
        }

        #endregion

        public void InitCommands()
        {
            AcceptRequestCommand = new RelayCommand(Executed_AcceptRequestCommand, CanExecute_AcceptRequestCommand);
            DeclineRequestCommand = new RelayCommand(Executed_DeclineRequestCommand, CanExecute_DeclineRequestCommand);
            CloseRequestHandlerViewCommand = new RelayCommand(Executed_CloseRequestHandlerViewCommand, CanExecute_CloseRequestHandlerViewCommand);
        }

        public void ShowTextBox()
        {
            int overlappingReservations = _requestService.GetOverlappingReservations(Request, _reservationService).Count;
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
