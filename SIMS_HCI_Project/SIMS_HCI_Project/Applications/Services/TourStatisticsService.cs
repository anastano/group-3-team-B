using SIMS_HCI_Project.Domain.DTOs;
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
    public class TourStatisticsService
    {
        private readonly IGuestTourAttendanceRepository _guestTourAttendanceRepository;

        public TourStatisticsService()
        {
            _guestTourAttendanceRepository = Injector.Injector.CreateInstance<IGuestTourAttendanceRepository>();
        }

        public TourStatisticsInfo GetTourStatistics(int tourTimeId)
        {
            List<AgeGroup> wantedAgeGroups = new List<AgeGroup> { new AgeGroup(0, 18), new AgeGroup(18, 50), new AgeGroup(50, 150) }; // move this to some global settings 
            
            Dictionary<AgeGroup, int> guestNumberByAgeGroup = new Dictionary<AgeGroup, int>();
            foreach (AgeGroup ageGroup in wantedAgeGroups)
            {
                guestNumberByAgeGroup.Add(ageGroup, _guestTourAttendanceRepository.GetGuestCountByAgeGroup(ageGroup, tourTimeId));
            }

            int guestsWithVoucher = _guestTourAttendanceRepository.GetGuestsWithVoucherCount(tourTimeId);
            int totalGuests = guestNumberByAgeGroup.Values.Sum();
            if (totalGuests == 0) totalGuests = 1;

            return new TourStatisticsInfo(guestNumberByAgeGroup, ((double)guestsWithVoucher / (double)totalGuests) * 100);
        }

        public TourTime GetTopTour()
        {
            return _guestTourAttendanceRepository.GetAll().
                Where(gta => gta.TourReservation.TourTime.Status == TourStatus.COMPLETED).
                GroupBy(gta => gta.TourReservation.TourTimeId).
                OrderByDescending(gta => gta.Count()).
                First().First().TourReservation.TourTime;
        }

        public TourTime GetTopTourByYear(int year)
        {
            var attendancesToCheck = _guestTourAttendanceRepository.GetAll().
                                                                    Where(gta => gta.TourReservation.TourTime.DepartureTime.Year == year
                                                                    && gta.TourReservation.TourTime.Status == TourStatus.COMPLETED);
            if (attendancesToCheck.Count() == 0) return null;

            return attendancesToCheck.GroupBy(gta => gta.TourReservation.TourTimeId)
                                    .OrderByDescending(gta => gta.Count())
                                    .First().First().TourReservation.TourTime;
        }
    }
}
