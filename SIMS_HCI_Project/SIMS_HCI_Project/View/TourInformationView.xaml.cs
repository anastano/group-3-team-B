using SIMS_HCI_Project.Controller;
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
                UpdateButtonStatus();
            }
        }

        private TourTimeController _tourTimeController;

        public TourInformationView(Tour tour, TourTimeController tourTimeController, TourTime tourTime = null)
        {
            InitializeComponent();

            Tour = tour;
            _tourTimeController = tourTimeController;

            SetSelectedTourTime(tourTime);
            UpdateButtonStatus();

            DataContext = this;
        }

        private void btnStartTour_Click(object sender, RoutedEventArgs e)
        {
            _tourTimeController.StartTour(SelectedTourTime);
            UpdateButtonStatus();

            Window tourProgress = new TourProgressView(_tourTimeController, SelectedTourTime);
            tourProgress.Owner = this;
            tourProgress.Show();
        }

        private void btnCancelTour_Click(object sender, RoutedEventArgs e)
        {
            _tourTimeController.CancelTour(SelectedTourTime);
            UpdateButtonStatus();
        }

        private void SetSelectedTourTime(TourTime tourTime)
        {
            if (tourTime != null)
            {
                SelectedTourTime = tourTime;
            }
            else
            {
                SelectedTourTime = Tour.DepartureTimes[0];
            }
        }

        private void UpdateButtonStatus()
        {
            switch(SelectedTourTime.Status)
            {
                case TourStatus.NOT_STARTED:
                    btnStartTour.Content = "Start Tour";
                    if(SelectedTourTime.DepartureTime.Date != DateTime.Today || _tourTimeController.HasTourInProgress(SelectedTourTime.Tour.GuideId))
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
                    btnStartTour.IsEnabled = true;
                    break;
                case TourStatus.COMPLETED:
                case TourStatus.CANCELED:
                    btnStartTour.Content = "See History";
                    btnStartTour.IsEnabled = true;
                    break;
            }
        }
    }
}
