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

        public double FindOccupancyPercentageLastYear(int accommodationId)
        {
            int occupiedDaysNum = 0;
            DateRange yearDateRange = new DateRange(DateTime.Today.AddYears(-1), DateTime.Today);
            foreach (AccommodationReservation reservation in _reservationRepository.GetByAccommodationId(accommodationId)) {
                DateRange reservationDateRange = new DateRange(reservation.Start, reservation.End);
                if (reservationDateRange.IsInside(yearDateRange))
                {
                    occupiedDaysNum += (reservation.End - reservation.Start).Days;
                }
            }

            return (double)occupiedDaysNum * 100 / 365;
        }

        public int FindReservationCountLastYear(int accommodationId)
        {
            int reservationCount = 0;
            DateRange yearDateRange = new DateRange(DateTime.Today.AddYears(-1), DateTime.Today);
            foreach (AccommodationReservation reservation in _reservationRepository.GetByAccommodationId(accommodationId))
            {
                DateRange reservationDateRange = new DateRange(reservation.Start, reservation.End);
                if (reservationDateRange.IsInside(yearDateRange))
                {
                    reservationCount ++;
                }
            }

            return reservationCount;
        }

        public LocationInfo FindBestLocationInYear(int ownerId) 
        {
            int bestLocationId = 1;
            int maxlocationReservationCount = 0;
            double maxlocationOccupancyPercentage = 0;

            foreach (Location location in _locationRepository.GetAll())
            {
                int locationReservationCount = 0;
                double locationOccupancyPercentage = 0;

                foreach (Accommodation accommodation in _accommodationRepository.GetByLocationIdAndOwnerId(location.Id, ownerId)) 
                {
                    locationReservationCount += FindReservationCountLastYear(accommodation.Id);
                    locationOccupancyPercentage += FindOccupancyPercentageLastYear(accommodation.Id);
                }

                if (locationReservationCount > maxlocationReservationCount && locationOccupancyPercentage>maxlocationReservationCount)
                {
                    maxlocationReservationCount = locationReservationCount;
                    maxlocationOccupancyPercentage = locationOccupancyPercentage;
                    bestLocationId = location.Id;
                }
            }

            LocationInfo bestLocation = new LocationInfo(bestLocationId, maxlocationReservationCount, maxlocationOccupancyPercentage);
            bestLocation.Location = _locationRepository.GetById(bestLocationId);
            
            return bestLocation;
        }

        public Accommodation FindWorstAccommodationLastYear(int ownerId)
        {
            Accommodation worstAccommodation = _accommodationRepository.GetById(1);
            int minReservationCount = 100;
            double minOccupancyPercentage = 100;

            foreach(Accommodation accommodation in _accommodationRepository.GetByOwnerId(ownerId))
            {
                if (FindReservationCountLastYear(accommodation.Id) < minReservationCount && FindOccupancyPercentageLastYear(accommodation.Id) < minOccupancyPercentage)
                {
                    worstAccommodation = accommodation;
                    minReservationCount = FindReservationCountLastYear(accommodation.Id);
                    minOccupancyPercentage = FindOccupancyPercentageLastYear(accommodation.Id);
                }
            }
            return worstAccommodation;
        }
    }
}
