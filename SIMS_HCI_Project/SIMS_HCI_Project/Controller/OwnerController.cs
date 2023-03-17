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
    public class OwnerController : ISubject
    {
        private readonly List<IObserver> _observers;
        private readonly OwnerFileHandler _fileHandler;

        private static List<Owner> _owners;

        public OwnerController()
        {
            if (_owners == null)
            {
                _owners = new List<Owner>();
            }

            _fileHandler = new OwnerFileHandler();
            _observers = new List<IObserver>();

        }

        public List<Owner> GetAll()
        {
            return _owners;
        }

        public void Load()
        {
            _owners = _fileHandler.Load();
        }


        public void Save() 
        {
            _fileHandler.Save(_owners);
        }

        public void Add(Accommodation accommodation)
        {
            //TO DO IF NEEDED
        }

        public void Remove(Accommodation accommodation)
        {
            //TO DO IF NEEDED
        }

        public void Edit(Accommodation accommodation)
        {
            //TO DO IF NEEDED
        }

        public Owner FindById(string id)
        {
            return _owners.Find(o => o.Id == id);
        }

        public void FillOwnerAccommodationList()
        {
            AccommodationController accommodationController = new AccommodationController();

            foreach(Owner owner in _owners)
            {
                owner.Accommodations.Clear();

                foreach(Accommodation accommodation in accommodationController.GetAll())
                {
                    if(accommodation.OwnerId == owner.Id)
                    {
                        owner.Accommodations.Add(accommodation);
                    }
                }
            }
        }

        public void FillOwnerReservationList()
        {
            AccommodationController accommodationController = new AccommodationController();
            AccommodationReservationController reservationController = new AccommodationReservationController();

            foreach (Owner owner in _owners)
            {
                owner.Reservations.Clear();

                foreach (AccommodationReservation reservation in reservationController.GetAll())
                {
                    string accommodationOwnerId = accommodationController.FindById(reservation.AccommodationId).OwnerId;

                    if ( accommodationOwnerId == owner.Id)
                    {
                        owner.Reservations.Add(reservation);
                    }
                }
            }
        }


        public void AddAccommodationToOwner(Accommodation accommodation)
        {
            Owner owner = FindById(accommodation.OwnerId);
            owner.Accommodations.Add(accommodation);
        }

        public void AddReservationToOwner(AccommodationReservation reservation)   /// be sure to add this when making new reservations (CHECK THIS when Miljana finishes adding reservations)
        {
            AccommodationController accommodationController = new AccommodationController();

            Owner owner = FindById(accommodationController.FindById(reservation.AccommodationId).OwnerId);
            owner.Reservations.Add(reservation);
        }

        public List<Accommodation> GetAccommodations(string ownerId)
        {
            Owner owner = FindById(ownerId);
            return owner.Accommodations;
        }

        public List<AccommodationReservation> GetReservations(string ownerId)
        {
            Owner owner = FindById(ownerId);
            return owner.Reservations;
        }

        public List<AccommodationReservation> GetUnratedReservations(string ownerId)
        {
            List<AccommodationReservation> unratedReservations = new List<AccommodationReservation>();

            foreach (AccommodationReservation reservation in GetReservations(ownerId))
            {
                if(IsCompleted(reservation) && IsWithinFiveDaysAfterCheckout(reservation) &&  !IsRated(reservation))
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
            if(DateTime.Today <= reservation.End.AddDays(5))
            {
                return true;
            }
            return false;
        }

        public bool IsRated(AccommodationReservation reservation)
        {
            OwnerGuestRatingController ratingController = new OwnerGuestRatingController();

            foreach  (OwnerGuestRating rating in ratingController.GetAll())
            {
                if(rating.ReservationId == reservation.Id)
                {
                    return true;
                }
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
