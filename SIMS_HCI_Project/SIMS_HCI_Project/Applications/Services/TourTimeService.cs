using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Applications.Services
{
    public class TourTimeService
    {
        private readonly ITourTimeRepository _tourTimeRepository;

        public TourTimeService()
        {
            _tourTimeRepository = Injector.Injector.CreateInstance<ITourTimeRepository>();
        }

        public TourTime GetById(int id)
        {
            return _tourTimeRepository.GetById(id);
        }

        public List<TourTime> GetAll()
        {
            return _tourTimeRepository.GetAll();
        }

        public List<TourTime> GetAllByGuideId(int id)
        {
            return _tourTimeRepository.GetAllByGuideId(id);
        }

        public void CancelTour(TourTime tourTime, TourVoucherService tourVoucherService, TourReservationService tourReservationService)
        {
            _tourTimeRepository.CancelTour(tourTime);
            List<TourReservation> cancelledReservations = tourReservationService.CancelReservationsByTour(tourTime.TourId);
            tourVoucherService.GiveVouchersToGuestsWithReservation(cancelledReservations);
        }
        public void ConnectGuestAttendances(GuestTourAttendanceService guestTourAttendanceService)
        {
            foreach (TourTime tourTime in _tourTimeRepository.GetAll())
            {
                tourTime.GuestAttendances = guestTourAttendanceService.GetAllByTourId(tourTime.Id);
            }
        }
        public void ConnectCurrentKeyPoints()
        {
            foreach (TourTime tourTime in _tourTimeRepository.GetAll())
            {
                tourTime.CurrentKeyPoint = tourTime.Tour.KeyPoints[tourTime.CurrentKeyPointIndex];
            }
        }
        public void ReduceAvailablePlaces(TourTime selectedTourTime, int requestedPartySize)
        {
            TourTime tourTime = GetById(selectedTourTime.Id);

            tourTime.Available -= requestedPartySize;
        }
        public void CheckAndUpdateStatus()
        {
            _tourTimeRepository.CheckAndUpdateStatus();
        }

        public List<int> GetYearsWithToursByGuide(int guideId)
        {
            return _tourTimeRepository.GetAll().Where(tt => tt.Status == TourStatus.COMPLETED).Select(tt => tt.DepartureTime.Year).Distinct().ToList();
        }
    }
}
