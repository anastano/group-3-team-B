using Org.BouncyCastle.Cms;
using SIMS_HCI_Project.Domain.DTOs;
using SIMS_HCI_Project.Domain.Models;
using SIMS_HCI_Project.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Applications.Services
{
    public class AccommodationYearStatisticsService
    {
        private readonly IAccommodationReservationRepository _reservationRepository;
        private readonly IRescheduleRequestRepository _requestRepository;
        private readonly IRenovationRecommendationRepository _recommendationRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IAccommodationRepository _accommodationRepository;

        public AccommodationYearStatisticsService()
        {
            _reservationRepository = Injector.Injector.CreateInstance<IAccommodationReservationRepository>();
            _requestRepository = Injector.Injector.CreateInstance<IRescheduleRequestRepository>();
            _recommendationRepository = Injector.Injector.CreateInstance<IRenovationRecommendationRepository>();
            _locationRepository = Injector.Injector.CreateInstance<ILocationRepository>();
            _accommodationRepository = Injector.Injector.CreateInstance<IAccommodationRepository>();
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

    }
}
