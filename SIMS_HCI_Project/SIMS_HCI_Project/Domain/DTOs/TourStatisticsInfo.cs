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
        public int HadVoucherPercentage { get; set; }

        public TourStatisticsInfo(Dictionary<AgeGroup, int> guestNumbersByAge, int hadVoucherPercentage)
        {
            GuestNumbersByAge = guestNumbersByAge;
            HadVoucherPercentage = hadVoucherPercentage;
        }
    }
}
