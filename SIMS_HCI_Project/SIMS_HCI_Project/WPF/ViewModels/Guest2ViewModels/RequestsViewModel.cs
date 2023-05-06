using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Views.Guest2Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace SIMS_HCI_Project.WPF.ViewModels.Guest2ViewModels
{
    public class RequestsViewModel
    {
        public NavigationService NavigationService;
        public Guest2 Guest2 { get; set; }
        public RequestsView RequestsView { get; set; }
        public RelayCommand Help { get; set; }


        public RequestsViewModel(Guest2 guest2, NavigationService navigationService, RequestsView requestsView)
        {
            Guest2 = guest2;
            NavigationService = navigationService;
            RequestsView = requestsView;

            InitCommands();
        }

        public void InitCommands()
        {
            //Help
        }
    }
}
