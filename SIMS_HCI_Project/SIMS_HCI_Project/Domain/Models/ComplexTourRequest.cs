using SIMS_HCI_Project.Domain.DTOs;
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

        public bool HasAcceptedPart(int guideId)
        {
            return this.TourRequests.Any(r => r.TourId != -1 && r.Tour.GuideId == guideId);
        }

        public bool IsTimeSlotScheduled(DateRange dateRange)
        {
            return TourRequests.Any(r => r.TourId != -1 && dateRange.DoesOverlap(new DateRange(r.Tour.DepartureTimes[0].DepartureTime, r.Tour.Duration)));
        }

        public bool AllPartsAccepted()
        {
            return !TourRequests.Any(r => r.TourId == -1);
        }
    }
}
