using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMS_HCI_Project.Domain.Models;

using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Views.Guest2Views;
using System.Windows;
using SIMS_HCI_Project.Applications.Services;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Windows.Navigation;

namespace SIMS_HCI_Project.WPF.ViewModels.Guest2ViewModels
{
    public class RateSelectedReservationViewModel 
    {
        #region Services
        private TourService _tourService;
        private TourReservationService _tourReservationService;
        private TourVoucherService _tourVoucherService;
        private GuestTourAttendanceService _guestTourAttendanceService;
        private TourRatingService _tourRatingService;
        #endregion
        #region Commands
        public RelayCommand ConfirmRatingCommand { get; set; }
        public RelayCommand QuitRatingCommand { get; set; }
        public RelayCommand AddImageCommand { get; set; }
        #endregion
        public RateSelectedReservationView RateSelectedReservationView { get; set; }
        public TourReservation TourReservation { get; set; }
        public Guest2 Guest { get; set; }
        public TourRating TourRating { get; set; }
        public ObservableCollection<string> Images { get; set; }
        public string SelectedTourInfo { get; set; }

        private string _imageURL;
        public string ImageURL
        {
            get { return _imageURL; }
            set
            {
                _imageURL = value;
                OnPropertyChanged();
            }
        }

        private int _overallExperience;
        public int OverallExperience
        {
            get => _overallExperience;
            set
            {
                if (value != _overallExperience)
                {
                    _overallExperience = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _organisation;
        public int Organisation
        {
            get => _organisation;
            set
            {
                if (value != _organisation)
                {
                    _organisation = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _interestingness;
        public int Interestingness
        {
            get => _interestingness;
            set
            {
                if (value != _interestingness)
                {
                    _interestingness = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _guidesKnowledge;
        public int GuidesKnowledge
        {
            get => _guidesKnowledge;
            set
            {
                if (value != _guidesKnowledge)
                {
                    _guidesKnowledge = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _guidesLanguage;
        public int GuidesLanguage
        {
            get => _guidesLanguage;
            set
            {
                if (value != _guidesLanguage)
                {
                    _guidesLanguage = value;
                    OnPropertyChanged();
                }
            }
        }
        private String _additionalComment;
        public String AdditionalComment
        {
            get => _additionalComment;
            set
            {
                if (value != _additionalComment)
                {
                    _additionalComment = value;
                    OnPropertyChanged();
                }
            }
        }
        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        public NavigationService NavigationService { get; set; }

        public RateSelectedReservationViewModel(Guest2 guest2, TourReservation selectedReservation, RateSelectedReservationView rateSelectedReservationView, NavigationService navigationService)
        {
            Guest = guest2;
            TourReservation = selectedReservation;
            RateSelectedReservationView = rateSelectedReservationView;
            NavigationService = navigationService;
            TourRating = new TourRating();
            Images = new ObservableCollection<string>();

            InitCommands();
            LoadFromFiles();

            SelectedTourInfo = GetSelectedTourInfo();

        }

        private string GetSelectedTourInfo()
        {
            string message = "You are currently rating tour: " + TourReservation.TourTime.Tour.Title + ", on location: " + TourReservation.TourTime.Tour.Location.City + ", " + TourReservation.TourTime.Tour.Location.Country + ". Your guide was: " + TourReservation.TourTime.Tour.Guide.Name + " " + TourReservation.TourTime.Tour.Guide.Surname + ". Comment and images' url are optional.";
            return message;
                }

        public void LoadFromFiles()
        {
            _tourService = new TourService();
            _tourReservationService = new TourReservationService();
            _tourVoucherService = new TourVoucherService();
            _guestTourAttendanceService = new GuestTourAttendanceService();
            _tourRatingService = new TourRatingService();
        }
        public void InitCommands()
        {
            QuitRatingCommand = new RelayCommand(ExecuteQuitRating, CanExecuteCancel);
            ConfirmRatingCommand = new RelayCommand(ExecutedConfirmRating, CanExecuteConfirmRating);
            AddImageCommand = new RelayCommand(ExecutedAddImage, CanExecuteAddImage);
        }
        #region Commands
        private MessageBoxResult ConfirmAddImage()
        {
            string sMessageBoxText = $"Are you sure you want to add this image?";
            string sCaption = $"Add image";
            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxImage icnMessageBox = MessageBoxImage.Question;

            MessageBoxResult result = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
            return result;
        }
        private void ExecutedAddImage(object sender)
        {

            if (ImageURL != "" & ConfirmAddImage() == MessageBoxResult.Yes)
            {
                Images.Add(ImageURL);
                ImageURL = "";
            }
        }
        private bool CanExecuteAddImage(object sender)
        {
            return true;
        }
        private MessageBoxResult ConfirmQuit()
        {
            string sMessageBoxText = $"Are you sure you want to quit?";
            string sCaption = $"Quit";
            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxImage icnMessageBox = MessageBoxImage.Question;

            MessageBoxResult result = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
            return result;
        }
        private void ExecuteQuitRating(object sender)
        {
            if(ConfirmQuit() == MessageBoxResult.Yes)
            NavigationService.Navigate(new TourRatingView(Guest, NavigationService));
        }
        public bool CanExecuteCancel(object sender)
        {
            return true;
        }

        private MessageBoxResult ConfirmRating()
        {
            string sMessageBoxText = $"Are you sure you want to submit thit this rating?";
            string sCaption = $"Rating submission";
            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxImage icnMessageBox = MessageBoxImage.Question;

            MessageBoxResult result = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
            return result;
        }

        private void ExecutedConfirmRating(object sender)
        {
            if (ConfirmRating() == MessageBoxResult.Yes)
            {
                //TourRating.ReservationId = TourReservation.Id;
                //TourRating.GuideId = TourReservation.TourTime.Tour.GuideId;
                //TourRating.GuestId = Guest.Id;
                //TourRating.TourReservation = TourReservation;
                TourRating.Attendance = _guestTourAttendanceService.GetByGuestAndTourTimeIds(Guest.Id, TourReservation.TourTimeId);
                TourRating.Images = Images.ToList();

                _tourRatingService.Add(TourRating);
                MessageBox.Show("Tour rating is submited.");
                NavigationService.Navigate(new TourRatingView(Guest, NavigationService));
            }
        }
        public bool CanExecuteConfirmRating(object sender)
        {
            return true;
        }
        #endregion
    }
}
