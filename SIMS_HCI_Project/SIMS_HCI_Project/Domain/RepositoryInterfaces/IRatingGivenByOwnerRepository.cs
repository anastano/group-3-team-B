using SIMS_HCI_Project.Domain.Models;
using System.Collections.Generic;

namespace SIMS_HCI_Project.Repositories
{
    public interface IRatingGivenByOwnerRepository
    {
        void Add(RatingGivenByOwner rating);
        List<RatingGivenByOwner> GetAll();
        RatingGivenByOwner GetById(int id);
        RatingGivenByOwner GetByReservationId(int reservationId);
        List<RatingGivenByOwner> GetByGuestId(int ownerId);
        int GetRatingCountForCategory(int guestId, string categoryName, int ratingValue);
        int GetRatingCountForCleanliness(int guestId, int ratingValue);
        int GetRatingCountForRuleCompliance(int guestId, int ratingValue);
        void Save();
    }
}