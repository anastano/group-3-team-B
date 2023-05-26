using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.FileHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Repositories
{
    public class RatingGivenByOwnerRepository : IRatingGivenByOwnerRepository
    {
        private readonly RatingGivenByOwnerFileHandler _fileHandler;

        private static List<RatingGivenByOwner> _ratings;

        public RatingGivenByOwnerRepository()
        {
            _fileHandler = new RatingGivenByOwnerFileHandler();
            if (_ratings == null)
            {
                _ratings = _fileHandler.Load();
            }
        }

        public int GenerateId()
        {
            return _ratings.Count == 0 ? 1 : _ratings[_ratings.Count - 1].Id + 1;
        }

        public void Save()
        {
            _fileHandler.Save(_ratings);
        }

        public RatingGivenByOwner GetById(int id)
        {
            return _ratings.Find(r => r.Id == id);
        }

        public RatingGivenByOwner GetByReservationId(int reservationId)
        {
            return _ratings.Find(r => r.ReservationId == reservationId);
        }
        public List<RatingGivenByOwner> GetByGuestId(int guestId)
        {
            return _ratings.FindAll(r => r.Reservation.GuestId == guestId);
        }
        public List<RatingGivenByOwner> GetAll()
        {
            return _ratings;
        }
        public void Add(RatingGivenByOwner rating)
        {
            rating.Id = GenerateId();
            _ratings.Add(rating);
            Save();
        }
        public int GetRatingCountForCategory(int guestId, string categoryName, int ratingValue)
        {
            if (categoryName.ToLower() == "rulecompliance")
            {
                return GetRatingCountForRuleCompliance(guestId, ratingValue);
            }
            else
            {
                return GetRatingCountForCleanliness(guestId, ratingValue);
            }
        }
        public int GetRatingCountForCleanliness(int guestId, int ratingValue)
        {
            return _ratings.FindAll(r => r.Reservation.GuestId == guestId && r.Cleanliness == ratingValue).Count;
        }
        public int GetRatingCountForRuleCompliance(int guestId, int ratingValue)
        {
            return _ratings.FindAll(r => r.Reservation.GuestId == guestId && r.RuleCompliance == ratingValue).Count;
        }
        public double GetAverageRatingForCleanliness(int guestId)
        {
            return _ratings.Where(r => r.Reservation.GuestId == guestId).Average(r => r.Cleanliness);
        }
        public double GetAverageRatingForRuleCompliance(int guestId)
        {
            return _ratings.Where(r => r.Reservation.GuestId == guestId).Average(r => r.RuleCompliance);
        }
    }
}
