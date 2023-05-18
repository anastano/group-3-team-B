using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Domain.DTOs
{
    public class AccommodationYear
    {
        public int Year { get; set; }
        public int ReservationCount { get; set; }
        public int CancellationCount { get; set; }
        public int ReschedulingCount { get; set; }
        public int RecommendationCount { get; set; }

        public AccommodationYear() { }
        public AccommodationYear(int year, int reservationCount, int cancellationCount, int reschedulingCount, int recommendationCount)
        {
            Year = year;
            ReservationCount = reservationCount;
            CancellationCount = cancellationCount;
            ReschedulingCount = reschedulingCount;
            RecommendationCount = recommendationCount;
        }
    }
}
