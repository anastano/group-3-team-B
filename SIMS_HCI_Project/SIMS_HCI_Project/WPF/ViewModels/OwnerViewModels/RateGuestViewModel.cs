using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Validations;
using SIMS_HCI_Project.WPF.Views.OwnerViews;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace SIMS_HCI_Project.WPF.ViewModels.OwnerViewModels
{
    public class RateGuestViewModel : INotifyPropertyChanged
    {
        private readonly RatingGivenByOwnerService _ownerRatingService;
        public RateGuestView RateGuestView { get; set; }
        public UnratedGuestsViewModel UnratedGuestsVM { get; set; }
        public RatingGivenByOwner Rating { get; set; }
        public AccommodationReservation Reservation { get; set; }

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
        public RelayCommand CloseViewCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public RateGuestViewModel(RateGuestView rateGuestView, UnratedGuestsViewModel unratedGuestsVM, AccommodationReservation reservation) 
        {
            InitCommands();
            _ownerRatingService = new RatingGivenByOwnerService();
            RateGuestView = rateGuestView;
            UnratedGuestsVM = unratedGuestsVM;
            ValidatedRating = new RatingGivenByOwnerValidation();
            Rating = new RatingGivenByOwner();
            this.Reservation = reservation;
        }

        #region Commands

        public void Executed_RateGuestCommand(object obj)
        {
            ValidatedRating.Validate();
            if (ValidatedRating.IsValid)
            {
                    Rating.ReservationId = Reservation.Id;
                    Rating.Cleanliness = ValidatedRating.Cleanliness ?? 0;
                    Rating.RuleCompliance = ValidatedRating.RuleCompliance ?? 0;
                    Rating.Reservation = Reservation;
                    Rating.AdditionalComment = AdditionalComment;
                    _ownerRatingService.Add(Rating);
                    RateGuestView.Close();
                    UnratedGuestsVM.UpdateUnratedReservations();
            }
            else
            {
                MessageBox.Show("Not all fields are fill in correctly.");
            }
        }

        public void Executed_CloseViewCommand(object obj)
        {
            RateGuestView.Close();
        }

        #endregion

        public void InitCommands()
        {
            RateGuestCommand = new RelayCommand(Executed_RateGuestCommand);
            CloseViewCommand = new RelayCommand(Executed_CloseViewCommand);
        }      

    }
}
