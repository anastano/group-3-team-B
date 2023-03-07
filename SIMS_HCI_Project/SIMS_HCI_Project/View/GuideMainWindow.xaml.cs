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
    /// Interaction logic for GuideMainWindow.xaml
    /// </summary>
    public partial class GuideMainWindow : Window
    {
        public GuideMainWindow()
        {
            InitializeComponent();
        }

        private void btnTourInput_Click(object sender, RoutedEventArgs e)
        {
            Window inputTourWindow = new TourInputWindow();
            inputTourWindow.Show();
        }

        private void btnTodayTours_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
