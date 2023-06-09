using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Domain.Models
{
    public class ComplexTourRequest : TourRequest
    {
        public int Id { get; set; }
        public List<RegularTourRequest> TourRequests { get; set; }

        public ComplexTourRequest()
        {
            Status = TourRequestStatus.PENDING;
            TourRequests = new List<RegularTourRequest>();
        }
    }
}
