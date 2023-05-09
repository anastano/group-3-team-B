using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.ViewModels.Guest2ViewModels;
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

namespace SIMS_HCI_Project.WPF.Views.Guest2Views
{
    /// <summary>
    /// Interaction logic for TourImagesView.xaml
    /// </summary>
    public partial class TourImagesView : Page
    {
        public TourImagesView(Guest2 guest, NavigationService navigationService, Tour tour)
        {
            InitializeComponent();
            this.DataContext = new TourImagesViewModel(guest, navigationService, tour);
        }
    }
}
