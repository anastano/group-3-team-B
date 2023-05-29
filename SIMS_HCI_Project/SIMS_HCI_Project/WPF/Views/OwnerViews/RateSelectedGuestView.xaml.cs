using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.ViewModels.OwnerViewModels;
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
using System.Windows.Shapes;

namespace SIMS_HCI_Project.WPF.Views.OwnerViews
{
    /// <summary>
    /// Interaction logic for RateSelectedGuestView.xaml
    /// </summary>
    public partial class RateSelectedGuestView : Window
    {
        public RateSelectedGuestView(UnratedReservationsViewModel unratedReservationsVM, AccommodationReservation selectedReservation)
        {
            InitializeComponent();
            this.DataContext = new RateSelectedGuestViewModel(this, unratedReservationsVM, selectedReservation);
        }
    }
}
