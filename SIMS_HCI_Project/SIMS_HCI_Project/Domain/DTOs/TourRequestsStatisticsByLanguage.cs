using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Domain.DTOs
{
    public class TourRequestsStatisticsByLanguage
    {
        public string Language { get; set; }
        public int NumberOfRequests { get; set; }

        public TourRequestsStatisticsByLanguage()
        {

        }

        public TourRequestsStatisticsByLanguage(string language, int numberOfGuests)
        {
            Language = language;
            NumberOfRequests = numberOfGuests;
        }
    }
}
