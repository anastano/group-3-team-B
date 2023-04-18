using SIMS_HCI_Project.Applications.Services;
using SIMS_HCI_Project.Controller;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Domain.RepositoryInterfaces;
using SIMS_HCI_Project.FileHandlers;
using SIMS_HCI_Project.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Repositories
{
    public class TourReservationRepository : ITourReservationRepository, ISubject
    {
        private readonly TourReservationFileHandler _fileHandler;
        private static List<TourReservation> _reservations;
        private readonly List<IObserver> _observers;

        public TourReservationRepository()
        {
            _fileHandler = new TourReservationFileHandler();
            if (_reservations == null)
            {
                Load();
            }
            _observers = new List<IObserver>();
        }

        public void Load()
        {
            _reservations = _fileHandler.Load();
        }

        public void Save()
        {
            _fileHandler.Save(_reservations);
        }

        public TourReservation GetById(int id)
        {
            return _reservations.Find(r => r.Id == id);
        }

        public List<TourReservation> GetAll()
        {
            return _reservations;
        }

        public List<TourReservation> GetAllByGuestId(int id)
        {
            return _reservations.FindAll(r => r.Guest2Id == id);
        }

        public List<TourReservation> GetAllByTourTimeId(int id)
        {
            return _reservations.FindAll(r => r.TourTimeId == id);
        }

        public TourReservation GetByGuestAndTour(int guestId, int tourTimeId)
        {
            return _reservations.Where(tr => tr.Guest2Id == guestId && tr.TourTimeId == tourTimeId).First();
        }

        // Fix this #New
        public List<TourReservation> GetUnratedReservations(int guestId, GuestTourAttendanceService guestTourAttendanceService, TourRatingService tourRatingService, TourService tourService)
        {
            List<TourReservation> unratedReservations = new List<TourReservation>();
            foreach (TourReservation reservation in GetAllByGuestId(guestId))
            {
                if (IsCompleted(reservation) && WasPresentInTourTime(guestId, reservation.TourTime.Id, guestTourAttendanceService, tourService) && !(tourRatingService.IsRated(reservation.Id)))
                {
                    unratedReservations.Add(reservation);
                }
            }
            return unratedReservations;
        }

        // Fix this #New
        public bool WasPresentInTourTime(int guestId, int tourTimeId, GuestTourAttendanceService guestTourAttendanceService, TourService tourService)
        {
            List<TourTime> toursAttended = guestTourAttendanceService.GetTourTimesWhereGuestWasPresent(guestId, tourService);
            return toursAttended.Any(ta => ta.Id == tourTimeId);
        }

        // Move to model #New
        public bool IsCompleted(TourReservation reservation)
        {
            return reservation.TourTime.Status == TourStatus.COMPLETED;
        }

        public void BulkUpdate(List<TourReservation> tourReservations)
        {
            foreach (TourReservation tourReservation in tourReservations)
            {
                TourReservation toUpdate = GetById(tourReservation.Id);
                toUpdate = tourReservation;
            }

            Save();

            return tourReservations;
        }
        public List<TourReservation> GetAllByGuestId(int id)
        {
            return _reservations.FindAll(r => r.Guest2Id == id);
        }

        public List<TourReservation> GetActiveByGuestId(int id)
        {
            return _reservations.FindAll(r => r.Guest2Id == id && r.TourTime.Status == TourStatus.IN_PROGRESS && r.Status == TourReservationStatus.GOING);
        }

        public List<TourReservation> GetAll()
        {
            return _reservations;
        }

        public TourReservation GetById(int id)
        {
            return _reservations.Find(r => r.Id == id);
        }

        private int GenerateId()
        {
            if (_reservations.Count == 0)
            {
                return 1;
            }
            return _reservations[_reservations.Count - 1].Id + 1;
        }

        public void Add(TourReservation tourReservation)
        {
            tourReservation.Id = GenerateId();
            _reservations.Add(tourReservation);

            Save();
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
