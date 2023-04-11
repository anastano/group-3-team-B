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

        public void Load()
        {
            _reservations = _fileHandler.Load();
        }

        public void Save()
        {
            _fileHandler.Save(_reservations);
        }

        public List<TourReservation> GetAllByTourTimeId(int id)
        {
            return _reservations.FindAll(r => r.TourTimeId == id);
        }

        public List<TourReservation> CancelReservationsByTour(int tourTimeId)
        {
            List<TourReservation> tourReservations = GetAllByTourTimeId(tourTimeId);

            foreach (TourReservation tourReservation in tourReservations)
            {
                tourReservation.Status = TourReservationStatus.CANCELLED;
            }

            Save();

            return tourReservations;
        }
    }
}
