using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Observer;
using SIMS_HCI_Project.WPF.Commands;
//using SIMS_HCI_Project.WPF.Views;
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
using System.Windows.Shapes;


namespace SIMS_HCI_Project.WPF.Views.Guest2Views
{
    /// <summary>
    /// Interaction logic for RateSelectedReservation.xaml
    /// </summary>
    public partial class RateSelectedReservationView : Window
    {
        public RateSelectedReservationView(Guest2 guest2, TourReservation tourReservation)
        {
            InitializeComponent();
            this.DataContext = new RateSelectedReservationViewModel( guest2, tourReservation, this);
        }
    }
}
