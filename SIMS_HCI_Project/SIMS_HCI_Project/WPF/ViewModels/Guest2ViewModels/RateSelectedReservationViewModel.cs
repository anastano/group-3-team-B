using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Observer;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Views.Guest2Views;
using System.Windows;
using SIMS_HCI_Project.Applications.Services;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace SIMS_HCI_Project.WPF.ViewModels.Guest2ViewModels
{
    public class RateSelectedReservationViewModel
    {
        private TourService _tourService;
        private TourReservationService _tourReservationService;
        private TourVoucherService _tourVoucherService;
        private LocationService _locationService;
        private GuestTourAttendanceService _guestTourAttendanceService;
        private TourRatingService _tourRatingService;

        public RateSelectedReservationView RateSelectedReservationView { get; set; }
        public TourReservation TourReservation { get; set; }
        public Guest2 Guest { get; set; }
        public TourRating TourRating { get; set; }

        public RelayCommand Back { get; set; }
        public RelayCommand ConfirmRating { get; set; }
        public RelayCommand Cancel { get; set; }
        public RelayCommand AddImage { get; set; }
        public ObservableCollection<string> Images { get; set; }

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
        /*private String _images;
        public String Images
        {
            get => _images;
            set
            {
                if (value != _images)
                {
                    _images = value;
                    OnPropertyChanged();
                }
            }
        }*/

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public RateSelectedReservationViewModel(Guest2 guest2, TourReservation selectedReservation, RateSelectedReservationView rateSelectedReservationView)
        {
            Guest = guest2;
            TourReservation = selectedReservation;
            RateSelectedReservationView = rateSelectedReservationView;
            TourRating = new TourRating();
            Images = new ObservableCollection<string>();

            InitCommands();
            LoadFromFiles();

        }

        public void LoadFromFiles()
        {
            _tourService = new TourService();
            _tourReservationService = new TourReservationService();
            _tourVoucherService = new TourVoucherService();
            _locationService = new LocationService();
            _guestTourAttendanceService = new GuestTourAttendanceService();
            _tourRatingService = new TourRatingService();

            _tourService.ConnectLocations();
            _tourService.ConnectKeyPoints();
            _tourService.ConnectDepartureTimes();

            _tourReservationService.ConnectVouchers(_tourVoucherService);
            _tourReservationService.ConnectTourTimes(_tourService);
            _tourReservationService.ConnectAvailablePlaces(_tourService);

            _tourService.CheckAndUpdateStatus();
        }
        public void InitCommands()
        {
            Back = new RelayCommand(Executed_Back, CanExecute_Back);
            Cancel = new RelayCommand(Executed_Cancel, CanExecute_Cancel);
            ConfirmRating = new RelayCommand(Executed_ConfirmRating, CanExecute_ConfirmRating);
            AddImage = new RelayCommand(Executed_AddImage, CanExecute_AddImage);
        }
        private void Executed_AddImage(object sender)
        {
            if (ImageURL != "")
            {
                Images.Add(ImageURL);
                ImageURL = "";
            }
        }
        private bool CanExecute_AddImage(object sender)
        {
            return true;
        }
        private void Executed_Back(object sender)
        {
            Window window = new TourRatingView(Guest);
            window.Show();
            RateSelectedReservationView.Close();
        }
        public bool CanExecute_Back(object sender)
        {
            return true;
        }
        private void Executed_Cancel(object sender)
        {
            Window window = new TourRatingView(Guest);
            window.Show();
            RateSelectedReservationView.Close();
        }
        public bool CanExecute_Cancel(object sender)
        {
            return true;
        }

        private void Executed_ConfirmRating(object sender)
        {
            // dodaj validaciju da mora poslati kompletnu formu da bi se prihvatila
            TourRating.ReservationId = TourReservation.Id;
            TourRating.GuideId = TourReservation.TourTime.Tour.GuideId;
            TourRating.GuestId = Guest.Id;
            TourRating.TourReservation = TourReservation;
            TourRating.Images = Images.ToList();

            _tourRatingService.Add(TourRating);
            MessageBox.Show("Submited");
            Window window = new TourRatingView(Guest);
            window.Show();
            RateSelectedReservationView.Close();

        }
        public bool CanExecute_ConfirmRating(object sender)
        {
            return true;
        }
    }
}
