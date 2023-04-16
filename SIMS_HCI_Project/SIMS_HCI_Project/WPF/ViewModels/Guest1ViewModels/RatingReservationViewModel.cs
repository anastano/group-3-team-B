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

        private Frame frame;
        public Frame Frame
        {
            get { return frame; }
            set { frame = value; }
        }

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
        private String _images;
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
            //Guest1MainView = guest1MainView;
            //ReservationsView = reservationsView;
            Reservation = reservation;
            InitCommands();
        }
        public void Executed_ReviewReservationCommand(object obj)
        {
            _ratingService.Add(new RatingGivenByGuest(Reservation.Id, Cleanliness, Correcntess, AdditionalComment, Images));
            //Guest1MainView.MainGuestFrame.Content = ReservationsView;
            this.Frame.Navigate(new ReservationsView(_accommodationReservationService, Reservation.Guest));
        }

        public bool CanExecute_ReviewReservationCommand(object obj)
        {
            return true;
        }
        public void InitCommands()
        {
            ReviewReservationCommand = new RelayCommand(Executed_ReviewReservationCommand, CanExecute_ReviewReservationCommand);
        }
    }
}
