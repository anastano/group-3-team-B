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
using System.Windows.Controls.DataVisualization;

namespace SIMS_HCI_Project.WPF.ViewModels.Guest1ViewModels
{
    internal class RatingReservationViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private NavigationService _navigationService;
        private AccommodationReservationService _accommodationReservationService;
        private RatingGivenByGuestService _ratingService;
        private RenovationRecommendationService _recommendationService;
        public AccommodationReservation Reservation { get; set; }
        public RelayCommand ReviewReservationCommand { get; set; }
        public RelayCommand CancelReviewCommand { get; set; }
        public RelayCommand RemoveImageCommand { get; set; }
        public RelayCommand AddImageCommand { get; set; }
        public RelayCommand RecommendRenovationCommand { get; set; }
        public RelayCommand StarRateCorrectnessCommand { get; set; }
        public RelayCommand StarRateCleanlinessCommand { get; set; }
        public RenovationRecommendation Recommendation { get; set; }
        public ObservableCollection<string> Images { get; set; }
        public String SelectedUrl { get; set; }
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
        private bool _isFilled;
        public bool IsFilled
        {
            get => _isFilled;
            set
            {
                if (value != _isFilled)
                {
                    _isFilled = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _isChecked;
        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                if (value != _isChecked)
                {
                    _isChecked = value;
                    OnPropertyChanged();
                }
            }
        }
        private double _selectedStarCleanliness;
        public double  SelectedStarCleanliness
        {
            get => _selectedStarCleanliness;
            set
            {
                if (value != _selectedStarCleanliness)
                {
                    _selectedStarCleanliness = value;
                    OnPropertyChanged(nameof(SelectedStarCleanliness));
                }
            }
        }
        private double _selectedStarCorrectness;
        public double SelectedStarCorrectness
        {
            get => _selectedStarCorrectness;
            set
            {
                if (value != _selectedStarCorrectness)
                {
                    _selectedStarCorrectness = value;
                    OnPropertyChanged(nameof(SelectedStarCorrectness));
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
            _navigationService.RecommendationChanged += OnRecommendationChanged;
            _accommodationReservationService = new AccommodationReservationService();
            _ratingService = new RatingGivenByGuestService();
            _recommendationService = new RenovationRecommendationService();
            Reservation = reservation;
            Images = new ObservableCollection<string>();
            Owner = Reservation.Accommodation.Owner.Name + " " + Reservation.Accommodation.Owner.Surname;
            Rating = new RatingGivenByGuest();
            Rating.ReservationId = Reservation.Id;
            Rating.Reservation = Reservation;
            Recommendation = new RenovationRecommendation();
            InitialProperties(); 
            InitCommands();
        }
        private void OnRecommendationChanged()
        {
            Recommendation = _navigationService.NavigationStore.Recommendation;
            IsFilled = true;

        }
        public void InitialProperties()
        {
            SelectedStarCleanliness = 0;
            SelectedStarCorrectness = 0;
            ImageUrl = " ";
            IsFilled = false;
            IsChecked = false;
        }
        public void ExecutedReviewReservationCommand(object obj)
        {
            Rating.Cleanliness = (int)SelectedStarCleanliness;
            Rating.Correctness = (int)SelectedStarCorrectness;
            if (IsValid)
            {
                _ratingService.RateReservation(_accommodationReservationService, Rating);
                if (IsChecked && IsFilled)
                {
                    _recommendationService.Add(_navigationService.NavigationStore.Recommendation, _ratingService.GetByReservationId(Rating.ReservationId));
                }
                _navigationService.Navigate(new ReservationsViewModel(Reservation.Guest, _navigationService, 1), "My Reservations");
            }
        }
        public void ExecutedRecommendRenovationCommand(object obj)
        {
            _navigationService.Navigate(new RenovationRecommendationViewModel(_navigationService, Recommendation), "Recommend renovation");
        }
        public void ExecutedCancelReviewCommand(object obj)
        {
            //ne moze back jer ako smo usli u recommend onda nam je to prethodni
            _navigationService.Navigate(new ReservationsViewModel(Reservation.Guest, _navigationService, 1), "My Reservations");
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
        public void ExecutedStarCleanlinessCommand(object obj)
        {
            SelectedStarCleanliness  = Convert.ToDouble(obj);
        }
        public void ExecutedStarCorrectnessCommand(object obj)
        {
            SelectedStarCorrectness = Convert.ToDouble(obj);
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
            StarRateCleanlinessCommand = new RelayCommand(ExecutedStarCleanlinessCommand, CanExecute);
            StarRateCorrectnessCommand = new RelayCommand(ExecutedStarCorrectnessCommand, CanExecute);
        }
        public string Error => null;
        public string this[string columnName]
        {
            get
            {

                if (columnName == nameof(SelectedStarCleanliness))
                {
                    if (SelectedStarCleanliness == 0)
                        return "Cleanliness is required";
                }
                else if (columnName == nameof(SelectedStarCorrectness))
                {
                    if (SelectedStarCorrectness == 0)
                        return "Correctness is required";
                }


                return null;
            }
        }

        private readonly string[] _validatedProperties = { "SelectedStarCleanliness", "SelectedStarCorrectness" };

        public bool IsValid
        {
            get
            {
                foreach (var property in _validatedProperties)
                {
                    if (this[property] != null)
                        return false;
                }

                return true;
            }
        }
    }
}
