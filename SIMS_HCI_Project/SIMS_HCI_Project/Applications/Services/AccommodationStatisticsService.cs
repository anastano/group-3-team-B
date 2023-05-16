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

        public AccommodationStatisticsService()
        {
            _reservationRepository = Injector.Injector.CreateInstance<IAccommodationReservationRepository>();
            _requestRepository = Injector.Injector.CreateInstance<IRescheduleRequestRepository>();
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

                AccommodationYear accommodationYear = new AccommodationYear(year, reservations, cancellations, reshedulings);
                accommodationYears.Add(accommodationYear);
            }
            return accommodationYears;
        }

        public int GetReservationCountByYearAndAccommodationId(int year, int accommodationId)
        {
            return _reservationRepository.GetByAccommodationId(accommodationId).FindAll(r => r.Start.Year == year).Count();
        }

        public int GetCancellationCountByYearAndAccommodationId(int year, int accommodationId)
        {
            return _reservationRepository.GetByAccommodationId(accommodationId).FindAll(r => r.Start.Year == year
                                          && r.Status == Domain.Models.AccommodationReservationStatus.CANCELLED).Count();
        }

        public int GetReshedulingCountByYearAndAccommodationId(int year, int accommodationId)
        {
            return _requestRepository.GetByAccommodationId(accommodationId).FindAll(r => r.AccommodationReservation.Start.Year == year
                                           && r.Status == Domain.Models.RescheduleRequestStatus.ACCEPTED).Count();
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
                string name = new System.Globalization.DateTimeFormatInfo().GetMonthName(monthIndex).ToString();

                AccommodationMonth accommodationMonth = new AccommodationMonth(monthIndex, name, reservations, cancellations, reshedulings);
                accommodationMonths.Add(accommodationMonth);
            }
            return accommodationMonths;
        }

        public int GetReservationCountByMonthAndAccommodationId(int monthIndex, int year, int accommodationId)
        {
            return _reservationRepository.GetByAccommodationId(accommodationId).FindAll(r => r.Start.Year == year
                                    && r.Start.Month == monthIndex).Count();
        }

        public int GetCancellationCountByMonthAndAccommodationId(int monthIndex, int year, int accommodationId)
        {
            return _reservationRepository.GetByAccommodationId(accommodationId).FindAll(r => r.Start.Year == year
                        && r.Start.Month == monthIndex && r.Status == Domain.Models.AccommodationReservationStatus.CANCELLED).Count(); ;
        }

        public int GetReshedulingCountByMonthAndAccommodationId(int monthIndex, int year, int accommodationId)
        {
            return _requestRepository.GetByAccommodationId(accommodationId).FindAll(r => r.AccommodationReservation.Start.Year == year
                         && r.AccommodationReservation.Start.Month == monthIndex && r.Status == Domain.Models.RescheduleRequestStatus.ACCEPTED).Count();
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
