using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Views.OwnerViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace SIMS_HCI_Project.WPF.ViewModels.OwnerViewModels
{
    public class SelectedGuestReviewViewModel : INotifyPropertyChanged
    {
        public SelectedGuestReviewView SelectedGuestReviewView { get; set; }
        public GuestReviewsView GuestReviewsView { get; set; }
        public RatingGivenByGuest SelectedReview { get; set; }
        public List<string> Images { get; set; }

        private string _image;
        public string Image
        {
            get => _image;
            set
            {
                if (value != _image)
                {

                    _image = value;
                    OnPropertyChanged(nameof(Image));
                }
            }
        }

        private int _currentImageIndex = 0;

        public RelayCommand CloseSelectedGuestReviewViewCommand { get; set; }
        public RelayCommand HomePageFromSelectedReviewCommand { get; set; }
        
        public RelayCommand NextGuestReviewImageCommand { get; set; }
        public RelayCommand PreviousGuestReviewImageCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public SelectedGuestReviewViewModel(SelectedGuestReviewView selectedGuestReviewView, GuestReviewsView guestReviewsView, RatingGivenByGuest selectedReview) 
        {
            InitCommands();

            SelectedGuestReviewView = selectedGuestReviewView;
            GuestReviewsView = guestReviewsView;
            SelectedReview = selectedReview;
            Images = selectedReview.Images;
            Image = Images[_currentImageIndex];
        }

        private void ChangeOutrangeCurrentImageIndex()
        {
            if (_currentImageIndex < 0)
            {
                _currentImageIndex = SelectedReview.Images.Count - 1;
            }
            else if (_currentImageIndex >= SelectedReview.Images.Count)
            {
                _currentImageIndex = 0;
            }
        }

        #region Commands
        public void Executed_NextGuestReviewImageCommand(object obj)
        {
            _currentImageIndex++;
            ChangeOutrangeCurrentImageIndex();
            Image = Images[_currentImageIndex];
        }

        public bool CanExecute_NextGuestReviewImageCommand(object obj)
        {
            return true;
        }

        public void Executed_PreviousGuestReviewImageCommand(object obj)
        {
            _currentImageIndex--;
            ChangeOutrangeCurrentImageIndex();
            Image = Images[_currentImageIndex];
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
    }
}
