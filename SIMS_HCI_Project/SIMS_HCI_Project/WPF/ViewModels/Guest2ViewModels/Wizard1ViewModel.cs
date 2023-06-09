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
    public class Wizard1ViewModel
    {
        public Guest2 Guest { get; set; }
        public NavigationService NavigationService { get; set; }
        public Wizard1View Wizard1View { get; set; }
        public RelayCommand NextCommand { get; set; }
        public RelayCommand PreviousCommand { get; set; }
        public RelayCommand ExitCommand { get; set; }
        public Wizard1ViewModel(Wizard1View wizard1View, Guest2 guest2, NavigationService navigationService)
        {
            Wizard1View = wizard1View;
            Guest = guest2;
            NavigationService = navigationService;
            InitCommands();

        }

        public void InitCommands()
        {
            NextCommand = new RelayCommand(ExecutedNext, CanExecute);
            ExitCommand = new RelayCommand(ExecutedExit, CanExecute);
        }

        public void ExecutedNext(object obj)
        {
            NavigationService.Navigate(new Wizard2View(Guest, NavigationService));
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
