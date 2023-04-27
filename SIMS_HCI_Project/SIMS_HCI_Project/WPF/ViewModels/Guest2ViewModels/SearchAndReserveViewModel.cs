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
        private NavigationService NavigationService;
        public Guest2 Guest2 { get; set; }
        public SearchAndReserveView SearchAndReserveView { get; set; }
        public RelayCommand Back { get; set; }
        public RelayCommand Help { get; set; }
        
        public Tour SelectedTour { get; set; }
        public SearchAndReserveViewModel( Guest2 guest2, Tour selectedTour, SearchAndReserveView searchAndReserveView, NavigationService navigationService)
        {
            Guest2 = guest2;
            SelectedTour = selectedTour;
            SearchAndReserveView = searchAndReserveView;
            NavigationService = navigationService;
            
            InitCommands();

        }

        public void InitCommands()
        {
            Back = new RelayCommand(ExecuteBack, CanExecute);
        }

        public void ExecuteBack(object sender)
        {
            NavigationService.GoBack();
        }
        public bool CanExecute(object sender)
        {
            return true;
        }
    }
}
