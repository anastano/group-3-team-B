﻿using SIMS_HCI_Project.Domain.Models;
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
        private readonly IRegularTourRequestRepository _regularTourRequestRepository;
        private readonly IAccommodationRepository _accommodationRepository;
        private readonly IAccommodationReservationRepository _accommodationReservationRepository;
        private readonly IRatingGivenByGuestRepository _ratingGivenByGuestRepository;
        private readonly IRatingGivenByOwnerRepository _ratingGivenByOwnerRepository;
        private readonly IRescheduleRequestRepository _rescheduleRequestRepository;
        private readonly ISuperGuestTitleRepository _superGuestTitleRepository;
        private readonly IRenovationRepository _renovationRepository;
        private readonly IRenovationRecommendationRepository _renovationRecommendationRepository;
        private readonly IForumRepository _forumRepository;
        private readonly IForumCommentRepository _forumCommentRepository;
        private readonly ISuperGuideFlagRepository _superGuideFlagRepository;
        private readonly IComplexTourRequestRepository _complexTourRequestRepository;

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
            _regularTourRequestRepository = Injector.Injector.CreateInstance<IRegularTourRequestRepository>();
            _accommodationRepository = Injector.Injector.CreateInstance<IAccommodationRepository>();
            _accommodationReservationRepository = Injector.Injector.CreateInstance<IAccommodationReservationRepository>();
            _ratingGivenByGuestRepository = Injector.Injector.CreateInstance<IRatingGivenByGuestRepository>();
            _ratingGivenByOwnerRepository = Injector.Injector.CreateInstance<IRatingGivenByOwnerRepository>();
            _rescheduleRequestRepository = Injector.Injector.CreateInstance<IRescheduleRequestRepository>();
            _superGuestTitleRepository = Injector.Injector.CreateInstance<ISuperGuestTitleRepository>();
            _renovationRepository = Injector.Injector.CreateInstance<IRenovationRepository>();
            _renovationRecommendationRepository = Injector.Injector.CreateInstance<IRenovationRecommendationRepository>();
            _forumRepository = Injector.Injector.CreateInstance<IForumRepository>();
            _forumCommentRepository = Injector.Injector.CreateInstance<IForumCommentRepository>();
            _superGuideFlagRepository = Injector.Injector.CreateInstance<ISuperGuideFlagRepository>();
            _complexTourRequestRepository = Injector.Injector.CreateInstance<IComplexTourRequestRepository>();
        }

        public void LoadConnections()
        {
            if (_connectionsLoaded) return;

            ConnectTourFields();
            ConnectGuestAttendanceFields();
            ConnectTourTimeFields();
            ConnectRatingFields();
            ConnectTourReservationFields();
            ConnectRegularTourRequestFields();
            ConnectAccommodationFields();
            ConnectAccommodationReservationFields();
            ConnectRatingGivenByGuestFields();
            ConnectRatingGivenByOwnerFields();
            ConnectRescheduleRequestFields();
            ConnectSuperGuestTitleFields();
            ConnectRenovationFields();
            ConnectRecommendationRenovationFields();
            ConnectGuideFields();
            ConnectForumFields();
            ConnectForumCommentFields();
            ConnectComplexTourRequestFields();

            _connectionsLoaded = true;
        }

        private void ConnectGuideFields()
        {
            foreach(Guide guide in _tourRepository.GetAll().Select(t => t.Guide).Distinct().ToList())
            {
                guide.AllTours = _tourRepository.GetAllByGuide(guide.Id);
                guide.TodaysTours = _tourTimeRepository.GetAllByGuideId(guide.Id).Where(t => t.DepartureTime.Date == DateTime.Today).ToList();
                guide.SuperFlags = _superGuideFlagRepository.GetValidByGuide(guide.Id);
            }
        }

        private void ConnectTourFields()
        {
            foreach (Tour tour in _tourRepository.GetAll())
            {
                tour.Location = _locationRepository.GetById(tour.LocationId);
                tour.KeyPoints = _tourKeyPointRepository.GetByIds(tour.KeyPointsIds);
                tour.Guide = (Guide) _userRepository.GetById(tour.GuideId);
            }
        }

        private void ConnectTourTimeFields()
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
                guestTourAttendance.TourReservation = _tourReservationRepository.GetById(guestTourAttendance.TourReservationId);
                guestTourAttendance.KeyPointJoined = _tourKeyPointRepository.GetById(guestTourAttendance.KeyPointJoinedId);
            }
        }

        private void ConnectRatingFields()
        {
            foreach (TourRating tourRating in _tourRatingRepository.GetAll())
            {
                tourRating.Attendance = _guestTourAttendanceRepository.GetById(tourRating.AttendanceId);
            }
        }

        private void ConnectTourReservationFields()
        {
            foreach (TourReservation tourReservation in _tourReservationRepository.GetAll())
            {
                tourReservation.TourTime = _tourTimeRepository.GetById(tourReservation.TourTimeId);
                tourReservation.TourVoucher = _tourVoucherRepository.GetById(tourReservation.VoucherUsedId);
                tourReservation.Guest = (Guest2)_userRepository.GetById(tourReservation.GuestId);
            }
        }

        private void ConnectRegularTourRequestFields()
        {
            foreach(RegularTourRequest request in _regularTourRequestRepository.GetAll())
            {
                request.Location = _locationRepository.GetById(request.LocationId);
                request.Guest = new Guest2(_userRepository.GetById(request.GuestId));
                request.Tour = _tourRepository.GetById(request.TourId);
                request.ComplexTourRequest = _complexTourRequestRepository.GetById(request.ComplexTourRequestId);
            }
        }

        private void ConnectComplexTourRequestFields()
        {
            foreach (ComplexTourRequest request in _complexTourRequestRepository.GetAll())
            {
                request.TourRequests = _regularTourRequestRepository.GetAllPartsOfComplex(request.Id);
                request.Guest = new Guest2(_userRepository.GetById(request.GuestId));
            }
        }

        public void ConnectAccommodationFields()
        {
            foreach (Accommodation accommodation in _accommodationRepository.GetAll())
            {
                accommodation.Owner = (Owner)_userRepository.GetById(accommodation.OwnerId);
                accommodation.Location = _locationRepository.GetById(accommodation.LocationId);
            }
        }
        public void ConnectAccommodationReservationFields()
        {
            foreach (AccommodationReservation reservation in _accommodationReservationRepository.GetAll())
            {
                reservation.Accommodation = _accommodationRepository.GetById(reservation.AccommodationId);
                reservation.Guest = (Guest1)_userRepository.GetById(reservation.GuestId);
            }
        }
        public void ConnectRatingGivenByGuestFields()
        {
            foreach (RatingGivenByGuest rating in _ratingGivenByGuestRepository.GetAll())
            {
                rating.Reservation = _accommodationReservationRepository.GetById(rating.ReservationId);
            }
        }
        public void ConnectRatingGivenByOwnerFields()
        {
            foreach (RatingGivenByOwner rating in _ratingGivenByOwnerRepository.GetAll())
            {
                rating.Reservation = _accommodationReservationRepository.GetById(rating.ReservationId);
            }
        }
        public void ConnectRescheduleRequestFields()
        {
            foreach (RescheduleRequest request in _rescheduleRequestRepository.GetAll())
            {
                request.AccommodationReservation = _accommodationReservationRepository.GetById(request.AccommodationReservationId);
            }
        }
        public void ConnectSuperGuestTitleFields()
        {
            foreach (SuperGuestTitle title in _superGuestTitleRepository.GetAll())
            {
                title.Guest = (Guest1)_userRepository.GetById(title.GuestId);
            }
        }
        public void ConnectRenovationFields()
        {
            foreach (Renovation renovation in _renovationRepository.GetAll())
            {
                renovation.Accommodation = _accommodationRepository.GetById(renovation.AccommodationId);
            }
        }
        public void ConnectRecommendationRenovationFields()
        {
            foreach (RenovationRecommendation recommendation in _renovationRecommendationRepository.GetAll())
            {
                recommendation.Rating = _ratingGivenByGuestRepository.GetById(recommendation.RatingId);
            }
        }
        public void ConnectForumFields()
        {
            foreach (Forum forum in _forumRepository.GetAll())
            {
                forum.Location = _locationRepository.GetById(forum.LocationId);
                forum.User = _userRepository.GetById(forum.UserId);
                forum.Comments = _forumCommentRepository.GetByForumId(forum.Id);
            }
        }
        public void ConnectForumCommentFields()
        {
            foreach (ForumComment comment in _forumCommentRepository.GetAll())
            {
                comment.Forum = _forumRepository.GetById(comment.ForumId);
                comment.User = _userRepository.GetById(comment.UserId);
            }
        }

    }
}
