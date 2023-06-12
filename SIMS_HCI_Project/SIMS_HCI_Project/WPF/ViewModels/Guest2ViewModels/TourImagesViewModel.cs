using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace SIMS_HCI_Project.WPF.ViewModels.Guest2ViewModels
{
    public class TourImagesViewModel : INotifyPropertyChanged
    {
        public Guest2 Guest { get; set; }
        public NavigationService NavigationService { get; set; }
        public Tour Tour { get; set; }
        private int _currentImageIndex = 0;
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
        public RelayCommand NextImageCommand { get; set; }
        public RelayCommand PreviousImageCommand { get; set; }
        public RelayCommand BackCommand { get; set; }
        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public TourImagesViewModel(Guest2 guest, NavigationService navigationService, Tour tour)
        {
            Guest = guest;
            NavigationService = navigationService;
            Tour = tour;
            Image = Tour.Images[_currentImageIndex];
            InitCommands();
        }

        private void InitCommands()
        {
            NextImageCommand = new RelayCommand(ExecutedNextImageCommand, CanExecute);
            PreviousImageCommand = new RelayCommand(ExecutedPreviousImageCommand, CanExecute);
            BackCommand = new RelayCommand(ExecutedBackCommand, CanExecute);
        }

        #region Commands
        public void ExecutedNextImageCommand(object obj)
        {
            _currentImageIndex++;
            ChangeOutrangeCurrentImageIndex();
            Image = Tour.Images[_currentImageIndex];
        }
        public void ExecutedBackCommand(object obj)
        {
            NavigationService.GoBack();
        }
        public void ExecutedPreviousImageCommand(object obj)
        {
            _currentImageIndex--;
            ChangeOutrangeCurrentImageIndex();
            Image = Tour.Images[_currentImageIndex];
        }
        public bool CanExecute(object obj)
        {
            return true;
        }
        private void ChangeOutrangeCurrentImageIndex()
        {
            if (_currentImageIndex < 0)
            {
                _currentImageIndex = Tour.Images.Count - 1;
            }
            else if (_currentImageIndex >= Tour.Images.Count)
            {
                _currentImageIndex = 0;
            }
        }
        #endregion
    }
}
