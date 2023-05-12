using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Views.OwnerViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace SIMS_HCI_Project.WPF.ViewModels.OwnerViewModels
{
    public class AccommodationImagesViewModel
    {
        private readonly AccommodationService _accommodationService;
        public AccommodationImagesView AccommodationImagesView { get; set; }
        public List<string> Images { get; set; }
        private int _currentImageIndex = 0;

        public RelayCommand PreviousAccommodationImageCommand { get; set; }
        public RelayCommand NextAccommodationImageCommand { get; set; }
        public RelayCommand CloseAccommodationImageViewCommand { get; set; }

        public AccommodationImagesViewModel(AccommodationImagesView accommodationsImagesView, AccommodationService accommodationService, Accommodation accommodation )
        {
            InitCommands();

            _accommodationService = accommodationService;

            AccommodationImagesView = accommodationsImagesView;
            Images = _accommodationService.GetImages(accommodation.Id);
            LoadImage();
        }

        private void LoadImage()
        {
            if (IsImagesEmpty())
            {
                AccommodationImagesView.AccommodationImage.Source = LoadDefaultImage();
            }
            else
            {
                ChangeOutrangeCurrentImageIndex();

                AccommodationImagesView.AccommodationImage.Source = ConvertUrlToImage();
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

        #region Commands
        public void Executed_PreviousAccommodationImageCommand(object obj)
        {
            _currentImageIndex--;
            LoadImage();
        }

        public bool CanExecute_PreviousAccommodationImageCommand(object obj)
        {
            return true;
        }

        public void Executed_NextAccommodationImageCommand(object obj)
        {
            _currentImageIndex++;
            LoadImage();
        }

        public bool CanExecute_NextAccommodationImageCommand(object obj)
        {
            return true;
        }

        public void Executed_CloseAccommodationImageViewCommand(object obj)
        {
            AccommodationImagesView.Close();
        }

        public bool CanExecute_CloseAccommodationImageViewCommand(object obj)
        {
            return true;
        }

        #endregion

        public void InitCommands()
        {
            PreviousAccommodationImageCommand = new RelayCommand(Executed_PreviousAccommodationImageCommand, CanExecute_PreviousAccommodationImageCommand);
            NextAccommodationImageCommand = new RelayCommand(Executed_NextAccommodationImageCommand, CanExecute_NextAccommodationImageCommand);
            CloseAccommodationImageViewCommand = new RelayCommand(Executed_CloseAccommodationImageViewCommand, CanExecute_CloseAccommodationImageViewCommand);
        }
    }
}
