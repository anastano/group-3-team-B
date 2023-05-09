using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.WPF.Commands;
using SIMS_HCI_Project.WPF.Views.OwnerViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SIMS_HCI_Project.WPF.ViewModels.OwnerViewModels
{
    public class RateSelectedGuestViewModel
    {
        private readonly RatingGivenByOwnerService _ownerRatingService;
        public RateSelectedGuestView RateSelectedGuestView { get; set; }
        public RatingGivenByOwner Rating { get; set; }
        public AccommodationReservation SelectedReservation { get; set; }

        public RelayCommand RateGuestCommand { get; set; }
        public RelayCommand CloseRateSelectedGuestViewCommand { get; set; }

        public RateSelectedGuestViewModel(RateSelectedGuestView rateSelectedGuestView, RatingGivenByOwnerService ownerRatingService, 
            AccommodationReservation selectedReservation) 
        {
            InitCommands();
            _ownerRatingService = ownerRatingService;
            RateSelectedGuestView = rateSelectedGuestView;
            Rating = new RatingGivenByOwner();
            SelectedReservation = selectedReservation;
        }       

        #region Commands
        public void Executed_RateGuestCommand(object obj)
        {
            Rating.ReservationId = SelectedReservation.Id;
            Rating.Reservation = SelectedReservation;
            _ownerRatingService.Add(Rating);
            RateSelectedGuestView.Close();
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

        public void Update()
        {

        }
    }
}
