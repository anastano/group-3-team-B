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
    /// Interaction logic for OwnerAccommodationImagesView.xaml
    /// </summary>
    public partial class OwnerAccommodationImagesView : Window
    {
        private readonly AccommodationController _accommodationController;
        public List<string> Images { get; set; }
        private int _currentImageIndex = 0;
        public OwnerAccommodationImagesView(AccommodationController accommodationController, Accommodation accommodation)
        {
            InitializeComponent();
            _accommodationController = accommodationController;
            Images = accommodationController.GetAllImages(accommodation.Id);
            LoadImage();
        }

        private void LoadImage()
        {
            if (IsImagesEmpty())
            {
                AccommodationImage.Source = LoadDefaultImage();
            }
            else
            {
                ChangeOutrangeCurrentImageIndex();

                AccommodationImage.Source = ConvertUrlToImage();
            }

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

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Right))
                btnNextImage_Click(sender, e);
            else if (Keyboard.IsKeyDown(Key.Left))
                btnPreviousImage_Click(sender, e);
            else if (Keyboard.IsKeyDown(Key.Escape))
                Close();
        }
    }
}
