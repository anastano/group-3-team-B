using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Domain.DTOs
{
    public class TourStatisticsInfo
    {
        public Dictionary<AgeGroup, int> GuestNumbersByAge { get; set; }
        public double WithVoucherPercentage { get; set; }
        public double WithoutVoucherPercentage { get; set; }

        public TourStatisticsInfo(Dictionary<AgeGroup, int> guestNumbersByAge, double withVoucherPercentage)
        {
            GuestNumbersByAge = guestNumbersByAge;
            WithVoucherPercentage = withVoucherPercentage;
            WithoutVoucherPercentage = 100 - WithVoucherPercentage;
        }
    }
}
