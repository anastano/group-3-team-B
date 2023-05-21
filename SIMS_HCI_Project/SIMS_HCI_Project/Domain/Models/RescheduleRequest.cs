using System;
using System.Collections.Generic;
using System.Linq;
using SIMS_HCI_Project.Serializer;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_HCI_Project.Domain.Models
{
    public enum RescheduleRequestStatus { ACCEPTED, DENIED, PENDING };

    public class RescheduleRequest
    {
        public int Id { get; set; }
        public int AccommodationReservationId { get; set; }
        public AccommodationReservation AccommodationReservation { get; set; }
        public DateTime WantedStart { get; set; }
        public DateTime WantedEnd { get; set; }
        public RescheduleRequestStatus Status { get; set; }
        public String OwnerComment { get; set; }

        public RescheduleRequest() { }

        public RescheduleRequest(AccommodationReservation reservation, DateTime wantedStart, DateTime wantedEnd)
        {
            AccommodationReservationId = reservation.Id;
            AccommodationReservation = reservation;
            WantedStart = wantedStart;
            WantedEnd = wantedEnd;
            Status = RescheduleRequestStatus.PENDING;
            OwnerComment = "";
        }

    }
}
