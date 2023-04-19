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
    public class TourReservationService //TODO: add functions for creating reservation
    {
        private readonly ITourReservationRepository _tourReservationRepository;

        public TourReservationService()
        {
            _tourReservationRepository = Injector.Injector.CreateInstance<ITourReservationRepository>();
        }

        public void Add(TourReservation tourReservation)
        {
            _tourReservationRepository.Add(tourReservation);
        }
       
        public List<TourReservation> GetActiveByGuestId(int id)
        {
            return _tourReservationRepository.GetActiveByGuestId(id);
        }

        public List<TourReservation> GetAll()
        {
            return _tourReservationRepository.GetAll();
        }

        public List<TourReservation> GetAllByGuestId(int id)
        {
            return _tourReservationRepository.GetAllByGuestId(id);
        }

        public List<TourReservation> GetUnratedReservations(int guestId, GuestTourAttendanceService guestTourAttendanceService, TourRatingService tourRatingService, TourService tourService)
        {
            return _tourReservationRepository.GetUnratedReservations(guestId, guestTourAttendanceService, tourRatingService, tourService); // !
        }

        public void ReduceAvailablePlaces(TourService tourService, TourTime selectedTourTime, int requestedPartySize)
        {
            TourTime tourTime = tourService.GetTourInstance(selectedTourTime.Id);

            tourTime.Available -= requestedPartySize;
        }

        public void NotifyObservers()
        {
            _tourReservationRepository.NotifyObservers();
        }

        public void Subscribe(IObserver observer)
        {
            _tourReservationRepository.Subscribe(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            _tourReservationRepository.Unsubscribe(observer);
        }
    }
}
