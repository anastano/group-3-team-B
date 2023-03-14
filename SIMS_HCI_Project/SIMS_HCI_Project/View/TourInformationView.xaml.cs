using SIMS_HCI_Project.Model;
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

namespace SIMS_HCI_Project.View
{
    public partial class TourInformationView : Window
    {
        public Tour Tour { get; set; }
        private TourTime _selectedTourTime;
        public TourTime SelectedTourTime
        {
            get { return _selectedTourTime; }
            set
            {
                _selectedTourTime = value;
                UpdateButtons();
            }
        }

        public TourInformationView(Tour tour, TourTime tourTime = null)
        {
            InitializeComponent();
            Tour = tour;
            if (tourTime != null)
            {
                SelectedTourTime = tourTime;
            }
            else
            {
                SelectedTourTime = tour.DepartureTimes[0];
            }
            DataContext = this;

            UpdateButtons();
        }

        private void btnStartTour_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnCancelTour_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UpdateButtons()
        {
            switch(SelectedTourTime.Status)
            {
                case TourStatus.NOT_STARTED:
                    btnStartTour.Content = "Start Tour";
                    if(SelectedTourTime.DepartureTime < DateTime.Now.AddDays(-1))
                    {
                        btnStartTour.IsEnabled = false;
                    }
                    else
                    {
                        btnStartTour.IsEnabled = true;
                    }
                    break;
                case TourStatus.IN_PROGRESS:
                    btnStartTour.Content = "See Progress";
                    break;
                case TourStatus.COMPLETED:
                case TourStatus.CANCELED:
                    btnStartTour.IsEnabled = false;
                    break;
            }
        }
    }
}
