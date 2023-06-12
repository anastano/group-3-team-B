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
    public class DeclineRequestViewModel
    {
        private readonly RescheduleRequestService _requestService;
        private readonly NotificationService _notificationService;
        public RequestView RequestView { get; set; }
        public DeclineRequestView DeclineRequestView { get; set; }
        public RequestsViewModel RequestsVM { get; set; }
        public RescheduleRequest Request { get; set; } 
        public RelayCommand SubmitCommand { get; set; }
        public RelayCommand CloseViewCommand { get; set; }
        

        public DeclineRequestViewModel(RequestView requestView, DeclineRequestView declineRequestView, RequestsViewModel requestsVM, RescheduleRequest request) 
        {
            InitCommands();

            _requestService = new RescheduleRequestService();
            _notificationService = new NotificationService();

            RequestView = requestView;
            DeclineRequestView = declineRequestView;
            RequestsVM = requestsVM;
            Request = request;           
        }

        #region Commands
        public void Executed_SubmitCommand(object obj)
        {
            _requestService.EditStatus(Request.Id, RescheduleRequestStatus.DENIED);

            String Message = "Request to reschedule the reservation for '" + Request.AccommodationReservation.Accommodation.Name + "' has been DENIED";
            _notificationService.Add(new Notification(Message, Request.AccommodationReservation.GuestId, false));

            RequestView.Close();
            DeclineRequestView.Close();
            RequestsVM.UpdatePendingRequests();
        }

        public void Executed_CloseViewCommand(object obj)
        {
            DeclineRequestView.Close();
        }

        #endregion

        public void InitCommands()
        {
            SubmitCommand = new RelayCommand(Executed_SubmitCommand);
            CloseViewCommand = new RelayCommand(Executed_CloseViewCommand);
        }
    }
}
