using SIMS_HCI_Project.Controller;
using SIMS_HCI_Project.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
    public partial class GuideMainView : Window
    {
        public Guide Guide { get; set; }
        public TourTime SelectedTourTime { get; set; }

        private TourController _tourController;
        private TourTimeController _tourTimeController;

        public GuideMainView(Guide guide)
        {
            InitializeComponent();

            _tourController = new TourController();
            _tourTimeController = new TourTimeController();

            _tourController.LoadConnections();
            _tourTimeController.LoadConnections();

            Guide = guide;
            LoadGuideInformation();

            DataContext = this;
        }

        private void btnTourInput_Click(object sender, RoutedEventArgs e)
        {
            Window inputTourWindow = new TourInputView(_tourController, Guide);
            inputTourWindow.Owner = this;
            inputTourWindow.Show();
        }

        private void btnAllTours_Click(object sender, RoutedEventArgs e)
        {
            Window allTours = new GuideAllToursView(_tourTimeController, Guide);
            allTours.Owner = this;
            allTours.Show();
        }

        private void btnTourInfo_Click(object sender, RoutedEventArgs e)
        {
            Window tourInformation = new TourInformationView(SelectedTourTime.Tour, _tourTimeController, SelectedTourTime);
            tourInformation.Owner = this;
            tourInformation.Show();
        }

        private void LoadGuideInformation()
        {
            Guide.TodaysTours = new ObservableCollection<TourTime>(_tourTimeController.GetTodaysByGuideId(Guide.Id));
            Guide.Tours = new ObservableCollection<Tour>(_tourController.GetAllByGuideId(Guide.Id));
        }
    }
}
