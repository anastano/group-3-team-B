using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Views.Guest2Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace SIMS_HCI_Project.WPF.ViewModels.Guest2ViewModels
{
    public class SearchAndReserveViewModel
    {
        public NavigationService NavigationService { get; set; }
        public Guest2 Guest { get; set; }
        public SearchAndReserveView SearchAndReserveView { get; set; }
        public RelayCommand BackCommand { get; set; }
        public RelayCommand HelpCommand { get; set; }
        
        public Tour SelectedTour { get; set; }
        public SearchAndReserveViewModel( Guest2 guest, Tour selectedTour, SearchAndReserveView searchAndReserveView, NavigationService navigationService)
        {
            Guest = guest;
            SelectedTour = selectedTour;
            SearchAndReserveView = searchAndReserveView;
            NavigationService = navigationService;
            
            InitCommands();

        }

        public void InitCommands()
        {
            BackCommand = new RelayCommand(ExecuteBack, CanExecute);
            //HelpCommand
        }

        public void ExecuteBack(object sender)
        {
            //NavigationService.Navigate(new TourSearchView(Guest, NavigationService));
            NavigationService.GoBack(); //because of notifications details

        }
        public bool CanExecute(object sender)
        {
            return true;
        }
    }
}
