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
using SIMS_HCI_Project.WPF.ViewModels.Guest2ViewModels;
using SIMS_HCI_Project.Domain.Models;



namespace SIMS_HCI_Project.WPF.Views.Guest2Views
{
    /// <summary>
    /// Interaction logic for Requests.xaml
    /// </summary>
    public partial class RequestsView : Page
    {
        public RequestsView(Guest2 guest2, NavigationService navigationService, int selectedTabIndex)
        {
            InitializeComponent();
            this.RegularRequests.Content = new RegularRequestsView(guest2, navigationService);
            this.ComplexRequests.Content = new ComplexRequestsView(guest2, navigationService);
            this.DataContext = new RequestsViewModel(guest2, navigationService, this, selectedTabIndex);
        }
    }


}
