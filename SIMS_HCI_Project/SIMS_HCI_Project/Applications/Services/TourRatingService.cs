using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Repositories;
using SIMS_HCI_Project.Observer;
using SIMS_HCI_Project.Domain.RepositoryInterfaces;

namespace SIMS_HCI_Project.Applications.Services
{
    public class TourRatingService
    {
        private readonly ITourRatingRepository _tourRatingRepository;
        private readonly ITourReservationRepository _tourReservationRepository;
        private readonly IGuestTourAttendanceRepository _guestTourAttendanceRepository;

        public TourRatingService()
        {
            _tourRatingRepository = Injector.Injector.CreateInstance<ITourRatingRepository>();
            _tourReservationRepository = Injector.Injector.CreateInstance<ITourReservationRepository>();
            _guestTourAttendanceRepository = Injector.Injector.CreateInstance<IGuestTourAttendanceRepository>();
        }

        public TourRating GetById(int id)
        {
            return _tourRatingRepository.GetById(id);
        }

        public List<TourRating> GetByTourId(int tourTimeId)
        {
            return _tourRatingRepository.GetByTourId(tourTimeId);
        }

        public bool IsRated(int reservationId)
        {
            return _tourRatingRepository.IsRated(reservationId);
        }
        public List<TourRating> GetAll()
        {
            return _tourRatingRepository.GetAll();
        }

        public void LoadConnections()
        {
            ConnectReservations();
            ConnectAttendances();
        }

        private void ConnectReservations()
        {
            foreach (TourRating tourRating in _tourRatingRepository.GetAll())
            {
                tourRating.TourReservation = _tourReservationRepository.GetById(tourRating.ReservationId);
            }
        }

        private void ConnectAttendances()
        {
            foreach (TourRating tourRating in _tourRatingRepository.GetAll())
            {
                tourRating.Attendance = _guestTourAttendanceRepository.GetByGuestAndTourTimeIds(tourRating.GuestId, tourRating.TourReservation.TourTimeId);
            }
        }

        public void Add(TourRating tourRating)
        {
            _tourRatingRepository.Add(tourRating);
        }

        public void MarkAsInvalid(TourRating tourRating)
        {
            _tourRatingRepository.MarkAsInvalid(tourRating);
        }

        public void NotifyObservers()
        {
            _tourRatingRepository.NotifyObservers();
        }

        public void Subscribe(IObserver observer)
        {
            _tourRatingRepository.Subscribe(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            _tourRatingRepository.Unsubscribe(observer);
        }
    }
}
