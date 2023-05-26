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
    public class RescheduleRequestsViewModel
    {
        private readonly RescheduleRequestService _requestService;
        private readonly AccommodationReservationService _reservationService;
        private readonly NotificationService _notificationService;

        public RescheduleRequestsView RequestsView { get; set; }
        public Owner Owner { get; set; }    
        public ObservableCollection<RescheduleRequest> PendingRequests { get; set; }
        public RescheduleRequest SelectedRequest { get; set; }

        public RelayCommand ShowSelectedRequestCommand { get; set; }
        public RelayCommand CloseRescheduleRequestsViewCommand { get; set; }

        public RescheduleRequestsViewModel(RescheduleRequestsView requestsView, RescheduleRequestService rescheduleRequestService, 
            AccommodationReservationService reservationSevice, NotificationService notificationService, Owner owner)
        {
            InitCommands();

            _requestService = rescheduleRequestService;
            _reservationService = reservationSevice;
            _notificationService = notificationService;

            RequestsView = requestsView;
            Owner = owner;           
            PendingRequests = new ObservableCollection<RescheduleRequest>(_requestService.GetPendingByOwnerId(Owner.Id));
        }

        #region Commands
        public void Executed_ShowSelectedRequestCommand(object obj)
        {
            if (SelectedRequest != null)
            {
                Window requestHandlerView = new RequestHandlerView(this, _requestService, _reservationService, _notificationService, SelectedRequest);
                requestHandlerView.Show();
            }
            else
            {
                MessageBox.Show("No request has been selected");
            }
        }

        public bool CanExecute_ShowSelectedRequestCommand(object obj)
        {
            return true;
        }

        public void Executed_CloseRescheduleRequestsViewCommand(object obj)
        {
            RequestsView.Close();
        }

        public bool CanExecute_CloseRescheduleRequestsViewCommand(object obj)
        {
            return true;
        }
        #endregion

        public void InitCommands()
        {
            ShowSelectedRequestCommand = new RelayCommand(Executed_ShowSelectedRequestCommand, CanExecute_ShowSelectedRequestCommand);
            CloseRescheduleRequestsViewCommand = new RelayCommand(Executed_CloseRescheduleRequestsViewCommand, CanExecute_CloseRescheduleRequestsViewCommand);
        }

        public void UpdatePendingRequests()
        {
            PendingRequests.Clear();
            foreach (RescheduleRequest request in _requestService.GetPendingByOwnerId(Owner.Id))
            {
                PendingRequests.Add(request);
            }
        }
    }
}
