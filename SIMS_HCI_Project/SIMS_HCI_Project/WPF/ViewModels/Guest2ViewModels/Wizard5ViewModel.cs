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
    public class Wizard5ViewModel
    {
        public Guest2 Guest { get; set; }
        public NavigationService NavigationService { get; set; }
        public Wizard5View Wizard5View { get; set; }
        public RelayCommand NextCommand { get; set; }
        public RelayCommand PreviousCommand { get; set; }
        public RelayCommand ExitCommand { get; set; }
        public Tour SelectedTour { get; set; }
        public Wizard5ViewModel(Wizard5View wizard5View, Guest2 guest2, NavigationService navigationService, Tour selectedTour)
        {
            Wizard5View = wizard5View;
            Guest = guest2;
            NavigationService = navigationService;
            SelectedTour = selectedTour;
            InitCommands();

        }

        public void InitCommands()
        {
            ExitCommand = new RelayCommand(ExecutedExit, CanExecute);
            PreviousCommand = new RelayCommand(ExecutedPrevious, CanExecute);
        }
        
        public void ExecutedPrevious(object obj)
        {
            NavigationService.Navigate(new Wizard3View(Guest, NavigationService, SelectedTour));
        }
        public void ExecutedExit(object obj)
        {
            NavigationService.Navigate(new TourSearchView(Guest, NavigationService));
        }

        public bool CanExecute(object obj)
        {
            return true;
        }
    }
}
