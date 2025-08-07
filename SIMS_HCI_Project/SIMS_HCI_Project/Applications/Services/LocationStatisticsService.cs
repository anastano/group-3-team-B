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
    public class LocationStatisticsService
    {
        private readonly IAccommodationReservationRepository _reservationRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IAccommodationRepository _accommodationRepository;

        public LocationStatisticsService()
        {
            _reservationRepository = Injector.Injector.CreateInstance<IAccommodationReservationRepository>();
            _locationRepository = Injector.Injector.CreateInstance<ILocationRepository>();
            _accommodationRepository = Injector.Injector.CreateInstance<IAccommodationRepository>();
        }

        public double FindOccupancyPercentageLastYear(int accommodationId)
        {
            int occupiedDaysNum = 0;
            DateRange yearDateRange = new DateRange(DateTime.Today.AddYears(-1), DateTime.Today);
            foreach (AccommodationReservation reservation in _reservationRepository.GetByAccommodationId(accommodationId))
            {
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
                    reservationCount++;
                }
            }

            return reservationCount;
        }

        public LocationInfo FindBestLocationLastYear(int ownerId)
        {
            LocationInfo bestLocation = new LocationInfo(_locationRepository.GetById(1), 0, 0);

            foreach (Location location in _locationRepository.GetAll())
            {
                int locationReservationCount;
                double locationOccupancyPercentage;

                CalculateLocationOccupancyStatistics(ownerId, location, out locationReservationCount, out locationOccupancyPercentage);

                if (locationReservationCount > bestLocation.ReservationCountInLastYear && locationOccupancyPercentage > bestLocation.OccupancyPercentageInLastYear)
                {
                    bestLocation = new LocationInfo(_locationRepository.GetById(location.Id), locationReservationCount, locationOccupancyPercentage);
                }
            }

            return bestLocation;
        }

        private void CalculateLocationOccupancyStatistics(int ownerId, Location location, out int locationReservationCount, out double locationOccupancyPercentage)
        {
            locationReservationCount = 0;
            locationOccupancyPercentage = 0;
            foreach (Accommodation accommodation in _accommodationRepository.GetByLocationIdAndOwnerId(location.Id, ownerId))
            {
                locationReservationCount += FindReservationCountLastYear(accommodation.Id);
                locationOccupancyPercentage += FindOccupancyPercentageLastYear(accommodation.Id);
            }
        }

        public LocationInfo FindWorstLocationLastYear(int ownerId)
        {
            LocationInfo worstLocation = new LocationInfo(_locationRepository.GetById(1), 100, 100);

            foreach (Location location in _locationRepository.GetAll())
            {
                int locationReservationCount;
                double locationOccupancyPercentage;

                CalculateLocationOccupancyStatistics(ownerId, location, out locationReservationCount, out locationOccupancyPercentage);

                if (locationReservationCount < worstLocation.ReservationCountInLastYear && locationOccupancyPercentage < worstLocation.OccupancyPercentageInLastYear)
                {
                    worstLocation = new LocationInfo(_locationRepository.GetById(location.Id), locationReservationCount, locationOccupancyPercentage);
                }
            }

            return worstLocation;
        }
    }
}
