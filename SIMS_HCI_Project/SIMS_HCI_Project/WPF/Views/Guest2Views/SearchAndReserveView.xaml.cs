using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.ViewModels.Guest2ViewModels;
using System.Windows.Navigation;



namespace SIMS_HCI_Project.WPF.Views.Guest2Views
{
    /// <summary>
    /// Interaction logic for SearchAndReserveView.xaml
    /// </summary>
    public partial class SearchAndReserveView : Page
    {

        public SearchAndReserveView(Guest2 guest2, Tour selectedTour, NavigationService navigationService)
        {

            InitializeComponent();

            TopFrame.Content =new TourReservationView(selectedTour, guest2);
            BottomFrame.Content = new TourSuggestionsView(selectedTour.Location, guest2, navigationService);
            this.DataContext = new SearchAndReserveViewModel( guest2, selectedTour, this, navigationService);
        }
    }
}
