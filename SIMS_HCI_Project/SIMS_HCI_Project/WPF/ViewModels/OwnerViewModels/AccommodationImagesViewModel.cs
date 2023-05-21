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
    public class AccommodationImagesViewModel : INotifyPropertyChanged
    {
        private readonly AccommodationService _accommodationService;
        public AccommodationImagesView AccommodationImagesView { get; set; }
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

        public RelayCommand PreviousAccommodationImageCommand { get; set; }
        public RelayCommand NextAccommodationImageCommand { get; set; }
        public RelayCommand CloseAccommodationImageViewCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public AccommodationImagesViewModel(AccommodationImagesView accommodationsImagesView, AccommodationService accommodationService, Accommodation accommodation )
        {
            InitCommands();

            _accommodationService = accommodationService;

            AccommodationImagesView = accommodationsImagesView;
            Images = _accommodationService.GetImages(accommodation.Id);
            Image = Images[_currentImageIndex];
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

        #region Commands
        public void Executed_PreviousAccommodationImageCommand(object obj)
        {
            _currentImageIndex--;
            ChangeOutrangeCurrentImageIndex();
            Image = Images[_currentImageIndex];
        }

        public bool CanExecute_PreviousAccommodationImageCommand(object obj)
        {
            return true;
        }

        public void Executed_NextAccommodationImageCommand(object obj)
        {
            _currentImageIndex++;
            ChangeOutrangeCurrentImageIndex();
            Image = Images[_currentImageIndex];
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
