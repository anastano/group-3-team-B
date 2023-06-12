using SIMS_HCI_Project.Domain.DTOs;
using SIMS_HCI_Project.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Applications.Services
{
    public class AccommodationMonthStatisticsService
    {
        private readonly IAccommodationReservationRepository _reservationRepository;
        private readonly IRescheduleRequestRepository _requestRepository;
        private readonly IRenovationRecommendationRepository _recommendationRepository;

        public AccommodationMonthStatisticsService()
        {
            _reservationRepository = Injector.Injector.CreateInstance<IAccommodationReservationRepository>();
            _requestRepository = Injector.Injector.CreateInstance<IRescheduleRequestRepository>();
            _recommendationRepository = Injector.Injector.CreateInstance<IRenovationRecommendationRepository>();
        }

        public List<AccommodationMonth> GetMonthsByAccommodationIdAndYear(int accommodationId, int year)
        {
            List<AccommodationMonth> accommodationMonths = new List<AccommodationMonth>();

            for (int monthIndex = 1; monthIndex <= 12; monthIndex++)
            {
                int reservations = GetReservationCountByMonthAndAccommodationId(monthIndex, year, accommodationId);
                int cancellations = GetCancellationCountByMonthAndAccommodationId(monthIndex, year, accommodationId);
                int reshedulings = GetReshedulingCountByMonthAndAccommodationId(monthIndex, year, accommodationId);
                int recommendations = GetRecommendationCountByMonthAndAccommodationId(monthIndex, year, accommodationId);
                string name = new System.Globalization.DateTimeFormatInfo().GetMonthName(monthIndex).ToString();

                AccommodationMonth accommodationMonth = new AccommodationMonth(monthIndex, name, reservations, cancellations, reshedulings, recommendations);
                accommodationMonths.Add(accommodationMonth);
            }
            return accommodationMonths;
        }

        public int GetReservationCountByMonthAndAccommodationId(int monthIndex, int year, int accommodationId)
        {
            return _reservationRepository.GetReservationCountByMonthAndAccommodationId(monthIndex, year, accommodationId);
        }

        public int GetCancellationCountByMonthAndAccommodationId(int monthIndex, int year, int accommodationId)
        {
            return _reservationRepository.GetCancellationCountByMonthAndAccommodationId(monthIndex, year, accommodationId);
        }

        public int GetReshedulingCountByMonthAndAccommodationId(int monthIndex, int year, int accommodationId)
        {
            return _requestRepository.GetReshedulingCountByMonthAndAccommodationId(monthIndex, year, accommodationId);
        }

        public int GetRecommendationCountByMonthAndAccommodationId(int monthIndex, int year, int accommodationId)
        {
            return _recommendationRepository.GetRecommendationCountByMonthAndAccommodationId(monthIndex, year, accommodationId);
        }

        public string FindBestMonthInYear(int year, int accommodationId)
        {
            int maxReservations = 0;
            int bestMonth = 1;

            for (int monthIndex = 1; monthIndex <= 12; monthIndex++)
            {
                if (GetReservationCountByMonthAndAccommodationId(monthIndex, year, accommodationId) > maxReservations)
                {
                    maxReservations = GetReservationCountByMonthAndAccommodationId(monthIndex, year, accommodationId);
                    bestMonth = monthIndex;
                }
            }

            string bestMonthName = new System.Globalization.DateTimeFormatInfo().GetMonthName(bestMonth).ToString();
            return bestMonthName;
        }

    }
}
