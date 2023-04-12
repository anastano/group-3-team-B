using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Applications.Services
{
    public class TourReservationService
    {
        private readonly ITourReservationRepository _tourReservationRepository;

        public TourReservationService()
        {
            _tourReservationRepository = Injector.Injector.CreateInstance<ITourReservationRepository>();
        }

        public void Load()
        {
            _tourReservationRepository.Load();
        }

        public void Save()
        {
            _tourReservationRepository.Save();
        }

        public List<TourReservation> GetAllByTourTimeId(int id)
        {
            return _tourReservationRepository.GetAllByTourTimeId(id);
        }

        public List<TourReservation> CancelReservationsByTour(int tourTimeId)
        {
            return _tourReservationRepository.CancelReservationsByTour(tourTimeId);
        }

    }
}
