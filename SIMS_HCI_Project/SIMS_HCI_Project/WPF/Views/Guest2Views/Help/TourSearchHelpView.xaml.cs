using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
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

namespace SIMS_HCI_Project.WPF.Views.Guest2Views.Help
{
    /// <summary>
    /// Interaction logic for TourSearchHelpView.xaml
    /// </summary>
    public partial class TourSearchHelpView : Page
    {
        public NavigationService NavigationService { get; set; }
        public Guest2 Guest { get; set; }
        public TourSearchHelpView(Guest2 guest, NavigationService navigationService)
        {
            NavigationService = navigationService;
            Guest = guest;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           NavigationService.Navigate(new TourSearchView(Guest, NavigationService));

        }
    }
}
