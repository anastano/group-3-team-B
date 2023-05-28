using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Domain.RepositoryInterfaces;
using SIMS_HCI_Project.FileHandlers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Repositories
{
    public class TourReservationRepository : ITourReservationRepository
    {
        private readonly TourReservationFileHandler _fileHandler;
        private static List<TourReservation> _reservations;

        public TourReservationRepository()
        {
            _fileHandler = new TourReservationFileHandler();
            if (_reservations == null)
            {
                Load();
            }
        }

        private void Load()
        {
            _reservations = _fileHandler.Load();
        }

        private void Save()
        {
            _fileHandler.Save(_reservations);
        }

        private int GenerateId()
        {
            return _reservations.Count == 0 ? 1 : _reservations[_reservations.Count - 1].Id + 1;
        }

        public TourReservation GetById(int id)
        {
            return _reservations.Find(r => r.Id == id);
        }

        public List<TourReservation> GetAll()
        {
            return _reservations;
        }

        public List<TourReservation> GetAllByTourTimeId(int id)
        {
            return _reservations.FindAll(r => r.TourTimeId == id);
        }

        public List<TourReservation> GetAllByGuestId(int id)
        {
            return _reservations.FindAll(r => r.GuestId == id);
        }

        public List<TourReservation> GetActiveByGuestId(int id)
        {
            return _reservations.FindAll(r => r.GuestId == id && r.TourTime.Status == TourStatus.IN_PROGRESS && r.Status == TourReservationStatus.GOING);
        }
       
        public List<TourReservation> GetAllByGuestIdAndTourId(int guestId, int tourId)
        {
            return _reservations.FindAll(r => r.GuestId == guestId && r.TourTime.Tour.Id == tourId);
        }

        public void Add(TourReservation tourReservation)
        {
            tourReservation.Id = GenerateId();
            _reservations.Add(tourReservation);
            Save();
        }

        public void BulkUpdate(List<TourReservation> tourReservations)
        {
            foreach (TourReservation tourReservation in tourReservations)
            {
                TourReservation toUpdate = GetById(tourReservation.Id);
                toUpdate = tourReservation;
            }
            Save();
        }
    }
}
