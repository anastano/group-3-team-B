using SIMS_HCI_Project.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using SIMS_HCI_Project.Applications.Services;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Views.Guest2Views;
using SIMS_HCI_Project.WPF.ViewModels.Guest2ViewModels;


namespace SIMS_HCI_Project.WPF.ViewModels.Guest2ViewModels
{
    public class CreateRegularRequestViewModel
    {
        public NavigationService NavigationService { get; set; }
        public Guest2 Guest { get; set; }
        public RelayCommand CompleteRequest { get; set; }
        public RelayCommand QuitRequest { get; set; }
        public CreateRegularRequestView CreateRegularRequestView { get; set; }


        public CreateRegularRequestViewModel(Guest2 guest, NavigationService navigationService, CreateRegularRequestView createRegularRequestView)
        {
            Guest = guest;
            NavigationService = navigationService;
            CreateRegularRequestView = createRegularRequestView;

            InitCommands();
            LoadFromFiles();

        }

        private void LoadFromFiles()
        {

        }

        private void InitCommands()
        {
            CompleteRequest = new RelayCommand(ExecuteCompleteRequest, CanExecute);
            QuitRequest = new RelayCommand(ExecuteQuitRequest, CanExecute);
        }

        private void ExecuteQuitRequest(object obj)
        {
            NavigationService.Navigate(new RequestsView(Guest, NavigationService));
        }

        private void ExecuteCompleteRequest(object obj)
        {

        }

        private bool CanExecute(object obj)
        {
            return true;
        }
    }
}
