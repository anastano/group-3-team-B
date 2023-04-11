using SIMS_HCI_Project.Model;
using SIMS_HCI_Project.Observer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace SIMS_HCI_Project.View
{
    /// <summary>
    /// Interaction logic for TourRatingView.xaml
    /// </summary>
    public partial class TourRatingView : Window, IObserver
    {
        Guest2 Guest { get; set; }
        TourReservation SelectedReservation { get; set; }
        public ObservableCollection<TourReservation> UnratedReservations { get; set; }

        public TourRatingView(Guest2 guest)
        {
            InitializeComponent();
            Guest = guest;

            DataContext = this;
        }

        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}
