using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Domain.RepositoryInterfaces;
using SIMS_HCI_Project.Observer;
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
        private readonly ITourTimeRepository _tourTimeRepository;

        public TourReservationService()
        {
            _tourReservationRepository = Injector.Injector.CreateInstance<ITourReservationRepository>();
            _tourTimeRepository = Injector.Injector.CreateInstance<ITourTimeRepository>();
        }

        public List<TourReservation> GetAll()
        {
            return _tourReservationRepository.GetAll();
        }

        public List<TourReservation> GetActiveByGuestId(int id)
        {
            return _tourReservationRepository.GetActiveByGuestId(id);
        }

        public List<TourReservation> GetAllByTourTimeId(int id)
        {
            return _tourReservationRepository.GetAllByTourTimeId(id);
        }

        public List<TourReservation> GetAllByGuestId(int id)
        {
            return _tourReservationRepository.GetAllByGuestId(id);
        }

        public List<TourReservation> GetUnratedReservations(int guestId, GuestTourAttendanceService guestTourAttendanceService, TourRatingService tourRatingService)
        {
            List<TourReservation> unratedReservations = new List<TourReservation>();
            foreach (TourReservation reservation in GetAllByGuestId(guestId))
            {
                if (reservation.TourTime.IsCompleted && WasPresentInTourTime(guestId, reservation.TourTime.Id, guestTourAttendanceService) && !(tourRatingService.IsRated(reservation.Id)))
                {
                    unratedReservations.Add(reservation);
                }
            }
            return unratedReservations;
        }
       
        public void Add(TourReservation tourReservation)
        {
            tourReservation.TourTime = _tourTimeRepository.GetById(tourReservation.TourTimeId);
            _tourReservationRepository.Add(tourReservation);
        }

        public bool WasPresentInTourTime(int guestId, int tourTimeId, GuestTourAttendanceService guestTourAttendanceService)
        {
            List<TourTime> toursAttended = guestTourAttendanceService.GetTourTimesWhereGuestWasPresent(guestId);
            return toursAttended.Any(ta => ta.Id == tourTimeId);
        }

        public void ReduceAvailablePlaces(TourService tourService, TourTime selectedTourTime, int requestedPartySize)
        {
            TourTime tourTime = tourService.GetTourInstance(selectedTourTime.Id);

            tourTime.Available -= requestedPartySize;
        }
    }
}
