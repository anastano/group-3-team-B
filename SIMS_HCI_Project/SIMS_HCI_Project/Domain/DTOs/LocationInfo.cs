using SIMS_HCI_Project.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Domain.DTOs
{
    public class LocationInfo
    {
        public int LocationId { get; set; }
        public Location Location { get; set; }
        public int ReservationCountInLastYear { get; set; }
        public double OccupancyPercentageInLastYear { get; set; }

        public LocationInfo() { }
        public LocationInfo(Location location, int reservationCount, double occupacyPercentage)
        {
            LocationId = location.Id;
            Location = location;
            ReservationCountInLastYear = reservationCount;
            OccupancyPercentageInLastYear = occupacyPercentage;
        }
    }
}
