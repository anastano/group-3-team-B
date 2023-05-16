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
    public class SelectedGuestReviewViewModel
    {
        public SelectedGuestReviewView SelectedGuestReviewView { get; set; }
        public GuestReviewsView GuestReviewsView { get; set; }
        public RatingGivenByGuest SelectedReview { get; set; }
        public int CurrentImageIndex;

        public RelayCommand CloseSelectedGuestReviewViewCommand { get; set; }
        public RelayCommand HomePageFromSelectedReviewCommand { get; set; }
        
        public RelayCommand NextGuestReviewImageCommand { get; set; }
        public RelayCommand PreviousGuestReviewImageCommand { get; set; }

        public SelectedGuestReviewViewModel(SelectedGuestReviewView selectedGuestReviewView, GuestReviewsView guestReviewsView, RatingGivenByGuest selectedReview) 
        {
            InitCommands();

            SelectedGuestReviewView = selectedGuestReviewView;
            GuestReviewsView = guestReviewsView;
            SelectedReview = selectedReview;

            LoadImage();

        }

        #region Commands
        public void Executed_NextGuestReviewImageCommand(object obj)
        {
            CurrentImageIndex++;
            LoadImage();
        }

        public bool CanExecute_NextGuestReviewImageCommand(object obj)
        {
            return true;
        }

        public void Executed_PreviousGuestReviewImageCommand(object obj)
        {
            CurrentImageIndex--;
            LoadImage();
        }

        public bool CanExecute_PreviousGuestReviewImageCommand(object obj)
        {
            return true;
        }

        public void Executed_CloseSelectedGuestReviewViewCommand(object obj)
        {
            SelectedGuestReviewView.Close();
        }

        public bool CanExecute_CloseSelectedGuestReviewViewCommand(object obj)
        {
            return true;
        }

        public void Executed_HomePageFromSelectedReviewCommand(object obj)
        {
            SelectedGuestReviewView.Close();
            GuestReviewsView.Close();
        }

        public bool CanExecute_HomePageFromSelectedReviewCommand(object obj)
        {
            return true;
        }
        #endregion

        public void InitCommands()
        {
            CloseSelectedGuestReviewViewCommand = new RelayCommand(Executed_CloseSelectedGuestReviewViewCommand, CanExecute_CloseSelectedGuestReviewViewCommand);
            HomePageFromSelectedReviewCommand = new RelayCommand(Executed_HomePageFromSelectedReviewCommand, CanExecute_HomePageFromSelectedReviewCommand);
            NextGuestReviewImageCommand = new RelayCommand(Executed_NextGuestReviewImageCommand, CanExecute_NextGuestReviewImageCommand);
            PreviousGuestReviewImageCommand = new RelayCommand(Executed_PreviousGuestReviewImageCommand, CanExecute_PreviousGuestReviewImageCommand);
        }

        private void LoadImage()
        {
            if (IsImageListEmpty())
            {
                SelectedGuestReviewView.imgGuestReviewImage.Source = LoadDefaultImage();
            }
            else
            {
                ChangeOutrangeCurrentImageIndex();

                SelectedGuestReviewView.imgGuestReviewImage.Source = ConvertUrlToImage();
            }

        }

        private void ChangeOutrangeCurrentImageIndex()
        {
            if (CurrentImageIndex < 0)
            {
                CurrentImageIndex = SelectedReview.Images.Count - 1;
            }
            else if (CurrentImageIndex >= SelectedReview.Images.Count)
            {
                CurrentImageIndex = 0;
            }
        }

        private bool IsImageListEmpty()
        {
            return SelectedReview.Images.Count == 1 && SelectedReview.Images[0].Equals("");
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
            var fullImagePath = SelectedReview.Images[CurrentImageIndex];
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(fullImagePath, UriKind.Absolute);
            image.EndInit();

            return image;
        }

    }
}
