using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SIMS_HCI_Project.Domain.DTOs
{
    public class AccommodationMonth
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public int ReservationCount { get; set; }
        public int CancellationCount { get; set; }
        public int ReschedulingCount { get; set; }
        public int RecommendationCount { get; set; }

        public AccommodationMonth() { }
        public AccommodationMonth(int monthIndex, string name, int reservationCount, int cancellationCount, int reschedulingCount, int recommendationCount)
        {
            Index = monthIndex;
            Name = name;
            ReservationCount = reservationCount;
            CancellationCount = cancellationCount;
            ReschedulingCount = reschedulingCount;
            RecommendationCount = recommendationCount;
        }
    }
}
