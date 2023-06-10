﻿using SIMS_HCI_Project.Domain.RepositoryInterfaces;
using SIMS_HCI_Project.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Injector
{
    public class Injector
    {
        private static Dictionary<Type, object> _implementations = new Dictionary<Type, object>
        {
            { typeof(IUserRepository), new UserRepository() },
            { typeof(ILocationRepository), new LocationRepository() },
            { typeof(IAccommodationRepository), new AccommodationRepository() },
            { typeof(IAccommodationReservationRepository), new AccommodationReservationRepository() },
            { typeof(IRescheduleRequestRepository), new RescheduleRequestRepository() },
            { typeof(IRatingGivenByGuestRepository), new RatingGivenByGuestRepository() },
            { typeof(IRatingGivenByOwnerRepository), new RatingGivenByOwnerRepository() },
            { typeof(IRenovationRecommendationRepository), new RenovationRecommendationRepository() },
            { typeof(ISuperGuestTitleRepository), new SuperGuestTitleRepository() },
            { typeof(IRenovationRepository), new RenovationRepository() },
            { typeof(INotificationRepository), new NotificationRepository() },
            { typeof(ITourReservationRepository), new TourReservationRepository() },
            { typeof(ITourTimeRepository), new TourTimeRepository() },
            { typeof(ITourVoucherRepository), new TourVoucherRepository() },
            { typeof(ITourRepository), new TourRepository() },
            { typeof(ITourKeyPointRepository), new TourKeyPointRepository() },
            { typeof(IGuestTourAttendanceRepository), new GuestTourAttendanceRepository() },
            { typeof(ITourRatingRepository), new TourRatingRepository() },
            { typeof(IRegularTourRequestRepository), new RegularTourRequestRepository() },
            { typeof(IForumRepository), new ForumRepository() },
            { typeof(IForumCommentRepository), new ForumCommentRepository() },
            { typeof(IForumCommentReportRepository), new ForumCommentReportRepository() },
            { typeof(ISuperGuideFlagRepository), new SuperGuideFlagRepository() },
            { typeof(IComplexTourRequestRepository), new ComplexTourRequestRepository() }
        };

        public static T CreateInstance<T>()
        {
            Type type = typeof(T);

            if (_implementations.ContainsKey(type))
            {
                return (T)_implementations[type];
            }

            throw new ArgumentException($"No implementation found for type {type}");
        }
    }
}
