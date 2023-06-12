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
    public class ReviewViewModel : INotifyPropertyChanged
    {
        private int _currentImageIndex;
        public ReviewView ReviewView { get; set; }
        public ReviewsView ReviewsView { get; set; }
        public RatingGivenByGuest Review { get; set; }

        #region OnPropertyChanged
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
        public RelayCommand NextImageCommand { get; set; }
        public RelayCommand PreviousImageCommand { get; set; }
        public RelayCommand CloseViewCommand { get; set; }
        public RelayCommand HomeViewCommand { get; set; }      


        public ReviewViewModel(ReviewView reviewView, ReviewsView reviewsView, RatingGivenByGuest review) 
        {
            InitCommands();

            ReviewView = reviewView;
            ReviewsView = reviewsView;

            _currentImageIndex = 0;
            Review = review;
            Images = review.Images;
            Image = Images[_currentImageIndex];
        }

        private void ChangeOutrangeCurrentImageIndex()
        {
            if (_currentImageIndex < 0)
            {
                _currentImageIndex = Review.Images.Count - 1;
            }
            else if (_currentImageIndex >= Review.Images.Count)
            {
                _currentImageIndex = 0;
            }
        }

        #region Commands
        public void Executed_NextImageCommand(object obj)
        {
            _currentImageIndex++;
            ChangeOutrangeCurrentImageIndex();
            Image = Images[_currentImageIndex];
        }

        public void Executed_PreviousImageCommand(object obj)
        {
            _currentImageIndex--;
            ChangeOutrangeCurrentImageIndex();
            Image = Images[_currentImageIndex];
        }

        public void Executed_CloseViewCommand(object obj)
        {
            ReviewView.Close();
        }

        public void Executed_HomeViewCommand(object obj)
        {
            ReviewView.Close();
            ReviewsView.Close();
        }

        #endregion

        public void InitCommands()
        {
            NextImageCommand = new RelayCommand(Executed_NextImageCommand);
            PreviousImageCommand = new RelayCommand(Executed_PreviousImageCommand);
            CloseViewCommand = new RelayCommand(Executed_CloseViewCommand);
            HomeViewCommand = new RelayCommand(Executed_HomeViewCommand);
        }
    }
}
