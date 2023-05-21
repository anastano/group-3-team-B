using SIMS_HCI_Project.Domain.DTOs;
using SIMS_HCI_Project.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Applications.Services
{
    public class AccommodationStatisticsService
    {
        private readonly IAccommodationReservationRepository _reservationRepository;
        private readonly IRescheduleRequestRepository _requestRepository;
        private readonly IRenovationRecommendationRepository _recommendationRepository;

        public AccommodationStatisticsService()
        {
            _reservationRepository = Injector.Injector.CreateInstance<IAccommodationReservationRepository>();
            _requestRepository = Injector.Injector.CreateInstance<IRescheduleRequestRepository>();
            _recommendationRepository = Injector.Injector.CreateInstance<IRenovationRecommendationRepository>();
        }

        public List<AccommodationYear> GetYearsByAccommodationId(int accommodationId)
        {
            List<int> years = _reservationRepository.GetByAccommodationId(accommodationId).Select(r => r.Start.Year).Distinct().ToList();
            List<AccommodationYear> accommodationYears= new List<AccommodationYear>();

            foreach (int year in years)
            {
                int reservations = GetReservationCountByYearAndAccommodationId(year, accommodationId);
                int cancellations = GetCancellationCountByYearAndAccommodationId(year, accommodationId);
                int reshedulings = GetReshedulingCountByYearAndAccommodationId(year, accommodationId);
                int recommendations = GetRecommendationCountByYearAndAccommodationId(year, accommodationId);

                AccommodationYear accommodationYear = new AccommodationYear(year, reservations, cancellations, reshedulings, recommendations);
                accommodationYears.Add(accommodationYear);
            }
            return accommodationYears;
        }

        public int GetReservationCountByYearAndAccommodationId(int year, int accommodationId)
        {
            return _reservationRepository.GetReservationCountByYearAndAccommodationId(year, accommodationId);
        }

        public int GetCancellationCountByYearAndAccommodationId(int year, int accommodationId)
        {
            return _reservationRepository.GetCancellationCountByYearAndAccommodationId(year, accommodationId);
        }

        public int GetReshedulingCountByYearAndAccommodationId(int year, int accommodationId)
        {
            return _requestRepository.GetReshedulingCountByYearAndAccommodationId(year, accommodationId);
        }

        public int GetRecommendationCountByYearAndAccommodationId(int year, int accommodationId)
        {
            return _recommendationRepository.GetRecommendationCountByYearAndAccommodationId(year, accommodationId);
        }

        public int FindBestYear(int accommodationId)
        {
            List<int> years = _reservationRepository.GetByAccommodationId(accommodationId).Select(r => r.Start.Year).Distinct().ToList();
            int maxReservations = 0;
            int bestYear = years.FirstOrDefault();

            foreach (int year in years)
            { 
                if(GetReservationCountByYearAndAccommodationId(year, accommodationId) > maxReservations)
                {
                    maxReservations = GetReservationCountByYearAndAccommodationId(year, accommodationId);
                    bestYear = year;
                }
            }
            return bestYear;
        }

        public List<AccommodationMonth> GetMonthsByAccommodationIdAndYear(int accommodationId, int year)
        { 
            List<AccommodationMonth> accommodationMonths = new List<AccommodationMonth>();

            for (int monthIndex=1; monthIndex<=12; monthIndex++)
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

            for(int monthIndex=1; monthIndex<=12; monthIndex++)
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
