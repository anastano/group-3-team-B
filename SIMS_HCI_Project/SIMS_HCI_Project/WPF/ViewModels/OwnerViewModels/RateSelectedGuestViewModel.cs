using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Validations;
using SIMS_HCI_Project.WPF.Views.OwnerViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace SIMS_HCI_Project.WPF.ViewModels.OwnerViewModels
{
    public class RateSelectedGuestViewModel : INotifyPropertyChanged
    {
        private readonly RatingGivenByOwnerService _ownerRatingService;
        public RateSelectedGuestView RateSelectedGuestView { get; set; }
        public UnratedReservationsViewModel UnratedReservationsVM { get; set; }
        public RatingGivenByOwner Rating { get; set; }
        public AccommodationReservation SelectedReservation { get; set; }

        #region OnPropertyChanged

        private string _additionalComment;
        public string AdditionalComment
        {
            get => _additionalComment;
            set
            {
                if (value != _additionalComment)
                {

                    _additionalComment = value;
                    OnPropertyChanged(nameof(AdditionalComment));
                }
            }
        }

        private RatingGivenByOwnerValidation _validatedRating;
        public RatingGivenByOwnerValidation ValidatedRating
        {
            get => _validatedRating;
            set
            {
                if (value != _validatedRating)
                {

                    _validatedRating = value;
                    OnPropertyChanged(nameof(ValidatedRating));
                }
            }
        }

        #endregion

        public RelayCommand RateGuestCommand { get; set; }
        public RelayCommand CloseRateSelectedGuestViewCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public RateSelectedGuestViewModel(RateSelectedGuestView rateSelectedGuestView, UnratedReservationsViewModel unratedReservationsVM, AccommodationReservation selectedReservation) 
        {
            InitCommands();
            _ownerRatingService = new RatingGivenByOwnerService();
            RateSelectedGuestView = rateSelectedGuestView;
            UnratedReservationsVM = unratedReservationsVM;
            ValidatedRating = new RatingGivenByOwnerValidation();
            Rating = new RatingGivenByOwner();
            SelectedReservation = selectedReservation;
        }

        #region Commands

        public void Executed_RateGuestCommand(object obj)
        {
            ValidatedRating.Validate();
            if (ValidatedRating.IsValid)
            {
                    Rating.ReservationId = SelectedReservation.Id;
                    Rating.Cleanliness = ValidatedRating.Cleanliness ?? 0;
                    Rating.RuleCompliance = ValidatedRating.RuleCompliance ?? 0;
                    Rating.Reservation = SelectedReservation;
                    Rating.AdditionalComment = AdditionalComment;
                    _ownerRatingService.Add(Rating);
                    RateSelectedGuestView.Close();
                    UnratedReservationsVM.UpdateUnratedReservations();
            }
            else
            {
                MessageBox.Show("Not all fields are fill in correctly.");
            }
        }

        public bool CanExecute_RateGuestCommand(object obj)
        {
                return true;
        }

        public void Executed_CloseRateSelectedGuestViewCommand(object obj)
        {
            RateSelectedGuestView.Close();
        }

        public bool CanExecute_CloseRateSelectedGuestViewCommand(object obj)
        {
            return true;
        }
        #endregion

        public void InitCommands()
        {
            RateGuestCommand = new RelayCommand(Executed_RateGuestCommand, CanExecute_RateGuestCommand);
            CloseRateSelectedGuestViewCommand = new RelayCommand(Executed_CloseRateSelectedGuestViewCommand, CanExecute_CloseRateSelectedGuestViewCommand);
        }      

    }
}
