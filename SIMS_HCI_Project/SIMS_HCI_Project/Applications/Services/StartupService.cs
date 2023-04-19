using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Domain.RepositoryInterfaces;
using SIMS_HCI_Project.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Applications.Services
{
    class StartupService
    {
        private readonly ITourTimeRepository _tourTimeRepository;
        private readonly ITourRepository _tourRepository;
        private readonly ITourKeyPointRepository _tourKeyPointRepository;
        private readonly ITourRatingRepository _tourRatingRepository;
        private readonly IGuestTourAttendanceRepository _guestTourAttendanceRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly ITourVoucherRepository _tourVoucherRepository;
        private readonly ITourReservationRepository _tourReservationRepository;
        private readonly IUserRepository _userRepository;

        private static bool _connectionsLoaded = false;

        public StartupService()
        {
            _tourTimeRepository = Injector.Injector.CreateInstance<ITourTimeRepository>();
            _tourRepository = Injector.Injector.CreateInstance<ITourRepository>();
            _tourKeyPointRepository = Injector.Injector.CreateInstance<ITourKeyPointRepository>();
            _tourRatingRepository = Injector.Injector.CreateInstance<ITourRatingRepository>();
            _guestTourAttendanceRepository = Injector.Injector.CreateInstance<IGuestTourAttendanceRepository>();
            _locationRepository = Injector.Injector.CreateInstance<ILocationRepository>();
            _tourVoucherRepository = Injector.Injector.CreateInstance<ITourVoucherRepository>();
            _userRepository = Injector.Injector.CreateInstance<IUserRepository>();
            _tourReservationRepository = Injector.Injector.CreateInstance<ITourReservationRepository>();
        }

        public void LoadConnections()
        {
            if (_connectionsLoaded) return;

            ConnectTourFields();
            ConnectTourTimeFields();
            ConnectGuestAttendanceFields();
            ConnectRatingFields();
            ConnectTourReservationFields();
            _connectionsLoaded = true;
        }

        public void ConnectTourFields()
        {
            foreach (Tour tour in _tourRepository.GetAll())
            {
                tour.Location = _locationRepository.GetById(tour.LocationId);
                tour.KeyPoints = _tourKeyPointRepository.GetByIds(tour.KeyPointsIds);
                tour.Guide = new Guide(_userRepository.GetById(tour.GuideId));
            }
        }

        public void ConnectTourTimeFields()
        {
            foreach (TourTime tourTime in _tourTimeRepository.GetAll())
            {
                tourTime.Tour = _tourRepository.GetById(tourTime.TourId);
                tourTime.Tour.DepartureTimes.Add(tourTime);
                tourTime.CurrentKeyPoint = tourTime.Tour.KeyPoints[tourTime.CurrentKeyPointIndex];
                tourTime.GuestAttendances = _guestTourAttendanceRepository.GetAllByTourId(tourTime.Id);

                List<TourReservation> reservations = _tourReservationRepository.GetAllByTourTimeId(tourTime.Id);
                tourTime.Available = tourTime.Tour.MaxGuests - reservations.Select(r => r.PartySize).Sum();
            }
        }

        private void ConnectGuestAttendanceFields()
        {
            foreach (GuestTourAttendance guestTourAttendance in _guestTourAttendanceRepository.GetAll())
            {
                guestTourAttendance.Guest = new Guest2(_userRepository.GetById(guestTourAttendance.GuestId));
                guestTourAttendance.TourTime = _tourTimeRepository.GetById(guestTourAttendance.TourTimeId);
                guestTourAttendance.TourReservation = _tourReservationRepository.GetByGuestAndTour(guestTourAttendance.GuestId, guestTourAttendance.TourTimeId);
                guestTourAttendance.KeyPointJoined = _tourKeyPointRepository.GetById(guestTourAttendance.KeyPointJoinedId);
            }
        }

        private void ConnectRatingFields()
        {
            foreach (TourRating tourRating in _tourRatingRepository.GetAll())
            {
                tourRating.TourReservation = _tourReservationRepository.GetById(tourRating.ReservationId); 
                tourRating.Attendance = _guestTourAttendanceRepository.GetByGuestAndTourTimeIds(tourRating.GuestId, tourRating.TourReservation.TourTimeId);
            }
        }

        public void ConnectTourReservationFields()
        {
            foreach (TourReservation tourReservation in _tourReservationRepository.GetAll())
            {
                tourReservation.TourTime = _tourTimeRepository.GetById(tourReservation.TourTimeId);
                tourReservation.TourVoucher = _tourVoucherRepository.GetById(tourReservation.VoucherUsedId);
            }
        }
    }
}
