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
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SIMS_HCI_Project.WPF.ViewModels.Guest1ViewModels
{
    internal class AccommodationImagesViewModel : INotifyPropertyChanged
    {
        private readonly AccommodationReservationService _reservationService;
        private int _currentImageIndex = 0;
        public Accommodation Accommodation { get; set; }
        public Guest1 Guest { get; set; }
        public int DaysNumber { get; set; }
        public int GuestsNumber { get; set; }
        public String Name { get; set; }

        private String _image;
        public String Image
        {
            get => _image;
            set
            {
                if (value != _image)
                {
                    _image = value;
                    OnPropertyChanged();
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public RelayCommand NextImageCommand { get; set; }
        public RelayCommand PreviousImageCommand { get; set; }
        public RelayCommand BackCommand { get; set; }
        private object _currentViewModel;
        public object CurrentViewModel

        {
            get => _currentViewModel;
            set
            {
                if (value != _currentViewModel)
                {
                    _currentViewModel = value;
                    OnPropertyChanged();
                }
            }
        }
        public AccommodationImagesViewModel(Accommodation accommodation, Guest1 guest, int guests, int days, string name)
        {
            _reservationService = new AccommodationReservationService();
            Accommodation = accommodation;
            Image = Accommodation.Images[_currentImageIndex];
            Guest = guest;
            GuestsNumber = guests;
            DaysNumber = days;
            Name = name;
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
        public void ExecutedBackCommand(object obj)
        {
            CurrentViewModel = new AccommodationSearchViewModel(Guest, GuestsNumber, DaysNumber);
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
            BackCommand = new RelayCommand(ExecutedBackCommand, CanExecute);

        }

    }
}
