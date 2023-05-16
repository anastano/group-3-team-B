using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.FileHandlers;
using SIMS_HCI_Project.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Repositories
{
    public class RatingGivenByOwnerRepository : ISubject, IRatingGivenByOwnerRepository
    {
        private readonly List<IObserver> _observers;
        private readonly RatingGivenByOwnerFileHandler _fileHandler;

        private static List<RatingGivenByOwner> _ratings;

        public RatingGivenByOwnerRepository()
        {
            _fileHandler = new RatingGivenByOwnerFileHandler();
            if(_ratings == null)
            {
                _ratings = _fileHandler.Load();
            }
            _observers = new List<IObserver>();
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
            NotifyObservers();
        }
        public int GetRatingCountForCategory(int guestId, string categoryName, int ratingValue)
        {
            if(categoryName.ToLower() == "rulecompliance")
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
        public void NotifyObservers()
        {
            foreach (var observer in _observers)
            {
                observer.Update();
            }
        }

        public void Subscribe(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            _observers.Remove(observer);
        }
    }
}
