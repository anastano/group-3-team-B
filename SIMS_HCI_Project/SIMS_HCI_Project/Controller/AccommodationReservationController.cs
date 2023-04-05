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
    public class AccommodationReservationController : ISubject
    {
        private readonly List<IObserver> _observers;
        private readonly AccommodationReservationFileHandler _fileHandler;

        private static List<AccommodationReservation> _reservations;
        private readonly OwnerController _ownerController;

        public AccommodationReservationController()
        {
            _fileHandler = new AccommodationReservationFileHandler();
            Load();
            _observers = new List<IObserver>();
            ConvertReservedAccommodationsIntoCompleted(DateTime.Now);

            _ownerController = new OwnerController();

        }
        public void ConnectAccommodationsWithReservations(AccommodationController accommodationController)
        {
            foreach (AccommodationReservation reservation in _reservations)
            {
                reservation.Accommodation = accommodationController.FindById(reservation.AccommodationId);
            }
        }
        public void ConvertReservedAccommodationsIntoCompleted(DateTime currentDate)
        {
            foreach(var reservation in _reservations)
            {
                if(reservation.End < currentDate && reservation.Status == AccommodationReservationStatus.RESERVED)
                {
                    reservation.Status = AccommodationReservationStatus.COMPLETED;
                }
            }
            Save();
        }
        public List<AccommodationReservation> GetAll()
        {
            return _reservations;
        }
        public void Load()
        {
            _reservations = _fileHandler.Load();
        }
        public void Save()
        {
            _fileHandler.Save(_reservations);
        }
        public int GenerateId()
        {
            if (_reservations.Count == 0)
            {
                return 1;
            }
            return _reservations[_reservations.Count - 1].Id + 1;
        }
        public void Add(Accommodation accommodation, AccommodationReservation reservation, Guest1 guest)
        {
            reservation.Id = GenerateId();
            _reservations.Add(reservation);
            accommodation.Reservations.Add(reservation);
            guest.Reservations.Add(reservation);
            Save();
            NotifyObservers();
        }
        public void Remove(AccommodationReservation reservation)
        {
            // TO DO
        }
        public void EditStatus(int id, AccommodationReservationStatus status)
        {
            AccommodationReservation reservation = _reservations.Find(r => r.Id == id);
            reservation.Status = status;
            Save();
            NotifyObservers();
        }
        public AccommodationReservation FindById(int id)
        {
            return _reservations.Find(r => r.Id == id);
        }

        public void FillOwnerReservationList(int ownerId) //CHANGE THIS AS SOON AS YOU CAN (FindAll(r=> r.Accommodation.OwnerId == ownerId))
        {
            AccommodationController accommodationController = new AccommodationController();

            _ownerController.FindById(ownerId).Reservations.Clear();

            foreach (AccommodationReservation reservation in GetAll())
            {
                int accommodationOwnerId = accommodationController.FindById(reservation.AccommodationId).OwnerId;

                if (accommodationOwnerId == ownerId)
                {
                    _ownerController.FindById(ownerId).Reservations.Add(reservation);
                }
            }
        }

        public void AddReservationToOwner(AccommodationReservation reservation)   /// be sure to add this when making new reservations (CHECK THIS when Miljana finishes adding reservations)
        {
            AccommodationController accommodationController = new AccommodationController();

            Owner owner = _ownerController.FindById(accommodationController.FindById(reservation.AccommodationId).OwnerId);
            owner.Reservations.Add(reservation);
        }

        public List<AccommodationReservation> GetReservationsInProgress(int ownerId)
        {
            List<AccommodationReservation> reservationsInProgress = new List<AccommodationReservation>();

            foreach (AccommodationReservation reservation in _ownerController.FindById(ownerId).Reservations)
            {
                if (IsInProgress(reservation))
                {
                    reservationsInProgress.Add(reservation);
                }
            }
            return reservationsInProgress;
        }


        public bool IsInProgress(AccommodationReservation reservation)
        {
            return DateTime.Today >= reservation.Start && DateTime.Today <= reservation.End;
        }

        public List<AccommodationReservation> Search(string accommodationName, string guestName, int ownerId) //CHANGE ID TO NAME HERE WHEN YOU GET ACCOMMODATION OBJECT
        {
            List<AccommodationReservation> reservations = _ownerController.FindById(ownerId).Reservations;
            
            var filtered = from _reservation in reservations
                           where (string.IsNullOrEmpty(accommodationName) || _reservation.AccommodationId.ToString().ToLower().Contains(accommodationName.ToLower()))
                           && (string.IsNullOrEmpty(guestName) || _reservation.GuestId.ToString().ToLower().Contains(guestName.ToLower()))
                           select _reservation;
            
            return filtered.ToList();
        }

        public List<AccommodationReservation> GetAllByGuestId(int id)
        {
            return _reservations.FindAll(g => g.GuestId == id);
        }
        public List<AccommodationReservation> GetAllByStatusAndGuestId(int id, AccommodationReservationStatus status)
        {
            return _reservations.FindAll(g => g.GuestId == id && g.Status==status);
        }
        public static List<AccommodationReservation> GetAllByAccommodationId(int id)
        {
            return _reservations.FindAll(a => a.AccommodationId == id);
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
