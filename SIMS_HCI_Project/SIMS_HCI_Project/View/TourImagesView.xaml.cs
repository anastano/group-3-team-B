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
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;
using Image = System.Windows.Controls.Image;

namespace SIMS_HCI_Project.View
{
    /// <summary>
    /// Interaction logic for TourImagesView.xaml
    /// </summary>
    public partial class TourImagesView : Window
    {
        private TourController _tourController;
        public List<string> Images { get; set; }
        private int _currentImageIndex = 0;

        public TourImagesView(TourController tourController, Tour tour)
        {
            InitializeComponent();
            _tourController = tourController;
            Images = tourController.GetImages(tour.Id);
            LoadImage();
        }

        private void LoadImage()
        {
            if (IsImagesEmpty())
            {
                TourImage.Source = LoadDefaultImage();
            }
            else
            {
                ChangeOutrangeCurrentImageIndex();
                TourImage.Source = ConvertUrlToImage();
            }
            
        }

        private bool IsImagesEmpty()
        {
            return Images.Count == 1 && Images[0].Equals("");
        }
        private BitmapImage LoadDefaultImage()
        {
            BitmapImage defaultImage = new BitmapImage();
            defaultImage.BeginInit();
            defaultImage.UriSource = new Uri("https://thumbs.dreamstime.com/z/no-image-available-icon-photo-camera-flat-vector-illustration-132483141.jpg");
            defaultImage.EndInit();

            return defaultImage;
        }
        private void ChangeOutrangeCurrentImageIndex()
        {
            if (_currentImageIndex < 0)
            {
                _currentImageIndex = Images.Count - 1;
            }
            else if (_currentImageIndex >= Images.Count)
            {
                _currentImageIndex = 0;
            }
        }
        private BitmapImage ConvertUrlToImage()
        {
            var fullImagePath = Images[_currentImageIndex];
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(fullImagePath, UriKind.Absolute);
            image.EndInit();

            return image;
        }
        private void btnPreviousImage_Click(object sender, RoutedEventArgs e)
        {
            _currentImageIndex--;
            LoadImage();
        }

        private void btnNextImage_Click(object sender, RoutedEventArgs e)
        {
            _currentImageIndex++;
            LoadImage();
        }
    }
}
