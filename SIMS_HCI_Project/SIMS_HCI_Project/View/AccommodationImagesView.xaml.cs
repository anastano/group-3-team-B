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
    /// Interaction logic for AccommodationImagesView.xaml
    /// </summary>
    public partial class AccommodationImagesView : Window
    {
        private readonly AccommodationController _accommodationController;
        public List<string> Images { get; set; }
        private int _currentIndex = 0;
        public AccommodationImagesView(AccommodationController accommodationController, Accommodation accommodation)
        {
            InitializeComponent();
            _accommodationController = accommodationController;
            Images = accommodationController.GetAllImages(accommodation.Id);
            LoadImage();
        }

        private void LoadImage()
        {
            if(Images.Count == 1 && Images[0].Equals(""))
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri("https://thumbs.dreamstime.com/z/no-image-available-icon-photo-camera-flat-vector-illustration-132483141.jpg");
                bitmap.EndInit();

                Image.Source = bitmap;
            }
            else
            {
                if (_currentIndex < 0)
                {
                    _currentIndex = Images.Count - 1;
                }
                else if (_currentIndex >= Images.Count)
                {
                    _currentIndex = 0;
                }

                var fullFilePath = Images[_currentIndex];
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(fullFilePath, UriKind.Absolute);
                bitmap.EndInit();

                Image.Source = bitmap;
            }
            
        }

        private void btnPreviousImage_Click(object sender, RoutedEventArgs e)
        {
            _currentIndex--;
            LoadImage();
        }

        private void btnNextImage_Click(object sender, RoutedEventArgs e)
        {
            _currentIndex++;
            LoadImage();
        }
    }
}

