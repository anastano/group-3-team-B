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
    public partial class GuideAllToursView : Window
    {
        public Guide Guide { get; set; }
        public Tour SelectedTour { get; set; }

        private TourTimeController _tourTimeController;

        public GuideAllToursView(TourTimeController tourTimeController, Guide guide)
        {
            InitializeComponent();

            Guide = guide;
            _tourTimeController = tourTimeController;

            DataContext = this;
        }

        private void btnTourInfo_Click(object sender, RoutedEventArgs e)
        {
            Window tourInformation = new TourInformationView(SelectedTour, _tourTimeController);
            tourInformation.Owner = this;
            tourInformation.Show();
        }
    }
}
