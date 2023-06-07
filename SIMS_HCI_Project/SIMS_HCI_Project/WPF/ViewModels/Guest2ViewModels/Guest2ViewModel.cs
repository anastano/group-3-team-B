using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Views.Guest2Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace SIMS_HCI_Project.WPF.ViewModels.Guest2ViewModels      //main window with navigation
{
    public class Guest2ViewModel
    {
        public NavigationService NavigationService;
        public Guest2 Guest2 { get; set; }
        public Guest2View Guest2View { get; set; }

        public RelayCommand SearchAndReserve { get; set; }
        public RelayCommand RateTour { get; set; }
        public RelayCommand Requests { get; set; }
        public RelayCommand Statistics { get; set; }
        public RelayCommand Profile { get; set; }
        public RelayCommand Logout { get; set; }
        public bool IsWizardOn { get; set; }

        //public Guest2ViewModel()
        //{

        //}

        public Guest2ViewModel(NavigationService navigationService, Guest2 guest2, Guest2View guest2View)
        {
            NavigationService = navigationService;
            Guest2 = guest2;
            Guest2View = guest2View;

            TourReservationService tourReservationService = new TourReservationService();

            int reservationsNumber = tourReservationService.GetAllByGuestId(Guest2.Id).Count();

            if(reservationsNumber > 0)
            {
                //IsWizardOn = true;
                NavigationService.Navigate(new TourSearchView(Guest2, NavigationService));
            }
            else
            {
                NavigationService.Navigate(new Wizard1View(Guest2, NavigationService));
            }

            InitCommands();

        }

        public void InitCommands()
        {
            SearchAndReserve = new RelayCommand(ExecuteNavigateToSearchAndReserve, CanExecuteNavigate);
            RateTour = new RelayCommand(ExecuteNavigateToRateTour, CanExecuteNavigate);
            Requests = new RelayCommand(ExecuteNavigateToRequests, CanExecuteNavigate);
            Statistics = new RelayCommand(ExecuteNavigateToStatistics, CanExecuteNavigate);
            Profile = new RelayCommand(ExecuteNavigateToProfile, CanExecuteNavigate);
            Logout = new RelayCommand(ExecuteLogout, CanExecuteNavigate);
        }
        private void ExecuteNavigateToSearchAndReserve(object sender)
        {
            NavigationService.Navigate(new TourSearchView(Guest2, NavigationService));
        }
        private void ExecuteNavigateToRateTour(object sender)
        {
            NavigationService.Navigate(new TourRatingView(Guest2, NavigationService));
        }
        private void ExecuteNavigateToRequests(object sender)
        {
            NavigationService.Navigate(new RequestsView(Guest2, NavigationService));
        }
        private void ExecuteNavigateToStatistics(object sender)
        {
            NavigationService.Navigate(new StatisticsView(Guest2, NavigationService));
        }
        private void ExecuteNavigateToProfile(object sender)
        {
            //NavigationService.Navigate(new Guest2MainView(Guest2, NavigationService));
            NavigationService.Navigate(new ProfileView(Guest2, NavigationService));
        }
        private void ExecuteLogout(object sender)
        {
            Guest2View.Close();
        }
        private bool CanExecuteNavigate(object sender)
        {
            return true;
        }

    }
}
