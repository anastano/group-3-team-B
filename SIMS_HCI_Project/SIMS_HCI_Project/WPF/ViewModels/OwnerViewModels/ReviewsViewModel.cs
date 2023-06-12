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
    public class ReviewsViewModel
    {
        private readonly RatingGivenByGuestService _guestRatingService;
        private readonly RatingGivenByOwnerService _ownerRatingService;
        public ReviewsView ReviewsView { get; set; }
        public Owner Owner { get; set; }
        public ObservableCollection<RatingGivenByGuest> Reviews { get; set; }
        public RatingGivenByGuest SelectedReview { get; set; }
        public RelayCommand ShowReviewCommand { get; set; }
        public RelayCommand CloseViewCommand { get; set; }

        public ReviewsViewModel(ReviewsView reviewsView, Owner owner)
        {
            InitCommands();

            _guestRatingService = new RatingGivenByGuestService();
            _ownerRatingService = new RatingGivenByOwnerService();

            ReviewsView = reviewsView;
            Owner = owner;
            Reviews = new ObservableCollection<RatingGivenByGuest>(_guestRatingService.GetRatedByOwnerId(_ownerRatingService, Owner.Id));
        }

        #region Commands
        public void Executed_ShowReviewCommand(object obj)
        {
            if (SelectedReview != null)
            {
                Window selectedGuestReviewView = new ReviewView(ReviewsView, SelectedReview);
                selectedGuestReviewView.Show();
            }
            else
            {
                MessageBox.Show("No review has been selected");
            }
        }

        public void Executed_CloseViewCommand(object obj)
        {
            ReviewsView.Close();
        }

        #endregion

        public void InitCommands()
        {
            ShowReviewCommand = new RelayCommand(Executed_ShowReviewCommand);
            CloseViewCommand = new RelayCommand(Executed_CloseViewCommand);
        }

    }
}
