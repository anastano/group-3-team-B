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
    public class ProfileNotificationsViewModel
    {
        public Guest2 Guest { get; set; }
        public NavigationService NavigationService { get; set; }
        public Frame ProfileFrame { get; set; }

        public RelayCommand BackCommand { get; set; }
        public RelayCommand HelpCommand { get; set; }

        public ProfileNotificationsViewModel(Guest2 guest, NavigationService navigationService, Frame profileFrame)
        {
            Guest = guest;
            NavigationService = navigationService;
            ProfileFrame = profileFrame;
            InitCommands();
        }
        //napravi fju da boja elemenata koji su procitani bude druhacija od onih koje nisu
        private void InitCommands()
        {
            BackCommand = new RelayCommand(ExecuteBackCommand, CanExecute);
            //HelpCommand todo

        }

        private void ExecuteBackCommand(object obj)
        {
            ProfileFrame.NavigationService.Navigate(new ProfileMainView(Guest, NavigationService, ProfileFrame));
        }

        private bool CanExecute(object obj)
        {
            return true;
        }
    }
}
