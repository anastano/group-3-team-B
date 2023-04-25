using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace SIMS_HCI_Project.WPF.ViewModels.Guest1ViewModels
{
    internal class AccommodationImagesViewModel
    {
        private readonly AccommodationReservationService _reservationService;
        private int _currentImageIndex = 0;
        public Accommodation Accommodation { get; set; }
        public String Image { get; set; }
        public RelayCommand NextImageCommand { get; set; }
        public RelayCommand PreviousImageCommand { get; set; }
        public AccommodationImagesViewModel(Accommodation accommodation)
        {
            _reservationService = new AccommodationReservationService();
            Accommodation = accommodation;
            Image = Accommodation.Images[_currentImageIndex];
            InitCommands();
        }
        private void ChangeOutrangeCurrentImageIndex()
        {
            if (_currentImageIndex < 0)
            {
                _currentImageIndex = Accommodation.Images.Count - 1;
            }
            else if (_currentImageIndex >= Accommodation.Images.Count)
            {
                _currentImageIndex = 0;
            }
        }
        #region Commands
        public void ExecutedNextImageCommand(object obj)
        {
            _currentImageIndex++;
            ChangeOutrangeCurrentImageIndex();
            Image = Accommodation.Images[_currentImageIndex];
           

        }
        public void ExecutedPreviousImageCommand(object obj)
        {
            _currentImageIndex--;
            ChangeOutrangeCurrentImageIndex();
            Image = Accommodation.Images[_currentImageIndex];
        }
        public bool CanExecute(object obj)
        {
            return true;
        }
        #endregion

        public void InitCommands()
        {
            NextImageCommand = new RelayCommand(ExecutedNextImageCommand, CanExecute);
            PreviousImageCommand = new RelayCommand(ExecutedPreviousImageCommand, CanExecute);

        }

    }
}
