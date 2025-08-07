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
    public class RequestsViewModel
    {
        private readonly RescheduleRequestService _requestService;
        public RequestsView RequestsView { get; set; }
        public Owner Owner { get; set; }    
        public ObservableCollection<RescheduleRequest> PendingRequests { get; set; }
        public RescheduleRequest SelectedRequest { get; set; }
        public RelayCommand ShowRequestCommand { get; set; }
        public RelayCommand CloseViewCommand { get; set; }

        public RequestsViewModel(RequestsView requestsView, Owner owner)
        {
            InitCommands();

            _requestService = new RescheduleRequestService();

            RequestsView = requestsView;
            Owner = owner;           
            PendingRequests = new ObservableCollection<RescheduleRequest>(_requestService.GetPendingByOwnerId(Owner.Id));
        }

        #region Commands
        public void Executed_ShowRequestCommand(object obj)
        {
            if (SelectedRequest != null)
            {
                Window requestHandlerView = new RequestView(this, SelectedRequest);
                requestHandlerView.Show();
            }
            else
            {
                MessageBox.Show("No request has been selected");
            }
        }

        public void Executed_CloseViewCommand(object obj)
        {
            RequestsView.Close();
        }

        #endregion

        public void InitCommands()
        {
            ShowRequestCommand = new RelayCommand(Executed_ShowRequestCommand);
            CloseViewCommand = new RelayCommand(Executed_CloseViewCommand);
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
