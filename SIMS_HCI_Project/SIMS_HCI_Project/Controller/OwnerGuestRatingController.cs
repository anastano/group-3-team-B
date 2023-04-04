using SIMS_HCI_Project.FileHandler;
using SIMS_HCI_Project.Model;
using SIMS_HCI_Project.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Controller
{
    public class OwnerGuestRatingController : ISubject
    {
        private readonly List<IObserver> _observers;
        private readonly OwnerGuestRatingFileHandler _fileHandler;

        private static List<OwnerGuestRating> _ratings;

        private readonly OwnerController _ownerController;

        public OwnerGuestRatingController()
        {
            if (_ratings == null)
            {
                _ratings = new List<OwnerGuestRating>();
            }

            _fileHandler = new OwnerGuestRatingFileHandler();
            _observers = new List<IObserver>();

            _ownerController = new OwnerController();

        }

        public List<OwnerGuestRating> GetAll()
        {
            return _ratings;
        }

        public void Load()
        {
            _ratings = _fileHandler.Load();
        }

        public void Save()
        {
            _fileHandler.Save(_ratings);
        }

        public int GenerateId()
        {
            if (_ratings.Count == 0)
            {
                return 1;
            }
            return _ratings[_ratings.Count - 1].Id + 1;

        }

        public void Add(OwnerGuestRating rating)
        {
            rating.Id = GenerateId();
            _ratings.Add(rating);
            Save();
            NotifyObservers();

        }

        public OwnerGuestRating FindById(int id)
        {
            return _ratings.Find(r => r.Id == id);
        }

        public List<AccommodationReservation> GetUnratedReservations(int ownerId)
        {
            List<AccommodationReservation> unratedReservations = new List<AccommodationReservation>();

            foreach (AccommodationReservation reservation in _ownerController.FindById(ownerId).Reservations)
            {
                if (IsCompleted(reservation) && IsWithinFiveDaysAfterCheckout(reservation) && !IsRated(reservation))
                {
                    unratedReservations.Add(reservation);
                }
            }
            return unratedReservations;
        }

        public bool IsCompleted(AccommodationReservation reservation)
        {
            return reservation.Status == ReservationStatus.COMPLETED;
        }

        public bool IsWithinFiveDaysAfterCheckout(AccommodationReservation reservation)
        {
            return DateTime.Today <= reservation.End.AddDays(5);
        }

        public bool IsRated(AccommodationReservation reservation)
        {
            if (_ratings.Find(r => r.ReservationId == reservation.Id) != null)
            {
                return true;
            }
            return false;
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
