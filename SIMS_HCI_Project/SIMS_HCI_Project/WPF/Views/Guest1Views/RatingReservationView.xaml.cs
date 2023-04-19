using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.ViewModels.Guest1ViewModels;
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

namespace SIMS_HCI_Project.WPF.Views.Guest1Views
{
    /// <summary>
    /// Interaction logic for RatingReservationView.xaml
    /// </summary>
    public partial class RatingReservationView : Page
    {
        public RatingReservationView(AccommodationReservationService reservationService, AccommodationReservation reservation)
        {
            InitializeComponent();
            this.DataContext = new RatingReservationViewModel(this, reservationService, reservation);
        }
    }
}
