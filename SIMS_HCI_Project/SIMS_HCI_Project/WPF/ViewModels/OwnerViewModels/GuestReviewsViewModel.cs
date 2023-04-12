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
    public class GuestReviewsViewModel
    {

        private readonly RatingGivenByGuestService _guestRatingService;
        private readonly RatingGivenByOwnerService _ownerRatingService;

        public GuestReviewsView GuestReviewsView { get; set; }
        public Owner Owner { get; set; }
        public ObservableCollection<RatingGivenByGuest> GuestReviews { get; set; }
        public RatingGivenByGuest SelectedReview { get; set; }
        public RelayCommand ShowSelectedReviewCommand { get; set; }

        public GuestReviewsViewModel(GuestReviewsView guestReviewsView, RatingGivenByGuestService guestRatingService,
            RatingGivenByOwnerService ownerRatingService, Owner owner)
        {
            InitCommands();

            _guestRatingService = guestRatingService;
            _ownerRatingService = ownerRatingService;

            GuestReviewsView = guestReviewsView;
            Owner = owner;
            GuestReviews = new ObservableCollection<RatingGivenByGuest>(_guestRatingService.GetRatedByOwnerId(_ownerRatingService, Owner.Id));
        }

        #region Commands
        public void Executed_ShowSelectedReviewCommand(object obj)
        {

        }

        public bool CanExecute_ShowSelectedReviewCommand(object obj)
        {
            return true;
        }
        #endregion

        public void InitCommands()
        {
            ShowSelectedReviewCommand = new RelayCommand(Executed_ShowSelectedReviewCommand, CanExecute_ShowSelectedReviewCommand);
        }

    }
}
