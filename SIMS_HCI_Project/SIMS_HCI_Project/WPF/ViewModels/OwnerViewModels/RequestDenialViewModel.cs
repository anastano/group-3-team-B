using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Views.OwnerViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SIMS_HCI_Project.WPF.ViewModels.OwnerViewModels
{
    public class RequestDenialViewModel
    {
        private readonly RescheduleRequestService _requestService;
        private readonly NotificationService _notificationService;

        public RequestHandlerView RequestHandlerView { get; set; }
        public RequestDenialView RequestDenialView { get; set; }
        public RescheduleRequest Request { get; set; }
        
        public RelayCommand SubmitRequestDenialCommand { get; set; }

        public RequestDenialViewModel(RequestHandlerView requestHandlerView, RequestDenialView requestDenialView, RescheduleRequestService requestService, 
            NotificationService notificationService, RescheduleRequest selectedRequest) 
        {
            InitCommands();

            _requestService = requestService;
            _notificationService = notificationService;

            RequestHandlerView = requestHandlerView;
            RequestDenialView = requestDenialView;
            Request = selectedRequest;           
        }

        #region Commands
        public void Executed_SubmitRequestDenialCommand(object obj)
        {
            _requestService.EditStatus(Request.Id, RescheduleRequestStatus.DENIED);

            String Message = "Request to reschedule the reservation for '" + Request.AccommodationReservation.Accommodation.Name + "' has been DENIED";
            _notificationService.Add(new Notification(Message, Request.AccommodationReservation.GuestId, false));

            RequestHandlerView.Close();
            RequestDenialView.Close();
        }

        public bool CanExecute_SubmitRequestDenialCommand(object obj)
        {
            return true;
        }
        #endregion

        public void InitCommands()
        {
            SubmitRequestDenialCommand = new RelayCommand(Executed_SubmitRequestDenialCommand, CanExecute_SubmitRequestDenialCommand);
        }
    }
}
