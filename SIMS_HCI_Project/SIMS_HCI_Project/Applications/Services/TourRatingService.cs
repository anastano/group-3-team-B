using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Repositories;

using SIMS_HCI_Project.Domain.RepositoryInterfaces;

namespace SIMS_HCI_Project.Applications.Services
{
    public class TourRatingService
    {
        private readonly ITourRatingRepository _tourRatingRepository;

        public TourRatingService()
        {
            _tourRatingRepository = Injector.Injector.CreateInstance<ITourRatingRepository>();
        }

        public List<TourRating> GetAllByTourId(int tourTimeId)
        {
            return _tourRatingRepository.GetAllByTourId(tourTimeId);
        }

        public void Add(TourRating tourRating)
        {
            _tourRatingRepository.Add(tourRating);

            SuperFlagsService superFlagsService = new SuperFlagsService();
            superFlagsService.ReviseGuideFlagStatusForLanguage(tourRating.Attendance.TourReservation.TourTime.Tour.GuideId, tourRating.Attendance.TourReservation.TourTime.Tour.Language);
        }

        public bool IsRated(int reservationId)
        {
            return _tourRatingRepository.IsRated(reservationId);
        }

        public void MarkAsInvalid(TourRating tourRating)
        {
            tourRating.IsValid = false;
            _tourRatingRepository.Update(tourRating);
        }
    }
}
