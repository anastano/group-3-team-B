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

namespace SIMS_HCI_Project.WPF.ViewModels.Guest1ViewModels
{
    internal class MyRatingsViewModel
    {
        private readonly RatingGivenByOwnerService _ownerRatingService;
        private readonly RatingGivenByGuestService _guestRatingService;
        public Guest1 Guest { get; set; }
        public ObservableCollection<RatingGivenByOwner> RatingsGivenByOwners { get; set; }
        public MyRatingsViewModel(Guest1 guest)
        {
            _ownerRatingService = new RatingGivenByOwnerService();
            _guestRatingService = new RatingGivenByGuestService();
            Guest = guest;
            RatingsGivenByOwners = new ObservableCollection<RatingGivenByOwner>(_ownerRatingService.GetRatedByGuestId(_guestRatingService, Guest.Id));
        }

    }
}
