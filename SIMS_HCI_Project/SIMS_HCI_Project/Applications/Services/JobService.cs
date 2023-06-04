using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Domain.RepositoryInterfaces;
using SIMS_HCI_Project.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Applications.Services
{
    class JobService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITourTimeRepository _tourTimeRepository;
        private readonly ITourReservationRepository _tourReservationRepository;
        private readonly ITourVoucherRepository _tourVoucherRepository;

        public JobService()
        {
            _userRepository = Injector.Injector.CreateInstance<IUserRepository>();
            _tourTimeRepository = Injector.Injector.CreateInstance<ITourTimeRepository>();
            _tourReservationRepository = Injector.Injector.CreateInstance<ITourReservationRepository>();
            _tourVoucherRepository = Injector.Injector.CreateInstance<ITourVoucherRepository>();
        }

        public void GuideResign(Guide guide)
        {
            guide.ActiveAccount = false;
            _userRepository.Update(guide);

            foreach(Tour tour in guide.AllTours)
            {
                CancelTourTimes(tour);
            }
        }

        private void CancelTourTimes(Tour tour)
        {
            foreach(TourTime tourTime in tour.DepartureTimes)
            {
                if (tourTime.IsCanceled || tourTime.IsCompleted) continue;

                tourTime.Cancel();
                _tourTimeRepository.Update(tourTime);
                CancelReservations(tourTime);
            }
        }

        private void CancelReservations(TourTime tourTime)
        {
            List<TourReservation> tourReservationsToCancel = _tourReservationRepository.GetAllByTourTimeId(tourTime.Id);
            List<TourVoucher> givenTourVouchers = new List<TourVoucher>();

            foreach (TourReservation tourReservation in tourReservationsToCancel)
            {
                tourReservation.Cancel();
                givenTourVouchers.Add(new TourVoucher(tourReservation.GuestId, "RESIGNVOUCHER", DateTime.Now, DateTime.Now.AddYears(2)));
            }
            _tourReservationRepository.BulkUpdate(tourReservationsToCancel);
            _tourVoucherRepository.AddBulk(givenTourVouchers);
        }
    }
}
