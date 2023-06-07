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
    public class Wizard2ViewModel
    {
        public Guest2 Guest { get; set; }
        public NavigationService NavigationService { get; set; }
        public Wizard2View Wizard2View { get; set; }
        public RelayCommand NextCommand { get; set; }
        public RelayCommand PreviousCommand { get; set; }
        public RelayCommand ExitCommand { get; set; }
        public Wizard2ViewModel(Wizard2View wizard2View, Guest2 guest2, NavigationService navigationService)
        {
            Wizard2View = wizard2View;
            Guest = guest2;
            NavigationService = navigationService;
            InitCommands();

        }

        public void InitCommands()
        {
            NextCommand = new RelayCommand(ExecutedNext, CanExecute);
            ExitCommand = new RelayCommand(ExecutedExit, CanExecute);
            PreviousCommand = new RelayCommand(ExecutedPrevious, CanExecute);
        }

        public void ExecutedNext(object obj)
        {
            NavigationService.Navigate(new Wizard3View(Guest, NavigationService));
        }
        public void ExecutedPrevious(object obj)
        {
            NavigationService.Navigate(new Wizard1View(Guest, NavigationService));
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
