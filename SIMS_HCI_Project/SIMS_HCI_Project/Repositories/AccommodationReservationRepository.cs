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
    public class AccommodationReservationRepository : ISubject, IAccommodationReservationRepository
    {
        private readonly List<IObserver> _observers;
        private readonly AccommodationReservationFileHandler _fileHandler;

        private static List<AccommodationReservation> _reservations;

        public AccommodationReservationRepository()
        {
            _fileHandler = new AccommodationReservationFileHandler();          
            _reservations = _fileHandler.Load();

            _observers = new List<IObserver>();
        }
        public void ConvertReservedReservationIntoCompleted(DateTime currentDate)
        {
            foreach (var reservation in _reservations)
            {
                if (reservation.End < currentDate && reservation.Status == AccommodationReservationStatus.RESERVED)
                {
                    reservation.Status = AccommodationReservationStatus.COMPLETED;
                }
            }
            Save();
        }
        public int GenerateId()
        {
            return _reservations.Count == 0 ? 1 : _reservations[_reservations.Count - 1].Id + 1;
        }

        public void Save()
        {
            _fileHandler.Save(_reservations);
        }

        public AccommodationReservation GetById(int id)
        {
            return _reservations.Find(r => r.Id == id);
        }

        public List<AccommodationReservation> GetAll()
        {
            return _reservations;
        }

        public List<AccommodationReservation> GetByOwnerId(int ownerId)
        {
            return _reservations.FindAll(r => r.Accommodation.OwnerId == ownerId);
        }
        public List<AccommodationReservation> GetByGuestId(int guestId)
        {
            return _reservations.FindAll(r => r.GuestId == guestId);
        }

        public List<AccommodationReservation> GetByAccommodationId(int accommodationId)
        {
            return _reservations.FindAll(r => r.AccommodationId == accommodationId);
        }
        public List<AccommodationReservation> GetAllByStatusAndGuestId(int id, AccommodationReservationStatus status)
        {
            return _reservations.FindAll(g => g.GuestId == id && g.Status == status);
        }

        public void EditStatus(int reservationId, AccommodationReservationStatus status)
        {
            AccommodationReservation reservation = _reservations.Find(r => r.Id == reservationId);
            reservation.Status = status;
            Save();
            NotifyObservers();
        }

        public void EditReservation(RescheduleRequest request)
        {
            AccommodationReservation reservation = _reservations.Find(r => r.Id == request.AccommodationReservationId);
            reservation.Start = request.WantedStart;
            reservation.End = request.WantedEnd;
            reservation.Status = AccommodationReservationStatus.RESCHEDULED;
            Save();
            NotifyObservers();
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
