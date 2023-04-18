using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Views.Guest1Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SIMS_HCI_Project.WPF.ViewModels.Guest1ViewModels
{
    internal class RatingReservationViewModel
    {
        private AccommodationReservationService _accommodationReservationService;
        private RatingGivenByGuestService _ratingService;
        public RatingReservationView RatingReservationView { get; set; }
        public Guest1MainView Guest1MainView { get; set; }
        public ReservationsView ReservationsView { get; set; }
        public AccommodationReservation Reservation { get; set; }
        public RelayCommand ReviewReservationCommand { get; set; }
        public RelayCommand CancelReviewCommand { get; set; }
        public RelayCommand RemoveImageCommand { get; set; }
        public RelayCommand AddImageCommand { get; set; }

        private Frame frame;
        public Frame Frame
        {
            get { return frame; }
            set { frame = value; }
        }
        public ObservableCollection<string> Images { get; set; }
        private int _cleanliness;
        public int Cleanliness
        {
            get => _cleanliness;
            set
            {
                if (value != _cleanliness)
                {
                    _cleanliness = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _correctness;
        public int Correcntess
        {
            get => _correctness;
            set
            {
                if (value != _correctness)
                {
                    _correctness = value;
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public RatingReservationViewModel(RatingReservationView ratingReservationView, AccommodationReservationService reservationService, AccommodationReservation reservation)
        {
            _accommodationReservationService = reservationService;
            _ratingService = new RatingGivenByGuestService();
            RatingReservationView = ratingReservationView;
            Reservation = reservation;
            Images = new ObservableCollection<string>();
            InitCommands();
        }
        public void ExecutedReviewReservationCommand(object obj)
        {
            _ratingService.Add(new RatingGivenByGuest(Reservation.Id, Cleanliness, Correcntess, AdditionalComment, new List<string>(Images)));
            this.Frame.Navigate(new ReservationsView(_accommodationReservationService, Reservation.Guest));
        }
        public void ExecutedCancelReviewCommand(object obj)
        {
            this.Frame.Navigate(new ReservationsView(_accommodationReservationService, Reservation.Guest));
        }
        public void ExecutedRemoveImageCommand(object obj)
        {
            if (RatingReservationView.lbImages.SelectedItem != null)
            {
                Images.RemoveAt(RatingReservationView.lbImages.SelectedIndex);
            }

        }
        public void ExecutedAddImageCommand(object obj)
        {
            if (!ImageUrl.Equals(""))
            {
                Images.Add(ImageUrl);
                ImageUrl = "";
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
        }
    }
}
