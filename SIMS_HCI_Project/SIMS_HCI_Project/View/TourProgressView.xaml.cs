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
    /// <summary>
    /// Interaction logic for TourProgressView.xaml
    /// </summary>
    public partial class TourProgressView : Window
    {
        public TourTime TourTime { get; set; }
        public GuestTourAttendance SelectedGuest { get; set; }
        private TourTimeController _tourTimeController;
        private GuestTourAttendanceController _guestTourAttendanceController;

        public TourProgressView(TourTimeController tourTimeController, TourTime tourTime)
        {
            InitializeComponent();
            TourTime = tourTime;
            _tourTimeController = tourTimeController;
            _guestTourAttendanceController = new GuestTourAttendanceController();
            //SelectedGuest = new GuestTourAttendance();
            UpdateButtons();

            DataContext = this;
        }

        private void btnNextKeyPoint_Click(object sender, RoutedEventArgs e)
        {
            _tourTimeController.MoveToNextKeyPoint(TourTime);
            UpdateButtons();
        }

        private void btnCancelTour_Click(object sender, RoutedEventArgs e)
        {
            _tourTimeController.EndTour(TourTime);
        }

        private void MarkGuestAsPresent_Click(object sender, RoutedEventArgs e)
        {
            _guestTourAttendanceController.MarkGuestAsPresent(SelectedGuest);
        }

        private void UpdateButtons()
        {
            if (TourTime.CurrentKeyPointIndex == TourTime.Tour.KeyPoints.Count - 1)
            {
                btnNextKeyPoint.Content = "End Tour";
                btnCancelTour.IsEnabled = false;
            }
        }
    }
}
