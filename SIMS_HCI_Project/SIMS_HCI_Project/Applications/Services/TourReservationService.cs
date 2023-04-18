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

        public void Load()
        {
            _tourReservationRepository.Load();
        }

        public void Save()
        {
            _tourReservationRepository.Save();
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
        public TourReservation GetById(int id)
        {
            return _tourReservationRepository.GetById(id);
        }
        public List<TourReservation> GetAllByTourTimeId(int id)
        {
            return _tourReservationRepository.GetAllByTourTimeId(id);
        }
        public List<TourReservation> GetAllByGuestId(int id)
        {
            return _tourReservationRepository.GetAllByGuestId(id);
        }

        public List<TourReservation> GetUnratedReservations(int guestId, GuestTourAttendanceService guestTourAttendanceService, TourRatingService tourRatingService, TourTimeService tourTimeService)
        {
            return _tourReservationRepository.GetUnratedReservations(guestId, guestTourAttendanceService, tourRatingService, tourTimeService);
        }
        public List<TourReservation> CancelReservationsByTour(int tourTimeId)
        {
            return _tourReservationRepository.CancelReservationsByTour(tourTimeId);
        }

        public void CancelReservation(int reservationId)
        {
            _tourReservationRepository.CancelReservation(reservationId);
        }

        public void ConnectAvailablePlaces(TourTimeService tourTimeService)
        {
            foreach (TourTime tourTime in tourTimeService.GetAll())
            {
                //private static List<TourReservation> _reservations = new List<TourReservation>();
       var _reservations =_tourReservationRepository.GetAllByTourTimeId(tourTime.Id);
                tourTime.Available = tourTime.Tour.MaxGuests;

                if (_reservations == null)
                {
                    tourTime.Available = tourTime.Tour.MaxGuests;
                    return;
                }

                foreach (TourReservation tourReservation in _reservations)
                {
                    tourTime.Available -= tourReservation.PartySize;
                }

            }
        }

        public void ConnectTourTimes(TourTimeService tourTimeService)
        {
            foreach (TourReservation tourReservation in _tourReservationRepository.GetAll())
            {
                tourReservation.TourTime = tourTimeService.GetById(tourReservation.TourTimeId);
            }
        }

        public void ConnectVouchers(TourVoucherService tourVoucherService)
        {
            foreach (TourReservation tourReservation in _tourReservationRepository.GetAll())
            {
                tourReservation.TourVoucher = tourVoucherService.GetById(tourReservation.VoucherUsedId);
            }
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
