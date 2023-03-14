using SIMS_HCI_Project.Controller;
using SIMS_HCI_Project.Model;
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
    public partial class GuideMainView : Window
    {
        /* TODO Load all data/connections on the start*/

        public Guide Guide { get; set; }
        private TourController _tourController;

        public GuideMainView(Guide guide)
        {
            InitializeComponent();
            _tourController = new TourController();
            Guide = guide;
            Guide.Tours = new ObservableCollection<Tour>(_tourController.GetAllByGuideId(guide.Id));

            DataContext = this;
        }

        private void btnTourInput_Click(object sender, RoutedEventArgs e)
        {
            Window inputTourWindow = new TourInputView(_tourController, Guide);
            inputTourWindow.Show();
        }

        private void btnTodayTours_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
