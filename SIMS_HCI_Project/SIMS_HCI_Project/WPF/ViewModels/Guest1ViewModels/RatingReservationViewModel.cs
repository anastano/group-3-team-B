using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Services;
using SIMS_HCI_Project.WPF.Views.Guest1Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SIMS_HCI_Project.WPF.ViewModels.Guest1ViewModels
{
    internal class RatingReservationViewModel : INotifyPropertyChanged
    {
        private NavigationService _navigationService;
        private AccommodationReservationService _accommodationReservationService;
        private RatingGivenByGuestService _ratingService;
        public AccommodationReservation Reservation { get; set; }
        public RelayCommand ReviewReservationCommand { get; set; }
        public RelayCommand CancelReviewCommand { get; set; }
        public RelayCommand RemoveImageCommand { get; set; }
        public RelayCommand AddImageCommand { get; set; }
        public RelayCommand RecommendRenovationCommand { get; set; }
        public ObservableCollection<string> Images { get; set; }
        public String SelectedUrl { get; set; }
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
        private String _owner;
        public String Owner
        {
            get => _owner;
            set
            {
                if (value != _owner)
                {
                    _owner = value;
                    OnPropertyChanged();
                }
            }
        }
        private RatingGivenByGuest _rating;
        public RatingGivenByGuest Rating
        {
            get => _rating;
            set
            {
                if (value != _rating)
                {
                    _rating = value;
                    OnPropertyChanged();
                }
            }
        }

        private String _imageUrl;
        public String ImageUrl
        {
            get => _imageUrl;
            set
            {
                if (value != _imageUrl)
                {
                    _imageUrl = value;
                    OnPropertyChanged();
                }
            }
        }
        private Regex urlRegex = new Regex("(http(s?)://.)([/|.|\\w|\\s|-])*\\.(?:jpg|gif|png)|(^$)");

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public RatingReservationViewModel(AccommodationReservation reservation, NavigationService navigationService)
        {
            _navigationService = navigationService;
            _accommodationReservationService = new AccommodationReservationService();
            _ratingService = new RatingGivenByGuestService();
            Reservation = reservation;
            Images = new ObservableCollection<string>();
            Owner = Reservation.Accommodation.Owner.Name + " " + Reservation.Accommodation.Owner.Surname;
            Rating = new RatingGivenByGuest();
            InitialProperties(); 
            InitCommands();
        }
        public void InitialProperties()
        {
            ImageUrl = " ";
        }
        public void ExecutedReviewReservationCommand(object obj)
        {
            //ovaj objekat cemo u konstruktor staviti
            //Rating.RenovationRecommendation = _navigationService._navigationstore.Recommendation;
            //_ratingService.RateReservation(_accommodationReservationService, new RatingGivenByGuest(Reservation.Id, Cleanliness, Correcntess, AdditionalComment, new List<string>(Images)));
            _navigationService.Navigate(new ReservationsViewModel(Reservation.Guest, _navigationService), "My Reservations");
        }
        public void ExecutedRecommendRenovationCommand(object obj)
        {
            _navigationService.Navigate(new RenovationRecommendationViewModel(_navigationService), "Recommend renovation");
        }
        public void ExecutedCancelReviewCommand(object obj)
        {
            _navigationService.NavigateBack();
        }
        public void ExecutedRemoveImageCommand(object obj)
        {
            if (SelectedUrl != null)
            {
                Rating.Images.Remove(SelectedUrl);
                Images.Remove(SelectedUrl);
            }

        }
        public void ExecutedAddImageCommand(object obj)
        {
            Match match = urlRegex.Match(ImageUrl);
            if (match.Success)
            {
                Rating.Images.Add(ImageUrl);
                Images.Add(ImageUrl);
                ImageUrl = "";
                //return "URL is not in valid format.";
            }
        }
        public bool CanExecute(object obj)
        {
            return true;
        }
        public void InitCommands()
        {
            ReviewReservationCommand = new RelayCommand(ExecutedReviewReservationCommand, CanExecute);
            CancelReviewCommand = new RelayCommand(ExecutedCancelReviewCommand, CanExecute);
            RemoveImageCommand = new RelayCommand(ExecutedRemoveImageCommand, CanExecute);
            AddImageCommand = new RelayCommand(ExecutedAddImageCommand, CanExecute);
            RecommendRenovationCommand = new RelayCommand(ExecutedRecommendRenovationCommand, CanExecute);
        }
    }
}
