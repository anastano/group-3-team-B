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

namespace SIMS_HCI_Project.WPF.ViewModels.Guest2ViewModels
{
    public class TourSuggestionsViewModel
    {
        public Location Location { get; set; }
        public Guest2 Guest2 { get; set; }
        public Tour SelectedTour { get; set; }
        public List<Tour> Tours { get; set; }

        private TourService _tourService = new TourService();
        public TourSuggestionsView TourSuggestionsView { get; set; }
        public NavigationService NavigationService { get; set; }

        public RelayCommand Reserve { get; set; }
        public TourSuggestionsViewModel(TourSuggestionsView tourSuggestionsView,  Location location, Guest2 guest, NavigationService navigationService)
        {
            TourSuggestionsView = tourSuggestionsView;
            Location = location;
            Guest2 = guest;
            NavigationService = navigationService;

            Tours = new List<Tour>(_tourService.SearchByLocation(Location));
            InitCommands();
        }

        public void InitCommands()
        {
            Reserve = new RelayCommand(ExecuteReserve, CanExecute);
        }

        public void ExecuteReserve(object sender)
        {
            NavigationService.Navigate(new SearchAndReserveView(Guest2, SelectedTour, NavigationService));
        }
        public bool CanExecute(object sender)
        {
            return true;
        }

    }
}
